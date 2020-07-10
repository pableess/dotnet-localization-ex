// Code modfied from aspnetcore source code https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization/src
// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin.Localization
{
    /// <summary>
    /// Determines the culture information for a request via the configured delegate.
    /// </summary>
    public class CustomRequestCultureProvider : RequestCultureProvider
    {
        private readonly Func<IOwinContext, Task<ProviderCultureResult>> _provider;

        /// <summary>
        /// Creates a new <see cref="CustomRequestCultureProvider"/> using the specified delegate.
        /// </summary>
        /// <param name="provider">The provider delegate.</param>
        public CustomRequestCultureProvider(Func<IOwinContext, Task<ProviderCultureResult>> provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            _provider = provider;
        }

        /// <inheritdoc />
        public override Task<ProviderCultureResult> DetermineProviderCultureResult(IOwinContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            return _provider(httpContext);
        }
    }
}
