#include "Image.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"

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

        EnsureImageHandlersInitialized();
        _bitmap = wxBitmap(managedInputStream);
    }

    void Image::Initialize(const Size& size)
    {
        EnsureImageHandlersInitialized();
        _bitmap = wxBitmap(fromDip(size, nullptr));
    }

    /*static */void Image::EnsureImageHandlersInitialized()
    {
        static bool imageHandlersInitialized = false;
        if (!imageHandlersInitialized)
        {
            wxInitAllImageHandlers();
            imageHandlersInitialized = true;
        }
    }

    void Image::CopyFrom(Image* otherImage)
    {
        auto otherBitmap = otherImage->GetBitmap();
        _bitmap = otherImage->GetBitmap().GetSubBitmap(wxRect(0, 0, otherBitmap.GetWidth(), otherBitmap.GetHeight()));
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