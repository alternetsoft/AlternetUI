using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates all possible edit kinds in <see cref="PropertyGrid"/>.
    /// </summary>
    public enum PropertyGridEditKindAll
    {
        // ==== Strings

        /// <inheritdoc cref="PropertyGridEditKindString.Simple"/>
        String,

        /// <inheritdoc cref="PropertyGridEditKindString.Long"/>
        StringLong,

        /// <inheritdoc cref="PropertyGridEditKindString.FileName"/>
        StringFilename,

        /// <inheritdoc cref="PropertyGridEditKindString.Directory"/>
        StringDirectory,

        /// <inheritdoc cref="PropertyGridEditKindString.ImageFileName"/>
        StringImageFilename,

        // ==== Enums

        /// <summary>
        /// Uses <see cref="System.Enum"/> editor for editing enumerations with
        /// <see cref="FlagsAttribute"/>.
        /// </summary>
        EnumFlags,

        /// <summary>
        /// Uses <see cref="System.Enum"/> editor with list of values.
        /// </summary>
        Enum,

        /// <summary>
        /// Uses <see cref="System.Enum"/> editor with list of values and editable value.
        /// </summary>
        EnumEditable,

        // ==== Signed integers.

        /// <summary>
        /// Uses <see cref="long"/> editor.
        /// </summary>
        Int64,

        /// <summary>
        /// Uses <see cref="int"/> editor.
        /// </summary>
        Int32,

        /// <summary>
        /// Uses <see cref="short"/> editor.
        /// </summary>
        Int16,

        /// <summary>
        /// Uses <see cref="sbyte"/> editor.
        /// </summary>
        SByte,

        // ==== Unsigned integers.

        /// <summary>
        /// Uses <see cref="ulong"/> editor.
        /// </summary>
        UInt64,

        /// <summary>
        /// Uses <see cref="uint"/> editor.
        /// </summary>
        UInt32,

        /// <summary>
        /// Uses <see cref="ushort"/> editor.
        /// </summary>
        UInt16,

        /// <summary>
        /// Uses <see cref="byte"/> editor.
        /// </summary>
        Byte,

        // ==== Float numbers.

        /// <summary>
        /// Uses <see cref="double"/> editor.
        /// </summary>
        Double,

        /// <summary>
        /// Uses <see cref="float"/> editor.
        /// </summary>
        Single,

        /// <summary>
        /// Uses <see cref="decimal"/> editor.
        /// </summary>
        Decimal,

        // ==== Colors.

        /// <summary>
        /// Uses <see cref="Alternet.Drawing.Color"/> editor.
        /// </summary>
        Color,

        /// <summary>
        /// Uses <see cref="Alternet.Drawing.Color"/> editor with system colors list.
        /// </summary>
        ColorSystem,

        // ==== DateTime

        /// <summary>
        /// Uses <see cref="DateTime"/> editor for date editing.
        /// </summary>
        Date,

        /// <summary>
        /// Uses <see cref="DateTime"/> editor for time editing.
        /// </summary>
        Time,

        /// <summary>
        /// Uses <see cref="DateTime"/> editor for date and time editing.
        /// </summary>
        DateTime,

        // ==== Other

        /// <summary>
        /// Uses <see cref="bool"/> editor.
        /// </summary>
        Bool,

        /// <summary>
        /// Uses other editor.
        /// </summary>
        Other,
    }
}