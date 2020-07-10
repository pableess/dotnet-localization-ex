using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Localization.Autofac
{
    internal class RequestCultureInjectionProviderAdapterFactory : IRequestCultureProviderAdapterFactory
    {
        public IRequestCultureProvider CreateAdapter<TCultureProvider>() where TCultureProvider : IRequestCultureProvider
        {
            return new RequestCultureProviderInjectionAdapter<TCultureProvider>();
        }
    }
}
