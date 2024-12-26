using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates known text value types.
    /// </summary>
    public enum KnownInputType
    {
        /// <summary>
        /// Value type is not specified.
        /// </summary>
        None,

/*
        /// <summary>
        /// Value is <see cref="string"/>.
        /// </summary>
        String,

        /// <summary>
        /// Value is <see cref="bool"/>.
        /// </summary>
        Boolean,

        /// <summary>
        /// Value is <see cref="char"/>.
        /// </summary>
        Char,
*/

        /// <summary>
        /// Value is <see cref="sbyte"/>.
        /// </summary>
        SByte,

        /// <summary>
        /// Value is <see cref="byte"/>.
        /// </summary>
        Byte,

        /// <summary>
        /// Value is <see cref="short"/>.
        /// </summary>
        Int16,

        /// <summary>
        /// Value is <see cref="ushort"/>.
        /// </summary>
        UInt16,

        /// <summary>
        /// Value is <see cref="int"/>.
        /// </summary>
        Int32,

        /// <summary>
        /// Value is <see cref="uint"/>.
        /// </summary>
        UInt32,

        /// <summary>
        /// Value is <see cref="long"/>.
        /// </summary>
        Int64,

        /// <summary>
        /// Value is <see cref="ulong"/>.
        /// </summary>
        UInt64,

        /// <summary>
        /// Value is <see cref="float"/>.
        /// </summary>
        Single,

        /// <summary>
        /// Value is unsigned <see cref="float"/>.
        /// </summary>
        USingle,

        /// <summary>
        /// Value is <see cref="double"/>.
        /// </summary>
        Double,

        /// <summary>
        /// Value is unsigned <see cref="double"/>.
        /// </summary>
        UDouble,

        /// <summary>
        /// Value is <see cref="decimal"/>.
        /// </summary>
        Decimal,

        /// <summary>
        /// Value is unsigned <see cref="decimal"/>.
        /// </summary>
        UDecimal,

/*
        /// <summary>
        /// Value is <see cref="DateTime"/>.
        /// </summary>
        DateTime,

        /// <summary>
        /// Value is date part of <see cref="DateTime"/>.
        /// </summary>
        Date,

        /// <summary>
        /// Value is time part of <see cref="DateTime"/>.
        /// </summary>
        Time,

        /// <summary>
        /// Value is e-mail.
        /// </summary>
        EMail,

        /// <summary>
        /// Value is url.
        /// </summary>
        Url,
*/
    }
}