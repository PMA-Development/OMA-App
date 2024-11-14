using Android.Renderscripts;
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

        public MyTasksModalViewModel(TaskDTO task, Action closePopupAction, OMAClient client)
        {
            TaskObj = task;
            _closePopupAction = closePopupAction;
            _client = client;
        }


        [RelayCommand]
        private async Task Complete()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Write your finished Description", "");

            if (String.IsNullOrWhiteSpace(result))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Description can't be empty", "Ok");
                return;
            }
            TaskObj.FinishDescription = result;
            TaskObj.IsCompleted = true;
            await _client.UpdateTaskAsync(TaskObj);
            _closePopupAction?.Invoke();
        }

        [RelayCommand]
        private async Task Close()
        {
            _closePopupAction?.Invoke();
        }
    }
}
