﻿using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
using OMA_App.Models;
using OMA_App.Storage;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class TasksPageViewModel : BaseViewModel
    {
        private readonly OMAClient _client;

        public ObservableCollection<TaskDTO> Tasks { get; set; } = new();

        public TasksPageViewModel(OMAClient client, ErrorService errorService, IConnectivity connectivity)
            : base(errorService, connectivity)
        {
            _client = client;
        }

        public async Task LoadTasks()
        {
            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }
                var templist = await _client.GetUncompletedTasksAsync();

                if (templist.Count != null)
                {
                    Tasks.Clear();
                    foreach (var task in templist)
                    {
                        Tasks.Add(task);
                    }
                }
               
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to load tasks: {e.Message}");
            }
        }

        [RelayCommand]
        private async Task Refresh()
        {
            IsRefreshing = true;
            await LoadTasks();
            IsRefreshing = false;
        }


        [RelayCommand]
        private async Task Accept(TaskDTO task)
        {

            bool result = await Application.Current.MainPage.DisplayAlert("Accept Task", "Do you want to accept this task?", "Yes", "No");

            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("No connectivity!",
                    $"Please check internet and try again.", "OK");
                return;
            }

            if (result)
            {
                task.UserID = Guid.Parse(await TokenService.GetUserIdAsync());
                try
                {
                    await _client.UpdateTaskAsync(task);
                }
                catch (Exception e)
                {
                    await _errorService.DisplayAlertAsync("Error", $"Something went wrong when accepting the task: {e.Message}");
                }
            }
        }

        [RelayCommand]
        private async Task EscalateTask(TaskDTO task)
        {
            try
            {
                string action = await Application.Current.MainPage.DisplayActionSheet("Escalate to?", "Cancel", null, "Level 1", "Level 2", "Level 3");
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }

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
                await LoadTasks(); // we reload the entire list to match server values and fix a problem with the LevelEnumToStringConverter not working as intended
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to escalate the task: {e.Message}");
            }
        }
    }
}
