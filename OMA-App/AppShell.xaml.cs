using CommunityToolkit.Mvvm.Messaging;
using IdentityModel.OidcClient;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using OMA_App.Authentication;
using OMA_App.MessageClass;
using OMA_App.Views;

namespace OMA_App
{
    public partial class AppShell : Shell
    {
        private const string IsLoggedInKey = "IsLoggedIn";

        private readonly OidcClient _oidcClient;
        private readonly AuthenticationService _authenticationService;

        public AppShell(OidcClient oidcClient, AuthenticationService authenticationService)
        {
            InitializeComponent();
            _oidcClient = oidcClient;
            _authenticationService = authenticationService;

            Routing.RegisterRoute("mainpage_route", typeof(MainPage));
            Routing.RegisterRoute(nameof(IslandPage), typeof(IslandPage));
            Routing.RegisterRoute(nameof(MyTasksPage), typeof(MyTasksPage));
            Routing.RegisterRoute(nameof(TasksPage), typeof(TasksPage));

 
            InitializeLoginState();

        }

        private async void InitializeLoginState()
        {

            bool isLoggedIn = Preferences.Get(IsLoggedInKey, false);

            if (!isLoggedIn)
            {
               var result = await _oidcClient.LoginAsync();

                if (result.IsError)
                {
                    await Application.Current.MainPage.DisplayAlert("Logout Error", $"An error occurred during Login: {result.Error}","ok");
                    return;
                }

                Preferences.Get(IsLoggedInKey, true);
                await _authenticationService.SignInAsync(result);
      
            }
        }

      
    }
}
