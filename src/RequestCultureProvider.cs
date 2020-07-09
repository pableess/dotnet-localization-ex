// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Threading.Tasks;
using Microsoft.Owin;

namespace Owin.Localization
{
    /// <summary>
    /// An abstract base class provider for determining the culture information of an <see cref="OwinRequest"/>.
    /// </summary>
    public abstract class RequestCultureProvider : IRequestCultureProvider
    {
        /// <summary>
        /// Result that indicates that this instance of <see cref="RequestCultureProvider" /> could not determine the
        /// request culture.
        /// </summary>
        protected static readonly Task<ProviderCultureResult> NullProviderCultureResult = Task.FromResult(default(ProviderCultureResult));

        /// <summary>
        /// The current options for the <see cref="RequestLocalizationMiddleware"/>.
        /// </summary>
        public RequestLocalizationOptions Options { get; set; }

        /// <inheritdoc />
        public abstract Task<ProviderCultureResult> DetermineProviderCultureResult(IOwinContext httpContext);
    }
}
