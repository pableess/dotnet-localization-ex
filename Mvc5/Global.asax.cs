using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Owin.Extensions.Localization;

namespace Mvc5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();


            this.OnExecuteRequestStep((ctx, step) =>
            {
                if (ctx.Items.Contains(RequestLocalizationMiddleware.OwinEnvironmentKey))
                    ctx.GetOwinContext()?.RestoreCulture();
            });
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
