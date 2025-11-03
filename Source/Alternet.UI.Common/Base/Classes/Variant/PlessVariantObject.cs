using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal class PlessVariantObject : IPlessVariantExtender
    {
        public long ToInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt64(value.AsObject, provider);

        public decimal ToDecimal(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDecimal(value.AsObject, provider);

        public ulong ToUInt64(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt64(value.AsObject, provider);

        public uint ToUInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt32(value.AsObject, provider);

        public ushort ToUInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToUInt16(value.AsObject, provider);

        public DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDateTime(value.AsObject, provider);

        public byte ToByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToByte(value.AsObject, provider);

        public double ToDouble(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToDouble(value.AsObject, provider);

        public short ToInt16(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt16(value.AsObject, provider);

        public int ToInt32(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToInt32(value.AsObject, provider);

        public sbyte ToSByte(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSByte(value.AsObject, provider);

        public float ToSingle(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToSingle(value.AsObject, provider);

        public char ToChar(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToChar(value.AsObject, provider);

        public bool ToBoolean(in PlessVariant value, IFormatProvider? provider)
            => Convert.ToBoolean(value.AsObject, provider);

        public object? ToType(in PlessVariant value, Type conversionType, IFormatProvider? provider)
            => Convert.ChangeType(value.AsObject, conversionType, provider);

        public long ToInt64(in PlessVariant value) => Convert.ToInt64(value.AsObject);

        public decimal ToDecimal(in PlessVariant value) => Convert.ToDecimal(value.AsObject);

        public ulong ToUInt64(in PlessVariant value) => Convert.ToUInt64(value.AsObject);

        public uint ToUInt32(in PlessVariant value) => Convert.ToUInt32(value.AsObject);

        public ushort ToUInt16(in PlessVariant value) => Convert.ToUInt16(value.AsObject);

        public DateTime ToDateTime(in PlessVariant value) => Convert.ToDateTime(value.AsObject);

        public byte ToByte(in PlessVariant value) => Convert.ToByte(value.AsObject);

        public double ToDouble(in PlessVariant value) => Convert.ToDouble(value.AsObject);

        public short ToInt16(in PlessVariant value) => Convert.ToInt16(value.AsObject);

        public int ToInt32(in PlessVariant value) => Convert.ToInt32(value.AsObject);

        public sbyte ToSByte(in PlessVariant value) => Convert.ToSByte(value.AsObject);

        public float ToSingle(in PlessVariant value) => Convert.ToSingle(value.AsObject);

        public char ToChar(in PlessVariant value) => Convert.ToChar(value.AsObject);

        public bool ToBoolean(in PlessVariant value) => Convert.ToBoolean(value.AsObject);

        public object? GetAsObject(in PlessVariant d1)
        {
            return d1.Data;
        }

        public int Compare(in PlessVariant d1, in PlessVariant d2)
        {
            throw new ArgumentException();
        }

        public bool Equals(in PlessVariant d1, in PlessVariant d2)
        {
            if (d1.ValueType == d2.ValueType)
                return d1.Data == d2.Data;
            return false;
        }

        public string ToString(in PlessVariant value, string format)
        {
            return value.AsObject?.ToString() ?? string.Empty;
        }

        public string ToString(in PlessVariant value, IFormatProvider? provider)
        {
            return value.AsObject?.ToString() ?? string.Empty;
        }

        public string ToString(in PlessVariant value, string? format, IFormatProvider? provider)
        {
            return value.AsObject?.ToString() ?? string.Empty;
        }

        public int GetHashCode(in PlessVariant value)
        {
            return value.AsObject?.GetHashCode() ?? 0;
        }

        public string ToString(in PlessVariant value)
        {
            return value.AsObject?.ToString() ?? string.Empty;
        }
    }
}
