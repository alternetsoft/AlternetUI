#include "PropertyGridChoices.h"

namespace Alternet::UI
{
	PropertyGridChoices* PropertyGridChoices::Choices(void* handle)
	{
		return (PropertyGridChoices*)handle;
	}

	void* PropertyGridChoices::CreatePropertyGridChoices()
	{
		return new PropertyGridChoices();
	}

	void PropertyGridChoices::Delete(void* handle)
	{
		delete (PropertyGridChoices*)handle;
	}

	void PropertyGridChoices::Add(void* handle, const string& text, 
		int value, ImageSet* bitmapBundle)
	{
		PropertyGridChoices* instance = Choices(handle);

		instance->choices.Add(wxStr(text), ImageSet::BitmapBundle(bitmapBundle), value);
	}

	PropertyGridChoices::PropertyGridChoices()
	{
	}

	PropertyGridChoices::~PropertyGridChoices()
	{

	}
}
