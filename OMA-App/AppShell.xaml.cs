using OMA_App.Views;

namespace OMA_App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(IslandPage), typeof(IslandPage));
        }
    }
}
