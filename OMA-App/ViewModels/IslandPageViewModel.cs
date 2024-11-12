// File: ViewModels/IslandPageViewModel.cs
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.Models;

namespace OMA_App.ViewModels
{
    public partial class IslandPageViewModel : BaseViewModel
    {
        public ObservableCollection<TurbineDTO> Turbines { get; set; }

        private OMAClient _client;

        [ObservableProperty]
        private string searchText;

        public IslandPageViewModel(OMAClient client)
        {
            _client = client;
            GetTurbines();
        }

        private async void GetTurbines()
        {
           var tempList =  await _client.GetTurbinesAsync();
            Turbines = tempList.ToObservableCollection();
        }


        [RelayCommand]
        private async System.Threading.Tasks.Task Return()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void OpenTurbine()
        {
            // Handle turbine modal opening logic
        }

        // Method triggered whenever SearchText changes
        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            //if (string.IsNullOrWhiteSpace(SearchText))
            //{
            //    Turbines = new ObservableCollection<Turbine>(GenerateTurbines());
            //}
            //else
            //{
            //    var filteredTurbines = GenerateTurbines()
            //        .Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
            //        .ToList();

            //    Turbines = new ObservableCollection<Turbine>(filteredTurbines);
            //}
        }

       
    }
}
