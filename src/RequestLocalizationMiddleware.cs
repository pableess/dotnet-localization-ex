// Code modfied from aspnetcore source code https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization/src
// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Microsoft.Owin;

namespace Owin.Extensions.Localization
{
    /// <summary>
    /// Enables automatic setting of the culture for <see cref="Microsoft.Owin.OwinRequest"/>s based on information
    /// sent by the client in headers and logic provided by the application.
    /// </summary>
    public class RequestLocalizationMiddleware : OwinMiddleware
    {
        public const string CultureKey = "Culture";
        public const string UICultureKey = "UICulture";
        public const string OwinEnvironmentKey = "owin.Environment";

        private static readonly int MaxCultureFallbackDepth = 5;

        private readonly RequestLocalizationOptions _options;

        /// <summary>
        /// Creates a new <see cref="RequestLocalizationMiddleware"/>.
        /// </summary>
        /// <param name="next">The <see cref="RequestDelegate"/> representing the next middleware in the pipeline.</param>
        /// <param name="options">The <see cref="RequestLocalizationOptions"/> representing the options for the. 
        /// <see cref="RequestLocalizationMiddleware"/></param>
        public RequestLocalizationMiddleware(OwinMiddleware next, RequestLocalizationOptions options) :base(next)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Invokes the logic of the middleware.
        /// </summary>
        /// <param name="context">The <see cref="IOwinContext"/>.</param>
        /// <returns>A <see cref="Task"/> that completes when the middleware has completed processing.</returns>
        public override async Task Invoke(IOwinContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var requestCulture = _options.DefaultRequestCulture;

            IRequestCultureProvider winningProvider = null;

            if (_options.RequestCultureProviders != null)
            {
                foreach (var provider in _options.RequestCultureProviders)
                {
                    var providerResultCulture = await provider.DetermineProviderCultureResult(context);
                    if (providerResultCulture == null)
                    {
                        continue;
                    }
                    var cultures = providerResultCulture.Cultures;
                    var uiCultures = providerResultCulture.UICultures;

                    CultureInfo cultureInfo = null;
                    CultureInfo uiCultureInfo = null;
                    if (_options.SupportedCultures != null)
                    {
                        cultureInfo = GetCultureInfo(
                            cultures,
                            _options.SupportedCultures,
                            _options.FallBackToParentCultures);

                        if (cultureInfo == null)
                        {
                            System.Diagnostics.Trace.WriteLine($"{provider.GetType().Name} returned the following unsupported cultures '{string.Join(", ", cultures)}'");
                        }
                    }

                    if (_options.SupportedUICultures != null)
                    {
                        uiCultureInfo = GetCultureInfo(
                            uiCultures,
                            _options.SupportedUICultures,
                            _options.FallBackToParentUICultures);

                        if (uiCultureInfo == null)
                        {
                            System.Diagnostics.Trace.WriteLine($"{provider.GetType().Name} returned the following unsupported UI cultures '{string.Join(", ", uiCultures)}'");
                        }
                    }

                    if (cultureInfo == null && uiCultureInfo == null)
                    {
                        continue;
                    }

                    if (cultureInfo == null && uiCultureInfo != null)
                    {
                        cultureInfo = _options.DefaultRequestCulture.Culture;
                    }
                    else if (cultureInfo != null && uiCultureInfo == null)
                    {
                        uiCultureInfo = _options.DefaultRequestCulture.UICulture;
                    }

                    var result = new RequestCulture(cultureInfo, uiCultureInfo);

                    if (result != null)
                    {
                        requestCulture = result;
                        winningProvider = provider;
                        break;
                    }
                }
            }

            context.Set<IRequestCultureFeature>("RequestCultureFeature", new RequestCultureFeature(requestCulture, winningProvider));

            SetCurrentThreadCulture(requestCulture);

            if (_options.ApplyCurrentCultureToResponseHeaders)
            {
                context.Response.Headers.Add("Content-Language", new[] { requestCulture.UICulture.Name });
            }

            await this.Next.Invoke(context);
        }

        private static void SetCurrentThreadCulture(RequestCulture requestCulture)
        {
            CultureInfo.CurrentCulture = requestCulture.Culture;
            CultureInfo.CurrentUICulture = requestCulture.UICulture;
        }

        private static CultureInfo GetCultureInfo(
            IList<StringSegment> cultureNames,
            IList<CultureInfo> supportedCultures,
            bool fallbackToParentCultures)
        {
            foreach (var cultureName in cultureNames)
            {
                // Allow empty string values as they map to InvariantCulture, whereas null culture values will throw in
                // the CultureInfo ctor
                if (cultureName != null)
                {
                    var cultureInfo = GetCultureInfo(cultureName, supportedCultures, fallbackToParentCultures, currentDepth: 0);
                    if (cultureInfo != null)
                    {
                        return cultureInfo;
                    }
                }
            }

            return null;
        }

        private static CultureInfo GetCultureInfo(StringSegment name, IList<CultureInfo> supportedCultures)
        {
            // Allow only known culture names as this API is called with input from users (HTTP requests) and
            // creating CultureInfo objects is expensive and we don't want it to throw either.
            if (name == null || supportedCultures == null)
            {
                return null;
            }
            var culture = supportedCultures.FirstOrDefault(
                supportedCulture => StringSegment.Equals(supportedCulture.Name, name, StringComparison.OrdinalIgnoreCase));

            if (culture == null)
            {
                return null;
            }

            return CultureInfo.ReadOnly(culture);
        }

        private static CultureInfo GetCultureInfo(
            StringSegment cultureName,
            IList<CultureInfo> supportedCultures,
            bool fallbackToParentCultures,
            int currentDepth)
        {
            var culture = GetCultureInfo(cultureName, supportedCultures);

            if (culture == null && fallbackToParentCultures && currentDepth < MaxCultureFallbackDepth)
            {
                var lastIndexOfHyphen = cultureName.LastIndexOf('-');

                if (lastIndexOfHyphen > 0)
                {
                    // Trim the trailing section from the culture name, e.g. "fr-FR" becomes "fr"
                    var parentCultureName = cultureName.Subsegment(0, lastIndexOfHyphen);

                    culture = GetCultureInfo(parentCultureName, supportedCultures, fallbackToParentCultures, currentDepth + 1);
                }
            }

            return culture;
        }

    }
}
