using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    internal partial class WxDrawing
    {
        /// <inheritdoc/>
        public override object CreateImage()
        {
            return new UI.Native.Image();
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, string fileName)
        {
            return ((UI.Native.Image)image).SaveToFile(fileName);
        }

        /// <inheritdoc/>
        public override bool ImageSave(object image, Stream stream, ImageFormat format)
        {
            var outputStream = new UI.Native.OutputStream(stream);
            return ((UI.Native.Image)image).SaveToStream(outputStream, format.ToString());
        }

        /// <inheritdoc/>
        public override SizeI GetImagePixelSize(object image)
        {
            return ((UI.Native.Image)image).PixelSize;
        }

        /// <inheritdoc/>
        public override bool ImageLoadFromStream(object image, Stream stream)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            if (inputStream is null)
                return false;

            return ((UI.Native.Image)image).LoadFromStream(inputStream);
        }
    }
}
