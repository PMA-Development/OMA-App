using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OMA_App.Modals;
using OMA_App.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public partial class MyTasksViewModel : BaseViewModel
    {
        public ObservableCollection<TaskObj> Tasks { get; set; }



        public MyTasksViewModel()
        {
            Tasks = new ObservableCollection<TaskObj>();
            LoadTasks();
        }

        private void LoadTasks()
        {
            for (int i = 0; i < 10; i++)
            {
                TaskObj task = new TaskObj
                {
                    TaskID = i,
                    Title = "Replacement sensor",
                    Type = "Type: Vedligeholdelse",
                    Description = "Description: bla bla bla\nNordsø 1- A1",
                    Level = 1,
                    TurbineID = i
                };
                Tasks.Add(task);
            }
        }

        [RelayCommand]
        private async Task Item(TaskObj task)
        {
            await Application.Current.MainPage.ShowPopupAsync(new MyTasksModal(task));
        }
    }
}
