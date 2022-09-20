#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Api/OutputStream.h"

namespace Alternet::UI
{
    class ManagedOutputStream : public wxOutputStream
    {
    public:
        ManagedOutputStream(OutputStream* inputStream) : _outputStream(inputStream) {}

        virtual wxFileOffset GetLength() const override { return _outputStream->GetLength(); }

        virtual bool IsOk() const override { return _outputStream->GetIsOK(); }

        virtual bool IsSeekable() const override { return _outputStream->GetIsSeekable(); }
    protected:

        virtual size_t OnSysWrite(const void* buffer, size_t bufsize) override
        {
            return (size_t)_outputStream->Write((void*)buffer, (void*)bufsize);
        }

        virtual wxFileOffset OnSysSeek(wxFileOffset pos, wxSeekMode mode) override
        {
            wxFileOffset actualPosition = 0;
            switch (mode)
            {
            case wxFromStart:
                actualPosition = pos;
                break;
            case wxFromCurrent:
                actualPosition = TellO() + pos;
                break;
            case wxFromEnd:
                actualPosition = GetLength() - 1 - pos;
                break;
            default:
                throwExInvalidOp;
            }
                
            _outputStream->SetPosition(actualPosition);
            return actualPosition;
        }

        virtual wxFileOffset OnSysTell() const override { return _outputStream->GetPosition(); }

    private:
        OutputStream* _outputStream;
    };
}
