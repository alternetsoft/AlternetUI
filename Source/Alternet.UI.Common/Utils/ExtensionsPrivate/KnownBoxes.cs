using System.Runtime.CompilerServices;

namespace Alternet.UI.Extensions
{
    /// <summary>
    /// Contains boxed boolean values which can be used to avoid extra memory allocation.
    /// </summary>
#pragma warning disable
    public static class BoolBoxes
#pragma warning restore
    {
        /// <summary>
        /// Gets boxed 'true' boolean value.
        /// </summary>
        public static readonly object TrueBox = true;

        /// <summary>
        /// Gets boxed 'false' boolean value.
        /// </summary>
        public static readonly object FalseBox = false;

        /// <summary>
        /// Gets boxed boolean value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Box(bool value)
        {
            if (value)
                return TrueBox;
            else
                return FalseBox;
        }
    }

    /// <summary>
    /// Contains boxed nullable boolean values which can be used to avoid extra memory allocation.
    /// </summary>
#pragma warning disable
    public static class NullBoolBoxes
#pragma warning restore
    {
        /// <summary>
        /// Gets boxed 'true' nullable boolean value.
        /// </summary>
        public static readonly object TrueBox = (bool?)true;

        /// <summary>
        /// Gets boxed 'false' nullable boolean value.
        /// </summary>
        public static readonly object FalseBox = (bool?)false;

        /// <summary>
        /// Gets boxed 'null' boolean value.
        /// </summary>
#pragma warning disable
        public static readonly object NullBox = (bool?)null;
#pragma warning restore

        /// <summary>
        /// Gets boxed nullable boolean value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object Box(bool? value)
        {
            if (value.HasValue)
            {
                if (value == true)
                    return TrueBox;
                else
                    return FalseBox;
            }
            else
                return NullBox;
        }
    }
}