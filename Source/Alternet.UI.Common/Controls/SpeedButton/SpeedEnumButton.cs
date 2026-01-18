using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using Alternet.Drawing;
using Alternet.UI.Extensions;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements <see cref="SpeedButton"/> for selecting of the enum values.
    /// </summary>
    public partial class SpeedEnumButton : SpeedButtonWithListPopup
    {
        /// <summary>
        /// Gets or sets the default kind of popup window used by the control.
        /// </summary>
        public static new PickerPopupKind DefaultPopupKind = PickerPopupKind.ContextMenu;

        private Type? enumType;
        private IEnumerable? excludeValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedEnumButton"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public SpeedEnumButton(AbstractControl parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedEnumButton"/> class.
        /// </summary>
        public SpeedEnumButton()
        {
            PopupWindowTitle = CommonStrings.Default.WindowTitleSelectValue;
            PopupKind = DefaultPopupKind;
        }

        /// <summary>
        /// Gets or sets the type of the enumeration which values are shown in the popup.
        /// </summary>
        [Browsable(false)]
        public virtual Type? EnumType
        {
            get
            {
                return enumType;
            }

            set
            {
                if (enumType == value)
                    return;
                enumType = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// Gets or sets the values to be excluded from the popup list.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable? ExcludeValues
        {
            get => excludeValues;

            set
            {
                if (excludeValues == value)
                    return;
                excludeValues = value;
                ReloadItems();
            }
        }

        /// <summary>
        /// Gets or sets a predicate used to determine whether a given enum element
        /// should be included in the popup list.
        /// </summary>
        /// <remarks>
        /// The predicate receives an item as input (which may be <c>null</c>) and returns <c>true</c>
        /// if the item should be included, or <c>false</c> otherwise.
        /// </remarks>
        /// <value>
        /// A delegate of type <see cref="Predicate{Object}"/> that returns <c>true</c>
        /// for items to include;
        /// otherwise, <c>false</c>. May be <c>null</c> to disable filtering.
        /// </value>
        [Browsable(false)]
        public virtual Predicate<object?>? IncludeValuePredicate { get; set; }

        /// <summary>
        /// Sets display text of the enum item.
        /// </summary>
        /// <param name="value">The value of the enum item.</param>
        /// <param name="text">The display text to assign.</param>
        /// <returns></returns>
        public virtual bool SetDisplayText(object value, string text)
        {
            var item = FindItemWithValue(value);
            if (item is not null)
            {
                item.DisplayText = text;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the value and optionally updates the enumeration type.
        /// </summary>
        /// <param name="value">The new value to be assigned.</param>
        /// <param name="setEnumType">A boolean indicating whether to update the enumeration
        /// type based on the provided value. <see langword="true"/> to update
        /// the enumeration type; otherwise, <see langword="false"/>.</param>
        public virtual void SetValue(object value, bool setEnumType = true)
        {
            if (setEnumType)
                EnumType = value?.GetType();
            Value = value;
        }

        /// <inheritdoc/>
        protected override BaseCollection<ListControlItem> GetItems()
        {
            if(enumType is null)
                return new BaseCollection<ListControlItem>();

            var collection = EnumUtils.GetEnumItemsAsListItems(
                enumType,
                GetValueAsString,
                (item) =>
                {
                    if(IncludeValuePredicate is not null)
                    {
                        var isIncluded = IncludeValuePredicate(item);
                        if (!isIncluded)
                            return false;
                    }

                    if (ExcludeValues is null)
                        return true;
                    foreach (var excludeValue in ExcludeValues)
                    {
                        if (excludeValue?.Equals(item) == true)
                            return false;
                    }

                    return true;
                });
            return collection ?? new();
        }
    }
}