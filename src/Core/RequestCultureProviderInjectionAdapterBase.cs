using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin.Localization
{
    /// <summary>
    /// Base class for implmenting an adapter that uses a dependency injection container to resolve the provider instance instance of using a single instance during option configuration.
    /// This allows provider dependencies to be resolved per request (things like DB connections, shared caches, etc)
    /// </summary>
    /// <typeparam name="TCultureProvider"></typeparam>
    public abstract class RequestCultureProviderInjectionAdapterBase<TCultureProvider> : IRequestCultureProvider
        where TCultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// Gets and instance of TCultureProvider
        /// </summary>
        /// <returns></returns>
        protected abstract  TCultureProvider ResolveProvider(IOwinContext httpContext);

        /// <summary>
        /// /
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task<ProviderCultureResult> DetermineProviderCultureResult(IOwinContext httpContext)
        {
            var provider = this.ResolveProvider(httpContext);

            if (provider != null)
            {
                return provider.DetermineProviderCultureResult(httpContext);
            }

            throw new InvalidOperationException($"IRequestCultureProvider of type {typeof(TCultureProvider)} could not be resolved");
        }
    }
}
