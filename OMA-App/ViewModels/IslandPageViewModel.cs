using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OMA_App.API;
using OMA_App.Models;
using OMA_App.ErrorServices;

namespace OMA_App.ViewModels
{
    [QueryProperty(nameof(IslandIdString), "IslandId")]  
    public partial class IslandPageViewModel : BaseViewModel
    {
        private readonly OMAClient _client;

        [ObservableProperty]
        private int islandId;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private ObservableCollection<TurbineDTO> turbines = new();

        private ICollection<TurbineDTO> _allTurbines;

        public IslandPageViewModel(OMAClient client)
        {
            _client = client;
            _allTurbines = new ObservableCollection<TurbineDTO>();
        }

        private string islandIdString;
        public string IslandIdString
        {
            get => islandIdString;
            set
            {
                islandIdString = value;
                if (int.TryParse(value, out var id))
                {
                    IslandId = id;
                    GetTurbines();
                }
            }
        }

        private async void GetTurbines()
        {
            _allTurbines = await _client.GetTurbinesIslandAsync(islandId);
            PerformSearch();
        }

        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Turbines = new ObservableCollection<TurbineDTO>(_allTurbines);
            }
            else
            {
                var filteredTurbines = _allTurbines
                    .Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Turbines = new ObservableCollection<TurbineDTO>(filteredTurbines);
            }
        }


        [RelayCommand]
        private async Task Return()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void OpenTurbine()
        {
            // Handle turbine modal opening logic
        }
    }
}
