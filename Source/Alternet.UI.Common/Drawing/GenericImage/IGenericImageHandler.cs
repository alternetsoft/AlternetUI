using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.UI;

namespace Alternet.Drawing
{
    public interface IGenericImageHandler : IDisposable
    {
        GenericImageLoadFlags LoadFlags { get; set; }

        bool HasAlpha { get; }

        bool HasMask { get; }

        int Width { get; }

        int Height { get; }

        bool IsOk { get; }

        void SetAlpha(int x, int y, byte alpha);

        void ClearAlpha();

        void SetMask(bool hasMask = true);

        void SetMaskColor(RGBValue rgb);

        bool SetMaskFromImage(GenericImage image, RGBValue mask);

        void SetOptionAsString(string name, string value);

        void SetOptionAsInt(string name, int value);

        void SetRGB(int x, int y, RGBValue rgb);

        void SetRGBRect(RGBValue rgb, RectI? rect = null);

        void SetImageType(BitmapType type);

        GenericImage Copy();

        bool Reset(int width, int height, bool clear = false);

        void Clear(byte value = 0);

        void Reset();

        Color FindFirstUnusedColor(RGBValue? startRGB = null);

        void InitAlpha();

        GenericImage Blur(int blurRadius);

        GenericImage BlurHorizontal(int blurRadius);

        GenericImage BlurVertical(int blurRadius);

        GenericImage Mirror(bool horizontally = true);

        void Paste(
            GenericImage image,
            int x,
            int y,
            GenericImageAlphaBlendMode alphaBlend = GenericImageAlphaBlendMode.Overwrite);

        void Replace(RGBValue r1, RGBValue r2);

        void Rescale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

        void ResizeNoScale(
            SizeI size,
            PointI pos,
            RGBValue? color = null);

        GenericImage SizeNoScale(
            SizeI size,
            PointI pos = default,
            RGBValue? color = null);

        GenericImage Rotate90(bool clockwise = true);

        GenericImage Rotate180();

        void RotateHue(double angle);

        void ChangeSaturation(double factor);

        void ChangeBrightness(double factor);

        void ChangeHSV(double angleH, double factorS, double factorV);

        GenericImage Scale(
            int width,
            int height,
            GenericImageResizeQuality quality = GenericImageResizeQuality.Normal);

        bool ConvertAlphaToMask(byte threshold);

        bool ConvertAlphaToMask(RGBValue rgb, byte threshold);

        GenericImage ConvertToGreyscale(double weightR, double weightG, double weightB);

        GenericImage ConvertToGreyscale();

        GenericImage ConvertToMono(RGBValue rgb);

        GenericImage ConvertToDisabled(byte brightness = 255);

        GenericImage ChangeLightness(int ialpha);

        byte GetAlpha(int x, int y);

        RGBValue GetRGB(int x, int y);

        Color GetPixel(int x, int y, bool withAlpha = false);

        void SetPixel(int x, int y, Color color, bool withAlpha = false);

        byte GetRed(int x, int y);

        byte GetGreen(int x, int y);

        byte GetBlue(int x, int y);

        RGBValue GetMaskRGB();

        byte GetMaskRed();

        byte GetMaskGreen();

        byte GetMaskBlue();

        string GetOptionAsString(string name);

        int GetOptionAsInt(string name);

        GenericImage GetSubImage(RectI rect);

        BitmapType GetImageType();

        bool HasOption(string name);

        bool IsTransparent(int x, int y, byte threshold);

        bool LoadFromStream(
            Stream stream,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        bool LoadFromFile(
            string filename,
            BitmapType bitmapType = BitmapType.Any,
            int index = -1);

        bool LoadFromFile(
            string name,
            string mimetype,
            int index = -1);

        bool LoadFromStream(
            Stream stream,
            string mimetype,
            int index = -1);

        bool SaveToStream(Stream stream, string mimetype);

        bool SaveToFile(string filename, BitmapType bitmapType);

        bool SaveToFile(string filename, string mimetype);

        bool SaveToFile(string filename);

        bool SaveToStream(Stream stream, BitmapType type);

        void SetNativeData(
            IntPtr data,
            int new_width,
            int new_height,
            bool static_data = false);

        IntPtr GetNativeAlphaData();

        IntPtr GetNativeData();

        bool CreateNativeData(
            int width,
            int height,
            IntPtr data,
            bool staticData = false);

        bool CreateNativeData(
            int width,
            int height,
            IntPtr data,
            IntPtr alpha,
            bool staticData = false);

        void SetNativeAlphaData(
            IntPtr alpha = default,
            bool staticData = false);

        void SetNativeData(IntPtr data, bool staticData = false);
    }
}
