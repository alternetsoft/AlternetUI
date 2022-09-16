#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class UnmanagedStream : public Object
    {
#include "Api/UnmanagedStream.inc"
    public:
        UnmanagedStream(wxMemoryOutputStream* wxMemoryOutputStream);

    private:

        wxStreamBuffer* _buffer = nullptr;
        wxMemoryOutputStream* _wxMemoryOutputStream = nullptr;
        wxMemoryInputStream* _wxMemoryInputStream = nullptr;
    };
}
