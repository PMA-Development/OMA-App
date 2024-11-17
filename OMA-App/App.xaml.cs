using IdentityModel.OidcClient;
using OMA_App.Authentication;
using OMA_App.ViewModels;
using OMA_App.Views;

namespace OMA_App
{
    public partial class App : Application
    {
        public App(OidcClient oidcClient,AuthenticationService authService)
        {
            InitializeComponent();
            authService.InitializeAsync();
            MainPage = new AppShell(oidcClient,authService);

        }
    }
}
