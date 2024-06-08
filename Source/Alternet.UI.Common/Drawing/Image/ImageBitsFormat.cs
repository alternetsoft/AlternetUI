using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public struct ImageBitsFormat
    {
        public int BitsPerPixel;

        public bool HasAlpha;

        public int SizePixel;

        public int Red;

        public int Green;

        public int Blue;

        public int Alpha;

        /// <summary>
        /// Tests whether two specified <see cref="ImageBitsFormat"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="ImageBitsFormat"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="ImageBitsFormat"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ImageBitsFormat"/> structures
        /// are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(ImageBitsFormat left, ImageBitsFormat right)
        {
            return
                left.BitsPerPixel == right.BitsPerPixel &&
                left.HasAlpha == right.HasAlpha &&
                left.SizePixel == right.SizePixel &&
                left.Red == right.Red &&
                left.Green == right.Green &&
                left.Blue == right.Blue &&
                left.Alpha == right.Alpha;
        }

        /// <summary>
        /// Tests whether two specified <see cref="ImageBitsFormat"/> structures are not equivalent.
        /// </summary>
        /// <param name="left">The <see cref="ImageBitsFormat"/> that is to the left
        /// of the equality operator.</param>
        /// <param name="right">The <see cref="ImageBitsFormat"/> that is to the right
        /// of the equality operator.</param>
        /// <returns><c>true</c> if the two <see cref="ImageBitsFormat"/> structures
        /// are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(ImageBitsFormat left, ImageBitsFormat right)
        {
            return !(left == right);
        }

        public readonly void Log(string? sectionName = null)
        {
            var self = this;

            App.LogSection(() =>
            {
                App.LogNameValue("BitsPerPixel", self.BitsPerPixel);
                App.LogNameValue("HasAlpha", self.HasAlpha);
                App.LogNameValue("SizePixel", self.SizePixel);
                App.LogNameValue("Red", self.Red);
                App.LogNameValue("Green", self.Green);
                App.LogNameValue("Blue", self.Blue);
                App.LogNameValue("Alpha", self.Alpha);
            }, sectionName);
        }

        public readonly override bool Equals(object? obj)
        {
            if (obj is not ImageBitsFormat value)
                return false;
            return this == value;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(
                BitsPerPixel.GetHashCode(),
                HasAlpha.GetHashCode(),
                SizePixel.GetHashCode(),
                Red.GetHashCode(),
                Green.GetHashCode(),
                Blue.GetHashCode(),
                Alpha.GetHashCode());
        }
    }
}