// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "PageSettings.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API PageSettings* PageSettings_Create_()
{
    return MarshalExceptions<PageSettings*>([&](){
            return new PageSettings();
        });
}

ALTERNET_UI_API c_bool PageSettings_GetColor_(PageSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetColor();
        });
}

ALTERNET_UI_API void PageSettings_SetColor_(PageSettings* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetColor(value);
        });
}

ALTERNET_UI_API c_bool PageSettings_GetLandscape_(PageSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetLandscape();
        });
}

ALTERNET_UI_API void PageSettings_SetLandscape_(PageSettings* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetLandscape(value);
        });
}

ALTERNET_UI_API Thickness_C PageSettings_GetMargins_(PageSettings* obj)
{
    return MarshalExceptions<Thickness_C>([&](){
            return obj->GetMargins();
        });
}

ALTERNET_UI_API void PageSettings_SetMargins_(PageSettings* obj, Thickness value)
{
    MarshalExceptions<void>([&](){
            obj->SetMargins(value);
        });
}

ALTERNET_UI_API Size_C PageSettings_GetCustomPaperSize_(PageSettings* obj)
{
    return MarshalExceptions<Size_C>([&](){
            return obj->GetCustomPaperSize();
        });
}

ALTERNET_UI_API void PageSettings_SetCustomPaperSize_(PageSettings* obj, Size value)
{
    MarshalExceptions<void>([&](){
            obj->SetCustomPaperSize(value);
        });
}

ALTERNET_UI_API c_bool PageSettings_GetUseCustomPaperSize_(PageSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetUseCustomPaperSize();
        });
}

ALTERNET_UI_API void PageSettings_SetUseCustomPaperSize_(PageSettings* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetUseCustomPaperSize(value);
        });
}

ALTERNET_UI_API PaperKind PageSettings_GetPaperSize_(PageSettings* obj)
{
    return MarshalExceptions<PaperKind>([&](){
            return obj->GetPaperSize();
        });
}

ALTERNET_UI_API void PageSettings_SetPaperSize_(PageSettings* obj, PaperKind value)
{
    MarshalExceptions<void>([&](){
            obj->SetPaperSize(value);
        });
}

ALTERNET_UI_API PrinterResolutionKind PageSettings_GetPrinterResolution_(PageSettings* obj)
{
    return MarshalExceptions<PrinterResolutionKind>([&](){
            return obj->GetPrinterResolution();
        });
}

ALTERNET_UI_API void PageSettings_SetPrinterResolution_(PageSettings* obj, PrinterResolutionKind value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrinterResolution(value);
        });
}

