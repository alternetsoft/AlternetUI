using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods which implement different operations with
    /// the <see cref="PlessVariant"/> structure. Use
    /// <see cref="PlessVariant.SetExtender"/> in order to set custom
    /// variant extender for the specified variant type.
    /// </summary>
    public partial interface IPlessVariantExtender
    {
        /// <summary>
        /// Converts the variant to its equivalent <see cref="string"/> representation
        /// using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="provider"/>.</returns>
        /// <param name="value">Variant to convert.</param>
        string ToString(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Converts the variant to its equivalent <see cref="string"/>
        /// representation using
        /// the specified format and culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="format"/> and <paramref name="provider"/>.</returns>
        /// <param name="value">Variant to convert.</param>
        string ToString(in PlessVariant value, string? format, IFormatProvider? provider);

        /// <summary>
        /// Converts the variant to its equivalent <see cref="string"/> representation,
        /// using the specified format.
        /// </summary>
        /// <param name="format">A standard or custom format string.</param>
        /// <returns>The <see cref="string"/> representation of the value as
        /// specified by <paramref name="format"/>.</returns>
        /// <param name="value">Variant to convert.</param>
        string ToString(in PlessVariant value, string format);

        /// <summary>
        /// Converts the variant to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <returns>The <see cref="string"/> representation of the value.</returns>
        /// <param name="value">Variant to convert.</param>
        string ToString(in PlessVariant value);

        /// <summary>
        /// Serves as a hash function for the variant.
        /// </summary>
        /// <returns>A hash code for the specified variant value.</returns>
        int GetHashCode(in PlessVariant value);

        /// <summary>
        /// Gets whether two variants are equal.
        /// </summary>
        /// <param name="d1">First variant to compare.</param>
        /// <param name="d2">Second variant to compare.</param>
        /// <returns></returns>
        bool Equals(in PlessVariant d1, in PlessVariant d2);

        /// <summary>
        /// Compares two variants.
        /// </summary>
        /// <param name="d1">First variant to compare.</param>
        /// <param name="d2">Second variant to compare.</param>
        /// <returns></returns>
        int Compare(in PlessVariant d1, in PlessVariant d2);

        /// <summary>
        /// Gets variant as <see cref="object"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        object? GetAsObject(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="long"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        long ToInt64(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="decimal"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        decimal ToDecimal(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="ulong"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        ulong ToUInt64(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="uint"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        uint ToUInt32(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="ushort"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        ushort ToUInt16(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="DateTime"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        DateTime ToDateTime(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="byte"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        byte ToByte(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="char"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        char ToChar(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="double"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        double ToDouble(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="short"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        short ToInt16(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="int"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        int ToInt32(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="sbyte"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        sbyte ToSByte(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="float"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        float ToSingle(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="bool"/>.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <returns></returns>
        bool ToBoolean(in PlessVariant value);

        /// <summary>
        /// Gets variant as <see cref="bool"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        bool ToBoolean(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="byte"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        byte ToByte(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="char"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        char ToChar(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="DateTime"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        DateTime ToDateTime(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="decimal"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        decimal ToDecimal(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="double"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        double ToDouble(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="short"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        short ToInt16(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="int"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        int ToInt32(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="long"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        long ToInt64(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="sbyte"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        sbyte ToSByte(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="float"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        float ToSingle(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="object"/> of the specified type using format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        object? ToType(in PlessVariant value, Type resultType, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="ushort"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        ushort ToUInt16(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="uint"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        uint ToUInt32(in PlessVariant value, IFormatProvider? provider);

        /// <summary>
        /// Gets variant as <see cref="ulong"/> using the specified format provider.
        /// </summary>
        /// <param name="value">Variant to convert.</param>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        ulong ToUInt64(in PlessVariant value, IFormatProvider? provider);
    }
}
