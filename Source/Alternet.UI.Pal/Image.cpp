#include "Image.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"
#include "wx/wxprec.h"
#include "wx/rawbmp.h"

namespace Alternet::UI
{
    void wxImageAddHandlerV2(wxImageHandler* handler)
    {
        if (wxImage::FindHandler(handler->GetType()) == 0)
            wxImage::AddHandler(handler);
    }

    void Image::wxInitAllImageHandlersV2()
    {
#if wxUSE_LIBPNG
        wxImageAddHandlerV2(new wxPNGHandler);
#endif
#if wxUSE_LIBJPEG
        wxImageAddHandlerV2(new wxJPEGHandler);
#endif
#if wxUSE_LIBTIFF
        wxImageAddHandlerV2(new wxTIFFHandler);
#endif
#if wxUSE_GIF
        wxImageAddHandlerV2(new wxGIFHandler);
#endif
#if wxUSE_PNM
        wxImageAddHandlerV2(new wxPNMHandler);
#endif
#if wxUSE_PCX
        wxImageAddHandlerV2(new wxPCXHandler);
#endif
#if wxUSE_IFF
        wxImageAddHandlerV2(new wxIFFHandler);
#endif
#if wxUSE_ICO_CUR
        wxImageAddHandlerV2(new wxICOHandler);
        wxImageAddHandlerV2(new wxCURHandler);
        wxImageAddHandlerV2(new wxANIHandler);
#endif
#if wxUSE_TGA
        wxImageAddHandlerV2(new wxTGAHandler);
#endif
#if wxUSE_XPM
        wxImageAddHandlerV2(new wxXPMHandler);
#endif
    }

    Image::Image()
    {
        EnsureImageHandlersInitialized();
    }

    Image::~Image()
    {
    }

    void Image::LoadSvgFromStream(void* stream, int width, int height)
    {
        InputStream inputStream(stream);
        ManagedInputStream managedInputStream(&inputStream);

        const size_t len = static_cast<size_t>(managedInputStream.GetLength());
        wxCharBuffer buf(len);
        char* const ptr = buf.data();

        if (managedInputStream.ReadAll(ptr, len))
        {
            auto size = wxSize(width, height);
            auto bitmapBundle = wxBitmapBundle::FromSVG(ptr, size);
            _bitmap = bitmapBundle.GetBitmap(size);
        }
        else
            _bitmap = wxBitmap();
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
        auto image = _bitmap.ConvertToImage();
        image.SaveFile(wxStr(fileName));
    }

    void Image::Initialize(const Size& size)
    {
        if (size.Width == 0 || size.Height == 0)
        {
            _bitmap = wxBitmap();
        }
        else
        {
            _bitmap = wxBitmap(fromDip(size, nullptr), 32);
        }
    }

