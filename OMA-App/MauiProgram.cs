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

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true // Bypass SSL certificate errors for development
            };

            var httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(2) // Increase timeout to 2 minutes or adjust as needed
            };

            builder.Services.AddSingleton(new OidcClient(new()
            {
                Authority = "https://53c7-85-203-210-151.ngrok-free.app", // Your IdentityServer URL
                ClientId = "OMA-Maui", // Your Client ID
                Scope = "openid profile email api role", // Scopes needed
                RedirectUri = "myapp://", // Custom URI scheme for your MAUI app
                PostLogoutRedirectUri = "myapp://", // Custom URI for logout

                // Use the WebAuthenticatorBrowser for authentication
                Browser = new WebAuthenticatorBrowser(),

                // Backchannel handler for HTTPS requests
                BackchannelHandler = handler
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
