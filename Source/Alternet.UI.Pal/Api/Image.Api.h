// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Image.h"
#include "InputStream.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Image* Image_Create()
{
    return new Image();
}

ALTERNET_UI_API int Image_GetWidth(Image* obj)
{
    return obj->GetWidth();
}

ALTERNET_UI_API void Image_LoadFromStream(Image* obj, void* stream)
{
    obj->LoadFromStream(stream);
}

