// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

#pragma once

#include "PrinterSettings.h"
#include "PageSettings.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API PrinterSettings* PrinterSettings_Create_()
{
    return MarshalExceptions<PrinterSettings*>([&](){
            return new PrinterSettings();
        });
}

ALTERNET_UI_API Duplex PrinterSettings_GetDuplex_(PrinterSettings* obj)
{
    return MarshalExceptions<Duplex>([&](){
            return obj->GetDuplex();
        });
}

ALTERNET_UI_API void PrinterSettings_SetDuplex_(PrinterSettings* obj, Duplex value)
{
    MarshalExceptions<void>([&](){
            obj->SetDuplex(value);
        });
}

ALTERNET_UI_API int PrinterSettings_GetFromPage_(PrinterSettings* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetFromPage();
        });
}

ALTERNET_UI_API void PrinterSettings_SetFromPage_(PrinterSettings* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetFromPage(value);
        });
}

ALTERNET_UI_API int PrinterSettings_GetToPage_(PrinterSettings* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetToPage();
        });
}

ALTERNET_UI_API void PrinterSettings_SetToPage_(PrinterSettings* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetToPage(value);
        });
}

ALTERNET_UI_API int PrinterSettings_GetMinimumPage_(PrinterSettings* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetMinimumPage();
        });
}

ALTERNET_UI_API void PrinterSettings_SetMinimumPage_(PrinterSettings* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetMinimumPage(value);
        });
}

ALTERNET_UI_API int PrinterSettings_GetMaximumPage_(PrinterSettings* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetMaximumPage();
        });
}

ALTERNET_UI_API void PrinterSettings_SetMaximumPage_(PrinterSettings* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetMaximumPage(value);
        });
}

ALTERNET_UI_API PrintRange PrinterSettings_GetPrintRange_(PrinterSettings* obj)
{
    return MarshalExceptions<PrintRange>([&](){
            return obj->GetPrintRange();
        });
}

ALTERNET_UI_API void PrinterSettings_SetPrintRange_(PrinterSettings* obj, PrintRange value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrintRange(value);
        });
}

ALTERNET_UI_API c_bool PrinterSettings_GetCollate_(PrinterSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetCollate();
        });
}

ALTERNET_UI_API void PrinterSettings_SetCollate_(PrinterSettings* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetCollate(value);
        });
}

ALTERNET_UI_API int PrinterSettings_GetCopies_(PrinterSettings* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetCopies();
        });
}

ALTERNET_UI_API void PrinterSettings_SetCopies_(PrinterSettings* obj, int value)
{
    MarshalExceptions<void>([&](){
            obj->SetCopies(value);
        });
}

ALTERNET_UI_API PageSettings* PrinterSettings_GetDefaultPageSettings_(PrinterSettings* obj)
{
    return MarshalExceptions<PageSettings*>([&](){
            return obj->GetDefaultPageSettings();
        });
}

ALTERNET_UI_API void PrinterSettings_SetDefaultPageSettings_(PrinterSettings* obj, PageSettings* value)
{
    MarshalExceptions<void>([&](){
            obj->SetDefaultPageSettings(value);
        });
}

ALTERNET_UI_API c_bool PrinterSettings_GetPrintToFile_(PrinterSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetPrintToFile();
        });
}

ALTERNET_UI_API void PrinterSettings_SetPrintToFile_(PrinterSettings* obj, c_bool value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrintToFile(value);
        });
}

ALTERNET_UI_API char16_t* PrinterSettings_GetPrinterName_(PrinterSettings* obj)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(obj->GetPrinterName());
        });
}

ALTERNET_UI_API void PrinterSettings_SetPrinterName_(PrinterSettings* obj, const char16_t* value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrinterName(ToOptional(value));
        });
}

ALTERNET_UI_API c_bool PrinterSettings_GetIsValid_(PrinterSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsValid();
        });
}

ALTERNET_UI_API c_bool PrinterSettings_GetIsDefaultPrinter_(PrinterSettings* obj)
{
    return MarshalExceptions<c_bool>([&](){
            return obj->GetIsDefaultPrinter();
        });
}

ALTERNET_UI_API char16_t* PrinterSettings_GetPrintFileName_(PrinterSettings* obj)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(obj->GetPrintFileName());
        });
}

ALTERNET_UI_API void PrinterSettings_SetPrintFileName_(PrinterSettings* obj, const char16_t* value)
{
    MarshalExceptions<void>([&](){
            obj->SetPrintFileName(ToOptional(value));
        });
}

