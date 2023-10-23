using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// <see cref="ICalendarDateAttr"/> is a custom attributes for a <see cref="Calendar"/> date.
    /// </summary>
    public interface ICalendarDateAttr : IDisposableObject
    {
        /// <summary>
        /// Gets or sets the text color assigned for the calendar date.
        /// </summary>
        public Color? TextColor { get; set; }

        /// <summary>
        /// Gets or sets the background color assigned for the calendar date.
        /// </summary>
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the border color assigned for the calendar date.
        /// </summary>
        public Color? BorderColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this calendar day is displayed as a holiday.
        /// </summary>
        public bool IsHoliday { get; set; }

        /// <summary>
        /// Gets or sets the border assigned for the calendar date.
        /// </summary>
        public CalendarDateBorder Border { get; set; }

        /// <summary>
        /// Gets a value indicating whether text color is assigned for the calendar date.
        /// </summary>
        public bool HasTextColor { get; }

        /// <summary>
        /// Gets a value indicating whether background color is assigned for the calendar date.
        /// </summary>
        public bool HasBackgroundColor { get; }

        /// <summary>
        /// Gets a value indicating whether border color is assigned for the calendar date.
        /// </summary>
        public bool HasBorderColor { get; }

        /// <summary>
        /// Gets a value indicating whether font is assigned for the calendar date.
        /// </summary>
        public bool HasFont { get; }

        /// <summary>
        /// Gets a value indicating whether border style is assigned for the calendar date.
        /// </summary>
        public bool HasBorder { get; }
    }
}