using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for text editors.
    /// </summary>
    [ControlCategory("Hidden")]
    public abstract partial class CustomTextBox
        : Control, ICustomTextBox, IReadOnlyStrings, IValidatorReporter, IObjectToStringOptions,
        INotifyDataErrorInfo
    {
        private const TextBoxOptions DefaultOptions = TextBoxOptions.IntRangeInError;
        private const bool DefaultAllowEmptyText = true;

        [DefaultValue(DefaultAllowEmptyText)]
        [AutoReset]
        private bool allowEmptyText = DefaultAllowEmptyText;

        [AutoReset]
        [DefaultValue(DefaultOptions)]
        private TextBoxOptions options = DefaultOptions;

        [AutoReset]
        private CultureInfo? culture;

        [AutoReset]
        private TypeConverter? typeConverter;

        [AutoReset]
        private ITypeDescriptorContext? context;

        [AutoReset]
        private IObjectToString? converter;

        [AutoReset]
        private string? defaultFormat;

        [AutoReset]
        private NumberStyles? numberStyles;

        [AutoReset(false)]
        private IValidatorReporter? validatorReporter;

        [AutoReset]
        private string? validatorErrorText;

        [AutoReset]
        private object? emptyTextValue;

        [AutoReset]
        private int minLength;

        [AutoReset]
        private int maxLength;

        [AutoReset]
        private KnownInputType? inputType;

        [AutoReset]
        private object? minValue;

        [AutoReset]
        private object? maxValue;

        [AutoReset]
        private Type? dataType;

        [AutoReset]
        private string? defaultText;

        [AutoReset]
        private int reportedErrorCount;

        [AutoReset]
        private StringSearch? search;

        [AutoReset]
        private Exception? textAsValueError;

        [AutoReset]
        private TrimTextRules trimTextRules;

        [AutoReset]
        private TextBoxInitializeEventArgs? inputTypeArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextBox"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public CustomTextBox(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTextBox"/> class.
        /// </summary>
        public CustomTextBox()
        {
        }

        /// <summary>
        /// Occurs when <see cref="string"/> is converted to value. This is static event
        /// and is called for all the editors.
        /// </summary>
        public static event EventHandler<StringToObjectEventArgs>? GlobalStringToValue;

        /// <summary>
        /// Occurs when value is converted to <see cref="string"/>. This is static event
        /// and is called for all the editors.
        /// </summary>
        public static event EventHandler<ObjectToStringEventArgs>? GlobalValueToString;

        /// <summary>
        /// Occurs when <see cref="string"/> is converted to value in this control.
        /// </summary>
        public static event EventHandler<StringToObjectEventArgs>? StringToValue;

        /// <summary>
        /// Occurs when value is converted to <see cref="string"/> in this control.
        /// </summary>
        public static event EventHandler<ObjectToStringEventArgs>? ValueToString;

        /// <summary>
        /// Occurs when <see cref="ReportValidatorError"/> is called.
        /// </summary>
        /// <remarks>
        /// You can handle this event in order to show validation error information.
        /// </remarks>
        public event ErrorStatusEventHandler? ErrorStatusChanged;

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
        /// application needs to report user an error in
        /// <see cref="AbstractControl.Text"/> property.
        /// </summary>
        public static Color DefaultErrorBackgroundColor { get; set; } = Color.Red;

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a foreground color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="AbstractControl.Text"/> property.
        /// </summary>
        public static Color DefaultErrorForegroundColor { get; set; } = Color.White;

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorForegroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether to use <see cref="DefaultErrorBackgroundColor"/>
        /// when application needs to report user an error.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>.
        /// Do not set <c>true</c> on MacOs as it is not supported.
        /// </remarks>
        public static bool DefaultErrorUseBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets default value for the <see cref="ResetErrorBackgroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorBackgroundMethod { get; set; }
            = ResetColorType.Auto;

        /// <summary>
        /// Gets or sets default value for the <see cref="ResetErrorForegroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorForegroundMethod { get; set; }
            = ResetColorType.Auto;

        /// <inheritdoc/>
        public override bool HasErrors
        {
            get
            {
                return GetErrors().Any();
            }
        }

        /// <summary>
        /// Gets or sets default value for the <see cref="AbstractControl.Text"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual string? DefaultText
        {
            get => defaultText;
            set => defaultText = value;
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
        /// when it already is filled up to the maximal length, 'TextMaxLength'
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
                value = Math.Max(0, value);
                if (maxLength == value)
                    return;
                maxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets validator reporter object or control.
        /// </summary>
        /// <remarks>
        /// This property can be used to store reference to control that
        /// reports validation or other errors to the end users. Usually
        /// this is a <see cref="PictureBox"/> with error image.
        /// </remarks>
        [Browsable(false)]
        public virtual IValidatorReporter? ValidatorReporter
        {
            get => validatorReporter;
            set => validatorReporter = value;
        }

        /// <summary>
        /// Gets or sets a text string that can be used as validator error message.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual string? ValidatorErrorText
        {
            get => validatorErrorText;
            set => validatorErrorText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether empty string
        /// is allowed in <see cref="AbstractControl.Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AllowEmptyText
        {
            get => allowEmptyText;
            set => allowEmptyText = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// <see cref="AbstractControl.Text"/> is required to be not empty.
        /// This is an opposite of <see cref="AllowEmptyText"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>false</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsRequired
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
        /// Gets or sets whether error reporter is automatically shown/hidden when
        /// error state is changed.
        /// </summary>
        public virtual bool AutoShowError { get; set; } = false;

        /// <summary>
        /// Gets or sets data value in cases when <see cref="AbstractControl.Text"/>
        /// property is empty.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual object? EmptyTextValue
        {
            get => emptyTextValue;
            set => emptyTextValue = value;
        }

        /// <summary>
        /// Gets or sets string search provider.
        /// </summary>
        [Browsable(false)]
        public virtual StringSearch Search
        {
            get => search ??= new(this);

            set
            {
                if (value is null)
                    return;
                search = value;
            }
        }

        /// <summary>
        /// Gets or sets method which is used to clear error state
        /// if error background color is used for reporting it.
        /// </summary>
        [Browsable(false)]
        public virtual ResetColorType? ResetErrorBackgroundMethod { get; set; }

        /// <summary>
        /// Gets or sets method which is used to clear error state
        /// if error foreground color is used for reporting it.
        /// </summary>
        [Browsable(false)]
        public virtual ResetColorType? ResetErrorForegroundMethod { get; set; }

        /// <summary>
        /// Gets whether <see cref="AbstractControl.Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="AbstractControl.Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);

        bool? IObjectToStringOptions.UseInvariantCulture
        {
            get => Options.HasFlag(TextBoxOptions.UseInvariantCulture);

            set
            {
                if (value ?? false)
                    Options |= TextBoxOptions.UseInvariantCulture;
                else
                    Options &= ~TextBoxOptions.UseInvariantCulture;
            }
        }

        /// <summary>
        /// Gets last error occurred inside <see cref="TextAsValue"/> property getter or setter.
        /// </summary>
        [Browsable(false)]
        public virtual Exception? TextAsValueError
        {
            get => textAsValueError;
            private set => textAsValueError = value;
        }

        /// <summary>
        /// Gets or sets text trimming rules used in <see cref="TextAsValue"/> setter and some
        /// other places.
        /// </summary>
        public virtual TrimTextRules TrimTextRules
        {
            get => trimTextRules;
            set => trimTextRules = value;
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.Text"/>
        /// as <see cref="object"/> using <see cref="DataType"/>, <see cref="TypeConverter"/>
        /// and other properties which define text to/from value conversion rules.
        /// </summary>
        [Browsable(false)]
        public virtual object? TextAsValue
        {
            get
            {
                TextAsValueError = null;

                try
                {
                    if (TextToValueWithEvent(out object? result))
                        return result;

                    if (DataType is null || DataType == typeof(string))
                        return Text;

                    var typeConverter = TypeConverter ??
                        StringConverters.Default.GetTypeConverter(DataType);

                    if (typeConverter is null)
                        return null;

                    var isBaseTypeConverter = typeConverter.GetType() == typeof(TypeConverter);

                    if (isBaseTypeConverter)
                        return null;

                    result = StringUtils.ParseWithTypeConverter(
                                Text,
                                typeConverter,
                                Context,
                                Culture,
                                Options.HasFlag(TextBoxOptions.UseInvariantCulture));

                    return result;
                }
                catch (Exception e)
                {
                    TextAsValueError = e;
                    return EmptyTextValue;
                }
            }

            set
            {
                TextAsValueError = null;

                try
                {
                    var s = ObjectToString(value, Options | TextBoxOptions.UseTypeConverter);

                    var trimmed = StringUtils.Trim(s, TrimTextRules);

                    Text = trimmed ?? string.Empty;
                }
                catch (Exception e)
                {
                    Text = string.Empty;
                    TextAsValueError = e;
                }
            }
        }

        /// <summary>
        /// Gets <see cref="AbstractControl.Text"/> property value as <see cref="int"/>.
        /// </summary>
        [Browsable(false)]
        public virtual int TextAsInt32
        {
            get
            {
                try
                {
                    var resultAsNumber = TextAsNumber;

                    if (resultAsNumber is null)
                        return 0;

                    int result = Convert.ToInt32(resultAsNumber);
                    return result;
                }
                catch (Exception ex)
                {
                    Nop(ex);
                    return 0;
                }
            }

            set
            {
                SetTextAsInt32(value);
            }
        }

        /// <summary>
        /// Gets <see cref="AbstractControl.Text"/> property value as <see cref="long"/>.
        /// </summary>
        [Browsable(false)]
        public virtual long TextAsInt64
        {
            get
            {
                try
                {
                    var resultAsNumber = TextAsNumber;

                    if (resultAsNumber is null)
                        return 0;

                    var result = Convert.ToInt64(resultAsNumber);
                    return result;
                }
                catch (Exception ex)
                {
                    Nop(ex);
                    return 0;
                }
            }

            set
            {
                SetTextAsInt64(value);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="AbstractControl.Text"/> property value
        /// as <see cref="object"/> of a number type.
        /// </summary>
        /// <remarks>
        /// If <see cref="AbstractControl.Text"/> property doesn't contain
        /// a number value, <c>null</c> is returned.
        /// </remarks>
        [Browsable(false)]
        public virtual object? TextAsNumber
        {
            get
            {
                if (TextToValueWithEvent(out object? convertedValue))
                    return convertedValue;

                if (!IsNumber)
                {
                    var converted = StringUtils.TryParseNumberWithDelegates(
                                Text,
                                NumberStyles ?? System.Globalization.NumberStyles.Any,
                                FormatProvider,
                                out var result,
                                StringUtils.TryParseNumberDelegates);
                    if (converted)
                        return result;
                }

                var isOk = StringUtils.TryParseNumber(
                    GetDataTypeCode(),
                    Text,
                    NumberStyles,
                    FormatProvider,
                    out convertedValue);
                if (isOk)
                    return convertedValue;
                else
                    return null;
            }

            set
            {
                SetTextAsObject(value);
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
        [Browsable(false)]
        public virtual object? MinValue
        {
            get => minValue;

            set
            {
                minValue = value;
            }
        }

        /// <summary>
        /// Gets or sets init arguments which are used when <see cref="InputType"/>
        /// property is assigned.
        /// </summary>
        [Browsable(false)]
        public virtual TextBoxInitializeEventArgs? InputTypeArgs
        {
            get => inputTypeArgs;
            set => inputTypeArgs = value;
        }

        /// <summary>
        /// Gets or sets input type. Default is Null.
        /// </summary>
        public virtual KnownInputType? InputType
        {
            get
            {
                return inputType;
            }

            set
            {
                if (inputType == value)
                    return;
                inputType = value;
                TextBoxInitializers.Default.Initialize(
                    this,
                    value ?? KnownInputType.None,
                    InputTypeArgs);
                inputType = value;
            }
        }

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
        [Browsable(false)]
        public virtual object? MaxValue
        {
            get => maxValue;
            set => maxValue = value;
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
        /// Gets or sets the minimum number of characters
        /// the user must enter into the control.
        /// </summary>
        /// <remarks>
        /// Currently this property doesn't affect control's behavior.
        /// You can implement your own validation rules using 'TextChanged' event.
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

        int IReadOnlyStrings.Count => GetNumberOfLines();

        [Browsable(false)]
        internal new LayoutStyle? Layout
        {
            get => base.Layout;
            set => base.Layout = value;
        }

        string? IReadOnlyStrings.this[int index]
        {
            get
            {
                try
                {
                    return GetLineText(index);
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes <see cref="PictureBox"/> with error image and other options.
        /// </summary>
        /// <param name="picture"></param>
        public static void InitErrorPicture(PictureBox picture)
        {
            picture.Image = KnownColorSvgImages.GetErrorImage();
            picture.VerticalAlignment = UI.VerticalAlignment.Center;
            picture.ImageVisible = false;
            picture.ImageStretch = false;
            picture.TabStop = false;
            picture.Margin = (KnownMetrics.ControlLabelDistance, 1, 1, 1);
            picture.ParentBackColor = true;

            picture.MouseLeftButtonUp -= Picture_MouseLeftButtonUp;
            picture.MouseLeftButtonUp += Picture_MouseLeftButtonUp;

            static void Picture_MouseLeftButtonUp(object? sender, MouseEventArgs e)
            {
                if (sender is not PictureBox pictureBox)
                    return;

                pictureBox.HideToolTip();

                ToolTipFactory.ShowToolTip(
                    pictureBox,
                    null,
                    pictureBox.ToolTip,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Sets <see cref="CustomTextBox.ValidatorErrorText"/> property
        /// to <paramref name="knownError"/>.
        /// </summary>
        /// <param name="knownError">Known error identifier.</param>
        public virtual void SetErrorText(ValueValidatorKnownError knownError)
        {
            ValidatorErrorText = GetKnownErrorText(knownError);
        }

        /// <summary>
        /// Sets <see cref="CustomTextBox.ValidatorErrorText"/>
        /// with default error text for the data type specified in <see cref="DataType"/>.
        /// </summary>
        public virtual void SetErrorTextFromDataType()
        {
            if(DataType is null)
            {
                ValidatorErrorText = null;
                return;
            }

            if (IsHexNumber)
            {
                SetErrorText(ValueValidatorKnownError.HexNumberIsExpected);
                return;
            }

            if (IsFloat)
            {
                if (IsSignedNumber)
                    SetErrorText(ValueValidatorKnownError.FloatIsExpected);
                else
                    SetErrorText(ValueValidatorKnownError.UnsignedFloatIsExpected);
                return;
            }

            if (IsUnsignedInt)
            {
                SetErrorText(ValueValidatorKnownError.UnsignedNumberIsExpected);
                return;
            }

            if (IsSignedInt)
            {
                SetErrorText(ValueValidatorKnownError.NumberIsExpected);
                return;
            }

            SetErrorText(ValueValidatorKnownError.None);
        }

        void IValidatorReporter.SetErrorStatus(object? sender, bool showError, string? errorText)
        {
            ErrorStatusChanged?.Invoke(this, new(showError, errorText));
        }

        /// <summary>
        /// Gets "real" minimal value taking into account <see cref="CustomTextBox.DataType"/>
        /// and <see cref="MinValue"/>.
        /// </summary>
        public virtual object? GetRealMinValue()
        {
            var typeCode = GetDataTypeCode();
            var result = MathUtils.Max(AssemblyUtils.GetMinValue(typeCode), MinValue);
            return result;
        }

        /// <inheritdoc/>
        public override bool IsValidInputChar(char ch)
        {
            if (ch == CharUtils.BackspaceChar)
                return true;

            var result = base.IsValidInputChar(ch);
            return result;
        }

        /// <summary>
        /// Gets "real" maximal value taking into account <see cref="CustomTextBox.DataType"/>
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
        /// Gets 'Empty Text' error status if empty text is not allowed.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasErrorEmptyText()
        {
            if (string.IsNullOrEmpty(Text))
            {
                if (AllowEmptyText)
                    return false;
                else
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reports an error if <see cref="AbstractControl.Text"/> property is empty
        /// and it is not allowed (<see cref="CustomTextBox.AllowEmptyText"/> is <c>false</c>).
        /// </summary>
        public virtual bool ReportErrorEmptyText(Action<string>? errorEnumerator = null)
        {
            var result = HasErrorEmptyText();
            if (result)
            {
                ReportValidatorError(true, null, errorEnumerator);
            }

            return result;
        }

        /// <summary>
        /// Reports an error if <paramref name="value"/>
        /// is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error. If
        /// <see cref="CustomTextBox.DataType"/> is assigned, it is also used
        /// to get possible min and max
        /// values.
        /// </remarks>
        public virtual bool ReportErrorMinMaxValue(
            object? value,
            Action<string>? errorEnumerator = null)
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
                        GetKnownErrorText(ValueValidatorKnownError.MinimumValue),
                        errorEnumerator);
                    return true;
                case ValueInRangeResult.Greater:
                    ReportValidatorError(
                        true,
                        GetKnownErrorText(ValueValidatorKnownError.MaximumValue),
                        errorEnumerator);
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Reports an error if <see cref="CustomTextBox.DataType"/> is a number type and
        /// <see cref="TextAsNumber"/> is <c>null</c>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error.
        /// </remarks>
        public virtual bool ReportErrorBadNumber(Action<string>? errorEnumerator = null)
        {
            if (DataTypeIsNumber() && !string.IsNullOrEmpty(Text))
            {
                var value = TextAsNumber;
                if (value is null)
                {
                    ReportValidatorError(true, null, errorEnumerator);
                    return true;
                }

                if (ReportErrorMinMaxValue(value, errorEnumerator))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets 'Min Length' error status if <see cref="MinLength"/> is specified.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasErrorMinLength()
        {
            if (MinLength > 0)
            {
                if (Text.Length < MinLength)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets 'Max Length' error status if <see cref="MaxLength"/> is specified.
        /// </summary>
        /// <returns></returns>
        public virtual bool HasErrorMaxLength()
        {
            if (MaxLength > 0)
            {
                if (Text.Length > MaxLength)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Reports an error if length of the <see cref="AbstractControl.Text"/> property value
        /// is less than <see cref="MinLength"/> or greater than <see cref="MaxLength"/>.
        /// </summary>
        /// <returns><c>true</c> if validation error was reported; <c>false</c> if
        /// validation is ok and error was not reported.</returns>
        /// <remarks>
        /// <see cref="ReportValidatorError"/> is used to report the error.
        /// </remarks>
        public virtual bool ReportErrorMinMaxLength(Action<string>? errorEnumerator = null)
        {
            var hasErrorMinLength = HasErrorMinLength();

            if (hasErrorMinLength)
            {
                ReportValidatorError(
                    true,
                    GetKnownErrorText(ValueValidatorKnownError.MinimumLength),
                    errorEnumerator);
            }

            var hasErrorMaxLength = HasErrorMaxLength();

            if (hasErrorMaxLength)
            {
                ReportValidatorError(
                    true,
                    GetKnownErrorText(ValueValidatorKnownError.MaximumLength),
                    errorEnumerator);
            }

            return hasErrorMinLength || hasErrorMaxLength;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <returns>The validation errors.</returns>
        public virtual IEnumerable<string> GetErrors()
        {
            List<string> result = new();

            if (RunDefaultValidation(ErrorEnumerator))
                return Array.Empty<string>();

            return result;

            void ErrorEnumerator(string s)
            {
                if (result.IndexOf(s) < 0)
                    result.Add(s);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable GetErrors(string? propertyName)
        {
            return GetErrors();
        }

        /// <summary>
        /// Runs default validation of the <see cref="AbstractControl.Text"/> property.
        /// </summary>
        /// <remarks>
        /// Validation errors are reported using <see cref="ReportValidatorError"/>.
        /// </remarks>
        /// <returns>
        /// <c>true</c> if no validation errors were reported; <c>false</c> otherwise.
        /// </returns>
        public virtual bool RunDefaultValidation(Action<string>? errorEnumerator = null)
        {
            var errorCount = 0;

            var hasErrorEmptyText = ReportErrorEmptyText(ErrorEnumerator);
            var hasErrorBadNumber = ReportErrorBadNumber(ErrorEnumerator);
            var hasErrorMinMaxLength = ReportErrorMinMaxLength(ErrorEnumerator);

            var hasError = hasErrorEmptyText || hasErrorBadNumber || hasErrorMinMaxLength;

            if (!hasError)
                ReportValidatorError(false);
            RaiseErrorsChanged();
            return !hasError;

            void RaiseErrorsChanged()
            {
                if (errorCount != reportedErrorCount)
                {
                    reportedErrorCount = errorCount;
                    BubbleErrorsChanged(new DataErrorsChangedEventArgs(null));
                }
            }

            void ErrorEnumerator(string s)
            {
                errorCount++;
                errorEnumerator?.Invoke(s);
            }
        }

        /// <summary>
        /// Returns <see cref="TypeCode"/> for the <see cref="CustomTextBox.DataType"/> property
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
        /// Returns <c>true</c> if <see cref="CustomTextBox.DataType"/> is a number type.
        /// </summary>
        /// <returns></returns>
        public virtual bool DataTypeIsNumber() => AssemblyUtils.IsTypeCodeNumber(GetDataTypeCode());

        /// <summary>
        /// Returns minimal and maximal possible values for the <see cref="CustomTextBox.DataType"/>
        /// as a range string and formats it using <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Range string format. Example: "Range is [{0}]."</param>
        /// <param name="needUnsigned">Whether to force 0 as min value if it is less than 0.</param>
        /// <remarks>
        /// If <paramref name="format"/> is <c>null</c>, range string is returned unformatted.
        /// </remarks>
        public virtual string? GetMinMaxRangeStr(
            string? format = null,
            bool needUnsigned = false)
        {
            var s = AssemblyUtils.GetMinMaxRangeStr(
                GetDataTypeCode(),
                format,
                GetRealMinValue(),
                GetRealMaxValue(),
                needUnsigned);
            return s;
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

        /// <summary>
        /// Assigns <see cref="DataType"/> from the specified <paramref name="typeCode"/>
        /// and calls <see cref="SetTextAsObject"/>.
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="value"></param>
        public virtual void SetTextAsNumber(NumericTypeCode typeCode, object value)
        {
            DataType = AssemblyUtils.TypeFromTypeCode((TypeCode)typeCode);
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
        /// Sets <see cref="CustomTextBox.DataType"/> property to <typeparamref name="T"/>
        /// and <see cref="CharValidator"/> to the appropriate validator provider.
        /// </summary>
        /// <typeparam name="T">New <see cref="CustomTextBox.DataType"/> property value.</typeparam>
        public virtual void UseCharValidator<T>()
        {
            DataType = typeof(T);
            CharValidator = Alternet.UI.CharValidator.CreateValidator(typeof(T));
        }

        /// <summary>
        /// Sets <see cref="DataType"/> to the specified type and
        /// and <see cref="CharValidator"/> to the appropriate validator provider.
        /// </summary>
        /// <param name="type">New <see cref="CustomTextBox.DataType"/> property value.</param>
        /// <param name="charValidator">Whether to create and assign
        /// appropriate char validator or assign <see cref="CharValidator"/> to Null.</param>
        public virtual void SetValidator(Type? type, bool charValidator)
        {
            DataType = type;
            SetErrorTextFromDataType();

            if (charValidator)
                CharValidator = Alternet.UI.CharValidator.CreateValidator(type);
            else
                CharValidator = null;
        }

        /// <summary>
        /// Sets text as value (using <see cref="SetTextAsObject"/>)
        /// and assigns appropriate char and value validators.
        /// </summary>
        /// <param name="value">Object which will be converted to string and assigned
        /// to <see cref="AbstractControl.Text"/> property.</param>
        /// <param name="charValidator">Whether to set char validator.</param>
        public virtual void SetValueAndValidator(object? value, bool charValidator)
        {
            SetValidator(value?.GetType(), charValidator);
            Options |= TextBoxOptions.DefaultValidation;
            SetTextAsObject(value);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="string"/> and assigns
        /// <see cref="AbstractControl.Text"/>
        /// property with the converted value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <remarks>
        /// This method uses <see cref="CustomTextBox.Converter"/>,
        /// <see cref="CustomTextBox.DefaultFormat"/> and
        /// <see cref="CustomTextBox.FormatProvider"/> properties. Depending
        /// on the values of these properties, different conversion methods are used.
        /// </remarks>
        /// <remarks>
        /// If <see cref="CustomTextBox.DataType"/> property is <c>null</c>, it is set to
        /// the type of <paramref name="value"/>.
        /// </remarks>
        public virtual void SetTextAsObject(object? value)
        {
            Text = ObjectToString(value) ?? string.Empty;
        }

        /// <summary>
        /// Converts the specified object to string using conversion
        /// rules specified in the control.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="optionsOverride">Conversion options. If not specified,
        /// <see cref="Options"/> is used.</param>
        /// <returns></returns>
        public virtual string? ObjectToString(
            object? value,
            TextBoxOptions? optionsOverride = null)
        {
            if (value is null)
                return null;

            optionsOverride ??= options;

            var valueToString = ValueToString ?? GlobalValueToString;

            if (valueToString is not null)
            {
                var e = new ValueConvertEventArgs<object?, string?>(value);
                valueToString(this, e);
                if (e.Handled)
                    return e.Result;
            }

            var type = value.GetType();
            DataType ??= type;
            var converter = Converter;
            bool usedTypeConverter = false;

            if (converter is null && optionsOverride.Value.HasFlag(TextBoxOptions.UseTypeConverter))
            {
                converter = StringConverters.Default.CreateAdapter(TypeConverter);

                if (converter is null)
                {
                    converter = StringConverters.Default.CreateAdapterForTypeConverter(type);
                }

                usedTypeConverter = converter is not null;
            }

            if (converter is null)
            {
                var typeCode = AssemblyUtils.GetRealTypeCode(type);
                converter = StringConverters.Default.GetConverter(typeCode);
            }

            if (converter is null)
            {
                return value.ToString();
            }

            var converted = converter.TryConvert(
                this,
                value,
                this,
                out var conversionResult);

            if (converted)
            {
                return conversionResult;
            }

            if (usedTypeConverter)
            {
                return converter.ToString(
                    this,
                    value,
                    Context,
                    Culture,
                    optionsOverride.Value.HasFlag(TextBoxOptions.UseInvariantCulture));
            }

            if (DefaultFormat is null)
            {
                if (FormatProvider is null)
                    return converter.ToString(this, value);
                else
                    return converter.ToString(this, value, FormatProvider);
            }
            else
            {
                if (FormatProvider is null)
                    return converter.ToString(this, value, DefaultFormat);
                else
                {
                    return converter.ToString(
                        this,
                        value,
                        DefaultFormat,
                        FormatProvider);
                }
            }
        }

        /// <summary>
        /// Reports text validation error.
        /// </summary>
        /// <param name="showError">Indicates whether to show/hide error.</param>
        /// <param name="errorEnumerator">Optional action which is called for every error.</param>
        /// <param name="errorText">Specifies error text.</param>
        /// <remarks>
        /// Uses <see cref="CustomTextBox.DefaultErrorBackgroundColor"/>,
        /// <see cref="CustomTextBox.DefaultErrorForegroundColor"/>,
        /// <see cref="CustomTextBox.ValidatorErrorText"/>,
        /// <see cref="CustomTextBox.DefaultValidatorErrorText"/> and
        /// <see cref="CustomTextBox.ValidatorReporter"/> properties.
        /// </remarks>
        /// <remarks>
        /// <see cref="CustomTextBox.ValidatorReporter"/> property must support
        /// <see cref="IValidatorReporter"/>
        /// interface in order to be used in this method. <see cref="PictureBox"/> supports
        /// this interface.
        /// </remarks>
        public virtual string? ReportValidatorError(
            bool showError,
            string? errorText = null,
            Action<string>? errorEnumerator = null)
        {
            var hint = string.Empty;
            if (!showError)
            {
                if (DefaultErrorUseBackgroundColor)
                {
                    ResetBackgroundColor(ResetErrorBackgroundMethod
                        ?? DefaultResetErrorBackgroundMethod);
                }

                if (DefaultErrorUseForegroundColor)
                {
                    ResetForegroundColor(ResetErrorForegroundMethod
                        ?? DefaultResetErrorForegroundMethod);
                }
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

            if (!string.IsNullOrEmpty(hint))
                errorEnumerator?.Invoke(hint!);

            return hint;

            void Report(IValidatorReporter? reporter)
            {
                if (AutoShowError && reporter is AbstractControl reporterControl)
                {
                    if(reporterControl != this)
                    {
                        Post(() =>
                        {
                            if(DisposingOrDisposed || reporterControl.DisposingOrDisposed)
                                return;
                            reporterControl.Visible = showError;
                        });
                    }
                }

                reporter?.SetErrorStatus(this, showError, hint);
            }
        }

        /// <summary>
        /// Resets fields and properties before editing new value with the different data type.
        /// You can call this method in order to reset members related to data type, formatting
        /// and value conversion.
        /// </summary>
        public virtual void ResetInputSettings()
        {
            allowEmptyText = DefaultAllowEmptyText;
            options = DefaultOptions;
            culture = null;
            typeConverter = null;
            context = null;
            converter = null;
            defaultFormat = null;
            numberStyles = null;
            validatorErrorText = null;
            emptyTextValue = null;
            minLength = default;
            maxLength = default;
            inputType = null;
            minValue = null;
            maxValue = null;
            dataType = null;
            defaultText = null;
            search = null;
            textAsValueError = null;
            trimTextRules = default;
        }

        /// <summary>
        /// Gets known error text.
        /// </summary>
        /// <param name="kind">Error kind.</param>
        public virtual string? GetKnownErrorText(ValueValidatorKnownError kind)
        {
            var needUnsigned = kind == ValueValidatorKnownError.UnsignedNumberIsExpected
                || kind == ValueValidatorKnownError.UnsignedFloatIsExpected;

            string AddRangeSuffix(string s)
            {
                if (options.HasFlag(TextBoxOptions.IntRangeInError))
                {
                    var rangeStr = GetMinMaxRangeStr(
                            ErrorMessages.Default.ValidationRangeFormat,
                            needUnsigned);
                    return $"{s} {rangeStr}".Trim();
                }
                else
                    return s;
            }

            switch (kind)
            {
                case ValueValidatorKnownError.None:
                    return null;
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
                    return string.Format(
                        ErrorMessages.Default.ValidationMinimumValue,
                        GetMinValueAsString(needUnsigned));
                case ValueValidatorKnownError.MaximumValue:
                    return string.Format(
                        ErrorMessages.Default.ValidationMaximumValue,
                        GetRealMaxValue());
                case ValueValidatorKnownError.MinMaxLength:
                    return string.Format(
                        ErrorMessages.Default.ValidationMinMaxLength,
                        MinLength,
                        MaxLength);
                case ValueValidatorKnownError.ValueIsRequired:
                    return ErrorMessages.Default.ValidationValueIsRequired;
                case ValueValidatorKnownError.EMailIsExpected:
                    return ErrorMessages.Default.ValidationEMailIsExpected;
                case ValueValidatorKnownError.UrlIsExpected:
                    return ErrorMessages.Default.ValidationUrlIsExpected;
                default:
                    var defaultResult = ValidatorErrorText ?? DefaultValidatorErrorText;
                    return defaultResult ?? ErrorMessages.Default.ValidationInvalidFormat;
            }
        }

        /// <summary>
        /// Gets minimal possible value as <see cref="string"/>.
        /// </summary>
        /// <param name="needUnsigned">Whether to return 0 even if
        /// minimal value is less than 0.</param>
        /// <returns></returns>
        protected virtual object? GetMinValueAsString(bool needUnsigned)
        {
            var result = GetRealMinValue();

            if (!needUnsigned)
            {
                return result;
            }

            if (result == null)
                return null;
            var resultStr = result.ToString()?.Trim();

            if (resultStr is null || resultStr.Length == 0)
                return result;
            if (resultStr[0] == '-')
                return "0";
            return result;
        }
    }
}
