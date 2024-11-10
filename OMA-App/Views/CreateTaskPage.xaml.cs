using OMA_App.Storage;
using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class CreateTaskPage : ContentPage
{
	CreateTaskViewModel vm;

	public CreateTaskPage(CreateTaskViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

}