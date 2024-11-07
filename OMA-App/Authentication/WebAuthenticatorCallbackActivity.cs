#if ANDROID
using Android.App;
using Android.Content.PM;
using Microsoft.Maui.Authentication;

namespace OMA_App.Authentication
{
    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop, Exported = true)] // Add Exported = true
    [IntentFilter(
        new[] { Android.Content.Intent.ActionView },
        Categories = new[] { Android.Content.Intent.CategoryDefault, Android.Content.Intent.CategoryBrowsable },
        DataScheme = "myapp", // Replace with your app's scheme
        DataHost = "auth"     // Replace with your app's host
    )]
    public class WebAuthenticatorCallbackActivity : Microsoft.Maui.Authentication.WebAuthenticatorCallbackActivity
    {
    }
}
#endif
