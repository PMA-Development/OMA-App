using OMA_App.API;
using OMA_App.ErrorServices;
using OMA_App.ViewModels;

namespace OMA_App.Views;

public partial class TurbineGraphPage : ContentPage
{
	public TurbineGraphPage(ErrorService errorService, IConnectivity connectivity, OMAClient oMAClient, int Id)
	{
		InitializeComponent();
		BindingContext = new TurbineGraphViewModel(errorService,connectivity, oMAClient, Id);
    }
}