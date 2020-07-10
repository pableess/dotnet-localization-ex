using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Owin.Localization;
using Autofac;
using System.Runtime.InteropServices;

[assembly: OwinStartup(typeof(Mvc5.Startup))]

namespace Mvc5
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<TestCultureProvider>().InstancePerLifetimeScope();

            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            var options = new RequestLocalizationOptions()
                .UseAutofacInjection();
            options.AddSupportedCultures("en-US", "de");
            options.AddSupportedUICultures("en-US", "de");
            options.RequestCultureProviders.Clear();
            options.AddInjectedProvider<TestCultureProvider>();



            app.UseRequestLocalization(options);
        }
    }
}
