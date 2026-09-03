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
    public partial class PlessCalendarDateAttr : ImmutableObject, ICalendarDateAttr
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCalendarDateAttr"/> class.
        /// </summary>
        public PlessCalendarDateAttr()
        {
        }

        /// <inheritdoc/>
        public virtual Color? TextColor { get; set; }

        /// <inheritdoc/>
        public virtual Color? BackgroundColor { get; set; }

        /// <inheritdoc/>
        public virtual Color? BorderColor { get; set; }

        /// <inheritdoc/>
        public virtual bool IsHoliday { get; set; }

        /// <inheritdoc/>
        public virtual CalendarDateBorder Border { get; set; }

        /// <inheritdoc/>
        public bool HasTextColor => TextColor != null;

        /// <inheritdoc/>
        public bool HasBackgroundColor => BackgroundColor != null;

        /// <inheritdoc/>
        public bool HasBorderColor => BorderColor != null;

        /// <inheritdoc/>
        public bool HasBorder => Border != CalendarDateBorder.None;
    }
}