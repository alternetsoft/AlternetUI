using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public abstract class PlessImageHandler : DisposableObject
    {
        public virtual bool Load(string name, BitmapType type)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.OpenRead(name);
                return LoadFromStream(stream, type);
            });
        }

        public abstract bool LoadFromStream(Stream stream);

        public virtual bool LoadFromStream(Stream stream, BitmapType type)
        {
            return LoadFromStream(stream);
        }

        public virtual bool SaveToFile(string name, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                var bitmapType = Image.GetBitmapTypeFromFileName(name);
                return SaveToStream(stream, bitmapType, quality);
            });
        }

        public virtual bool SaveToFile(string name, BitmapType type, int quality)
        {
            return InsideTryCatch(() =>
            {
                using var stream = FileSystem.Default.Create(name);
                return SaveToStream(stream, type, quality);
            });
        }

        public virtual bool SaveToStream(Stream stream, ImageFormat format, int quality)
        {
            var bitmapType = format.AsBitmapType();
            if (bitmapType == BitmapType.Invalid)
                return false;
            return SaveToStream(stream, bitmapType, quality);
        }

        public abstract bool SaveToStream(Stream stream, BitmapType type, int quality);
    }
}
