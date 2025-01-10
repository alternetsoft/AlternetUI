using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Base.Collections;

namespace Alternet.UI
{
    /// <summary>
    /// Implements panel with settings. Each settings panel item is
    /// a labeled control which allows to edit individual setting.
    /// Items are declared using logical definitions (for example, boolean setting or
    /// string setting) and are not bound to the specific controls.
    /// </summary>
    public partial class PanelSettings : HiddenBorder
    {
        private static EnumArray<PanelSettingsItemKind, ItemToControlDelegate> itemToControl = new();

        private readonly Collection<PanelSettingsItem> items;

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelSettings"/> class.
        /// </summary>
        public PanelSettings()
        {
            items = new();
            items.ThrowOnNullAdd = true;
            items.ItemInserted += ItemInserted;
            items.ItemRemoved += ItemRemoved;
        }

        /// <summary>
        /// Encapsulates a method that is used when item is converted to the control.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="createdControl"></param>
        /// <returns></returns>
        public delegate object? ItemToControlDelegate(PanelSettingsItem item, object? createdControl);

        /// <summary>
        /// Gets collection of the items. Each of the items defines individual
        /// setting with label, value and style options.
        /// </summary>
        public Collection<PanelSettingsItem> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Registers function which is called when item is converted to the control.
        /// </summary>
        /// <param name="kind">Item kind.</param>
        /// <param name="func">Function which is called when item is converted to the control.</param>
        public static void RegisterItemToControl(
            PanelSettingsItemKind kind,
            ItemToControlDelegate func)
        {
            itemToControl[kind] = func;
        }

        /// <summary>
        /// Adds item with the generic text label.
        /// </summary>
        /// <param name="label">Text label.</param>
        /// <returns></returns>
        public PanelSettingsItem AddLabel(object? label)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Label;
            return item;
        }

        /// <summary>
        /// Adds item with the button.
        /// </summary>
        /// <param name="label">Text label which will be shown next to the editor.</param>
        /// <param name="clickAction">Action which is invoked when button is clicked.</param>
        /// <returns></returns>
        public PanelSettingsItem AddButton(
            object? label,
            Action<PanelSettingsItem, EventArgs> clickAction)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Button;
            item.Value = clickAction;
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the nullable value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text label which will be shown next to the editor.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public PanelSettingsItem AddNullableValue<T>(object label, T? defaultValue)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Value;
            item.ValueType = typeof(T);
            item.IsNullable = true;
            item.Value = defaultValue;
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the nullable value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text label which will be shown next to the editor.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <param name="pickList">Collection of possible values.</param>
        /// <returns></returns>
        public PanelSettingsItem AddSelector<T>(
            object label,
            T? defaultValue,
            IEnumerable<T> pickList)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Selector;
            item.ValueType = typeof(T);
            item.IsNullable = false;
            item.Value = defaultValue;
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the not null value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text label which will be shown next to the editor.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns></returns>
        public PanelSettingsItem AddValue<T>(object label, T defaultValue)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Value;
            item.ValueType = typeof(T);
            item.IsNullable = false;
            item.Value = defaultValue;
            return item;
        }

        /// <summary>
        /// Called when item is removed from the <see cref="Items"/> collection.
        /// </summary>
        protected virtual void ItemRemoved(object? sender, int index, PanelSettingsItem item)
        {
        }

        /// <summary>
        /// Called when item is added to the <see cref="Items"/> collection.
        /// </summary>
        protected virtual void ItemInserted(object? sender, int index, PanelSettingsItem item)
        {
        }
    }
}
