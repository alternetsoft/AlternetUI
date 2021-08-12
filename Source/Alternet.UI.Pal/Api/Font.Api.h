// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.

#pragma once

#include "Font.h"
#include "ApiUtils.h"

using namespace Alternet::UI;

ALTERNET_UI_API Font* Font_Create_()
{
    return new Font();
}

ALTERNET_UI_API char16_t* Font_GetName_(Font* obj)
{
    return AllocPInvokeReturnString(obj->GetName());
}

ALTERNET_UI_API float Font_GetSize_(Font* obj)
{
    return obj->GetSize();
}

ALTERNET_UI_API void* Font_OpenFamiliesArray_()
{
    return Font::OpenFamiliesArray();
}

ALTERNET_UI_API int Font_GetFamiliesItemCount_(void* array)
{
    return Font::GetFamiliesItemCount(array);
}

ALTERNET_UI_API char16_t* Font_GetFamiliesItemAt_(void* array, int index)
{
    return AllocPInvokeReturnString(Font::GetFamiliesItemAt(array, index));
}

ALTERNET_UI_API void Font_CloseFamiliesArray_(void* array)
{
    Font::CloseFamiliesArray(array);
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

ALTERNET_UI_API char16_t* Font_ToString__(Font* obj)
{
    return AllocPInvokeReturnString(obj->ToString_());
}

ALTERNET_UI_API c_bool Font_IsEqualTo_(Font* obj, Font* other)
{
    return obj->IsEqualTo(other);
}

ALTERNET_UI_API int Font_GetHashCode__(Font* obj)
{
    return obj->GetHashCode_();
}

