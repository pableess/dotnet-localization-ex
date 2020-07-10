using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Owin.Localization.Unity
{
    internal class RequestCultureInjectionProviderAdapterFactory : IRequestCultureProviderAdapterFactory
    {

        private readonly IUnityContainer unityContainer;

        public RequestCultureInjectionProviderAdapterFactory(IUnityContainer container)
        {
            this.unityContainer = container ?? throw new ArgumentNullException(nameof(container));
        }

        public IRequestCultureProvider CreateAdapter<TCultureProvider>() where TCultureProvider : IRequestCultureProvider
        {
            return new RequestCultureProviderInjectionAdapter<TCultureProvider>(this.unityContainer);
        }
    }
}
