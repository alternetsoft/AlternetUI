#include "Image.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"
#include "GenericImage.h"

#include <wx/wxprec.h>
#include <wx/rawbmp.h>

#include "../../External/WxWidgets/3rdparty/nanosvg/src/nanosvg.h"
#include "../../External/WxWidgets/3rdparty/nanosvg/src/nanosvgrast.h"

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

	class wxBitmapBundleImplSVG : public wxBitmapBundleImpl
	{
	public:
		// Ctor must be passed a valid NSVGimage and takes ownership of it.
		wxBitmapBundleImplSVG(NSVGimage* svgImage, const wxSize& sizeDef, const Color& color)
			: m_svgImage(svgImage),
			m_svgRasterizer(nsvgCreateRasterizer()),
			m_sizeDef(sizeDef),
			m_color(color)
		{
		}

		~wxBitmapBundleImplSVG()
		{
			nsvgDeleteRasterizer(m_svgRasterizer);
			nsvgDelete(m_svgImage);
		}

		virtual wxSize GetDefaultSize() const wxOVERRIDE;
		virtual wxSize GetPreferredBitmapSizeAtScale(double scale) const wxOVERRIDE;
		virtual wxBitmap GetBitmap(const wxSize& size) wxOVERRIDE;

	private:
		wxBitmap DoRasterize(const wxSize& size);

		Color m_color;
		NSVGimage* const m_svgImage;
		NSVGrasterizer* const m_svgRasterizer;

		const wxSize m_sizeDef;

		// Cache the last used bitmap (may be invalid if not used yet).
		//
		// Note that we cache only the last bitmap and not all the bitmaps ever
		// requested from GetBitmap() for the different sizes because there would
		// be no way to clear such cache and its growth could be unbounded,
		// resulting in too many bitmap objects being used in an application using
		// SVG for all of its icons.
		wxBitmap m_cachedBitmap;

		wxDECLARE_NO_COPY_CLASS(wxBitmapBundleImplSVG);
	};

	// ============================================================================
	// wxBitmapBundleImplSVG implementation
	// ============================================================================

	wxSize wxBitmapBundleImplSVG::GetDefaultSize() const
	{
		return m_sizeDef;
	}

	wxSize wxBitmapBundleImplSVG::GetPreferredBitmapSizeAtScale(double scale) const
	{
		// We consider that we can render at any scale.
		return m_sizeDef * scale;
	}

	wxBitmap wxBitmapBundleImplSVG::GetBitmap(const wxSize& size)
	{
		if (!m_cachedBitmap.IsOk() || m_cachedBitmap.GetSize() != size)
		{
			m_cachedBitmap = DoRasterize(size);
		}

		return m_cachedBitmap;
	}

	#define NSVG_RGB(r, g, b) (((unsigned int)r) | ((unsigned int)g << 8) | ((unsigned int)b << 16))

	#define NSVG_ARGB(a, r, g, b) (((unsigned int)r) | ((unsigned int)g << 8) | ((unsigned int)b << 16) | ((unsigned int)a << 24))

	wxBitmap wxBitmapBundleImplSVG::DoRasterize(const wxSize& size)
	{
		wxVector<unsigned char> buffer(size.x * size.y * 4);

		if (!m_color.IsEmpty() && !m_color.IsBlack())
		{
			for (NSVGshape* shape = m_svgImage->shapes; shape != NULL; shape = shape->next)
			{
				shape->fill.type = NSVG_PAINT_COLOR;
				shape->fill.color = NSVG_ARGB(m_color.A, m_color.R, m_color.G, m_color.B);
			}
		}

		nsvgRasterize
		(
			m_svgRasterizer,
			m_svgImage,
			0.0, 0.0,           // no offset
			wxMin
			(
				size.x / m_svgImage->width,
				size.y / m_svgImage->height
			),                  // scale
			&buffer[0],
			size.x, size.y,
			size.x * 4            // stride -- we have no gaps between lines
		);

		wxBitmap bitmap(size, 32);
		wxAlphaPixelData bmpdata(bitmap);
		wxAlphaPixelData::Iterator dst(bmpdata);

		const unsigned char* src = &buffer[0];
		for (int y = 0; y < size.y; ++y)
		{
			dst.MoveTo(bmpdata, 0, y);
			for (int x = 0; x < size.x; ++x)
			{
				const unsigned char a = src[3];
				dst.Red() = src[0] * a / 255;
				dst.Green() = src[1] * a / 255;
				dst.Blue() = src[2] * a / 255;
				dst.Alpha() = a;

				++dst;
				src += 4;
			}
		}

		return bitmap;
	}

	wxBitmapBundle wxBitmapBundleFromSVG(char* data, const wxSize& sizeDef, const Color& color)
	{
		NSVGimage* const svgImage = nsvgParse(data, "px", 96);
		if (!svgImage)
			return wxBitmapBundle();

		// Somewhat unexpectedly, a non-null but empty image is returned even if
		// the data is not SVG at all, e.g. without this check creating a bundle
		// from any random file with FromSVGFile() would "work".
		if (svgImage->width == 0 && svgImage->height == 0 && !svgImage->shapes)
		{
			nsvgDelete(svgImage);
			return wxBitmapBundle();
		}

		return wxBitmapBundle::FromImpl(new wxBitmapBundleImplSVG(svgImage, sizeDef, color));
	}

