using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantString : IPlessVariantExtender
    {
        static PlessVariantString()
        {
        }

        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsString, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsString, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsString, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsString, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsString, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsString, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsString, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsString, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsString, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsString, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsString, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsString, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsString, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsString, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsString, conversionType, provider);

        public long ToInt64(in PlessVariant value) => long.Parse(value.AsString);

        public decimal ToDecimal(in PlessVariant value) => decimal.Parse(value.AsString);

        public ulong ToUInt64(in PlessVariant value) => ulong.Parse(value.AsString);

        public uint ToUInt32(in PlessVariant value) => uint.Parse(value.AsString);

        public ushort ToUInt16(in PlessVariant value) => ushort.Parse(value.AsString);

        public DateTime ToDateTime(in PlessVariant value) => DateTime.Parse(value.AsString);

        public byte ToByte(in PlessVariant value) => byte.Parse(value.AsString);

        public double ToDouble(in PlessVariant value) => double.Parse(value.AsString);

        public short ToInt16(in PlessVariant value) => short.Parse(value.AsString);

        public int ToInt32(in PlessVariant value) => int.Parse(value.AsString);

        public sbyte ToSByte(in PlessVariant value) => sbyte.Parse(value.AsString);

        public float ToSingle(in PlessVariant value) => float.Parse(value.AsString);

        public bool ToBoolean(in PlessVariant value) => bool.Parse(value.AsString);

        public char ToChar(in PlessVariant value)
        {
            var s = value.AsString;

            if (s == null || s.Length == 0)
                return '\0';
            return s[0];
        }

        public object? GetAsObject(in PlessVariant d1)
        {
            return d1.Data;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsString.CompareTo(d2.AsString);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return ((string?)d1.Data) == ((string?)d2.Data);
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsString;
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsString.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsString.ToString(provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsString.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsString;
        }
    }
}
