using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.Drawing
{
    /// <summary>
    /// <see cref="SvgImage"/> descendant which works with two color svg images.
    /// </summary>
    public class TwoColorSvgImage : SvgImage
    {
        /// <inheritdoc cref="SvgImage.SvgImage(string, SvgImageDataKind)"/>
        public TwoColorSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Auto)
            : base(urlOrData, kind)
        {
        }

        /// <inheritdoc cref="SvgImage.SvgImage(Stream)"/>
        public TwoColorSvgImage(Stream stream)
            : base(stream)
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Two;
    }
}
