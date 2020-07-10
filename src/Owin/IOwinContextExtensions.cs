using System.Globalization;
using Microsoft.Owin;

namespace Owin.Localization
{
    public static class IOwinContextExtensions
    {
        /// <summary>
        /// Sets the logical call context culture from OwinContext values
        /// </summary>
        /// <param name="owinContext"></param>
        public static void RestoreCulture(this IOwinContext owinContext)
        {
            var requestCulture = owinContext.GetRequestCulture();

            if (requestCulture != null)
            {
                CultureInfo.CurrentCulture = requestCulture.Culture;
                CultureInfo.CurrentUICulture = requestCulture.UICulture;
            }
        }

        /// <summary>
        /// Gets the request culture from the owin 
        /// </summary>
        /// <param name="owinContext"></param>
        /// <returns></returns>
        public static RequestCulture GetRequestCulture(this IOwinContext owinContext)
        {
            return owinContext?.Get<IRequestCultureFeature>(RequestCultureFeature.RequestCultureFeatureKey)?.RequestCulture;
        }

        /// <summary>
        /// Gets the request culture feature from the owin context
        /// </summary>
        /// <param name="owinContext"></param>
        /// <returns></returns>
        public static IRequestCultureFeature GetRequestCultureFeature(this IOwinContext owinContext)
        {
            return owinContext?.Get<IRequestCultureFeature>(RequestCultureFeature.RequestCultureFeatureKey);
        }
    }
}
