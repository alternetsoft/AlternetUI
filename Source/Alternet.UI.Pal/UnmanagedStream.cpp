#include "UnmanagedStream.h"

namespace Alternet::UI
{
    UnmanagedStream::UnmanagedStream()
    {
    }

    UnmanagedStream::~UnmanagedStream()
    {
    }

    int64_t UnmanagedStream::GetLength()
    {
        return int64_t();
    }

    bool UnmanagedStream::GetIsOK()
    {
        return false;
    }

    bool UnmanagedStream::GetIsSeekable()
    {
        return false;
    }

    int64_t UnmanagedStream::GetPosition()
    {
        return int64_t();
    }

    void UnmanagedStream::SetPosition(int64_t value)
    {
    }

    void* UnmanagedStream::Read(void* buffer, int bufferCount, void* length)
    {
        return nullptr;
    }
}
