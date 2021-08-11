// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Font.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Font* Font_Create_()
{
    return new Font();
}

ALTERNET_UI_API void Font_Initialize_(Font* obj, const char16_t* familyName, float emSize)
{
    obj->Initialize(familyName, emSize);
}

ALTERNET_UI_API void Font_InitializeWithDefaultFont_(Font* obj)
{
    obj->InitializeWithDefaultFont();
}

