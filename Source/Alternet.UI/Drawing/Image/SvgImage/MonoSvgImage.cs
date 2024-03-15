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
    /// <see cref="SvgImage"/> descendant which works with mono (single color) svg images.
    /// </summary>
    public class MonoSvgImage : SvgImage
    {
        /// <inheritdoc cref="SvgImage.SvgImage(string, SvgImageDataKind)"/>
        public MonoSvgImage(string urlOrData, SvgImageDataKind kind = SvgImageDataKind.Auto)
            : base(urlOrData, kind)
        {
        }

        /// <inheritdoc cref="SvgImage.SvgImage(Stream)"/>
        public MonoSvgImage(Stream stream)
            : base(stream)
        {
        }

        /// <inheritdoc/>
        public override SvgImageNumOfColors NumOfColors => SvgImageNumOfColors.Many;
    }
}
