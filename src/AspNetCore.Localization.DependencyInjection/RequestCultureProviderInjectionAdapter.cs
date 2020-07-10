using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Localization.DependencyInjection
{
    /// <summary>
    /// Wrapper culture provider that resolves another type of provider using the owin request's lifetime scope during and forwards the determination to that provider
    /// This allows for injecting in dependencies like shared cache or database connections, http clients etc using DI best practices
    /// </summary>
    /// <typeparam name="TCultureProvider"></typeparam>
    internal class RequestCultureProviderInjectionAdapter<TCultureProvider> : IRequestCultureProvider
        where TCultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// /
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
            var provider = httpContext.RequestServices.GetService<TCultureProvider>();

            if (provider != null)
            {
                return provider.DetermineProviderCultureResult(httpContext);
            }

            throw new InvalidOperationException($"IRequestCultureProvider of type {typeof(TCultureProvider)} could not be resolved from the IServiceProvider");
        }
    }
}
