#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Alternet.UI.Markup
{
    ///<summary>
    ///     The IUriContext interface allows elements (like Frame, PageViewer) and type converters
    ///     (like BitmapImage TypeConverters) a way to ensure that base uri is set on them by the 
    ///     parser, codegen for xaml, baml and caml cases.  The elements can then use this base uri
    ///     to navigate.
    ///</summary>
    public interface IUriContext
    {
        /// <summary>
        ///     Provides the base uri of the current context.
        /// </summary>
        Uri BaseUri
        {
            get;
            set;
        }
    }


}
