using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the result of an attempt to assign a value to a property.
    /// </summary>
    /// <remarks>This enumeration is used to indicate whether a property
    /// assignment operation succeeded, failed, or was not allowed.</remarks>
    public enum PropertyUpdateResult
    {
        /// <summary>
        /// Represents a failure result or state in an operation.
        /// </summary>
        /// <remarks>
        /// This enumeration value is typically used to indicate
        /// that an operation did not succeed.</remarks>
        Failure,

        /// <summary>
        /// Represents the result of an operation indicating success.
        /// </summary>
        /// <remarks>This value is typically used to signify that an operation completed
        /// successfully.</remarks>
        Ok,

        /// <summary>
        /// Represents a condition or state where changes are not allowed.
        /// </summary>
        /// <remarks>This value is typically used to indicate that an
        /// operation or property cannot be modified.</remarks>
        CannotChange,
    }
}