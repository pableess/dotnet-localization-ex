using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin;

namespace Owin.Localization.Autofac
{
    /// <summary>
    /// Wrapper culture provider that resolves another type of provider using the owin request's lifetime scope during and forwards the determination to that provider
    /// This allows for injecting in dependencies like shared cache or database connections, http clients etc using DI best practices
    /// </summary>
    /// <typeparam name="TCultureProvider"></typeparam>
    internal class RequestCultureProviderInjectionAdapter<TCultureProvider> : RequestCultureProviderInjectionAdapterBase<TCultureProvider>
        where TCultureProvider : IRequestCultureProvider
    {
        protected override TCultureProvider ResolveProvider(IOwinContext httpContext)
        {
            var scope = httpContext.GetAutofacLifetimeScope();

            if (scope == null)
            {
                throw new InvalidOperationException("Autofac lifetime scope is not present on the owin context.  Ensure Autofac middleware had been addeed");
            }

            return scope.Resolve<TCultureProvider>();
        }
    }
}
