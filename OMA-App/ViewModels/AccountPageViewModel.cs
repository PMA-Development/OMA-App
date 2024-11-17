using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.OidcClient;
using OMA_App.Authentication;
using OMA_App.MessageClass;
using OMA_App.Storage;
using OMA_App.ErrorServices;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class AccountPageViewModel : BaseViewModel
    {
        private readonly OidcClient _client;
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private bool isLoggedIn; // Binds to toggle Login and Logout button visibility

        public AccountPageViewModel(OidcClient client, AuthenticationService authenticationService, ErrorService errorService)
            : base(errorService)
        {
            _authService = authenticationService;
            _client = client;

            // Check initial login state
            Task.Run(CheckLoginAsync);
        }

        private async Task CheckLoginAsync()
        {
            var accessToken = await TokenService.GetAccessTokenAsync();
            IsLoggedIn = !string.IsNullOrEmpty(accessToken);
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            var result = await _client.LoginAsync();

            if (result.IsError)
            {
                await _errorService.DisplayAlertAsync("Login Error", $"An error occurred during login: {result.Error}");
                return;
            }

            await _authService.SignInAsync(result);
            IsLoggedIn = true;
            NotifyLoginStateChanged(true);
        }

        [RelayCommand]
        public async Task LogoutAsync()
        {
            var idToken = await TokenService.GetIdentityTokenAsync();
            var logoutRequest = new LogoutRequest { IdTokenHint = idToken };

            var result = await _client.LogoutAsync(logoutRequest);

            if (result.IsError)
            {
                await _errorService.DisplayAlertAsync("Logout Error", $"An error occurred during logout: {result.Error}");
                return;
            }

            await _authService.SignOutAsync();
            IsLoggedIn = false;
            NotifyLoginStateChanged(false);
        }

        private void NotifyLoginStateChanged(bool isLoggedIn)
        {
            WeakReferenceMessenger.Default.Send(new LoginStateChangedMessage(isLoggedIn));
        }
    }
}
