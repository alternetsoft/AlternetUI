// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "ImageList.h"
#include "Image.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API ImageList* ImageList_Create_()
{
    return new ImageList();
}

ALTERNET_UI_API Size_C ImageList_GetPixelImageSize_(ImageList* obj)
{
    return obj->GetPixelImageSize();
}

ALTERNET_UI_API void ImageList_SetPixelImageSize_(ImageList* obj, Size value)
{
    obj->SetPixelImageSize(value);
}

ALTERNET_UI_API SizeF_C ImageList_GetImageSize_(ImageList* obj)
{
    return obj->GetImageSize();
}

ALTERNET_UI_API void ImageList_SetImageSize_(ImageList* obj, SizeF value)
{
    obj->SetImageSize(value);
}

ALTERNET_UI_API void ImageList_AddImage_(ImageList* obj, Image* image)
{
    obj->AddImage(image);
}

