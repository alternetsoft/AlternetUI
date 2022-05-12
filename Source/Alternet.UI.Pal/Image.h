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

    private:
        wxBitmap _bitmap; // reference-counted, so use copy-by-value.
    };
}
