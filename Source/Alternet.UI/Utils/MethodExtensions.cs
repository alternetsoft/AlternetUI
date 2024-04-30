using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Contains extension methods for standard classes.
    /// </summary>
    public static class MethodExtensions
    {
        /// <summary>
        /// Get bitmap of the size appropriate for the DPI scaling used by the given control.
        /// </summary>
        /// <remarks>
        /// This helper function simply combines
        /// <see cref="GetPreferredBitmapSizeFor(ImageSet, Control)"/> and
        /// <see cref="ImageSet.AsImage(SizeI)"/>, i.e.it returns a (normally unscaled) bitmap
        /// from the <see cref="ImageSet"/> of the closest size to the size that should
        /// be used at the DPI scaling of the provided control.
        /// </remarks>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <param name="imageSet"><see cref="ImageSet"/> instance.</param>
        public static Image AsImageFor(this ImageSet imageSet, Control control)
            => new Bitmap(imageSet, control);

        /// <summary>
        /// Get the size that would be best to use for this <see cref="ImageSet"/> at the DPI
        /// scaling factor used by the given control.
        /// </summary>
        /// <param name="control">Control to get DPI scaling factor from.</param>
        /// <returns></returns>
        /// <remarks>
        /// This is just a convenient wrapper for
        /// <see cref="ImageSet.GetPreferredBitmapSizeAtScale"/> calling
        /// that function with the result of <see cref="Control.GetPixelScaleFactor"/>.
        /// </remarks>
        /// <param name="imageSet"><see cref="ImageSet"/> instance.</param>
        public static SizeI GetPreferredBitmapSizeFor(this ImageSet imageSet, Control control)
        {
            return ((UI.Native.ImageSet)imageSet.NativeObject).GetPreferredBitmapSizeFor(control.WxWidget);
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="graphics">Drawing context.</param>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <param name="control">The control used to get scaling factor. Optional.</param>
        /// <param name="descent">Dimension from the baseline of the font to
        /// the bottom of the descender (the size of the tail below the baseline).</param>
        /// <param name="externalLeading">Any extra vertical space added to the
        /// font by the font designer (inter-line interval).</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        public static SizeD GetTextExtent(
            this Graphics graphics,
            string text,
            Font font,
            out double descent,
            out double externalLeading,
            Control? control = null)
        {
            var dc = (UI.Native.DrawingContext)graphics.NativeObject;

            var result = dc.GetTextExtent(
                text,
                (UI.Native.Font)font.NativeObject,
                control is null ? default : control.WxWidget);
            descent = result.X;
            externalLeading = result.Y;
            return result.Size;
        }

        /// <summary>
        /// Gets the dimensions of the string using the specified font.
        /// </summary>
        /// <param name="graphics">Drawing context.</param>
        /// <param name="text">The text string to measure.</param>
        /// <param name="font">The Font used to get text dimensions.</param>
        /// <param name="control">The control used to get scaling factor. Can be null.</param>
        /// <returns><see cref="SizeD"/> with the total calculated width and height
        /// of the text.</returns>
        /// <remarks>
        /// This function only works with single-line strings.
        /// It works faster than MeasureText methods.
        /// </remarks>
        public static SizeD GetTextExtent(
            this Graphics graphics,
            string text,
            Font font,
            Control? control)
        {
            var dc = (UI.Native.DrawingContext)graphics.NativeObject;
            var result = dc.GetTextExtentSimple(
                text,
                (UI.Native.Font)font.NativeObject,
                control is null ? default : control.WxWidget);
            return result;
        }

        /// <summary>
        /// Gets the size of the image in device-independent units (1/96th inch
        /// per unit).
        /// </summary>
        public static SizeD SizeDip(this Image image, Control control)
            => control.PixelToDip(image.PixelSize);

        /// <summary>
        /// Gets image rect as (0, 0, SizeDip().Width, SizeDip().Height).
        /// </summary>
        public static RectD BoundsDip(this Image image, Control control)
        {
            var size = SizeDip(image, control);
            return (0, 0, size.Width, size.Height);
        }
    }
}
