using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// A picker for <see cref="CalendarItem"/> status.
    /// It allows to select a <see cref="CalendarItemStatus"/> value from the list of known statuses
    /// which are listed in <see cref="KnownCalendarItemMarkers.Statuses"/>.
    /// </summary>
    public partial class CalendarItemStatusPicker : DrawingResourcePicker
    {
        /// <summary>
        /// Occurs when the gear button of the picker is clicked.
        /// </summary>
        public static event EventHandler<HandledEventArgs>? DefaultGearButtonClick;

        /// <summary>
        /// Gets or sets a value indicating whether the gear button is visible by default.
        /// </summary>
        public static bool DefaultGearButtonVisible { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarItemStatusPicker"/> class.
        /// </summary>
        public CalendarItemStatusPicker()
        {
            foreach (var item in KnownCalendarItemMarkers.Statuses)
            {
                Add(item);
            }

            if (DefaultGearButtonVisible)
                RequireGearButton();
        }

        /// <summary>
        /// Gets or sets the selected <see cref="CalendarItemStatus"/> value.
        /// </summary>
        public new virtual CalendarItemStatus? Value
        {
            get => base.Value as CalendarItemStatus;
            set
            {
                base.Value = value;
            }
        }

        /// <inheritdoc/>
        protected override void OnGearButtonClick(object? sender, EventArgs e)
        {
            var handledEventArgs = new HandledEventArgs();

            DefaultGearButtonClick?.Invoke(sender, handledEventArgs);

            if (handledEventArgs.Handled)
                return;

            base.OnGearButtonClick(sender, handledEventArgs);
        }
    }
}
