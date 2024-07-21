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
    /// Implements abstract base class with some common methods and properties which
    /// allow to work with images.
    /// </summary>
    public abstract class PlessImageHandler : DisposableObject
    {
        /// <inheritdoc cref="Image.Load(string, BitmapType)"/>
        public virtual bool Load(string name, BitmapType type)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.OpenRead(name);
                return LoadFromStream(stream, type);
            });
        }

        /// <summary>
        /// Loads image from stream.
        /// </summary>
        /// <param name="stream">Stream with image data.</param>
        /// <returns></returns>
        public abstract bool LoadFromStream(Stream stream);

        /// <inheritdoc cref="Image.Load(Stream, BitmapType)"/>
        public virtual bool LoadFromStream(Stream stream, BitmapType type)
        {
            return LoadFromStream(stream);
        }

        /// <inheritdoc cref="Image.Save(string, int?)"/>
        public virtual bool SaveToFile(string name, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                var bitmapType = Image.GetBitmapTypeFromFileName(name);
                return SaveToStream(stream, bitmapType, quality);
            });
        }

        /// <inheritdoc cref="Image.Save(string, BitmapType, int?)"/>
        public virtual bool SaveToFile(string name, BitmapType type, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                return SaveToStream(stream, type, quality);
            });
        }

        /// <inheritdoc cref="Image.Save(Stream, ImageFormat, int?)"/>
        public virtual bool SaveToStream(Stream stream, ImageFormat format, int quality)
        {
            var bitmapType = format.AsBitmapType();
            if (bitmapType == BitmapType.Invalid)
                return false;
            return SaveToStream(stream, bitmapType, quality);
        }

        /// <inheritdoc cref="Image.Save(Stream, BitmapType, int?)"/>
        public abstract bool SaveToStream(Stream stream, BitmapType type, int quality);
    }
}
