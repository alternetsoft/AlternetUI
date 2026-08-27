using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        /// Gets or sets default minimum margin between child controls in this panel.
        /// </summary>
        public static Thickness DefaultMinChildMargin = (5, 1, 5, 1);

        /// <summary>
        /// Gets or sets default horizontal line margin.
        /// </summary>
        public static Thickness DefaultHorizontalLineMargin = (5, 10, 5, 10);

        /// <summary>
        /// Gets or sets default margin for the check image of <see cref="XCheckBox"/> control in this panel.
        /// </summary>
        public static Thickness DefaultCheckImageMargin = 0;

        /// <summary>
        /// Gets or sets a default dictionary of type converters for the settings panel items.
        /// The type converter is used to convert the item value to and from string representation.
        /// </summary>
        public static BaseDictionary<Type, Type> DefaultTypeConverters = new();

        /// <summary>
        /// Gets or sets a value indicating whether the combo box mouse
        /// wheel is allowed by default when it is used in the <see cref="PanelSettings"/>.
        /// </summary>
        public static bool DefaultAllowComboBoxMouseWheel = false;

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

        private static long globalCounter;
        private static EnumArray<PanelSettingsItemKind, ItemToControlDelegate?> itemToControl = new();

        private readonly BaseCollection<PanelSettingsItem> items;
        private readonly bool autoCreate = true;
        private BaseConcurrentStack<ObjectUniqueId>? radioGroupStack;

        static PanelSettings()
        {
            RegisterDefaultConversions(RegisterConversion);

            DefaultTypeConverters.Add(typeof(float), typeof(SingleConverter));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PanelSettings"/> class.
        /// </summary>
        public PanelSettings()
        {
            items = new(CollectionSecurityFlags.NoNullOrReplace);
            items.ItemInserted += ItemInserted;
            items.ItemRemoved += ItemRemoved;
            MinChildMargin = DefaultMinChildMargin;
            Layout = LayoutStyle.Vertical;
            UserPaint = true;
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
        /// <param name="sender">Sender of the event.</param>
        /// <param name="createdControl">If not Null, contains previously created control.
        /// In this case you need only to update control's properties. If passed control is not
        /// of the desired type, just create new control.</param>
        /// <returns>The created or updated control.</returns>
        public delegate object? ItemToControlDelegate(
            PanelSettings sender,
            PanelSettingsItem item,
            object? createdControl);

        /// <summary>
        /// Encapsulates a method that is used when conversion from item
        /// to control is registered.
        /// </summary>
        /// <param name="kind">Item kind.</param>
        /// <param name="conversion">Conversion function.</param>
        /// <param name="platform">Platform kind for which registration is done.</param>
        public delegate void RegisterConversionDelegate(
            PanelSettingsItemKind kind,
            ItemToControlDelegate? conversion,
            UIPlatformKind platform = UIPlatformKind.Unspecified);

        /// <summary>
        /// Gets whether controls are automatically created and updated
        /// when items are changed.
        /// </summary>
        public virtual bool AutoCreate
        {
            get => autoCreate;
        }

        /// <summary>
        /// Gets or sets a dictionary of type converters for the settings panel items.
        /// The type converter is used to convert the item value to and from string representation.
        /// </summary>
        public virtual BaseDictionary<Type, Type>? TypeConverters { get; set; }

        /// <summary>
        /// Gets collection of the items. Each of the items defines individual
        /// setting with label, value and style options.
        /// </summary>
        public virtual BaseCollection<PanelSettingsItem> Items
        {
            get
            {
                return items;
            }
        }

        /// <summary>
        /// Registers default conversions in the specified register.
        /// </summary>
        /// <param name="register">The delegate to call for the registration.</param>
        public static void RegisterDefaultConversions(RegisterConversionDelegate register)
        {
            register(PanelSettingsItemKind.Line, DefaultItemToLineControl);
            register(PanelSettingsItemKind.Spacer, DefaultItemToSpacerControl);
            register(PanelSettingsItemKind.Label, DefaultItemToLabelControl);
            register(PanelSettingsItemKind.LinkLabel, DefaultItemToLinkLabelControl);
            register(PanelSettingsItemKind.Enum, DefaultItemToEnumControl);
            register(PanelSettingsItemKind.Value, DefaultItemToValueControl);
            register(PanelSettingsItemKind.Button, DefaultItemToButtonControl);
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Line"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        public static object? DefaultItemToLineControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var spacer = CreateOrUpdateControl<HorizontalLine>(sender, item, control);
            spacer.Margin = DefaultHorizontalLineMargin;
            return spacer;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Spacer"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        public static object? DefaultItemToSpacerControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var spacer = CreateOrUpdateControl<Spacer>(sender, item, control);

            SizeD spacerSize = DefaultSpacerSize;

            var minHeight = item.CreateArg?.CustomAttr["MinHeight"] as float?;
            if (minHeight.HasValue && minHeight.Value > spacerSize.Height)
            {
                spacerSize.Height = minHeight.Value;
            }

            spacer.SuggestedSize = spacerSize;

            return spacer;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Label"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        public static object? DefaultItemToLabelControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var result = CreateOrUpdateControl<Label>(sender, item, control);
            result.HorizontalAlignment = HorizontalAlignment.Left;
            UpdateText(sender, item, result);
            return result;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.LinkLabel"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        public static object? DefaultItemToLinkLabelControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var result = CreateOrUpdateControl<LinkLabel>(sender, item, control);
            UpdateText(sender, item, result);

            result.HorizontalAlignment = HorizontalAlignment.Left;
            result.LinkClicked -= LinkLabelClicked;
            result.LinkClicked += LinkLabelClicked;

            void LinkLabelClicked(object? sender, CancelEventArgs e)
            {
                e.Cancel = true;
                Post(() =>
                {
                    item.ClickAction?.Invoke(item, EventArgs.Empty);
                });
            }

            return result;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Enum"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? DefaultItemToEnumControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var result = CreateOrUpdateEnumEdit(sender, item, control);
            return result;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Value"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? DefaultItemToValueControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            if (item.ValueType is null)
            {
                return CreateOrUpdateTextBox(sender, item, control);
            }

            Type realType = AssemblyUtils.GetRealType(item.ValueType);
            var typeCode = Type.GetTypeCode(realType);
            var isIntNumber = AssemblyUtils.IsTypeCodeSignedInt(typeCode) || AssemblyUtils.IsTypeCodeUnsignedInt(typeCode);

            if (isIntNumber)
            {
                var useUpDown = item.CreateArg?.CustomFlags["UseUpDown"] ?? false;

                if (useUpDown)
                {
                    return CreateOrUpdateIntPicker(sender, item, control);
                }
                else
                {
                    return CreateOrUpdateTextBox(sender, item, control);
                }
            }

            if (realType == typeof(bool))
            {
                return CreateOrUpdateCheckBox(sender, item, control);
            }

            if (realType == typeof(Color))
            {
                return CreateOrUpdateColorEdit(sender, item, control);
            }

            if (realType == typeof(TimeOnly))
            {
                return CreateOrUpdateTimeEdit(sender, item, control);
            }

            if (realType == typeof(DateOnly))
            {
                return CreateOrUpdateDateEdit(sender, item, control);
            }

            if (realType == typeof(DateTime))
            {
                return CreateOrUpdateDateTimeEdit(sender, item, control);
            }

            var result = CreateOrUpdateTextBox(sender, item, control);
            return result;
        }

        /// <summary>
        /// Default conversion method from <see cref="PanelSettingsItemKind.Button"/> item
        /// to the appropriate control.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? DefaultItemToButtonControl(PanelSettings sender, PanelSettingsItem item, object? control)
        {
            var result = CreateOrUpdateControl<XButton>(sender, item, control);
            UpdateText(sender, item, result);

            result.ClickAction = () =>
            {
                result.RunWhenIdle(() =>
                {
                    item.ClickAction?.Invoke(item, EventArgs.Empty);
                });
            };

            return result;
        }

        /// <summary>
        /// Creates or updates a checkbox control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateCheckBox(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var args = item.CreateArg;
            var isRadioButton = args is not null && args.CustomFlags["IsRadioButton"];

            XCheckBox checkBox;

            if (isRadioButton)
            {
                checkBox = CreateOrUpdateControl<XRadioButton>(sender, item, control);
            }
            else
            {
                checkBox = CreateOrUpdateControl<XCheckBox>(sender, item, control);
            }

            checkBox.Item.CheckBoxMargin = DefaultCheckImageMargin;

            UpdateText(sender, item, checkBox);

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

        /// <summary>
        /// Creates or updates a color edit control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateColorEdit(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControlAndLabel<ColorPickerAndButton>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var colorEditor = result.MainControl;

            colorEditor.HasBtnComboBox = false;
            colorEditor.Buttons.Visible = false;

            var args = item.CreateArg;
            var hasEmptyColor = args is not null && args.CustomFlags["HasEmptyColor"];
            var hasTransparentColor = args is not null && args.CustomFlags["HasTransparentColor"];

            if (hasEmptyColor)
            {
                colorEditor.MainControl.ListBox.AddEmptyColor();
            }

            if (hasTransparentColor)
            {
                colorEditor.MainControl.ListBox.AddTransparentColor();
            }

            colorEditor.ButtonClick -= ButtonClick;
            colorEditor.ButtonClick += ButtonClick;

            if (item.Value is Color colorValue)
                colorEditor.ColorPicker.Value = colorValue;

            colorEditor.ColorPicker.ValueChanged -= ValueChanged;
            colorEditor.ColorPicker.ValueChanged += ValueChanged;

            void ButtonClick(object? sender, ControlAndButtonClickEventArgs e)
            {
                colorEditor.ColorPicker.ShowColorPopup();
            }

            void ValueChanged(object? sender, EventArgs e)
            {
                item.Value = colorEditor.ColorPicker.Value;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates an enum edit control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateEnumEdit(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControlAndLabel<EnumPickerAndButton>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            result.MainControl.HasBtnComboBox = false;
            result.MainControl.Buttons.Visible = false;
            UpdateText(sender, item, result.Label);

            var enumEditor = result.MainControl;
            enumEditor.ButtonClick -= ButtonClick;
            enumEditor.ButtonClick += ButtonClick;

            if (item.ValueType is not null)
               enumEditor.EnumPicker.EnumType = AssemblyUtils.GetRealType(item.ValueType);

            if (item.Value is not null)
                enumEditor.EnumPicker.Value = item.Value;

            enumEditor.EnumPicker.ValueChanged -= ValueChanged;
            enumEditor.EnumPicker.ValueChanged += ValueChanged;

            void ButtonClick(object? sender, ControlAndButtonClickEventArgs e)
            {
                enumEditor.EnumPicker.ShowPopup();
            }

            void ValueChanged(object? sender, EventArgs e)
            {
                item.Value = enumEditor.EnumPicker.Value;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates a time edit control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateTimeEdit(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControlAndLabel<TimePicker>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var timeEditor = result.MainControl;

            if (item.Value is DateTime dateTimeValue)
                timeEditor.Value = dateTimeValue;
            else
                if (item.Value is TimeOnly timeOnlyValue)
                    timeEditor.AsTimeOnly = timeOnlyValue;

            timeEditor.ValueChanged -= ValueChanged;
            timeEditor.ValueChanged += ValueChanged;

            void ValueChanged(object? sender, EventArgs e)
            {
                item.Value = timeEditor.AsTimeOnly;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates an integer picker control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateIntPicker(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControlAndLabel<XIntPicker>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var intPicker = result.MainControl;

            if (item.Value is int intValue)
                intPicker.Value = intValue;

            intPicker.ValueChanged -= SelectorChanged;
            intPicker.ValueChanged += SelectorChanged;

            void SelectorChanged(object? sender, EventArgs e)
            {
                item.Value = intPicker.Value;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates a date edit control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateDateEdit(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var result = CreateOrUpdateControlAndLabel<DatePicker>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var dateEditor = result.MainControl;

            if (item.Value is DateTime dateTimeValue)
                dateEditor.Value = dateTimeValue;
            else
                if (item.Value is DateOnly dateOnlyValue)
                    dateEditor.AsDateOnly = dateOnlyValue;

            dateEditor.ValueChanged -= ValueChanged;
            dateEditor.ValueChanged += ValueChanged;

            void ValueChanged(object? sender, EventArgs e)
            {
                item.Value = dateEditor.AsDateOnly;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates a DateTime edit control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateDateTimeEdit(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var args = item.CreateArg;
            var kind = (args?.CustomAttr["Kind"] as DateTimePickerKind?) ?? DateTimePickerKind.DateTime;

            if (kind == DateTimePickerKind.Date)
            {
                return CreateOrUpdateDateEdit(sender, item, control);
            }

            if (kind == DateTimePickerKind.Time)
            {
                return CreateOrUpdateTimeEdit(sender, item, control);
            }

            var result = CreateOrUpdateControlAndLabel<DateTimePicker>(sender, item, control);
            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var editor = result.MainControl;

            editor.Kind = kind;

            if (item.Value is DateTime dateTimeValue)
                editor.Value = dateTimeValue;

            editor.ValueChanged -= ValueChanged;
            editor.ValueChanged += ValueChanged;

            void ValueChanged(object? sender, EventArgs e)
            {
                item.Value = editor.Value;
            }

            return result;
        }

        /// <summary>
        /// Creates or updates an input control for the specified item.
        /// </summary>
        /// <param name="sender">The <see cref="PanelSettings"/> instance that is sending the request.</param>
        /// <param name="item">Item to convert.</param>
        /// <param name="control">The existing control which properties should
        /// be updated using item's properties. Can be null, in this case new control
        /// need to be created.</param>
        /// <returns>The control used to represent <see cref="PanelSettingsItem"/>.</returns>
        public static object? CreateOrUpdateTextBox(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
        {
            var args = item.CreateArg;
            var checkBoxInLabel = args is not null && args.CustomFlags["CheckBoxInLabel"];
            var useMemo = args is not null && args.CustomFlags["IsMultiline"];
            var minHeight = args?.CustomAttr["MinHeight"] as int?;

            ControlAndLabel<TextBoxAndButton, GenericControl>? result;

            result = control as ControlAndLabel<TextBoxAndButton, GenericControl>;

            if (result is null)
            {
                var typeOfTextBox = useMemo ? typeof(MemoAndButton) : typeof(TextBoxAndButton);

                if (checkBoxInLabel)
                {
                    result = new ControlAndLabel<TextBoxAndButton, GenericControl>(typeof(XCheckBox), typeOfTextBox);

                    if (result.Label is XCheckBox checkBox)
                    {
                        checkBox.Item.CheckBoxMargin = DefaultCheckImageMargin;
                    }
                }
                else
                {
                    result = new ControlAndLabel<TextBoxAndButton, GenericControl>(typeof(Label), typeOfTextBox);
                }

                if (minHeight.HasValue)
                {
                    result.MainControl.MainControl.MinHeight = minHeight.Value;
                }
                ;
            }

            UpdateCommonProps(sender, item, result);

            result.LabelToControl = StackPanelOrientation.Vertical;
            UpdateText(sender, item, result.Label);

            var textBox = result.MainControl;
            textBox.HasBtnComboBox = false;
            textBox.Buttons.Visible = false;

            textBox.TextBox.ValueHelper.SetValidator(item.ValueType, false);
            textBox.TextBox.ValueHelper.AutoShowError = true;
            textBox.TextBox.ValueHelper.Options |= TextBoxOptions.DefaultValidation;
            textBox.TextBox.ValueHelper.TextAsValue = item.Value;
            textBox.TextBox.ValueHelper.IsRequired = GetFlagIsRequired(item.CreateArg);

            textBox.DelayedTextChanged -= TextChanged;
            textBox.DelayedTextChanged += TextChanged;

            void TextChanged(object? control, EventArgs e)
            {
                var previousValue = item.Value;

                try
                {
                    if (item.ValueType is not null && item.ValueType != typeof(string))
                    {
                        var dictionary = sender.TypeConverters ?? DefaultTypeConverters;

                        var found = dictionary.TryGetValue(item.ValueType, out var converter);

                        if (found && converter is not null)
                        {
                            var converterInstance = Activator.CreateInstance(converter);

                            if (converterInstance is TypeConverter typeConverter)
                            {
                                var convertedValue = typeConverter.ConvertFromString(textBox.TextBox.Text);
                                item.Value = convertedValue;
                                return;
                            }
                        }
                    }

                    item.Value = textBox.TextBox.ValueHelper.TextAsValue;
                }
                catch (Exception ex)
                {
                    item.Value = previousValue;
                    sender.RaiseProcessException(new ThrowExceptionEventArgs(ex));
                }
            }

            return result;
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
            UIPlatformKind platform = UIPlatformKind.Unspecified)
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
            UIPlatformKind platform = UIPlatformKind.Unspecified)
        {
            return itemToControl[kind];
        }

        /// <summary>
        /// Adds item with an empty space.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <param name="minHeight">Minimum height of the spacer.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddSpacer(Coord minHeight = 0, CustomEventArgs? e = null)
        {
            if (minHeight > 0)
            {
                e ??= new();
                e.Attr("MinHeight", minHeight);
            }

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
        /// Gets the control which is used to represent the specified item.
        /// This control contains inner label and editor controls. 
        /// Use <see cref="GetItemControlLabel"/> and <see cref="GetItemControlEditor"/> to get label and editor controls.
        /// </summary>
        /// <param name="item">The panel settings item.</param>
        /// <returns>The control representing the item, or null if not found.</returns>
        public virtual AbstractControl? GetItemControl(PanelSettingsItem? item)
        {
            if (item == null)
                return null;
            var id = item.UniqueId;

            foreach (var itemControl in Children)
            {
                if (itemControl.CustomAttr["PanelSettingsItem"]?.Equals(id) == true)
                    return itemControl;
            }

            return null;
        }

        /// <summary>
        /// Gets the label control which is used to represent the specified item.
        /// </summary>
        /// <param name="item">The panel settings item.</param>
        /// <returns>The label control representing the item, or null if not found.</returns>
        public virtual AbstractControl? GetItemControlLabel(PanelSettingsItem? item)
        {
            var itemControl = GetItemControl(item);

            if (itemControl is IControlAndLabel controlAndLabel)
                return controlAndLabel.Label;
            return null;
        }

        /// <summary>
        /// Gets the editor control which is used to represent the specified item.
        /// </summary>
        /// <param name="item">The panel settings item.</param>
        /// <returns>The editor control representing the item, or null if not found.</returns>
        public virtual AbstractControl? GetItemControlEditor(PanelSettingsItem? item)
        {
            var itemControl = GetItemControl(item);

            if (itemControl is IControlAndLabel controlAndLabel)
                return controlAndLabel.MainControl;
            return null;
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
        /// Begins radio button group. All items added after this call will be part of the same radio button group.
        /// Use <see cref="EndRadioGroup"/> to end the radio button group.
        /// </summary>
        /// <returns>Radio group identifier.</returns>
        public virtual ObjectUniqueId BeginRadioGroup()
        {
            radioGroupStack ??= new();
            var result = new ObjectUniqueId(ref globalCounter);
            radioGroupStack.Push(result);
            return result;
        }

        /// <summary>
        /// Gets the current radio button group identifier.
        /// Use <see cref="BeginRadioGroup"/> to start a new radio button group and <see cref="EndRadioGroup"/> to end it.
        /// </summary>
        /// <returns>Current radio group identifier. Returns null if no radio group is active.</returns>
        public ObjectUniqueId? GetCurrentRadioGroup()
        {
            if (radioGroupStack == null || radioGroupStack.Count == 0)
                return null;
            if (radioGroupStack.TryPeek(out var result))
                return result;
            return null;
        }

        /// <summary>
        /// Ends radio button group started with <see cref="BeginRadioGroup"/>.
        /// All items added after this call will not be part of the same radio button group.
        /// </summary>
        public virtual void EndRadioGroup()
        {
            radioGroupStack?.TryPop(out _);
        }

        /// <summary>
        /// Adds a group of checkboxes for editing of the specified enum type.
        /// Each checkbox corresponds to a specific flag value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <param name="label">The label for the checkbox group.</param>
        /// <param name="getValue">The function to get the current value.</param>
        /// <param name="setValue">The action to set the value.</param>
        /// <param name="itemTitles">The titles for the checkboxes. Optional.
        /// If not provided, the values will be used as titles.</param>
        /// <param name="itemValues">The values for the checkboxes. Enum elements are specified here.</param>
        /// <param name="e">Additional arguments.</param>
        public virtual void AddFlagCheckBoxes<TEnum>(
            object? label,
            Func<TEnum> getValue,
            Action<TEnum> setValue,
            object?[]? itemTitles,
            TEnum[] itemValues,
            CustomEventArgs? e = null)
            where TEnum : struct, Enum
        {
            if (label is not null)
            {
                AddLabel(label);
            }

            for (int i = 0; i < itemValues.Length; i++)
            {
                var value = itemValues[i];
                var title = itemTitles?[i] ?? value.ToString();

                var item = AddInput<bool>(
                    title,
                    () => getValue().HasFlag(value),
                    (isChecked) =>
                    {
                        var oldValue = getValue();
                        var underlying = Convert.ToUInt64(oldValue);
                        var flag = Convert.ToUInt64(value);

                        ulong newUnderlying;

                        if (isChecked)
                        {
                            newUnderlying = underlying | flag;
                        }
                        else
                        {
                            newUnderlying = underlying & ~flag;
                        }

                        var newValue = (TEnum)Enum.ToObject(typeof(TEnum), newUnderlying);
                        setValue(newValue);
                    },
                    e);
            }
        }

        /// <summary>
        /// Adds a group of radio buttons for the specified value type. Each radio button corresponds to a specific value.
        /// This can be used to allow the user to select one value from a predefined set of options.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="label">The label for the radio button group.</param>
        /// <param name="getValue">The function to get the current value.</param>
        /// <param name="setValue">The action to set the value.</param>
        /// <param name="itemTitles">The titles for the radio buttons. Optional.
        /// If not provided, the values will be used as titles.</param>
        /// <param name="itemValues">The values for the radio buttons.</param>
        /// <param name="e">Additional arguments.</param>
        public virtual void AddRadioButtons<T>(
            object? label,
            Func<T> getValue,
            Action<T> setValue,
            object?[]? itemTitles,
            T[] itemValues,
            CustomEventArgs? e = null)
        {
            if (label is not null)
            {
                AddLabel(label);
            }

            e ??= new CustomEventArgs();
            e.CustomFlags["IsRadioButton"] = true;

            var groupIdentifier = BeginRadioGroup();

            for (int i = 0; i < itemValues.Length; i++)
            {
                var value = itemValues[i];
                var title = itemTitles?[i] ?? value?.ToString();

                if (title is null)
                    continue;

                var item = AddInput<bool>(
                    title,
                    () => getValue()?.Equals(value) ?? false,
                    (isChecked) =>
                    {
                        if (isChecked)
                            setValue(value);
                    },
                    e);
                var control = GetItemControl(item);

                if (control is XRadioButton radioButton)
                {
                    radioButton.RadioGroupId = groupIdentifier;
                }
            }

            EndRadioGroup();
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
        /// Adds horizontal line.
        /// </summary>
        /// <param name="e">Additional arguments.</param>
        /// <returns></returns>
        public virtual PanelSettingsItem AddHorizontalLine(CustomEventArgs? e = null)
        {
            PanelSettingsItem item
                = CreateItemCore("HorizontalLine", PanelSettingsItemKind.Line, null, e);
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
            PanelSettingsItem item;

            var valueSource = new PropertyValueSource(propContainer, propName);
            var flagsOrEnum = PropertyGridUtils.IsFlagsOrEnum(propContainer, valueSource.PropInfo);

            switch (flagsOrEnum)
            {
                default:
                case FlagsOrEnum.None:
                    item = CreateItemCore(label, PanelSettingsItemKind.Value, valueSource, e);
                    break;
                case FlagsOrEnum.Enum:
                    item = CreateItemCore(label, PanelSettingsItemKind.Enum, valueSource, e);
                    break;
                case FlagsOrEnum.Flags:
                    App.LogError("PanelSettings.AddInput: Enum with [Flags] is not supported");
                    item = new PanelSettingsItem();
                    break;
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
            var itemControl = GetItemControl(item);

            if (itemControl != null)
            {
                itemControl.Parent = null;
                itemControl.Dispose();
            }

            item.Owner = null;

            if (!AutoCreate)
                return;
        }

        /// <summary>
        /// Called when item is added to the <see cref="Items"/> collection.
        /// </summary>
        protected virtual void ItemInserted(object? sender, int index, PanelSettingsItem item)
        {
            if (item.Owner is null)
            {
                item.Owner = this;
            }
            else
            {
                throw new InvalidOperationException("Item already has an owner.");
            }

            if (!AutoCreate)
                return;
            var conversion = item.ItemToControl ?? GetRegisteredConversion(item.Kind);
            if (conversion is null)
                return;
            var obj = conversion(this, item, null);
            if (obj is not AbstractControl control)
                return;
            control.CustomAttr["PanelSettingsItem"] = item.UniqueId;
            control.Parent = this;
        }

        private static void UpdateCommonProps(PanelSettings sender, PanelSettingsItem item, AbstractControl control)
        {
            control.Visible = item.IsVisible;
            control.Enabled = item.IsEnabled;
        }

        private static void UpdateText(PanelSettings sender, PanelSettingsItem item, AbstractControl control)
        {
            var text = item.Label?.ToString() ?? string.Empty;
            control.Text = text;
        }

        private static T CreateOrUpdateControl<T>(PanelSettings sender, PanelSettingsItem item, object? control)
            where T : AbstractControl, new()
        {
            T? typedControl = control as T ?? new T();
            UpdateCommonProps(sender, item, typedControl);
            return typedControl;
        }

        private static ControlAndLabel<TControl, GenericControl> CreateOrUpdateControlAndLabel<TControl>(
            PanelSettings sender,
            PanelSettingsItem item,
            object? control)
            where TControl : AbstractControl, new()
        {
            var args = item.CreateArg;
            var checkBoxInLabel = args is not null && args.CustomFlags["CheckBoxInLabel"];

            ControlAndLabel<TControl, GenericControl>? result;

            result = control as ControlAndLabel<TControl, GenericControl>;

            if (result is null)
            {
                if (checkBoxInLabel)
                {
                    result = new ControlAndLabel<TControl, GenericControl>(typeof(XCheckBox));

                    if (result.Label is XCheckBox checkBox)
                    {
                        checkBox.Item.CheckBoxMargin = DefaultCheckImageMargin;
                    }
                }
                else
                {
                    result = new ControlAndLabel<TControl, GenericControl>(typeof(Label));
                }
            }

            UpdateCommonProps(sender, item, result);
            return result;
        }

        private static bool GetFlagIsRequired(CustomEventArgs? e)
        {
            if (e is null)
                return false;
            return e.CustomFlags["IsRequired"];
        }
    }

    /// <summary>
    /// Represents a scrollable panel settings control that contains a <see cref="PanelSettings"/> instance.
    /// Use <see cref="Panel"/> to access the contained <see cref="PanelSettings"/> instance.
    /// </summary>
    public partial class ScrollablePanelSettings : ScrollViewer
    {
        private readonly PanelSettings panel = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrollablePanelSettings"/> class.
        /// </summary>
        public ScrollablePanelSettings()
        {
            panel.Parent = base.Content;
        }

        /// <summary>
        /// Gets the <see cref="PanelSettings"/> instance that is used to manage the items and their corresponding controls.
        /// </summary>
        [Browsable(false)]
        public PanelSettings Panel => panel;
    }
}
