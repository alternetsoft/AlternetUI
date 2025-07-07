using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents strongly-typed identifiers for commonly used numeric primitive types.
    /// Provides a semantic layer over <see cref="System.TypeCode"/>
    /// for numeric-only classifications.
    /// </summary>
    public enum NumericTypeCode
    {
        /// <summary>
        /// An unsigned 8-bit integer. Maps to <see cref="TypeCode.Byte"/>.
        /// </summary>
        Byte = TypeCode.Byte,

        /// <summary>
        /// A signed 8-bit integer. Maps to <see cref="TypeCode.SByte"/>.
        /// </summary>
        SByte = TypeCode.SByte,

        /// <summary>
        /// A signed 16-bit integer. Maps to <see cref="TypeCode.Int16"/>.
        /// </summary>
        Int16 = TypeCode.Int16,

        /// <summary>
        /// An unsigned 16-bit integer. Maps to <see cref="TypeCode.UInt16"/>.
        /// </summary>
        UInt16 = TypeCode.UInt16,

        /// <summary>
        /// A signed 32-bit integer. Maps to <see cref="TypeCode.Int32"/>.
        /// </summary>
        Int32 = TypeCode.Int32,

        /// <summary>
        /// An unsigned 32-bit integer. Maps to <see cref="TypeCode.UInt32"/>.
        /// </summary>
        UInt32 = TypeCode.UInt32,

        /// <summary>
        /// A signed 64-bit integer. Maps to <see cref="TypeCode.Int64"/>.
        /// </summary>
        Int64 = TypeCode.Int64,

        /// <summary>
        /// An unsigned 64-bit integer. Maps to <see cref="TypeCode.UInt64"/>.
        /// </summary>
        UInt64 = TypeCode.UInt64,

        /// <summary>
        /// A single-precision floating-point number. Maps to <see cref="TypeCode.Single"/>.
        /// </summary>
        Single = TypeCode.Single,

        /// <summary>
        /// A double-precision floating-point number. Maps to <see cref="TypeCode.Double"/>.
        /// </summary>
        Double = TypeCode.Double,

        /// <summary>
        /// A high-precision decimal number. Maps to <see cref="TypeCode.Decimal"/>.
        /// </summary>
        Decimal = TypeCode.Decimal,
    }
}












