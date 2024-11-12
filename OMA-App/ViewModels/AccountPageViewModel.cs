using IdentityModel.OidcClient;
using OMA_App.Authentication;
using OMA_App.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            CheckLogin();

        }
        public AccountPageViewModel()
        {

        }

        public async Task CheckLogin()
        {
            if (TokenService.GetAccessTokenAsync() == null)
            {
                await Login();
            }
            else
            {
                await Logout();
            }

            
        }

        private async Task Login()
        {
            var result = await _client.LoginAsync();
            
            if (result.IsError)
            {
                // Handle error
                return;
            }

            await _authService.SignInAsync(result);
        }

        private async Task Logout()
        {
            var result = await _client.LogoutAsync(new LogoutRequest { IdTokenHint = await TokenService.GetIdentityTokenAsync()});

            if (result.IsError)
            {
                // Handle error
                return;
            }

            _authService.SignOut();

        }


    }
}
