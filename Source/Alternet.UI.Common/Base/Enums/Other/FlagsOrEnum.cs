using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the classification of an object as either a standard enumeration,
    /// a flags enumeration, or neither.
    /// </summary>
    /// <remarks>This enumeration is used to distinguish between objects that are standard enums,
    /// enums marked with the <see cref="System.FlagsAttribute"/>, or objects that are
    /// not enums at all.</remarks>
    public enum FlagsOrEnum
    {
        /// <summary>
        /// The object is not an enum or flags.
        /// </summary>
        None,

        /// <summary>
        /// The object is a standard enum.
        /// </summary>
        Enum,

        /// <summary>
        /// The object is an enum marked with [Flags] attribute.
        /// </summary>
        Flags,
    }
}