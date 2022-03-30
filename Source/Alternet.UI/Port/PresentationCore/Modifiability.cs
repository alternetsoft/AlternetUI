#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

//
// Description: Modifiability enum used by LocalizabilityAttribute 
//

namespace Alternet.UI
{
    internal enum Modifiability
    {
        /// <summary>
        /// Targeted value is not modifiable by localizers.
        /// </summary>
        Unmodifiable = 0,

        /// <summary>
        /// Targeted value is modifiable by localizers.
        /// </summary>
        Modifiable   = 1,

        /// <summary>
        /// Targeted value's modifiability inherits from the the parent nodes.
        /// </summary>
        Inherit      = 2, 
    }
}    

