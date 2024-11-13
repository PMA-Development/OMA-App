using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OMA_App.API;
using OMA_App.Authentication;
using OMA_App.Storage;
using OMA_App.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly OMAClient _client;


        public ObservableCollection<IslandDTO> Islands { get; set; } = new();

        public MainPageViewModel(OMAClient client)
        {
            _client = client;

        }

        public async Task LoadIslands()
        {
            var tempList = await _client.GetIslandsAsync();
            Islands.Clear();
            foreach (var item in tempList)
            {
                Islands.Add(item);
            }
        }


        [RelayCommand]
        private async Task OpenIsland(IslandDTO island)
        {
            // Convert IslandId to string for query parameter
            var route = $"{nameof(IslandPage)}?IslandId={island.IslandID.ToString()}";
            await Shell.Current.GoToAsync(route);
        }


    }


}
