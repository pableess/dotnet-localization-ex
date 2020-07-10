# dotnet localization ex

This project contains the following localization enhancements for .NET web applications.

1. Adds dependency injection support for IRequestCultureProviders in AspNetCore 
    - Why? The AspNetCore Localization middleware allows only the addition of Provider instances to the RequestCultureOptions. For simple providers this is not a problem because the culture can be determined from information on the request. However, more complex custom providers may use external systems like databases, web services, distributed caches etc. This allows the providers to be injected per request allows these dependencies to work much better with other Dependency Injection oriented services.

2. Provides a port of AspNetCore localization middleware (Microsoft.AspNetCore.Localization) to owin middleware.  
    - This allows a similar programming model as AspNetCore localization request providers making it much eaiser to write custom providers that multi-target both Owin and AspNetCore.  This support also contains support for dependency injection of the providers.

See https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization for details and documentation.

### Packages 

This project provides the following packages:

+ AspNetCore.Localization.DependencyInjection
    - Dependency injection support for AspNetCore localization middleware culture providers
+ Owin.Localization 
    - Port of Microsoft.AspNetCore.Localization middleware to OWIN middleware.  Also adds dependency injection support
+ Owin.Localization.Autofac 
    - Autofac container integration
+ Owin.Localization.Unity 
    - Unity container integration


### Installing 

The packages are available via NuGet.

AspNetCore packages
```
dotnet add package AspNetCore.Localization.DependencyInjection
```
OWIN packages
```
Install-Package Owin.Localization
Install-Package Owin.Localization.Autofac
Install-Package Owin.Localization.Unity
```
# Usage

## AspNetCore

```csharp
// in ConfigureServices
services.AddScoped<TestCultureProvider>();

...

 // in RequestLocalizationOptions configuration
RequestLocalizationOptions options = new RequestLocalizationOptions().AddInitialInjectedProvider<TestCultureProvider>();

```

## OWIN

The owin usage is pretty much the same as using AspNetCore localization middleware.  See https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization for details and documentation.

### Dependency Injection with Owin
The dependency injection support with OWIN requires an addition extension method is called on the RequestCultureOptions before calling .AddInjectedProvider<T>() or .AddInitialInjectedProvider<T>()

```csharp
var options = new RequestLocalizationOptions().UseUnityInjection(UnityConfig.Container);

or 

var options = new RequestLocalizationOptions().UseAutofacInjection();
```
* the providers also need to be registred as scoped services with the container

###  AspNet MVC projects
AspNet MVC projects require a small amount of additonal code to ensure that the assigned culture flows accross from the OWIN middleware to the AspNet request MVC pipeline, because of the way AspNet handles async Owin middleware.

For an AspNet Mvc (.NET Framework 4.7.1 or higher) project, override the Init() of the HttpApplication class as follows:

```csharp
   public override void Init()
   {
        base.Init();

        // flow owin culture to every aspnet step 
        this.OnExecuteRequestStep((ctx, step) =>
        {
           if (ctx.Items.Contains(RequestLocalizationMiddleware.OwinEnvironmentKey))
              ctx.GetOwinContext()?.RestoreCulture();
        });
   }
```




