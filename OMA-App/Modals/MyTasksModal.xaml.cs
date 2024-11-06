using CommunityToolkit.Maui.Views;

namespace OMA_App.Modals;

public partial class MyTasksModal : Popup
{
	public MyTasksModal()
	{
		InitializeComponent();
	}

    private void Close_Button(object sender, EventArgs e)
    {
		Close();
    }
}