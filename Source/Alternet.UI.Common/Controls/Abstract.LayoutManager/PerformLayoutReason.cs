using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the reason for performing layout.
    /// </summary>
    public enum PerformLayoutReason
    {
        /// <summary>
        /// The reason for performing layout is not specified.
        /// </summary>
        NotSpecified,

        /// <summary>
        /// The reason for performing layout is that the control's <see cref="AbstractControl.SuggestedSize"/> property has changed.
        /// </summary>
        SuggestedSizeChanged,
    }
}
