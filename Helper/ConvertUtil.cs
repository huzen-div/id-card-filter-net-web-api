using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BroadcastAPI.Helper
{
    public class ConvertUtil
    {
        public static string obj2string(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
        }
    }
}
