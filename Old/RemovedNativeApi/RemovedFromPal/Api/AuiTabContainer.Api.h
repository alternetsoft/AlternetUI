// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "AuiTabContainer.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API AuiTabContainer* AuiTabContainer_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<AuiTabContainer*>([&](){
    #endif
        return new AuiTabContainer();
    #if !defined(__WXMSW__)
    });
    #endif
}
