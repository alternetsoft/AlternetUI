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
        wxASSERT(_image == nullptr);
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);
        wxImage::AddHandler(new wxPNGHandler); // or wxInitAllImageHandlers()
        _image = new wxImage(managedInputStream);
    }

    wxImage* Image::GetImage()
    {
        wxASSERT(_image);
        return _image;
    }
    
    SizeF Image::GetSize()
    {
        wxASSERT(_image);
        return toDip(_image->GetSize(), nullptr);
    }
    
    Size Image::GetPixelSize()
    {
        wxASSERT(_image);
        return _image->GetSize();
    }
}