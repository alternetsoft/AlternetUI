using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a control that can be used to display or edit unformatted text.
    /// </summary>
    /// <remarks>
    /// With the <see cref="TextBox"/> control, the user can enter text in
    /// an application.
    /// </remarks>
    [DefaultEvent("TextChanged")]
    [DefaultBindingProperty("Text")]
    [ControlCategory("Common")]
    public class TextBox : CustomTextEdit, IValidatorReporter
    {
        /// <summary>
        /// Identifies the <see cref="Text"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(TextBox),
                new FrameworkPropertyMetadata(
                        string.Empty,
                        PropMetadataOption.BindsTwoWayByDefault | PropMetadataOption.AffectsPaint,
                        new PropertyChangedCallback(OnTextPropertyChanged),
                        new CoerceValueCallback(CoerceText),
                        true, // IsAnimationProhibited
                        UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent =
            EventManager.RegisterRoutedEvent(
            "TextChanged",
            RoutingStrategy.Bubble,
            typeof(TextChangedEventHandler),
            typeof(TextBox));

        private bool isRichEdit = false;
        private bool multiline = false;
        private bool hasBorder = true;
        private bool readOnly = false;
        private TextBoxTextWrap textWrap;
        private GenericAlignment textAlign;
        private IValueValidator? validator;
        private int maxLength;
        private int minLength;
        private TextBoxOptions options = TextBoxOptions.IntRangeInError;

        static TextBox()
        {
            var choices = PropertyGrid.CreateChoices();
            choices.Add(GenericAlignment.Left);
            choices.Add(GenericAlignment.Right);
            choices.Add(GenericAlignment.CenterHorizontal);
            var prm = PropertyGrid.GetNewItemParams(typeof(TextBox), nameof(TextBox.TextAlign));
            prm.EnumIsFlags = false;
            prm.Choices = choices;

            var useErrorColors = Application.IsWindowsOS;
            DefaultErrorUseForegroundColor = useErrorColors;
            DefaultErrorUseBackgroundColor = useErrorColors;
        }

        /// <summary>
        /// Occurs when <see cref="Multiline"/> property value changes.
        /// </summary>
        public event EventHandler? MultilineChanged;

        /// <summary>
        /// Occurs when <see cref="ReadOnly"/> property value changes.
        /// </summary>
        public event EventHandler? ReadOnlyChanged;

        /// <summary>
        /// Occurs when <see cref="CurrentPosition"/> property value changes.
        /// </summary>
        /// <remarks>
        /// You need to call <see cref="IdleAction"/> in the
        /// <see cref="Application.Idle"/> event handler in order to enable
        /// <see cref="CurrentPositionChanged"/> event firing.
        /// </remarks>
        public event EventHandler? CurrentPositionChanged;

        /// <summary>
        /// Occurs when <see cref="ReportValidatorError"/> is called.
        /// </summary>
        /// <remarks>
        /// You can handle this event in order to show validation error information.
        /// </remarks>
        public event ErrorStatusEventHandler? ErrorStatusChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Text"/> property changes.
        /// </summary>
        public event TextChangedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        /// <summary>
        /// Occurs when Enter key is pressed in the control.
        /// </summary>
        /// <remarks>
        /// Event is raised only if <see cref="ProcessEnter"/> is true.
        /// </remarks>
        public event EventHandler? EnterPressed;

        /// <summary>
        /// Occurs when url clicked in the text.
        /// </summary>
        /// <remarks>
        /// This event is not fired on MacOs. On this os, url is
        /// automatically opened in the default browser.
        /// </remarks>
        public event UrlEventHandler? TextUrl;

        /// <summary>
        /// Occurs when maximal text length is reached.
        /// </summary>
        /// <remarks>
        /// Use <see cref="MaxLength"/> to set maximal text length.
        /// </remarks>
        public event EventHandler? TextMaxLength;

        /// <summary>
        /// Occurs when <see cref="HasBorder"/> property value changes.
        /// </summary>
        public event EventHandler? HasBorderChanged;

        /// <summary>
        /// Gets or sets a text string that can be used as a default validator error message.
        /// </summary>
        /// <remarks>
        /// This property can be used when <see cref="ValidatorErrorText"/> is <c>null</c>.
        /// </remarks>
        public static string? DefaultValidatorErrorText { get; set; }

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a background color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="Text"/> property.
        /// </summary>
        public static Color DefaultErrorBackgroundColor { get; set; } = Color.Red;

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a foreground color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="Text"/> property.
        /// </summary>
        public static Color DefaultErrorForegroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorForegroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c> on Windows and <c>false</c> on other platforms.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorBackgroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c> on Windows and <c>false</c> on other platforms.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets default value of the <see cref="AutoUrlOpen"/> property.
        /// </summary>
        public static bool DefaultAutoUrlOpen { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are opened in the default browser.
        /// </summary>
        /// <remarks>
        /// Use <see cref="AutoUrl"/> in order to highlight and underline urls.
        /// </remarks>
        public virtual bool AutoUrlOpen { get; set; } = DefaultAutoUrlOpen;

        /// <summary>
        /// Gets or sets a bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <see cref="Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it if <see cref="TextBox"/> edits a number value or
        /// for any other purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual NumberStyles? NumberStyles { get; set; }

        /// <summary>
        /// Gets or sets an object that supplies culture-specific formatting information
        /// about <see cref="Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets default format used in value to string convertion.
        /// </summary>
        public virtual string? DefaultFormat { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IObjectToString"/> provider which is used in
        /// value to string convertion.
        /// </summary>
        public virtual IObjectToString? Converter { get; set; }

        /// <summary>
        /// Gets or sets default value for the <see cref="Text"/> property.
        /// </summary>
        public virtual string? DefaultText { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Type"/> of the <see cref="Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual Type? DataType { get; set; }

        /// <summary>
        /// Gets or sets validator reporter object or control.
        /// </summary>
        /// <remarks>
        /// This propety can be used to store reference to control that
        /// reports validation or other errors to the end users. Usually
        /// this is a <see cref="PictureBox"/> with error image.
        /// </remarks>
        [Browsable(false)]
        public virtual object? ValidatorReporter { get; set; }

        /// <summary>
        /// Gets or sets a text string that can be used as validator error message.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual string? ValidatorErrorText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether empty string is allowed in <see cref="Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AllowEmptyText { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Text"/> is required to be not empty.
        /// This is an opposite of <see cref="AllowEmptyText"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public bool IsRequired
        {
            get
            {
                return !AllowEmptyText;
            }

            set
            {
                AllowEmptyText = !value;
            }
        }

        /// <summary>
        /// Gets or sets data value in cases when <see cref="Text"/> property is empty.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual object? EmptyTextValue { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IValueValidator"/> for the <see cref="TextBox"/> control.
        /// </summary>
        /// <remarks>
        /// <see cref="IValueValidator"/> allows to set limitations on possible values of
        /// the <see cref="Text"/> property. See <see cref="IValueValidatorText"/> and
        /// <see cref="ValueValidatorFactory.CreateValueValidatorText"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual IValueValidator? Validator
        {
            get
            {
                return validator;
            }

            set
            {
                if (validator == value)
                    return;
                validator = value;
                if (validator == null)
                    Handler.NativeControl.Validator = IntPtr.Zero;
                else
                    Handler.NativeControl.Validator = validator.Handle;
            }
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public override string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }

            set
            {
                SetValue(TextProperty, value);
            }
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.TextBox;

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline text
        /// box control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the control is a multiline text box
        /// control; otherwise, <see langword="false"/>. The default
        /// is <see langword="false"/>.
        /// </value>
        public virtual bool Multiline
        {
            get
            {
                return multiline;
            }

            set
            {
                if (multiline == value)
                    return;
                multiline = value;
                MultilineChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text control is in rich edit mode
        /// </summary>
        /// <remarks>
        /// In the rich edit mode it is possible to apply text formatting (for example
        /// change text font or color). Also it is possible to edit large texts.
        /// </remarks>
        public virtual bool IsRichEdit
        {
            get
            {
                return isRichEdit;
            }

            set
            {
                if (isRichEdit == value)
                    return;
                isRichEdit = value;
                Handler.IsRichEdit = value;
            }
        }

        /// <summary>
        /// Gets or sets text wrap style fot the multiline <see cref="TextBox"/> controls.
        /// </summary>
        /// <remarks>
        /// Default value is TextBoxTextWrap.Best. Some platforms do not support all wrap styles.
        /// </remarks>
        [DefaultValue(TextBoxTextWrap.Best)]
        public virtual TextBoxTextWrap TextWrap
        {
            get
            {
                return textWrap;
            }

            set
            {
                if (textWrap == value)
                    return;
                textWrap = value;
                Handler.TextWrap = value;
            }
        }

        /// <summary>
        /// Gets or sets text alignment for the <see cref="TextBox"/> control.
        /// </summary>
        /// <remarks>
        /// Only <see cref="GenericAlignment.Left"/>, <see cref="GenericAlignment.Right"/>
        /// and <see cref="GenericAlignment.CenterHorizontal"/> are supported.
        /// </remarks>
        /// <remarks>
        /// Default value is <see cref="GenericAlignment.Left"/>.
        /// </remarks>
        [DefaultValue(GenericAlignment.Left)]
        public virtual GenericAlignment TextAlign
        {
            get
            {
                return textAlign;
            }

            set
            {
                if (textAlign == value)
                    return;
                textAlign = value;
                Handler.TextAlign = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the text box is read-only; otherwise,
        /// <see langword="false"/>. The default is <see
        /// langword="false"/>.
        /// </value>
        /// <remarks>
        /// When this property is set to <see langword="true"/>, the contents
        /// of the control cannot be changed by the user at runtime.
        /// With this property set to <see langword="true"/>, you can still
        /// set the value of the <see cref="Text"/> property in code.
        /// </remarks>
        public virtual bool ReadOnly
        {
            get
            {
                return readOnly;
            }

            set
            {
                if (readOnly == value)
                    return;

                readOnly = value;
                ReadOnlyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters
        /// the user can enter into the control.
        /// </summary>
        /// <remarks>
        /// If new max length is 0, the previously set max length limit, if any,
        /// is discarded and the user may enter as much text as
        /// the underlying native text control widget
        /// supports (typically at least 32Kb).
        /// </remarks>
        /// <remarks>
        /// If the user tries to enter more characters into the text control
        /// when it already is filled up to the maximal length, a <see cref="TextMaxLength"/>
        /// event is sent to
        /// notify about it (giving it the possibility to
        /// show an explanatory message, for example) and the extra
        /// input is discarded.
        /// </remarks>
        /// <remarks>
        /// Note that on Linux this function may only be used with
        /// single line text controls.
        /// </remarks>
        public virtual int MaxLength
        {
            get
            {
                return maxLength;
            }

            set
            {
                if (maxLength == value || value < 0)
                    return;
                maxLength = value;
                if (options.HasFlag(TextBoxOptions.SetNativeMaxLength))
                    Handler.SetMaxLength((ulong)value);
            }
        }

        /// <summary>
        /// Gets or sets the minimum number of characters
        /// the user must enter into the control.
        /// </summary>
        /// <remarks>
        /// Currently this property doesn't affect <see cref="TextBox"/> behavior.
        /// You can implement your own validation rules using <see cref="TextChanged"/> event.
        /// </remarks>
        public virtual int MinLength
        {
            get
            {
                return minLength;
            }

            set
            {
                if (minLength == value || value < 0)
                    return;
                minLength = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                HasBorderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether control has selected text.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasSelection
        {
            get
            {
                return Handler.HasSelection;
            }
        }

        /// <summary>
        /// Gets or sets whether text is modified in the control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsModified
        {
            get
            {
                return Handler.IsModified;
            }

            set
            {
                Handler.IsModified = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Text"/> property value as <see cref="object"/> of a number type.
        /// </summary>
        /// <remarks>
        /// If <see cref="Text"/> property doesn't contain a number value, <c>null</c> is returned.
        /// </remarks>
        [Browsable(false)]
        public virtual object? TextAsNumber
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Text))
                    return this.EmptyTextValue;

                var typeCode = GetDataTypeCode();
                if (!AssemblyUtils.IsTypeCodeNumber(typeCode))
                    return SmartTextAsNumber();

                var isOk = StringUtils.TryParseNumber(
                    typeCode,
                    Text,
                    NumberStyles,
                    FormatProvider,
                    out var result);
                if (isOk)
                    return result;
                else
                    return null;

                object? UseDelegate(TryParseNumberDelegate proc)
                {
                    var numberStyles = NumberStyles ?? System.Globalization.NumberStyles.Any;
                    var isOk = proc(
                        Text,
                        numberStyles,
                        FormatProvider,
                        out result);
                    if (isOk)
                        return result;
                    else
                        return null;
                }

                object? SmartTextAsNumber()
                {
                    TryParseNumberDelegate[] procs =
                    {
                        StringUtils.TryParseInt32,
                        StringUtils.TryParseUInt32,
                        StringUtils.TryParseInt64,
                        StringUtils.TryParseUInt64,
                        StringUtils.TryParseDouble,
                        StringUtils.TryParseDecimal,
                    };

                    foreach (var proc in procs)
                    {
                        object? result = UseDelegate(proc);
                        if (result is not null)
                            return result;
                    }

                    return null;
                }
            }

            set
            {
                SetTextAsObject(value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be copied,
        /// which allows the<see cref="Copy"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the current selection can be copied;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCopy
        {
            get
            {
                return Handler.CanCopy;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be cut,
        /// which allows the<see cref="Cut"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the current selection can be cut;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanCut
        {
            get
            {
                return Handler.CanCut;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current selection can be replaced with
        /// the contents of the Clipboard, which allows
        /// the<see cref="Paste"/> method to succeed.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the data can be pasted;
        /// otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanPaste
        {
            get
            {
                return Handler.CanPaste;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user can redo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can redo the previous
        /// operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns>
        [Browsable(false)]
        public virtual bool CanRedo
        {
            get
            {
                return Handler.CanRedo;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selected text
        /// in the text box control remains highlighted when the
        /// control loses focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the selected text does not appear
        /// highlighted when the text box control loses focus;
        /// <see langword="false" />, if the selected text remains
        /// highlighted when the text box control loses focus.
        /// The default is <see langword="true" />.
        /// </returns>
        public virtual bool HideSelection
        {
            get
            {
                return Handler.HideSelection;
            }

            set
            {
                Handler.HideSelection = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TAB key is received and
        /// processed by the control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the TAB key is received by the control;
        /// <see langword="false" />, if the TAB key is not received by the control.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// Normally, TAB key is used for passing to the next control in a dialog.
        /// For the control created with this style, you can still use
        /// Ctrl-Enter to pass to the next control from the keyboard.
        /// </remarks>
        public virtual bool ProcessTab
        {
            get
            {
                return Handler.ProcessTab;
            }

            set
            {
                Handler.ProcessTab = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="EnterPressed"/>
        /// event is fired.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if <see cref="EnterPressed"/> event is generated;
        /// <see langword="false" />, if the ENTER key is either processed
        /// internally by the control or used for navigation between dialog controls.
        /// The default is <see langword="false" />.
        /// </returns>
        public virtual bool ProcessEnter
        {
            get
            {
                return Handler.ProcessEnter;
            }

            set
            {
                Handler.ProcessEnter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the input text is password and
        /// all characters are shown as *.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if all characters of the input text are shown
        /// as password chars;
        /// <see langword="false" />, if text is shown normally.
        /// The default is <see langword="false" />.
        /// </returns>
        public virtual bool IsPassword
        {
            get
            {
                return Handler.IsPassword;
            }

            set
            {
                Handler.IsPassword = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether urls in the input text
        /// are highlighted and underlined.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if urls in the input text
        /// are highlighted and underlined;
        /// <see langword="false" />, if urls in the input text are shown as normal
        /// text. The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This property affects control behavior when
        /// <see cref="IsRichEdit"/> property is <see langword="true" />.
        /// </remarks>
        /// <remarks>
        /// <see cref="TextUrl"/> event is fired when url is clicked.
        /// </remarks>
        public virtual bool AutoUrl
        {
            get
            {
                return Handler.AutoUrl;
            }

            set
            {
                Handler.AutoUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether vertical scrollbar is
        /// hidden in the control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if vertical scrollbar is hidden;
        /// <see langword="false" />, if vertical scrollbar is shown.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// This property is for multiline controls only. Hidden vertical scrollbar
        /// limits the amount of text which can be entered into the control to
        /// what can be displayed in it under Windows but not under Linux.
        /// Currently not implemented for the other platforms.
        /// </remarks>
        public virtual bool HideVertScrollbar
        {
            get
            {
                return Handler.HideVertScrollbar;
            }

            set
            {
                Handler.HideVertScrollbar = value;
            }
        }

        /// <summary>
        /// Gets or sets current position of the caret.
        /// </summary>
        /// <remarks>
        /// Uses <see cref="GetInsertionPoint"/> and <see cref="PositionToXY"/> to get the position.
        /// Uses <see cref="SetInsertionPoint"/> and <see cref="XYToPosition"/> to set the position.
        /// </remarks>
        /// <remarks>
        /// Caret position starts from (0,0).
        /// </remarks>
        [Browsable(false)]
        public virtual Int32Point? CurrentPosition
        {
            get
            {
                var insertPoint = GetInsertionPoint();
                var currentPos = PositionToXY(insertPoint);
                if (currentPos == Int32Point.MinusOne)
                    return null;
                return currentPos;
            }

            set
            {
                value ??= Int32Point.Empty;
                var insertPoint = XYToPosition((long)value.Value.X, (long)value.Value.Y);
                SetInsertionPoint(insertPoint);
                CurrentPositionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets position of the caret which was reported in the event.
        /// </summary>
        [Browsable(false)]
        public virtual Int32Point? ReportedPosition { get; set; }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can undo the previous
        /// operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns >
        [Browsable(false)]
        public virtual bool CanUndo
        {
            get
            {
                return Handler.CanUndo;
            }
        }

        /// <summary>
        /// Gets whether control contains text.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsEmpty
        {
            get
            {
                return Handler.IsEmpty;
            }
        }

        /// <summary>
        /// Gets or sets flags which customize behavior and visual style of the control.
        /// </summary>
        [Browsable(false)]
        public virtual TextBoxOptions Options
        {
            get
            {
                return options;
            }

            set
            {
                if (options == value)
                    return;
                options = value;
            }
        }

        /// <summary>
        /// Gets or sets a hint shown in an empty unfocused text control.
        /// </summary>
        public virtual string? EmptyTextHint
        {
            get
            {
                return Handler.EmptyTextHint;
            }

            set
            {
                value ??= string.Empty;
                Handler.EmptyTextHint = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum value that can be
        /// entered in the control.
        /// </summary>
        /// <remarks>
        /// This property has sense for the numbers and some other types.
        /// Default value is <c>null</c>.
        /// </remarks>
        /// <remarks>
        /// <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual object? MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value that can be
        /// entered in the control.
        /// </summary>
        /// <remarks>
        /// This property has sense for the numbers and some other types.
        /// Default value is <c>null</c>.
        /// </remarks>
        /// <remarks>
        /// <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual object? MaxValue { get; set; }

        internal new NativeTextBoxHandler Handler =>
            (NativeTextBoxHandler)base.Handler;

        /// <summary>
        /// Creates new custom text style.
        /// </summary>
        /// <returns></returns>
        public static ITextBoxTextAttr CreateTextAttr()
        {
            return new TextBoxTextAttr();
        }

        /// <inheritdoc cref="ValueValidatorFactory.CreateValidator(ValueValidatorKind)"/>
        public static IValueValidatorText CreateValidator(ValueValidatorKind kind)
        {
            return ValueValidatorFactory.CreateValidator(kind);
        }

        /// <inheritdoc cref="ValueValidatorFactory.CreateValidator(TypeCode)"/>
        public static IValueValidatorText CreateValidator(TypeCode typeCode)
        {
            return ValueValidatorFactory.CreateValidator(typeCode);
        }

        /// <summary>
        /// Initializes <see cref="PictureBox"/> with error image and other options.
        /// </summary>
        /// <param name="picture"></param>
        public static void InitErrorPicture(PictureBox picture)
        {
            picture.Image = KnownSvgImages.GetWarningImage();
            picture.VerticalAlignment = VerticalAlignment.Center;
            picture.ImageVisible = false;
            picture.ImageStretch = false;
            picture.TabStop = false;

            picture.MouseLeftButtonUp += Picture_MouseLeftButtonUp;

            static void Picture_MouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
            {
                if (sender is not PictureBox pictureBox)
                    return;
                e.Handled = true;

                RichToolTip.Show(
                    ErrorMessages.Default.ErrorTitle,
                    pictureBox.ToolTip,
                    pictureBox,
                    RichToolTipKind.None,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates <see cref="IValueValidatorText"/> instance for the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        public static IValueValidatorText CreateValidator(Type type)
        {
            var typeCode = AssemblyUtils.GetRealTypeCode(type);
            return CreateValidator(typeCode);
        }

        void IValidatorReporter.SetErrorStatus(object? sender, bool showError, string? errorText)
        {
            ErrorStatusChanged?.Invoke(this, new(showError, errorText));
        }

        /// <summary>
        /// Gets "real" minimal value taking into account <see cref="DataType"/>
        /// and <see cref="MinValue"/>.
        /// </summary>
        public virtual object? GetRealMinValue()
        {
            var typeCode = GetDataTypeCode();
            var result = MathUtils.Max(AssemblyUtils.GetMinValue(typeCode), MinValue);
            return result;
        }

        /// <summary>
        /// Gets "real" maxmimal value taking into account <see cref="DataType"/>
        /// and <see cref="MinValue"/>.
        /// </summary>
        public virtual object? GetRealMaxValue()
        {
            var typeCode = GetDataTypeCode();
            var result = MathUtils.Min(AssemblyUtils.GetMaxValue(typeCode), MaxValue);
            return result;
        }

        /// <summary>
        /// Returns <see cref="TextAsNumber"/> or <paramref name="defValue"/> if it is <c>null</c>.
        /// </summary>
        /// <typeparam name="T">Type of the result.</typeparam>
        /// <param name="defValue">Default value.</param>
        public T TextAsNumberOrDefault<T>(T defValue)
        {
            var result = TextAsNumber;
            if (result is null)
                return defValue;
            return (T)result;
        }

        /// <summary>
        /// Changes the style of selection (if any). If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="style">The new text style.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetSelectionStyle(ITextBoxTextAttr style)
        {
            if (HasSelection)
            {
                var selectionStart = GetSelectionStart();
                var selectionEnd = GetSelectionEnd();
                var result = SetStyle(selectionStart, selectionEnd, style);
                return result;
            }
            else
            {
                var position = GetInsertionPoint();
                var result = SetStyle(position, position, style);
                return result;
            }
        }

        /// <summary>
        /// Toggles <see cref="FontStyle"/> of the selection. If no text is selected, style of the
        /// insertion point is changed.
        /// </summary>
        /// <param name="toggle">Font style to toggle</param>
        public virtual void ToggleSelectionFontStyle(FontStyle toggle)
        {
            var position = GetInsertionPoint();
            var fs = GetStyle(position);
            var style = fs.GetFontStyle();
            style = Font.ChangeFontStyle(style, toggle, !style.HasFlag(toggle));

            var newStyle = TextBox.CreateTextAttr();
            newStyle.Copy(fs);
            newStyle.SetFontStyle(style);
            SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Clears text formatting when <see cref="TextBox"/> is in rich edit mode.
        /// </summary>
        public void ClearTextFormatting()
        {
            var attr = CreateTextAttr();
            attr.SetFlags(TextBoxTextAttrFlags.All);
            SetSelectionStyle(attr);
        }

        /// <summary>
        /// Sets foreground and background colors of the selection.  If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        /// <param name="textColor">Foreground color.</param>
        /// <param name="backColor">Background color.</param>
        /// <remarks>
        /// If any of the color parameters is null, it will not be changed.
        /// </remarks>
        public void SelectionSetColor(Color? textColor, Color? backColor = null)
        {
            var position = GetInsertionPoint();
            var fs = GetStyle(position);
            var newStyle = TextBox.CreateTextAttr();
            newStyle.Copy(fs);
            if (backColor is not null)
                newStyle.SetBackgroundColor(backColor.Value);
            if (textColor is not null)
                newStyle.SetTextColor(textColor.Value);
            SetSelectionStyle(newStyle);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Bold"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void ToggleSelectionBold()
        {
            ToggleSelectionFontStyle(FontStyle.Bold);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Italic"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void ToggleSelectionItalic()
        {
            ToggleSelectionFontStyle(FontStyle.Italic);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Underlined"/> style of the selection. If no text is selected,
        /// style of the insertion point is changed.
        /// </summary>
        public virtual void ToggleSelectionUnderline()
        {
            ToggleSelectionFontStyle(FontStyle.Underlined);
        }

        /// <summary>
        /// Toggles <see cref="FontStyle.Strikethrough"/> style of the selection. If no text is
        /// selected, style of the insertion point is changed.
        /// </summary>
        public virtual void ToggleSelectionStrikethrough()
        {
            ToggleSelectionFontStyle(FontStyle.Strikethrough);
        }

        /// <summary>
        /// Sets <see cref="DataType"/> property to <typeparamref name="T"/>
        /// and <see cref="Validator"/> to the appropriate validator provider.
        /// </summary>
        /// <typeparam name="T">New <see cref="DataType"/> property value.</typeparam>
        public virtual void UseValidator<T>()
        {
            DataType = typeof(T);
            Validator = CreateValidator(typeof(T));
        }

        /// <summary>
        /// Reports an error if <see cref="Text"/> property is empty
        /// and it is not allowed (<see cref="AllowEmptyText"/> is <c>false</c>).
        /// </summary>
        public virtual bool ReportErrorEmptyText()
        {
            if (string.IsNullOrEmpty(Text))
            {
                if (AllowEmptyText)
                {
                    return false;
                }
                else
                {
                    ReportValidatorError(true);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reports an error if <paramref name="value"/>
        /// is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error. If
        /// <see cref="DataType"/> is assigned, it is also used to get possible min and max
        /// values.
        /// </remarks>
        public virtual bool ReportErrorMinMaxValue(object? value)
        {
            if (value is null)
                return false;
            var valueInRange = MathUtils.ValueInRange(
                value,
                GetRealMinValue(),
                GetRealMaxValue());
            switch (valueInRange)
            {
                case ValueInRangeResult.Less:
                    ReportValidatorError(
                        true,
                        GetKnownErrorText(ValueValidatorKnownError.MinimumValue));
                    return true;
                case ValueInRangeResult.Greater:
                    ReportValidatorError(
                        true,
                        GetKnownErrorText(ValueValidatorKnownError.MaximumValue));
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reports an error if <see cref="DataType"/> is a number type and
        /// <see cref="TextAsNumber"/> is <c>null</c>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error.
        /// </remarks>
        public virtual bool ReportErrorBadNumber()
        {
            if (DataTypeIsNumber() && !string.IsNullOrEmpty(Text))
            {
                var value = TextAsNumber;
                if (value is null)
                {
                    ReportValidatorError(true);
                    return true;
                }

                if (ReportErrorMinMaxValue(value))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reports an error if length of the <see cref="Text"/> property value
        /// is less than <see cref="MinLength"/> or greater than <see cref="MaxLength"/>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error.
        /// </remarks>
        public virtual bool ReportErrorMinMaxLength()
        {
            if (MinLength > 0)
            {
                if (Text.Length < MinLength)
                {
                    ReportValidatorError(
                        true,
                        GetKnownErrorText(ValueValidatorKnownError.MinimumLength));
                    return true;
                }
            }

            if (MaxLength > 0)
            {
                if (Text.Length > MaxLength)
                {
                    ReportValidatorError(
                        true,
                        GetKnownErrorText(ValueValidatorKnownError.MaximumLength));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Runs default validation of the <see cref="Text"/> property.
        /// </summary>
        /// Validation errors are reported using <see cref="ReportValidatorError"/>.
        public virtual void RunDefaultValidation()
        {
            if (ReportErrorEmptyText())
                return;

            if (ReportErrorBadNumber())
                return;

            if (ReportErrorMinMaxLength())
                return;

            ReportValidatorError(false);
        }

        /// <summary>
        /// Gets known error text.
        /// </summary>
        /// <param name="kind">Error kind.</param>
        public virtual string GetKnownErrorText(ValueValidatorKnownError kind)
        {
            string AddRangeSuffix(string s)
            {
                if (options.HasFlag(TextBoxOptions.IntRangeInError))
                {
                    var rangeStr = GetMinMaxRangeStr(ErrorMessages.Default.ValidationRangeFormat);
                    return $"{s} {rangeStr}".Trim();
                }
                else
                    return s;
            }

            switch (kind)
            {
                case ValueValidatorKnownError.NumberIsExpected:
                    return AddRangeSuffix(ErrorMessages.Default.ValidationNumberIsExpected);
                case ValueValidatorKnownError.UnsignedNumberIsExpected:
                    return AddRangeSuffix(ErrorMessages.Default.ValidationUnsignedNumberIsExpected);
                case ValueValidatorKnownError.FloatIsExpected:
                    return ErrorMessages.Default.ValidationFloatIsExpected;
                case ValueValidatorKnownError.UnsignedFloatIsExpected:
                    return ErrorMessages.Default.ValidationUnsignedFloatIsExpected;
                case ValueValidatorKnownError.HexNumberIsExpected:
                    return ErrorMessages.Default.ValidationHexNumberIsExpected;
                case ValueValidatorKnownError.InvalidFormat:
                    return ErrorMessages.Default.ValidationInvalidFormat;
                case ValueValidatorKnownError.MinimumLength:
                    return string.Format(ErrorMessages.Default.ValidationMinimumLength, MinLength);
                case ValueValidatorKnownError.MaximumLength:
                    return string.Format(ErrorMessages.Default.ValidationMaximumLength, MaxLength);
                case ValueValidatorKnownError.MinimumValue:
                    return string.Format(ErrorMessages.Default.ValidationMinimumValue, MinValue);
                case ValueValidatorKnownError.MaximumValue:
                    return string.Format(ErrorMessages.Default.ValidationMaximumValue, MaxValue);
                case ValueValidatorKnownError.MinMaxLength:
                    return string.Format(
                        ErrorMessages.Default.ValidationMinMaxLength,
                        MinLength,
                        MaxLength);
                case ValueValidatorKnownError.ValueIsRequired:
                    return ErrorMessages.Default.ValidationValueIsRequired;
                case ValueValidatorKnownError.EMailIsExpected:
                    return ErrorMessages.Default.ValidationEMailIsExpected;
                default:
                    var defaultResult = ValidatorErrorText ?? DefaultValidatorErrorText;
                    return defaultResult ?? ErrorMessages.Default.ValidationInvalidFormat;
            }
        }

        /// <summary>
        /// Gets the length of the specified line, not including any trailing
        /// newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The length of the line, or -1 if lineNo was invalid.</returns>
        public virtual int GetLineLength(long lineNo)
        {
            return Handler.GetLineLength(lineNo);
        }

        /// <summary>
        /// Returns the contents of a given line in the text control, not
        /// including any trailing newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The contents of the line.</returns>
        public virtual string GetLineText(long lineNo)
        {
            return Handler.GetLineText(lineNo);
        }

        /// <summary>
        ///     Raises the <see cref="TextMaxLength"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextMaxLength(EventArgs e)
        {
            TextMaxLength?.Invoke(this, e);
        }

        /// <summary>
        ///     Raises the <see cref="TextUrl"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="UrlEventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextUrl(UrlEventArgs e)
        {
            // Under MacOs url parameter of the event data is always empty,
            // so event is not fired. Also on MacOs url is opened automatically.
            if (Application.IsMacOs)
                return;
            TextUrl?.Invoke(this, e);
            if (e.Cancel)
                return;
            if (AutoUrlOpen && !string.IsNullOrEmpty(e.Url))
                AppUtils.OpenUrl(e.Url!);
        }

        /// <summary>
        ///     Raises the <see cref="EnterPressed"/> event.
        /// </summary>
        /// <param name="e">
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnEnterPressed(EventArgs e)
        {
            EnterPressed?.Invoke(this, e);
        }

        /// <summary>
        /// Returns <see cref="TypeCode"/> for the <see cref="DataType"/> property
        /// or <see cref="TypeCode.String"/>.
        /// </summary>
        /// <returns></returns>
        public virtual TypeCode GetDataTypeCode()
        {
            if (DataType is null)
                return TypeCode.String;
            var typeCode = AssemblyUtils.GetRealTypeCode(DataType);
            return typeCode;
        }

        /// <summary>
        /// Returns <c>true</c> if <see cref="DataType"/> is a number type.
        /// </summary>
        /// <returns></returns>
        public virtual bool DataTypeIsNumber() => AssemblyUtils.IsTypeCodeNumber(GetDataTypeCode());

        /// <summary>
        /// Returns minimal and maximal possible values for the <see cref="DataType"/>
        /// as a range string and formats it using <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Range string format. Example: "Range is [{0}]."</param>
        /// <remarks>
        /// If <paramref name="format"/> is <c>null</c>, range string is returned unformatted.
        /// </remarks>
        public virtual string? GetMinMaxRangeStr(string? format = null)
        {
            var s = AssemblyUtils.GetMinMaxRangeStr(GetDataTypeCode(), format, MinValue, MaxValue);
            return s;
        }

        /// <summary>
        /// Returns the number of lines in the text control buffer.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The returned number is the number of logical lines, i.e.just the count of
        /// the number of newline characters in the control + 1, for GTK
        /// and OSX/Cocoa ports while it is the number of physical lines,
        /// i.e.the count of
        /// lines actually shown in the control, in MSW and OSX/Carbon.Because of
        /// this discrepancy, it is not recommended to use this function.
        /// </remarks>
        /// <remarks>
        /// Note that even empty text controls have one line (where the
        /// insertion point is), so this function never returns 0.
        /// </remarks>
        public virtual int GetNumberOfLines()
        {
            return Handler.GetNumberOfLines();
        }

        /// <summary>
        /// Converts given position to a zero-based column, line number pair.
        /// </summary>
        /// <param name="pos">Position in the text.</param>
        /// <returns>Point.X receives zero based column number. Point.Y receives
        /// zero based line number. If failure returns (-1,-1).</returns>
        public virtual Int32Point PositionToXY(long pos)
        {
            return Handler.PositionToXY(pos);
        }

        /// <summary>
        /// Converts given text position to client coordinates in pixels.
        /// </summary>
        /// <param name="pos">
        /// Text position in 0 to <see cref="GetLastPosition"/>
        /// range (inclusive).
        /// </param>
        /// <returns>
        /// On success returns a wxPoint which contains client
        /// coordinates for the given position in pixels, otherwise
        /// returns (-1,-1).
        /// </returns>
        /// <remarks>
        /// This function allows finding where is the character at the
        /// given position displayed in the text control.
        /// </remarks>
        /// <remarks>
        /// Availability: only available for the MSW, GTK ports.
        /// Additionally, GTK only implements this method for multiline
        /// controls and (-1,-1) is always returned for the single line ones.
        /// </remarks>
        public virtual Point PositionToCoords(long pos)
        {
            return Handler.PositionToCoords(pos);
        }

        /// <summary>
        /// Makes the line containing the given position visible.
        /// </summary>
        /// <param name="pos">The position that should be visible.</param>
        public virtual void ShowPosition(long pos)
        {
            Handler.ShowPosition(pos);
        }

        /// <summary>
        /// Converts the given zero based column and line number to a position.
        /// </summary>
        /// <param name="x">The column number.</param>
        /// <param name="y">The line number.</param>
        /// <returns>The position value, or -1 if x or y was invalid.</returns>
        public virtual long XYToPosition(long x, long y)
        {
            return Handler.XYToPosition(x, y);
        }

        /// <summary>
        /// Clears all text in the control.
        /// </summary>
        public virtual void Clear()
        {
            Handler.Clear();
        }

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Copy()
        {
            Handler.Copy();
        }

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        public virtual void Cut()
        {
            Handler.Cut();
        }

        /// <summary>
        /// Appends the text to the end of the text control.
        /// </summary>
        /// <param name="text">Text to write to the control.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendText(string text)
        {
            Handler.AppendText(text);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsSByte(sbyte value)
        {
            DataType ??= typeof(byte);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsByte(byte value)
        {
            DataType ??= typeof(byte);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsInt16(short value)
        {
            DataType ??= typeof(short);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsUInt16(ushort value)
        {
            DataType ??= typeof(ushort);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsInt32(int value)
        {
            DataType ??= typeof(int);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsUInt32(uint value)
        {
            DataType ??= typeof(uint);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsInt64(long value)
        {
            DataType ??= typeof(long);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsUInt64(ulong value)
        {
            DataType ??= typeof(ulong);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsSingle(float value)
        {
            DataType ??= typeof(float);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsDouble(double value)
        {
            DataType ??= typeof(double);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsDecimal(decimal value)
        {
            DataType ??= typeof(decimal);
            SetTextAsObject(value);
        }

        /// <inheritdoc cref="SetTextAsObject"/>
        public virtual void SetTextAsDateTime(DateTime value)
        {
            DataType ??= typeof(DateTime);
            SetTextAsObject(value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="string"/> and assigns <see cref="Text"/>
        /// property with the converted value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <remarks>
        /// This method uses <see cref="Converter"/>, <see cref="DefaultFormat"/> and
        /// <see cref="FormatProvider"/> propertties. Depending
        /// on the values of these properties, different conversion methods are used.
        /// </remarks>
        /// <remarks>
        /// If <see cref="DataType"/> property is <c>null</c>, it is set to
        /// the type of <paramref name="value"/>.
        /// </remarks>
        public virtual void SetTextAsObject(object? value)
        {
            if (value is null)
            {
                Text = string.Empty;
                return;
            }

            var type = value.GetType();
            DataType ??= type;
            var typeCode = AssemblyUtils.GetRealTypeCode(type);

            var converter = Converter ??
                ObjectToStringFactory.Default.GetConverter(typeCode);
            if (converter is null)
            {
                Text = value.ToString() ?? string.Empty;
                return;
            }

            if (DefaultFormat is null)
            {
                if (FormatProvider is null)
                {
                    Text = converter.ToString(value) ?? string.Empty;
                    return;
                }
                else
                {
                    Text = converter.ToString(value, FormatProvider) ?? string.Empty;
                    return;
                }
            }
            else
            {
                if (FormatProvider is null)
                {
                    Text = converter.ToString(value, DefaultFormat) ?? string.Empty;
                    return;
                }
                else
                {
                    Text = converter.ToString(value, DefaultFormat, FormatProvider) ?? string.Empty;
                    return;
                }
            }
        }

        /// <summary>
        /// Appends new line to the end of the text control.
        /// </summary>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendNewLine()
        {
            AppendText("\n");
        }

        /// <summary>
        /// Appends text and styles to the end of the text control.
        /// </summary>
        /// <param name="list">List containing strings or
        /// <see cref="ITextBoxTextAttr"/> instances.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual void AppendTextAndStyles(IEnumerable<object> list)
        {
            foreach (object item in list)
            {
                var ta = item as ITextBoxTextAttr;
                if (ta is not null)
                {
                    SetDefaultStyle(ta);
                    continue;
                }

                if (item != null)
                    AppendText(item.ToString()!);
            }
        }

        /// <summary>
        /// Returns the insertion point, or cursor, position.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// This is defined as the zero based index of the character position to the
        /// right of the insertion point. For example, if the insertion point is at
        /// the end of the single-line text control, it is equal to
        /// <see cref="GetLastPosition"/>.
        /// </remarks>
        /// <remarks>
        /// Notice that insertion position is, in general, different from the
        /// index of the character the cursor position at in the string returned
        /// by GetValue(). While this is always the case for the single line
        /// controls, multi-line controls can use two characters "\\r\\n" as
        /// line separator (this is notably the case under MSW) meaning that
        /// indices in the control and its string value are offset by 1 for
        /// every line.
        /// </remarks>
        public virtual long GetInsertionPoint()
        {
            return Handler.GetInsertionPoint();
        }

        /// <summary>
        /// Returns current character or an empty string.
        /// </summary>
        /// <returns></returns>
        public virtual string GetCurrentChar()
        {
            long pos = GetInsertionPoint();
            if (pos == GetLastPosition())
                return string.Empty;
            return GetRange(pos, pos + 1);
        }

        /// <summary>
        /// Replaces the current selection in the control with the
        /// contents of the Clipboard.
        /// </summary>
        public virtual void Paste()
        {
            Handler.Paste();
        }

        /// <summary>
        /// Redos the last edit operation in the control.
        /// </summary>
        public virtual void Redo()
        {
            Handler.Redo();
        }

        /// <summary>
        /// Removes the text starting at the first given position
        /// up to(but not including) the character at the last position.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <remarks>
        /// This function puts the current insertion point position at
        /// to as a side effect.
        /// </remarks>
        public virtual void Remove(long from, long to)
        {
            Handler.Remove(from, to);
        }

        /// <summary>
        /// Replaces the text starting at the first position up
        /// to(but not including) the character at the last position
        /// with the given text.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <param name="value">The value to replace the existing text with.</param>
        /// <remarks>
        /// This function puts the current insertion point position
        /// at to as a side effect.
        /// </remarks>
        public virtual void Replace(long from, long to, string value)
        {
            Handler.Replace(from, to, value);
        }

        /// <summary>
        /// Sets the insertion point at the given position.
        /// </summary>
        /// <param name="pos">Position to set, in the range from 0
        /// to <see cref="GetLastPosition"/> inclusive.</param>
        public virtual void SetInsertionPoint(long pos)
        {
            Handler.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point at the end of the text control.
        /// </summary>
        public virtual void SetInsertionPointEnd()
        {
            Handler.SetInsertionPointEnd();
        }

        /// <summary>
        /// Call this method in <see cref="Application.Idle"/> event handler
        /// in order to update information related to the current selection and caret position.
        /// </summary>
        public virtual void IdleAction()
        {
            if (CurrentPositionChanged is not null)
            {
                var currentPos = CurrentPosition;
                if (ReportedPosition != currentPos)
                {
                    ReportedPosition = currentPos;
                    CurrentPositionChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Selects the text starting at the first position up to
        /// (but not including) the character at the last position.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <remarks>
        /// If both parameters are equal to -1 all text in the control is selected.
        /// </remarks>
        /// <remarks>
        /// Notice that the insertion point will be moved to from by this function.
        /// </remarks>
        public virtual void SetSelection(long from, long to)
        {
            Handler.SetSelection(from, to);
        }

        /// <summary>
        /// Selects all text in the control.
        /// </summary>
        public virtual void SelectAll()
        {
            if (IsRichEdit)
            {
                DoInsideUpdate(() =>
                {
                    SetSelection(-1, -1);
                });
            }
            else
                Handler.SelectAll();
        }

        /// <summary>
        /// Clears selection. All text becomes unselected.
        /// </summary>
        public virtual void SelectNone()
        {
            Handler.SelectNone();
        }

        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public virtual void Undo()
        {
            Handler.Undo();
        }

        /// <summary>
        /// Writes the text into the text control at the current insertion position.
        /// </summary>
        /// <param name="text">Text to write to the text control.</param>
        /// <remarks>
        /// Newlines in the text string are the only special characters
        /// allowed, and they will cause appropriate line breaks.
        /// </remarks>
        /// <remarks>
        /// After the write operation, the insertion point will be at the
        /// end of the inserted text, so subsequent write operations will be appended.
        /// </remarks>
        /// <remarks>
        /// To append text after the user may have interacted with the control,
        /// call <see cref="SetInsertionPointEnd"/> before writing.
        /// </remarks>
        public virtual void WriteText(string text)
        {
            Handler.WriteText(text);
        }

        /// <summary>
        /// Returns the string containing the text starting in the
        /// positions from and up to to in the control.
        /// </summary>
        /// <param name="from">The first position.</param>
        /// <param name="to">The last position.</param>
        /// <returns>
        /// <see cref="string"/> containing the text from the first
        /// to the last position.
        /// </returns>
        /// <remarks>
        /// The positions must have been returned by another control method.
        /// Please note that the positions in a multiline text control do not
        /// correspond to the indices in the string returned by Value because of the
        /// different new line representations(CR or CR LF). This method should be
        /// used to obtain the correct results instead of extracting parts of
        /// the entire value. It may also be more efficient, especially if the
        /// control contains a lot of data.
        /// </remarks>
        public virtual string GetRange(long from, long to)
        {
            return Handler.GetRange(from, to);
        }

        /// <summary>
        /// Gets the text currently selected in the control.
        /// </summary>
        /// <returns></returns>
        public virtual string GetStringSelection()
        {
            return Handler.GetStringSelection();
        }

        /// <summary>
        /// Clears internal undo buffer. No undo operations are available
        /// after this operation.
        /// </summary>
        public virtual void EmptyUndoBuffer()
        {
            Handler.EmptyUndoBuffer();
        }

        /// <summary>
        /// Return true if the given position is valid, i.e. positive
        /// and less than the last position.
        /// </summary>
        /// <param name="pos">Position to check.</param>
        /// <returns></returns>
        public virtual bool IsValidPosition(long pos)
        {
            return Handler.IsValidPosition(pos);
        }

        /// <summary>
        /// Returns the zero based index of the last position in
        /// the text control, which is equal to the number of characters
        /// in the control.
        /// </summary>
        /// <returns></returns>
        public virtual long GetLastPosition()
        {
            return Handler.GetLastPosition();
        }

        /// <summary>
        /// Gets the current selection start position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionStart()
        {
            return Handler.GetSelectionStart();
        }

        /// <summary>
        /// Gets the current selection end position.
        /// </summary>
        /// <returns></returns>
        public virtual long GetSelectionEnd()
        {
            return Handler.GetSelectionEnd();
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls
        /// <see cref="OnTextChanged"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        public void RaiseTextChanged(TextChangedEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnTextChanged(e);
        }

        /// <summary>
        /// Moves caret to the beginning of the text.
        /// </summary>
        public virtual void MoveToBeginOfText()
        {
            SetInsertionPoint(0);
        }

        /// <summary>
        /// Moves caret to the end of the text.
        /// </summary>
        public virtual void MoveToEndOfText()
        {
            SetInsertionPointEnd();
        }

        /// <summary>
        /// Returns the style currently used for the new text.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual ITextBoxTextAttr GetDefaultStyle()
        {
            return new TextBoxTextAttr(Handler.GetDefaultStyle());
        }

        /// <summary>
        /// Returns the style at this position in the text control.
        /// </summary>
        /// <param name="pos">The position for which text style is returned.</param>
        /// <returns></returns>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual ITextBoxTextAttr GetStyle(long pos)
        {
            return new TextBoxTextAttr(Handler.GetStyle(pos));
        }

        /// <summary>
        /// Changes the default style to use for the new text which is
        /// going to be added to the control using <see cref="WriteText"/>
        /// or <see cref="AppendText"/>.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// true on success, false if an error occurred(this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If either of the font, foreground, or background colour is not set in
        /// style, the values of the previous default style are used for them. If the
        /// previous default style didn't set them neither, the global font or colors
        /// of the text control itself are used as fall back.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return Handler.SetDefaultStyle(s.Handle);
        }

        /// <summary>
        /// Changes the style of the given range.
        /// </summary>
        /// <param name="start">The start of the range to change.</param>
        /// <param name="end">The end of the range to change.</param>
        /// <param name="style">The new style for the range.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        /// <remarks>
        /// Turn on <see cref="TextBox.IsRichEdit"/> in order to use this method.
        /// </remarks>
        public virtual bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return Handler.SetStyle(start, end, s.Handle);
        }

        /// <summary>
        /// Reports text validation error.
        /// </summary>
        /// <param name="showError">Indicates whether to show/hide error.</param>
        /// <param name="errorText">Specifies error text.</param>
        /// <remarks>
        /// Uses <see cref="DefaultErrorBackgroundColor"/>, <see cref="DefaultErrorForegroundColor"/>,
        /// <see cref="ValidatorErrorText"/>, <see cref="DefaultValidatorErrorText"/> and
        /// <see cref="ValidatorReporter"/> properties.
        /// </remarks>
        /// <remarks>
        /// <see cref="ValidatorReporter"/> property must support <see cref="IValidatorReporter"/>
        /// interface in order to be used in this method. <see cref="PictureBox"/> supports
        /// this interface.
        /// </remarks>
        public virtual void ReportValidatorError(bool showError, string? errorText = null)
        {
            var hint = string.Empty;
            if (!showError)
            {
                if (DefaultErrorUseBackgroundColor)
                    ResetBackgroundColor();
                if (DefaultErrorUseForegroundColor)
                    ResetForegroundColor();
            }
            else
            {
                if (DefaultErrorUseBackgroundColor)
                    BackgroundColor = DefaultErrorBackgroundColor;
                if (DefaultErrorUseForegroundColor)
                    ForegroundColor = DefaultErrorForegroundColor;
                hint = errorText ?? ValidatorErrorText;
                hint ??= DefaultValidatorErrorText;
            }

            Report(ValidatorReporter as IValidatorReporter);
            Report(this);

            void Report(IValidatorReporter? reporter)
            {
                reporter?.SetErrorStatus(this, showError, hint);
            }
        }

        /// <summary>
        /// Executes a browser command with the specified name and parameters.
        /// </summary>
        /// <param name = "cmdName" >
        /// Name of the command to execute.
        /// </param>
        /// <param name = "args" >
        /// Parameters of the command.
        /// </param>
        /// <returns>
        /// An <see cref="object"/> representing the result of the command execution.
        /// </returns>
        public virtual object? DoCommand(string cmdName, params object?[] args)
        {
            if (cmdName == "GetReportedUrl")
            {
                return Handler.ReportedUrl;
            }

            return null;
        }

        /// <summary>
        /// Sets text alignment in the current position to <paramref name="alignment"/>.
        /// </summary>
        /// <param name="alignment">New alignment value.</param>
        public virtual void SelectionSetAlignment(TextBoxTextAttrAlignment alignment)
        {
            var fs = CreateTextAttr();
            fs.SetAlignment(alignment);
            SetSelectionStyle(fs);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Center"/>
        /// </summary>
        public virtual void SelectionAlignCenter()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Center);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Left"/>
        /// </summary>
        public virtual void SelectionAlignLeft()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Left);
        }

        /// <summary>
        /// Sets <see cref="ValidatorErrorText"/> property to <paramref name="knownError"/>.
        /// </summary>
        /// <param name="knownError">Known error identifier.</param>
        public virtual void SetErrorText(ValueValidatorKnownError knownError)
        {
            ValidatorErrorText = GetKnownErrorText(knownError);
        }

        /// <summary>
        /// Handles default rich text editor keys.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        /// <remarks>
        /// You can use this method in the <see cref="UIElement.KeyDown"/> event handlers.
        /// </remarks>
        public virtual void HandleRichEditKeys(KeyEventArgs e)
        {
            if (KeyInfo.Run(KnownKeys.RichEditKeys.SelectAll, e, SelectAll))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleBold, e, ToggleSelectionBold))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleItalic, e, ToggleSelectionItalic))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleUnderline, e, ToggleSelectionUnderline))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ToggleStrikethrough, e, ToggleSelectionStrikethrough))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.LeftAlign, e, SelectionAlignLeft))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.CenterAlign, e, SelectionAlignCenter))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.RightAlign, e, SelectionAlignRight))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.Justify, e, SelectionJustify))
                return;
            if (KeyInfo.Run(KnownKeys.RichEditKeys.ClearTextFormatting, e, ClearTextFormatting))
                return;
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Right"/>
        /// </summary>
        public virtual void SelectionAlignRight()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Right);
        }

        /// <summary>
        /// Sets text alignment in the current position to <see cref="TextBoxTextAttrAlignment.Justified"/>
        /// </summary>
        public virtual void SelectionJustify()
        {
            SelectionSetAlignment(TextBoxTextAttrAlignment.Justified);
        }

        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            RaiseEvent(e);
            if (Options.HasFlag(TextBoxOptions.DefaultValidation))
                RunDefaultValidation();
        }

        /*
        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }*/

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeTextBoxHandler();
        }

        private static object CoerceText(DependencyObject d, object value) =>
            value ?? string.Empty;

        /// <summary>
        /// Callback for changes to the Text property
        /// </summary>
        private static void OnTextPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = (TextBox)d;
            textBox.OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}