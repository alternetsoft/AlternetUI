using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI;

using SkiaSharp;

namespace Alternet.Drawing
{
    public partial class SkiaDrawing : NativeDrawing
    {
        /// <inheritdoc/>
        public override object CreateImage()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, Stream stream, ImageFormat format)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(object image)
        {
            throw new NotImplementedException();
        }
    }
}