//================================================

	wxBitmapBundle Image::CreateFromSvgStream(void* stream, int width, int height, const Color& color)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		managedInputStream.SeekI(0);

		const size_t len = static_cast<size_t>(managedInputStream.GetLength());
		wxCharBuffer buf(len);
		char* const ptr = buf.data();

		if (managedInputStream.ReadAll(ptr, len))
		{
			auto size = wxSize(width, height);
			auto bitmapBundle = wxBitmapBundleFromSVG(ptr, size, color);
			return bitmapBundle;
		}
		else
			return wxBitmapBundle();
	}

	void Image::LoadSvgFromStream(void* stream, int width, int height, const Color& color)
	{
		auto bundle = CreateFromSvgStream(stream, width, height, color);

		if (bundle.IsOk())
		{
			auto size = wxSize(width, height);
			_bitmap = bundle.GetBitmap(size);
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

	void* Image::ConvertToGenericImage()
	{
		return new GenericImage(_bitmap.ConvertToImage());
	}

	void Image::LoadFromGenericImage(void* image, int depth)
	{
		_bitmap = wxBitmap(((GenericImage*)image)->_image, depth);
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

	bool Image::GetIsOk()
	{
		return _bitmap.IsOk();
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

	void Image::Rescale(const Int32Size& sizeNeeded)
	{
		wxBitmap::Rescale(_bitmap, sizeNeeded);
	}

	int Image::GetDefaultBitmapType()
	{
		return wxBITMAP_DEFAULT_TYPE;
	}

	bool Image::GetHasAlpha()
	{
		return _bitmap.HasAlpha();
	}

	void Image::SetHasAlpha(bool value)
	{
		_bitmap.UseAlpha(value);
	}

	int Image::GetPixelWidth()
	{
		return _bitmap.GetWidth();
	}
	int Image::GetPixelHeight()
	{
		return _bitmap.GetHeight();
	}

	int Image::GetDepth()
	{
		return _bitmap.GetDepth();
	}

	void Image::ResetAlpha()
	{
		_bitmap.ResetAlpha();
	}

	bool Image::LoadFile(const string& name, int type)
	{
		return _bitmap.LoadFile(wxStr(name), (wxBitmapType)type);
	}

	bool Image::SaveFile(const string& name, int type)
	{
		return _bitmap.SaveFile(wxStr(name), (wxBitmapType)type);
	}

	bool Image::SaveStream(void* stream, int type)
	{
		OutputStream outputStream(stream);
		ManagedOutputStream managedOutputStream(&outputStream);

		auto image = _bitmap.ConvertToImage();

		return image.SaveFile(managedOutputStream, (wxBitmapType)type);
	}

	bool Image::LoadStream(void* stream, int type)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		wxImage image;
		auto result = image.LoadFile(managedInputStream, (wxBitmapType)type);

		if (result)
		{
			_bitmap = wxBitmap(image);
			return true;
		}
		else
		{
			_bitmap = wxBitmap();
			return false;
		}
	}

	Image* Image::ConvertToDisabled(uint8_t brightness)
	{
		auto disabled = _bitmap.ConvertToDisabled(brightness);
		auto result = new Image();
		result->_bitmap = disabled;
		return result;
	}

	Image* Image::GetSubBitmap(const Int32Rect& rect)
	{
		auto sub = _bitmap.GetSubBitmap(rect);
		auto result = new Image();
		result->_bitmap = sub;
		return result;
	}
}