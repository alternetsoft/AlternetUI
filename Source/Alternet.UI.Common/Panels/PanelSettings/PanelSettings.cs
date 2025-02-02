using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Base.Collections;
using Alternet.Drawing;

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

        /// <summary>
        /// Gets or sets default distance between label and text.
        /// </summary>
        public static int DefaultLabelToTextMargin = 3;

        private static
            EnumArray<PanelSettingsItemKind, ItemToControlDelegate?> itemToControl = new();

        private readonly Collection<PanelSettingsItem> items;

        private bool autoCreate = true;

        static PanelSettings()
        {
            Fn(RegisterConversion);

            void Fn(RegisterConversionDelegate register)
            {
                /* registration call */

                register(
                    PanelSettingsItemKind.Spacer,
                    (item, control) =>
                    {
                        var spacer = CreateOrUpdateControl<Spacer>(item, control);
                        spacer.SuggestedSize = DefaultSpacerSize;
                        return spacer;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.Label,
                    (item, control) =>
                    {
                        var result = CreateOrUpdateControl<GenericLabel>(item, control);
                        result.HorizontalAlignment = HorizontalAlignment.Left;
                        UpdateText(item, result);
                        return result;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.LinkLabel,
                    (item, control) =>
                    {
                        var result = CreateOrUpdateControl<GenericLabel>(item, control);
                        UpdateText(item, result);

                        result.MakeAsLinkLabel();

                        result.HorizontalAlignment = HorizontalAlignment.Left;
                        result.Click -= LinkLabelClicked;
                        result.Click += LinkLabelClicked;

                        void LinkLabelClicked(object? sender, EventArgs e)
                        {
                            result.RunWhenIdle(() =>
                            {
                                item.ClickAction?.Invoke(item, EventArgs.Empty);
                            });
                        }

                        return result;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.Value,
                    (item, control) =>
                    {
                        if (item.ValueType == typeof(bool))
                        {
                            return CreateOrUpdateCheckBox(item, control);
                        }

                        if (item.ValueType == typeof(Color))
                        {
                            return CreateOrUpdateColorComboBox(item, control);
                        }

                        var result = CreateOrUpdateInput(item, control);
                        return result;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.Button,
                    (item, control) =>
                    {
                        var result = CreateOrUpdateControl<Button>(item, control);
                        UpdateText(item, result);

                        result.ClickAction = () =>
                        {
                            result.RunWhenIdle(() =>
                            {
                                item.ClickAction?.Invoke(item, EventArgs.Empty);
                            });
                        };

                        return result;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.Selector,
                    (item, control) =>
                    {
                        var result = CreateOrUpdateSelector(item, control, false);
                        return result;
                    });

                /* registration call */

                register(
                    PanelSettingsItemKind.EditableSelector,
                    (item, control) =>
                    {
                        var result = CreateOrUpdateSelector(item, control, true);
                        return result;
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
        public virtual bool AutoCreate
        {
            get => autoCreate;

            set
            {
                autoCreate = value;
            }
        }

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
        /// Adds item with an empty space.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddSpacer(CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore("Spacer", PanelSettingsItemKind.Spacer, null, e);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the generic text label.
        /// </summary>
        /// <param name="label">Text.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddLabel(object label, CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore(label, PanelSettingsItemKind.Label, null, e);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the link label.
        /// </summary>
        /// <param name="clickAction">Action which is invoked when link label is clicked.</param>
        /// <param name="label">Text.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddLinkLabel(
            object label,
            ItemActionDelegate? clickAction,
            CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore(label, PanelSettingsItemKind.LinkLabel, null, e);
            item.ClickAction = clickAction;
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the link label.
        /// </summary>
        /// <param name="clickAction">Action which is invoked when link label is clicked.</param>
        /// <param name="label">Text.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddLinkLabel(
            object label,
            Action? clickAction,
            CustomEventArgs? e = null)
        {
            return AddLinkLabel(
            label,
            (item, e) =>
            {
                clickAction?.Invoke();
            },
            e);
        }

        /// <summary>
        /// Adds item with the button.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="clickAction">Action which is invoked when button is clicked.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddButton(
            object label,
            Action? clickAction,
            CustomEventArgs? e = null)
        {
            return AddButton(
            label,
            (item, e) =>
            {
                clickAction?.Invoke();
            },
            e);
        }

        /// <summary>
        /// Adds item with the button.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="clickAction">Action which is invoked when button is clicked.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddButton(
            object label,
            ItemActionDelegate? clickAction,
            CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore(label, PanelSettingsItemKind.Button, null, e);
            item.ClickAction = clickAction;
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the nullable value of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="pickList">Collection of possible values.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddSelector<T>(
            object label,
            IEnumerable<T> pickList,
            IValueSource<object>? valueSource = null,
            CustomEventArgs? e = null)
        {
            var item
                = CreateItemCore(label, PanelSettingsItemKind.Selector, valueSource, e);
            item.ValueType = typeof(T);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the value of the specified type.
        /// Value is specified using <see cref="IValueSource{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddInput<T>(
            object label,
            IValueSource<object>? valueSource = null,
            CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore(label, PanelSettingsItemKind.Value, valueSource, e);
            item.ValueType = typeof(T);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the value of the specified type.
        /// Value is specified using getter and setter delegates.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="getValue"></param>
        /// <param name="setValue"></param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddInput<T>(
            object label,
            Func<T> getValue,
            Action<T> setValue,
            CustomEventArgs? e = null)
        {
            var valueSource = new DelegatesValueSource<T>(getValue, setValue);
            PanelSettingsItem item
                = CreateItemCore(label, PanelSettingsItemKind.Value, valueSource, e);
            item.ValueType = typeof(T);
            Items.Add(item);
            return item;
        }

        /// <summary>
        /// Adds item with the editor for the property of the specified object.
        /// Value is specified using property name and property container.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="propContainer">Object which contains the property.</param>
        /// <param name="propName">Property name.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddInput(
            object label,
            object propContainer,
            string propName,
            CustomEventArgs? e = null)
        {
            var valueSource = new PropertyValueSource(propContainer, propName);

            var realType = AssemblyUtils.GetRealType(valueSource.ValueType);
            var isEnum = realType.IsEnum;

            PanelSettingsItem item;

            if (isEnum)
            {
                var propInfo = valueSource.PropInfo;

                var prm = PropertyGrid.ConstructNewItemParams(propContainer, propInfo);
                bool isFlags;
                if (prm.EnumIsFlags is null)
                    isFlags = AssemblyUtils.EnumIsFlags(realType);
                else
                    isFlags = prm.EnumIsFlags.Value;

                if (isFlags)
                {
                    App.LogError("PanelSettings.AddInput: Enum with [Flags] is not supported");
                    item = new PanelSettingsItem();
                }
                else
                {
                    var pickList = Enum.GetValues(realType);
                    item = CreateItemCore(label, PanelSettingsItemKind.Selector, valueSource, e);
                    List<object> list = new(pickList.Length);
                    foreach (var element in pickList)
                        list.Add(element);
                    item.PickList = list;
                }
            }
            else
            {
                item = CreateItemCore(label, PanelSettingsItemKind.Value, valueSource, e);
            }

            item.ValueType = valueSource.ValueType;
            Items.Add(item);
            return item;
        }

        internal static void UpdateTitle(PanelSettingsItem item, AbstractControl control)
        {
            var text = item.Label?.ToString() ?? string.Empty;
            control.Title = text;
        }

        /// <summary>
        /// Adds horizontal line.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        internal virtual PanelSettingsItem AddHorizontalLine(CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore("HorizontalLine", PanelSettingsItemKind.Line, null, e);
            Items.Add(item);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates item with the specified parameters.
        /// </summary>
        /// <param name="label">Text which will be shown next to the editor.</param>
        /// <param name="kind"></param>
        /// <param name="valueSource">Source of the value. If Null an internal
        /// value container is used.</param>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        protected virtual PanelSettingsItem CreateItemCore(
            object label,
            PanelSettingsItemKind kind,
            IValueSource<object>? valueSource,
            CustomEventArgs? e)
        {
            PanelSettingsItem item = new();
            item.CreateArg = e;
            item.Kind = kind;
            item.Label = label;
            if (valueSource is not null)
                item.ValueSource = valueSource;
            return item;
        }

        /// <summary>
        /// Called when item is removed from the <see cref="Items"/> collection.
        /// </summary>
        protected virtual void ItemRemoved(object? sender, int index, PanelSettingsItem item)
        {
            if (!AutoCreate)
                return;
        }

        /// <summary>
        /// Called when item is added to the <see cref="Items"/> collection.
        /// </summary>
        protected virtual void ItemInserted(object? sender, int index, PanelSettingsItem item)
        {
            if (!AutoCreate)
                return;
            var conversion = GetRegisteredConversion(item.Kind);
            if (conversion is null)
                return;
            var obj = conversion(item, null);
            if (obj is not AbstractControl control)
                return;
            control.Parent = this;
        }

        private static object? CreateOrUpdateCheckBox(
            PanelSettingsItem item,
            object? control)
        {
            var checkBox = CreateOrUpdateControl<CheckBox>(item, control);
            UpdateText(item, checkBox);

            if (item.Value is bool isChecked)
                checkBox.Checked = isChecked;

            checkBox.CheckedChanged -= CheckBoxChecked;
            checkBox.CheckedChanged += CheckBoxChecked;

            void CheckBoxChecked(object? sender, EventArgs e)
            {
                item.Value = checkBox.IsChecked;
            }

            return checkBox;
        }

        private static object? CreateOrUpdateColorComboBox(
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControl<ControlAndLabel<ColorComboBox, Label>>(item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(item, result.Label);

            var colorEditor = result.MainControl;

            if (item.Value is not null)
                colorEditor.Value = (Color)item.Value;

            colorEditor.SelectedIndexChanged -= SelectorChaned;
            colorEditor.SelectedIndexChanged += SelectorChaned;

            void SelectorChaned(object? sender, EventArgs e)
            {
                item.Value = colorEditor.Value;
            }

            return result;
        }

        private static object? CreateOrUpdateInput(
            PanelSettingsItem item,
            object? control)
        {
            var result
                = CreateOrUpdateControl<ControlAndLabel<TextBoxAndButton, Label>>(item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(item, result.Label);

            var textBox = result.MainControl;
            textBox.HasBtnComboBox = false;

            textBox.TextBox.SetValidator(item.ValueType, false);
            textBox.TextBox.AutoShowError = true;
            textBox.TextBox.Options |= TextBoxOptions.DefaultValidation;
            textBox.TextBox.TextAsValue = item.Value;
            textBox.TextBox.IsRequired = GetFlagIsRequired(item.CreateArg);

            textBox.DelayedTextChanged -= TextChanged;
            textBox.DelayedTextChanged += TextChanged;

            void TextChanged(object? sender, EventArgs e)
            {
                item.Value = textBox.TextBox.TextAsValue;
            }

            return result;
        }

        private static object? CreateOrUpdateSelector(
            PanelSettingsItem item,
            object? control,
            bool isEditable)
        {
            var result
                = CreateOrUpdateControl<ControlAndLabel<ComboBoxAndButton, Label>>(item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(item, result.Label);

            var comboBox = result.MainControl;

            comboBox.IsEditable = isEditable;

            if (comboBox.Items.Count == 0 && item.PickList is not null)
            {
                comboBox.DoInsideUpdate(() =>
                {
                    comboBox.Items.AddRange(item.PickList);
                });
            }

            comboBox.SelectedItem = item.Value;

            comboBox.DelayedTextChanged -= SelectorChaned;
            comboBox.DelayedTextChanged += SelectorChaned;

            void SelectorChaned(object? sender, EventArgs e)
            {
                item.Value = comboBox.SelectedItem;
            }

            return result;
        }

        private static void UpdateCommonProps(PanelSettingsItem item, AbstractControl control)
        {
            control.Visible = item.IsVisible;
            control.Enabled = item.IsEnabled;
        }

        private static void UpdateText(PanelSettingsItem item, AbstractControl control)
        {
            var text = item.Label?.ToString() ?? string.Empty;
            control.Text = text;
        }

        private static T CreateOrUpdateControl<T>(PanelSettingsItem item, object? control)
            where T : AbstractControl, new()
        {
            T? typedControl = control as T ?? new T();
            UpdateCommonProps(item, typedControl);
            return typedControl;
        }

        private static bool GetFlagIsRequired(CustomEventArgs? e)
        {
            if (e is null)
                return false;
            return e.CustomFlags["IsRequired"];
        }
    }
}
