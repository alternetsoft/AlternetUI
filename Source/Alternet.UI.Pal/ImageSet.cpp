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
		_bundle.AddIcon(icon);
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

		_bundle.AddIcon(managedInputStream);
	}

	wxIconBundle* ImageSet::GetIconBundle()
	{
		return &_bundle;
	}
}
