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
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeZonePicker"/> class.
        /// </summary>
        public TimeZonePicker()
        {
            AddDefaultItems();
            Value = TimeZoneInfo.Local;
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
