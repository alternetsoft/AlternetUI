using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains properties and methods related to <see cref="IObjectToString"/>.
    /// </summary>
    public class ObjectToStringFactory : BaseObject
    {
        /// <summary>
        /// Gets <see cref="Type"/> to <see cref="TypeConverter"/> dictionary.
        /// </summary>
        public static readonly IndexedValues<Type, TypeConverter> Converters = new();

        /// <summary>
        /// Gets or sets whether to handle <see cref="TypeDescriptor.Refreshed"/> event.
        /// Default is False.
        /// </summary>
        public static bool HandleTypeDescriptorRefreshed = true;

        private static IndexedValues<Type, Type>? convertersOverride;

        static ObjectToStringFactory()
        {
            TypeDescriptor.Refreshed += TypeDescriptorRefreshed;

            void TypeDescriptorRefreshed(RefreshEventArgs e)
            {
                if(HandleTypeDescriptorRefreshed)
                    Converters.Clear();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectToStringFactory"/> class.
        /// </summary>
        public ObjectToStringFactory()
        {
        }

        /// <summary>
        /// Gets or sets default <see cref="ObjectToStringFactory"/> implementation.
        /// </summary>
        public static ObjectToStringFactory Default { get; set; } = new();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="sbyte"/> type.
        /// </summary>
        public virtual IObjectToString SByteToString { get; set; } = new SByteToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="byte"/> type.
        /// </summary>
        public virtual IObjectToString ByteToString { get; set; } = new ByteToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="short"/> type.
        /// </summary>
        public virtual IObjectToString Int16ToString { get; set; } = new Int16ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="ushort"/> type.
        /// </summary>
        public virtual IObjectToString UInt16ToString { get; set; } = new UInt16ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="int"/> type.
        /// </summary>
        public virtual IObjectToString Int32ToString { get; set; } = new Int32ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="uint"/> type.
        /// </summary>
        public virtual IObjectToString UInt32ToString { get; set; } = new UInt32ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="long"/> type.
        /// </summary>
        public virtual IObjectToString Int64ToString { get; set; } = new Int64ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="ulong"/> type.
        /// </summary>
        public virtual IObjectToString UInt64ToString { get; set; } = new UInt64ToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="float"/> type.
        /// </summary>
        public virtual IObjectToString SingleToString { get; set; } = new SingleToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="double"/> type.
        /// </summary>
        public virtual IObjectToString DoubleToString { get; set; } = new DoubleToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/>
        /// implementation for the <see cref="decimal"/> type.
        /// </summary>
        public virtual IObjectToString DecimalToString { get; set; } = new DecimalToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/>
        /// implementation for the <see cref="DateTime"/> type.
        /// </summary>
        public virtual IObjectToString DateTimeToString { get; set; } = new DateTimeToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the case if
        /// type is <see cref="TypeCode.Empty"/>.
        /// </summary>
        public virtual IObjectToString EmptyToString { get; set; } = new EmptyToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the case if
        /// type is <see cref="TypeCode.DBNull"/>.
        /// </summary>
        public virtual IObjectToString DBNullToString { get; set; } = new DBNullToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="object"/> type.
        /// </summary>
        public virtual IObjectToString ObjectToString { get; set; } = new ObjectToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="bool"/> type.
        /// </summary>
        public virtual IObjectToString BooleanToString { get; set; } = new BooleanToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="char"/> type.
        /// </summary>
        public virtual IObjectToString CharToString { get; set; } = new CharToStringConverter();

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the <see cref="string"/> type.
        /// </summary>
        public virtual IObjectToString StringToString { get; set; } = new StringToStringConverter();

        /// <summary>
        /// Registers an override <see cref="TypeConverter"/> for the specified type.
        /// After override is registered, it is returned when
        /// <see cref="GetTypeConverter"/>
        /// is called. In order to unregister an override, call it with Null parameter.
        /// </summary>
        /// <param name="type">Type for which <see cref="TypeConverter"/>
        /// override is registered.</param>
        /// <param name="typeConverterType">Type of the <see cref="TypeConverter"/> descendant
        /// which is used as an override.</param>
        public virtual void RegisterTypeConverter(Type type, Type? typeConverterType)
        {
            convertersOverride ??= new();
            convertersOverride[type] = typeConverterType;
        }

        /// <summary>
        /// Similar to <see cref="RegisterTypeConverter(Type, Type)"/>.
        /// </summary>
        /// <typeparam name="TType">Type for which converter is registered.</typeparam>
        /// <typeparam name="TTypeConverter">Type of the converter.</typeparam>
        public void RegisterTypeConverter<TType, TTypeConverter>()
            where TTypeConverter : TypeConverter
        {
            RegisterTypeConverter(typeof(TType), typeof(TTypeConverter));
        }

        /// <summary>
        /// Gets <see cref="TypeConverter"/> object for the specified type.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <param name="toString">Whether conversion is from object to string. If not Null,
        /// type converter is quieried whether it can convert to/from string.
        /// If Null, no such check is performed.</param>
        /// <param name="culture">Optional <see cref="CultureInfo"/> object used to create
        /// <see cref="TypeConverter"/> instance.</param>
        public virtual TypeConverter? GetTypeConverter(
            Type type,
            bool? toString = true,
            CultureInfo? culture = null)
        {
            TypeConverter? CreateTypeConverter(Type? converterType)
            {
                if (converterType is null)
                    return null;
                var typeConverter = Activator.CreateInstance(
                    converterType,
                    BindingFlags.Instance | BindingFlags.CreateInstance | BindingFlags.Public,
                    null,
                    null,
                    culture ?? App.InvariantEnglishUS) as TypeConverter;
                return typeConverter;
            }

            if (type is null)
                return null;

            var typeConverter = Converters.GetValue(type, InternalWithOverride);

            TypeConverter? InternalWithOverride()
            {
                TypeConverter? result = null;

                if (convertersOverride is not null)
                {
                    var converterType = convertersOverride[type];
                    result = CreateTypeConverter(converterType);
                }

                /*
                result ??= TypeConverterHelper.GetTypeConverter(
                    type,
                    culture ?? App.InvariantEnglishUS);
                */

                result ??= TypeDescriptor.GetConverter(type);

                var isBaseOrNull
                    = result is null || result.GetType() == typeof(TypeConverter);

                if (isBaseOrNull)
                {
                    var parseMethod = type.GetMethod(
                        "Parse",
                        BindingFlags.Static | BindingFlags.Public,
                        null,
                        [typeof(string)],
                        null);

                    if(parseMethod is not null)
                    {
                        result = new TypeConverterUsingParse(parseMethod);
                    }
                }

                return result;
            }

            if (typeConverter is null)
                return null;

            if(toString is not null)
            {
                var canConvert = toString.Value
                    ? typeConverter.CanConvertTo(typeof(string))
                    : typeConverter.CanConvertFrom(typeof(string));
                if (!canConvert)
                    return null;
            }

            return typeConverter;
        }

        /// <summary>
        /// Creates <see cref="IObjectToString"/> implementation
        /// for the specified type converter.
        /// </summary>
        /// <param name="converter">Type converter.</param>
        /// <returns></returns>
        public virtual IObjectToString? CreateAdapter(TypeConverter? converter)
        {
            if (converter is null)
                return null;
            return new TypeConverterAdapter(converter);
        }

        /// <summary>
        /// Gets <see cref="IObjectToString"/> implementation
        /// for the specified type using adapted <see cref="TypeConverter"/>
        /// which is obtained using <see cref="GetTypeConverter"/>.
        /// </summary>
        /// <param name="type">Type.</param>
        public virtual IObjectToString? CreateAdapterForTypeConverter(Type type)
        {
            var typeConverter = GetTypeConverter(type);
            var result = CreateAdapter(typeConverter);
            return result;
        }

        /// <summary>
        /// Gets default <see cref="IObjectToString"/> implementation for the
        /// specified <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="code">Type code.</param>
        public virtual IObjectToString? GetConverter(TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Empty:
                    return Default.EmptyToString;
                case TypeCode.Object:
                default:
                    return Default.ObjectToString;
                case TypeCode.DBNull:
                    return Default.DBNullToString;
                case TypeCode.Boolean:
                    return Default.BooleanToString;
                case TypeCode.Char:
                    return Default.CharToString;
                case TypeCode.SByte:
                    return Default.SByteToString;
                case TypeCode.Byte:
                    return Default.ByteToString;
                case TypeCode.Int16:
                    return Default.Int16ToString;
                case TypeCode.UInt16:
                    return Default.UInt16ToString;
                case TypeCode.Int32:
                    return Default.Int32ToString;
                case TypeCode.UInt32:
                    return Default.UInt32ToString;
                case TypeCode.Int64:
                    return Default.Int64ToString;
                case TypeCode.UInt64:
                    return Default.UInt64ToString;
                case TypeCode.Single:
                    return Default.SingleToString;
                case TypeCode.Double:
                    return Default.DoubleToString;
                case TypeCode.Decimal:
                    return Default.DecimalToString;
                case TypeCode.DateTime:
                    return Default.DateTimeToString;
                case TypeCode.String:
                    return Default.StringToString;
            }
        }

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
                catch(Exception e)
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