using CommunityToolkit.Maui.Views;
using OMA_App.Modals;
using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class IslandPage : ContentPage
{

	public IslandPage(IslandPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
       
	}

  
}