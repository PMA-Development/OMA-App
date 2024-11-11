using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using IdentityModel.OidcClient;
using OMA_App.Authentication;
using Microsoft.Extensions.Configuration;
using OMA_App.Views;
using OMA_App.ViewModels;
using OMA_App.Modals;
using CommunityToolkit.Maui.Core;
using OMA_App.API;

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
                Authority = Constants.Authority,
                ClientId = Constants.ClientId,
                Scope = Constants.Scope,
                PostLogoutRedirectUri = Constants.PostLogoutRedirectUri,
                RedirectUri = Constants.RedirectUri,
                Browser = new WebAuthenticatorBrowser()
            }));


            builder.Services.AddScoped(sp => new OMAClient(Constants.APIURI, new HttpClient()));
            builder.Services.AddTransient<MyTasksModal>();
            
            // Continue initializing your .NET MAUI App here

            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<MainPage>();

            builder.Services.AddSingleton<CreateTaskViewModel>();
            builder.Services.AddSingleton<CreateTaskPage>();

            builder.Services.AddSingleton<IslandPageViewModel>();
            builder.Services.AddSingleton<IslandPage>();

            builder.Services.AddSingleton<MyTasksViewModel>();
            builder.Services.AddSingleton<MyTasksPage>();

            builder.Services.AddSingleton<TasksPageViewModel>();
            builder.Services.AddSingleton<TasksPage>();


            builder.Services.AddSingleton<AuthenticationService>();
#if DEBUG
            builder.Logging.AddDebug();

#endif
            return builder.Build();
        }
    }
}
