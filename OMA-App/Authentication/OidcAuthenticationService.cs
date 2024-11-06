using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Authentication
{
    public class OidcAuthenticationService
    {
        private readonly OidcClient _oidcClient;

        public OidcAuthenticationService()
        {
            var options = new OidcClientOptions
            {
                Authority = "https://192.168.1.100:5000", // Your Duende IdentityServer URL
                ClientId = "OMA-Maui", // Your client ID from IdentityServer
                RedirectUri = "myapp://auth", // Custom redirect URI for your MAUI app
                Scope = "openid profile email role", // Scopes you need
                Browser = new WebAuthenticatorBrowser() // Use the WebAuthenticator API for browser interaction
            };

            _oidcClient = new OidcClient(options);
        }

        public async Task<LoginResult> LoginAsync()
        {
            var result = await _oidcClient.LoginAsync(new LoginRequest());

            if (result.IsError)
            {
                Console.WriteLine($"Login error: {result.Error}");
                return null;
            }

            Console.WriteLine($"Access Token: {result.AccessToken}");
            return result;
        }

        public async Task LogoutAsync()
        {
            var result = await _oidcClient.LogoutAsync();

            if (result.IsError)
            {
                Console.WriteLine($"Logout error: {result.Error}");
            }
            else
            {
                Console.WriteLine("User logged out successfully.");
            }
        }
    }

}