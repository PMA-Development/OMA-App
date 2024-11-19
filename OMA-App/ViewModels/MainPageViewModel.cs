using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
using OMA_App.Models;
using OMA_App.Views;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly OMAClient _client;



        public ObservableCollection<IslandTask> IslandsWithTasks { get; set; } = new();
        public ObservableCollection<IslandDTO> Islands { get; set; } = new();
        public ObservableCollection<TaskDTO> TaskDTOs { get; set; } = new();

        public MainPageViewModel(OMAClient client, ErrorService errorService)
            : base(errorService)
        {
            _client = client;
        }

        public async Task LoadIslandsWithTasks()
        {
            try
            {
                var islands = await _client.GetIslandsAsync();
                var turbines = await _client.GetTurbinesAsync();
                var allTasks = await _client.GetUncompletedTasksAsync();

                if (islands != null && turbines != null && allTasks != null)
                {
                    IslandsWithTasks.Clear();

                    var tasksByTurbine = allTasks
                        .GroupBy(task => task.TurbineID)
                        .ToDictionary(group => group.Key, group => group.ToList());

                    foreach (var island in islands)
                    {
                        var islandTurbines = turbines.Where(turbine => turbine.IslandID == island.IslandID).ToList();

                        var islandTasks = new List<TaskDTO>();
                        foreach (var turbine in islandTurbines)
                        {
                            if (tasksByTurbine.ContainsKey(turbine.TurbineID))
                            {
                                islandTasks.AddRange(tasksByTurbine[turbine.TurbineID]);
                            }
                        }

                        var islandWithTasks = new IslandTask
                        {
                            IslandDTO = island,
                            TaskDTOs = new ObservableCollection<TaskDTO>(islandTasks.Take(4))
                        };

                        IslandsWithTasks.Add(islandWithTasks);
                    }
                }
               

               
            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"An error occurred while loading islands and tasks: {e.Message}");
            }
        }

        [RelayCommand]
        private async Task OpenIsland(IslandDTO island)
        {
            var route = $"{nameof(IslandPage)}?IslandId={island.IslandID.ToString()}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
