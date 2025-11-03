using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantChar : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsChar, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsChar, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsChar, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsChar, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsChar, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsChar, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsChar, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsChar, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsChar, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsChar, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsChar, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsChar, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => value.AsChar;

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsChar, provider);

        public object ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsChar, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsChar);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsChar);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsChar);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsChar);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsChar);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsChar);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsChar);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsChar);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsChar);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsChar);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsChar);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsChar);

        public char ToChar(in PlessVariant value) => value.AsChar;

        public bool ToBoolean(in PlessVariant value)
            => value.AsChar == 'T' || value.AsChar == 't';

        public object GetAsObject(in PlessVariant d1)
        {
            return d1.AsChar;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsChar.CompareTo(d2.AsChar);
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.AsChar == d2.AsChar;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsChar.ToString();
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsChar.ToString(provider);
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsChar.ToString(provider);
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsChar.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsChar.ToString();
        }
    }
}
