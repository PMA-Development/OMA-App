using CommunityToolkit.Maui.Views;
using OMA_App.API;
using OMA_App.ViewModels;

namespace OMA_App.Modals;

public partial class IslandModal : Popup
{
	private readonly OMAClient _client;
	public IslandModal(int turbineId, OMAClient client)
	{
		InitializeComponent();
        _client = client;
        BindingContext = new IslandModalViewModel(turbineId, () => Close(),_client);

    }
}