#include "Image.h"

namespace Alternet::UI
{
    Image::Image()
    {
    }

    Image::~Image()
    {
    }

    void Image::LoadFromStream(void* stream)
    {
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);

        static bool imageHandlersInitialized = false;
        if (!imageHandlersInitialized)
        {
            wxInitAllImageHandlers();
            imageHandlersInitialized = true;
        }

        _bitmap = wxBitmap(managedInputStream);
    }

    void Image::Initialize(const Size& size)
    {
        _bitmap = wxBitmap(fromDip(size, nullptr));
    }

    wxBitmap Image::GetBitmap()
    {
        return _bitmap;
    }

    void Image::SetBitmap(const wxBitmap& value)
    {
        _bitmap = value;
    }
    
    Size Image::GetSize()
    {
        return toDip(_bitmap.GetSize(), nullptr);
    }
    
    Int32Size Image::GetPixelSize()
    {
        return _bitmap.GetSize();
    }
}