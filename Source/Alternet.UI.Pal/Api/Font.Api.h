// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Font.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Font* Font_Create_()
{
    return new Font();
}

ALTERNET_UI_API void* Font_OpenFamiliesArray_(Font* obj)
{
    return obj->OpenFamiliesArray();
}

ALTERNET_UI_API int Font_GetFamiliesItemCount_(Font* obj, void* array)
{
    return obj->GetFamiliesItemCount(array);
}

ALTERNET_UI_API char16_t* Font_GetFamiliesItemAt_(Font* obj, void* array, int index)
{
    return AllocPInvokeReturnString(obj->GetFamiliesItemAt(array, index));
}

ALTERNET_UI_API void Font_CloseFamiliesArray_(Font* obj, void* array)
{
    obj->CloseFamiliesArray(array);
}

ALTERNET_UI_API void Font_Initialize_(Font* obj, GenericFontFamily genericFamily, const char16_t* familyName, float emSize)
{
    obj->Initialize(genericFamily, ToOptional(familyName), emSize);
}

ALTERNET_UI_API void Font_InitializeWithDefaultFont_(Font* obj)
{
    obj->InitializeWithDefaultFont();
}

ALTERNET_UI_API c_bool Font_IsFamilyValid_(const char16_t* fontFamily)
{
    return Font::IsFamilyValid(fontFamily);
}

ALTERNET_UI_API char16_t* Font_GetGenericFamilyName_(GenericFontFamily genericFamily)
{
    return AllocPInvokeReturnString(Font::GetGenericFamilyName(genericFamily));
}

