using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to commonly used animated images for UI elements.
    /// </summary>
    /// <remarks>This class contains static properties that return instances of animated images, which are
    /// initialized on first access. The images are designed for use in loading indicators and similar UI
    /// components.</remarks>
    public static class KnownAnimatedImages
    {
        private static AnimatedImage? dualRing32;
        private static AnimatedImage? dualRing64;

        /// <summary>
        /// Gets the animated image that displays a dual ring loading indicator at 32x32 pixels.
        /// </summary>
        /// <remarks>This property lazily creates the animated image on first access. Use it to visually
        /// indicate a loading or processing state in user interfaces.</remarks>
        public static AnimatedImage DualRing32 => dualRing32 ??= new AnimatedImage(KnownAnimatedImageUrls.DualRing32);

        /// <summary>
        /// Gets the animated image that displays a dual ring loading indicator at 64 pixels in size.
        /// </summary>
        /// <remarks>This property returns a cached instance of the animated image, which is loaded from a
        /// predefined URL. The returned image can be reused across multiple UI elements to indicate loading or
        /// processing states efficiently.</remarks>
        public static AnimatedImage DualRing64 => dualRing64 ??= new AnimatedImage(KnownAnimatedImageUrls.DualRing64);
    }
}
