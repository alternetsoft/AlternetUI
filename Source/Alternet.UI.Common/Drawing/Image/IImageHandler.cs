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
        double ScaleFactor { get; set; }

        SizeI DipSize { get; }

        double ScaledHeight { get; }

        SizeI ScaledSize { get; }

        double ScaledWidth { get; }

        SizeI PixelSize { get; }

        bool IsOk { get; }

        bool HasAlpha { get; set; }

        int Depth { get; }

        bool SaveToStream(Stream stream, ImageFormat format);

        bool LoadFromStream(Stream stream);

        bool Load(string name, BitmapType type);

        bool LoadFromStream(Stream stream, BitmapType type);

        bool SaveToFile(string name);

        bool SaveToFile(string name, BitmapType type);

        bool SaveToStream(Stream stream, BitmapType type);

        GenericImage ToGenericImage();

        IImageHandler ConvertToDisabled(byte brightness = 255);

        IImageHandler GetSubBitmap(RectI rect);

        bool GrayScale();

        void ResetAlpha();

        void Rescale(SizeI sizeNeeded);
    }
}
