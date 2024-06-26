using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates behavior and styling options for a text box control.
    /// </summary>
    /// <remarks>
    /// This enumeration supports a bitwise combination of its member values.
    /// </remarks>
    [Flags]
    public enum TextBoxOptions
    {
        /// <summary>
        /// Indicates whether to assign value of the 'MaxLength' property
        /// to the handler.
        /// </summary>
        /// <remarks>
        /// When this flag is specified, value of 'MaxLength' is set to the
        /// handler and 'TextMaxLength' event is fired when max length is reached.
        /// </remarks>
        SetNativeMaxLength = 1,

        /// <summary>
        /// Adds integer min/max value range to the error messages.
        /// </summary>
        IntRangeInError = 2,

        /// <summary>
        /// Performs default value validation.
        /// </summary>
        DefaultValidation = 4,
    }
}
