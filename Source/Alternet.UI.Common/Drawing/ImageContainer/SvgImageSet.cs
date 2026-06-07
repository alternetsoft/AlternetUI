using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of images in different resolutions based on the same svg image.
    /// Images are generated from svg on demand and are cached in the set.
    /// </summary>
    public partial class SvgImageSet : ImageSet
    {
        private readonly SvgImage svgImage;
        private readonly SizeI defaultSize;
        private readonly bool? isDark;
        private readonly Color? color;

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image,
        /// base size and color theme.
        /// </summary>
        /// <param name="svgImage">The SVG image to use for the image set.</param>
        /// <param name="defaultSize">The default size of the images in the set.</param>
        /// <param name="isDark">Indicates whether the image set is for a dark theme.</param>
        public SvgImageSet(SvgImage svgImage, SizeI defaultSize, bool isDark)
        {
            this.isDark = isDark;
            this.svgImage = svgImage;
            this.defaultSize = defaultSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified stream with SVG data,
        /// default size and optional color.
        /// </summary>
        /// <param name="stream">The stream containing the SVG data.</param>
        /// <param name="defaultSize">The default size of the images in the set.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        /// <param name="throwException">Indicates whether to throw an exception on error. Optional.</param>
        public SvgImageSet(
            Stream stream,
            SizeI defaultSize,
            Color? color = null,
            bool throwException = true)
        {
            this.defaultSize = defaultSize;
            this.color = color;

            if (color is null)
                svgImage = new ColorSvgImage(stream, throwException);
            else
                svgImage = new MonoSvgImage(stream, throwException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image
        /// loaded from url or data string, base size, and optional color.
        /// </summary>
        /// <param name="urlOrData">The URL or data of the SVG image.</param>
        /// <param name="defaultSize">The default size of the images in the set.</param>
        /// <param name="kind">The kind of SVG image data.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        /// <param name="throwException">Indicates whether to throw an exception on error. Optional.</param>
        public SvgImageSet(
            string urlOrData,
            SizeI defaultSize,
            SvgImageDataKind kind = SvgImageDataKind.Url,
            Color? color = null,
            bool throwException = true)
        {
            this.defaultSize = defaultSize;
            this.color = color;

            if (color is null)
                svgImage = new ColorSvgImage(urlOrData, kind, throwException);
            else
                svgImage = new MonoSvgImage(urlOrData, kind, throwException);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SvgImageSet"/> class using the specified SVG image,
        /// base size and optional color.
        /// </summary>
        /// <param name="svg">The SVG image to use for the image set.</param>
        /// <param name="defaultSize">The default size of the images in the set.</param>
        /// <param name="color">The color to apply to the SVG image. Optional.</param>
        public SvgImageSet(SvgImage svg, SizeI defaultSize, Color? color = null)
        {
            this.svgImage = svg;
            this.defaultSize = defaultSize;
            this.color = color;
        }

        /// <summary>
        /// Gets the SVG image used as the source for generating images in the set.
        /// </summary>
        public SvgImage SvgImage => svgImage;

        /// <summary>
        /// Gets color theme of the image set. Returns null if the image set is not associated with any color theme.
        /// </summary>
        public bool? IsDark => isDark;

        /// <summary>
        /// Gets the color applied to the SVG image, or null if no color is applied.
        /// </summary>
        public Color? SvgColor => color;

        /// <inheritdoc/>
        public override SizeI DefaultSize
        {
            get
            {
                return defaultSize;
            }
        }

        /// <inheritdoc/>
        public override Image? GetExactImage(SizeI size)
        {
            var result = base.GetExactImage(size);

            result ??= AddSvg(svgImage, size, isDark, color);

            return result;
        }
    }
}
