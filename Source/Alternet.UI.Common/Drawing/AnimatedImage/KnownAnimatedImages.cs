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
        private static AnimatedImage? hourGlass;
        private static AnimatedImage? dualRing64;
        private static AnimatedImage? loadingIndicator32;
        private static AnimatedImage? loadingIndicator64;
        private static AnimatedImageSet? dualRingSet;
        private static AnimatedImageSet? loadingIndicatorSet;

        /// <summary>
        /// Gets or sets the loading indicator 32x32 animation used in the application. If not explicitly set, the default
        /// animation is DualRing32.
        /// </summary>
        /// <remarks>Setting this property to <c>null</c> will revert the loading indicator to the default
        /// DualRing32 animation. This property allows customization of the loading indicator to suit different
        /// application themes or requirements.</remarks>
        public static AnimatedImage LoadingIndicator32
        {
            get
            {
                return loadingIndicator32 ?? DualRing32;
            }

            set
            {
                loadingIndicator32 = value;
            }
        }

        /// <summary>
        /// Gets or sets the loading indicator 64x64 animation displayed during loading operations. If not explicitly set, the default
        /// animation is DualRing64.
        /// </summary>
        /// <remarks>Setting this property to <c>null</c> will revert the loading indicator to the default
        /// DualRing64 animation. This property allows customization of the loading indicator to suit different
        /// application themes or requirements.</remarks>
        public static AnimatedImage LoadingIndicator64
        {
            get
            {
                return loadingIndicator64 ?? DualRing64;
            }

            set
            {
                loadingIndicator64 = value;
            }
        }

        /// <summary>
        /// Gets or sets the animated image that displays a dual ring loading indicator at 32x32 pixels.
        /// </summary>
        /// <remarks>This property lazily creates the animated image on first access. Use it to visually
        /// indicate a loading or processing state in user interfaces.</remarks>
        public static AnimatedImage DualRing32
        {
            get
            {
                return dualRing32 ??= new AnimatedImage(KnownAnimatedImageUrls.DualRing32);
            }

            set
            {
                dualRing32 = value;
            }
        }

        /// <summary>
        /// Gets or sets the animated hourglass image used to indicate a loading or processing state.
        /// </summary>
        /// <remarks>The hourglass image is lazily initialized the first time it is accessed. Setting this
        /// property allows for customization of the loading indicator.</remarks>
        public static AnimatedImage HourGlass
        {
            get
            {
                return hourGlass ??= new AnimatedImage(KnownAnimatedImageUrls.HourGlass);
            }

            set
            {
                hourGlass = value;
            }
        }

        /// <summary>
        /// Gets or sets the animated image that displays a dual ring loading indicator at 64 pixels in size.
        /// </summary>
        /// <remarks>This property returns a cached instance of the animated image, which is loaded from a
        /// predefined URL. The returned image can be reused across multiple UI elements to indicate loading or
        /// processing states efficiently.</remarks>
        public static AnimatedImage DualRing64
        {
            get
            {
                return dualRing64 ??= new AnimatedImage(KnownAnimatedImageUrls.DualRing64);
            }

            set
            {
                dualRing64 = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="AnimatedImageSet"/> instance that includes both the 32x32 and 64 pixel dual ring loading indicators.
        /// </summary>
        public static AnimatedImageSet DualRingSet
        {
            get
            {
                if (dualRingSet == null)
                {
                    dualRingSet = new AnimatedImageSet();

                    dualRingSet.Add(DualRing32, 1.0f, disposeImage: false);
                    dualRingSet.Add(DualRing64, 2.0f, disposeImage: false);
                }

                return dualRingSet;
            }

            set
            {
                dualRingSet = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="AnimatedImageSet"/> instance that includes both loading indicator animations in different sizes.
        /// </summary>
        public static AnimatedImageSet LoadingIndicatorSet
        {
            get
            {
                if (loadingIndicatorSet == null)
                {
                    loadingIndicatorSet = new AnimatedImageSet();

                    loadingIndicatorSet.Add(LoadingIndicator32, 1.0f, disposeImage: false);
                    loadingIndicatorSet.Add(LoadingIndicator64, 2.0f, disposeImage: false);
                }

                return loadingIndicatorSet;
            }

            set
            {
                loadingIndicatorSet = value;
            }
        }
    }
}
