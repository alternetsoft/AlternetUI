using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantEmpty : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider) => 0;

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider) => decimal.Zero;

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider) => 0;

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider) => 0;

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider) => 0;

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider) => Convert.ToDateTime(0);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider) => 0;

        public double ToDouble(in PlessVariant value, IFormatProvider? provider) => 0;

        public short ToInt16(in PlessVariant value, IFormatProvider? provider) => 0;

        public int ToInt32(in PlessVariant value, IFormatProvider? provider) => 0;

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider) => 0;

        public float ToSingle(in PlessVariant value, IFormatProvider? provider) => 0;

        public char ToChar(in PlessVariant value, IFormatProvider? provider) => '\0';

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider) => false;

        public object? ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider) => null;

        public long ToInt64(in PlessVariant value) => 0;

        public decimal ToDecimal(in PlessVariant value) => decimal.Zero;

        public ulong ToUInt64(in PlessVariant value) => 0;

        public uint ToUInt32(in PlessVariant value) => 0;

        public ushort ToUInt16(in PlessVariant value) => 0;

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(0);

        public byte ToByte(in PlessVariant value) => 0;

        public double ToDouble(in PlessVariant value) => 0;

        public short ToInt16(in PlessVariant value) => 0;

        public int ToInt32(in PlessVariant value) => 0;

        public sbyte ToSByte(in PlessVariant value) => 0;

        public float ToSingle(in PlessVariant value) => 0;

        public char ToChar(in PlessVariant value) => '\0';

        public bool ToBoolean(in PlessVariant value) => false;

        public object? GetAsObject(in PlessVariant d1)
        {
            return null;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return 0;
            return -1;
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            return d1.ValueType == d2.ValueType;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return string.Empty;
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return string.Empty;
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return string.Empty;
        }

        public int GetHashCode(in PlessVariant value)
        {
            return string.Empty.GetHashCode();
        }

        public string ToString(in PlessVariant value)
        {
            return string.Empty;
        }
    }
}
