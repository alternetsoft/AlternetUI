#include "IconSet.h"
#include "Api/InputStream.h"
#include "ManagedInputStream.h"
#include "Image.h"

#include <wx/wxprec.h>

namespace Alternet::UI
{
	IconSet::IconSet()
	{
	}

	IconSet::~IconSet()
	{
	}

	bool IconSet::IsOk()
	{
		return _iconBundle.IsOk();
	}

	void IconSet::AddImage(Image* image)
	{
		wxIcon icon;
		icon.CopyFromBitmap(image->GetBitmap());
		_iconBundle.AddIcon(icon);
	}

	void IconSet::LoadFromStream(void* stream)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);

		Image::EnsureImageHandlersInitialized();
		managedInputStream.SeekI(0);
		_iconBundle.AddIcon(managedInputStream);
	}

	void IconSet::Clear()
	{
		_iconBundle = wxIconBundle();
	}
}
