using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantDouble : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsDouble, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsDouble, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsDouble, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsDouble, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsDouble, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsDouble, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsDouble, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => value.AsDouble;

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsDouble, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsDouble, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsDouble, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsDouble, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsDouble, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsDouble, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsDouble, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsDouble);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsDouble);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsDouble);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsDouble);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsDouble);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsDouble);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsDouble);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsDouble);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsDouble);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsDouble);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsDouble);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsDouble);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsDouble);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsDouble);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsDouble;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDouble.CompareTo(d2.AsDouble);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsDouble == d2.AsDouble;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsDouble.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsDouble.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsDouble.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsDouble.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsDouble.ToString();
        }
    }
}
