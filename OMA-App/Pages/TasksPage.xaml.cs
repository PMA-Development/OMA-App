using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace OMA_App.Pages;

public partial class TasksPage : ContentPage
{
    public IEnumerable<Tasks> TaskList { get; set; } = [];

    public TasksPage()
	{
		InitializeComponent();
        List<Tasks> tasks = [];
        for (int i = 0; i < 20; i++)
		{
            Tasks task = new() 
            { 
                TaskID = i, 
                Title = "Replacement sensor",
                Type = "Type: Vedligeholdelse", 
                Description = "Description: bla bla bla\nNordsø 1- A1", 
                Level = 1, 
                TurbineID = i 
            };
            tasks.Add(task);
        }
        TaskList = tasks;

        _collectionView.ItemsSource = TaskList;
    }

	public class Tasks
	{
        public int TaskID { get; set; }
        public string Type { get; set; } = string.Empty;		
        public string Title { get; set; } = string.Empty;		
        public int Level { get; set; }
        public string Description { get; set; } = string.Empty;
        public int TurbineID { get; set; }			
    }
}