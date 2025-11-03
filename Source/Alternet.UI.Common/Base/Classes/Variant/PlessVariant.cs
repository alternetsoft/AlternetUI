using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Implements variant structure which can contain data with different types.
    /// </summary>
    public partial struct PlessVariant : IValueToString, IEquatable<PlessVariant>, IComparable,
        IComparable<PlessVariant>, IFormattable, IConvertible
    {
        /// <summary>
        /// Gets an empty variant.
        /// </summary>
        public static readonly PlessVariant Empty = new();

        /// <summary>
        /// Gets variant as value with <see cref="TypeCode.DBNull"/> type.
        /// </summary>
        public static readonly PlessVariant DBNull = GetDBNull();

        internal SimpleVariantData SimpleData;
        internal object? Data;

        private const int TypeCodeCount = 19;

        private static readonly IPlessVariantExtender[] Extenders
            = new IPlessVariantExtender[TypeCodeCount];

        private TypeCode dataType;

        static PlessVariant()
        {
            IPlessVariantExtender extenderEmpty = new PlessVariantEmpty();
            IPlessVariantExtender extenderInt = new PlessVariantInt64();
            IPlessVariantExtender extenderUInt = new PlessVariantUInt64();

            SetExtender(TypeCode.Empty, extenderEmpty);
            SetExtender(TypeCode.DBNull, extenderEmpty);
            SetExtender(TypeCode.Object, new PlessVariantObject());
            SetExtender(TypeCode.Boolean, new PlessVariantBoolean());
            SetExtender(TypeCode.Char, new PlessVariantChar());
            SetExtender(TypeCode.Single, new PlessVariantSingle());
            SetExtender(TypeCode.Double, new PlessVariantDouble());
            SetExtender(TypeCode.Decimal, new PlessVariantDecimal());
            SetExtender(TypeCode.DateTime, new PlessVariantDateTime());
            SetExtender(TypeCode.String, new PlessVariantString());

            SetExtender(TypeCode.SByte, extenderInt);
            SetExtender(TypeCode.Int16, extenderInt);
            SetExtender(TypeCode.Int32, extenderInt);
            SetExtender(TypeCode.Int64, extenderInt);

            SetExtender(TypeCode.UInt64, extenderUInt);
            SetExtender(TypeCode.Byte, extenderUInt);
            SetExtender(TypeCode.UInt16, extenderUInt);
            SetExtender(TypeCode.UInt32, extenderUInt);
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="string"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(string v)
        {
            Data = v;
            dataType = TypeCode.String;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="object"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(object v)
        {
            Data = v;
            dataType = TypeCode.Object;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="bool"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(bool v)
        {
            dataType = TypeCode.Boolean;
            SimpleData.AsBoolean = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="char"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(char v)
        {
            dataType = TypeCode.Char;
            SimpleData.AsChar = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="long"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(long v)
        {
            dataType = TypeCode.Int64;
            SimpleData.AsInt = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="ulong"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(ulong v)
        {
            dataType = TypeCode.UInt64;
            SimpleData.AsUInt = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="float"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(float v)
        {
            dataType = TypeCode.Single;
            SimpleData.AsSingle = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="double"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(double v)
        {
            dataType = TypeCode.Double;
            SimpleData.AsDouble = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="decimal"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(decimal v)
        {
            dataType = TypeCode.Decimal;
            SimpleData.AsDecimal = v;
        }

        /// <summary>
        /// Creates variant with the default value of the <see cref="DateTime"/> type.
        /// </summary>
        /// <param name="v">Default value.</param>
        public PlessVariant(DateTime v)
        {
            dataType = TypeCode.DateTime;
            SimpleData.AsDateTime = v;
        }

        /// <summary>
        /// Gets whether variant has <see cref="TypeCode.Empty"/> type.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return ValueType == TypeCode.Empty;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ValueType = TypeCode.Empty;
                Data = null;
            }
        }

        /// <summary>
        /// Gets whether variant has <see cref="TypeCode.DBNull"/> type.
        /// </summary>
        public bool IsDBNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return ValueType == TypeCode.DBNull;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                ValueType = TypeCode.DBNull;
                Data = null;
            }
        }

        /// <summary>
        /// Gets whether variant is null.
        /// </summary>
        public bool IsNull
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (IsDBNull || IsEmpty)
                    return true;
                if (Data == null && (ValueType == TypeCode.String || ValueType == TypeCode.Object))
                    return true;
                return false;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                if (ValueType == TypeCode.String || ValueType == TypeCode.Object)
                    return;
                ValueType = TypeCode.Empty;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="string"/>.
        /// </summary>
        public string AsString
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (ValueType == TypeCode.String)
                    return (string?)Data ?? string.Empty;
                return Extender.ToString() ?? string.Empty;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = value;
                ValueType = TypeCode.String;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="object"/>.
        /// </summary>
        public object? AsObject
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                if (ValueType == TypeCode.Object)
                    return Data;
                return Extender.GetAsObject(this);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = value;
                ValueType = TypeCode.Object;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="bool"/>.
        /// </summary>
        public bool AsBoolean
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsBoolean;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsBoolean = value;
                ValueType = TypeCode.Boolean;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="char"/>.
        /// </summary>
        public char AsChar
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsChar;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsChar = value;
                ValueType = TypeCode.Char;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="long"/>.
        /// </summary>
        public long AsInt64
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsInt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsInt = value;
                ValueType = TypeCode.Int64;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="ulong"/>.
        /// </summary>
        public ulong AsUInt64
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsUInt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsUInt = value;
                ValueType = TypeCode.UInt64;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="float"/>.
        /// </summary>
        public float AsSingle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsSingle;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsSingle = value;
                ValueType = TypeCode.Single;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="double"/>.
        /// </summary>
        public double AsDouble
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDouble;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDouble = value;
                ValueType = TypeCode.Double;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="decimal"/>.
        /// </summary>
        public decimal AsDecimal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDecimal;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDecimal = value;
                ValueType = TypeCode.Decimal;
            }
        }

        /// <summary>
        /// Gets or sets value as <see cref="DateTime"/>.
        /// </summary>
        public DateTime AsDateTime
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return SimpleData.AsDateTime;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Data = null;
                SimpleData.AsDateTime = value;
                ValueType = TypeCode.DateTime;
            }
        }

        /// <summary>
        /// Gets variant extender implementation.
        /// </summary>
        public readonly IPlessVariantExtender Extender
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Extenders[(int)ValueType];
        }

        /// <summary>
        /// Gets type code of the value stored in this variant.
        /// </summary>
        public TypeCode ValueType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get
            {
                return dataType;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set
            {
                dataType = value;
            }
        }

        internal int AsInt32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (int)AsInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsInt64 = value;
        }

        internal short AsInt16
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (short)AsInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsInt64 = value;
        }

        internal uint AsUInt32
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (uint)AsUInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsUInt64 = value;
        }

        internal ushort AsUInt16
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (ushort)AsUInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsUInt64 = value;
        }

        internal byte AsByte
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (byte)AsUInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsUInt64 = value;
        }

        internal sbyte AsSByte
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            readonly get => (sbyte)AsInt64;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => AsInt64 = value;
        }

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="decimal"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator decimal(PlessVariant value)
            => GetExtender(value.ValueType).ToDecimal(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator DateTime(PlessVariant value)
            => GetExtender(value.ValueType).ToDateTime(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="sbyte"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator sbyte(PlessVariant value)
            => GetExtender(value.ValueType).ToSByte(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="byte"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte(PlessVariant value)
            => GetExtender(value.ValueType).ToByte(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="char"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator char(PlessVariant value)
            => GetExtender(value.ValueType).ToChar(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="double"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double(PlessVariant value)
            => GetExtender(value.ValueType).ToDouble(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="float"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator float(PlessVariant value)
            => GetExtender(value.ValueType).ToSingle(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="long"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator long(PlessVariant value)
            => GetExtender(value.ValueType).ToInt64(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="int"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(PlessVariant value)
            => GetExtender(value.ValueType).ToInt32(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="ulong"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ulong(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt64(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt32(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="ushort"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ushort(PlessVariant value)
            => GetExtender(value.ValueType).ToUInt16(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="short"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator short(PlessVariant value)
            => GetExtender(value.ValueType).ToInt16(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator bool(PlessVariant value)
            => GetExtender(value.ValueType).ToBoolean(value);

        /// <summary>
        /// Implements explicit conversion operator from
        /// variant to <see cref="string"/> value.
        /// </summary>
        /// <param name="value">Variant to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator string(PlessVariant value)
        {
            return GetExtender(value.ValueType).ToString(value);
        }

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="bool"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(bool b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="char"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(char b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="float"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(float b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="double"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(double b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="decimal"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(decimal b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="DateTime"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(DateTime b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="string"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(string b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="byte"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(byte b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="short"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(short b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="sbyte"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(sbyte b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="ushort"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(ushort b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="long"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(long b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="ulong"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(ulong b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="uint"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(uint b) => new(b);

        /// <summary>
        /// Implements implicit conversion operator from the
        /// <see cref="int"/> value to the variant.
        /// </summary>
        /// <param name="b">Value to convert</param>.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PlessVariant(int b) => new(b);

        /// <summary>
        /// Determines whether one specified <see cref="PlessVariant" /> is less
        /// than another specified <see cref="PlessVariant" />.</summary>
        /// <param name="d1">The first object to compare.</param>
        /// <param name="d2">The second object to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="d1" /> is less than
        /// <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) < 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="PlessVariant" /> is greater
        /// than another specified <see cref="PlessVariant" />.</summary>
        /// <param name="d1">The first object to compare.</param>
        /// <param name="d2">The second object to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="d1" /> is greater than
        /// <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) > 0;
        }

        /// <summary>
        /// Tests whether two specified variants are equivalent.
        /// </summary>
        /// <param name="d1">The <see cref="PlessVariant"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="d2">The <see cref="PlessVariant"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="PlessVariant"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PlessVariant d1, PlessVariant d2)
        {
            return Equals(d1, d2);
        }

        /// <summary>
        /// Tests whether two specified variants are different.
        /// </summary>
        /// <param name="d1">The <see cref="PlessVariant"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="d2">The <see cref="PlessVariant"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="PlessVariant"/> structures
        /// are different; otherwise, <c>false</c>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PlessVariant d1, PlessVariant d2)
        {
            return !Equals(d1, d2);
        }

        /// <summary>
        /// Determines whether one specified <see cref="PlessVariant" /> is less or equal
        /// to another specified <see cref="PlessVariant" />.</summary>
        /// <param name="d1">The first object to compare.</param>
        /// <param name="d2">The second object to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="d1" /> is less or equal to
        /// <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) <= 0;
        }

        /// <summary>
        /// Determines whether one specified <see cref="PlessVariant" /> is greater
        /// or equal to another specified <see cref="PlessVariant" />.</summary>
        /// <param name="d1">The first object to compare.</param>
        /// <param name="d2">The second object to compare.</param>
        /// <returns>
        /// <see langword="true" /> if <paramref name="d1" /> is greater or equal to
        /// <paramref name="d2" />; otherwise, <see langword="false" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2) >= 0;
        }

        /// <summary>
        /// Gets whether to variants are equal.
        /// </summary>
        /// <param name="d1">First value to compare.</param>
        /// <param name="d2">Second value to compare.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Equals(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Equals(d1, d2);
        }

        /// <summary>
        /// Gets <see cref="IPlessVariantExtender"/> for the specified variant type.
        /// </summary>
        /// <param name="valueType">Type of the variant.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IPlessVariantExtender GetExtender(TypeCode valueType)
        {
            return Extenders[(int)valueType];
        }

        /// <summary>
        /// Compares two variant values.
        /// </summary>
        /// <param name="d1">First value to compare.</param>
        /// <param name="d2">Second value to compare.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Compare(PlessVariant d1, PlessVariant d2)
        {
            return GetExtender(d1.ValueType).Compare(d1, d2);
        }

        /// <summary>
        /// Sets <see cref="IPlessVariantExtender"/> for the specified variant type.
        /// </summary>
        /// <param name="typeCode">Type of the variant.</param>
        /// <param name="extender"><see cref="IPlessVariantExtender"/> provider to use
        /// with the specified variant type.</param>
        public static void SetExtender(TypeCode typeCode, IPlessVariantExtender extender)
        {
            Extenders[(int)typeCode] = extender;
        }

        /// <summary>
        /// Compares this variant with another variant.
        /// </summary>
        /// <param name="value">Value to compare with.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(PlessVariant value)
        {
            return Compare(this, value);
        }

        /// <summary>
        /// Compare this object with the another object.
        /// </summary>
        /// <param name="value">Object to compare with.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">if <paramref name="value"/>
        /// is not <see cref="PlessVariant"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int CompareTo(object? value)
        {
            if (value == null)
                return 1;
            if (value is PlessVariant variant)
                return CompareTo(variant);
            throw new ArgumentException();
        }

        /// <summary>
        /// Converts this variant to <see cref="bool"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ToBoolean() => Extender.ToBoolean(this);

        /// <summary>
        /// Converts this variant to <see cref="byte"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly byte ToByte() => Extender.ToByte(this);

        /// <summary>
        /// Converts this variant to <see cref="char"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly char ToChar() => Extender.ToChar(this);

        /// <summary>
        /// Converts this variant to <see cref="DateTime"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly DateTime ToDateTime() => Extender.ToDateTime(this);

        /// <summary>
        /// Converts this variant to <see cref="decimal"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly decimal ToDecimal() => Extender.ToDecimal(this);

        /// <summary>
        /// Converts this variant to <see cref="double"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double ToDouble() => Extender.ToDouble(this);

        /// <summary>
        /// Converts this variant to <see cref="short"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly short ToInt16() => Extender.ToInt16(this);

        /// <summary>
        /// Converts this variant to <see cref="int"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ToInt32() => Extender.ToInt32(this);

        /// <summary>
        /// Converts this variant to <see cref="long"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long ToInt64() => Extender.ToInt64(this);

        /// <summary>
        /// Converts this variant to <see cref="sbyte"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly sbyte ToSByte() => Extender.ToSByte(this);

        /// <summary>
        /// Converts this variant to <see cref="float"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ToSingle() => Extender.ToSingle(this);

        /// <summary>
        /// Converts this variant to <see cref="ushort"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ushort ToUInt16() => Extender.ToUInt16(this);

        /// <summary>
        /// Converts this variant to <see cref="uint"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToUInt32() => Extender.ToUInt32(this);

        /// <summary>
        /// Converts this variant to <see cref="ulong"/> if possible.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong ToUInt64() => Extender.ToUInt64(this);

        /// <summary>
        /// Returns the hash code of this variant.
        /// </summary>
        /// <returns></returns>
        public readonly override int GetHashCode()
        {
            return Extender.GetHashCode(this);
        }

        /// <summary>
        /// Checks if this object equals another object.
        /// </summary>
        /// <param name="value">Object to compare with. May be null.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified object is a <see cref="PlessVariant"/> and it equals
        /// this object; <c>false</c> otherwise.
        /// </returns>
        public override readonly bool Equals(object? value)
        {
            if (value is not PlessVariant v)
                return false;
            return Equals(this, v);
        }

        /// <summary>
        /// Checks if this object equals another object.
        /// </summary>
        /// <param name="value">Object to compare with.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to
        /// this object; <c>false</c> otherwise.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(PlessVariant value)
        {
            return Equals(this, value);
        }

        /// <inheritdoc cref="IValueToString.ToString()"/>
        public readonly override string ToString()
        {
            return Extender.ToString(this);
        }

        /// <inheritdoc cref="IValueToString.ToString(string)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string format)
        {
            return Extender.ToString(this, format);
        }

        /// <inheritdoc cref="IValueToString.ToString(IFormatProvider)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(IFormatProvider? provider)
        {
            return Extender.ToString(this, provider);
        }

        /// <inheritdoc cref="IValueToString.ToString(string, IFormatProvider)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly string ToString(string? format, IFormatProvider? provider)
        {
            return Extender.ToString(this, format, provider);
        }

        /// <summary>
        /// Gets type code of this variant.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly TypeCode GetTypeCode()
        {
            return ValueType;
        }

        /// <summary>
        /// Converts this variant to <see cref="bool"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool ToBoolean(IFormatProvider? provider)
        {
            return Extender.ToBoolean(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="byte"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly byte ToByte(IFormatProvider? provider)
        {
            return Extender.ToByte(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="char"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly char ToChar(IFormatProvider? provider)
        {
            return Extender.ToChar(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="DateTime"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly DateTime ToDateTime(IFormatProvider? provider)
        {
            return Extender.ToDateTime(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="decimal"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly decimal ToDecimal(IFormatProvider? provider)
        {
            return Extender.ToDecimal(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="double"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double ToDouble(IFormatProvider? provider)
        {
            return Extender.ToDouble(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="short"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly short ToInt16(IFormatProvider? provider)
        {
            return Extender.ToInt16(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="int"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int ToInt32(IFormatProvider? provider)
        {
            return Extender.ToInt32(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="long"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly long ToInt64(IFormatProvider? provider)
        {
            return Extender.ToInt64(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="sbyte"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly sbyte ToSByte(IFormatProvider? provider)
        {
            return Extender.ToSByte(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="float"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly float ToSingle(IFormatProvider? provider)
        {
            return Extender.ToSingle(this, provider);
        }

        readonly object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
        {
            return Extender.ToType(this, conversionType, provider)!;
        }

        /// <summary>
        /// Converts this variant to <see cref="ushort"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ushort ToUInt16(IFormatProvider? provider)
        {
            return Extender.ToUInt16(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="uint"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly uint ToUInt32(IFormatProvider? provider)
        {
            return Extender.ToUInt32(this, provider);
        }

        /// <summary>
        /// Converts this variant to <see cref="ulong"/> if possible
        /// using the specified format provider.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting
        /// information.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ulong ToUInt64(IFormatProvider? provider)
        {
            return Extender.ToUInt64(this, provider);
        }

        private static PlessVariant GetDBNull()
        {
            PlessVariant result = new();
            result.IsDBNull = true;
            return result;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct SimpleVariantData
        {
            public static readonly SimpleVariantData Empty = new();

            [FieldOffset(0)]
            public bool AsBoolean;

            [FieldOffset(0)]
            public char AsChar;

            [FieldOffset(0)]
            public long AsInt;

            [FieldOffset(0)]
            public ulong AsUInt;

            [FieldOffset(0)]
            public float AsSingle;

            [FieldOffset(0)]
            public double AsDouble;

            [FieldOffset(0)]
            public decimal AsDecimal;

            [FieldOffset(0)]
            public DateTime AsDateTime;
        }
    }
}
