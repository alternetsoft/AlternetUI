using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Storage for <see cref="SvgImage"/> conversions to <see cref="Image"/>
    /// for the different visual states and light/dark flags.
    /// </summary>
    public struct CachedSvgImage
    {
        private EnumArray<VisualControlState, LightDarkImage?> imageData = new();
        private SvgImage? svgImage;
        private SizeI? svgSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedSvgImage"/> struct.
        /// </summary>
        public CachedSvgImage()
        {
        }

        /// <summary>
        /// Gets or sets svg size.
        /// </summary>
        public SizeI? SvgSize
        {
            readonly get => svgSize;

            set
            {
                if (svgSize == value)
                    return;
                svgSize = value;
                imageData.Reset();
            }
        }

        /// <summary>
        /// Gets or sets svg.
        /// </summary>
        public SvgImage? SvgImage
        {
            readonly get
            {
                return svgImage;
            }

            set
            {
                if (svgImage == value)
                    return;
                svgImage = value;
                imageData.Reset();
            }
        }

        /// <summary>
        /// Gets image for the specified item state and light/dark theme flag.
        /// </summary>
        /// <param name="state">Item state.</param>
        /// <param name="isDark">Light/dark theme flag</param>
        /// <returns></returns>
        public readonly Image? GetImage(VisualControlState state, bool isDark)
        {
            var result = imageData[state]?.GetImage(isDark);
            return result;
        }

        /// <summary>
        /// Sets image for the specified color theme light/dark theme flag.
        /// </summary>
        /// <param name="state">Visual state (normal, disabled, selected)
        /// for which image is set.</param>
        /// <param name="image">New image value.</param>
        /// <param name="isDark">Whether theme is dark.</param>
        public void SetImage(VisualControlState state, Image? image, bool isDark)
        {
            imageData[state] ??= new();
            imageData[state]!.SetImage(isDark, image);
        }
    }
}
