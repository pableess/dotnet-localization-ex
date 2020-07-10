// Code modfied from aspnetcore source code https://github.com/dotnet/aspnetcore/tree/master/src/Middleware/Localization/src
// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin.Localization
{
    /// <summary>
    /// Represents a provider for determining the culture information of an <see cref="IOwinContext"/>.
    /// </summary>
    public interface IRequestCultureProvider
    {
        /// <summary>
        /// Implements the provider to determine the culture of the given request.
        /// </summary>
        /// <param name="httpContext">The <see cref="IOwinContext"/> for the request.</param>
        /// <returns>
        ///     The determined <see cref="ProviderCultureResult"/>.
        ///     Returns <c>null</c> if the provider couldn't determine a <see cref="ProviderCultureResult"/>.
        /// </returns>
        Task<ProviderCultureResult> DetermineProviderCultureResult(IOwinContext httpContext);
    }
}
