using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantBoolean : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsBoolean, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsBoolean, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsBoolean, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsBoolean, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsBoolean, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsBoolean, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsBoolean, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsBoolean, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsBoolean, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsBoolean, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsBoolean, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsBoolean, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsBoolean, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider) => value.AsBoolean;

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsBoolean, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsBoolean);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsBoolean);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsBoolean);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsBoolean);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsBoolean);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsBoolean);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsBoolean);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsBoolean);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsBoolean);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsBoolean);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsBoolean);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsBoolean);

        public char ToChar(in PlessVariant value) => value.AsBoolean ? 'T' : 'F';

        public bool ToBoolean(in PlessVariant value) => value.AsBoolean;

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsBoolean;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsBoolean.CompareTo(d2.AsBoolean);
            return string.Compare(d1.ToString(), d2.ToString());
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsBoolean == d2.AsBoolean;
            return d1.ToString() == d2.ToString();
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsBoolean.ToString();
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsBoolean.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsBoolean.ToString(provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsBoolean.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsBoolean.ToString();
        }
    }
}
