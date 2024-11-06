using IdentityModel.OidcClient;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using OMA_App.Authentication;
using OMA_App.Pages;
using System.Diagnostics;
using System.Text;

namespace OMA_App
{
    public partial class MainPage : ContentPage
    {

        private readonly OidcClient _client = default!;
        private string? _currentAccessToken;
        public IEnumerable<Island> Islands { get; set; } = [];

        public MainPage(OidcClient client)
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
                //editor.Text = result.Error;
                return;
            }

            _currentAccessToken = result.AccessToken;

            var sb = new StringBuilder(128);

            sb.AppendLine("claims:");
            foreach (var claim in result.User.Claims)
            {
                sb.AppendLine($"{claim.Type}: {claim.Value}");
            }

            sb.AppendLine();
            sb.AppendLine("access token:");
            sb.AppendLine(result.AccessToken);

            if (!string.IsNullOrWhiteSpace(result.RefreshToken))
            {
                sb.AppendLine();
                sb.AppendLine("refresh token:");
                sb.AppendLine(result.RefreshToken);
            }

            //editor.Text = sb.ToString();
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
