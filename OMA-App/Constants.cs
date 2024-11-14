using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App
{
    public static class Constants
    {
        // OIDC Configuration
        public static readonly string Authority = "https://web2.pcsyd.dk";
        public static readonly string APIURI = "https://1kkdhxfq-6001.euw.devtunnels.ms";
        public static readonly string ClientId = "OMA-Maui";
        public static readonly string Scope = "openid profile role";
        public static readonly string PostLogoutRedirectUri = "myapp://auth";
        public static readonly string RedirectUri = "myapp://auth";
    }
}
