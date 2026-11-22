using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that allows users to select a time zone from a list of available time zones.
    /// </summary>
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class TimeZonePicker : ListPicker, IReadOnlyProperty
    {
        private SpeedButton? clearButton;
        private SpeedButton? localTimeZoneButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZonePicker"/> class.
        /// </summary>
        public TimeZonePicker()
        {
            AddDefaultItems();
            Value = TimeZoneInfo.Local;
        }

        /// <summary>
        /// Gets the "Clear" button in the picker, which allows users to clear the value.
        /// </summary>
        public SpeedButton? ClearButton => clearButton;

        /// <summary>
        /// Gets the "Select Local Time Zone" button in the picker, which allows users to select the local time zone.
        /// </summary>
        public SpeedButton? LocalTimeZoneButton => localTimeZoneButton;

        /// <inheritdoc/>
        protected override void InitPopupWindow()
        {
            base.InitPopupWindow();

            clearButton = PopupWindow.AddClearButton(OnClearButtonClick);

            localTimeZoneButton = PopupWindow.AddCustomButton(
                        null,
                        KnownSvgImages.ImgGlobeLocation,
                        Localized.SelectLocalTimeZone,
                        OnLocalTimeZoneClick);
        }

        /// <summary>
        /// Handles the click event of the "Select Local Time Zone" button in the picker.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnLocalTimeZoneClick(object? sender, EventArgs e)
        {
            Value = TimeZoneInfo.Local;
            PopupWindow.HidePopup(ModalResult.Canceled);
        }
        
        /// <summary>
        /// Handles the click event of the "Clear" button in the picker.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnClearButtonClick(object? sender, EventArgs e)
        {
            Value = null;
            PopupWindow.HidePopup(ModalResult.Canceled);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the picker can be cleared (i.e., set to no selection).
        /// </summary>
        public virtual bool CanClear
        {
            get => clearButton?.Visible ?? false;
            set
            {
                if (value == CanClear)
                    return;
                clearButton?.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the picker is in read-only mode.
        /// </summary>
        public virtual bool ReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected <see cref="TimeZoneInfo"/> item in the picker.
        /// </summary>
        public new virtual TimeZoneInfo? Value
        {
            get => (TimeZoneInfo?)SelectedItem;
            set
            {
                var index = IndexOfItem(value);

                if (index is not null)
                {
                    var item = Items[index.Value] as TimeZoneInfo;
                    if (item is not null)
                    {
                        SelectedItem = item;
                        return;
                    }
                }

                SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the index of the specified <see cref="TimeZoneInfo"/> item in the picker,
        /// or returns <c>null</c> if the item is not found.
        /// </summary>
        /// <param name="value">The <see cref="TimeZoneInfo"/> item to find.</param>
        /// <returns>The index of the specified <see cref="TimeZoneInfo"/> item, or <c>null</c>
        /// if the item is not found.</returns>
        public virtual int? IndexOfItem(TimeZoneInfo? value)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is TimeZoneInfo timeZone && timeZone == value)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Selects the item at the specified index in the picker, if it exists.
        /// </summary>
        /// <param name="index">The index of the item to select.</param>
        public virtual void SelectItemWithIndex(int? index)
        {
            if (index is not null)
            {
                var item = Items[index.Value] as TimeZoneInfo;
                if (item is not null)
                    Value = item;
            }
        }

        /// <inheritdoc/>
        public override void ShowPopup()
        {
            if (ReadOnly)
                return;
            base.ShowPopup();
        }

        /// <summary>
        /// Adds default items to the picker.
        /// This method can be overridden in derived classes to customize the default items.
        /// </summary>
        protected virtual void AddDefaultItems()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones();
            foreach (var tz in timeZones)
            {
                Items.Add(tz);
            }
        }
    }
}
