#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/image.h>
#include <wx/bitmap.h>
#include <wx/rawbmp.h>

namespace Alternet::UI
{
    typedef wxPixelData<wxBitmap, wxNativePixelFormat> ImageNativePixelData;

    typedef wxPixelData<wxBitmap, wxAlphaPixelFormat> ImageAlphaPixelData;

    typedef wxPixelData<wxImage, wxImagePixelFormat> ImageGenericPixelData;


    class GenericImage : public Object
    {
#include "Api/GenericImage.inc"
    public:
        wxImage _image;

        static void EnsureImageHandlersInitialized();
        static void wxInitAllImageHandlersV2();

        GenericImage(const wxImage& image)
        {
            _image = image;
        }
    private:
        ImageGenericPixelData* pixelData = nullptr;
        int _stride = 0;
    };
}
