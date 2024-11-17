using IdentityModel.OidcClient;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Storage
{
    public static class TokenService
    {
        private const string AccessTokenKey = "access_token";
        private const string UserIdKey = "UserId";
        private const string IdentityTokenKey = "IdentityToken";

        public static async Task SaveTokensAsync(LoginResult token)
        {
            var handler = new JwtSecurityTokenHandler();

            var JwtToken = handler.ReadJwtToken(token.AccessToken);
            var _ = JwtToken.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            await SecureStorage.SetAsync(AccessTokenKey, token.AccessToken);
            await SecureStorage.SetAsync(UserIdKey, _);
            await SecureStorage.SetAsync(IdentityTokenKey, token.IdentityToken);
        }

        public static async Task<string> GetIdentityTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(IdentityTokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving identity token: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetAccessTokenAsync()
        {
            try
            {
                return await SecureStorage.GetAsync(AccessTokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving access token: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> GetUserIdAsync()
        {
            return await SecureStorage.GetAsync(UserIdKey);
        }


        public static async Task<bool> IsAccessTokenExpiredAsync()
        {
            try
            {
                var accessToken = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(accessToken))
                    return true;

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);

                // Checks if token has expired
                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking token expiration: {ex.Message}");
                return true; // we assume expired if there's an error
            }
        }


        public static async Task ClearTokensAsync()
        {
            try
            {
                SecureStorage.Remove(AccessTokenKey);
                SecureStorage.Remove(UserIdKey);
                SecureStorage.Remove(IdentityTokenKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing tokens: {ex.Message}");
            }
        }
    }
}
