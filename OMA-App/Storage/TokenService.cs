using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Storage
{
    public static class TokenService
    {
        private const string AccessTokenKey = "access_token";

        public static async Task SaveAccessTokenAsync(string token)
        {
            await SecureStorage.SetAsync(AccessTokenKey, token);
        }

        public static async Task<string> GetAccessTokenAsync()
        {
            return await SecureStorage.GetAsync(AccessTokenKey);
        }

        public static void ClearAccessToken()
        {
            SecureStorage.Remove(AccessTokenKey);
        }
    }
}
