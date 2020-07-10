using System;
using Microsoft.Owin;
using Unity;

namespace Owin.Localization.Unity
{
    /// <summary>
    /// Wrapper culture provider that resolves another type of provider using the owin request's lifetime scope during and forwards the determination to that provider
    /// This allows for injecting in dependencies like shared cache or database connections, http clients etc using DI best practices
    /// </summary>
    /// <typeparam name="TCultureProvider"></typeparam>
    public class RequestCultureProviderInjectionAdapter<TCultureProvider> : RequestCultureProviderInjectionAdapterBase<TCultureProvider>
        where TCultureProvider : IRequestCultureProvider
    {
        private readonly IUnityContainer unityContainer;

        public RequestCultureProviderInjectionAdapter(IUnityContainer container)
        {
            this.unityContainer = container;
        }

        protected override TCultureProvider ResolveProvider(IOwinContext httpContext)
        {
            return this.unityContainer.Resolve<TCultureProvider>();
        }
    }
}
