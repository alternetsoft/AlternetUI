#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/bitmap.h>
#include <wx/rawbmp.h>

namespace Alternet::UI
{
    typedef wxPixelData<wxBitmap, wxNativePixelFormat> ImageNativePixelData;

    typedef wxPixelData<wxBitmap, wxAlphaPixelFormat> ImageAlphaPixelData;

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
        ImageAlphaPixelData* pixelData = nullptr;
        int _stride = 0;
    };
}
