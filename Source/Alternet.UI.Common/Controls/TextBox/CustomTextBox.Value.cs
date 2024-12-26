using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class CustomTextBox
    {
        private IFormatProvider? formatProvider;

        /// <summary>
        /// Gets whether <see cref="DataType"/> is a number type.
        /// </summary>
        [Browsable(false)]
        public bool IsNumber
        {
            get
            {
                return DataTypeIsNumber();
            }
        }

        /// <summary>
        /// Gets whether <see cref="DataType"/> specifies a signed integer number
        /// (int, long, sbyte, short).
        /// Additionally <see cref="MinValue"/>
        /// is also checked whether it allows negative numbers.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsSignedInt
        {
            get
            {
                var typeCode = GetDataTypeCode();

                if (AssemblyUtils.IsTypeCodeSignedInt(typeCode) && IsMinValueNegativeOrNull)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets whether <see cref="DataType"/> specifies an unsigned integer number
        /// (uint, ulong, byte, ushort).
        /// </summary>
        [Browsable(false)]
        public virtual bool IsUnsignedInt
        {
            get
            {
                var typeCode = GetDataTypeCode();

                if (AssemblyUtils.IsTypeCodeUnsignedInt(typeCode))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets whether <see cref="DataType"/> specifies a float number
        /// (single, double, decimal).
        /// </summary>
        [Browsable(false)]
        public virtual bool IsFloat
        {
            get
            {
                var typeCode = GetDataTypeCode();

                if (AssemblyUtils.IsTypeCodeFloat(typeCode))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets whether <see cref="DataType"/> specifies a signed number.
        /// Additionally <see cref="MinValue"/>
        /// is also checked whether it allows negative numbers.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsSignedNumber
        {
            get
            {
                if (IsNumber && IsMinValueNegativeOrNull)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets whether <see cref="MinValue"/> is specified and negative.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMinValueNegativeOrNull
        {
            get
            {
                var realMinValue = GetRealMinValue();

                if (realMinValue is null)
                    return true;
                else
                {
                    var typeCode = GetDataTypeCode();
                    var signed = MathUtils.LessThanDefault(typeCode, realMinValue);
                    return signed ?? true;
                }
            }
        }

        /// <summary>
        /// Gets whether <see cref="NumberStyles"/>
        /// has <see cref="System.Globalization.NumberStyles.HexNumber"/> flag.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsHexNumber
        {
            get
            {
                if (NumberStyles is not null
                    && NumberStyles.Value.HasFlag(System.Globalization.NumberStyles.HexNumber))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Type"/> of the <see cref="AbstractControl.Text"/> property.
        /// </summary>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual Type? DataType
        {
            get => dataType;
            set => dataType = value;
        }

        /// <inheritdoc cref="IObjectToStringOptions.NumberStyles"/>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it if <see cref="TextBox"/> edits a number value or
        /// for any other purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual NumberStyles? NumberStyles
        {
            get => numberStyles;
            set => numberStyles = value;
        }

        /// <inheritdoc cref="IObjectToStringOptions.FormatProvider"/>
        /// <remarks>
        /// Default value is <c>null</c>. <see cref="TextBox"/> behavior is not affected
        /// by this property, you can use it for any purposes.
        /// </remarks>
        [Browsable(false)]
        public virtual IFormatProvider? FormatProvider
        {
            get => formatProvider;
            set => formatProvider = value;
        }

        /// <inheritdoc cref="IObjectToStringOptions.DefaultFormat"/>
        [Browsable(false)]
        public virtual string? DefaultFormat
        {
            get => defaultFormat;
            set => defaultFormat = value;
        }

        /// <inheritdoc cref="IObjectToStringOptions.Converter"/>
        [Browsable(false)]
        public virtual IObjectToString? Converter
        {
            get => converter;
            set => converter = value;
        }

        /// <summary>
        /// Gets or sets <see cref="TypeConverter"/> used for the text to/from value conversion.
        /// You also need to specify <see cref="TextBoxOptions.UseTypeConverter"/>
        /// in <see cref="Options"/>.
        /// </summary>
        [Browsable(false)]
        public virtual TypeConverter? TypeConverter
        {
            get => typeConverter;
            set => typeConverter = value;
        }

        /// <summary>
        /// Gets or sets <see cref="ITypeDescriptorContext"/> value which is used
        /// when text to/from value is converted using <see cref="TypeConverter"/>.
        /// </summary>
        [Browsable(false)]
        public virtual ITypeDescriptorContext? Context
        {
            get => context;
            set => context = value;
        }

        /// <summary>
        /// Gets or sets <see cref="CultureInfo"/> value which is used
        /// when text to/from value is converted using <see cref="TypeConverter"/>.
        /// </summary>
        [Browsable(false)]
        public virtual CultureInfo? Culture
        {
            get => culture;
            set => culture = value;
        }

        private bool TextToValueWithEvent(out object? result)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                result = this.EmptyTextValue;
                return true;
            }

            var stringToValue = StringToValue ?? GlobalStringToValue;

            if (stringToValue is not null)
            {
                var e = new ValueConvertEventArgs<string?, object?>(Text);
                stringToValue(this, e);
                if (e.Handled)
                {
                    result = e.Result;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
