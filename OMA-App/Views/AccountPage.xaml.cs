using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class AccountPage : ContentPage
{

    private readonly AccountPageViewModel _vm;
    public AccountPage(AccountPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = _vm = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.CheckLoginAsync();

    }
}