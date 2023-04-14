#include "Label.h"

namespace Alternet::UI
{
	Label::Label():
		_text(*this, u"", &Control::IsWxWindowCreated, &Label::RetrieveText, &Label::ApplyText)
	{
		GetDelayedValues().Add(&_text);
	}

	Label::~Label()
	{
	}

	string Label::GetText()
	{
		return _text.Get();
	}

	void Label::SetText(const string& value)
	{
		_text.Set(value);
	}

	wxWindow* Label::CreateWxWindowCore(wxWindow* parent)
	{
		auto staticText = new wxStaticText(
			parent, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, 0);
		return staticText;
	}

	wxStaticText* Label::GetStaticText()
	{
		return dynamic_cast<wxStaticText*>(GetWxWindow());
	}

	string Label::RetrieveText()
	{
		return wxStr(GetStaticText()->GetLabel());
	}

	void Label::ApplyText(const string& value)
	{
		GetStaticText()->SetLabel(wxStr(value));
	}

	bool Label::IsEllipsized()
	{
		return GetStaticText()->IsEllipsized();
	}

	void Label::Wrap(int width)
	{
		GetStaticText()->Wrap(width);
	}
}
