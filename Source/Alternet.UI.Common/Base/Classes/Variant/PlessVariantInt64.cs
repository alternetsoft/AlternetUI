using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantInt64 : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider) => value.AsInt64;

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsInt64, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsInt64, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsInt64, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsInt64, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsInt64, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsInt64, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsInt64, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsInt64, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsInt64, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsInt64, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsInt64, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsInt64, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsInt64, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsInt64, conversionType, provider);

        public long ToInt64(in PlessVariant value) => value.AsInt64;

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsInt64);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsInt64);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsInt64);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsInt64);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsInt64);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsInt64);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsInt64);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsInt64);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsInt64);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsInt64);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsInt64);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsInt64);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsInt64);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsInt64;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsInt64.CompareTo(d2.AsInt64);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsInt64 == d2.AsInt64;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsInt64.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsInt64.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsInt64.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsInt64.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsInt64.ToString();
        }
    }
}
