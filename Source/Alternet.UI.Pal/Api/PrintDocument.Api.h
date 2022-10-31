// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

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
    return MarshalExceptions<PrintDocument*>([&](){
            return new PrintDocument();
        });
}

ALTERNET_UI_API char16_t* PrintDocument_GetDocumentName_(PrintDocument* obj)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(obj->GetDocumentName());
        });
}

ALTERNET_UI_API void PrintDocument_SetDocumentName_(PrintDocument* obj, const char16_t* value)
{
    MarshalExceptions<void>([&](){
            obj->SetDocumentName(value);
        });
}

ALTERNET_UI_API PrinterSettings* PrintDocument_GetPrinterSettings_(PrintDocument* obj)
{
    return MarshalExceptions<PrinterSettings*>([&](){
            return obj->GetPrinterSettings();
        });
}

ALTERNET_UI_API void PrintDocument_SetPrinterSettings_(PrintDocument* obj, PrinterSettings* value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrinterSettings(value);
        });
}

ALTERNET_UI_API PageSettings* PrintDocument_GetDefaultPageSettings_(PrintDocument* obj)
{
    return MarshalExceptions<PageSettings*>([&](){
            return obj->GetDefaultPageSettings();
        });
}

ALTERNET_UI_API void PrintDocument_SetDefaultPageSettings_(PrintDocument* obj, PageSettings* value)
{
    MarshalExceptions<void>([&](){
            obj->SetDefaultPageSettings(value);
        });
}

ALTERNET_UI_API DrawingContext* PrintDocument_GetPrintPage_DrawingContext_(PrintDocument* obj)
{
    return MarshalExceptions<DrawingContext*>([&](){
            return obj->GetPrintPage_DrawingContext();
        });
}

ALTERNET_UI_API c_bool PrintDocument_GetPrintPage_HasMorePages_(PrintDocument* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetPrintPage_HasMorePages();
        });
}

ALTERNET_UI_API void PrintDocument_SetPrintPage_HasMorePages_(PrintDocument* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrintPage_HasMorePages(value);
        });
}

ALTERNET_UI_API PageSettings* PrintDocument_GetPrintPage_PageSettings_(PrintDocument* obj)
{
    return MarshalExceptions<PageSettings*>([&](){
            return obj->GetPrintPage_PageSettings();
        });
}

ALTERNET_UI_API Rect_C PrintDocument_GetPrintPage_MarginBounds_(PrintDocument* obj)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetPrintPage_MarginBounds();
        });
}

ALTERNET_UI_API Rect_C PrintDocument_GetPrintPage_PhysicalPageBounds_(PrintDocument* obj)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetPrintPage_PhysicalPageBounds();
        });
}

ALTERNET_UI_API Rect_C PrintDocument_GetPrintPage_PageBounds_(PrintDocument* obj)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetPrintPage_PageBounds();
        });
}

ALTERNET_UI_API Rect_C PrintDocument_GetPrintPage_PrintablePageBounds_(PrintDocument* obj)
{
    return MarshalExceptions<Rect_C>([&](){
            return obj->GetPrintPage_PrintablePageBounds();
        });
}

ALTERNET_UI_API void PrintDocument_Print_(PrintDocument* obj)
{
    MarshalExceptions<void>([&](){
            obj->Print();
        });
}

ALTERNET_UI_API void PrintDocument_SetEventCallback_(PrintDocument::PrintDocumentEventCallbackType callback)
{
    PrintDocument::SetEventCallback(callback);
}

