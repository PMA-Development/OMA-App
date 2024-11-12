using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Core.Extensions;
using OMA_App.API;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OMA_App.Storage;

namespace OMA_App.ViewModels
{
    public partial class CreateTaskViewModel : BaseViewModel
    {
        private readonly OMAClient _client;


        private TaskDTO task = new();


        private ObservableCollection<User> Users = new();

        private ObservableCollection<Turbine> Turbines = new();

        [ObservableProperty]
        private User selectedUser;

        public CreateTaskViewModel(OMAClient client)
        {
            _client = client;
            GetUsers();
            GetTurbines();
        }

        private async void GetUsers()
        {
            var tempList = await _client.GetUsersAsync();
            Users = tempList.ToObservableCollection();
        }

        private async void GetTurbines()
        {
            var tempList = await _client.GetTurbinesAsync();
            Turbines = tempList.ToObservableCollection();
        }

        partial void OnSelectedUserChanged(User value)
        {
            // Update Task.UserId whenever SelectedUser changes
            task.User.UserID = value.UserID;
        }

        [RelayCommand]
        private async Task CreateTask()
        {
            //TODO: Change Task Object
            //task.Owner.UserID = await TokenService.GetUserIdAsync();
            await _client.AddTaskAsync(task);
        }
    }
}
