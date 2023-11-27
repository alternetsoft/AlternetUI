#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"

namespace Alternet::UI
{
    class ImageSet : public Object
    {
#include "Api/ImageSet.inc"
    public:
        wxBitmapBundle GetBitmapBundle();

        static wxBitmapBundle BitmapBundle(ImageSet* bimtapBundle)
        {
            if (bimtapBundle == nullptr)
                return wxBitmapBundle();
            return bimtapBundle->GetBitmapBundle();
        }
    private:
        bool _readOnly = false;
        bool _bitmapBundleValid = false;
        wxBitmapBundle _bitmapBundle;
        
        wxVector<wxBitmap> _bitmaps;

        void InvalidateBitmapBundle();
        wxIconBundle GetIconBundle();
    };
}
