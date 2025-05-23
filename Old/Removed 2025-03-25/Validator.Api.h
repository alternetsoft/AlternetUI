// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "Validator.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Validator* Validator_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Validator*>([&](){
    #endif
        return new Validator();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void Validator_SuppressBellOnError_(c_bool suppress)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        Validator::SuppressBellOnError(suppress);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool Validator_IsSilent_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return Validator::IsSilent();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

