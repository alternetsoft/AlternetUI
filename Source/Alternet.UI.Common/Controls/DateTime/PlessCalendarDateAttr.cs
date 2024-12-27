using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ICalendarDateAttr"/> interface implementation that does nothing.
    /// </summary>
    public class PlessCalendarDateAttr : ImmutableObject, ICalendarDateAttr
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCalendarDateAttr"/> class.
        /// </summary>
        public PlessCalendarDateAttr()
        {
        }

        /// <inheritdoc/>
        public Color? TextColor { get; set; }

        /// <inheritdoc/>
        public Color? BackgroundColor { get; set; }

        /// <inheritdoc/>
        public Color? BorderColor { get; set; }

        /// <inheritdoc/>
        public bool IsHoliday { get; set; }

        /// <inheritdoc/>
        public CalendarDateBorder Border { get; set; }

        /// <inheritdoc/>
        public bool HasTextColor { get; set; }

        /// <inheritdoc/>
        public bool HasBackgroundColor { get; set; }

        /// <inheritdoc/>
        public bool HasBorderColor { get; set; }

        /// <inheritdoc/>
        public bool HasFont { get; set; }

        /// <inheritdoc/>
        public bool HasBorder { get; set; }
    }
}