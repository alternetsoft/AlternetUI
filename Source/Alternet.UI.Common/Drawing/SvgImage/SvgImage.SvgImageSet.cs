using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class SvgImage
    {
        /// <summary>
        /// Gets <see cref="SvgImageSet"/> with the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// This function caches results and returns the same image set instance on subsequent calls with the same size
        /// and color theme.
        /// </summary>
        /// <param name="size">Base image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns>Image set containing the loaded SVG image.</returns>
        public virtual SvgImageSet ToNormalSvgImageSet(int size, bool isDark)
        {
            return ToSvgImageSet(size, KnownSvgColor.Normal, isDark);
        }

        /// <summary>
        /// Gets <see cref="SvgImageSet"/> with the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// This function caches results and returns the same image set instance on subsequent calls with the same size
        /// and color theme.
        /// </summary>
        /// <param name="size">Base image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns>Image set containing the loaded SVG image.</returns>
        public virtual SvgImageSet ToDisabledSvgImageSet(int size, bool isDark)
        {
            return ToSvgImageSet(size, KnownSvgColor.Disabled, isDark);
        }

        /// <summary>
        /// Gets svg image as <see cref="SvgImageSet"/> with the specified base size and known svg color.
        /// </summary>
        /// <param name="size">Base image size in pixels.</param>
        /// <param name="knownColor">Known image color.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual SvgImageSet ToSvgImageSet(int size, KnownSvgColor knownColor, bool isDark)
        {
            if (!IsMono && knownColor == KnownSvgColor.Normal)
                return ToSvgImageSet(size);

            Resize(size);

            SvgImageSet? result;

            result = data[size]?.GetKnownColorSvgImageSet(isDark, knownColor);

            if (result is not null)
                return result;

            var color = GetSvgColor(knownColor, isDark);
            result = CreateSvgImageSet(size, color);

            if (data[size] is null)
                data[size] = new();

            data[size]!.SetKnownColorSvgImageSet(isDark, knownColor, result);

            return result;
        }

        /// <summary>
        /// Gets mono svg image as <see cref="SvgImageSet"/> filled using the specified color.
        /// Svg images with two or more colors are returned as is.
        /// </summary>
        /// <param name="size">Base image size in pixels.</param>
        /// <param name="color">Svg image color.</param>
        /// <returns></returns>
        public virtual SvgImageSet ToSvgImageSetWithColor(int size, Color color)
        {
            if (!IsMono)
                return ToSvgImageSet(size);

            Resize(size);

            data[size] ??= new();
            data[size]!.ColoredSvgImageSet ??= new();

            var images = data[size]!.ColoredSvgImageSet!;

            if (images.TryGetValue(color, out var result))
                return result;

            result = CreateSvgImageSet(size, color);
            images[color] = result;
            return result;
        }

        /// <summary>
        /// Gets svg image as <see cref="SvgImageSet"/> with default toolbar image size.
        /// </summary>
        public virtual SvgImageSet ToSvgImageSet()
        {
            var result = ToSvgImageSet(ToolBarUtils.GetDefaultImageSize().Width);
            return result;
        }

        /// <summary>
        /// Gets SVG image set with a single image of the specified size created using svg data.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns>SVG image set containing the loaded SVG image.</returns>
        public virtual SvgImageSet ToSvgImageSet(int size)
        {
            Resize(size);
            var result = data[size]?.OriginalSvgImageSet;
            if (result is not null)
                return result;
            result = CreateSvgImageSet(size);
            data[size] ??= new();
            data[size]!.OriginalSvgImageSet = result;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="SvgImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns><see cref="SvgImageSet"/> containing the loaded SVG image.</returns>
        public SvgImageSet CreateSvgImageSet(int size, Color? color = null)
        {
            return CreateSvgImageSet((size, size), color);
        }

        /// <summary>
        /// Creates new <see cref="SvgImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns><see cref="SvgImageSet"/> containing the loaded SVG image.</returns>
        public virtual SvgImageSet CreateSvgImageSet(SizeI size, Color? color = null)
        {
            SvgImageSet result = new (this, size, color);
            return result;
        }

    }
}
