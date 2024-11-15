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
            try
            {
                var tempList = await _client.GetUsersAsync();
                Users.Clear();
                foreach (var user in tempList)
                {
                    Users.Add(user);
                }
            }
            catch (ApiException apiEx)
            {
                await ShowErrorAlert(apiEx);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"An unexpected error occurred: {ex.Message}");
            }
        }

        private async Task GetIslands()
        {
            try
            {
                var tempList = await _client.GetIslandsAsync();
                Islands.Clear();
                foreach (var island in tempList)
                {
                    Islands.Add(island);
                }
            }
            catch (ApiException apiEx)
            {
                await ShowErrorAlert(apiEx);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"An unexpected error occurred: {ex.Message}");
            }
        }

        private async void LoadTurbinesForIsland()
        {
            if (SelectedIsland == null) return;

            try
            {
                var turbinesForIsland = await _client.GetTurbinesIslandAsync(SelectedIsland.IslandID);
                FilteredTurbines.Clear();
                foreach (var turbine in turbinesForIsland)
                {
                    FilteredTurbines.Add(turbine);
                }
            }
            catch (ApiException apiEx)
            {
                await ShowErrorAlert(apiEx);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"An unexpected error occurred: {ex.Message}");
            }
        }

        //TODO: fix so it does not complain about not finding turbines on finished task creation
        partial void OnSelectedIslandChanged(IslandDTO value)
        {
            
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
            try
            {
                Task.Level = LevelEnum._1;
                Task.FinishDescription = "";
                Task.OwnerID = Guid.Parse(await TokenService.GetUserIdAsync());

                int result = await _client.AddTaskAsync(Task);

       
                await DisplayAlertAsync("Success", "Task has been created successfully");


                ResetFields();
            }
            catch (ApiException apiEx)
            {

                await ShowErrorAlert(apiEx);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
            {

                await DisplayAlertAsync("Error", "Invalid input or data missing. Please review your information.");
            }
            catch (Exception ex)
            {

                await DisplayAlertAsync("Error", $"An unexpected error occurred: {ex.Message}");
            }
        }


        private async Task ShowErrorAlert(ApiException apiEx)
        {
            string message = apiEx.StatusCode switch
            {
                400 => "Invalid input. Please check your data.",
                401 => "You are not authorized to perform this action.",
                500 => "An error occurred on the server. Please try again later.",
                _ => $"Unexpected error: {apiEx.Message}",
            };
            await DisplayAlertAsync("Error", message);
        }


        private async Task DisplayAlertAsync(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

    }
}
