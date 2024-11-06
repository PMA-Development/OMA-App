using IdentityModel.OidcClient.Browser;
using Microsoft.Maui.Authentication;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace OMA_App.Authentication
{
    public class WebAuthenticatorBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken)
        {
            try
            {
                var authResult = await WebAuthenticator.AuthenticateAsync(
                    new Uri(options.StartUrl),
                    new Uri(options.EndUrl)
                );

                return new BrowserResult
                {
                    Response = authResult?.ToString() ?? string.Empty,
                    ResultType = BrowserResultType.Success
                };
            }
            catch (TaskCanceledException)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UserCancel
                };
            }
            catch (Exception ex)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.Message
                };
            }
        }
    }
}
