// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2022.</auto-generated>
#pragma once
#include "ApiUtils.h"

namespace Alternet::UI
{
    #pragma pack(push, 1)
    struct DragEventData
    {
        void* data;
        double mouseClientLocationX;
        double mouseClientLocationY;
        DragDropEffects effect;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct QueryContinueDragEventData
    {
        c_bool escapePressed;
        DragAction action;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct TextInputEventData
    {
        char16_t keyChar;
        int64_t timestamp;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct KeyEventData
    {
        Key key;
        int64_t timestamp;
        c_bool isRepeat;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct MouseButtonEventData
    {
        int64_t timestamp;
        void* targetControl;
        MouseButton changedButton;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct MouseEventData
    {
        int64_t timestamp;
        void* targetControl;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct MouseWheelEventData
    {
        int64_t timestamp;
        void* targetControl;
        int delta;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct TreeViewItemEventData
    {
        void* item;
    };
    #pragma pack(pop)
    
    #pragma pack(push, 1)
    struct CommandEventData
    {
        char16_t* managedCommandId;
    };
    #pragma pack(pop)
    
}