using OMA_App.Storage;
using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class CreateTaskPage : ContentPage
{

	public CreateTaskPage(CreateTaskViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

        Task.Run(async () =>
        {
            await Task.Delay(100); // Small delay to ensure the ViewModel completes loading
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BindingContext = vm;
            });
        });
    }

}