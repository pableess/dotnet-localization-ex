using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owin.Localization.Autofac;

namespace Owin.Localization
{
    public static partial class RequestLocalizationOptionsExtensions
    {
        private static RequestCultureInjectionProviderAdapterFactory Factory = new RequestCultureInjectionProviderAdapterFactory();

        /// <summary>
        /// Adds Autofac DI support to the Localization Middelware providers
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RequestLocalizationOptions UseAutofacInjection(this RequestLocalizationOptions options)
        {
            return options?.UseInjectionAdapterFactory(Factory);
        }
    }
}
