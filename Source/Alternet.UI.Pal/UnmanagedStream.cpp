#include "UnmanagedStream.h"

namespace Alternet::UI
{
    UnmanagedStream::UnmanagedStream()
    {
        throwExNoInfo;
    }

    UnmanagedStream::UnmanagedStream(wxMemoryOutputStream* wxMemoryOutputStream) :
        _wxMemoryOutputStream(wxMemoryOutputStream),
        _buffer(wxMemoryOutputStream->GetOutputStreamBuffer()),
        _wxMemoryInputStream(new wxMemoryInputStream(*wxMemoryOutputStream))
    {
        _wxMemoryInputStream->SeekI(0);
    }

    UnmanagedStream::~UnmanagedStream()
    {
        if (_wxMemoryOutputStream != nullptr)
        {
            delete _wxMemoryOutputStream;
            _wxMemoryOutputStream = nullptr;
        }

        if (_wxMemoryInputStream != nullptr)
        {
            delete _wxMemoryInputStream;
            _wxMemoryInputStream = nullptr;
        }

        _buffer = nullptr; // deleted by wxMemoryOutputStream.
    }

    int64_t UnmanagedStream::GetLength()
    {
        return _wxMemoryInputStream->GetSize();
    }

    bool UnmanagedStream::GetIsOK()
    {
        return _wxMemoryInputStream->IsOk();
    }

    bool UnmanagedStream::GetIsSeekable()
    {
        return _wxMemoryInputStream->IsSeekable();
    }

    int64_t UnmanagedStream::GetPosition()
    {
        return _wxMemoryInputStream->TellI();
    }

    void UnmanagedStream::SetPosition(int64_t value)
    {
        _wxMemoryInputStream->SeekI(value);
    }

    void* UnmanagedStream::Read(void* buffer, int bufferCount, void* length)
    {
        if (!_wxMemoryInputStream->CanRead())
            return 0;

        _wxMemoryInputStream->Read(buffer, (size_t)length);

        return (void*)_wxMemoryInputStream->LastRead();
    }
}
