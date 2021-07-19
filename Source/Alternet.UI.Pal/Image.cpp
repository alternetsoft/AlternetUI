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
        wxInitAllImageHandlers(); // or wxImage::AddHandler(new wxPNGHandler);
        _image = wxImage(managedInputStream);
    }

    wxImage Image::GetImage()
    {
        return _image;
    }
    
    SizeF Image::GetSize()
    {
        return toDip(_image.GetSize(), nullptr);
    }
    
    Size Image::GetPixelSize()
    {
        return _image.GetSize();
    }
}