using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OMA_App.API;
using OMA_App.Models;
using OMA_App.ErrorServices;
using CommunityToolkit.Maui.Views;
using OMA_App.Views;
using OMA_App.Modals;

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
        private ObservableCollection<TurbineTask> turbinesTasks = new();

        private ICollection<TurbineTask> _allTurbinesTasks;

        public IslandPageViewModel(OMAClient client)
        {
            _client = client;
            _allTurbinesTasks = new ObservableCollection<TurbineTask>();
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
                    GetTurbinesWithTasks();
                }
            }
        }

        private async void GetTurbinesWithTasks()
        {

            _allTurbinesTasks.Clear();

            var turbines = await _client.GetTurbinesIslandAsync(islandId); 
            var allUncompletedTasks = await _client.GetUncompletedTasksAsync(); 


            var turbineIdsInIsland = turbines.Select(t => t.TurbineID).ToHashSet(); 
            var tasksByTurbine = allUncompletedTasks
                .Where(task => turbineIdsInIsland.Contains(task.TurbineID)) 
                .GroupBy(task => task.TurbineID) 
                .ToDictionary(group => group.Key, group => group.ToList());

      
            foreach (var turbine in turbines)
            {
                
                var turbineTasks = tasksByTurbine.ContainsKey(turbine.TurbineID)
                    ? tasksByTurbine[turbine.TurbineID]
                    : new List<TaskDTO>();

                
                var turbineTask = new TurbineTask
                {
                    Title = turbine.Title,
                    TurbineId = turbine.TurbineID,
                    TaskDTOs = new ObservableCollection<TaskDTO>(turbineTasks)
                };

                _allTurbinesTasks.Add(turbineTask);
            }

            PerformSearch();
        }



        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                TurbinesTasks = new ObservableCollection<TurbineTask>(_allTurbinesTasks);
            }
            else
            {
                var filteredTurbines = _allTurbinesTasks
                    .Where(t => t.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                TurbinesTasks = new ObservableCollection<TurbineTask>(filteredTurbines);
            }
        }

        [RelayCommand]
        private async Task Return()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async void OpenTurbine(int turbineID)
        {
           await Application.Current.MainPage.ShowPopupAsync(new IslandModal(turbineID, _client));
        }
    }
}
