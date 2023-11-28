// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2023 AlterNET Software.</auto-generated>

#pragma once

#include "GenericImage.h"
#include "InputStream.h"
#include "OutputStream.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API GenericImage* GenericImage_Create_()
{
    return new GenericImage();
}

ALTERNET_UI_API void* GenericImage_FindHandlerByExt_(const char16_t* extension, int bitmapType)
{
    return GenericImage::FindHandlerByExt(extension, bitmapType);
}

ALTERNET_UI_API void* GenericImage_FindHandlerByBitmapType_(int bitmapType)
{
    return GenericImage::FindHandlerByBitmapType(bitmapType);
}

ALTERNET_UI_API void* GenericImage_FindHandlerByMime_(const char16_t* mimetype)
{
    return GenericImage::FindHandlerByMime(mimetype);
}

ALTERNET_UI_API void GenericImage_InsertHandler_(void* handler)
{
    GenericImage::InsertHandler(handler);
}

ALTERNET_UI_API c_bool GenericImage_RemoveHandler_(const char16_t* name)
{
    return GenericImage::RemoveHandler(name);
}

ALTERNET_UI_API int GenericImage_GetImageCountInFile_(const char16_t* filename, int bitmapType)
{
    return GenericImage::GetImageCountInFile(filename, bitmapType);
}

ALTERNET_UI_API int GenericImage_GetImageCountInStream_(void* stream, int bitmapType)
{
    return GenericImage::GetImageCountInStream(stream, bitmapType);
}

ALTERNET_UI_API void* GenericImage_GetAlphaData_(void* handle)
{
    return GenericImage::GetAlphaData(handle);
}

ALTERNET_UI_API void* GenericImage_GetData_(void* handle)
{
    return GenericImage::GetData(handle);
}

ALTERNET_UI_API c_bool GenericImage_CreateData_(void* handle, int width, int height, void* data, c_bool static_data)
{
    return GenericImage::CreateData(handle, width, height, data, static_data);
}

ALTERNET_UI_API c_bool GenericImage_CreateAlphaData_(void* handle, int width, int height, void* data, void* alpha, c_bool static_data)
{
    return GenericImage::CreateAlphaData(handle, width, height, data, alpha, static_data);
}

ALTERNET_UI_API void GenericImage_SetAlphaData_(void* handle, void* alpha, c_bool static_data)
{
    GenericImage::SetAlphaData(handle, alpha, static_data);
}

ALTERNET_UI_API void GenericImage_SetData_(void* handle, void* data, c_bool static_data)
{
    GenericImage::SetData(handle, data, static_data);
}

ALTERNET_UI_API void GenericImage_SetDataWithSize_(void* handle, void* data, int new_width, int new_height, c_bool static_data)
{
    GenericImage::SetDataWithSize(handle, data, new_width, new_height, static_data);
}

ALTERNET_UI_API void* GenericImage_CreateImage_()
{
    return GenericImage::CreateImage();
}

ALTERNET_UI_API void* GenericImage_CreateImageWithSize_(int width, int height, c_bool clear)
{
    return GenericImage::CreateImageWithSize(width, height, clear);
}

ALTERNET_UI_API void* GenericImage_CreateImageFromFileWithBitmapType_(const char16_t* name, int bitmapType, int index)
{
    return GenericImage::CreateImageFromFileWithBitmapType(name, bitmapType, index);
}

ALTERNET_UI_API void* GenericImage_CreateImageFromFileWithMimeType_(const char16_t* name, const char16_t* mimetype, int index)
{
    return GenericImage::CreateImageFromFileWithMimeType(name, mimetype, index);
}

ALTERNET_UI_API void* GenericImage_CreateImageFromStreamWithBitmapData_(void* stream, int bitmapType, int index)
{
    return GenericImage::CreateImageFromStreamWithBitmapData(stream, bitmapType, index);
}

ALTERNET_UI_API void* GenericImage_CreateImageFromStreamWithMimeType_(void* stream, const char16_t* mimetype, int index)
{
    return GenericImage::CreateImageFromStreamWithMimeType(stream, mimetype, index);
}

ALTERNET_UI_API void* GenericImage_CreateImageWithSizeAndData_(int width, int height, void* data, c_bool static_data)
{
    return GenericImage::CreateImageWithSizeAndData(width, height, data, static_data);
}

ALTERNET_UI_API void* GenericImage_CreateImageWithAlpha_(int width, int height, void* data, void* alpha, c_bool static_data)
{
    return GenericImage::CreateImageWithAlpha(width, height, data, alpha, static_data);
}

