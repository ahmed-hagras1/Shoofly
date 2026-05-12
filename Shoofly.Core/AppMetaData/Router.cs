using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoofly.Core.AppMetaData
{
    public static class Router
    {
        // Define root constants
        public const string SignleRoute = "/{id}";
        public const string root = "Api";
        public const string version = "V1";
        public const string Rule = root + "/" + version + "/";

        public static class ClientRouting
        {
            public const string Prefix = Rule + "Client";
            public const string Register = Prefix + "/Register";
            // Final URL: api/v1/Client/Register
        }
        public static class AuthRouting
        {
            public const string Prefix = Rule + "Auth";
            public const string VerifyCode = Prefix + "/VerifyCode";
            // Final URL: api/v1/Auth/VerifyCode
        }
    }
}
