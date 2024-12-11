// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2024 AlterNET Software.</auto-generated>

#pragma once

#include "Keyboard.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API Keyboard* Keyboard_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Keyboard*>([&](){
    #endif
        return new Keyboard();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API char16_t Keyboard_GetInputChar_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<char16_t>([&](){
    #endif
        return Keyboard::GetInputChar();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API uint8_t Keyboard_GetInputEventCode_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<uint8_t>([&](){
    #endif
        return Keyboard::GetInputEventCode();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool Keyboard_GetInputHandled_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return Keyboard::GetInputHandled();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void Keyboard_SetInputHandled_(c_bool value)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        Keyboard::SetInputHandled(value);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API Key Keyboard_GetInputKey_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<Key>([&](){
    #endif
        return Keyboard::GetInputKey();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API c_bool Keyboard_GetInputIsRepeat_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<c_bool>([&](){
    #endif
        return Keyboard::GetInputIsRepeat();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API KeyStates Keyboard_GetKeyState_(Key key)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<KeyStates>([&](){
    #endif
        return Keyboard::GetKeyState(key);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void Keyboard_SetEventCallback_(Keyboard::KeyboardEventCallbackType callback)
{
    Keyboard::SetEventCallback(callback);
}

