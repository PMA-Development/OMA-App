// File: ViewModels/IslandPageViewModel.cs
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.Models;

namespace OMA_App.ViewModels
{
    public partial class IslandPageViewModel : BaseViewModel
    {
        public ObservableCollection<Turbine> Turbines { get; set; }

        [ObservableProperty]
        private string searchText;

        public IslandPageViewModel()
        {
            Turbines = new ObservableCollection<Turbine>(GenerateTurbines());
        }

        private IEnumerable<Turbine> GenerateTurbines()
        {
            return Enumerable.Range(1, 10).Select(i => new Turbine
            {
                TurbineID = i,
                Title = "A" + i,
                TelemetryID = i
            });
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

        // Method triggered whenever SearchText changes
        partial void OnSearchTextChanged(string value)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Turbines = new ObservableCollection<Turbine>(GenerateTurbines());
            }
            else
            {
                var filteredTurbines = GenerateTurbines()
                    .Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                Turbines = new ObservableCollection<Turbine>(filteredTurbines);
            }
        }

       
    }
}
