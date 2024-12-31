using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Alternet.UI
{
    public partial class StringConverters
    {
        /// <summary>
        /// Base <see cref="IObjectToString"/> interface implementation.
        /// </summary>
        public abstract class BaseToStringConverter : IObjectToString
        {
            /// <inheritdoc/>
            public virtual bool TryConvert(
                object? sender,
                object value,
                IObjectToStringOptions? args,
                out string? result)
            {
                try
                {
                    result = ToString(sender, value, args?.DefaultFormat, args?.FormatProvider);
                    return true;
                }
                catch (Exception e)
                {
                    App.LogError(e);
                    result = string.Empty;
                    return false;
                }
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ToString(sender, value);
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value, string? format)
            {
                return ToString(sender, value);
            }

            /// <inheritdoc/>
            public virtual string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ToString(sender, value);
            }

            /// <inheritdoc/>
            public virtual string? ToString(
                object? sender,
                object value,
                ITypeDescriptorContext? context,
                CultureInfo? culture,
                bool useInvariantConversion)
            {
                return ToString(sender, value);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="sbyte"/>.
        /// </summary>
        public class SByteToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((sbyte)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((sbyte)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((sbyte)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((sbyte)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="byte"/>.
        /// </summary>
        public class ByteToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((byte)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((byte)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((byte)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((byte)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="short"/>.
        /// </summary>
        public class Int16ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((short)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((short)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((short)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((short)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="ushort"/>.
        /// </summary>
        public class UInt16ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((ushort)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((ushort)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((ushort)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((ushort)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="int"/>.
        /// </summary>
        public class Int32ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((int)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((int)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((int)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((int)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="uint"/>.
        /// </summary>
        public class UInt32ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((uint)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((uint)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((uint)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((uint)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="long"/>.
        /// </summary>
        public class Int64ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((long)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((long)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((long)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((long)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="ulong"/>.
        /// </summary>
        public class UInt64ToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((ulong)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((ulong)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((ulong)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((ulong)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="float"/>.
        /// </summary>
        public class SingleToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((float)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((float)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((float)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((float)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="double"/>.
        /// </summary>
        public class DoubleToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((double)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((double)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((double)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((double)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="decimal"/>.
        /// </summary>
        public class DecimalToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((decimal)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((decimal)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((decimal)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((decimal)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="DateTime"/>.
        /// </summary>
        public class DateTimeToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((DateTime)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                IFormatProvider? provider)
            {
                return ((DateTime)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((DateTime)value).ToString(format);
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((DateTime)value).ToString(format, provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="TypeCode.Empty"/>.
        /// </summary>
        public class EmptyToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="TypeCode.DBNull"/>.
        /// </summary>
        public class DBNullToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="object"/>.
        /// </summary>
        public class ObjectToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return value.ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="bool"/>.
        /// </summary>
        public class BooleanToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((bool)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((bool)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((bool)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((bool)value).ToString(provider);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="char"/>.
        /// </summary>
        public class CharToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return ((char)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ((char)value).ToString(provider);
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return ((char)value).ToString();
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ((char)value).ToString(provider);
            }
        }

        /// <summary>
        /// <see cref="IObjectToString"/> interface implementation which
        /// uses <see cref="TypeConverter"/> for the conversion.
        /// </summary>
        public class TypeConverterAdapter : IObjectToString
        {
            private readonly TypeConverter converter;

            /// <summary>
            /// Initializes a new instance of the <see cref="TypeConverterAdapter"/> class.
            /// </summary>
            /// <param name="converter"></param>
            public TypeConverterAdapter(TypeConverter converter)
            {
                this.converter = converter;
            }

            /// <inheritdoc/>
            public virtual bool TryConvert(
                object? sender,
                object value,
                IObjectToStringOptions? args,
                out string? result)
            {
                try
                {
                    result = ToString(
                        sender,
                        value,
                        args?.Context,
                        args?.Culture,
                        args?.UseInvariantCulture ?? false);
                    return true;
                }
                catch (Exception e)
                {
                    App.LogError(e);
                    result = string.Empty;
                    return false;
                }
            }

            /// <inheritdoc/>
            public virtual string? ToString(
                object? sender,
                object value,
                ITypeDescriptorContext? context,
                CultureInfo? culture,
                bool useInvariantConversion)
            {
                string? result;

                if (useInvariantConversion)
                    result = InvariantConversion();
                else
                    result = Conversion();

                string? InvariantConversion()
                {
                    if (context is not null)
                    {
                        return converter.ConvertToInvariantString(context, value);
                    }
                    else
                    {
                        return converter.ConvertToInvariantString(value);
                    }
                }

                string? Conversion()
                {
                    if (context is null)
                    {
                        if (culture is null)
                        {
                            return converter.ConvertToString(value);
                        }
                        else
                        {
                            return converter.ConvertToString(null, culture, value);
                        }
                    }
                    else
                    {
                        if (culture is null)
                        {
                            return converter.ConvertToString(context, value);
                        }
                        else
                        {
                            return converter.ConvertToString(context, culture, value);
                        }
                    }
                }

                return result;
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value)
            {
                return ToString(sender, value, null, null, false);
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return ToString(sender, value);
            }

            /// <inheritdoc/>
            public virtual string? ToString(object? sender, object value, string? format)
            {
                return ToString(sender, value);
            }

            /// <inheritdoc/>
            public string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return ToString(sender, value);
            }
        }

        /// <summary>
        /// Implements <see cref="IObjectToString"/> interface for the conversion
        /// from <see cref="string"/>.
        /// </summary>
        public class StringToStringConverter : BaseToStringConverter, IObjectToString
        {
            /// <inheritdoc/>
            public override string? ToString(object? sender, object value)
            {
                return (string)value;
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, IFormatProvider? provider)
            {
                return (string)value;
            }

            /// <inheritdoc/>
            public override string? ToString(object? sender, object value, string? format)
            {
                return (string)value;
            }

            /// <inheritdoc/>
            public override string? ToString(
                object? sender,
                object value,
                string? format,
                IFormatProvider? provider)
            {
                return (string)value;
            }
        }
    }
}
