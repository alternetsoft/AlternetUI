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
    /// The calendar control allows the user to pick a date.
    /// </summary>
    public class Calendar : Control
    {
        /// <summary>
        /// Gets or sets the day that is considered the beginning of the week.
        /// </summary>
        /// <returns>A <see cref="DayOfWeek"/> that represents the beginning of the week.
        /// The default is <c>null</c> that is determined by the operating system.</returns>
        /// <remarks>
        /// <see cref="Calendar"/> supports only <see cref="DayOfWeek.Sunday"/>,
        /// <see cref="DayOfWeek.Monday"/> or <c>null</c> values for this property.
        /// </remarks>
        public DayOfWeek? FirstDayOfWeek
        {
            get
            {
                if (NativeControl.SundayFirst)
                    return DayOfWeek.Sunday;
                if (NativeControl.MondayFirst)
                    return DayOfWeek.Monday;
                return null;
            }

            set
            {
                if (FirstDayOfWeek == value)
                    return;

                if (value == DayOfWeek.Sunday)
                {
                    NativeControl.SundayFirst = true;
                    NativeControl.MondayFirst = false;
                }
                else
                if (value == DayOfWeek.Monday)
                {
                    NativeControl.SundayFirst = false;
                    NativeControl.MondayFirst = true;
                }
                else
                {
                    NativeControl.SundayFirst = false;
                    NativeControl.MondayFirst = false;
                }
            }
        }

        [Browsable(false)]
        internal new NativeCalendarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeCalendarHandler)base.Handler;
            }
        }

        internal Native.Calendar NativeControl => Handler.NativeControl;

        internal DayOfWeek FirstDayOfWeekUseGlobalization =>
            System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeCalendarHandler();
        }
    }
}


/*

The user can move the current selection using the keyboard and select the date
    by pressing <Return> or double clicking it.

Generic calendar has advanced possibilities for the customization of its display,
described below. If you want to use these possibilities on every platform,
set UseGeneric property.

All global settings (such as colours and fonts used) can, of course, be changed.
    But also, the display style for each day in the month can be set independently.

An item without custom attributes is drawn with the default colours and font and without
    border, but setting custom attributes with SetAttr() allows
    modifying its appearance. Just create a custom attribute object and
    set it for the day you want to be displayed specially (note that the control
    will take ownership of the pointer, i.e. it will delete it itself).

    A day may be marked as being a holiday, even if it is not recognized
    as one by wxDateTime using the wxCalendarDateAttr::SetHoliday() method.

As the attributes are specified for each day, they may change when the month is changed,
so you will often want to update them in EVT_CALENDAR_PAGE_CHANGED event handler.

If neither the wxCAL_SUNDAY_FIRST or wxCAL_MONDAY_FIRST style is given,
the first day of the week is determined from operating system's settings,
if possible. The native wxGTK calendar chooses the first weekday based on
locale, and these styles have no effect on it.

*/