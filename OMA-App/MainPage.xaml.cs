using OMA_App.Authentication;
using OMA_App.Pages;
using System.Diagnostics;

namespace OMA_App
{
    public partial class MainPage : ContentPage
    {
        private readonly OidcAuthenticationService _authService;
        public IEnumerable<Island> Islands { get; set; } = [];

        public MainPage()
        {
            InitializeComponent();
            List<Island> list = new List<Island>();
            for (int i = 1; i < 9; i++)
            {
                Island island = new()
                {
                    IslandID = i,
                    Title = "Nordsø " + i,
                    Abbreviation = "NS" + i,
                    TurbineID = i,
                };
                list.Add(island);
            }
            Islands = list;
            _collectionView.ItemsSource = Islands;
            _authService = new OidcAuthenticationService();
        }

        private async void OpenIsland(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new IslandPage());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var result = await _authService.LoginAsync();
            if (result != null)
            {
                Console.WriteLine("User logged in successfully.");
            }
        }


        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            await _authService.LogoutAsync();
        }
    }

   

    public class Island
    {
        public int IslandID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int TurbineID { get; set; }
    }

}
