using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// A picker for <see cref="CalendarItem"/> category.
    /// It allows to select a <see cref="CalendarItemCategory"/> value from the list of known categories
    /// which are listed in <see cref="KnownCalendarItemMarkers.Categories"/>.
    /// </summary>
    public partial class CalendarItemCategoryPicker : DrawingResourcePicker
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
        /// Initializes a new instance of the <see cref="CalendarItemCategoryPicker"/> class.
        /// </summary>
        public CalendarItemCategoryPicker()
        {
            foreach (var item in KnownCalendarItemMarkers.Categories)
            {
                Add(item);
            }

            if (DefaultGearButtonVisible)
                RequireGearButton();
        }

        /// <summary>
        /// Gets or sets the selected <see cref="CalendarItemCategory"/> value.
        /// </summary>
        public new virtual CalendarItemCategory? Value
        {
            get => base.Value as CalendarItemCategory;
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
