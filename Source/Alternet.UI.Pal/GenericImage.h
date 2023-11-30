#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/image.h>

namespace Alternet::UI
{
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
    
    };
}
