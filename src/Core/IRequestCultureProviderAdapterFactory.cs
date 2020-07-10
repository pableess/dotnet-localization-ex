using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owin.Localization
{
    public interface IRequestCultureProviderAdapterFactory
    {
        /// <summary>
        /// Create an adapter for the IRequestProvider type
        /// </summary>
        /// <typeparam name="TCultureProvider"></typeparam>
        /// <returns></returns>
        IRequestCultureProvider CreateAdapter<TCultureProvider>()
            where TCultureProvider : IRequestCultureProvider;
    }
}
