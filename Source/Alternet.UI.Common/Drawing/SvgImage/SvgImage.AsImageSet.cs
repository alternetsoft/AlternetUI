using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class SvgImage
    {
        /// <summary>
        /// Creates new <see cref="ImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns></returns>
        public ImageSet? CreateImageSet(int size, Color? color = null)
        {
            return CreateImageSet((size, size), color);
        }

        /// <summary>
        /// Creates new <see cref="ImageSet"/> and loads there this svg image
        /// with the specified size and color.
        /// </summary>
        /// <param name="size">Svg image size in pixels.</param>
        /// <param name="color">Color of the mono svg image. Optional.</param>
        /// <returns>Image set containing the loaded SVG image. Returned image set is immutable.</returns>
        public virtual ImageSet? CreateImageSet(SizeI size, Color? color = null)
        {
            var skiaBitmap = SkiaUtils.BitmapFromPicture(AsPicture, size.Width, size.Height, color);
            var bitmap = (Image)skiaBitmap;
            ImageSet result = new(bitmap);
            result.SetImmutable();
            return result;
        }

        /// <summary>
        /// Gets image with the specified size and known svg color.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="knownColor">Known image color.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns></returns>
        public virtual ImageSet? AsImageSet(int size, KnownSvgColor knownColor, bool isDark)
        {
            if (!IsMono && knownColor == KnownSvgColor.Normal)
                return AsImageSet(size);

            Resize(size);

            ImageSet? result;

            if (isDark)
                result = data[size]?.KnownColorImagesDark[(int)knownColor];
            else
                result = data[size]?.KnownColorImagesLight[(int)knownColor];

            if (result is not null)
                return result;

            var color = GetSvgColor(knownColor, isDark);
            result = CreateImageSet(size, color);

            if (data[size] is null)
                data[size] = new();

            if (isDark)
                data[size]!.KnownColorImagesDark[(int)knownColor] = result;
            else
                data[size]!.KnownColorImagesLight[(int)knownColor] = result;
            return result;
        }

        /// <summary>
        /// Gets mono svg image as <see cref="ImageSet"/> filled using the specified color.
        /// Svg images with two or more colors are returned as is.
        /// </summary>
        /// <param name="size">Svg image size.</param>
        /// <param name="color">Svg image color.</param>
        /// <returns></returns>
        public virtual ImageSet? ImageSetWithColor(int size, Color color)
        {
            if (!IsMono)
                return AsImageSet(size);

            Resize(size);

            data[size] ??= new();
            data[size]!.ColoredImages ??= new();

            var images = data[size]!.ColoredImages!;

            if (images.TryGetValue(color, out var result))
                return result;

            result = CreateImageSet(size, color);
            if (result is not null)
                images.Add(color, result);
            return result;
        }


        /// <summary>
        /// Gets svg image as <see cref="ImageSet"/> with default toolbar image size.
        /// </summary>
        public virtual ImageSet? AsImageSet()
        {
            var result = AsImageSet(ToolBarUtils.GetDefaultImageSize().Width);
            return result;
        }

        /// <summary>
        /// Gets image set with a single image of the specified size created using svg data.
        /// </summary>
        /// <param name="size">Image size</param>
        /// <returns>Image set containing the loaded SVG image. Returned image set
        /// is immutable and contains a single image with the specified size.</returns>
        public virtual ImageSet? AsImageSet(int size)
        {
            Resize(size);
            var result = data[size]?.OriginalImage;
            if (result is not null)
                return result;
            result = CreateImageSet(size);
            data[size] ??= new();
            data[size]!.OriginalImage = result;
            return result;
        }

        /// <summary>
        /// Gets image set with single image of the specified size and <see cref="KnownSvgColor.Normal"/> color.
        /// This function caches results and returns the same image set instance on subsequent calls with the same size
        /// and color theme.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns>Image set containing the loaded SVG image.
        /// Result is immutable and contains a single image with the specified color and size.</returns>
        public virtual ImageSet? AsNormal(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Normal, isDark);
        }

        /// <summary>
        /// Gets image set with single image of the specified size and <see cref="KnownSvgColor.Disabled"/> color.
        /// This function caches results and returns the same image set instance on subsequent calls with the same size
        /// and color theme.
        /// </summary>
        /// <param name="size">Image size in pixels.</param>
        /// <param name="isDark">Whether color theme is dark.</param>
        /// <returns>Image set containing the loaded SVG image.
        /// Result is immutable and contains a single image with the specified color and size.</returns>
        public virtual ImageSet? AsDisabled(int size, bool isDark)
        {
            return AsImageSet(size, KnownSvgColor.Disabled, isDark);
        }
    }
}
