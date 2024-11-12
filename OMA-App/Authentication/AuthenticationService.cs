using IdentityModel.OidcClient;
using OMA_App.Storage;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Authentication
{
    public class AuthenticationService
    {
        private List<string> _roles = new();
        public IReadOnlyList<string> Roles => _roles.AsReadOnly();

        public bool IsAdmin => _roles.Contains("admin");

        public async Task InitializeAsync()
        {
            var token = await TokenService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                LoadRolesFromToken(token);
            }
        }

        private void LoadRolesFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Extract roles from token claims
            _roles = jwtToken.Claims
                .Where(c => c.Type == "role")
                .Select(c => c.Value)
                .ToList();
        }

        public async Task SignInAsync(LoginResult token)
        {
            await TokenService.SaveTokensAsync(token);
            LoadRolesFromToken(token.AccessToken);
        }

        public void SignOut()
        {
            _roles.Clear();
            TokenService.ClearTokens();
        }
    }
}
