// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "Clipboard.h"
#include "UnmanagedDataObject.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Clipboard* Clipboard_Create_()
{
    return MarshalExceptions<Clipboard*>([&](){
            return new Clipboard();
        });
}

ALTERNET_UI_API UnmanagedDataObject* Clipboard_GetDataObject_(Clipboard* obj)
{
    return MarshalExceptions<UnmanagedDataObject*>([&](){
            return obj->GetDataObject();
        });
}

ALTERNET_UI_API void Clipboard_SetDataObject_(Clipboard* obj, UnmanagedDataObject* value)
{
    MarshalExceptions<void>([&](){
            obj->SetDataObject(value);
        });
}

