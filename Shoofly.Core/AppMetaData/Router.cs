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
            public const string SignIn = Prefix + "/SignIn";
            public const string Logout = Prefix + "/Logout";
            public const string ResendCode = Prefix + "/ResendCode";
            public const string RefreshToken = Prefix + "/RefreshToken";
            public const string RevokeToken = Prefix + "/RevokeToken";
            public const string RevokeAllSessions = Prefix + "/RevokeAllSessions";
            // Final URL: api/v1/Auth/VerifyCode
        }
    }
}
