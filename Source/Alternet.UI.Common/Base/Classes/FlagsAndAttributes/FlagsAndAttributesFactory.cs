using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Factory with static methods which alow to create objects
    /// which implement <see cref="IFlagsAndAttributes"/> interface.
    /// </summary>
    public static class FlagsAndAttributesFactory
    {
        /// <summary>
        /// Creates <see cref="IFlagsAndAttributes"/> implementation.
        /// </summary>
        public static Func<IFlagsAndAttributes> Create { get; set; } = CreateDefault;

        /// <summary>
        /// Create flags and attributes with integer identifiers.
        /// </summary>
        public static Func<IIntFlagsAndAttributes> CreateIntFlagsAndAttributes
        { get; set; } = CreateDefaultIntFlagsAndAttributes;

        /// <summary>
        /// Creates default <see cref="IFlagsAndAttributes"/> implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IFlagsAndAttributes CreateDefault()
        {
            return new FlagsAndAttributes();
        }

        /// <summary>
        /// Default method to create flags and attributes with integer identifiers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IIntFlagsAndAttributes CreateDefaultIntFlagsAndAttributes()
        {
            return new IntFlagsAndAttributes();
        }
    }
}
