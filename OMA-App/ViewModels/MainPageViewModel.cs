using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using OMA_App.API;
using OMA_App.Authentication;
using OMA_App.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly OidcClient _client;
        private readonly AuthenticationService _authService;


        public ObservableCollection<Island> Islands { get; set; }

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
            await Shell.Current.GoToAsync(nameof(IslandPage));
        }
    }

    
}
