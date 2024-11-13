using OMA_App.Storage;
using OMA_App.ViewModels;
using System.Runtime.Serialization;

namespace OMA_App.Views;

public partial class CreateTaskPage : ContentPage
{
    CreateTaskViewModel _vm;
    public CreateTaskPage(CreateTaskViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _vm = vm;

    }

	protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.GetProperties();
    }

}