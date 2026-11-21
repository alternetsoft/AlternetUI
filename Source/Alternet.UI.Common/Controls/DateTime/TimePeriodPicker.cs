using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// A control that allows users to select a time period, represented by a numeric value and a unit of
    /// measurement (e.g., days, hours, minutes).
    /// </summary>
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class TimePeriodPicker : ListPicker, IReadOnlyProperty
    {
        /// <summary>
        /// Specifies the default set of time period items to be displayed in the picker,
        /// covering a range of common durations from minutes to weeks.
        /// </summary>
        public static TimePeriod[] DefaultItems =
        [
            new (0, TimePeriodUnit.Minutes),
            new (5, TimePeriodUnit.Minutes),
            new (10, TimePeriodUnit.Minutes),
            new (15, TimePeriodUnit.Minutes),
            new (30, TimePeriodUnit.Minutes),
            new (45, TimePeriodUnit.Minutes),

            new (1, TimePeriodUnit.Hours),
            new (2, TimePeriodUnit.Hours),
            new (3, TimePeriodUnit.Hours),
            new (4, TimePeriodUnit.Hours),
            new (5, TimePeriodUnit.Hours),
            new (6, TimePeriodUnit.Hours),
            new (7, TimePeriodUnit.Hours),
            new (8, TimePeriodUnit.Hours),
            new (9, TimePeriodUnit.Hours),
            new (10, TimePeriodUnit.Hours),
            new (11, TimePeriodUnit.Hours),
            new (12, TimePeriodUnit.Hours),
            new (18, TimePeriodUnit.Hours),

            new (1, TimePeriodUnit.Days),
            new (2, TimePeriodUnit.Days),
            new (3, TimePeriodUnit.Days),
            new (4, TimePeriodUnit.Days),

            new (1, TimePeriodUnit.Weeks),
            new (2, TimePeriodUnit.Weeks),
        ];

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriodPicker"/> class.
        /// </summary>
        public TimePeriodPicker()
            : this(addDefaultItems: true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimePeriodPicker"/> class
        /// with an option to add default time period items to the picker.
        /// </summary>
        public TimePeriodPicker(bool addDefaultItems)
        {
            if (addDefaultItems)
            {
                AddDefaultItems();
            }

            Value = new TimePeriod(15, TimePeriodUnit.Minutes);
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
        /// Gets or sets the currently selected time period,
        /// represented by a numeric value and a unit of measurement.
        /// </summary>
        public new virtual TimePeriod Value
        {
            get => (TimePeriod?)SelectedItem ?? new TimePeriod(0, TimePeriodUnit.Minutes);
            set
            {
                var index = IndexOfItem(value);

                if (index is not null)
                {
                    var item = Items[index.Value] as TimePeriod;
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
        /// Gets a value indicating whether the currently selected time period is null,
        /// which can be used to determine if no specific time period has been chosen by the user.
        /// </summary>
        [Browsable(false)]
        public bool ValuePeriodIsNull => Value.IsNull;

        /// <summary>
        /// Gets a value indicating whether the currently selected time period is zero,
        /// which can be used to determine if the selected time period represents a duration of zero.
        /// </summary>
        [Browsable(false)]
        public bool ValuePeriodIsZero => Value.IsZero;

        /// <summary>
        /// Gets the index of the "None" item in the picker, if it exists.
        /// </summary>
        /// <returns>The index of the "None" item, or <c>null</c> if it does not exist.</returns>
        public virtual int? IndexOfNoneItem()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is TimePeriod timePeriod && timePeriod.Value is null)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Gets the index of the item in the picker that corresponds to the
        /// specified <see cref="TimeSpan"/>, if it exists.
        /// </summary>
        /// <param name="timeSpan">The <see cref="TimeSpan"/> to find.</param>
        /// <returns>The index of the item that corresponds to the specified <see cref="TimeSpan"/>,
        /// or <c>null</c> if it does not exist.</returns>
        public virtual int? IndexOfItem(TimeSpan timeSpan)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is TimePeriod timePeriod && timePeriod.Value is not null
                    && timePeriod.Value.Value == timeSpan)
                    return i;
            }

            return null;
        }

        /// <summary>
        /// Defines flags that control the filtering behavior of items in the <see cref="TimePeriodPicker"/>.
        /// </summary>
        [Flags]
        public enum FilterItemFlags
        {
            /// <summary>
            /// Indicates that no filtering should be applied to the items in the picker.
            /// </summary>
            All,

            /// <summary>
            /// Indicates that items with a <c>null</c> value should be ignored
            /// when filtering the items in the picker.
            /// </summary>
            IgnoreNull = 1 << 0,

            /// <summary>
            /// Indicates that items with a value of zero should be ignored
            /// when filtering the items in the picker.
            /// </summary>
            IgnoreZero = 1 << 1,

            /// <summary>
            /// Indicates that both items with a <c>null</c> value and items with
            /// a value of zero should be ignored when filtering the items in the picker.
            /// </summary>
            IgnoreNullAndZero = IgnoreNull | IgnoreZero,

            /// <summary>
            /// Indicates that items with a non-zero value should be ignored
            /// when filtering the items in the picker.
            /// </summary>
            IgnoreNonZero = 1 << 2,
        }

        /// <summary>
        /// Returns a filtered collection of <see cref="TimePeriod"/> items based
        /// on the specified <paramref name="flags"/>.
        /// </summary>
        /// <param name="flags">The flags that control the filtering behavior.</param>
        /// <returns>A collection of <see cref="TimePeriod"/> items that match the filtering criteria.</returns>
        public virtual IEnumerable<TimePeriod> GetFiltered(FilterItemFlags flags)
        {
            foreach (var item in Items)
            {
                if (item is TimePeriod timePeriod)
                {
                    if (flags.HasFlag(FilterItemFlags.IgnoreNull) && timePeriod.IsNull)
                        continue;
                    if (flags.HasFlag(FilterItemFlags.IgnoreZero) && timePeriod.IsZero)
                        continue;
                    if (flags.HasFlag(FilterItemFlags.IgnoreNonZero) && timePeriod.HasNonZeroValue)
                        continue;
                    yield return timePeriod;
                }
            }
        }

        /// <summary>
        /// Sets the text format for the items in the picker that match the specified <paramref name="flags"/>.
        /// </summary>
        /// <param name="format">The text format to set for the items.</param>
        /// <param name="flags">The flags that control which items to apply the text format to.</param>
        public virtual void SetItemTextFormat(string? format, FilterItemFlags flags = FilterItemFlags.IgnoreNullAndZero)
        {
            foreach (var item in GetFiltered(flags))
            {
                item.TextFormat = format;
            }
        }

        /// <summary>
        /// Sets the text label of the item in the picker that corresponds to the specified <see cref="TimeSpan"/>,
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="text"></param>
        public virtual bool SetItemText(TimeSpan timeSpan, string text)
        {
            return SetItemText(IndexOfItem(timeSpan), text);
        }

        /// <summary>
        /// Sets the text label of the item at the specified index in the picker, if it exists.
        /// </summary>
        /// <param name="index">The index of the item to set the text for.</param>
        /// <param name="text">The text to set for the item.</param>
        public virtual bool SetItemText(int? index, string text)
        {
            if (index is not null)
            {
                var item = Items[index.Value] as TimePeriod;
                if (item is not null)
                {
                    item.Text = text;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the index of the specified <see cref="TimePeriod"/> item in the picker, if it exists.
        /// </summary>
        /// <param name="item">The <see cref="TimePeriod"/> item to find.</param>
        /// <returns>The index of the specified item, or <c>null</c> if it does not exist.</returns>
        public virtual int? IndexOfItem(TimePeriod item)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i] is TimePeriod timePeriod && timePeriod.Equals(item))
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
                var item = Items[index.Value] as TimePeriod;
                if (item is not null)
                    Value = item;
            }
        }

        /// <summary>
        /// Selects the "None" item in the picker,
        /// allowing users to indicate that no specific time period is chosen.
        /// </summary>
        public virtual void SelectNoneItem()
        {
            SelectItemWithIndex(IndexOfNoneItem());
        }

        /// <summary>
        /// Inserts a "None" item at the beginning of the picker,
        /// allowing users to select a state where no time period is specified.
        /// </summary>
        public virtual TimePeriod AddNoneItem(string? labelText)
        {
            if (labelText is null)
                Items.Insert(0, TimePeriod.None.Clone());
            else
                Items.Insert(0, new TimePeriod(null, labelText));
            return TimePeriod.None;
        }

        /// <summary>
        /// Determines whether the default set of time period items should include an item with a value of zero.
        /// </summary>
        /// <returns></returns>
        public virtual bool AllowZeroInDefaultItems()
        {
            return true;
        }

        /// <inheritdoc/>
        public override void ShowPopup()
        {
            if (ReadOnly)
                return;
            base.ShowPopup();
        }
        
        /// <summary>
        /// Adds the default set of time period items to the picker, which can be overridden in derived classes.
        /// By default uses the <see cref="DefaultItems"/> array to populate the picker with common time periods.
        /// </summary>
        public virtual void AddDefaultItems()
        {
            var allowZero = AllowZeroInDefaultItems();

            foreach (var item in DefaultItems)
            {
                if (item.Value is null)
                    continue;

                if (!allowZero && item.Value.Value == TimeSpan.Zero)
                    continue;
                Items.Add(item.Clone());
            }
        }
    }

    /// <summary>
    /// Represents a specialized button control that allows users to select a snooze time period from
    /// a predefined list of durations.
    /// </summary>
    [ControlCategory(KnownControlCategory.DateTime)]
    public partial class SnoozeSpeedButton : TimePeriodPicker
    {
        private string? labelText;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnoozeSpeedButton"/> class.
        /// </summary>
        public SnoozeSpeedButton()
        {
            ImageVisible = false;
            PopupWindowTitle = CommonStrings.Default.WindowTitleRemindMeIn;
        }

        /// <summary>
        /// Gets or sets the text label displayed on the button, which can be customized to indicate the snooze action.
        /// If not set, a default label text is used.
        /// </summary>
        public virtual string? LabelText
        {
            get => labelText;
            set => labelText = value;
        }

        /// <inheritdoc/>
        protected override string CoerceLabelText(string text)
        {
            return labelText ?? CommonStrings.Default.ButtonSnooze;
        }

        /// <inheritdoc/>
        public override bool UseSpeedButtonTheme()
        {
            return true;
        }

        /// <inheritdoc/>
        public override bool AllowZeroInDefaultItems()
        {
            return false;
        }
    }

}
