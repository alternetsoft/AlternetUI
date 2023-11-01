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
    public class TextBox : Control
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

        private bool hasBorder = true;
        private bool multiline = false;
        private bool readOnly = false;
        private bool isRichEdit = false;
        private TextBoxTextWrap textWrap;
        private GenericAlignment textAlign;
        private IValueValidator? validator;
        private int maxLength;
        private int minLength;
        private TextBoxOptions options;

        static TextBox()
        {
            var choices = PropertyGrid.CreateChoices();
            choices.Add(GenericAlignment.Left);
            choices.Add(GenericAlignment.Right);
            choices.Add(GenericAlignment.CenterHorizontal);
            var prm = PropertyGrid.GetNewItemParams(typeof(TextBox), nameof(TextBox.TextAlign));
            prm.EnumIsFlags = false;
            prm.Choices = choices;
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
        public event EventHandler? TextUrl;

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

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>A string containing the text contents of the text box. The
        /// default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the
        /// text box. Setting this property replaces the contents of the text box
        /// with the specified string.
        /// </remarks>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public string Text
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
        /// Gets or sets a value indicating whether text control is in rich edit mode
        /// </summary>
        /// <remarks>
        /// In the rich edit mode it is possible to apply text formatting (for example
        /// change text font or color). Also it is possible to edit large texts.
        /// </remarks>
        public bool IsRichEdit
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
        public TextBoxTextWrap TextWrap
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
        public GenericAlignment TextAlign
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
        public bool ReadOnly
        {
            get
            {
                CheckDisposed();
                return readOnly;
            }

            set
            {
                CheckDisposed();
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
        public int MaxLength
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
                if(options.HasFlag(TextBoxOptions.SetNativeMaxLength))
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
        public int MinLength
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
        /// Gets or sets a value indicating whether this is a multiline text
        /// box control.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the control is a multiline text box
        /// control; otherwise, <see langword="false"/>. The default
        /// is <see langword="false"/>.
        /// </value>
        public bool Multiline
        {
            get
            {
                CheckDisposed();
                return multiline;
            }

            set
            {
                CheckDisposed();
                if (multiline == value)
                    return;

                multiline = value;
                MultilineChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public bool HasBorder
        {
            get
            {
                CheckDisposed();
                return hasBorder;
            }

            set
            {
                CheckDisposed();
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
        public bool HasSelection
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
        public bool IsModified
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
        public object? TextAsNumber
        {
            get
            {
                var typeCode = GetDataTypeCode();
                if (!AssemblyUtils.IsNumberTypeCode(typeCode))
                    return null;

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
        public bool CanCopy
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
        public bool CanCut
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
        public bool CanPaste
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
        public bool CanRedo
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
        public bool HideSelection
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
        public bool ProcessTab
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
        public bool ProcessEnter
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
        public bool IsPassword
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
        public bool AutoUrl
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
        public bool HideVertScrollbar
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
        /// Gets a value indicating whether the user can undo the previous operation
        /// in the control.
        /// </summary>
        /// <returns>
        /// <see langword = "true" /> if the user can undo the previous
        /// operation performed
        /// in the control; otherwise, <see langword = "false" />.
        /// </returns >
        [Browsable(false)]
        public bool CanUndo
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
        public bool IsEmpty
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
        public TextBoxOptions Options
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
        public string? EmptyTextHint
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
        public object? MinValue { get; set; }

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
        public object? MaxValue { get; set; }

        internal new NativeTextBoxHandler Handler =>
            (NativeTextBoxHandler)base.Handler;

        /// <summary>
        /// Creates new custom style.
        /// </summary>
        /// <returns></returns>
        public static ITextBoxTextAttr CreateTextAttr()
        {
            return new TextBoxTextAttr();
        }

        /// <inheritdoc cref="ValueValidatorFactory.CreateValidator"/>
        public static IValueValidatorText CreateValidator(ValueValidatorKind kind)
        {
            return ValueValidatorFactory.CreateValidator(kind);
        }

        /// <summary>
        /// Gets known error text.
        /// </summary>
        /// <param name="kind">Error kind.</param>
        public string GetKnownErrorText(ValueValidatorKnownError kind)
        {
            switch (kind)
            {
                case ValueValidatorKnownError.NumberIsExpected:
                    var rangeStr = GetMinMaxRangeStr(ErrorMessages.Default.ValidationRangeFormat);
                    return $"{ErrorMessages.Default.ValidationNumberIsExpected} {rangeStr}".Trim();
                case ValueValidatorKnownError.FloatIsExpected:
                    return ErrorMessages.Default.ValidationFloatIsExpected;
                case ValueValidatorKnownError.HexNumberIsExpected:
                    return ErrorMessages.Default.ValidationHexNumberIsExpected;
                case ValueValidatorKnownError.InvalidFormat:
                    return ErrorMessages.Default.ValidationInvalidFormat;
                case ValueValidatorKnownError.MinimumLength:
                    return string.Format(ErrorMessages.Default.ValidationMinimumLength, MinLength);
                case ValueValidatorKnownError.MaximumLength:
                    return string.Format(ErrorMessages.Default.ValidationMaximumLength, MaxLength);
                case ValueValidatorKnownError.MinMaxLength:
                    return string.Format(
                        ErrorMessages.Default.ValidationMinMaxLength,
                        MinLength,
                        MaxLength);
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
        public int GetLineLength(long lineNo)
        {
            return Handler.GetLineLength(lineNo);
        }

        /// <summary>
        /// Returns the contents of a given line in the text control, not
        /// including any trailing newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The contents of the line.</returns>
        public string GetLineText(long lineNo)
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
        ///     An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        public virtual void OnTextUrl(EventArgs e)
        {
            TextUrl?.Invoke(this, e);
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
        public virtual bool DataTypeIsNumber() => AssemblyUtils.IsNumberTypeCode(GetDataTypeCode());

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
        public int GetNumberOfLines()
        {
            return Handler.GetNumberOfLines();
        }

        /// <summary>
        /// Converts given position to a zero-based column, line number pair.
        /// </summary>
        /// <param name="pos">Position in the text.</param>
        /// <returns>Point.X receives zero based column number. Point.Y receives
        /// zero based line number. If failure returns (-1,-1).</returns>
        public Point PositionToXY(long pos)
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
        public Point PositionToCoords(long pos)
        {
            return Handler.PositionToCoords(pos);
        }

        /// <summary>
        /// Makes the line containing the given position visible.
        /// </summary>
        /// <param name="pos">The position that should be visible.</param>
        public void ShowPosition(long pos)
        {
            Handler.ShowPosition(pos);
        }

        /// <summary>
        /// Converts the given zero based column and line number to a position.
        /// </summary>
        /// <param name="x">The column number.</param>
        /// <param name="y">The line number.</param>
        /// <returns>The position value, or -1 if x or y was invalid.</returns>
        public long XYToPosition(long x, long y)
        {
            return Handler.XYToPosition(x, y);
        }

        /// <summary>
        /// Clears all text in the control.
        /// </summary>
        public void Clear()
        {
            Handler.Clear();
        }

        /// <summary>
        /// Copies the current selection in the control to the Clipboard.
        /// </summary>
        public void Copy()
        {
            Handler.Copy();
        }

        /// <summary>
        /// Moves the current selection in the control to the Clipboard.
        /// </summary>
        public void Cut()
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
        public void AppendText(string text)
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
        public virtual void SetTextAsInt(int value)
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
        public void AppendNewLine()
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
        public void AppendTextAndStyles(IEnumerable<object> list)
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
        public long GetInsertionPoint()
        {
            return Handler.GetInsertionPoint();
        }

        /// <summary>
        /// Returns current character or an empty string.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentChar()
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
        public void Paste()
        {
            Handler.Paste();
        }

        /// <summary>
        /// Redos the last edit operation in the control.
        /// </summary>
        public void Redo()
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
        public void Remove(long from, long to)
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
        public void Replace(long from, long to, string value)
        {
            Handler.Replace(from, to, value);
        }

        /// <summary>
        /// Sets the insertion point at the given position.
        /// </summary>
        /// <param name="pos">Position to set, in the range from 0
        /// to <see cref="GetLastPosition"/> inclusive.</param>
        public void SetInsertionPoint(long pos)
        {
            Handler.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point at the end of the text control.
        /// </summary>
        public void SetInsertionPointEnd()
        {
            Handler.SetInsertionPointEnd();
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
        public void SetSelection(long from, long to)
        {
            Handler.SetSelection(from, to);
        }

        /// <summary>
        /// Selects all text in the control.
        /// </summary>
        public void SelectAll()
        {
            Handler.SelectAll();
        }

        /// <summary>
        /// Clears selection. All text becomes unselected.
        /// </summary>
        public void SelectNone()
        {
            Handler.SelectNone();
        }

        /// <summary>
        /// Undoes the last edit operation in the control.
        /// </summary>
        public void Undo()
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
        public void WriteText(string text)
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
        /// <see cref="string"/> containing the texct from the first
        /// to the last position.
        /// </returns>
        /// <remarks>
        /// The positions must have been returned by another TextBox method.
        /// Please note that the positions in a multiline text control do not
        /// correspond to the indices in the string returned by Value because of the
        /// different new line representations(CR or CR LF). This method should be
        /// used to obtain the correct results instead of extracting parts of
        /// the entire value. It may also be more efficient, especially if the
        /// control contains a lot of data.
        /// </remarks>
        public string GetRange(long from, long to)
        {
            return Handler.GetRange(from, to);
        }

        /// <summary>
        /// Gets the text currently selected in the control.
        /// </summary>
        /// <returns></returns>
        public string GetStringSelection()
        {
            return Handler.GetStringSelection();
        }

        /// <summary>
        /// Clears internal undo buffer. No undo operations are available
        /// after this operation.
        /// </summary>
        public void EmptyUndoBuffer()
        {
            Handler.EmptyUndoBuffer();
        }

        /// <summary>
        /// Return true if the given position is valid, i.e. positive
        /// and less than the last position.
        /// </summary>
        /// <param name="pos">Position to check.</param>
        /// <returns></returns>
        public bool IsValidPosition(long pos)
        {
            return Handler.IsValidPosition(pos);
        }

        /// <summary>
        /// Returns the zero based index of the last position in
        /// the text control, which is equal to the number of characters
        /// in the control.
        /// </summary>
        /// <returns></returns>
        public long GetLastPosition()
        {
            return Handler.GetLastPosition();
        }

        /// <summary>
        /// Gets the current selection start position.
        /// </summary>
        /// <returns></returns>
        public long GetSelectionStart()
        {
            return Handler.GetSelectionStart();
        }

        /// <summary>
        /// Gets the current selection end position.
        /// </summary>
        /// <returns></returns>
        public long GetSelectionEnd()
        {
            return Handler.GetSelectionEnd();
        }

        /// <summary>
        /// Raises the <see cref="TextChanged"/> event and calls
        /// <see cref="OnTextChanged(EventArgs)"/>.
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
        public void MoveToBeginOfText()
        {
            SetInsertionPoint(0);
        }

        /// <summary>
        /// Moves caret to the end of the text.
        /// </summary>
        public void MoveToEndOfText()
        {
            SetInsertionPointEnd();
        }

        /// <summary>
        /// Returns the style currently used for the new text.
        /// </summary>
        /// <returns></returns>
        public ITextBoxTextAttr GetDefaultStyle()
        {
            return new TextBoxTextAttr(Handler.GetDefaultStyle());
        }

        /// <summary>
        /// Returns the style at this position in the text control.
        /// </summary>
        /// <param name="pos">The position for which text style is returned.</param>
        /// <returns></returns>
        public ITextBoxTextAttr GetStyle(long pos)
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
        public bool SetDefaultStyle(ITextBoxTextAttr style)
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
        /// true on success, false if an error occurred (this may also
        /// mean that the styles are not supported under this platform).
        /// </returns>
        /// <remarks>
        /// If any attribute within style is not set, the corresponding
        /// attribute from <see cref="GetDefaultStyle"/> is used.
        /// </remarks>
        public bool SetStyle(long start, long end, ITextBoxTextAttr style)
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
        public void ReportValidatorError(bool showError, string? errorText = null)
        {
            var hint = string.Empty;
            if (!showError)
            {
                ResetBackgroundColor();
                ResetForegroundColor();
            }
            else
            {
                BackgroundColor = DefaultErrorBackgroundColor;
                ForegroundColor = DefaultErrorForegroundColor;
                hint = errorText ?? ValidatorErrorText;
                hint ??= DefaultValidatorErrorText;
            }

            var reporter = ValidatorReporter as IValidatorReporter;
            reporter?.SetErrorStatus(this, showError, hint);
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
#pragma warning disable IDE0060 // Remove unused parameter
        public object? DoCommand(string cmdName, params object?[] args)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            if (cmdName == "GetReportedUrl")
            {
                return Handler.ReportedUrl;
            }

            return null;
        }

        /// <summary>
        /// Called when content in this Control changes.
        /// Raises the TextChanged event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTextChanged(TextChangedEventArgs e)
        {
            RaiseEvent(e);
        }

        /// <summary>
        /// Called when the value of the <see cref="Text"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnTextChanged(EventArgs e)
        {
        }

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
            textBox.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private void OnTextPropertyChanged(string oldText, string newText)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            OnTextChanged(new TextChangedEventArgs(TextChangedEvent));
        }
    }
}