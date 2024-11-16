using CommunityToolkit.Mvvm.Messaging;
using OMA_App.MessageClass;
using OMA_App.ViewModels;
using OMA_App.Views;

namespace OMA_App
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("mainpage_route", typeof(MainPage));
            Routing.RegisterRoute(nameof(IslandPage), typeof(IslandPage));
            Routing.RegisterRoute(nameof(MyTasksPage), typeof(MyTasksPage));
            Routing.RegisterRoute(nameof(TasksPage),typeof(TasksPage));
            WeakReferenceMessenger.Default.Register<LoginStateChangedMessage>(this, (recipient, message) =>
            {
                UpdateLoginTabTitle(message.Value);
            });
        }

        private void UpdateLoginTabTitle(bool isLoggedIn)
        {
            var loginTab = this.FindByName<Tab>("LoginTab");
            if (loginTab != null)
            {
                loginTab.Title = isLoggedIn ? "Logout" : "Login";
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
            WeakReferenceMessenger.Default.Unregister<LoginStateChangedMessage>(this);
        }
    }
}
