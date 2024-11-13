using IdentityModel.OidcClient;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using OMA_App.Authentication;
using OMA_App.Storage;
using OMA_App.ViewModels;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace OMA_App.Views
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel _vm;
        public MainPage(MainPageViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
            _vm = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
           await _vm.LoadIslands();
        }

    }


}
