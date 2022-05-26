// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>

#pragma once

#include "Menu.h"
#include "MenuItem.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Menu* Menu_Create_()
{
    return MarshalExceptions<Menu*>([&](){
            return new Menu();
        });
}

ALTERNET_UI_API int Menu_GetItemsCount_(Menu* obj)
{
    return MarshalExceptions<int>([&](){
            return obj->GetItemsCount();
        });
}

ALTERNET_UI_API void Menu_InsertItemAt_(Menu* obj, int index, MenuItem* item)
{
    MarshalExceptions<void>([&](){
            obj->InsertItemAt(index, item);
        });
}

ALTERNET_UI_API void Menu_RemoveItemAt_(Menu* obj, int index)
{
    MarshalExceptions<void>([&](){
            obj->RemoveItemAt(index);
        });
}

