using CommunityToolkit.Maui.Views;
using OMA_App.API;
using OMA_App.ViewModels;
using OMA_App.ErrorServices;

namespace OMA_App.Modals;

public partial class IslandModal : Popup
{

	private readonly OMAClient _client;
    private readonly ErrorService _errorServices;

    public IslandModal(int turbineId, OMAClient client, ErrorService errorServices)
	{
		InitializeComponent();
        _client = client;
       _errorServices = errorServices;
        BindingContext = new IslandModalViewModel(turbineId, () => Close(),_client, _errorServices);

    }
}