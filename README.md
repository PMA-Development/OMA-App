# OMA-APP

# Description
A mobile application for "GoGreen" to assist workers in managing tasks and viewing telemetry data, both recent and historical, displayed in graphs, Currently targeted Android.

# Project Details

| Platform            | GUI Framework | Timeframe | Database Solution         | Authentication Framework       |
|---------------------|---------------|-----------|---------------------------|--------------------------------|
| .NET 8 (MAUI)      | XAML          | November  | N/A (API-based backend)   | Duende IdentityServer (OIDC, OAuth2) |


# Getting Started

### Software Requirements to run Android Emulator
-   **.NET SDK**:
    -   Install the latest version of the **.NET SDK (version 8.0)**.
-   **Visual Studio**:
    -   Visual Studio 2022 (latest version) with the following workloads:
        -   **.NET Multi-platform App UI Development**
        -   **Mobile development with .NET**
        -   Ensure **Android SDKs** and **Emulator** components are checked in the installer.
-   **Android SDK and Emulator**:
    -   Install Android SDKs via the Android SDK Manager in Visual Studio.
    -   Ensure the correct API levels are installed for your target Android versions.
	
### Setup Instructions
1. Clone the Repository
2. Set up Constants.cs
```json
// OIDC Configuration
public static readonly string Authority = "";
public static readonly string ClientId = "";
public static readonly string Scope = "openid profile role";
public static readonly string PostLogoutRedirectUri = "myapp://auth";
public static readonly string RedirectUri = "myapp://auth";

// API Configuration
public static readonly string APIURI = "";
}
```
3. Start Emulator
4. Run the application



# NuGet Packages

[CommunityToolkit.Mvvm - 8.2.2](https://www.nuget.org/packages/CommunityToolkit.Mvvm/8.2.2#readme-body-tab) By Microsoft Toolkit

[CommunityToolkit.Maui - 9.1.0](https://www.nuget.org/packages/CommunityToolkit.Maui/9.1.0#readme-body-tab) By Microsoft Toolkit

[IdentityModel.OidcClient - 6.0.0](https://www.nuget.org/packages/IdentityModel.OidcClient/6.0.0)  By Duende

[Microsoft.Maui.Controls - 8.0.82](https://www.nuget.org/packages/Microsoft.Maui.Controls/8.0.82#readme-body-tab) By Microsoft

[Microsoft.Maui.Controls.Compatibility - 8.0.82](https://www.nuget.org/packages/Microsoft.Maui.Controls.Compatibility/8.0.82#readme-body-tab) By Microsoft

[Microsoft.Extensions.Logging.Debug - 8.0.0](https://www.nuget.org/packages/Microsoft.Extensions.Logging.Debug/8.0.0#readme-body-tab) By Microsoft

[Newtonsoft.Json - 13.0.3](https://www.nuget.org/packages/Newtonsoft.Json/13.0.3#readme-body-tab) By NewtonSoft

[System.IdentityModel.Tokens.Jwt - 8.2.0](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/8.2.0#readme-body-tab) By Microsoft

[Polly – 8.5.0](https://www.nuget.org/packages/Polly/8.5.0#readme-body-tab) By Polly

[Polly.Caching.Memory - 3.0.2](https://www.nuget.org/packages/Polly.Caching.Memory/3.0.2#readme-body-tab) By Polly

[Syncfusion.Maui.Charts - 27.2.2](https://www.nuget.org/packages/Syncfusion.Maui.Charts/27.2.2#readme-body-tab) By Syncfusion
