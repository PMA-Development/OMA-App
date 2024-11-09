using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using OMA_App.Authentication;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly OidcClient _client;
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private ObservableCollection<Island> _islands;

        public MainPageViewModel(OidcClient client, AuthenticationService authService)
        {
            _client = client;
            _authService = authService;

            Islands = new ObservableCollection<Island>();
            for (int i = 1; i < 9; i++)
            {
                Islands.Add(new Island
                {
                    IslandID = i,
                    Title = "Nordsø " + i,
                    Abbreviation = "NS" + i,
                    TurbineID = i,
                });
            }
        }

        public MainPageViewModel()
        {
            
        }

        [RelayCommand]
        private async Task Login()
        {
            var result = await _client.LoginAsync();

            if (result.IsError)
            {
                // Handle error
                return;
            }

            await _authService.SignInAsync(result.AccessToken);
        }

        [RelayCommand]
        private async Task OpenIsland(Island island)
        {
            // Navigate to IslandPage
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
