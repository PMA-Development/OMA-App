using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.ErrorServices;
using System;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MyTasksModalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TaskDTO taskObj;

        private readonly OMAClient _client;
        private readonly Action _closePopupAction;

        public MyTasksModalViewModel(TaskDTO task, Action closePopupAction, OMAClient client, ErrorService errorService)
            : base(errorService)
        {
            TaskObj = task;
            _closePopupAction = closePopupAction;
            _client = client;
        }

        [RelayCommand]
        private async Task Complete()
        {
            try
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Write your finished Description", "");

                if (string.IsNullOrWhiteSpace(result))
                {
                    await _errorService.DisplayAlertAsync("Error", "Description can't be empty");
                    return;
                }

                TaskObj.FinishDescription = result;
                TaskObj.IsCompleted = true;
                await _client.UpdateTaskAsync(TaskObj);
                _closePopupAction?.Invoke();
            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to complete the task: {e.Message}");
            }
        }

        //TODO: NOT YET IMPLEMENTED
        [RelayCommand]
        private async Task SendDrone()
        {
            try
            {
                bool result = await Application.Current.MainPage.DisplayAlert("Send drone?", "Do you wanna reserve a drone for this task?", "Yes", "No");

                if (result)
                {
                    await Application.Current.MainPage.DisplayAlert("NOT YET IMPLEMENTED", "Do you wanna reserve a drone for this task?", "no", "yes");
                    //await _client.AssignTaskToFirstAvailableDroneAsync(taskObj.TaskID);
                }

            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to save changes: {e.Message}");
            }
        }

        [RelayCommand]
        private async Task Save()
        {
            try
            {
                bool result = await Application.Current.MainPage.DisplayAlert("Save", "Do you want to save changes?", "Yes", "No");

                if (result)
                {
                    await _client.UpdateTaskAsync(TaskObj);
                }

                _closePopupAction?.Invoke();
            }
            catch (ApiException apiEx)
            {
                await _errorService.ShowErrorAlert(apiEx);
            }
            catch (Exception e)
            {
                await _errorService.DisplayAlertAsync("Error", $"Failed to save changes: {e.Message}");
            }
        }

        [RelayCommand]
        private Task Close()
        {
            _closePopupAction?.Invoke();
            return Task.CompletedTask;
        }
    }
}
