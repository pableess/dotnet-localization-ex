using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace Mvc6
{
    public class TestCultureProvider : IRequestCultureProvider
    {
        public TestCultureProvider()
        {
            Console.WriteLine("Injected!!!");
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            return Task.FromResult(new ProviderCultureResult("de"));
        }
    }
}
