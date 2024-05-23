using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates options for <see cref="TextBox"/> behavior and visual style customization.
    /// </summary>
    [Flags]
    public enum TextBoxOptions
    {
        /// <summary>
        /// Indicates whether to set value of <see cref="TextBox.MaxLength"/> property
        /// to the native control.
        /// </summary>
        /// <remarks>
        /// When this flag is specified, value of <see cref="TextBox.MaxLength"/> is set to the
        /// native control and <see cref="TextBox.TextMaxLength"/> event is fired when max length
        /// is reached.
        /// </remarks>
        SetNativeMaxLength = 1,

        /// <summary>
        /// Adds integer min/max value range to the error messages.
        /// </summary>
        IntRangeInError = 2,

        /// <summary>
        /// Perform default value validation using <see cref="CustomTextBox.RunDefaultValidation"/>.
        /// </summary>
        DefaultValidation = 4,
    }
}
