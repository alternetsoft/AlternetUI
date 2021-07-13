#include "Image.h"

namespace Alternet::UI
{
    Image::Image()
    {
    }

    Image::~Image()
    {
    }

    int Image::GetWidth()
    {
        wxASSERT(_image);
        return _image->GetWidth();
    }

    void Image::LoadFromStream(void* stream)
    {
        wxASSERT(_image == nullptr);
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);
        _image = new wxImage(managedInputStream);
    }
}