ALTERNET_UI_API void GenericImage_DeleteImage_(void* handle)
{
    GenericImage::DeleteImage(handle);
}

ALTERNET_UI_API void GenericImage_SetAlpha_(void* handle, int x, int y, uint8_t alpha)
{
    GenericImage::SetAlpha(handle, x, y, alpha);
}

ALTERNET_UI_API void GenericImage_ClearAlpha_(void* handle)
{
    GenericImage::ClearAlpha(handle);
}

ALTERNET_UI_API void GenericImage_SetLoadFlags_(void* handle, int flags)
{
    GenericImage::SetLoadFlags(handle, flags);
}

ALTERNET_UI_API void GenericImage_SetMask_(void* handle, c_bool hasMask)
{
    GenericImage::SetMask(handle, hasMask);
}

ALTERNET_UI_API void GenericImage_SetMaskColor_(void* handle, uint8_t red, uint8_t green, uint8_t blue)
{
    GenericImage::SetMaskColor(handle, red, green, blue);
}

ALTERNET_UI_API c_bool GenericImage_SetMaskFromImage_(void* handle, void* image, uint8_t mr, uint8_t mg, uint8_t mb)
{
    return GenericImage::SetMaskFromImage(handle, image, mr, mg, mb);
}

ALTERNET_UI_API void GenericImage_SetOptionString_(void* handle, const char16_t* name, const char16_t* value)
{
    GenericImage::SetOptionString(handle, name, value);
}

ALTERNET_UI_API void GenericImage_SetOptionInt_(void* handle, const char16_t* name, int value)
{
    GenericImage::SetOptionInt(handle, name, value);
}

ALTERNET_UI_API void GenericImage_SetRGB_(void* handle, int x, int y, uint8_t r, uint8_t g, uint8_t b)
{
    GenericImage::SetRGB(handle, x, y, r, g, b);
}

ALTERNET_UI_API void GenericImage_SetRGBRect_(void* handle, Int32Rect rect, uint8_t red, uint8_t green, uint8_t blue)
{
    GenericImage::SetRGBRect(handle, rect, red, green, blue);
}

ALTERNET_UI_API void GenericImage_SetImageType_(void* handle, int type)
{
    GenericImage::SetImageType(handle, type);
}

ALTERNET_UI_API void GenericImage_SetDefaultLoadFlags_(int flags)
{
    GenericImage::SetDefaultLoadFlags(flags);
}

ALTERNET_UI_API int GenericImage_GetLoadFlags_(void* handle)
{
    return GenericImage::GetLoadFlags(handle);
}

ALTERNET_UI_API void* GenericImage_Copy_(void* handle)
{
    return GenericImage::Copy(handle);
}

ALTERNET_UI_API c_bool GenericImage_CreateFreshImage_(void* handle, int width, int height, c_bool clear)
{
    return GenericImage::CreateFreshImage(handle, width, height, clear);
}

ALTERNET_UI_API void GenericImage_Clear_(void* handle, uint8_t value)
{
    GenericImage::Clear(handle, value);
}

ALTERNET_UI_API void GenericImage_DestroyImageData_(void* handle)
{
    GenericImage::DestroyImageData(handle);
}

ALTERNET_UI_API void GenericImage_InitAlpha_(void* handle)
{
    GenericImage::InitAlpha(handle);
}

ALTERNET_UI_API void* GenericImage_Blur_(void* handle, int blurRadius)
{
    return GenericImage::Blur(handle, blurRadius);
}

ALTERNET_UI_API void* GenericImage_BlurHorizontal_(void* handle, int blurRadius)
{
    return GenericImage::BlurHorizontal(handle, blurRadius);
}

ALTERNET_UI_API void* GenericImage_BlurVertical_(void* handle, int blurRadius)
{
    return GenericImage::BlurVertical(handle, blurRadius);
}

ALTERNET_UI_API void* GenericImage_Mirror_(void* handle, c_bool horizontally)
{
    return GenericImage::Mirror(handle, horizontally);
}

ALTERNET_UI_API void GenericImage_Paste_(void* handle, void* image, int x, int y, int alphaBlend)
{
    GenericImage::Paste(handle, image, x, y, alphaBlend);
}

ALTERNET_UI_API void GenericImage_Replace_(void* handle, uint8_t r1, uint8_t g1, uint8_t b1, uint8_t r2, uint8_t g2, uint8_t b2)
{
    GenericImage::Replace(handle, r1, g1, b1, r2, g2, b2);
}

