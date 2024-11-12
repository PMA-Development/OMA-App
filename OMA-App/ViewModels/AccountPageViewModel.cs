using IdentityModel.OidcClient;
using OMA_App.Authentication;
using OMA_App.Storage;
using System.Threading.Tasks;
using IdentityModel.Client;
using CommunityToolkit.Mvvm.Messaging;
using OMA_App.MessageClass;
using OMA_App.Views;

namespace OMA_App.ViewModels
{
    public partial class AccountPageViewModel : BaseViewModel
    {
        private readonly OidcClient _client;
        private readonly AuthenticationService _authService;

        public AccountPageViewModel(OidcClient client, AuthenticationService authenticationService)
        {
            _authService = authenticationService;
            _client = client;
            _ = CheckLoginAsync();
        }



        public async Task CheckLoginAsync()
        {
            var accessToken = await TokenService.GetAccessTokenAsync();
            if (string.IsNullOrEmpty(accessToken))
            {
                await LoginAsync();
            }
            else
            {
                await LogoutAsync();
            }

            
        }

      
        private async Task LoginAsync()
        {
            var result = await _client.LoginAsync();

            if (result.IsError)
            {
                Console.WriteLine($"Login error: {result.Error}");
                return;
            }

            
            await _authService.SignInAsync(result);
            NotifyLoginStateChanged(true);

        }


        private async Task LogoutAsync()
        {
            var idToken = await TokenService.GetIdentityTokenAsync();
            var logoutRequest = new LogoutRequest { IdTokenHint = idToken };

            var result = await _client.LogoutAsync(logoutRequest);

            if (result.IsError)
            {
                Console.WriteLine($"Logout error: {result.Error}");
                return;
            }

            _authService.SignOut();
            NotifyLoginStateChanged(false);

        }

        private void NotifyLoginStateChanged(bool isLoggedIn)
        {
            
            WeakReferenceMessenger.Default.Send(new LoginStateChangedMessage(isLoggedIn));
        }
    }
}
