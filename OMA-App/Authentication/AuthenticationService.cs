using IdentityModel.OidcClient;
using OMA_App.Storage;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace OMA_App.Authentication
{
    public class AuthenticationService
    {
        private List<string> _roles = new();
        public IReadOnlyList<string> Roles => _roles.AsReadOnly();

        // Property to check if the user has "admin" role
        public bool IsAdmin => HasRole("admin");

        // Initialize and load roles if the token is valid
        public async Task InitializeAsync()
        {
            try
            {
                var token = await TokenService.GetAccessTokenAsync();
                if (!string.IsNullOrEmpty(token) && !IsTokenExpired(token))
                {
                    LoadRolesFromToken(token);
                }
                else
                {
                    await SignOutAsync(); // Clear tokens if the token is invalid or expired
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during initialization: {ex.Message}");
            }
        }

        // Loads roles from the JWT access token
        private void LoadRolesFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Extract roles from token claims
                _roles = jwtToken.Claims
                    .Where(c => c.Type == "role")
                    .Select(c => c.Value)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading roles from token: {ex.Message}");
            }
        }

        // Sign in and load roles from the token
        public async Task SignInAsync(LoginResult token)
        {
            await TokenService.SaveTokensAsync(token);
            LoadRolesFromToken(token.AccessToken);
        }

        // Sign out, clear roles, and remove tokens
        public async Task SignOutAsync()
        {
            _roles.Clear();
            await TokenService.ClearTokensAsync();
        }

        // Checks if the token is expired
        private bool IsTokenExpired(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking token expiration: {ex.Message}");
                return true; // Assume expired if there is an error
            }
        }

        // Check if the user has a specific role
        public bool HasRole(string role)
        {
            return _roles.Contains(role);
        }
    }
}
