#include "ImageSet.h"

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
	
	wxIconBundle* ImageSet::GetIconBundle()
	{
		return &_bundle;
	}
}
