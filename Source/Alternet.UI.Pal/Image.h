#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "GenericImage.h"

#include <wx/bitmap.h>
#include <wx/rawbmp.h>

namespace Alternet::UI
{
    enum ImageStaticObjectId
    {
        ImageStaticObjectId_NativePixelFormat = 0,
        ImageStaticObjectId_AlphaPixelFormat = 1,
        ImageStaticObjectId_GenericPixelFormat = 2,
        ImageStaticObjectId_Unknown = -1,
    };

    enum ImageStaticPropertyId
    {
        ImageStaticPropertyId_BitsPerPixel = 0,
        ImageStaticPropertyId_HasAlpha = 1,
        ImageStaticPropertyId_SizePixel = 2,
        ImageStaticPropertyId_Red = 3,
        ImageStaticPropertyId_Green = 4,
        ImageStaticPropertyId_Blue = 5,
        ImageStaticPropertyId_Alpha = 6,
    };

    struct ImagePixelFormat
    {
    public:
        int BitsPerPixel = 0;
        int HasAlpha = 0;
        int SizePixel = 0;
        int Red = 0;
        int Green = 0;
        int Blue = 0;
        int Alpha = 0;

        void Log() const
        {
            LogMessage("BitsPerPixel = " + std::to_string(BitsPerPixel));
            LogMessage("HasAlpha = " + std::to_string(HasAlpha));
            LogMessage("SizePixel = " + std::to_string(SizePixel));
            LogMessage("RED = " + std::to_string(Red));
            LogMessage("GREEN = " + std::to_string(Green));
            LogMessage("BLUE = " + std::to_string(Blue));
            LogMessage("ALPHA = " + std::to_string(Alpha));
        }

        int GetProperty(ImageStaticPropertyId propId) const
        {
            switch (propId)
            {
                case ImageStaticPropertyId_BitsPerPixel:
                    return BitsPerPixel;
                case ImageStaticPropertyId_HasAlpha:
                    return HasAlpha;
                case ImageStaticPropertyId_SizePixel:
                    return SizePixel;
                case ImageStaticPropertyId_Red:
                    return Red;
                case ImageStaticPropertyId_Green:
                    return Green;
                case ImageStaticPropertyId_Blue:
                    return Blue;
                case ImageStaticPropertyId_Alpha:
                    return Alpha;
                default:
                    return 0;
            }
        }
    };

    class Image : public Object
    {
#include "Api/Image.inc"       
    public:                          
        wxBitmap GetBitmap();
        void SetBitmap(const wxBitmap& value);

        static wxBitmapBundle CreateFromSvgStr(const string& s, int width,
            int height, const Color& color);

        static wxBitmapBundle CreateFromSvgStream(void* stream, int width, int height,
            const Color& color);

        static wxBitmap GetWxBitmap(Image* bitmap)
        {
            if (bitmap != nullptr)
                return bitmap->GetBitmap();
            else
                return wxBitmap();
        }

        static wxBitmapType GetBitmapTypeFromFormat(const string& format);

        wxBitmap _bitmap; // reference-counted, so use copy-by-value.
    private:
        ImageAlphaPixelData* alphaPixelData = nullptr;
        ImageNativePixelData* nativePixelData = nullptr;
        int _stride = 0;
    };
}
