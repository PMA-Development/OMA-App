using CommunityToolkit.Maui.Views;
using OMA_App.Modals;
using System.Diagnostics;

namespace OMA_App.Pages;

public partial class MyTasksPage : ContentPage
{
    public IEnumerable<Tasks> TaskList { get; set; } = [];

    public MyTasksPage()
	{
		InitializeComponent();
        List<Tasks> tasks = [];
        for (int i = 0; i < 10; i++)
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

    private void OnItemClick(object sender, EventArgs e)
    {
        this.ShowPopup(new MyTasksModal());
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