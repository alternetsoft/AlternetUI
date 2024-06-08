#include "GenericImage.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"

namespace Alternet::UI
{
	void wxImageAddHandlerV2(wxImageHandler* handler)
	{
		if (wxImage::FindHandler(handler->GetType()) == 0)
			wxImage::AddHandler(handler);
	}

	void GenericImage::wxInitAllImageHandlersV2()
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

	/*static*/ void GenericImage::EnsureImageHandlersInitialized()
	{
		static bool imageHandlersInitialized = false;
		if (!imageHandlersInitialized)
		{
			wxInitAllImageHandlersV2();
			imageHandlersInitialized = true;
		}
	}

	GenericImage::GenericImage()
	{
		EnsureImageHandlersInitialized();
	}

	GenericImage::~GenericImage()
	{
	}

	void* GenericImage::CreateImage()
	{
		return new GenericImage();
	}

	void* GenericImage::CreateImageWithSize(int width, int height, bool clear)
	{
		return new GenericImage(wxImage(width, height, clear));
	}

	void* GenericImage::ConvertToDisabled(void* handle, uint8_t brightness)
	{
		auto image = ((GenericImage*)handle)->_image.ConvertToDisabled(brightness);
		return new GenericImage(image);
	}

	void* GenericImage::CreateImageFromFileWithBitmapType(const string& name, int bitmapType, int index)
	{
		return new GenericImage(wxImage(wxStr(name), (wxBitmapType)bitmapType, index));
	}

	void* GenericImage::CreateImageFromFileWithMimeType(const string& name, const string& mimetype, int index)
	{
		return new GenericImage(wxImage(wxStr(name), wxStr(mimetype), index));
	}

