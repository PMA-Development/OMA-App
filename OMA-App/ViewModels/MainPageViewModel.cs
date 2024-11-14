using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using OMA_App.API;
using OMA_App.Authentication;
using OMA_App.ErrorServices;
using OMA_App.Models;
using OMA_App.Storage;
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

        public MainPageViewModel(OMAClient client)
        {
            _client = client;

        }


        public async Task LoadIslandsWithTasks()
        {

            IslandsWithTasks.Clear();

            var islands = await _client.GetIslandsAsync();
            var turbines = await _client.GetTurbinesAsync(); 
            var allTasks = await _client.GetUncompletedTasksAsync();


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




        //public async Task LoadIslands()
        //{
        //    var tempList = await _client.GetIslandsAsync();
        //    Islands.Clear();
        //    foreach (var item in tempList)
        //    {
        //        Islands.Add(item);
        //    }

        //}

        //public async Task LoadTasksPerIsland(int id)
        //{
        //    TaskDTOs.Clear();
        //    var tasks = await _client.GetTasksFromIslandAsync(id);
        //    foreach (var task in tasks)
        //    {
        //        TaskDTOs.Add(task);
        //    }
        //}


        [RelayCommand]
        private async Task OpenIsland(IslandDTO island)
        {

            var route = $"{nameof(IslandPage)}?IslandId={island.IslandID.ToString()}";
            await Shell.Current.GoToAsync(route);
        }


    }
}



