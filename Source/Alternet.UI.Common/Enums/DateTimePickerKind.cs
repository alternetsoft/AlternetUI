using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies whether to edit date part or time part of
    /// the <see cref="DateTime"/> value in the <see cref="DateTimePicker"/> control.
    /// </summary>
    public enum DateTimePickerKind
    {
        /// <summary>
        /// Specifies to edit date part of the <see cref="DateTime"/> value
        /// in the <see cref="DateTimePicker"/> control.
        /// </summary>
        Date = 0,

        /// <summary>
        /// Specifies to edit time part of the <see cref="DateTime"/> value
        /// in the <see cref="DateTimePicker"/> control.
        /// </summary>
        Time = 1,
    }
}
