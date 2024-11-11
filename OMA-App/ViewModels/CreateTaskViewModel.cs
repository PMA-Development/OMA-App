using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class CreateTaskViewModel : BaseViewModel
    {
        OMAClient _client;

        public TaskDTO Task { get; set; } = new();

        public ObservableCollection<Turbine> Turbines { get; set; } = new();
        public ObservableCollection<User> Users { get; set; } = new();


        public CreateTaskViewModel(OMAClient client)
        {
            _client = client;
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

        [RelayCommand]
        private async Task CreateTask()
        {
            await _client.AddTaskAsync(Task);
        }
    }
}
