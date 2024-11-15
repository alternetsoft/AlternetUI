using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    internal partial interface IPlessVariantExtender
    {
        string ToString(in PlessVariant value, IFormatProvider provider);

        string ToString(in PlessVariant value, string format, IFormatProvider provider);

        string ToString(in PlessVariant value, string format);

        string ToString(in PlessVariant value);

        int GetHashCode(in PlessVariant value);

        bool Equals(in PlessVariant d1, in PlessVariant d2);

        int Compare(in PlessVariant d1, in PlessVariant d2);

        object? GetAsObject(in PlessVariant d1);

        long ToInt64(in PlessVariant value);

        decimal ToDecimal(in PlessVariant value);

        ulong ToUInt64(in PlessVariant value);

        uint ToUInt32(in PlessVariant value);

        ushort ToUInt16(in PlessVariant value);

        DateTime ToDateTime(in PlessVariant value);

        byte ToByte(in PlessVariant value);

        char ToChar(in PlessVariant value);

        double ToDouble(in PlessVariant value);

        short ToInt16(in PlessVariant value);

        int ToInt32(in PlessVariant value);

        sbyte ToSByte(in PlessVariant value);

        float ToSingle(in PlessVariant value);

        bool ToBoolean(in PlessVariant value);

        bool ToBoolean(in PlessVariant value, IFormatProvider provider);

        byte ToByte(in PlessVariant value, IFormatProvider provider);

        char ToChar(in PlessVariant value, IFormatProvider provider);

        DateTime ToDateTime(in PlessVariant value, IFormatProvider provider);

        decimal ToDecimal(in PlessVariant value, IFormatProvider provider);

        double ToDouble(in PlessVariant value, IFormatProvider provider);

        short ToInt16(in PlessVariant value, IFormatProvider provider);

        int ToInt32(in PlessVariant value, IFormatProvider provider);

        long ToInt64(in PlessVariant value, IFormatProvider provider);

        sbyte ToSByte(in PlessVariant value, IFormatProvider provider);

        float ToSingle(in PlessVariant value, IFormatProvider provider);

        object? ToType(in PlessVariant value, Type conversionType, IFormatProvider provider);

        ushort ToUInt16(in PlessVariant value, IFormatProvider provider);

        uint ToUInt32(in PlessVariant value, IFormatProvider provider);

        ulong ToUInt64(in PlessVariant value, IFormatProvider provider);
    }
}
