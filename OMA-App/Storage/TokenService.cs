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
        private const string IdentityToken = "IdentityToken";

        public static async Task SaveTokensAsync(LoginResult token)
        {
            var handler = new JwtSecurityTokenHandler();

            var JwtToken = handler.ReadJwtToken(token.AccessToken);
            var _ = JwtToken.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            await SecureStorage.SetAsync(AccessTokenKey, token.AccessToken);
            await SecureStorage.SetAsync(UserIdKey, _);
            await SecureStorage.SetAsync(IdentityToken, token.IdentityToken);
        }

        public static async Task<string> GetIdentityTokenAsync()
        {
            return await SecureStorage.GetAsync(IdentityToken);
        }
        public static async Task<string> GetAccessTokenAsync()
        {
            return await SecureStorage.GetAsync(AccessTokenKey);
        }

        public static async Task<string> GetUserIdAsync()
        {
            return await SecureStorage.GetAsync(UserIdKey);
        }



        public static void ClearTokens()
        {
            SecureStorage.Remove(AccessTokenKey);
            SecureStorage.Remove(UserIdKey);
            SecureStorage.Remove(IdentityToken);
        }
    }
}
