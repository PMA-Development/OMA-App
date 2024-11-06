using CommunityToolkit.Maui.Views;
using OMA_App.Modals;

namespace OMA_App.Pages;

public partial class IslandPage : ContentPage
{
    public IEnumerable<Turbine> Turbines = [];
	public IslandPage()
	{
		InitializeComponent();
        List<Turbine> turbines = [];
        for (int i = 1; i < 11; i++)
        {
            Turbine turbine = new()
            {
                TurbineID = i,
                Title = "A" + i,
                TelemetryID = i
            };
            turbines.Add(turbine);
        }

        Turbines = turbines;
        _collectionView.ItemsSource = Turbines;
	}

    private async void ReturnPage(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void OpenTurbine(object sender, EventArgs e)
    {
        this.ShowPopup(new IslandModal());
    }

    public class Turbine
    {
        public int TurbineID { get; set; }
        public string Title { get; set; } = string.Empty;
        public int TelemetryID { get; set; }
    }
}