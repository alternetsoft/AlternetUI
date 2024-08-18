using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Factory with static methods which alow to create custom attributes.
    /// </summary>
    public static class AttributesFactory
    {
        /// <summary>
        /// Creates <see cref="ICustomAttributes"/> implementation.
        /// </summary>
        public static Func<ICustomAttributes> Create { get; set; } = CreateDefault;

        /// <summary>
        /// Creates attribbutes with integer identifiers.
        /// </summary>
        public static Func<ICustomAttributes<int, object>> CreateIntAttributes
        { get; set; } = CreateDefaultIntAttributes;

        /// <summary>
        /// Default method to create attributes with integer identifiers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICustomAttributes<int, object> CreateDefaultIntAttributes()
        {
            return new FlagsAndAttributes<int, object>();
        }

        /// <summary>
        /// Creates default <see cref="ICustomAttributes"/> implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICustomAttributes CreateDefault()
        {
            return new FlagsAndAttributes();
        }
    }
}
