using System;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.DependencyInjection;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for the <see cref="RequestLocalizationOptions"/>.
    /// </summary>
    public static class RequestLocalizationOptionsExtensions
    {

        /// <summary>
        /// Adds a provider that will be resolved during the request via DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RequestLocalizationOptions AddInjectedProvider<T>(this RequestLocalizationOptions options)
            where T : IRequestCultureProvider
        {
            options.RequestCultureProviders.Add(new RequestCultureProviderInjectionAdapter<T>());
            return options;
        }

        /// <summary>
        /// Adds a provider that will be resolved during the request via DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <remarks>This method ensures that injected provider has priority over other <see cref="RequestCultureProvider"/> instances in <see cref="RequestLocalizationOptions.RequestCultureProviders"/>.</remarks>
        public static RequestLocalizationOptions AddInitialInjectedProvider<T>(this RequestLocalizationOptions options)
            where T : IRequestCultureProvider
        {
            options.RequestCultureProviders.Insert(0, new RequestCultureProviderInjectionAdapter<T>());
            return options;
        }
    }
}
