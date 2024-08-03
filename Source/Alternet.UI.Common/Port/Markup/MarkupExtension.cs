#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Alternet.UI
{
    /// <summary>
    ///  Base class for all Xaml markup extensions.  Only subclasses can
    ///  be instantiated.
    /// </summary>
    internal abstract class MarkupExtension 
    {
        /// <summary>
        ///  Return an object that should be set on the targetObject's targetProperty
        ///  for this markup extension.  
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        ///  The object to set on this property.
        /// </returns>
        public abstract object ProvideValue(IServiceProvider serviceProvider);
       
    }
}

