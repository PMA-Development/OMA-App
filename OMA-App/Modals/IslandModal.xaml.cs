using CommunityToolkit.Maui.Views;

namespace OMA_App.Modals;

public partial class IslandModal : Popup
{
	public IslandModal()
	{
		InitializeComponent();
	}

    private void Close_Button(object sender, EventArgs e)
    {
        Close();
    }
}