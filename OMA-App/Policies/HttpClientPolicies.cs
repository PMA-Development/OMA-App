using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Polly;
using System.Diagnostics;

namespace OMA_App.Policies
{
    public class HttpClientPolicies
    {
        public AsyncRetryPolicy<HttpResponseMessage> LoggingExponentialHttpRetry { get; }
        public HttpClientPolicies()
        {
            
            LoggingExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
            res => !res.IsSuccessStatusCode)
            .WaitAndRetryAsync
            (
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (ex, time) =>
                {
                    Debug.WriteLine($"========================> TimeSpan: {time.TotalSeconds}");
                }
            );
        }
    }
}
