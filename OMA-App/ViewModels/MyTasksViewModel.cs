using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.API;
using OMA_App.Modals;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MyTasksViewModel : BaseViewModel
    {
        public ObservableCollection<TaskDTO> Tasks { get; set; }



        public MyTasksViewModel()
        {
            Tasks = new ObservableCollection<TaskDTO>();
            LoadTasks();
        }

        private void LoadTasks()
        {
            for (int i = 0; i < 10; i++)
            {
                TaskDTO task = new TaskDTO
                {
                    TaskID = i,
                    Title = "Replacement sensor",
                    Type = "Type: Vedligeholdelse",
                    Description = "Description: bla bla bla\nNordsø 1- A1",
                };
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
