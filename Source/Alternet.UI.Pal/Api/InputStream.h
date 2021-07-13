// Auto generated code, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2021.
#pragma once
#include "../Common.h"
#include "../ApiTypes.h"

namespace Alternet::UI
{
    class InputStream
    {
        public:
            
            int64_t GetLength()
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TGetLength)trampolineLocatorCallback(Trampoline::GetLength);
                return trampoline(objectHandle);
            }
            typedef int64_t (*TGetLength)(void* objectHandle);
            
            
            bool GetIsOK()
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TGetIsOK)trampolineLocatorCallback(Trampoline::GetIsOK);
                return trampoline(objectHandle);
            }
            typedef bool (*TGetIsOK)(void* objectHandle);
            
            
            bool GetIsSeekable()
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TGetIsSeekable)trampolineLocatorCallback(Trampoline::GetIsSeekable);
                return trampoline(objectHandle);
            }
            typedef bool (*TGetIsSeekable)(void* objectHandle);
            
            
            int64_t GetPosition()
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TGetPosition)trampolineLocatorCallback(Trampoline::GetPosition);
                return trampoline(objectHandle);
            }
            typedef int64_t (*TGetPosition)(void* objectHandle);
            void SetPosition(int64_t value)
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TSetPosition)trampolineLocatorCallback(Trampoline::SetPosition);
                trampoline(objectHandle, value);
            }
            typedef void (*TSetPosition)(void* objectHandle, int64_t value);
            
            void* Read(void* buffer, void* length)
            {
                wxASSERT(trampolineLocatorCallback);
                auto trampoline = (TRead)trampolineLocatorCallback(Trampoline::Read);
                return trampoline(objectHandle, buffer, length);
            }
            typedef void* (*TRead)(void* objectHandle, void* buffer, void* length);
            public:
            enum class Trampoline
            {
                Read,
                GetLength,
                GetIsOK,
                GetIsSeekable,
                GetPosition,
                SetPosition,
            };
            typedef void* (*TTrampolineLocatorCallback)(Trampoline trampoline);
            static void SetTrampolineLocatorCallback(TTrampolineLocatorCallback value) { trampolineLocatorCallback = value; }
            InputStream(void* objectHandle_) : objectHandle(objectHandle_) {}
        private:
            void* objectHandle;
            inline static TTrampolineLocatorCallback trampolineLocatorCallback = nullptr;
    }
    ;
}
