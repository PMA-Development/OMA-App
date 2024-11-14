﻿using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
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
            ; _client = client;
        }

        public async Task LoadTasks()
        {
            Guid userId = Guid.Parse(await TokenService.GetUserIdAsync());
            ICollection<TaskDTO> templist;
            try
            {
                if (userId == Guid.Empty)
                    return;

                templist = await _client.GetUserTasksAsync(userId);

                Tasks.Clear();
                foreach (var task in templist)
                {
                    Tasks.Add(task);
                }

            }
            catch (Exception e)
            {

                await Application.Current.MainPage.DisplayAlert("", e.Message, "ok");
                return;
            }



        }

        [RelayCommand]
        private async Task ViewTask(TaskDTO task)
        {
            await Application.Current.MainPage.ShowPopupAsync(new MyTasksModal(task, _client));
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

            if (index >= 0)
                Tasks[index] = task;


        }
    }
}
