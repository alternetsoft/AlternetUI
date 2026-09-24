using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to edit <see cref="CalendarItem"/> properties in a user-friendly manner.
    /// </summary>
    public partial class CalendarItemPicker : TransparentPanel
    {
        private CalendarItem? calendarItem;

        /*
                private string? title;
                private string? description;
                private object? location;
                private CalendarItemCategory? category;
                private CalendarItemStatus? status;
                private object? userData;
                private string? userName;
        */

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemPicker"/> class.
        /// </summary>
        public CalendarItemPicker()
        {
        }

        /// <summary>
        /// Occurs when the <see cref="CalendarItem"/> being edited has changed.
        /// </summary>
        public event EventHandler? ValueChanged;

        /// <summary>
        /// Gets or sets the <see cref="CalendarItem"/> that is being edited by the picker.
        /// </summary>
        public virtual CalendarItem Value
        {
            get
            {
                if (calendarItem == null)
                {
                    calendarItem = CreateCalendarItem();
                    calendarItem.PropertyChanged += OnCalendarItemChanged;
                }

                return calendarItem;
            }

            set
            {
                if (calendarItem == value)
                    return;
                if (calendarItem != null)
                {
                    calendarItem.PropertyChanged -= OnCalendarItemChanged;
                }

                calendarItem = value;

                if (calendarItem != null)
                {
                    calendarItem.PropertyChanged += OnCalendarItemChanged;
                }
            }
        }

        /// <summary>
        /// Called when a property of the <see cref="CalendarItem"/> being edited has changed.
        /// This method can be overridden in derived classes to provide custom behavior when a property changes.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A <see cref="PropertyChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCalendarItemChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
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
