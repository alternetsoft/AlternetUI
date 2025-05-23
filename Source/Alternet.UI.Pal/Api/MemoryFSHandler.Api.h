// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>

#pragma once

#include "MemoryFSHandler.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API MemoryFSHandler* MemoryFSHandler_Create_()
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    return MarshalExceptions<MemoryFSHandler*>([&](){
    #endif
        return new MemoryFSHandler();
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void MemoryFSHandler_RemoveFile_(const char16_t* filename)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        MemoryFSHandler::RemoveFile(filename);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void MemoryFSHandler_AddTextFileWithMimeType_(const char16_t* filename, const char16_t* textdata, const char16_t* mimetype)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        MemoryFSHandler::AddTextFileWithMimeType(filename, textdata, mimetype);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void MemoryFSHandler_AddTextFile_(const char16_t* filename, const char16_t* textdata)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        MemoryFSHandler::AddTextFile(filename, textdata);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void MemoryFSHandler_AddFile_(const char16_t* filename, void* binarydata, int size)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        MemoryFSHandler::AddFile(filename, binarydata, size);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

ALTERNET_UI_API void MemoryFSHandler_AddFileWithMimeType_(const char16_t* filename, void* binarydata, int size, const char16_t* mimetype)
{
    #if !defined(__WXMSW__) || defined(_DEBUG)
    MarshalExceptions<void>([&](){
    #endif
        MemoryFSHandler::AddFileWithMimeType(filename, binarydata, size, mimetype);
    #if !defined(__WXMSW__) || defined(_DEBUG)
    });
    #endif
}

