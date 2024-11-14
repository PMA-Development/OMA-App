using OMA_App.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMA_App.Models
{
    public class IslandTask
    {
        public IslandDTO IslandDTO { get; set; }

        public ObservableCollection<TaskDTO> TaskDTOs { get; set; } = new();
    }
}
