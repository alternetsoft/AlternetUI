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
        /// <summary>
        /// Gets or sets default spacer size.
        /// Real spacer size equals to this value plus
        /// <see cref="AbstractControl.MinChildMargin"/> of the container.
        /// </summary>
        public static int DefaultSpacerSize = 2;

        private static
            EnumArray<PanelSettingsItemKind, ItemToControlDelegate?> itemToControl = new();

        private readonly Collection<PanelSettingsItem> items;

        static PanelSettings()
        {
            Fn(RegisterConversion);

            void Fn(RegisterConversionDelegate register)
            {
                T? ConvertItem<T>(PanelSettingsItem item, object? control)
                    where T : AbstractControl, new()
                {
                    var text = item.Label?.ToString() ?? string.Empty;
                    var isEmpty = string.IsNullOrEmpty(text);

                    if (control is T typedControl)
                    {
                        typedControl.Text = text;
                        typedControl.Visible = !isEmpty;
                        return typedControl;
                    }
                    else
                    {
                        if (isEmpty)
                            return null;
                        var created = new T();
                        created.Text = text;
                        return created;
                    }
                }

                register(
                    PanelSettingsItemKind.Spacer,
                    (item, control) =>
                    {
                        var spacer = ConvertItem<Spacer>(item, control);

                        if(spacer is not null)
                        {
                            spacer.SuggestedSize = DefaultSpacerSize;
                        }

                        return spacer;
                    });

                register(
                    PanelSettingsItemKind.Label,
                    (item, control) =>
                    {
                        return ConvertItem<Label>(item, control);
                    });

                register(
                    PanelSettingsItemKind.Value,
                    (item, control) =>
                    {
                        if (item.ValueType == typeof(bool))
                        {
                            var checkBox = ConvertItem<CheckBox>(item, control);

                            if(checkBox is not null)
                            {
                                if (item.Value is bool isChecked)
                                    checkBox.Checked = isChecked;

                                checkBox.CheckedChanged -= CheckBoxChecked;
                                checkBox.CheckedChanged += CheckBoxChecked;

                                void CheckBoxChecked(object? sender, EventArgs e)
                                {
                                    item.Value = checkBox.IsChecked;
                                }
                            }

                            return checkBox;
                        }

                        return null;
                    });

                register(
                    PanelSettingsItemKind.Button,
                    (item, control) =>
                    {
                        var result = ConvertItem<Button>(item, control);

                        if (result is not null)
                        {
                            result.ClickAction = () =>
                            {
                                item.ClickAction?.Invoke(item, EventArgs.Empty);
                            };
                        }

                        return result;
                    });

                register(
                    PanelSettingsItemKind.Selector,
                    (item, control) =>
                    {
                        return null;
                    });

                register(
                    PanelSettingsItemKind.EditableSelector,
                    (item, control) =>
                    {
                        return null;
                    });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelSettings"/> class.
        /// </summary>
        public PanelSettings()
        {
            items = new();
            items.ThrowOnNullAdd = true;
            items.ItemInserted += ItemInserted;
            items.ItemRemoved += ItemRemoved;
            MinChildMargin = 5;
            Layout = LayoutStyle.Vertical;
        }

        /// <summary>
        /// Encapsulates a method that is invoked when item is clicked, changed
        /// or in the similar places.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="e">Event arguments.</param>
        public delegate void ItemActionDelegate(PanelSettingsItem item, EventArgs e);

        /// <summary>
        /// Encapsulates a method that is used when item is converted to the control.
        /// </summary>
        /// <param name="item">Item for the conversion.</param>
        /// <param name="createdControl">If not Null, contains previously created control.
        /// In this case you need only to update control's properties. If passed control is not
        /// of the desired type, just create new control.</param>
        /// <returns></returns>
        public delegate object? ItemToControlDelegate(
            PanelSettingsItem item,
            object? createdControl);

        /// <summary>
        /// Encapsulates a method that is used when convertion from item
        /// to control is registered.
        /// </summary>
        /// <param name="kind">Item kind.</param>
        /// <param name="conversion">Conversion function.</param>
        /// <param name="platform">Platform kind for which registration is done.</param>
        public delegate void RegisterConversionDelegate(
            PanelSettingsItemKind kind,
            ItemToControlDelegate? conversion,
            UIPlatformKind platform = UIPlatformKind.Platformless);

        /// <summary>
        /// Gets or sets whether controls are automatically created and updated
        /// when items are changed.
        /// </summary>
        public virtual bool AutoCreate { get; set; } = false;

        /// <summary>
        /// Gets collection of the items. Each of the items defines individual
        /// setting with label, value and style options.
        /// </summary>
        internal virtual Collection<PanelSettingsItem> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Registers function which is called when item is converted to the control.
        /// </summary>
        /// <param name="platform">Platform kind.</param>
        /// <param name="kind">Item kind.</param>
        /// <param name="func">Function which is called when item
        /// is converted to the control.</param>
        public static void RegisterConversion(
            PanelSettingsItemKind kind,
            ItemToControlDelegate? func,
            UIPlatformKind platform = UIPlatformKind.Platformless)
        {
            itemToControl[kind] = func;
        }

        /// <summary>
        /// Gets registered function which is called when item is converted to the control.
        /// </summary>
        /// <param name="platform">Platform kind.</param>
        /// <param name="kind">Item kind.</param>
        public static ItemToControlDelegate? GetRegisteredConversion(
            PanelSettingsItemKind kind,
            UIPlatformKind platform = UIPlatformKind.Platformless)
        {
            return itemToControl[kind];
        }

        /// <summary>
        /// Adds horizontal line.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual void AddHorizontalLine(BaseEventArgs? e = null)
        {
            PanelSettingsItem item = new();
            item.Kind = PanelSettingsItemKind.Line;
            item.Label = "HorizontalLine";
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with an empty space.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual void AddSpacer(BaseEventArgs? e = null)
        {
            PanelSettingsItem item = new();
            item.Kind = PanelSettingsItemKind.Spacer;
            item.Label = "Spacer";
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with the generic text label.
        /// </summary>
        /// <param name="label">Text.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual void AddLabel(object? label, BaseEventArgs? e = null)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Label;
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with the button.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="clickAction">Action which is invoked when button is clicked.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual void AddButton(
            object? label,
            ItemActionDelegate? clickAction,
            BaseEventArgs? e = null)
        {
            PanelSettingsItem item = new();
            item.Label = label;
            item.Kind = PanelSettingsItemKind.Button;
            item.ClickAction = clickAction;
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with the editor for the nullable value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="pickList">Collection of possible values.</param>
        /// <param name="onChange">Action which is called when value is changed. Optional</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual void AddSelector<T>(
            object label,
            IEnumerable<T> pickList,
            IValueSource<object>? valueSource = null,
            ItemActionDelegate? onChange = null,
            BaseEventArgs? e = null)
        {
            var item = AddCore(label, valueSource, onChange);
            item.ValueType = typeof(T);
            item.Kind = PanelSettingsItemKind.Selector;
            item.IsNullable = false;
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with the editor for the not null value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="e">Additional arguments.</param>
        /// <param name="onChange">Action which is called when value is changed. Optional</param>
        /// <returns></returns>
        public virtual void AddInput<T>(
            object label,
            IValueSource<object>? valueSource = null,
            ItemActionDelegate? onChange = null,
            BaseEventArgs? e = null)
        {
            PanelSettingsItem item = AddCore(label, valueSource, onChange);
            item.ValueType = typeof(T);
            item.Kind = PanelSettingsItemKind.Value;
            item.IsNullable = false;
            Items.Add(item);
        }

        /// <summary>
        /// Adds item with the editor for the property of the specified object.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="propContainer">Object which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public void AddInput(
            object label,
            object propContainer,
            string propName,
            BaseEventArgs? e = null)
        {
            var valueSource = new PropertyValueSource(propContainer, propName);
            PanelSettingsItem item = AddCore(label, valueSource);
            item.Kind = PanelSettingsItemKind.Value;
            item.ValueType = valueSource.ValueType;
            item.IsNullable = false;
            Items.Add(item);
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
            var conversion = GetRegisteredConversion(item.Kind);
            if (conversion is null)
                return;
            var obj = conversion(item, null);
            if (obj is not AbstractControl control)
                return;
            control.Parent = this;
        }

        /// <summary>
        /// Adds item with the editor for the nullable value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text label which will be shown next to the editor.</param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="onChange">Action which is called when value is changed. Optional</param>
        /// <returns></returns>
        private void AddNullableInput<T>(
            object label,
            IValueSource<object>? valueSource = null,
            ItemActionDelegate? onChange = null)
        {
            var item = AddCore(label, valueSource, onChange);
            item.ValueType = typeof(T);
            item.Kind = PanelSettingsItemKind.Value;
            item.IsNullable = true;
            Items.Add(item);
        }

        private PanelSettingsItem AddCore(
            object label,
            IValueSource<object>? valueSource = null,
            ItemActionDelegate? onChange = null)
        {
            PanelSettingsItem item = new();
            item.Label = label;

            if (valueSource is not null)
                item.ValueSource = valueSource;

            item.ValueChangedAction = onChange;

            return item;
        }
    }
}
