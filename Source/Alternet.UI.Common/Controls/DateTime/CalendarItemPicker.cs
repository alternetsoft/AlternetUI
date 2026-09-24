using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to edit <see cref="CalendarItem"/> properties in a user-friendly manner.
    /// </summary>
    public partial class CalendarItemPicker : TransparentPanel
    {
        private CalendarItem? calendarItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemPicker"/> class.
        /// </summary>
        public CalendarItemPicker()
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="CalendarItem"/> that is being edited by the picker.
        /// </summary>
        public virtual CalendarItem CalendarItem
        {
            get => calendarItem ??= CreateCalendarItem();
            set
            {
                if (calendarItem == value)
                    return;
                calendarItem = value;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="CalendarItem"/>. This method can be overridden
        /// in derived classes to provide a custom implementation of <see cref="CalendarItem"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="CalendarItem"/>.</returns>
        protected virtual CalendarItem CreateCalendarItem()
        {
            return new CalendarItem();
        }
    }
}
