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
        /// Gets <see cref="ImgError"/> image for the specified bitmap size.
        /// If size is not specified, gets image for the default toolbar image size.
        /// </summary>
        /// <param name="size">Image size.</param>
        public static Image? GetErrorImage(int? size = null)
        {
            size ??= ToolBarUtils.GetDefaultImageSize().Width;
            var imageSet = KnownColorSvgImages.ImgError.AsImageSet(size.Value);
            var image = imageSet?.AsImage(size.Value);
            return image;
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