using Android.Renderscripts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using System;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MyTasksModalViewModel : BaseViewModel
    {
        [ObservableProperty]
        private TaskDTO taskObj;

        private readonly Action _closePopupAction;

        public MyTasksModalViewModel(TaskDTO task, Action closePopupAction)
        {
            TaskObj = task;
            _closePopupAction = closePopupAction;
        }

        [RelayCommand]
        private async Task Close()
        {
            _closePopupAction?.Invoke();
        }
    }
}