ALTERNET_UI_API void* GenericImage_Rescale_(void* handle, int width, int height, int quality)
{
    return GenericImage::Rescale(handle, width, height, quality);
}

ALTERNET_UI_API void* GenericImage_Resize_(void* handle, Int32Size size, Int32Point pos, int red, int green, int blue)
{
    return GenericImage::Resize(handle, size, pos, red, green, blue);
}

ALTERNET_UI_API void* GenericImage_Rotate90_(void* handle, c_bool clockwise)
{
    return GenericImage::Rotate90(handle, clockwise);
}

ALTERNET_UI_API void* GenericImage_Rotate180_(void* handle)
{
    return GenericImage::Rotate180(handle);
}

ALTERNET_UI_API void GenericImage_RotateHue_(void* handle, double angle)
{
    GenericImage::RotateHue(handle, angle);
}

ALTERNET_UI_API void GenericImage_ChangeSaturation_(void* handle, double factor)
{
    GenericImage::ChangeSaturation(handle, factor);
}

ALTERNET_UI_API void GenericImage_ChangeBrightness_(void* handle, double factor)
{
    GenericImage::ChangeBrightness(handle, factor);
}

ALTERNET_UI_API void GenericImage_ChangeHSV_(void* handle, double angleH, double factorS, double factorV)
{
    GenericImage::ChangeHSV(handle, angleH, factorS, factorV);
}

ALTERNET_UI_API void* GenericImage_Scale_(void* handle, int width, int height, int quality)
{
    return GenericImage::Scale(handle, width, height, quality);
}

ALTERNET_UI_API void* GenericImage_Size_(void* handle, Int32Size size, Int32Point pos, int red, int green, int blue)
{
    return GenericImage::Size(handle, size, pos, red, green, blue);
}

ALTERNET_UI_API c_bool GenericImage_ConvertAlphaToMask_(void* handle, uint8_t threshold)
{
    return GenericImage::ConvertAlphaToMask(handle, threshold);
}

ALTERNET_UI_API c_bool GenericImage_ConvertAlphaToMaskUseColor_(void* handle, uint8_t mr, uint8_t mg, uint8_t mb, uint8_t threshold)
{
    return GenericImage::ConvertAlphaToMaskUseColor(handle, mr, mg, mb, threshold);
}

ALTERNET_UI_API void* GenericImage_ConvertToGreyscaleEx_(void* handle, double weight_r, double weight_g, double weight_b)
{
    return GenericImage::ConvertToGreyscaleEx(handle, weight_r, weight_g, weight_b);
}

ALTERNET_UI_API void* GenericImage_ConvertToGreyscale_(void* handle)
{
    return GenericImage::ConvertToGreyscale(handle);
}

ALTERNET_UI_API void* GenericImage_ConvertToMono_(void* handle, uint8_t r, uint8_t g, uint8_t b)
{
    return GenericImage::ConvertToMono(handle, r, g, b);
}

ALTERNET_UI_API void* GenericImage_ConvertToDisabled_(void* handle, uint8_t brightness)
{
    return GenericImage::ConvertToDisabled(handle, brightness);
}

ALTERNET_UI_API void* GenericImage_ChangeLightness_(void* handle, int alpha)
{
    return GenericImage::ChangeLightness(handle, alpha);
}

ALTERNET_UI_API uint8_t GenericImage_GetAlpha_(void* handle, int x, int y)
{
    return GenericImage::GetAlpha(handle, x, y);
}

ALTERNET_UI_API uint8_t GenericImage_GetRed_(void* handle, int x, int y)
{
    return GenericImage::GetRed(handle, x, y);
}

ALTERNET_UI_API uint8_t GenericImage_GetGreen_(void* handle, int x, int y)
{
    return GenericImage::GetGreen(handle, x, y);
}

