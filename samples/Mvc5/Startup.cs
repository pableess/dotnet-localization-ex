using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Mvc5.Startup))]

namespace Mvc5
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseRequestLocalization("en-us", "de", "fr-ca");
        }
    }
}
