#include "Image.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"

namespace Alternet::UI
{
    Image::Image()
    {
        EnsureImageHandlersInitialized();
    }

    Image::~Image()
    {
    }

    void Image::LoadFromStream(void* stream)
    {
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);

        _bitmap = wxBitmap(managedInputStream);
    }

    void Image::SaveToStream(void* stream, const string& format)
    {
        OutputStream outputStream(stream);
        ManagedOutputStream managedOutputStream(&outputStream);

        _bitmap.ConvertToImage().SaveFile(managedOutputStream, GetBitmapTypeFromFormat(format));
    }

    void Image::SaveToFile(const string& fileName)
    {
        _bitmap.ConvertToImage().SaveFile(wxStr(fileName));
    }

    void Image::Initialize(const Size& size)
    {
        _bitmap = wxBitmap(fromDip(size, nullptr));
    }

    /*static*/ void Image::EnsureImageHandlersInitialized()
    {
        static bool imageHandlersInitialized = false;
        if (!imageHandlersInitialized)
        {
            wxInitAllImageHandlers();
            imageHandlersInitialized = true;
        }
    }

    /*static*/ wxBitmapType Image::GetBitmapTypeFromFormat(const string& format)
    {
        if (format == u"Bmp")
            return wxBITMAP_TYPE_BMP;
        if (format == u"Png")
            return wxBITMAP_TYPE_PNG;
        if (format == u"Jpeg")
            return wxBITMAP_TYPE_JPEG;
        if (format == u"Png")
            return wxBITMAP_TYPE_PNG;
        if (format == u"Tiff")
            return wxBITMAP_TYPE_TIFF;

        throwEx(u"Image format is not supported: " + format);
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