using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BroadcastAPI.Helper
{
    public static class JsonApiWrapperExtension
    {
        public static IApplicationBuilder UseJsonApiWrapper(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JsonApiWrapper>();
        }
    }
}
