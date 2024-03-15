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
        /// <inheritdoc cref="SvgImage.SvgImage(string, SvgImageDataKind)"/>
        public ColorSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Auto)
            : base(urlOrData, kind)
        {
        }

        /// <inheritdoc cref="SvgImage.SvgImage(Stream)"/>
        public ColorSvgImage(Stream stream)
            : base(stream)
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Many;
    }
}
