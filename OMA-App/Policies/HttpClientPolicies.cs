using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;
using System.Diagnostics;

namespace OMA_App.Policies
{
    public class HttpClientPolicies
    {
        public AsyncRetryPolicy<HttpResponseMessage> RetryPolicy { get; }
        public AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy { get; }
        public AsyncFallbackPolicy<HttpResponseMessage> FallbackPolicy { get; }

        public IAsyncPolicy<HttpResponseMessage> CombinedPolicy { get; }

        public HttpClientPolicies()
        {

            RetryPolicy = Policy
                .HandleResult<HttpResponseMessage>(res => !res.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (response, time) =>
                    {
                        Debug.WriteLine($"Retrying in {time.TotalSeconds} seconds due to: {response.Result?.StatusCode}");
                    }
                );

      
            TimeoutPolicy = Policy
                .TimeoutAsync<HttpResponseMessage>(
                    TimeSpan.FromSeconds(30),
                    TimeoutStrategy.Pessimistic);

          
            FallbackPolicy = Policy<HttpResponseMessage>
                .Handle<Exception>()  
                .FallbackAsync(
                    fallbackAction: async ct =>
                    {
                        Debug.WriteLine("Executing fallback action: returning default response.");
                        return new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                        {
                            Content = new StringContent("Request failed. Please try again later.")
                        };
                    },
                    onFallbackAsync: async ex =>
                    {
                        Debug.WriteLine($"Fallback triggered due to: {ex.Exception.Message}");
                        await Application.Current.MainPage.DisplayAlert("Request Failed", "Please try again later.", "OK");
                    });

       
            CombinedPolicy = Policy.WrapAsync(FallbackPolicy, TimeoutPolicy, RetryPolicy);
        }
    }
}
