using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    internal partial struct PlessVariant : IPlessVariantToString, IEquatable<PlessVariant>, IComparable,
        IComparable<PlessVariant>, IFormattable, IConvertible
    {
        public const int TypeCodeCount = 19;

        public static readonly IPlessVariantExtender[] Extenders = new IPlessVariantExtender[TypeCodeCount];

        public static readonly PlessVariant Empty = new();
        public static readonly PlessVariant DBNull = GetDBNull();

        internal SimpleVariantData SimpleData;
        internal object? Data;

        private TypeCode dataType;

        static PlessVariant()
        {
            IPlessVariantExtender extenderEmpty = new PlessVariantEmpty();
            IPlessVariantExtender extenderInt = new PlessVariantInt64();
            IPlessVariantExtender extenderUInt = new PlessVariantUInt64();

            Extenders[(int)TypeCode.Empty] = extenderEmpty;
            Extenders[(int)TypeCode.DBNull] = extenderEmpty;
            Extenders[(int)TypeCode.Object] = new PlessVariantObject();
            Extenders[(int)TypeCode.Boolean] = new PlessVariantBoolean();
            Extenders[(int)TypeCode.Char] = new PlessVariantChar();
            Extenders[(int)TypeCode.Single] = new PlessVariantSingle();
            Extenders[(int)TypeCode.Double] = new PlessVariantDouble();
            Extenders[(int)TypeCode.Decimal] = new PlessVariantDecimal();
            Extenders[(int)TypeCode.DateTime] = new PlessVariantDateTime();
            Extenders[(int)TypeCode.String] = new PlessVariantString();

            Extenders[(int)TypeCode.SByte] = extenderInt;
            Extenders[(int)TypeCode.Int16] = extenderInt;
            Extenders[(int)TypeCode.Int32] = extenderInt;
            Extenders[(int)TypeCode.Int64] = extenderInt;

            Extenders[(int)TypeCode.UInt64] = extenderUInt;
            Extenders[(int)TypeCode.Byte] = extenderUInt;
            Extenders[(int)TypeCode.UInt16] = extenderUInt;
            Extenders[(int)TypeCode.UInt32] = extenderUInt;
        }

        public PlessVariant(string v)
        {
            Data = v;
            dataType = TypeCode.String;
            SimpleData = SimpleVariantData.Empty;
        }

        public PlessVariant(object v)
        {
            Data = v;
            dataType = TypeCode.Object;
            SimpleData = SimpleVariantData.Empty;
        }

        public PlessVariant(bool v)
        {
            Data = null;
            dataType = TypeCode.Boolean;
            SimpleData = new SimpleVariantData();
            SimpleData.AsBoolean = v;
        }

        public PlessVariant(char v)
        {
            Data = null;
            dataType = TypeCode.Char;
            SimpleData = new SimpleVariantData();
            SimpleData.AsChar = v;
        }

        public PlessVariant(long v)
        {
            Data = null;
            dataType = TypeCode.Int64;
            SimpleData = new SimpleVariantData();
            SimpleData.AsInt = v;
        }

        public PlessVariant(ulong v)
        {
            Data = null;
            dataType = TypeCode.UInt64;
            SimpleData = new SimpleVariantData();
            SimpleData.AsUInt = v;
        }

        public PlessVariant(float v)
        {
            Data = null;
            dataType = TypeCode.Single;
            SimpleData = new SimpleVariantData();
            SimpleData.AsSingle = v;
        }

        public PlessVariant(double v)
        {
            Data = null;
            dataType = TypeCode.Double;
            SimpleData = new SimpleVariantData();
            SimpleData.AsDouble = v;
        }

        public PlessVariant(decimal v)
        {
            Data = null;
            dataType = TypeCode.Decimal;
            SimpleData = new SimpleVariantData();
            SimpleData.AsDecimal = v;
        }

        public PlessVariant(DateTime v)
        {
            Data = null;
            dataType = TypeCode.DateTime;
            SimpleData = new SimpleVariantData();
            SimpleData.AsDateTime = v;
        }

        public bool IsEmpty
        {
            readonly get
            {
                return ValueType == TypeCode.Empty;
            }

            set
            {
                ValueType = TypeCode.Empty;
                Data = null;
            }
        }

        public bool IsDBNull
        {
            readonly get
            {
                return ValueType == TypeCode.DBNull;
            }

            set
            {
                ValueType = TypeCode.DBNull;
                Data = null;
            }
        }

        public bool IsNull
        {
            readonly get
            {
                if (IsDBNull || IsEmpty)
                    return true;
                if (Data == null && (ValueType == TypeCode.String || ValueType == TypeCode.Object))
                    return true;
                return false;
            }

            set
            {
                Data = null;
                if (ValueType == TypeCode.String || ValueType == TypeCode.Object)
                    return;
                ValueType = TypeCode.Empty;
            }
        }

        public string AsString
        {
            readonly get
            {
                if (ValueType == TypeCode.String)
                    return (string?)Data ?? string.Empty;
                return Extender.ToString();
            }

            set
            {
                Data = value;
                ValueType = TypeCode.String;
            }
        }

        public object? AsObject
        {
            readonly get
            {
                if (ValueType == TypeCode.Object)
                    return Data;
                return Extender.GetAsObject(this);
            }

            set
            {
                Data = value;
                ValueType = TypeCode.Object;
            }
        }

        public bool AsBoolean
        {
            readonly get
            {
                return SimpleData.AsBoolean;
            }

            set
            {
                Data = null;
                SimpleData.AsBoolean = value;
                ValueType = TypeCode.Boolean;
            }
        }

        public char AsChar
        {
            readonly get
            {
                return SimpleData.AsChar;
            }

            set
            {
                Data = null;
                SimpleData.AsChar = value;
                ValueType = TypeCode.Char;
            }
        }

        public long AsInt
        {
            readonly get
            {
                return SimpleData.AsInt;
            }

            set
            {
                Data = null;
                SimpleData.AsInt = value;
                ValueType = TypeCode.Int64;
            }
        }

        public ulong AsUInt
        {
            readonly get
            {
                return SimpleData.AsUInt;
            }

            set
            {
                Data = null;
                SimpleData.AsUInt = value;
                ValueType = TypeCode.UInt64;
            }
        }

        public float AsSingle
        {
            readonly get
            {
                return SimpleData.AsSingle;
            }

            set
            {
                Data = null;
                SimpleData.AsSingle = value;
                ValueType = TypeCode.Single;
            }
        }

        public double AsDouble
        {
            readonly get
            {
                return SimpleData.AsDouble;
            }

            set
            {
                Data = null;
                SimpleData.AsDouble = value;
                ValueType = TypeCode.Double;
            }
        }

        public decimal AsDecimal
        {
            readonly get
            {
                return SimpleData.AsDecimal;
            }

            set
            {
                Data = null;
                SimpleData.AsDecimal = value;
                ValueType = TypeCode.Decimal;
            }
        }

        public DateTime AsDateTime
        {
            readonly get
            {
                return SimpleData.AsDateTime;
            }

            set
            {
                Data = null;
                SimpleData.AsDateTime = value;
                ValueType = TypeCode.DateTime;
            }
        }

        public readonly IPlessVariantExtender Extender
        {
            get => Extenders[(int)ValueType];
        }

        internal TypeCode ValueType
        {
            readonly get
            {
                return dataType;
            }

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
