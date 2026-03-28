using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to get known color svg images as <see cref="ImageSet"/> instances.
    /// </summary>
    public static class KnownColorSvgImages
    {
        private static ColorSvgImage? imgError;
        private static ColorSvgImage? imgWarning;
        private static ColorSvgImage? imgInformation;
        private static ColorSvgImage? imgLogo;

        /// <summary>
        /// Gets or sets logo svg image without text.
        /// </summary>
        public static ColorSvgImage ImgLogo
        {
            get => imgLogo ??= new(KnownColorSvgUrls.Logo);
            set => imgLogo = value;
        }

        /// <summary>
        /// Gets or sets 'Error' image.
        /// </summary>
        public static ColorSvgImage ImgError
        {
            get => imgError ??= new(KnownColorSvgUrls.Error);
            set => imgError = value;
        }

        /// <summary>
        /// Gets or sets 'Warning' image.
        /// </summary>
        public static ColorSvgImage ImgWarning
        {
            get => imgWarning ??= new(KnownColorSvgUrls.Warning);
            set => imgWarning = value;
        }

        /// <summary>
        /// Gets or sets 'Information' image.
        /// </summary>
        public static ColorSvgImage ImgInformation
        {
            get
            {
                return imgInformation ??= new(KnownColorSvgUrls.Information);
            }

            set => imgInformation = value;
        }

        /// <summary>
        /// Returns an error image as an Image object, optionally specifying the size.
        /// </summary>
        /// <param name="size">The desired width and height, in pixels, of the error image. If null, a default size is used.</param>
        /// <param name="control">The control with which is used in order to determine the default image size.</param>
        /// <returns>An Image representing the error icon, or null if the image cannot be created.</returns>
        public static Image? GetErrorImage(int? size = null, AbstractControl? control = null)
        {
            return KnownColorSvgImages.ImgError.ToImageWithDefaultSize(size, control);
        }

        /// <summary>
        /// Gets all images in <see cref="KnownColorSvgImages"/>.
        /// </summary>
        public static IEnumerable<SvgImage> GetAllImages()
        {
            return AssemblyUtils.GetStaticProperties<ColorSvgImage>(typeof(KnownColorSvgImages));
        }
    }
}