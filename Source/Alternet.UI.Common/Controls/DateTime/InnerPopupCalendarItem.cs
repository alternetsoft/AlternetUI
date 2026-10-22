using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a popup control that hosts a <see cref="CalendarItemPicker"/> control.
    /// </summary>
    public partial class InnerPopupCalendarItem : ResizablePopupControl<ScrollableCalendarItemPicker>
    {
        private static InnerPopupCalendarItem? instance;

        /// <summary>
        /// Gets the singleton instance of the <see cref="InnerPopupCalendarItem"/> class.
        /// If the instance does not exist, it is created and returned.
        /// Instance created only when it is first accessed, and subsequent accesses return the same instance.
        /// </summary>
        public static InnerPopupCalendarItem Instance
        {
            get
            {
                return instance ??= new();
            }

            set
            {
                instance = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupCalendarItem"/> class.
        /// </summary>
        /// <param name="createFlags">The create flags.</param>
        public InnerPopupCalendarItem(CreateFlags createFlags = CreateFlags.ResizableAndScrollable)
            : base(createFlags)
        {
            MinimumSize = new (300, 200);
            HideOnEscape = true;
            FitIntoParent = true;
            HideOnEnter = true;
        }
    }
}
