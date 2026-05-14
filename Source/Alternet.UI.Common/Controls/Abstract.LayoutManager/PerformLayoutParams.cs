using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the parameters for performing layout.
    /// </summary>
    public class PerformLayoutParams
    {
        /// <summary>
        /// Gets the default instance of the <see cref="PerformLayoutParams"/> class,
        /// which has the reason for performing layout set to <see cref="PerformLayoutReason.NotSpecified"/>.
        /// </summary>
        public static PerformLayoutParams Default { get; } = new PerformLayoutParams();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformLayoutParams"/> class with the specified reason for performing layout.
        /// </summary>
        /// <param name="reason">The reason for performing layout.</param>
        /// <param name="originalTarget">The original target of the layout operation. Optional.</param>
        public PerformLayoutParams(PerformLayoutReason reason, ILayoutItem? originalTarget = null)
        {
            Reason = reason;
            OriginalTarget = originalTarget;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformLayoutParams"/> class.
        /// </summary>
        public PerformLayoutParams()
        {
        }

        /// <summary>
        /// Gets the original target of the layout operation, which is the control that initiated the layout process.
        /// </summary>
        public ILayoutItem? OriginalTarget { get; }

        /// <summary>
        /// Gets the reason for performing layout.
        /// </summary>
        public PerformLayoutReason Reason { get; } = PerformLayoutReason.NotSpecified;

        /// <summary>
        /// Gets or sets an optional parameter that provides additional context for the reason.
        /// </summary>
        public object? ReasonParameter { get; set; }
    }
}
