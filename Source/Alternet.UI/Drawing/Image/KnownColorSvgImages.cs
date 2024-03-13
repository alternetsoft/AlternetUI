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
        private static SvgImage? imgError;
        private static SvgImage? imgWarning;
        private static SvgImage? imgInformation;

        /// <summary>
        /// Gets or sets 'Error' image.
        /// </summary>
        public static SvgImage ImgError
        {
            get => imgError ??= new(KnownColorSvgUrls.Error);
            set => imgError = value;
        }

        /// <summary>
        /// Gets or sets 'Warning' image.
        /// </summary>
        public static SvgImage ImgWarning
        {
            get => imgWarning ??= new(KnownColorSvgUrls.Warning);
            set => imgWarning = value;
        }

        /// <summary>
        /// Gets or sets 'Information' image.
        /// </summary>
        public static SvgImage ImgInformation
        {
            get => imgInformation ??= new(KnownColorSvgUrls.Information);
            set => imgInformation = value;
        }
    }
}