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
        wxIconBundle* GetIconBundle();
        wxBitmapBundle GetBitmapBundle();

        static wxBitmapBundle BitmapBundle(ImageSet* bimtapBundle)
        {
            if (bimtapBundle == nullptr)
                return wxBitmapBundle();
            return bimtapBundle->GetBitmapBundle();
        }
    private:
        wxIconBundle _iconBundle;
        
        bool _bitmapBundleValid = false;
        wxBitmapBundle _bitmapBundle;
        
        wxVector<wxBitmap> _bitmaps;

        void InvalidateBitmapBundle();
    };
}
