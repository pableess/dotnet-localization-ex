# Owin.Extensions.Localization
Port of AspNetCore localization middleware (Microsoft.AspNetCore.Localization) to owin middleware.  The allows a similar programming model as AspNetCore localization request providers making it much eaiser to write custom providers that multi-target both Owin and AspNetCore.

Localization middleware sets the requests culture and ui culture based a series of pluggable request culture providers.

Includes support for Accept-Header, Cookie, and Query string providers.

See https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization for details and documentation.

### Installing 

The package is available via NuGet.

```
Install-Package Owin.Extensions.Localization
```

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




