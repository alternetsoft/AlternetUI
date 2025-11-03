using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantDecimal : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsDecimal, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => value.AsDecimal;

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsDecimal, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsDecimal, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsDecimal, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsDecimal, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsDecimal, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsDecimal, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsDecimal, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsDecimal, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsDecimal, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsDecimal, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsDecimal, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsDecimal, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsDecimal, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsDecimal);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsDecimal);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsDecimal);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsDecimal);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsDecimal);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsDecimal);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsDecimal);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsDecimal);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsDecimal);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsDecimal);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsDecimal);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsDecimal);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsDecimal);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsDecimal);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsDecimal;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDecimal.CompareTo(d2.AsDecimal);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDecimal == d2.AsDecimal;
            try
            {
                return d1.AsDecimal == d2.ToDecimal();
            }
            catch
            {
                return false;
            }
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsDecimal.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsDecimal.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsDecimal.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsDecimal.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsDecimal.ToString();
        }
    }
}
