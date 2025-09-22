#include "ImageSet.h"
#include "GenericImage.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"
#include "Image.h"

#include <wx/wxprec.h>

namespace Alternet::UI
{
	ImageSet::ImageSet()
	{
	}

	ImageSet::~ImageSet()
	{
	}

	void ImageSet::AddImage(Image* image)
	{
		if (_readOnly)
			return;
		_bitmaps.push_back(image->GetBitmap());
		InvalidateBitmapBundle();
	}

	void ImageSet::LoadFromStream(void* stream)
	{
		if (_readOnly)
			return;
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		GenericImage::EnsureImageHandlersInitialized();

		managedInputStream.SeekI(0);
		_bitmaps.push_back(wxBitmap(managedInputStream));
		InvalidateBitmapBundle();
	}

	bool ImageSet::GetIsOk()
	{
		return GetBitmapBundle().IsOk();
	}

	bool ImageSet::GetIsReadOnly()
	{
		return _readOnly;
	}

	void ImageSet::Clear()
	{
		if (_readOnly || _bitmaps.empty())
			return;
		_bitmaps.clear();
		InvalidateBitmapBundle();
	}

	void ImageSet::LoadSvgFromString(const string& s, int width, int height, const Color& color)
	{
		Clear();
		_bitmapBundleValid = true;
		_readOnly = true;
		_bitmapBundle = Image::CreateFromSvgStr(s, width, height, color);
	}

	void ImageSet::LoadSvgFromStream(void* stream, int width, int height, const Color& color)
	{
		Clear();
		_bitmapBundleValid = true;
		_readOnly = true;
		_bitmapBundle = Image::CreateFromSvgStream(stream, width, height, color);
	}

	wxIconBundle ImageSet::GetIconBundle()
	{
		auto icons = wxIconBundle();

		return icons;
	}

	Int32Size ImageSet::GetDefaultSize()
	{
		return GetBitmapBundle().GetDefaultSize();
	}

	Int32Size ImageSet::GetPreferredBitmapSizeFor(void* window)
	{
		return GetBitmapBundle().GetPreferredBitmapSizeFor((wxWindow*)window);
	}

	Int32Size ImageSet::GetPreferredBitmapSizeAtScale(Coord scale)
	{
		return GetBitmapBundle().GetPreferredBitmapSizeAtScale(scale);
	}

	void ImageSet::InitImageFor(Image* image, void* window)
	{
		auto bitmapBundle = GetBitmapBundle();
		auto bitmap = bitmapBundle.GetBitmapFor((wxWindow*)window);
		image->_bitmap = bitmap;
	}

	void ImageSet::InitImage(Image* image, int width, int height)
	{
		auto bitmapBundle = GetBitmapBundle();
		auto bitmap = bitmapBundle.GetBitmap(wxSize(width, height));
		image->_bitmap = bitmap;
	}

	wxBitmapBundle ImageSet::GetBitmapBundle()
	{
		if (!_bitmapBundleValid)
		{
			if (_readOnly)
				return _bitmapBundle;
			_bitmapBundle = wxBitmapBundle::FromBitmaps(_bitmaps);
			_bitmapBundleValid = true;
		}

		return _bitmapBundle;
	}
	
	void ImageSet::InvalidateBitmapBundle()
	{
		if (_bitmapBundleValid)
		{
			_bitmapBundle = wxBitmapBundle();
			_bitmapBundleValid = false;
		}
	}
}
