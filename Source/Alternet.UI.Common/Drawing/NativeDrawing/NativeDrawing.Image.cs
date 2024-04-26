using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public partial class NativeDrawing
    {
        /// <summary>
        /// Creates native image.
        /// </summary>
        /// <returns></returns>
        public abstract object CreateImage();

        /// <summary>
        /// Saves native image to file.
        /// </summary>
        /// <param name="fileName">A string that contains the name of the file
        /// to which to save the image.</param>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract bool ImageSave(object image, string fileName);

        /// <summary>
        /// Saves native image to stream.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> where the image will be
        /// saved.</param>
        /// <param name="format">Image format.</param>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract bool ImageSave(object image, Stream stream, ImageFormat format);

        /// <summary>
        /// Gets native image size in pixels.
        /// </summary>
        /// <param name="image">Native image instance.</param>
        /// <returns></returns>
        public abstract SizeI GetImagePixelSize(object image);

        /// <summary>
        /// Loads native image from the stream.
        /// </summary>
        /// <param name="image">Native image instance.</param>
        /// <param name="stream">Stream with image data.</param>
        public abstract bool ImageLoadFromStream(object image, Stream stream);
    }
}
