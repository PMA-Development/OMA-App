using CommunityToolkit.Maui.Views;
using OMA_App.API;
using OMA_App.ErrorServices;
using OMA_App.Models;
using OMA_App.ViewModels;
using System.Runtime.CompilerServices;

namespace OMA_App.Modals;

public partial class MyTasksModal : Popup
{
    private readonly OMAClient _client;
    private readonly ErrorService _errorService;

    public MyTasksModal(TaskDTO task, OMAClient client, ErrorService errorService)
	{
		InitializeComponent();
        _client = client;
        _errorService = errorService;
        BindingContext = new MyTasksModalViewModel(task, () => Close(), _client, _errorService);

    }

}