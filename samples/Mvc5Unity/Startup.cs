using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Owin.Localization;

[assembly: OwinStartup(typeof(Mvc5.Startup))]

namespace Mvc5
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new RequestLocalizationOptions()
                .UseUnityInjection(UnityConfig.Container);
            options.AddSupportedCultures("en-US", "de");
            options.AddSupportedUICultures("en-US", "de");
            options.RequestCultureProviders.Clear();
            options.AddInjectedProvider<TestCultureProvider>();

            app.UseRequestLocalization(options);
        }
    }
}
