using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core.Extensions;
using OMA_App.API;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OMA_App.Storage;
using System.Linq;

namespace OMA_App.ViewModels
{
    public partial class CreateTaskViewModel : BaseViewModel
    {
        private readonly OMAClient _client;

        [ObservableProperty]
        private TaskDTO task = new();

        [ObservableProperty]
        private ObservableCollection<IslandDTO> islands = new();

        [ObservableProperty]
        private ObservableCollection<UserDTO> users = new();
        [ObservableProperty]
        private ObservableCollection<TurbineDTO> turbines = new();

        [ObservableProperty]
        private ObservableCollection<TurbineDTO> filteredTurbines = new();

        [ObservableProperty]
        private UserDTO selectedUser;

        [ObservableProperty]
        private IslandDTO selectedIsland;

        [ObservableProperty]
        private TurbineDTO selectedTurbine;

        public CreateTaskViewModel(OMAClient client)
        {
            _client = client;

        }

        public async Task GetProperties()
        {
            await GetUsers();
            await GetIslands();
        }

        private async Task GetUsers()
        {
            var tempList = await _client.GetUsersAsync();
            foreach (var user in tempList)
            {
                Users.Add(user);
            }
        }

        private async Task GetIslands()
        {
            var tempList = await _client.GetIslandsAsync();
            foreach (var island in tempList)
            {
                Islands.Add(island);
            }

        }

        private async void LoadTurbinesForIsland()
        {
            if (SelectedIsland == null) return;

            var turbinesForIsland = await _client.GetTurbinesIslandAsync(SelectedIsland.IslandID);
            FilteredTurbines.Clear();
            foreach (var turbine in turbinesForIsland)
            {
                FilteredTurbines.Add(turbine);
            }
        }

        partial void OnSelectedIslandChanged(IslandDTO value)
        {
            // Load turbines for the selected island whenever the island changes
            LoadTurbinesForIsland();
        }

        partial void OnSelectedUserChanged(UserDTO value)
        {
            if (value != null)
            {
                Task.UserID = value.UserID;
            }
        }

        partial void OnSelectedTurbineChanged(TurbineDTO value)
        {
            if (value != null)
            {
                Task.TurbineID = value.TurbineID;
            }
        }

        private void ResetFields()
        {
            Task = new TaskDTO();
            SelectedIsland = new();
            SelectedTurbine = new();
            SelectedUser = new();
        }

        [RelayCommand]
        private async Task CreateTask()
        {
            Task.Level = LevelEnum._1;
            Task.FinishDescription = "";
            Task.OwnerID = Guid.Parse(await TokenService.GetUserIdAsync());
            await _client.AddTaskAsync(Task);
            await Application.Current.MainPage.DisplayAlert("Confirm", "Task has been created", "ok");
            ResetFields();


        }
    }
}