	void* GenericImage::CreateImageFromStreamWithBitmapData(void* stream, int bitmapType, int index)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		auto image = wxImage(managedInputStream, (wxBitmapType)bitmapType, index);
		return new GenericImage(image);
	}

	void* GenericImage::CreateImageFromStreamWithMimeType(void* stream, const string& mimetype, int index)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		auto image = wxImage(managedInputStream, wxStr(mimetype), index);
		return new GenericImage(image);
	}

	void* GenericImage::CreateImageWithSizeAndData(int width, int height, void* data, bool static_data)
	{
		auto image = wxImage(width, height, (unsigned char*)data, static_data);
		return new GenericImage(image);
	}

	void* GenericImage::CreateImageWithAlpha(int width, int height, void* data, void* alpha, bool static_data)
	{
		auto image = wxImage(width, height, (unsigned char*)data, (unsigned char*)alpha, static_data);
		return new GenericImage(image);
	}

	void GenericImage::DeleteImage(void* handle)
	{
		delete (GenericImage*)handle;
	}

	void GenericImage::SetAlpha(void* handle, int x, int y, uint8_t alpha)
	{
		((GenericImage*)handle)->_image.SetAlpha(x, y, alpha);
	}

	void GenericImage::ClearAlpha(void* handle)
	{
		((GenericImage*)handle)->_image.ClearAlpha();
	}

	void GenericImage::SetLoadFlags(void* handle, int flags)
	{
		((GenericImage*)handle)->_image.SetLoadFlags(flags);
	}

	void GenericImage::SetMask(void* handle, bool hasMask)
	{
		((GenericImage*)handle)->_image.SetMask(hasMask);
	}

	void GenericImage::SetMaskColor(void* handle, uint8_t red, uint8_t green, uint8_t blue)
	{
		((GenericImage*)handle)->_image.SetMaskColour(red, green, blue);
	}

	bool GenericImage::SetMaskFromImage(void* handle, void* image, uint8_t mr, uint8_t mg, uint8_t mb)
	{
		return ((GenericImage*)handle)->_image.SetMaskFromImage(
			((GenericImage*)image)->_image, mr, mg, mb);
	}

	void GenericImage::SetOptionString(void* handle, const string& name, const string& value)
	{
		((GenericImage*)handle)->_image.SetOption(wxStr(name), wxStr(value));
	}

	void GenericImage::SetOptionInt(void* handle, const string& name, int value)
	{
		((GenericImage*)handle)->_image.SetOption(wxStr(name), value);
	}

	void GenericImage::SetRGB(void* handle, int x, int y, uint8_t r, uint8_t g, uint8_t b)
	{
		((GenericImage*)handle)->_image.SetRGB(x, y, r, g, b);
	}

	void GenericImage::SetRGBRect(void* handle, const Int32Rect& rect, uint8_t red, uint8_t green, uint8_t blue)
	{
		((GenericImage*)handle)->_image.SetRGB(rect, red, green, blue);
	}

	void GenericImage::SetImageType(void* handle, int type)
	{
		((GenericImage*)handle)->_image.SetType((wxBitmapType)type);
	}

	void GenericImage::SetDefaultLoadFlags(int flags)
	{
		wxImage::SetDefaultLoadFlags(flags);
	}

	int GenericImage::GetLoadFlags(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetLoadFlags();
	}

	void* GenericImage::Copy(void* handle)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Copy());
	}

	void GenericImage::Clear(void* handle, uint8_t value)
	{
		((GenericImage*)handle)->_image.Clear(value);
	}

	void GenericImage::DestroyImageData(void* handle)
	{
		((GenericImage*)handle)->_image.Destroy();
	}

	void GenericImage::InitAlpha(void* handle)
	{
		((GenericImage*)handle)->_image.InitAlpha();
	}

	void* GenericImage::Blur(void* handle, int blurRadius)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Blur(blurRadius));
	}

	void* GenericImage::BlurHorizontal(void* handle, int blurRadius)
	{
		return new GenericImage(((GenericImage*)handle)->_image.BlurHorizontal(blurRadius));
	}

	void* GenericImage::BlurVertical(void* handle, int blurRadius)
	{
		return new GenericImage(((GenericImage*)handle)->_image.BlurVertical(blurRadius));
	}

	void* GenericImage::Mirror(void* handle, bool horizontally)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Mirror(horizontally));
	}

	void GenericImage::Paste(void* handle, void* image, int x, int y, int alphaBlend)
	{
		((GenericImage*)handle)->_image.Paste(((GenericImage*)image)->_image, x, y,
			(wxImageAlphaBlendMode)alphaBlend);
	}

	void GenericImage::Replace(void* handle, uint8_t r1, uint8_t g1, uint8_t b1, uint8_t r2, uint8_t g2, uint8_t b2)
	{
		((GenericImage*)handle)->_image.Replace(r1, g1, b1, r2, g2, b2);
	}

	void GenericImage::Rescale(void* handle, int width, int height, int quality)
	{
		((GenericImage*)handle)->_image.Rescale(width, height, (wxImageResizeQuality)quality);
	}

	void GenericImage::Resize(void* handle, const Int32Size& size, const Int32Point& pos,
		int red, int green, int blue)
	{
		((GenericImage*)handle)->_image.Resize(size, pos, red, green, blue);
	}

	void* GenericImage::Rotate90(void* handle, bool clockwise)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Rotate90(clockwise));
	}

	void* GenericImage::Rotate180(void* handle)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Rotate180());
	}

	void GenericImage::RotateHue(void* handle, double angle)
	{
		((GenericImage*)handle)->_image.RotateHue(angle);
	}

	void GenericImage::ChangeSaturation(void* handle, double factor)
	{
		((GenericImage*)handle)->_image.ChangeSaturation(factor);
	}

	void GenericImage::ChangeBrightness(void* handle, double factor)
	{
		((GenericImage*)handle)->_image.ChangeBrightness(factor);
	}

	void GenericImage::ChangeHSV(void* handle, double angleH, double factorS, double factorV)
	{
		((GenericImage*)handle)->_image.ChangeHSV(angleH, factorS, factorV);
	}

	void* GenericImage::Scale(void* handle, int width, int height, int quality)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Scale(width, height,
			(wxImageResizeQuality)quality));
	}

	void* GenericImage::Size(void* handle, const Int32Size& size, const Int32Point& pos,
		int red, int green, int blue)
	{
		return new GenericImage(((GenericImage*)handle)->_image.Size(size, pos, red, green, blue));
	}

	bool GenericImage::ConvertAlphaToMask(void* handle, uint8_t threshold)
	{
		return ((GenericImage*)handle)->_image.ConvertAlphaToMask(threshold);
	}

	bool GenericImage::ConvertAlphaToMaskUseColor(void* handle, uint8_t mr, uint8_t mg,
		uint8_t mb, uint8_t threshold)
	{
		return ((GenericImage*)handle)->_image.ConvertAlphaToMask(mr, mg, mb, threshold);
	}

	void* GenericImage::ConvertToGreyscaleEx(void* handle, double weight_r, double weight_g,
		double weight_b)
	{
		return new GenericImage(((GenericImage*)handle)->
			_image.ConvertToGreyscale(weight_r, weight_g, weight_b));
	}

	void* GenericImage::ConvertToGreyscale(void* handle)
	{
		return new GenericImage(((GenericImage*)handle)->_image.ConvertToGreyscale());
	}

	void* GenericImage::ConvertToMono(void* handle, uint8_t r, uint8_t g, uint8_t b)
	{
		return new GenericImage(((GenericImage*)handle)->_image.ConvertToMono(r, g, b));
	}

	Color GenericImage::FindFirstUnusedColor(void* handle, uint8_t startR,
		uint8_t startG, uint8_t startB)
	{
		unsigned char r;
		unsigned char g;
		unsigned char b;

		((GenericImage*)handle)->_image.FindFirstUnusedColour(&r, &g, &b, startR, startG, startB);

		return Color(255, r, g, b);
	}

	void* GenericImage::ChangeLightness(void* handle, int alpha)
	{
		return new GenericImage(((GenericImage*)handle)->_image.ChangeLightness(alpha));
	}

	uint8_t GenericImage::GetAlpha(void* handle, int x, int y)
	{
		return ((GenericImage*)handle)->_image.GetAlpha(x, y);
	}

	uint8_t GenericImage::GetRed(void* handle, int x, int y)
	{
		return ((GenericImage*)handle)->_image.GetRed(x, y);
	}

	uint8_t GenericImage::GetGreen(void* handle, int x, int y)
	{
		return ((GenericImage*)handle)->_image.GetGreen(x, y);
	}

	uint8_t GenericImage::GetBlue(void* handle, int x, int y)
	{
		return ((GenericImage*)handle)->_image.GetBlue(x, y);
	}

	uint8_t GenericImage::GetMaskRed(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetMaskRed();
	}

	uint8_t GenericImage::GetMaskGreen(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetMaskGreen();
	}

	uint8_t GenericImage::GetMaskBlue(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetMaskBlue();
	}

	int GenericImage::GetWidth(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetWidth();
	}

	int GenericImage::GetHeight(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetHeight();
	}

	Int32Size GenericImage::GetSize(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetSize();
	}

	string GenericImage::GetOptionString(void* handle, const string& name)
	{
		return wxStr(((GenericImage*)handle)->_image.GetOption(wxStr(name)));
	}

	int GenericImage::GetOptionInt(void* handle, const string& name)
	{
		return ((GenericImage*)handle)->_image.GetOptionInt(wxStr(name));
	}

	void* GenericImage::GetSubImage(void* handle, const Int32Rect& rect)
	{
		return new GenericImage(((GenericImage*)handle)->_image.GetSubImage(rect));
	}

	int GenericImage::GetImageType(void* handle)
	{
		
		return ((GenericImage*)handle)->_image.GetType();;
	}

	bool GenericImage::HasAlpha(void* handle)
	{
		return ((GenericImage*)handle)->_image.HasAlpha();
	}

	bool GenericImage::HasMask(void* handle)
	{
		return ((GenericImage*)handle)->_image.HasMask();
	}

	bool GenericImage::HasOption(void* handle, const string& name)
	{
		return ((GenericImage*)handle)->_image.HasOption(wxStr(name));
	}

	bool GenericImage::IsOk(void* handle)
	{
		return ((GenericImage*)handle)->_image.IsOk();
	}

	bool GenericImage::IsTransparent(void* handle, int x, int y, uint8_t threshold)
	{
		return ((GenericImage*)handle)->_image.IsTransparent(x, y, threshold);
	}

	bool GenericImage::LoadStreamWithBitmapType(void* handle, void* stream, int bitmapType, int index)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		return ((GenericImage*)handle)->_image.LoadFile(managedInputStream,
			(wxBitmapType)bitmapType, index);
	}

	bool GenericImage::LoadFileWithBitmapType(void* handle, const string& name, int bitmapType, int index)
	{
		return ((GenericImage*)handle)->_image.LoadFile(wxStr(name),
			(wxBitmapType)bitmapType, index);
	}

	bool GenericImage::LoadFileWithMimeType(void* handle, const string& name,
		const string& mimetype, int index)
	{
		return ((GenericImage*)handle)->_image.LoadFile(wxStr(name),
			wxStr(mimetype), index);
	}

	bool GenericImage::LoadStreamWithMimeType(void* handle, void* stream,
		const string& mimetype, int index)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		return ((GenericImage*)handle)->_image.LoadFile(managedInputStream,
			wxStr(mimetype), index);
	}

	bool GenericImage::SaveStreamWithMimeType(void* handle, void* stream, const string& mimetype)
	{
		OutputStream outputStream(stream);
		ManagedOutputStream managedOutputStream(&outputStream);
		return ((GenericImage*)handle)->_image.SaveFile(managedOutputStream, wxStr(mimetype));
	}

	bool GenericImage::SaveFileWithBitmapType(void* handle, const string& name, int bitmapType)
	{
		return ((GenericImage*)handle)->_image.SaveFile(wxStr(name), (wxBitmapType)bitmapType);
	}

	bool GenericImage::SaveFileWithMimeType(void* handle, const string& name, const string& mimetype)
	{
		return ((GenericImage*)handle)->_image.SaveFile(wxStr(name), wxStr(mimetype));
	}

	bool GenericImage::SaveFile(void* handle, const string& name)
	{
		return ((GenericImage*)handle)->_image.SaveFile(wxStr(name));
	}

	bool GenericImage::SaveStreamWithBitmapType(void* handle, void* stream, int type)
	{
		OutputStream outputStream(stream);
		ManagedOutputStream managedOutputStream(&outputStream);
		return ((GenericImage*)handle)->_image.SaveFile(managedOutputStream, (wxBitmapType)type);
	}

	bool GenericImage::CanRead(const string& filename)
	{
		return wxImage::CanRead(wxStr(filename));
	}

	bool GenericImage::CanReadStream(void* stream)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		return wxImage::CanRead(managedInputStream);
	}

	int GenericImage::GetDefaultLoadFlags()
	{
		return wxImage::GetDefaultLoadFlags();
	}

	string GenericImage::GetImageExtWildcard()
	{
		return wxStr(wxImage::GetImageExtWildcard());
	}

	void GenericImage::AddHandler(void* handler)
	{
		wxImage::AddHandler((wxImageHandler*)handler);
	}

	void GenericImage::CleanUpHandlers()
	{
		wxImage::CleanUpHandlers();
	}

	void* GenericImage::FindHandlerByName(const string& name)
	{
		return wxImage::FindHandler(wxStr(name));
	}

	void* GenericImage::FindHandlerByExt(const string& extension, int bitmapType)
	{
		return wxImage::FindHandler(wxStr(extension), (wxBitmapType)bitmapType);
	}

	void* GenericImage::FindHandlerByBitmapType(int bitmapType)
	{
		return wxImage::FindHandler((wxBitmapType)bitmapType);
	}

	void* GenericImage::FindHandlerByMime(const string& mimetype)
	{
		return wxImage::FindHandlerMime(wxStr(mimetype));
	}

	void GenericImage::InsertHandler(void* handler)
	{
		wxImage::InsertHandler((wxImageHandler*)handler);
	}

	bool GenericImage::RemoveHandler(const string& name)
	{
		return wxImage::RemoveHandler(wxStr(name));
	}

	int GenericImage::GetImageCountInFile(const string& filename, int bitmapType)
	{
		return wxImage::GetImageCount(wxStr(filename), (wxBitmapType)bitmapType);;
	}

	int GenericImage::GetImageCountInStream(void* stream, int bitmapType)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		return wxImage::GetImageCount(managedInputStream, (wxBitmapType)bitmapType);
	}

	void* GenericImage::GetAlphaData(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetAlpha();
	}

	void* GenericImage::GetData(void* handle)
	{
		return ((GenericImage*)handle)->_image.GetData();
	}

	bool GenericImage::CreateData(void* handle, int width, int height, void* data, bool static_data)
	{
		return ((GenericImage*)handle)->_image.Create(width, height, (unsigned char*)data, static_data);
	}

	bool GenericImage::CreateAlphaData(void* handle, int width, int height, void* data,
		void* alpha, bool static_data)
	{
		return ((GenericImage*)handle)->_image.Create(width, height, (unsigned char*)data,
			(unsigned char*)alpha, static_data);
	}

	void GenericImage::SetData(void* handle, void* data, bool static_data)
	{
		((GenericImage*)handle)->_image.SetData((unsigned char*)data, static_data);
	}

	void GenericImage::SetDataWithSize(void* handle, void* data, int new_width,
		int new_height, bool static_data)
	{
		((GenericImage*)handle)->_image.SetData((unsigned char*)data, new_width,
			new_height, static_data);
	}

	void GenericImage::SetAlphaData(void* handle, void* alpha, bool static_data)
	{
		((GenericImage*)handle)->_image.SetAlpha((unsigned char*)alpha, static_data);
	}

	bool GenericImage::CreateFreshImage(void* handle, int width, int height, bool clear)
	{
		return ((GenericImage*)handle)->_image.Create(width, height, clear);
	}

	int GenericImage::GetStride(void* handle)
	{
		return ((GenericImage*)handle)->_stride;
	}

	void* GenericImage::LockBits(void* handle)
	{
		if (((GenericImage*)handle)->pixelData)
			return nullptr;

		 auto pixelData = new ImageGenericPixelData(((GenericImage*)handle)->_image);

		 ((GenericImage*)handle)->pixelData = pixelData;

		if (!pixelData)
			return nullptr;
		((GenericImage*)handle)->_stride = pixelData->GetRowStride();
		auto pixels = pixelData->GetPixels();
		return pixels.m_pRGB;
	}

	void GenericImage::UnlockBits(void* handle)
	{
		if (((GenericImage*)handle)->pixelData)
		{
			delete ((GenericImage*)handle)->pixelData;
			((GenericImage*)handle)->pixelData = nullptr;
		}
	}
}
