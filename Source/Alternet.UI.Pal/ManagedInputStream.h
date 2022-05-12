#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Api/InputStream.h"

namespace Alternet::UI
{
    class ManagedInputStream : public wxInputStream
    {
    public:
        ManagedInputStream(InputStream* inputStream) : _inputStream(inputStream) {}

        virtual wxFileOffset GetLength() const override { return _inputStream->GetLength(); }

        virtual bool IsOk() const override { return _inputStream->GetIsOK(); }

        virtual bool IsSeekable() const override { return _inputStream->GetIsSeekable(); }
    protected:

        virtual size_t OnSysRead(void* buffer, size_t size) override
        {
            return (size_t)_inputStream->Read(buffer, (void*)size);
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
                actualPosition = TellI() + pos;
                break;
            case wxFromEnd:
                actualPosition = GetLength() - 1 - pos;
                break;
            default:
                throwExInvalidOp;
            }
                
            _inputStream->SetPosition(actualPosition);
            return actualPosition;
        }

        virtual wxFileOffset OnSysTell() const override { return _inputStream->GetPosition(); }

    private:
        InputStream* _inputStream;
    };
}
