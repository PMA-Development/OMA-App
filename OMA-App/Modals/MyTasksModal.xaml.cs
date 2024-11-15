using CommunityToolkit.Maui.Views;
using OMA_App.API;
using OMA_App.Models;
using OMA_App.ViewModels;
using System.Runtime.CompilerServices;

namespace OMA_App.Modals;

public partial class MyTasksModal : Popup
{
    private readonly OMAClient _client;

    public MyTasksModal(TaskDTO task, OMAClient client)
	{
		InitializeComponent();
        _client = client;
        BindingContext = new MyTasksModalViewModel(task, () => Close(), _client);

    }

}