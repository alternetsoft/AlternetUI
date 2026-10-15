using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to edit <see cref="CalendarItem"/>
    /// properties in a user-friendly manner.
    /// </summary>
    [ControlCategory(KnownControlCategory.Other)]
    public partial class CalendarItemPicker : TransparentPanel
    {
        private readonly Label titleLabel = new();
        private readonly TextPicker titlePicker = new ();
        
        private readonly Label descriptionLabel = new();
        private readonly MultilineTextPicker descriptionPicker = new();

        private readonly Label locationLabel = new();
        private readonly TextPicker locationPicker = new();

        private readonly Label categoryLabel = new();
        private readonly DrawingResourcePicker categoryPicker = new();

        private readonly Label statusLabel = new();
        private readonly DrawingResourcePicker statusPicker = new();

        private CalendarItem? calendarItem;

        /*
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
            MinChildMargin = 5;
            Layout = LayoutStyle.Vertical;

            titleLabel.Text = CommonStrings.Default.CalendarItemLabelTitle + ":";
            descriptionLabel.Text = CommonStrings.Default.CalendarItemLabelDescription + ":";
            locationLabel.Text = CommonStrings.Default.CalendarItemLabelLocation + ":";
            categoryLabel.Text = CommonStrings.Default.CalendarItemLabelCategory + ":";
            statusLabel.Text = CommonStrings.Default.CalendarItemLabelStatus + ":";

            foreach (var item in KnownCalendarItemMarkers.Categories)
            {
                categoryPicker.Add(item);
            }

            foreach (var item in KnownCalendarItemMarkers.Statuses)
            {
                statusPicker.Add(item);
            }

            titleLabel.Parent = this;
            titlePicker.Parent = this;

            categoryLabel.Parent = this;
            categoryPicker.Parent = this;

            statusLabel.Parent = this;
            statusPicker.Parent = this;

            locationLabel.Parent = this;
            locationPicker.Parent = this;

            descriptionLabel.Parent = this;
            descriptionPicker.Parent = this;
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
                value ??= CreateCalendarItem();

                if (calendarItem != null)
                {
                    calendarItem.PropertyChanged -= OnCalendarItemChanged;
                }

                calendarItem = value;
                calendarItem.PropertyChanged += OnCalendarItemChanged;
                ValueToControls();
            }
        }

        /// <summary>
        /// Updates the controls of the picker to reflect the current values
        /// of the <see cref="CalendarItem"/> being edited.
        /// </summary>
        public virtual void ValueToControls()
        {
            titlePicker.Text = Value.Title ?? string.Empty;
            descriptionPicker.Text = Value.Description ?? string.Empty;
            locationPicker.Text = Value.Location?.ToString() ?? string.Empty;
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