ALTERNET_UI_API uint8_t GenericImage_GetBlue_(void* handle, int x, int y)
{
    return GenericImage::GetBlue(handle, x, y);
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskRed_(void* handle)
{
    return GenericImage::GetMaskRed(handle);
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskGreen_(void* handle)
{
    return GenericImage::GetMaskGreen(handle);
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskBlue_(void* handle)
{
    return GenericImage::GetMaskBlue(handle);
}

ALTERNET_UI_API int GenericImage_GetWidth_(void* handle)
{
    return GenericImage::GetWidth(handle);
}

ALTERNET_UI_API int GenericImage_GetHeight_(void* handle)
{
    return GenericImage::GetHeight(handle);
}

ALTERNET_UI_API Int32Size_C GenericImage_GetSize_(void* handle)
{
    return GenericImage::GetSize(handle);
}

ALTERNET_UI_API char16_t* GenericImage_GetOptionString_(void* handle, const char16_t* name)
{
    return AllocPInvokeReturnString(GenericImage::GetOptionString(handle, name));
}

ALTERNET_UI_API int GenericImage_GetOptionInt_(void* handle, const char16_t* name)
{
    return GenericImage::GetOptionInt(handle, name);
}

ALTERNET_UI_API void* GenericImage_GetSubImage_(void* handle, Int32Rect rect)
{
    return GenericImage::GetSubImage(handle, rect);
}

ALTERNET_UI_API int GenericImage_GetImageType_(void* handle)
{
    return GenericImage::GetImageType(handle);
}

ALTERNET_UI_API c_bool GenericImage_HasAlpha_(void* handle)
{
    return GenericImage::HasAlpha(handle);
}

ALTERNET_UI_API c_bool GenericImage_HasMask_(void* handle)
{
    return GenericImage::HasMask(handle);
}

ALTERNET_UI_API c_bool GenericImage_HasOption_(void* handle, const char16_t* name)
{
    return GenericImage::HasOption(handle, name);
}

ALTERNET_UI_API c_bool GenericImage_IsOk_(void* handle)
{
    return GenericImage::IsOk(handle);
}

ALTERNET_UI_API c_bool GenericImage_IsTransparent_(void* handle, int x, int y, uint8_t threshold)
{
    return GenericImage::IsTransparent(handle, x, y, threshold);
}

ALTERNET_UI_API c_bool GenericImage_LoadStreamWithBitmapType_(void* handle, void* stream, int bitmapType, int index)
{
    return GenericImage::LoadStreamWithBitmapType(handle, stream, bitmapType, index);
}

ALTERNET_UI_API c_bool GenericImage_LoadFileWithBitmapType_(void* handle, const char16_t* name, int bitmapType, int index)
{
    return GenericImage::LoadFileWithBitmapType(handle, name, bitmapType, index);
}

ALTERNET_UI_API c_bool GenericImage_LoadFileWithMimeType_(void* handle, const char16_t* name, const char16_t* mimetype, int index)
{
    return GenericImage::LoadFileWithMimeType(handle, name, mimetype, index);
}

ALTERNET_UI_API c_bool GenericImage_LoadStreamWithMimeType_(void* handle, void* stream, const char16_t* mimetype, int index)
{
    return GenericImage::LoadStreamWithMimeType(handle, stream, mimetype, index);
}

ALTERNET_UI_API c_bool GenericImage_SaveStreamWithMimeType_(void* handle, void* stream, const char16_t* mimetype)
{
    return GenericImage::SaveStreamWithMimeType(handle, stream, mimetype);
}

ALTERNET_UI_API c_bool GenericImage_SaveFileWithBitmapType_(void* handle, const char16_t* name, int bitmapType)
{
    return GenericImage::SaveFileWithBitmapType(handle, name, bitmapType);
}

ALTERNET_UI_API c_bool GenericImage_SaveFileWithMimeType_(void* handle, const char16_t* name, const char16_t* mimetype)
{
    return GenericImage::SaveFileWithMimeType(handle, name, mimetype);
}

ALTERNET_UI_API c_bool GenericImage_SaveFile_(void* handle, const char16_t* name)
{
    return GenericImage::SaveFile(handle, name);
}

ALTERNET_UI_API c_bool GenericImage_SaveStreamWithBitmapType_(void* handle, void* stream, int type)
{
    return GenericImage::SaveStreamWithBitmapType(handle, stream, type);
}

ALTERNET_UI_API c_bool GenericImage_CanRead_(const char16_t* filename)
{
    return GenericImage::CanRead(filename);
}

ALTERNET_UI_API c_bool GenericImage_CanReadStream_(void* stream)
{
    return GenericImage::CanReadStream(stream);
}

ALTERNET_UI_API int GenericImage_GetDefaultLoadFlags_()
{
    return GenericImage::GetDefaultLoadFlags();
}

ALTERNET_UI_API char16_t* GenericImage_GetImageExtWildcard_()
{
    return AllocPInvokeReturnString(GenericImage::GetImageExtWildcard());
}

ALTERNET_UI_API void GenericImage_AddHandler_(void* handler)
{
    GenericImage::AddHandler(handler);
}

ALTERNET_UI_API void GenericImage_CleanUpHandlers_()
{
    GenericImage::CleanUpHandlers();
}

ALTERNET_UI_API void* GenericImage_FindHandlerByName_(const char16_t* name)
{
    return GenericImage::FindHandlerByName(name);
}

