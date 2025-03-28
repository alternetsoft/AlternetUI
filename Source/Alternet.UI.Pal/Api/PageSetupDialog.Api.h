// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "PageSetupDialog.h"
#include "PrintDocument.h"
#include "Window.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API PageSetupDialog* PageSetupDialog_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PageSetupDialog*>([&](){
    #endif
        return new PageSetupDialog();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API PrintDocument* PageSetupDialog_GetDocument_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<PrintDocument*>([&](){
    #endif
        return obj->GetDocument();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetDocument_(PageSetupDialog* obj, PrintDocument* value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetDocument(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API Thickness_C PageSetupDialog_GetMinMargins_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Thickness_C>([&](){
    #endif
        return obj->GetMinMargins();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetMinMargins_(PageSetupDialog* obj, Thickness value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetMinMargins(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PageSetupDialog_GetMinMarginsValueSet_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetMinMarginsValueSet();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetMinMarginsValueSet_(PageSetupDialog* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetMinMarginsValueSet(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PageSetupDialog_GetAllowMargins_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetAllowMargins();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetAllowMargins_(PageSetupDialog* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetAllowMargins(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PageSetupDialog_GetAllowOrientation_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetAllowOrientation();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetAllowOrientation_(PageSetupDialog* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetAllowOrientation(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PageSetupDialog_GetAllowPaper_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetAllowPaper();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetAllowPaper_(PageSetupDialog* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetAllowPaper(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool PageSetupDialog_GetAllowPrinter_(PageSetupDialog* obj)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return obj->GetAllowPrinter();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void PageSetupDialog_SetAllowPrinter_(PageSetupDialog* obj, c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        obj->SetAllowPrinter(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API ModalResult PageSetupDialog_ShowModal_(PageSetupDialog* obj, Window* owner)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<ModalResult>([&](){
    #endif
        return obj->ShowModal(owner);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

