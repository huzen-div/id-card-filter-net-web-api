using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BroadcastAPI.Helper
{
    public class CustomException
    {
        public class TokenExpireExceptionEx : ArgumentException
        {
            public int ErrorCode { get; }
            public TokenExpireExceptionEx(string paramName, int errorCode)
                : base(paramName)
            {
                ErrorCode = errorCode;
            }
        }
    }
}
