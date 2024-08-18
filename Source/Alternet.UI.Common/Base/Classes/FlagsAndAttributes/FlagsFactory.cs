using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Factory with static methods which alow to create custom flags.
    /// </summary>
    public static class FlagsFactory
    {
        private static readonly AdvDictionary<string, int> IntIdentifiers = new();

        private static int intIdentifierCounter = 10000;

        /// <summary>
        /// Creates <see cref="ICustomFlags"/> implementation.
        /// </summary>
        public static Func<ICustomFlags> Create { get; set; } = CreateDefault;

        /// <summary>
        /// Creates flags with integer identifiers.
        /// </summary>
        public static Func<ICustomFlags<int>> CreateIntFlags { get; set; } = CreateDefaultIntFlags;

        /// <summary>
        /// Default method to create flags with integer identifiers.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICustomFlags<int> CreateDefaultIntFlags()
        {
            return new FlagsAndAttributes<int, object>();
        }

        /// <summary>
        /// Creates default <see cref="ICustomFlags"/> implementation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ICustomFlags CreateDefault()
        {
            return new FlagsAndAttributes();
        }

        /// <summary>
        /// Allocates integer identifier with the specified name.
        /// </summary>
        /// <param name="name">Identifier name.</param>
        /// <returns></returns>
        public static int AllocIntIdentifier(string name)
        {
            var result = IntIdentifiers.GetOrCreate(name, () => ++intIdentifierCounter);
            return result;
        }
    }
}
