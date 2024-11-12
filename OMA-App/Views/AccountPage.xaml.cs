using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage(AccountPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}