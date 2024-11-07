using IdentityModel.OidcClient;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using OMA_App.Authentication;
using OMA_App.Pages;
using OMA_App.Storage;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OMA_App
{
    public partial class MainPage : ContentPage
    {
        private readonly OidcClient _client = default!;
        private string? _currentAccessToken;
        public IEnumerable<Island> Islands { get; set; } = [];
        private readonly AuthenticationService _authService;
        public MainPage(OidcClient client, AuthenticationService authService)
        {
            InitializeComponent();
            _client = client;
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
            _authService = authService;
        }

        private async void OpenIsland(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new IslandPage());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            //editor.Text = "Login Clicked";

            var result = await _client.LoginAsync();

            if (result.IsError)
            {
                string test = result.Error;
                return;
            }

            await _authService.SignInAsync(result.AccessToken);


            

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
