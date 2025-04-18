// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "GenericImage.h"
#include "InputStream.h"
#include "OutputStream.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API GenericImage* GenericImage_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<GenericImage*>([&](){
    #endif
        return new GenericImage();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API Color_C GenericImage_FindFirstUnusedColor_(void* handle, uint8_t startR, uint8_t startG, uint8_t startB)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Color_C>([&](){
    #endif
        return GenericImage::FindFirstUnusedColor(handle, startR, startG, startB);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImage_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImage();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageWithSize_(int width, int height, c_bool clear)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageWithSize(width, height, clear);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageFromFileWithBitmapType_(const char16_t* name, int bitmapType, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageFromFileWithBitmapType(name, bitmapType, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageFromFileWithMimeType_(const char16_t* name, const char16_t* mimetype, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageFromFileWithMimeType(name, mimetype, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageFromStreamWithBitmapData_(void* stream, int bitmapType, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageFromStreamWithBitmapData(stream, bitmapType, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageFromStreamWithMimeType_(void* stream, const char16_t* mimetype, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageFromStreamWithMimeType(stream, mimetype, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageWithSizeAndData_(int width, int height, void* data, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageWithSizeAndData(width, height, data, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_CreateImageWithAlpha_(int width, int height, void* data, void* alpha, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::CreateImageWithAlpha(width, height, data, alpha, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_DeleteImage_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::DeleteImage(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetAlpha_(void* handle, int x, int y, uint8_t alpha)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetAlpha(handle, x, y, alpha);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_ClearAlpha_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::ClearAlpha(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetLoadFlags_(void* handle, int flags)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetLoadFlags(handle, flags);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetMask_(void* handle, c_bool hasMask)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetMask(handle, hasMask);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetMaskColor_(void* handle, uint8_t red, uint8_t green, uint8_t blue)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetMaskColor(handle, red, green, blue);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SetMaskFromImage_(void* handle, void* image, uint8_t mr, uint8_t mg, uint8_t mb)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SetMaskFromImage(handle, image, mr, mg, mb);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetOptionString_(void* handle, const char16_t* name, const char16_t* value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetOptionString(handle, name, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetOptionInt_(void* handle, const char16_t* name, int value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetOptionInt(handle, name, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetRGB_(void* handle, int x, int y, uint8_t r, uint8_t g, uint8_t b)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetRGB(handle, x, y, r, g, b);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetRGBRect_(void* handle, RectI rect, uint8_t red, uint8_t green, uint8_t blue)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetRGBRect(handle, rect, red, green, blue);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetImageType_(void* handle, int type)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetImageType(handle, type);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetDefaultLoadFlags_(int flags)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetDefaultLoadFlags(flags);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetLoadFlags_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetLoadFlags(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Copy_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Copy(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_CreateFreshImage_(void* handle, int width, int height, c_bool clear)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::CreateFreshImage(handle, width, height, clear);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_Clear_(void* handle, uint8_t value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::Clear(handle, value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_DestroyImageData_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::DestroyImageData(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_InitAlpha_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::InitAlpha(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Blur_(void* handle, int blurRadius)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Blur(handle, blurRadius);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_BlurHorizontal_(void* handle, int blurRadius)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::BlurHorizontal(handle, blurRadius);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_BlurVertical_(void* handle, int blurRadius)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::BlurVertical(handle, blurRadius);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Mirror_(void* handle, c_bool horizontally)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Mirror(handle, horizontally);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_Paste_(void* handle, void* image, int x, int y, int alphaBlend)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::Paste(handle, image, x, y, alphaBlend);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_Replace_(void* handle, uint8_t r1, uint8_t g1, uint8_t b1, uint8_t r2, uint8_t g2, uint8_t b2)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::Replace(handle, r1, g1, b1, r2, g2, b2);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_Rescale_(void* handle, int width, int height, int quality)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::Rescale(handle, width, height, quality);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_Resize_(void* handle, SizeI size, PointI pos, int red, int green, int blue)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::Resize(handle, size, pos, red, green, blue);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Rotate90_(void* handle, c_bool clockwise)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Rotate90(handle, clockwise);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Rotate180_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Rotate180(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_RotateHue_(void* handle, double angle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::RotateHue(handle, angle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_ChangeSaturation_(void* handle, double factor)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::ChangeSaturation(handle, factor);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_ChangeBrightness_(void* handle, double factor)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::ChangeBrightness(handle, factor);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_ChangeHSV_(void* handle, double angleH, double factorS, double factorV)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::ChangeHSV(handle, angleH, factorS, factorV);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Scale_(void* handle, int width, int height, int quality)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Scale(handle, width, height, quality);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_Size_(void* handle, SizeI size, PointI pos, int red, int green, int blue)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::Size(handle, size, pos, red, green, blue);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_ConvertAlphaToMask_(void* handle, uint8_t threshold)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::ConvertAlphaToMask(handle, threshold);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_ConvertAlphaToMaskUseColor_(void* handle, uint8_t mr, uint8_t mg, uint8_t mb, uint8_t threshold)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::ConvertAlphaToMaskUseColor(handle, mr, mg, mb, threshold);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_ConvertToGreyscaleEx_(void* handle, double weight_r, double weight_g, double weight_b)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::ConvertToGreyscaleEx(handle, weight_r, weight_g, weight_b);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_ConvertToGreyscale_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::ConvertToGreyscale(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_ConvertToMono_(void* handle, uint8_t r, uint8_t g, uint8_t b)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::ConvertToMono(handle, r, g, b);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_ConvertToDisabled_(void* handle, uint8_t brightness)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::ConvertToDisabled(handle, brightness);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_ChangeLightness_(void* handle, int alpha)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::ChangeLightness(handle, alpha);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetAlpha_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetAlpha(handle, x, y);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetRed_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetRed(handle, x, y);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetGreen_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetGreen(handle, x, y);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetBlue_(void* handle, int x, int y)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetBlue(handle, x, y);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskRed_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetMaskRed(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskGreen_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetMaskGreen(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t GenericImage_GetMaskBlue_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return GenericImage::GetMaskBlue(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetWidth_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetWidth(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetHeight_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetHeight(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API SizeI_C GenericImage_GetSize_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<SizeI_C>([&](){
    #endif
        return GenericImage::GetSize(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API char16_t* GenericImage_GetOptionString_(void* handle, const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<char16_t*>([&](){
    #endif
        return AllocPInvokeReturnString(GenericImage::GetOptionString(handle, name));
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetOptionInt_(void* handle, const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetOptionInt(handle, name);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_GetSubImage_(void* handle, RectI rect)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::GetSubImage(handle, rect);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetImageType_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetImageType(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_HasAlpha_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::HasAlpha(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_HasMask_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::HasMask(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_HasOption_(void* handle, const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::HasOption(handle, name);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_IsOk_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::IsOk(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_IsTransparent_(void* handle, int x, int y, uint8_t threshold)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::IsTransparent(handle, x, y, threshold);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_LoadStreamWithBitmapType_(void* handle, void* stream, int bitmapType, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::LoadStreamWithBitmapType(handle, stream, bitmapType, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_LoadFileWithBitmapType_(void* handle, const char16_t* name, int bitmapType, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::LoadFileWithBitmapType(handle, name, bitmapType, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_LoadFileWithMimeType_(void* handle, const char16_t* name, const char16_t* mimetype, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::LoadFileWithMimeType(handle, name, mimetype, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_LoadStreamWithMimeType_(void* handle, void* stream, const char16_t* mimetype, int index)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::LoadStreamWithMimeType(handle, stream, mimetype, index);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SaveStreamWithMimeType_(void* handle, void* stream, const char16_t* mimetype)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SaveStreamWithMimeType(handle, stream, mimetype);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SaveFileWithBitmapType_(void* handle, const char16_t* name, int bitmapType)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SaveFileWithBitmapType(handle, name, bitmapType);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SaveFileWithMimeType_(void* handle, const char16_t* name, const char16_t* mimetype)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SaveFileWithMimeType(handle, name, mimetype);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SaveFile_(void* handle, const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SaveFile(handle, name);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_SaveStreamWithBitmapType_(void* handle, void* stream, int type)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::SaveStreamWithBitmapType(handle, stream, type);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_CanRead_(const char16_t* filename)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::CanRead(filename);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_CanReadStream_(void* stream)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::CanReadStream(stream);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetDefaultLoadFlags_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetDefaultLoadFlags();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API char16_t* GenericImage_GetImageExtWildcard_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<char16_t*>([&](){
    #endif
        return AllocPInvokeReturnString(GenericImage::GetImageExtWildcard());
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_AddHandler_(void* handler)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::AddHandler(handler);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_CleanUpHandlers_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::CleanUpHandlers();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_FindHandlerByName_(const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::FindHandlerByName(name);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_FindHandlerByExt_(const char16_t* extension, int bitmapType)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::FindHandlerByExt(extension, bitmapType);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_FindHandlerByBitmapType_(int bitmapType)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::FindHandlerByBitmapType(bitmapType);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_FindHandlerByMime_(const char16_t* mimetype)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::FindHandlerByMime(mimetype);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_InsertHandler_(void* handler)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::InsertHandler(handler);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_RemoveHandler_(const char16_t* name)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::RemoveHandler(name);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetImageCountInFile_(const char16_t* filename, int bitmapType)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetImageCountInFile(filename, bitmapType);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetImageCountInStream_(void* stream, int bitmapType)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetImageCountInStream(stream, bitmapType);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_GetAlphaData_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::GetAlphaData(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_GetData_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::GetData(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_CreateData_(void* handle, int width, int height, void* data, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::CreateData(handle, width, height, data, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool GenericImage_CreateAlphaData_(void* handle, int width, int height, void* data, void* alpha, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return GenericImage::CreateAlphaData(handle, width, height, data, alpha, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetAlphaData_(void* handle, void* alpha, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetAlphaData(handle, alpha, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetData_(void* handle, void* data, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetData(handle, data, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_SetDataWithSize_(void* handle, void* data, int new_width, int new_height, c_bool static_data)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::SetDataWithSize(handle, data, new_width, new_height, static_data);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void* GenericImage_LockBits_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<void*>([&](){
    #endif
        return GenericImage::LockBits(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int GenericImage_GetStride_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return GenericImage::GetStride(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void GenericImage_UnlockBits_(void* handle)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        GenericImage::UnlockBits(handle);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

