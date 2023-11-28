#include "GenericImage.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"

namespace Alternet::UI
{
	GenericImage::GenericImage()
	{
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
	}

	void GenericImage::ClearAlpha(void* handle)
	{
	}

	void GenericImage::SetLoadFlags(void* handle, int flags)
	{
	}

	void GenericImage::SetMask(void* handle, bool hasMask)
	{
	}

	void GenericImage::SetMaskColor(void* handle, uint8_t red, uint8_t green, uint8_t blue)
	{
	}

	bool GenericImage::SetMaskFromImage(void* handle, void* image, uint8_t mr, uint8_t mg, uint8_t mb)
	{
		return false;
	}

	void GenericImage::SetOptionString(void* handle, const string& name, const string& value)
	{
	}

	void GenericImage::SetOptionInt(void* handle, const string& name, int value)
	{
	}

	void GenericImage::SetRGB(void* handle, int x, int y, uint8_t r, uint8_t g, uint8_t b)
	{
	}

	void GenericImage::SetRGBRect(void* handle, const Int32Rect& rect, uint8_t red, uint8_t green, uint8_t blue)
	{
	}

	void GenericImage::SetImageType(void* handle, int type)
	{
	}

	void GenericImage::SetDefaultLoadFlags(int flags)
	{
	}

	int GenericImage::GetLoadFlags(void* handle)
	{
		return 0;
	}

	void* GenericImage::Copy(void* handle)
	{
		return nullptr;
	}

	bool GenericImage::CreateFreshImage(void* handle, int width, int height, bool clear)
	{
		return false;
	}

	void GenericImage::Clear(void* handle, uint8_t value)
	{
	}

	void GenericImage::DestroyImageData(void* handle)
	{
	}

	void GenericImage::InitAlpha(void* handle)
	{
	}

	void* GenericImage::Blur(void* handle, int blurRadius)
	{
		return nullptr;
	}

	void* GenericImage::BlurHorizontal(void* handle, int blurRadius)
	{
		return nullptr;
	}

	void* GenericImage::BlurVertical(void* handle, int blurRadius)
	{
		return nullptr;
	}

	void* GenericImage::Mirror(void* handle, bool horizontally)
	{
		return nullptr;
	}

	void GenericImage::Paste(void* handle, void* image, int x, int y, int alphaBlend)
	{
	}

	void GenericImage::Replace(void* handle, uint8_t r1, uint8_t g1, uint8_t b1, uint8_t r2, uint8_t g2, uint8_t b2)
	{
	}

	void* GenericImage::Rescale(void* handle, int width, int height, int quality)
	{
		return nullptr;
	}

	void* GenericImage::Resize(void* handle, const Int32Size& size, const Int32Point& pos, int red, int green, int blue)
	{
		return nullptr;
	}

	void* GenericImage::Rotate90(void* handle, bool clockwise)
	{
		return nullptr;
	}

	void* GenericImage::Rotate180(void* handle)
	{
		return nullptr;
	}

	void GenericImage::RotateHue(void* handle, double angle)
	{
	}

	void GenericImage::ChangeSaturation(void* handle, double factor)
	{
	}

	void GenericImage::ChangeBrightness(void* handle, double factor)
	{
	}

	void GenericImage::ChangeHSV(void* handle, double angleH, double factorS, double factorV)
	{
	}

	void* GenericImage::Scale(void* handle, int width, int height, int quality)
	{
		return nullptr;
	}

	void* GenericImage::Size(void* handle, const Int32Size& size, const Int32Point& pos, int red, int green, int blue)
	{
		return nullptr;
	}

	bool GenericImage::ConvertAlphaToMask(void* handle, uint8_t threshold)
	{
		return false;
	}

	bool GenericImage::ConvertAlphaToMaskUseColor(void* handle, uint8_t mr, uint8_t mg, uint8_t mb, uint8_t threshold)
	{
		return false;
	}

	void* GenericImage::ConvertToGreyscaleEx(void* handle, double weight_r, double weight_g, double weight_b)
	{
		return nullptr;
	}

	void* GenericImage::ConvertToGreyscale(void* handle)
	{
		return nullptr;
	}

	void* GenericImage::ConvertToMono(void* handle, uint8_t r, uint8_t g, uint8_t b)
	{
		return nullptr;
	}

	void* GenericImage::ChangeLightness(void* handle, int alpha)
	{
		return nullptr;
	}

	uint8_t GenericImage::GetAlpha(void* handle, int x, int y)
	{
		return 0;
	}

	uint8_t GenericImage::GetRed(void* handle, int x, int y)
	{
		return 0;
	}

	uint8_t GenericImage::GetGreen(void* handle, int x, int y)
	{
		return 0;
	}

