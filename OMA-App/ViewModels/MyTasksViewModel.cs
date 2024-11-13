using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.Modals;
using OMA_App.Storage;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MyTasksViewModel : BaseViewModel
    {
        //TODO: Still need some work like messenges and error handling and Messenges for when getting data from the server
        private readonly OMAClient _client;

        public ObservableCollection<TaskDTO> Tasks { get; set; } = new();

        public MyTasksViewModel(OMAClient client)
        {
       ;    _client = client;
            LoadTasks();
        }

        public async Task LoadTasks()
        {
            var test = Guid.Parse(await TokenService.GetUserIdAsync());
            var templist = await _client.GetUserTasksAsync(test);
            Tasks.Clear();
            foreach (var task in templist)
            {
                Tasks.Add(task);
            }
        }

        [RelayCommand]
        private async Task Item(TaskDTO task)
        {
            await Application.Current.MainPage.ShowPopupAsync(new MyTasksModal(task));
        }
    }
}
