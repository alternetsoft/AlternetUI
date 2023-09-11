#include "PropertyGridChoices.h"

namespace Alternet::UI
{
	wxPGChoiceEntry& PropertyGridChoices::Item(void* handle, uint32_t ind)
	{
		PropertyGridChoices* instance = Choices(handle);
		wxPGChoiceEntry& item = instance->choices.Item(ind);
		return item;
	}

	void PropertyGridChoices::SetLabel(void* handle, uint32_t ind, const string& value)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		item.SetText(wxStr(value));
	}

	void* PropertyGridChoices::GetFont(void* handle, uint32_t ind)
	{
		return nullptr;
	}

	void PropertyGridChoices::SetBitmapFromItem(void* handle, uint32_t ind,
		void* handle2, uint32_t ind2)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		wxPGChoiceEntry& item2 = Item(handle2, ind2);
		item.SetBitmap(item2.GetBitmap());
	}
	
	void PropertyGridChoices::SetFontFromItem(void* handle, uint32_t ind,
		void* handle2, uint32_t ind2)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		wxPGChoiceEntry& item2 = Item(handle2, ind2);
		item.SetFont(item2.GetFont());
	}

	void PropertyGridChoices::SetFont(void* handle, uint32_t ind, void* font)
	{
	}

	void* PropertyGridChoices::GetBitmap(void* handle, uint32_t ind)
	{
		return nullptr;
	}

	void PropertyGridChoices::SetBitmap(void* handle, uint32_t ind, ImageSet* bitmap)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		item.SetBitmap(ImageSet::BitmapBundle(bitmap));
	}

	void PropertyGridChoices::SetFgCol(void* handle, uint32_t ind, const Color& color)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		item.SetFgCol(color);
	}

	void PropertyGridChoices::SetBgCol(void* handle, uint32_t ind, const Color& color)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		item.SetBgCol(color);
	}

	Color PropertyGridChoices::GetFgCol(void* handle, uint32_t ind)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		return item.GetFgCol();
	}

	Color PropertyGridChoices::GetBgCol(void* handle, uint32_t ind)
	{
		wxPGChoiceEntry& item = Item(handle, ind);
		return item.GetBgCol();
	}

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
