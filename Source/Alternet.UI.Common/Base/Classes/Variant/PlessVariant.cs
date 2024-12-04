using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements variant structure which can contain data with different types.
    /// </summary>
    internal partial struct PlessVariant : IValueToString, IEquatable<PlessVariant>, IComparable,
        IComparable<PlessVariant>, IFormattable, IConvertible
    {
        /// <summary>
        /// Gets an empty variant.
        /// </summary>
        public static readonly PlessVariant Empty = new();

        /// <summary>
        /// Gets variant as value with <see cref="TypeCode.DBNull"/> type.
        /// </summary>
        public static readonly PlessVariant DBNull = GetDBNull();

        internal SimpleVariantData SimpleData;
        internal object? Data;

        private const int TypeCodeCount = 19;

        private static readonly IPlessVariantExtender[] Extenders
            = new IPlessVariantExtender[TypeCodeCount];

        private TypeCode dataType;

        static PlessVariant()
        {
            IPlessVariantExtender extenderEmpty = new PlessVariantEmpty();
            IPlessVariantExtender extenderInt = new PlessVariantInt64();
            IPlessVariantExtender extenderUInt = new PlessVariantUInt64();

            SetVariantExtender(TypeCode.Empty, extenderEmpty);
            SetVariantExtender(TypeCode.DBNull, extenderEmpty);
            SetVariantExtender(TypeCode.Object, new PlessVariantObject());
            SetVariantExtender(TypeCode.Boolean, new PlessVariantBoolean());
            SetVariantExtender(TypeCode.Char, new PlessVariantChar());
            SetVariantExtender(TypeCode.Single, new PlessVariantSingle());
            SetVariantExtender(TypeCode.Double, new PlessVariantDouble());
            SetVariantExtender(TypeCode.Decimal, new PlessVariantDecimal());
            SetVariantExtender(TypeCode.DateTime, new PlessVariantDateTime());
            SetVariantExtender(TypeCode.String, new PlessVariantString());

            SetVariantExtender(TypeCode.SByte, extenderInt);
            SetVariantExtender(TypeCode.Int16, extenderInt);
            SetVariantExtender(TypeCode.Int32, extenderInt);
            SetVariantExtender(TypeCode.Int64, extenderInt);

            SetVariantExtender(TypeCode.UInt64, extenderUInt);
            SetVariantExtender(TypeCode.Byte, extenderUInt);
            SetVariantExtender(TypeCode.UInt16, extenderUInt);
            SetVariantExtender(TypeCode.UInt32, extenderUInt);
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="string"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(string v)
        {
            Data = v;
            dataType = TypeCode.String;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="object"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(object v)
        {
            Data = v;
            dataType = TypeCode.Object;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="bool"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(bool v)
        {
            dataType = TypeCode.Boolean;
            SimpleData.AsBoolean = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="char"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(char v)
        {
            dataType = TypeCode.Char;
            SimpleData.AsChar = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="long"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(long v)
        {
            dataType = TypeCode.Int64;
            SimpleData.AsInt = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="ulong"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(ulong v)
        {
            dataType = TypeCode.UInt64;
            SimpleData.AsUInt = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="float"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(float v)
        {
            dataType = TypeCode.Single;
            SimpleData.AsSingle = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="double"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(double v)
        {
            dataType = TypeCode.Double;
            SimpleData.AsDouble = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="decimal"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(decimal v)
        {
            dataType = TypeCode.Decimal;
            SimpleData.AsDecimal = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="DateTime"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(DateTime v)
        {
            dataType = TypeCode.DateTime;
            SimpleData.AsDateTime = v;
        }

        /// <summary>
        /// Gets whether variant has <see cref="TypeCode.Empty"/> type.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return ValueType == TypeCode.Empty;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ValueType = TypeCode.Empty;
                Data = null;
            }
        }

        /// <summary>
        /// Gets whether variant has <see cref="TypeCode.DBNull"/> type.
        /// </summary>
        public bool IsDBNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return ValueType == TypeCode.DBNull;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ValueType = TypeCode.DBNull;
                Data = null;
            }
        }

        /// <summary>
        /// Gets whether variant is null.
        /// </summary>
        public bool IsNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (IsDBNull || IsEmpty)
                    return true;
                if (Data == null && (ValueType == TypeCode.String || ValueType == TypeCode.Object))
                    return true;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                if (ValueType == TypeCode.String || ValueType == TypeCode.Object)
                    return;
                ValueType = TypeCode.Empty;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="string"/>.
        /// </summary>
        public string AsString
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (ValueType == TypeCode.String)
                    return (string?)Data ?? string.Empty;
                return Extender.ToString();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = value;
                ValueType = TypeCode.String;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="object"/>.
        /// </summary>
        public object? AsObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (ValueType == TypeCode.Object)
                    return Data;
                return Extender.GetAsObject(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = value;
                ValueType = TypeCode.Object;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="bool"/>.
        /// </summary>
        public bool AsBoolean
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsBoolean;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsBoolean = value;
                ValueType = TypeCode.Boolean;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="char"/>.
        /// </summary>
        public char AsChar
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsChar;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsChar = value;
                ValueType = TypeCode.Char;
            }
        }

        public long AsInt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsInt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsInt = value;
                ValueType = TypeCode.Int64;
            }
        }

        public ulong AsUInt
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsUInt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsUInt = value;
                ValueType = TypeCode.UInt64;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="float"/>.
        /// </summary>
        public float AsSingle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsSingle;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsSingle = value;
                ValueType = TypeCode.Single;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="double"/>.
        /// </summary>
        public double AsDouble
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDouble;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDouble = value;
                ValueType = TypeCode.Double;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="decimal"/>.
        /// </summary>
        public decimal AsDecimal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDecimal;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDecimal = value;
                ValueType = TypeCode.Decimal;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="DateTime"/>.
        /// </summary>
        public DateTime AsDateTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDateTime;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDateTime = value;
                ValueType = TypeCode.DateTime;
            }
        }

        /// <summary>
        /// Gets variant extender implementation.
        /// </summary>
        public readonly IPlessVariantExtender Extender
        {
            get => Extenders[(int)ValueType];
        }

        internal TypeCode ValueType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return dataType;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                dataType = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator decimal(PlessVariant value)
            => GetExtender(value.ValueType).ToDecimal(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DateTime(PlessVariant value)
            => GetExtender(value.ValueType).ToDateTime(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(PlessVariant value)
            => GetExtender(value.ValueType).ToSByte(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(PlessVariant value)
            => GetExtender(value.ValueType).ToByte(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator char(PlessVariant value)
            => GetExtender(value.ValueType).ToChar(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(PlessVariant value)
            => GetExtender(value.ValueType).ToDouble(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(PlessVariant value)
            => GetExtender(value.ValueType).ToSingle(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(PlessVariant value)
            => GetExtender(value.ValueType).ToInt64(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(PlessVariant value)
            => GetExtender(value.ValueType).ToInt32(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt64(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt32(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt16(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(PlessVariant value)
            => GetExtender(value.ValueType).ToInt16(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator bool(PlessVariant value)
            => GetExtender(value.ValueType).ToBoolean(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator string(PlessVariant value)
        {
            return GetExtender(value.ValueType).ToString(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(bool b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(char b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(float b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(double b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(decimal b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(DateTime b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(string b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(byte b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(short b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(sbyte b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(ushort b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(long b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(ulong b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(uint b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(int b) => new(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) < 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) > 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PlessVariant d1, PlessVariant d2)
        {
            return Equals(d1, d2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PlessVariant d1, PlessVariant d2)
        {
            return !Equals(d1, d2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) <= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Equals(d1, d2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPlessVariantExtender GetExtender(TypeCode valueType)
        {
            return Extenders[(int)valueType];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Compare(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2);
        }

        public static IPlessVariantExtender GetVariantExtender(TypeCode typeCode)
        {
            return Extenders[(int)typeCode];
        }

        public static void SetVariantExtender(TypeCode typeCode, IPlessVariantExtender extender)
        {
            Extenders[(int)typeCode] = extender;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(PlessVariant value)
        {
            return Compare(this, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(object value)
        {
            if (value == null)
                return 1;
            if (value is PlessVariant variant)
                return CompareTo(variant);
            throw new ArgumentException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ToBoolean() => Extender.ToBoolean(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly byte ToByte() => Extender.ToByte(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly char ToChar() => Extender.ToChar(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly DateTime ToDateTime() => Extender.ToDateTime(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly decimal ToDecimal() => Extender.ToDecimal(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double ToDouble() => Extender.ToDouble(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly short ToInt16() => Extender.ToInt16(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ToInt32() => Extender.ToInt32(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long ToInt64() => Extender.ToInt64(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly sbyte ToSByte() => Extender.ToSByte(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ToSingle() => Extender.ToSingle(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ushort ToUInt16() => Extender.ToUInt16(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToUInt32() => Extender.ToUInt32(this);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong ToUInt64() => Extender.ToUInt64(this);

        public readonly override int GetHashCode()
        {
            return Extender.GetHashCode(this);
        }

        public override readonly bool Equals(object value)
        {
            if (value is not PlessVariant v)
                return false;
            return Equals(this, v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PlessVariant value)
        {
            return Equals(this, value);
        }

        public readonly override string ToString()
        {
            return Extender.ToString(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string format)
        {
            return Extender.ToString(this, format);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(IFormatProvider provider)
        {
            return Extender.ToString(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string format, IFormatProvider provider)
        {
            return Extender.ToString(this, format, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TypeCode GetTypeCode()
        {
            return ValueType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ToBoolean(IFormatProvider provider)
        {
            return Extender.ToBoolean(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly byte ToByte(IFormatProvider provider)
        {
            return Extender.ToByte(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly char ToChar(IFormatProvider provider)
        {
            return Extender.ToChar(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly DateTime ToDateTime(IFormatProvider provider)
        {
            return Extender.ToDateTime(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly decimal ToDecimal(IFormatProvider provider)
        {
            return Extender.ToDecimal(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double ToDouble(IFormatProvider provider)
        {
            return Extender.ToDouble(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly short ToInt16(IFormatProvider provider)
        {
            return Extender.ToInt16(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ToInt32(IFormatProvider provider)
        {
            return Extender.ToInt32(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long ToInt64(IFormatProvider provider)
        {
            return Extender.ToInt64(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly sbyte ToSByte(IFormatProvider provider)
        {
            return Extender.ToSByte(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ToSingle(IFormatProvider provider)
        {
            return Extender.ToSingle(this, provider);
        }

        readonly object? IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Extender.ToType(this, conversionType, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ushort ToUInt16(IFormatProvider provider)
        {
            return Extender.ToUInt16(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToUInt32(IFormatProvider provider)
        {
            return Extender.ToUInt32(this, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong ToUInt64(IFormatProvider provider)
        {
            return Extender.ToUInt64(this, provider);
        }

        private static PlessVariant GetDBNull()
        {
            PlessVariant result = new();
            result.IsDBNull = true;
            return result;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct SimpleVariantData
        {
            public static readonly SimpleVariantData Empty = new();

            [FieldOffset(0)]
            public bool AsBoolean;

            [FieldOffset(0)]
            public char AsChar;

            [FieldOffset(0)]
            public long AsInt;

            [FieldOffset(0)]
            public ulong AsUInt;

            [FieldOffset(0)]
            public float AsSingle;

            [FieldOffset(0)]
            public double AsDouble;

            [FieldOffset(0)]
            public decimal AsDecimal;

            [FieldOffset(0)]
            public DateTime AsDateTime;
        }
    }
}
