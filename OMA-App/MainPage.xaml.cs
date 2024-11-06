using OMA_App.Pages;
using System.Diagnostics;

namespace OMA_App
{
    public partial class MainPage : ContentPage
    {
        public IEnumerable<Island> Islands { get; set; } = [];

        public MainPage()
        {
            InitializeComponent();
            List<Island> list = new List<Island>();
            for (int i = 1; i < 9; i++)
            {
                Island island = new()
                {
                    IslandID = i,
                    Title = "Nordsø " + i,
                    Abbreviation = "NS" + i,
                    TurbineID = i,
                };
                list.Add(island);
            }
            Islands = list;
            _collectionView.ItemsSource = Islands;
        }

        private async void OpenIsland(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new IslandPage());
        }
    }

    public class Island
    {
        public int IslandID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public int TurbineID { get; set; }
    }

}
