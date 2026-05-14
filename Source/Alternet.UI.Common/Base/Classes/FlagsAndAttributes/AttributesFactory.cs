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
        private static long uniqueAttributeNameCounter;

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

        /// <summary>
        /// Generates a unique attribute name with an optional prefix.
        /// </summary>
        /// <param name="prefix">The optional prefix to prepend to the unique identifier.</param>
        /// <returns>A unique attribute name string.</returns>
        public static string GenUniqueAttributeName(string? prefix = null)
        {
            var result = prefix + new ObjectUniqueId(ref uniqueAttributeNameCounter).ToString();
            return result;
        }
    }
}
