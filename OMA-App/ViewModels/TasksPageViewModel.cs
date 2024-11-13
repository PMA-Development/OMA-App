using CommunityToolkit.Mvvm.Input;
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

        private readonly OMAClient _client;

        public ObservableCollection<TaskDTO> Tasks { get; set; } = new();

        public TasksPageViewModel(OMAClient client)
        {
            _client = client;
        }

        public async Task LoadTasks()
        {
            var templist = await _client.GetTasksAsync();
            Tasks.Clear();
            foreach (var task in templist)
            {
                Tasks.Add(task);
            }
        }


    }
}
