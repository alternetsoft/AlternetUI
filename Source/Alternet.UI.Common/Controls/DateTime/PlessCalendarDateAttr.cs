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
    /// Represents the attributes of a calendar date, including text color, background color,
    /// border color, holiday status, and border style.
    /// </summary>
    public partial class PlessCalendarDateAttr : ImmutableObject, ICalendarDateAttr
    {
        private Color? textColor;
        private Color? backgroundColor;
        private Color? borderColor;
        private bool isHoliday;
        private CalendarDateBorder border;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCalendarDateAttr"/> class.
        /// </summary>
        /// <param name="border">The border settings for the calendar date. Optional. If not specified,
        /// the default value is <see cref="CalendarDateBorder.None"/>.</param>
        public PlessCalendarDateAttr(CalendarDateBorder border = 0)
        {
            this.border = border;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlessCalendarDateAttr"/> class with the specified text color.
        /// </summary>
        /// <param name="textColor">The text color for the calendar date.</param>
        public PlessCalendarDateAttr(Color textColor)
        {
            this.textColor = textColor;
        }

        /// <inheritdoc/>
        public virtual Color? TextColor
        {
            get => textColor;
            set
            {
                SetProperty(ref textColor, value);
            }
        }

        /// <inheritdoc/>
        public virtual Color? BackgroundColor
        {
            get => backgroundColor;
            set
            {
                SetProperty(ref backgroundColor, value);
            }
        }

        /// <inheritdoc/>
        public virtual Color? BorderColor
        {
            get => borderColor;
            set
            {
                SetProperty(ref borderColor, value);
            }
        }

        /// <inheritdoc/>
        public virtual bool IsHoliday
        {
            get => isHoliday;
            set
            {
                SetProperty(ref isHoliday, value);
            }
        }

        /// <inheritdoc/>
        public virtual CalendarDateBorder Border
        {
            get => border;
            set
            {
                SetProperty(ref border, value);
            }
        }

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