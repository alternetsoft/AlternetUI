// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "PrintDocument.h"
#include "PrinterSettings.h"
#include "PageSettings.h"
#include "DrawingContext.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API PrintDocument* PrintDocument_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PrintDocument*>([&](){
    #endif
        return new PrintDocument();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PrintDocument_GetOriginAtMargins_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetOriginAtMargins();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PrintDocument_SetOriginAtMargins_(PrintDocument* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetOriginAtMargins(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API char16_t* PrintDocument_GetDocumentName_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<char16_t*>([&](){
    #endif
        return AllocPInvokeReturnString(obj->GetDocumentName());
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PrintDocument_SetDocumentName_(PrintDocument* obj, const char16_t* value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetDocumentName(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API PrinterSettings* PrintDocument_GetPrinterSettings_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PrinterSettings*>([&](){
    #endif
        return obj->GetPrinterSettings();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API PageSettings* PrintDocument_GetPageSettings_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PageSettings*>([&](){
    #endif
        return obj->GetPageSettings();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API DrawingContext* PrintDocument_GetPrintPage_DrawingContext_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<DrawingContext*>([&](){
    #endif
        return obj->GetPrintPage_DrawingContext();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PrintDocument_GetPrintPage_HasMorePages_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetPrintPage_HasMorePages();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PrintDocument_SetPrintPage_HasMorePages_(PrintDocument* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetPrintPage_HasMorePages(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API RectD_C PrintDocument_GetPrintPage_MarginBounds_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<RectD_C>([&](){
    #endif
        return obj->GetPrintPage_MarginBounds();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API RectD_C PrintDocument_GetPrintPage_PhysicalPageBounds_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<RectD_C>([&](){
    #endif
        return obj->GetPrintPage_PhysicalPageBounds();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API RectD_C PrintDocument_GetPrintPage_PageBounds_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<RectD_C>([&](){
    #endif
        return obj->GetPrintPage_PageBounds();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API RectD_C PrintDocument_GetPrintPage_PrintablePageBounds_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<RectD_C>([&](){
    #endif
        return obj->GetPrintPage_PrintablePageBounds();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API int PrintDocument_GetPrintPage_PageNumber_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<int>([&](){
    #endif
        return obj->GetPrintPage_PageNumber();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PrintDocument_Print_(PrintDocument* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->Print();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PrintDocument_SetEventCallback_(PrintDocument::PrintDocumentEventCallbackType callback)
{
    PrintDocument::SetEventCallback(callback);
}

