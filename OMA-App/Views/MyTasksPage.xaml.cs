using CommunityToolkit.Maui.Views;
using OMA_App.Modals;
using OMA_App.Models;
using OMA_App.ViewModels;
using System.Diagnostics;

namespace OMA_App.Views;

public partial class MyTasksPage : ContentPage
{


    public MyTasksPage(MyTasksViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }

    
}