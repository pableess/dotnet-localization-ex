// Code modfied from aspnetcore source code https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization/src
// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 


using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Owin.Localization
{
    /// <summary>
    /// Extension methods for the <see cref="RequestLocalizationOptions"/>.
    /// </summary>
    public static class RequestLocalizationOptionsExtensions
    {
        /// <summary>
        /// Adds a new <see cref="RequestCultureProvider"/> to the <see cref="RequestLocalizationOptions.RequestCultureProviders"/>.
        /// </summary>
        /// <param name="requestLocalizationOptions">The cultures to be added.</param>
        /// <param name="requestCultureProvider">The cultures to be added.</param>
        /// <returns>The <see cref="RequestLocalizationOptions"/>.</returns>
        /// <remarks>This method ensures that <paramref name="requestCultureProvider"/> has priority over other <see cref="RequestCultureProvider"/> instances in <see cref="RequestLocalizationOptions.RequestCultureProviders"/>.</remarks>
        public static RequestLocalizationOptions AddInitialRequestCultureProvider(
            this RequestLocalizationOptions requestLocalizationOptions,
            RequestCultureProvider requestCultureProvider)
        {
            if (requestLocalizationOptions == null)
            {
                throw new ArgumentNullException(nameof(requestLocalizationOptions));
            }

            if (requestCultureProvider == null)
            {
                throw new ArgumentNullException(nameof(requestCultureProvider));
            }

            requestLocalizationOptions.RequestCultureProviders.Insert(0, requestCultureProvider);

            return requestLocalizationOptions;
        }


        private static ConditionalWeakTable<RequestLocalizationOptions, IRequestCultureProviderAdapterFactory> factories = new ConditionalWeakTable<RequestLocalizationOptions, IRequestCultureProviderAdapterFactory>();

        /// <summary>
        /// Sets the injection adapter factory for this request options instance
        /// </summary>
        /// <param name="requestLocalizationOptions"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static RequestLocalizationOptions UseInjectionAdapterFactory(this RequestLocalizationOptions requestLocalizationOptions, IRequestCultureProviderAdapterFactory factory)
        {
            if (requestLocalizationOptions == null)
            {
                throw new ArgumentNullException(nameof(requestLocalizationOptions));
            }
            
            factories.Add(requestLocalizationOptions, factory);
            return requestLocalizationOptions;
        }

        /// <summary>
        /// Gets the injection adapter factory for this options object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestLocalizationOptions"></param>
        /// <returns></returns>
        public static IRequestCultureProviderAdapterFactory GetInjectionAdapterFactory<T>(this RequestLocalizationOptions requestLocalizationOptions)
        {
            if (requestLocalizationOptions == null)
            {
                throw new ArgumentNullException(nameof(requestLocalizationOptions));
            }

            IRequestCultureProviderAdapterFactory fac = null;
            factories.TryGetValue(requestLocalizationOptions, out fac);
            return fac;
        }

        /// <summary>
        /// Adds a provider that will be resolved during the request via DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <returns></returns>
        public static RequestLocalizationOptions AddInjectedProvider<T>(this RequestLocalizationOptions options)
            where T : IRequestCultureProvider
        {
            var factory = options?.GetInjectionAdapterFactory<T>();

            if (factory == null)
            {
                throw new InvalidOperationException("Dependency injection has not been configured for this options instance.  You must first call UseXXX on the options instance before you can add injected providers");
            }

            options.RequestCultureProviders.Add(factory.CreateAdapter<T>());
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
            var factory = options?.GetInjectionAdapterFactory<T>();

            if (factory == null)
            {
                throw new InvalidOperationException("Dependency injection has not been configured for this options instance.  You must first call UseXXX on the options instance before you can add injected providers");
            }

            options.RequestCultureProviders.Insert(0, factory.CreateAdapter<T>());
            return options;
        }
    }
}
