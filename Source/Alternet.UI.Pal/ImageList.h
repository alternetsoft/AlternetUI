#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Image.h"

namespace Alternet::UI
{
    class ImageList
    {
#include "Api/ImageList.inc"
    public:
        wxImageList* GetImageList();

    private:
        wxImageList* _imageList;
    };
}
