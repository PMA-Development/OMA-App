using Microsoft.Maui.Controls;
using OMA_App.Models;
using OMA_App.ViewModels;
using System.Threading.Tasks;

namespace OMA_App.Views;

public partial class TasksPage : ContentPage
{
    
    private readonly TasksPageViewModel _vm;
    public TasksPage(TasksPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _vm = vm;
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadTasks();
    }

}