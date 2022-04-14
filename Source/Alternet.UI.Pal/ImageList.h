#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Image.h"
#include "Object.h"

namespace Alternet::UI
{
    class ImageList : public Object
    {
#include "Api/ImageList.inc"
    public:
        wxImageList* GetImageList();

    private:

        void CreateImageList();
        void DestroyImageList();
        void RecreateImageList();

        void AddImageCore(const wxImage& image);

        Int32Size _pixelImageSize;
        wxImageList* _imageList = nullptr;
    };
}
