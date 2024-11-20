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
using OMA_App.ErrorServices;
using Polly.Caching;
using Polly.Registry;
using Polly;
using OMA_App.Policies;

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


            builder.Services.AddMemoryCache();
            builder.Services.AddSingleton<IAsyncCacheProvider, Polly.Caching.Memory.MemoryCacheProvider>();
            builder.Services.AddSingleton<IReadOnlyPolicyRegistry<string>, PolicyRegistry>((serviceProvider) =>
            {
                PolicyRegistry registry = new();
                registry.Add("myCachePolicy",
                    Polly.Policy.CacheAsync(serviceProvider.GetRequiredService<IAsyncCacheProvider>().AsyncFor<HttpResponseMessage>(),
                        TimeSpan.FromSeconds(30)));
                return registry;
            });


            builder.Services.AddHttpClient();
            builder.Services.AddSingleton<HttpClientPolicies>();

            builder.Services.AddScoped<OMAClient>(sp =>
            {
                var policyRegistry = sp.GetRequiredService<IReadOnlyPolicyRegistry<string>>();
                var httpClient = sp.GetRequiredService<HttpClient>();
                var httpClientPolicies = sp.GetRequiredService<HttpClientPolicies>();
                return new OMAClient(policyRegistry, Constants.APIURI, httpClient, httpClientPolicies);
            });


            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            builder.Services.AddTransient<MyTasksModal>();
            builder.Services.AddTransient<IslandModal>();

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

            builder.Services.AddTransient<AccountPageViewModel>();
            builder.Services.AddTransient<AccountPage>();

            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<ErrorService>();

          

            
#if DEBUG
            builder.Logging.AddDebug();

#endif
            return builder.Build();
        }
    }
}