    /*static*/ void Image::EnsureImageHandlersInitialized()
    {
        static bool imageHandlersInitialized = false;
        if (!imageHandlersInitialized)
        {
            wxInitAllImageHandlersV2();
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
        if (!otherImage->_bitmap.IsOk())
            _bitmap = wxBitmap();
        else
        {
            auto otherBitmap = otherImage->GetBitmap();
            _bitmap = otherImage->GetBitmap().GetSubBitmap(wxRect(0, 0, otherBitmap.GetWidth(), otherBitmap.GetHeight()));
        }
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
        if (!_bitmap.IsOk())
            return Size();

        return toDip(_bitmap.GetSize(), nullptr);
    }
    
    Int32Size Image::GetPixelSize()
    {
        if (!_bitmap.IsOk())
            return Int32Size(0, 0);

        return _bitmap.GetSize();
    }

    bool Image::GrayScale()
    {
        Int32Size size = GetPixelSize();
        if (size.Width == 0 || size.Height == 0)
            return false;

        //wxBitmap bmp(width, height, 32); // explicit depth important under MSW
        wxAlphaPixelData data(_bitmap);
        if (!data)
        {
            // ... raw access to bitmap data unavailable, do something else ...
            return false;
        }

        /*if (data.GetWidth() < 20 || data.GetHeight() < 20)
        {
            // ... complain: the bitmap it too small ...
            return;
        }*/

        wxAlphaPixelData::Iterator p(data);

        // we draw a (10, 10)-(20, 20) rect manually using the given r, g, b
        //p.Offset(data, 10, 10);

        for (int y = 0; y < size.Height; ++y)
        {
            wxAlphaPixelData::Iterator rowStart = p;

            for (int x = 0; x < size.Width; ++x, ++p)
            {
                p.Red() = 150;
                p.Green() = 150;
                p.Blue() = 150;
            }

            p = rowStart;
            p.OffsetY(data, 1);
        }
        return true;
    }

    /*
    https://docs.wxwidgets.org/trunk/classwx_pixel_data.html
    https://stackoverflow.com/questions/2265910/convert-an-image-to-grayscale

    Variant 1:
    ==========
    for (x = 0; x < c.Width; x++)
             {
                 for (y = 0; y < c.Height; y++)
                 {
                     Color pixelColor = c.GetPixel(x, y);
                     Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                     c.SetPixel(x, y, newColor); // Now greyscale
                 }
             }
    Variant 2:
    ==========
    for (int i = 0; i < c.Width; i++)
    {
        for (int x = 0; x < c.Height; x++)
        {
            Color oc = c.GetPixel(i, x);
            int grayScale = (int)((oc.R * 0.3) + (oc.G * 0.59) + (oc.B * 0.11));
            Color nc = Color.FromArgb(oc.A, grayScale, grayScale, grayScale);
            d.SetPixel(i, x, nc);
        }
    }

    Variant 3:
    ==========
    for (int y = 0; y < bt.Height; y++)
    {
        for (int x = 0; x < bt.Width; x++)
        {
            Color c = bt.GetPixel(x, y);

            int r = c.R;
            int g = c.G;
            int b = c.B;
            int avg = (r + g + b) / 3;
            bt.SetPixel(x, y, Color.FromArgb(avg,avg,avg));
        }
    }

    Variant 4:
    ==========
    /// <summary>
    /// Change the RGB color to the Grayscale version
    /// </summary>
    /// <param name="color">The source color</param>
    /// <param name="volume">Gray scale volume between -255 - 255</param>
    /// <returns></returns>
    public virtual Color Grayscale(Color color, short volume = 0)
    {
        if (volume == 0) return color;
        var r = color.R;
        var g = color.G;
        var b = color.B;
        var mean = (r + g + b) / 3F;
        var n = volume / 255F;
        var o = 1 - n;
        return Color.FromArgb(color.A, Convert.ToInt32(r * o + mean * n), Convert.ToInt32(g * o + mean * n), Convert.ToInt32(b * o + mean * n));
    }

    public virtual Image Grayscale(Image source, short volume = 0)
    {
        if (volume == 0) return source;
        Bitmap bmp = new Bitmap(source);
        for (int x = 0; x < bmp.Width; x++)
            for (int y = 0; y < bmp.Height; y++)
            {
                Color c = bmp.GetPixel(x, y);
                if (c.A > 0)
                    bmp.SetPixel(x, y, Grayscale(c,volume));
            }
        return bmp;
    }

    Variant 5
    =========
    public static Bitmap MakeGrayscale2(Bitmap original)
    {
       unsafe
       {
          //create an empty bitmap the same size as original
          Bitmap newBitmap = new Bitmap(original.Width, original.Height);

          //lock the original bitmap in memory
          BitmapData originalData = original.LockBits(
             new Rectangle(0, 0, original.Width, original.Height),
             ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

          //lock the new bitmap in memory
          BitmapData newData = newBitmap.LockBits(
             new Rectangle(0, 0, original.Width, original.Height),
             ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

          //set the number of bytes per pixel
          int pixelSize = 3;

          for (int y = 0; y < original.Height; y++)
          {
             //get the data from the original image
             byte* oRow = (byte*)originalData.Scan0 + (y * originalData.Stride);

             //get the data from the new image
             byte* nRow = (byte*)newData.Scan0 + (y * newData.Stride);

             for (int x = 0; x < original.Width; x++)
             {
                //create the grayscale version
                byte grayScale =
                   (byte)((oRow[x * pixelSize] * .11) + //B
                   (oRow[x * pixelSize + 1] * .59) +  //G
                   (oRow[x * pixelSize + 2] * .3)); //R

                //set the new image's pixel to the grayscale version
                nRow[x * pixelSize] = grayScale; //B
                nRow[x * pixelSize + 1] = grayScale; //G
                nRow[x * pixelSize + 2] = grayScale; //R
             }
          }

          //unlock the bitmaps
          newBitmap.UnlockBits(newData);
          original.UnlockBits(originalData);

          return newBitmap;
       }
    }

    */
}