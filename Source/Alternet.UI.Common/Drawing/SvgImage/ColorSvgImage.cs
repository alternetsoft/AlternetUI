using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// <see cref="SvgImage"/> descendant which works with color svg images.
    /// </summary>
    public class ColorSvgImage : SvgImage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(string, SvgImageDataKind)"/> for the details.
        /// </remarks>
        public ColorSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Url)
            : base(urlOrData, kind)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        /// <remarks>
        /// See <see cref="SvgImage.SvgImage(Stream)"/> for the details.
        /// </remarks>
        public ColorSvgImage(Stream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSvgImage"/> class.
        /// </summary>
        public ColorSvgImage()
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Many;

        /// <summary>
        /// Creates <see cref="ColorSvgImage"/> from the specified svg file.
        /// </summary>
        /// <param name="path">Path to file.</param>
        /// <returns></returns>
        public static ColorSvgImage FromFile(string path)
        {
            using var stream = FileSystem.Default.OpenRead(path);
            var result = new ColorSvgImage(stream);
            return result;
        }
    }
}
