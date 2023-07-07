#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Image : public Object
    {
#include "Api/Image.inc"
    public:
        wxBitmap GetBitmap();
        void SetBitmap(const wxBitmap& value);

        static void EnsureImageHandlersInitialized();
        static void wxInitAllImageHandlersV2();
    private:

        static wxBitmapType GetBitmapTypeFromFormat(const string& format);

        wxBitmap _bitmap; // reference-counted, so use copy-by-value.
    };
}
