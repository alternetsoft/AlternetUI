#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Unifies enumerable, collection, and dictionary support that are useful for exposing a dictionary of names in a UIXML namescope.
    /// </summary>
    internal interface INameScopeDictionary : INameScope, IDictionary<string, object>
    {
    }
}
