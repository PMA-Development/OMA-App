using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
using OMA_App.Models;
using OMA_App.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class TasksPageViewModel : BaseViewModel
    {

        private readonly OMAClient _client;
        //TODO: remove accept button on tasks with users
        public ObservableCollection<TaskDTO> Tasks { get; set; } = new();

        public TasksPageViewModel(OMAClient client)
        {
            _client = client;
        }

        public async Task LoadTasks()
        {
            var templist = await _client.GetUncompletedTasksAsync();
            Tasks.Clear();
            foreach (var task in templist)
            {
                Tasks.Add(task);
            }
        }

        [RelayCommand]
        private async Task Accept(TaskDTO task)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("", "Do you want to accep this task?", "Yes", "No");

            if (result)
            {
                task.UserID = Guid.Parse(await TokenService.GetUserIdAsync());
                try
                {
                    await _client.UpdateTaskAsync(task);
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("", "Something went wrong when accepting task", "ok");
                }

            }

        }

        [RelayCommand]
        private async Task EscalateTask(TaskDTO task)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet("Escalate to?", "Cancel", null, "Level 1", "Level 2", "Level 3");
            int index = Tasks.IndexOf(task);
            switch (action)
            {
                case "Level 1":
                    task.Level = LevelEnum._1;
                    break;
                case "Level 2":
                    task.Level = LevelEnum._2;
                    break;
                case "Level 3":
                    task.Level = LevelEnum._3;
                    break;
                default:
                    return;
            }

            await _client.UpdateTaskAsync(task);

            await LoadTasks();  //we reload the entire list to match server values and fix a problem with the LevelEnumToStringConverter not working as intended


        }


    }
}
