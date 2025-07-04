﻿using System;
using System.Collections;
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
        public SpeedEnumButton(Control parent)
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

        /// <inheritdoc/>
        protected override BaseCollection<ListControlItem> GetItems()
        {
            var collection = EnumUtils.GetEnumItemsAsListItems(
                enumType,
                GetValueAsString,
                (item) =>
                {
                    if (ExcludeValues is null)
                        return true;
                    if (ExcludeValues is IList list)
                        return !list.Contains(item);
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