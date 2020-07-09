using System.Globalization;
using Microsoft.Owin;

namespace Owin.Extensions.Localization
{
    public static class IOwinContextExtensions
    {
        /// <summary>
        /// Sets the logical call context culture from OwinContext values
        /// </summary>
        /// <param name="owinContext"></param>
        public static void RestoreCulture(this IOwinContext owinContext)
        {
            object c = null;
            if (owinContext?.Environment.TryGetValue(RequestLocalizationMiddleware.CultureKey, out c) == true)
            {
                CultureInfo.CurrentCulture = (CultureInfo)c;
            }

            object uic = null;
            if (owinContext?.Environment.TryGetValue(RequestLocalizationMiddleware.UICultureKey, out uic) == true)
            {
                CultureInfo.CurrentUICulture = (CultureInfo)uic;
            }
        }
    }
}
