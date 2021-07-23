// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Image.h"
#include "InputStream.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Image* Image_Create_()
{
    return new Image();
}

ALTERNET_UI_API SizeF_C Image_GetSize_(Image* obj)
{
    return obj->GetSize();
}

ALTERNET_UI_API Size_C Image_GetPixelSize_(Image* obj)
{
    return obj->GetPixelSize();
}

ALTERNET_UI_API void Image_LoadFromStream_(Image* obj, void* stream)
{
    obj->LoadFromStream(stream);
}

