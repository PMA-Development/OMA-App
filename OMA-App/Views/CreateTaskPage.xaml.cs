using OMA_App.Storage;
using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class CreateTaskPage : ContentPage
{

	public CreateTaskPage(CreateTaskViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }

}