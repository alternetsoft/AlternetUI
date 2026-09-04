using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the interface for calendar date attributes. You need to invalidate the control after changing
    /// attributes if you want to see the changes immediately.
    /// </summary>
    public interface ICalendarDateAttr : IDisposableObject
    {
        /// <summary>
        /// Gets whether this object is immutable (properties are readonly).
        /// </summary>
        bool Immutable { get; }

        /// <summary>
        /// Gets or sets the text color assigned for the calendar date.
        /// </summary>
        Color? TextColor { get; set; }

        /// <summary>
        /// Gets or sets the background color assigned for the calendar date.
        /// </summary>
        /// <remarks>
        /// This property is not used by the <see cref="XCalendar"/> control.
        /// </remarks>
        Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the border color assigned for the calendar date.
        /// </summary>
        /// <remarks>
        /// This property is not used by the <see cref="XCalendar"/> control.
        /// </remarks>
        Color? BorderColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this calendar day is displayed as a holiday.
        /// </summary>
        /// <remarks>
        /// This property is not used by the <see cref="XCalendar"/> control.
        /// </remarks>
        bool IsHoliday { get; set; }

        /// <summary>
        /// Gets or sets the border assigned for the calendar date.
        /// </summary>
        /// <remarks>
        /// This property is not used by the <see cref="XCalendar"/> control.
        /// </remarks>
        CalendarDateBorder Border { get; set; }

        /// <summary>
        /// Gets a value indicating whether text color is assigned for the calendar date.
        /// </summary>
        bool HasTextColor { get; }

        /// <summary>
        /// Gets a value indicating whether background color is assigned for the calendar date.
        /// </summary>
        bool HasBackgroundColor { get; }

        /// <summary>
        /// Gets a value indicating whether border color is assigned for the calendar date.
        /// </summary>
        bool HasBorderColor { get; }

        /// <summary>
        /// Gets a value indicating whether border style is assigned for the calendar date.
        /// </summary>
        bool HasBorder { get; }
    }
}