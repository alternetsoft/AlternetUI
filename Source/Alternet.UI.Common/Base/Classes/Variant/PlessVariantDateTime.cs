using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantDateTime : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsDateTime, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsDateTime, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsDateTime, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsDateTime, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsDateTime, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => value.AsDateTime;

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsDateTime, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsDateTime, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsDateTime, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsDateTime, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsDateTime, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsDateTime, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsDateTime, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsDateTime, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsDateTime, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsDateTime);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsDateTime);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsDateTime);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsDateTime);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsDateTime);

        public DateTime ToDateTime(in PlessVariant value) => value.AsDateTime;

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsDateTime);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsDateTime);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsDateTime);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsDateTime);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsDateTime);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsDateTime);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsDateTime);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsDateTime);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsDateTime;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDateTime.CompareTo(d2.AsDateTime);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDateTime == d2.AsDateTime;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsDateTime.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsDateTime.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsDateTime.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsDateTime.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsDateTime.ToString();
        }
    }
}
