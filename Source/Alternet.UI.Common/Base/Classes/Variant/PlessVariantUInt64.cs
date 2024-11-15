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

        public long ToInt64(in PlessVariant value, IFormatProvider provider)
            => Convert.ToInt64(value.AsUInt, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDecimal(value.AsUInt, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider provider) => value.AsUInt;

        public uint ToUInt32(in PlessVariant value, IFormatProvider provider)
            => Convert.ToUInt32(value.AsUInt, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider provider)
            => Convert.ToUInt16(value.AsUInt, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDateTime(value.AsUInt, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider provider)
            => Convert.ToByte(value.AsUInt, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider provider)
            => Convert.ToDouble(value.AsUInt, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider provider)
            => Convert.ToInt16(value.AsUInt, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider provider)
            => Convert.ToInt32(value.AsUInt, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider provider)
            => Convert.ToSByte(value.AsUInt, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider provider)
            => Convert.ToSingle(value.AsUInt, provider);

        public char ToChar(in PlessVariant value, IFormatProvider provider)
            => Convert.ToChar(value.AsUInt, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider provider)
            => Convert.ToBoolean(value.AsUInt, provider);

        public object? ToType(in PlessVariant value, Type conversionType, IFormatProvider provider)
            => Convert.ChangeType(value.AsUInt, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsUInt);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsUInt);

        public ulong ToUInt64(in PlessVariant value) => value.AsUInt;

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsUInt);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsUInt);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsUInt);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsUInt);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsUInt);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsUInt);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsUInt);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsUInt);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsUInt);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsUInt);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsUInt);

        public object? GetAsObject(in PlessVariant d1)
        {
            return d1.AsUInt;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsUInt.CompareTo(d2.AsUInt);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsUInt == d2.AsUInt;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsUInt.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider provider)
        {
            return value.AsUInt.ToString(provider);
        }

        public string ToString(in PlessVariant value, string format, IFormatProvider provider)
        {
            return value.AsUInt.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsUInt.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsUInt.ToString();
        }
    }
}
