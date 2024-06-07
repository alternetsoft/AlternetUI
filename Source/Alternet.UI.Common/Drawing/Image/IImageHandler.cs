using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IImageHandler : IDisposable
    {
        Coord ScaleFactor { get; set; }

        SizeI DipSize { get; }

        Coord ScaledHeight { get; }

        SizeI ScaledSize { get; }

        Coord ScaledWidth { get; }

        SizeI PixelSize { get; }

        bool IsOk { get; }

        bool HasAlpha { get; set; }

        int Depth { get; }

        bool LoadFromStream(Stream stream);

        bool LoadFromStream(Stream stream, BitmapType type);

        bool SaveToStream(Stream stream, BitmapType type, int quality);

        GenericImage ToGenericImage();

        IImageHandler GetSubBitmap(RectI rect);

        bool GrayScale();

        bool ResetAlpha();

        bool Rescale(SizeI sizeNeeded);

        public IntPtr LockBits();

        public void UnlockBits();

        int GetStride();

        ISkiaSurface LockSurface();
    }
}
