#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description: Readability enum used by LocalizabilityAttribute 
//

namespace Alternet.UI
{
    internal enum Readability 
    {
        /// <summary>
        /// Targeted value is not readable.
        /// </summary>
        Unreadable = 0,

        /// <summary>
        /// Targeted value is readable text.
        /// </summary>
        Readable   = 1,

        /// <summary>
        /// Targeted value's readability inherites from parent nodes.
        /// </summary>
        Inherit    = 2,            
    }
}

