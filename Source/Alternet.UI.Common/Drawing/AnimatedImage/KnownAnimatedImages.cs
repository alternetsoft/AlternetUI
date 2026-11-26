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
        /// <summary>
        /// Gets or sets the duration in milliseconds for each frame of the SVG animation.
        /// </summary>
        public static int SvgAnimnationFrameDuration = 100;

        /// <summary>
        /// Gets or sets the color used for SVG animations,
        /// allowing for theme-based customization of animated images.
        /// </summary>
        public static ThemedColor? SvgAnimationColor { get; set; }

        /// <summary>
        /// Gets or sets an array of SVG images representing the frames of an hourglass animation.
        /// </summary>
        public static SvgImage[] SvgHourGlassImages =
        [
            KnownSvgImages.ImgHourglassStart,
            KnownSvgImages.ImgHourglassHalf,
            KnownSvgImages.ImgHourglassEnd,
        ];

        private static AnimatedImage? dualRing32;
        private static AnimatedImage? hourGlass;
        private static AnimatedImage? dualRing64;
        private static AnimatedImage? loadingIndicator32;
        private static AnimatedImage? loadingIndicator64;
        private static AnimatedImage? svgHourGlass32Light;
        private static AnimatedImage? svgHourGlass32Dark;
        private static AnimatedImageSet? dualRingSet;
        private static AnimatedImageSet? loadingIndicatorSet;

        static KnownAnimatedImages()
        {
        }

        /// <summary>
        /// Creates an <see cref="AnimatedImage"/> from an array of SVG images,
        /// with the specified size, color, and frame duration.
        /// </summary>
        /// <param name="svgImages">An array of SVG images to be used
        /// as frames for the animated image.</param>
        /// <param name="size">The size of each frame in pixels.</param>
        /// <param name="isDark">A value indicating whether
        /// to use the dark color variant for the SVG images.</param>
        /// <param name="frameDuration">The duration of each frame in milliseconds.</param>
        /// <param name="color">An optional color to apply to the SVG images.</param>
        /// <returns>An instance of <see cref="AnimatedImage"/>
        /// initialized with the specified parameters.</returns>
        public static AnimatedImage CreateAnimation(
            SvgImage[] svgImages,
            int size,
            bool isDark,
            int? frameDuration = null,
            ThemedColor? color = null)
        {
            if (svgImages == null || svgImages.Length == 0)
                return new();                

            frameDuration ??= SvgAnimnationFrameDuration;
            color ??= SvgAnimationColor;

            var c = color?.GetColor(isDark);

            var result = new AnimatedImage(svgImages, size, frameDuration.Value, c, isDark);
            return result;
        }

        /// <summary>
        /// Gets or sets the animated hourglass image used to indicate a loading or processing state.
        /// This property lazily initializes the hourglass animation on first access,
        /// allowing for efficient resource usage.
        /// This animation is for the light theme. For dark theme, use <see cref="SvgHourGlass32Dark"/>.
        /// </summary>
        public static AnimatedImage SvgHourGlass32Light
        {
            get
            {
                if (svgHourGlass32Light is null)
                {
                    svgHourGlass32Light = CreateAnimation(SvgHourGlassImages, 32, isDark: false, 250);
                    svgHourGlass32Light.OtherThemeVariant = SvgHourGlass32Dark;
                }

                return svgHourGlass32Light;
            }

            set
            {
                svgHourGlass32Light = value;
            }
        }

        /// <summary>
        /// Gets or sets the animated hourglass image used to indicate a loading or processing state.
        /// This property lazily initializes the hourglass animation on first access,
        /// allowing for efficient resource usage.
        /// This animation is for the dark theme. For light theme, use <see cref="SvgHourGlass32Light"/>.
        /// </summary>
        public static AnimatedImage SvgHourGlass32Dark
        {
            get
            {
                if (svgHourGlass32Dark is null)
                {
                    svgHourGlass32Dark = CreateAnimation(SvgHourGlassImages, 32, isDark: true, 250);
                    svgHourGlass32Dark.OtherThemeVariant = SvgHourGlass32Light;
                }

                return svgHourGlass32Dark;
            }

            set
            {
                svgHourGlass32Dark = value;
            }
        }

        /// <summary>
        /// Gets or sets the loading indicator 32x32 animation used in the application.
        /// If not explicitly set, the default
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
        /// Gets or sets the loading indicator 64x64 animation displayed during loading operations.
        /// If not explicitly set, the default
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
        /// Gets or sets <see cref="AnimatedImageSet"/> instance that includes both the 32x32 and
        /// 64 pixel dual ring loading indicators.
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
        /// Gets or sets <see cref="AnimatedImageSet"/> instance that includes both loading
        /// indicator animations in different sizes.
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
