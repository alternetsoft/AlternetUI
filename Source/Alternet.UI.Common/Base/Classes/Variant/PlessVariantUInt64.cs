using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantUInt64 : IPlessVariantExtender
    {
        static PlessVariantUInt64()
        {
        }

        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsUInt64, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsUInt64, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider) => value.AsUInt64;

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsUInt64, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsUInt64, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsUInt64, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsUInt64, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsUInt64, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsUInt64, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsUInt64, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsUInt64, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsUInt64, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsUInt64, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsUInt64, provider);

        public object? ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsUInt64, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsUInt64);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsUInt64);

        public ulong ToUInt64(in PlessVariant value) => value.AsUInt64;

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsUInt64);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsUInt64);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsUInt64);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsUInt64);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsUInt64);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsUInt64);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsUInt64);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsUInt64);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsUInt64);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsUInt64);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsUInt64);

        public object? GetAsObject(in PlessVariant d1)
        {
            return d1.AsUInt64;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsUInt64.CompareTo(d2.AsUInt64);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsUInt64 == d2.AsUInt64;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsUInt64.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsUInt64.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsUInt64.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsUInt64.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsUInt64.ToString();
        }
    }
}
