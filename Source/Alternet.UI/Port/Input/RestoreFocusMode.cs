// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Alternet.UI
{
    /// <summary>
    /// The focus restoration mode.
    /// </summary>
    public enum RestoreFocusMode
    {
        /// <summary>
        /// The framework automatically tries to restore focus to the element that last had focus.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// The framework does not restore focus.
        /// </summary>
        None
    }
}
