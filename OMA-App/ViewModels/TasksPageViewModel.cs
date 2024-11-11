using OMA_App.API;
using OMA_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.ViewModels
{
    public class TasksPageViewModel
    {
        public ObservableCollection<TaskObj> Tasks { get; set; }

        public TasksPageViewModel()
        {
            Tasks = new ObservableCollection<TaskObj>();
            LoadTasks();
        }

        private void LoadTasks()
        {
            for (int i = 0; i < 20; i++)
            {
                TaskObj task = new TaskObj
                {
                    TaskID = i,
                    Title = "Replacement sensor",
                    Type = "Type: Vedligeholdelse",
                    Description = "Description: bla bla bla\nNordsø 1- A1",
                };
                Tasks.Add(task);
            }
        }
    }
}
