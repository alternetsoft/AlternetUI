using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for text editors.
    /// </summary>
    public abstract class CustomTextBox : Control, ICustomTextBox, IReadOnlyStrings, IValidatorReporter
    {
        private StringSearch? search;
        private int minLength;
        private int maxLength;
        private TextBoxOptions options = TextBoxOptions.IntRangeInError;

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
        /// application needs to report user an error in <see cref="Control.Text"/> property.
        /// </summary>
        public static Color DefaultErrorBackgroundColor { get; set; } = Color.Red;

        /// <summary>
        /// Gets or sets default <see cref="Color"/> that can be used
        /// as a foreground color for the <see cref="TextBox"/> in cases when
        /// application needs to report user an error in <see cref="Control.Text"/> property.
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
        /// Gets or sets default value for the <see cref="ResetErrorBackgroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorBackgroundMethod { get; set; } = ResetColorType.Auto;

        /// <summary>
        /// Gets or sets default value for the <see cref="ResetErrorForegroundMethod"/>
        /// property.
        /// </summary>
        public static ResetColorType DefaultResetErrorForegroundMethod { get; set; } = ResetColorType.Auto;

        /// <summary>
        /// Gets or sets a bitwise combination of <see cref="NumberStyles"/> values that indicates
        /// the permitted format of <see cref="Control.Text"/>.
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
        /// about <see cref="Control.Text"/> property.
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
        [Browsable(false)]
        public virtual IObjectToString? Converter { get; set; }

        /// <summary>
        /// Gets or sets default value for the <see cref="Control.Text"/> property.
        /// </summary>
        public virtual string? DefaultText { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Type"/> of the <see cref="Control.Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual Type? DataType { get; set; }

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
                if (maxLength == value || value < 0)
                    return;
                maxLength = value;
            }
        }

        /// <summary>
        /// Gets or sets validator reporter object or control.
        /// </summary>
        /// <remarks>
        /// This propety can be used to store reference to control that
        /// reports validation or other errors to the end users. Usually
        /// this is a <see cref="PictureBox"/> with error image.
        /// </remarks>
        [Browsable(false)]
        public virtual IValidatorReporter? ValidatorReporter { get; set; }

        /// <summary>
        /// Gets or sets a text string that can be used as validator error message.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        public virtual string? ValidatorErrorText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether empty string is allowed in <see cref="Control.Text"/>.
        /// </summary>
        /// <remarks>
        /// Default value is <c>true</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual bool AllowEmptyText { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Control.Text"/> is required to be not empty.
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
        /// Gets or sets data value in cases when <see cref="Control.Text"/> property is empty.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual object? EmptyTextValue { get; set; }

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
        /// if error backround color is used for reporting it.
        /// </summary>
        public ResetColorType? ResetErrorBackgroundMethod { get; set; }

        /// <summary>
        /// Gets or sets method which is used to clear error state
        /// if error foreground color is used for reporting it.
        /// </summary>
        public ResetColorType? ResetErrorForegroundMethod { get; set; }

        /// <summary>
        /// Gets whether <see cref="Control.Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="Control.Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Gets or sets <see cref="Control.Text"/> property value as <see cref="object"/> of a number type.
        /// </summary>
        /// <remarks>
        /// If <see cref="Control.Text"/> property doesn't contain a number value, <c>null</c> is returned.
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
                    [
                        StringUtils.TryParseInt32,
                        StringUtils.TryParseUInt32,
                        StringUtils.TryParseInt64,
                        StringUtils.TryParseUInt64,
                        StringUtils.TryParseDouble,
                        StringUtils.TryParseDecimal,
                    ];

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
        [Browsable(false)]
        public virtual object? MaxValue { get; set; }

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
            picture.Image = KnownSvgImages.GetWarningImage(
                picture.GetSvgColor(KnownSvgColor.Error));
            picture.VerticalAlignment = VerticalAlignment.Center;
            picture.ImageVisible = false;
            picture.ImageStretch = false;
            picture.TabStop = false;

            picture.MouseLeftButtonUp += Picture_MouseLeftButtonUp;

            static void Picture_MouseLeftButtonUp(object? sender, MouseEventArgs e)
            {
                if (sender is not PictureBox pictureBox)
                    return;
                e.Handled = true;

                pictureBox.HideToolTip();

                RichToolTip.Show(
                    ErrorMessages.Default.ErrorTitle,
                    pictureBox.ToolTip,
                    pictureBox,
                    RichToolTipKind.None,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Returns the number of lines in the text control buffer.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// The returned number is the number of logical lines, i.e. just the count of
        /// the number of newline characters in the control + 1, for GTK
        /// and OSX/Cocoa ports while it is the number of physical lines,
        /// i.e. the count of
        /// lines actually shown in the control, in MSW and OSX/Carbon. Because of
        /// this discrepancy, it is not recommended to use this function.
        /// </remarks>
        /// <remarks>
        /// Note that even empty text controls have one line (where the
        /// insertion point is), so this function never returns 0.
        /// </remarks>
        public abstract int GetNumberOfLines();

        /// <summary>
        /// Returns the contents of a given line in the text control, not
        /// including any trailing newline character(s).
        /// </summary>
        /// <param name="lineNo">Line number (starting from zero).</param>
        /// <returns>The contents of the line.</returns>
        public abstract string GetLineText(long lineNo);

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

        /// <summary>
        /// Gets "real" maxmimal value taking into account <see cref="CustomTextBox.DataType"/>
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
        /// Reports an error if <see cref="Control.Text"/> property is empty
        /// and it is not allowed (<see cref="CustomTextBox.AllowEmptyText"/> is <c>false</c>).
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
        /// <see cref="CustomTextBox.DataType"/> is assigned, it is also used to get possible min and max
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
        /// Reports an error if <see cref="CustomTextBox.DataType"/> is a number type and
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
        /// Reports an error if length of the <see cref="Control.Text"/> property value
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
        /// Runs default validation of the <see cref="Control.Text"/> property.
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
        /// <remarks>
        /// If <paramref name="format"/> is <c>null</c>, range string is returned unformatted.
        /// </remarks>
        public virtual string? GetMinMaxRangeStr(string? format = null)
        {
            var s = AssemblyUtils.GetMinMaxRangeStr(GetDataTypeCode(), format, MinValue, MaxValue);
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
        /// Converts <paramref name="value"/> to <see cref="string"/> and assigns <see cref="Control.Text"/>
        /// property with the converted value.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <remarks>
        /// This method uses <see cref="CustomTextBox.Converter"/>, <see cref="CustomTextBox.DefaultFormat"/> and
        /// <see cref="CustomTextBox.FormatProvider"/> propertties. Depending
        /// on the values of these properties, different conversion methods are used.
        /// </remarks>
        /// <remarks>
        /// If <see cref="CustomTextBox.DataType"/> property is <c>null</c>, it is set to
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
        /// Reports text validation error.
        /// </summary>
        /// <param name="showError">Indicates whether to show/hide error.</param>
        /// <param name="errorText">Specifies error text.</param>
        /// <remarks>
        /// Uses <see cref="CustomTextBox.DefaultErrorBackgroundColor"/>, <see cref="CustomTextBox.DefaultErrorForegroundColor"/>,
        /// <see cref="CustomTextBox.ValidatorErrorText"/>, <see cref="CustomTextBox.DefaultValidatorErrorText"/> and
        /// <see cref="CustomTextBox.ValidatorReporter"/> properties.
        /// </remarks>
        /// <remarks>
        /// <see cref="CustomTextBox.ValidatorReporter"/> property must support <see cref="IValidatorReporter"/>
        /// interface in order to be used in this method. <see cref="PictureBox"/> supports
        /// this interface.
        /// </remarks>
        public virtual void ReportValidatorError(bool showError, string? errorText = null)
        {
            var hint = string.Empty;
            if (!showError)
            {
                if (DefaultErrorUseBackgroundColor)
                    ResetBackgroundColor(ResetErrorBackgroundMethod ?? DefaultResetErrorBackgroundMethod);
                if (DefaultErrorUseForegroundColor)
                    ResetForegroundColor(ResetErrorForegroundMethod ?? DefaultResetErrorForegroundMethod);
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
                case ValueValidatorKnownError.UrlIsExpected:
                    return ErrorMessages.Default.ValidationUrlIsExpected;
                default:
                    var defaultResult = ValidatorErrorText ?? DefaultValidatorErrorText;
                    return defaultResult ?? ErrorMessages.Default.ValidationInvalidFormat;
            }
        }
    }
}
