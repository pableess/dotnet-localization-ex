using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin.Localization.Unity;
using Unity;

namespace Owin.Localization
{
    public static partial class RequestLocalizationOptionsExtensions
    {
        /// <summary>
        /// Adds Unity DI support to the Localization Middelware providers
        /// </summary>
        /// <param name="options"></param>
        /// <param name="container">the unity container</param>
        /// <returns></returns>
        public static RequestLocalizationOptions UseUnityInjection(this RequestLocalizationOptions options, IUnityContainer container)
        {
            return options?.UseInjectionAdapterFactory(new RequestCultureInjectionProviderAdapterFactory(container));
        }
    }
}