	uint8_t GenericImage::GetBlue(void* handle, int x, int y)
	{
		return 0;
	}

	uint8_t GenericImage::GetMaskRed(void* handle)
	{
		return 0;
	}

	uint8_t GenericImage::GetMaskGreen(void* handle)
	{
		return 0;
	}

	uint8_t GenericImage::GetMaskBlue(void* handle)
	{
		return 0;
	}

	int GenericImage::GetWidth(void* handle)
	{
		return 0;
	}

	int GenericImage::GetHeight(void* handle)
	{
		return 0;
	}

	Int32Size GenericImage::GetSize(void* handle)
	{
		return Int32Size();
	}

	string GenericImage::GetOptionString(void* handle, const string& name)
	{
		return wxStr(wxEmptyString);
	}

	int GenericImage::GetOptionInt(void* handle, const string& name)
	{
		return 0;
	}

	void* GenericImage::GetSubImage(void* handle, const Int32Rect& rect)
	{
		return nullptr;
	}

	int GenericImage::GetImageType(void* handle)
	{
		return 0;
	}

	bool GenericImage::HasAlpha(void* handle)
	{
		return false;
	}

	bool GenericImage::HasMask(void* handle)
	{
		return false;
	}

	bool GenericImage::HasOption(void* handle, const string& name)
	{
		return false;
	}

	bool GenericImage::IsOk(void* handle)
	{
		return false;
	}

	bool GenericImage::IsTransparent(void* handle, int x, int y, uint8_t threshold)
	{
		return false;
	}

	bool GenericImage::LoadStreamWithBitmapType(void* handle, void* stream, int bitmapType, int index)
	{
		return false;
	}

	bool GenericImage::LoadFileWithBitmapType(void* handle, const string& name, int bitmapType, int index)
	{
		return false;
	}

	bool GenericImage::LoadFileWithMimeType(void* handle, const string& name, const string& mimetype, int index)
	{
		return false;
	}

	bool GenericImage::LoadStreamWithMimeType(void* handle, void* stream, const string& mimetype, int index)
	{
		return false;
	}

	bool GenericImage::SaveStreamWithMimeType(void* handle, void* stream, const string& mimetype)
	{
		return false;
	}

	bool GenericImage::SaveFileWithBitmapType(void* handle, const string& name, int bitmapType)
	{
		return false;
	}

	bool GenericImage::SaveFileWithMimeType(void* handle, const string& name, const string& mimetype)
	{
		return false;
	}

	bool GenericImage::SaveFile(void* handle, const string& name)
	{
		return false;
	}

	bool GenericImage::SaveStreamWithBitmapType(void* handle, void* stream, int type)
	{
		return false;
	}

	bool GenericImage::CanRead(const string& filename)
	{
		return false;
	}

	bool GenericImage::CanReadStream(void* stream)
	{
		return false;
	}

	int GenericImage::GetDefaultLoadFlags()
	{
		return false;
	}

	string GenericImage::GetImageExtWildcard()
	{
		return wxStr(wxEmptyString);
	}

	void GenericImage::AddHandler(void* handler)
	{
	}

	void GenericImage::CleanUpHandlers()
	{
	}

	void* GenericImage::FindHandlerByName(const string& name)
	{
		return nullptr;
	}

	void* GenericImage::FindHandlerByExt(const string& extension, int bitmapType)
	{
		return nullptr;
	}

	void* GenericImage::FindHandlerByBitmapType(int bitmapType)
	{
		return nullptr;
	}

	void* GenericImage::FindHandlerByMime(const string& mimetype)
	{
		return nullptr;
	}

	void GenericImage::InsertHandler(void* handler)
	{
	}

	bool GenericImage::RemoveHandler(const string& name)
	{
		return false;
	}

	int GenericImage::GetImageCountInFile(const string& filename, int bitmapType)
	{
		return 0;
	}

	int GenericImage::GetImageCountInStream(void* stream, int bitmapType)
	{
		return 0;
	}

	void* GenericImage::GetAlphaData(void* handle)
	{
		return nullptr;
	}

	void* GenericImage::GetData(void* handle)
	{
		return nullptr;
	}

	bool GenericImage::CreateData(void* handle, int width, int height, void* data, bool static_data)
	{
		return false;
	}

	bool GenericImage::CreateAlphaData(void* handle, int width, int height, void* data, void* alpha, bool static_data)
	{
		return false;
	}

	void GenericImage::SetData(void* handle, void* data, bool static_data)
	{
	}

	void GenericImage::SetDataWithSize(void* handle, void* data, int new_width, int new_height, bool static_data)
	{
	}

	void GenericImage::SetAlphaData(void* handle, void* alpha, bool static_data)
	{
	}
}
