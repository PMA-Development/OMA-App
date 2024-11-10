using Microsoft.Maui.Controls;
using OMA_App.Models;
using OMA_App.ViewModels;
using System.Threading.Tasks;

namespace OMA_App.Views;

public partial class TasksPage : ContentPage
{
    
    public TasksPage(TasksPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        
    }


}