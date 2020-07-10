using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Owin.Localization;

namespace Mvc5
{
    public class TestCultureProvider : IRequestCultureProvider
    {
        public TestCultureProvider()
        {
            Console.WriteLine("Injected!!!");
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(IOwinContext httpContext)
        {
            return Task.FromResult(new ProviderCultureResult("de"));
        }
    }
}
