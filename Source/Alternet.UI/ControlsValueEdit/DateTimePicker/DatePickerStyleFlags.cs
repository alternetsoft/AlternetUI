using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Defines <see cref="DateTimePicker"/> flags when it is in date editing mode.
    /// </summary>
    public enum DatePickerStyleFlags
    {
        /// <summary>
        /// Default style on this platform, either <see cref="Spin"/> or <see cref="DropDown"/>.
        /// </summary>
        Default = 0,

        /// <summary>
        /// A spin control-like date picker (not supported in generic version).
        /// </summary>
        Spin = 1,

        /// <summary>
        /// A combobox-like date picker (not supported in macOs version).
        /// </summary>
        DropDown = 2,

        /// <summary>
        /// Always show century in the default date display (otherwise it depends on
        /// the system date format which may include the century or not).
        /// </summary>
        ShowCentury = 4,

        /// <summary>
        /// Allow not having any valid date in the control (by default it always has
        /// some date, today initially if no valid date specified).
        /// </summary>
        AllowNone = 8,
    }
}
