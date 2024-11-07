using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using IdentityModel.OidcClient;
using OMA_App.Authentication;
namespace OMA_App
{
    public static class MauiProgram
    {


        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                // Initialize the .NET MAUI Community Toolkit by adding the below line of code
                .UseMauiCommunityToolkit()
                // After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            builder.Services.AddSingleton(new OidcClient(new()
            {
                Authority = "https://v8c0dbnw-5000.euw.devtunnels.ms",

                ClientId = "OMA-Maui",
                Scope = "openid profile role",
                PostLogoutRedirectUri = "myapp://auth",
                RedirectUri = "myapp://auth",

                // Use the WebAuthenticatorBrowser for authentication
                Browser = new WebAuthenticatorBrowser(),

            }));

            // Continue initializing your .NET MAUI App here
            builder.Services.AddSingleton<MainPage>();
#if DEBUG
            builder.Logging.AddDebug();

            #endif
            return builder.Build();
        }
    }
}
