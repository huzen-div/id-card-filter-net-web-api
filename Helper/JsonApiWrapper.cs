using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BroadcastAPI.Helper
{
    public class JsonApiWrapper
    {
        private readonly RequestDelegate Next;

        
        public JsonApiWrapper(RequestDelegate requestDelegate)
        {
            this.Next = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {

            Stream responseBody = context.Response.Body;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                if (!context.Request.Path.Value.ToUpper().EndsWith("_DOWNLOAD"))
                {
                    context.Response.Body = memoryStream;
                }

                await this.Next(context);

                if (context.Response.ContentType == null || !context.Response.ContentType.Equals("application/octet-stream"))
                {
                    memoryStream.Position = 0;
                    string responseString = new StreamReader(memoryStream).ReadToEnd();
                    string wrappedResponse = this.Wrap(responseString, context);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(wrappedResponse);

                    context.Response.Headers["Content-type"] = "application/vnd.api+json";
                    context.Response.Body = responseBody;
                    await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                }


            }
        }

        private String Wrap(string originalBody, HttpContext context)
        {

            if (originalBody != null && originalBody.StartsWith("nowrap"))
            {
                return originalBody.Replace("nowrap", "");
            }

            dynamic response;
            if (this.IsJsonString<ExpandoObject>(originalBody))
            {
                response = JsonConvert.DeserializeObject<ExpandoObject>(originalBody);
            }
            else
            {
                try
                {
                    response = JsonConvert.DeserializeObject(originalBody);
                }
                catch (Exception ex)
                {
                    response = originalBody;
                }
            }

            object wrapper;
            if (this.IsSuccessResponse(context.Response.StatusCode))
                wrapper = this.DataWrap(response);
            else
                wrapper = this.ErrorWrap(response, context.Response.StatusCode);

            string newBody = JsonConvert.SerializeObject(wrapper, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                    {
                        ProcessDictionaryKeys = true
                    }
                }
            });

            return newBody;
        }

        private Object DataWrap(dynamic response)
        {
            return new
            {
                status = true,
                message = "SUCCESS",
                data = response,
                isExpireToken = false
            };
        }

        private Object ErrorWrap(dynamic response, int statusCode)
        {
            string reason = "";
            bool isExpire = false;
          
            if (response.GetType().Name == "String")
            {
                reason = String.IsNullOrWhiteSpace(response) ? this.StatusCodeMessage(statusCode) : response;
            } else
            {
                reason = this.StatusCodeMessage(statusCode);
                
                if (response != null)
                {
                    List<object> lstValue = ((IDictionary<string, object>)response).Values.ToList();
                    if (lstValue != null)
                    {
                        isExpire = lstValue[0].ToString().Equals("TokenExpired") ? true : false;
                    }
                }
            }

            return new
            {
                status = false,
                message = statusCode,
                data = reason, 
                isExpireToken = isExpire
            };
        }

        private Boolean IsJsonString<T>(string text)
        {
            text = text.Trim();
            if ((text.StartsWith("{") && text.EndsWith("}")) || (text.StartsWith("[") && text.EndsWith("]")))
            {
                try
                {
                    var obj = JsonConvert.DeserializeObject<T>(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private Boolean IsSuccessResponse(int statusCode)
        {
            return (statusCode >= 200 && statusCode < 299);
        }

        private String StatusCodeMessage(int statusCode)
        {
            switch (statusCode)
            {
                case 412:
                    return "Token expired.";
                case 400:
                    return "Bad request.";
                case 401:
                    return "Unauthorized access.";
                case 402:
                    return "Payment required.";
                case 403:
                    return "Forbidden access.";
                case 404:
                    return "Resource not found.";
                case 405:
                    return "Method not allowed.";
                case 406:
                    return "Not acceptable.";
                case 407:
                    return "Proxy authentication required.";
                case 408:
                    return "Request timeout.";
                case 409:
                    return "Conflict";
                case 410:
                    return "Resource is gone.";
                case 411:
                    return "Length is required.";
                case 500:
                    return "Internal server error.";
                case 501:
                    return "Not implemented.";
                case 502:
                    return "Bad gateway.";
                case 503:
                    return "Service unavailable.";
                case 504:
                    return "Gateway timeout.";
                case 505:
                    return "HTTP version not supported.";
            }
            return "";
        }

    }
}
