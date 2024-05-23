using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Generic factory with static methods which create classes and interface implementations.
    /// </summary>
    public static class Factory
    {
        /// <summary>
        /// Creates <see cref="IFlagsAndAttributes"/> implementation.
        /// </summary>
        public static Func<IFlagsAndAttributes> CreateFlagsAndAttributes
            { get; set; } = CreateDefaultFlagsAndAttributes;

        /// <summary>
        /// Creates <see cref="ICustomFlags"/> implementation.
        /// </summary>
        public static Func<ICustomFlags> CreateCustomFlags { get; set; } = CreateDefaultCustomFlags;

        /// <summary>
        /// Creates <see cref="ICustomAttributes"/> implementation.
        /// </summary>
        public static Func<ICustomAttributes> CreateCustomAttributes
            { get; set; } = CreateDefaultCustomAttributes;

        /// <summary>
        /// Creates default <see cref="IFlagsAndAttributes"/> implementation.
        /// </summary>
        public static IFlagsAndAttributes CreateDefaultFlagsAndAttributes()
        {
            return new FlagsAndAttributes();
        }

        /// <summary>
        /// Creates default <see cref="ICustomFlags"/> implementation.
        /// </summary>
        public static ICustomFlags CreateDefaultCustomFlags()
        {
            return new FlagsAndAttributes();
        }

        /// <summary>
        /// Creates default <see cref="ICustomAttributes"/> implementation.
        /// </summary>
        public static ICustomAttributes CreateDefaultCustomAttributes()
        {
            return new FlagsAndAttributes();
        }
    }
}
