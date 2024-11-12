using IdentityModel.Client;
using IdentityModel.OidcClient.Browser;
using Microsoft.Maui.Authentication;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace OMA_App.Authentication
{
    public class WebAuthenticatorBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri(options.StartUrl),
                    new Uri(options.EndUrl));

                var url = new RequestUrl("myapp://auth")
                    .Create(new Parameters(result.Properties));

                return new BrowserResult
                {
                    Response = url,
                    ResultType = BrowserResultType.Success,
                };
            }
            catch (TaskCanceledException)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };
            }
        }
    }
}
