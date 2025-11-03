using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantSingle : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsSingle, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsSingle, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsSingle, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsSingle, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsSingle, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsSingle, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsSingle, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsSingle, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsSingle, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsSingle, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsSingle, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => value.AsSingle;

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsSingle, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsSingle, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsSingle, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsSingle);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsSingle);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsSingle);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsSingle);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsSingle);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsSingle);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsSingle);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsSingle);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsSingle);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsSingle);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsSingle);

        public float ToSingle(in PlessVariant value) => value.AsSingle;

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsSingle);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsSingle);

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsSingle;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsSingle.CompareTo(d2.AsSingle);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.SimpleData.AsSingle == d2.SimpleData.AsSingle;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsSingle.ToString(format);
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsSingle.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsSingle.ToString(format, provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsSingle.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsSingle.ToString();
        }
    }
}
