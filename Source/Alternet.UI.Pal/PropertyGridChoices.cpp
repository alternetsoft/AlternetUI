#include "PropertyGridChoices.h"

namespace Alternet::UI
{
	PropertyGridChoices::PropertyGridChoices()
	{
	}

	PropertyGridChoices::~PropertyGridChoices()
	{

	}

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

	string PropertyGridChoices::GetLabel(void* handle, uint32_t ind)
	{
		PropertyGridChoices* instance = Choices(handle);
		return wxStr(instance->choices.GetLabel(ind));
	}

	uint32_t PropertyGridChoices::GetCount(void* handle)
	{
		PropertyGridChoices* instance = Choices(handle);
		return instance->choices.GetCount();
	}

	int PropertyGridChoices::GetValue(void* handle, uint32_t ind)
	{
		PropertyGridChoices* instance = Choices(handle);
		return instance->choices.GetValue(ind);
	}

	int PropertyGridChoices::GetLabelIndex(void* handle, const string& str)
	{
		PropertyGridChoices* instance = Choices(handle);
		return instance->choices.Index(wxStr(str));
	}

	int PropertyGridChoices::GetValueIndex(void* handle, int val)
	{
		PropertyGridChoices* instance = Choices(handle);
		return instance->choices.Index(val);
	}

	void PropertyGridChoices::Insert(void* handle, int index, const string& text, 
		int value, ImageSet* bitmapBundle)
	{
		PropertyGridChoices* instance = Choices(handle);

		wxPGChoiceEntry entry = wxPGChoiceEntry(wxStr(text), value);
		if (bitmapBundle != nullptr)
			entry.SetBitmap(ImageSet::BitmapBundle(bitmapBundle));

		instance->choices.Insert(entry, index);
	}

	bool PropertyGridChoices::IsOk(void* handle)
	{
		PropertyGridChoices* instance = Choices(handle);
		return instance->choices.IsOk();
	}

	void PropertyGridChoices::RemoveAt(void* handle, uint64_t nIndex, uint64_t count)
	{
		PropertyGridChoices* instance = Choices(handle);
		instance->choices.RemoveAt(nIndex, count);
	}

	void PropertyGridChoices::Clear(void* handle)
	{
		PropertyGridChoices* instance = Choices(handle);
		instance->choices.Clear();
	}
}
