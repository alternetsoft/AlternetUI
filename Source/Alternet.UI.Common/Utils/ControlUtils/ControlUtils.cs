using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static methods and properties which are <see cref="Control"/> related.
    /// </summary>
    public static class ControlUtils
    {
        private static Control? empty;

        /// <summary>
        /// Gets an empty control for the debug purposes.
        /// </summary>
        public static Control Empty
        {
            get
            {
                return empty ??= new PlatformControl();
            }
        }

        /// <summary>
        /// Increazes width or height specified in <paramref name="currentValue"/>
        /// if it is less than value specified in <paramref name="minValueAtLeast"/>.
        /// </summary>
        /// <param name="currentValue">Current width or height.</param>
        /// <param name="minValueAtLeast">Minimal value of the result will be at least.</param>
        /// <returns></returns>
        public static Coord GrowCoord(Coord? currentValue, Coord minValueAtLeast)
        {
            minValueAtLeast = Math.Max(0, minValueAtLeast);

            if (currentValue is null || currentValue <= 0)
                return minValueAtLeast;
            var result = Math.Max(currentValue.Value, minValueAtLeast);
            return result;
        }

        /// <summary>
        /// Increazes size specified in <paramref name="currentSize"/>
        /// if it is less than size specified in <paramref name="minSizeAtLeast"/>.
        /// Width and height are increased individually.
        /// </summary>
        /// <param name="currentSize">Current size value.</param>
        /// <param name="minSizeAtLeast">Minimal size of the result will be at least.</param>
        /// <returns></returns>
        public static SizeD GrowSize(SizeD? currentSize, SizeD minSizeAtLeast)
        {
            var newWidth = GrowCoord(currentSize?.Width, minSizeAtLeast.Width);
            var newHeight = GrowCoord(currentSize?.Height, minSizeAtLeast.Height);
            return (newWidth, newHeight);
        }
    }
}
