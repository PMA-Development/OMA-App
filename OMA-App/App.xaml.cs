using OMA_App.Authentication;

namespace OMA_App
{
    public partial class App : Application
    {
        public App(AuthenticationService authService)
        {
            InitializeComponent();
            authService.InitializeAsync();
            MainPage = new AppShell();
        }
    }
}
