// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "AuiManagerEvent.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API AuiManagerEvent* AuiManagerEvent_Create_()
{
    #if !defined(__WXMSW__)
    return MarshalExceptions<AuiManagerEvent*>([&](){
    #endif
        return new AuiManagerEvent();
    #if !defined(__WXMSW__)
    });
    #endif
}

