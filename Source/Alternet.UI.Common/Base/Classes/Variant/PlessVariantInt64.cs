using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantInt64 : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider provider) => value.AsInt;

        public decimal ToDecimal(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDecimal(value.AsInt, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider provider)
            => Convert.ToUInt64(value.AsInt, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider provider)
            => Convert.ToUInt32(value.AsInt, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider provider)
            => Convert.ToUInt16(value.AsInt, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDateTime(value.AsInt, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider provider)
            => Convert.ToByte(value.AsInt, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDouble(value.AsInt, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider provider)
            => Convert.ToInt16(value.AsInt, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider provider)
            => Convert.ToInt32(value.AsInt, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider provider)
            => Convert.ToSByte(value.AsInt, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider provider)
            => Convert.ToSingle(value.AsInt, provider);

        public char ToChar(in PlessVariant value, IFormatProvider provider)
            => Convert.ToChar(value.AsInt, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider provider)
            => Convert.ToBoolean(value.AsInt, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(value.AsInt, conversionType, provider);

        public long ToInt64(in PlessVariant value) => value.AsInt;

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsInt);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsInt);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsInt);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsInt);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsInt);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsInt);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsInt);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsInt);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsInt);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsInt);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsInt);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsInt);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsInt);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsInt;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsInt.CompareTo(d2.AsInt);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsInt == d2.AsInt;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsInt.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider provider)
        {
            return value.AsInt.ToString(provider);
        }

        public string ToString(in PlessVariant value, string format, IFormatProvider provider)
        {
            return value.AsInt.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsInt.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsInt.ToString();
        }
    }
}
