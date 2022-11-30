#include "ImageSet.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"

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
		wxIcon icon;
		icon.CopyFromBitmap(image->GetBitmap());
		_iconBundle.AddIcon(icon);
		_bitmaps.push_back(image->GetBitmap());
		InvalidateBitmapBundle();
	}
	
	void ImageSet::LoadFromStream(void* stream)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		static bool imageHandlersInitialized = false;
		if (!imageHandlersInitialized)
		{
			wxInitAllImageHandlers();
			imageHandlersInitialized = true;
		}

		_iconBundle.AddIcon(managedInputStream);
		managedInputStream.SeekI(0);
		_bitmaps.push_back(wxBitmap(managedInputStream));
		InvalidateBitmapBundle();
	}

	wxIconBundle* ImageSet::GetIconBundle()
	{
		return &_iconBundle;
	}
	
	wxBitmapBundle ImageSet::GetBitmapBundle()
	{
		if (!_bitmapBundleValid)
		{
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
