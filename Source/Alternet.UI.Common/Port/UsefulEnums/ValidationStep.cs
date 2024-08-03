#pragma warning disable
#nullable disable
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// Description: Enum describing the different steps in the validation process.

using System.ComponentModel;

namespace Alternet.UI.Port
{
    /// <summary>
    /// This enum describes the different steps in the validation process.
    /// </summary>
    internal enum ValidationStep
    {
        /// <summary> Obtain the value from the target element </summary>
        RawProposedValue,
        /// <summary> Apply any conversions needed to produce a value suitable
        /// for the source </summary>
        ConvertedProposedValue,
        /// <summary> Update the source with the new value </summary>
        UpdatedValue,
        /// <summary> Commit the source's new values.  This step does nothing
        /// unless the source supports a commit mechanism such as <see cref="IEditableObject"/>
        /// </summary>
        CommittedValue,
    }
}
