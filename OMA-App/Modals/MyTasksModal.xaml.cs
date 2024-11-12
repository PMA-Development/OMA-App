using CommunityToolkit.Maui.Views;
using OMA_App.API;
using OMA_App.Models;

namespace OMA_App.Modals;

public partial class MyTasksModal : Popup
{
    public TaskDTO Task { get; private set; }
    public MyTasksModal(TaskDTO task)
	{
		InitializeComponent();
        Task = task;

        BindingContext = Task;
	}

    private void Close_Button(object sender, EventArgs e)
    {
		Close();
    }
}