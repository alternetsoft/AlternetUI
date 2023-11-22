using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the <see cref="CustomTextBox"/> methods and properties.
    /// </summary>
    public interface ICustomTextBox : IDisposable
    {
        /// <inheritdoc cref="CustomTextBox.ErrorStatusChanged"/>
        event ErrorStatusEventHandler? ErrorStatusChanged;

        /// <inheritdoc cref="CustomTextBox.NumberStyles"/>
        NumberStyles? NumberStyles { get; set; }

        /// <inheritdoc cref="CustomTextBox.FormatProvider"/>
        IFormatProvider? FormatProvider { get; set; }

        /// <inheritdoc cref="CustomTextBox.DefaultFormat"/>
        string? DefaultFormat { get; set; }

        /// <inheritdoc cref="CustomTextBox.DefaultText"/>
        string? DefaultText { get; set; }

        /// <inheritdoc cref="CustomTextBox.DataType"/>
        Type? DataType { get; set; }

        /// <inheritdoc cref="CustomTextBox.MaxLength"/>
        int MaxLength { get; set; }

        /// <inheritdoc cref="CustomTextBox.ValidatorReporter"/>
        IValidatorReporter? ValidatorReporter { get; set; }

        /// <inheritdoc cref="CustomTextBox.ValidatorErrorText"/>
        string? ValidatorErrorText { get; set; }

        /// <inheritdoc cref="CustomTextBox.AllowEmptyText"/>
        bool AllowEmptyText { get; set; }

        /// <inheritdoc cref="CustomTextBox.IsRequired"/>
        bool IsRequired { get; set; }

        /// <inheritdoc cref="CustomTextBox.EmptyTextValue"/>
        object? EmptyTextValue { get; set; }

        /// <inheritdoc cref="CustomTextBox.Search"/>
        StringSearch Search { get; set; }

        /// <inheritdoc cref="CustomTextBox.ResetErrorBackgroundMethod"/>
        ResetColorType? ResetErrorBackgroundMethod { get; set; }

        /// <inheritdoc cref="CustomTextBox.ResetErrorForegroundMethod"/>
        ResetColorType? ResetErrorForegroundMethod { get; set; }

        /// <inheritdoc cref="CustomTextBox.IsNullOrEmpty"/>
        bool IsNullOrEmpty { get; }

        /// <inheritdoc cref="CustomTextBox.IsNullOrWhiteSpace"/>
        bool IsNullOrWhiteSpace { get; }

        /// <inheritdoc cref="CustomTextBox.TextAsNumber"/>
        object? TextAsNumber { get; set; }

        /// <inheritdoc cref="CustomTextBox.MinValue"/>
        object? MinValue { get; set; }

        /// <inheritdoc cref="CustomTextBox.MaxValue"/>
        object? MaxValue { get; set; }

        /// <inheritdoc cref="CustomTextBox.Options"/>
        TextBoxOptions Options { get; set; }

        /// <inheritdoc cref="CustomTextBox.MinLength"/>
        int MinLength { get; set; }

        /// <inheritdoc cref="CustomTextBox.Text"/>
        string Text { get; set; }

        /// <inheritdoc cref="CustomTextBox.GetNumberOfLines"/>
        int GetNumberOfLines();

        /// <inheritdoc cref="CustomTextBox.GetLineText"/>
        string GetLineText(long lineNo);

        /// <inheritdoc cref="CustomTextBox.GetRealMinValue()"/>
        object? GetRealMinValue();

        /// <inheritdoc cref="CustomTextBox.GetRealMaxValue"/>
        object? GetRealMaxValue();

        /// <inheritdoc cref="CustomTextBox.TextAsNumberOrDefault"/>
        T TextAsNumberOrDefault<T>(T defValue);

        /// <inheritdoc cref="CustomTextBox.ReportErrorEmptyText"/>
        bool ReportErrorEmptyText();

        /// <inheritdoc cref="CustomTextBox.ReportErrorMinMaxValue"/>
        bool ReportErrorMinMaxValue(object? value);

        /// <inheritdoc cref="CustomTextBox.ReportErrorBadNumber"/>
        bool ReportErrorBadNumber();

        /// <inheritdoc cref="CustomTextBox.ReportErrorMinMaxLength"/>
        bool ReportErrorMinMaxLength();

        /// <inheritdoc cref="CustomTextBox.RunDefaultValidation"/>
        void RunDefaultValidation();

        /// <inheritdoc cref="CustomTextBox.GetDataTypeCode"/>
        TypeCode GetDataTypeCode();

        /// <inheritdoc cref="CustomTextBox.DataTypeIsNumber"/>
        bool DataTypeIsNumber();

        /// <inheritdoc cref="CustomTextBox.GetMinMaxRangeStr"/>
        string? GetMinMaxRangeStr(string? format = null);

        /// <inheritdoc cref="CustomTextBox.SetTextAsSByte"/>
        void SetTextAsSByte(sbyte value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsByte"/>
        void SetTextAsByte(byte value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsInt16"/>
        void SetTextAsInt16(short value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsUInt16"/>
        void SetTextAsUInt16(ushort value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsInt32"/>
        void SetTextAsInt32(int value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsUInt32"/>
        void SetTextAsUInt32(uint value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsInt64"/>
        void SetTextAsInt64(long value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsUInt64"/>
        void SetTextAsUInt64(ulong value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsSingle"/>
        void SetTextAsSingle(float value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsDouble"/>
        void SetTextAsDouble(double value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsDecimal"/>
        void SetTextAsDecimal(decimal value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsDateTime"/>
        void SetTextAsDateTime(DateTime value);

        /// <inheritdoc cref="CustomTextBox.SetTextAsObject"/>
        void SetTextAsObject(object? value);

        /// <inheritdoc cref="CustomTextBox.ReportValidatorError"/>
        void ReportValidatorError(bool showError, string? errorText = null);

        /// <inheritdoc cref="CustomTextBox.GetKnownErrorText"/>
        string GetKnownErrorText(ValueValidatorKnownError kind);
    }
}