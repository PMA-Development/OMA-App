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

        public static async Task SaveAccessTokenAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var test = handler.ReadJwtToken(token);
            var s = test.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            await SecureStorage.SetAsync(AccessTokenKey, token);
            await SecureStorage.SetAsync(UserIdKey,  s);
        }

        public static async Task<string> GetAccessTokenAsync()
        {
            return await SecureStorage.GetAsync(AccessTokenKey);
        }

        public static async Task<string> GetUserIdAsync()
        {
            return await SecureStorage.GetAsync(UserIdKey);
        }

        public static void ClearAccessToken()
        {
            SecureStorage.Remove(AccessTokenKey);
        }
    }
}
