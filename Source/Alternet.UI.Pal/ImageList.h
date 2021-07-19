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

        void CreateImageList();
        void DestroyImageList();
        void RecreateImageList();

        void AddImageCore(const wxImage& image);

        Size _pixelImageSize;
        wxImageList* _imageList = nullptr;
    };
}
