#include "LinkLabel.h"

namespace Alternet::UI
{
	LinkLabel::LinkLabel() :
		_text(*this, u"", &Control::IsWxWindowCreated, &LinkLabel::RetrieveText,
			&LinkLabel::ApplyText),
		_url(*this, u"", &Control::IsWxWindowCreated, &LinkLabel::RetrieveUrl,
			&LinkLabel::ApplyUrl)
	{
		GetDelayedValues().Add(&_text);
		GetDelayedValues().Add(&_url);
	}

	LinkLabel::~LinkLabel()
	{
	}

	string LinkLabel::GetText()
	{
		return _text.Get();
	}

	void LinkLabel::SetText(const string& value)
	{
		_text.Set(value);
	}

	string LinkLabel::GetUrl() 
	{
		return _url.Get();
	}
	
	void LinkLabel::SetUrl(const string& value)
	{
		_url.Set(value);
	}

	wxWindow* LinkLabel::CreateWxWindowCore(wxWindow* parent)
	{
		auto staticText = new wxHyperlinkCtrl(
			parent, wxID_ANY, " ", wxEmptyString);
		return staticText;
	}

	wxHyperlinkCtrl* LinkLabel::GetStaticText()
	{
		return dynamic_cast<wxHyperlinkCtrl*>(GetWxWindow());
	}

	string LinkLabel::RetrieveUrl()
	{
		return wxStr(GetStaticText()->GetURL());
	}

	void LinkLabel::ApplyUrl(const string& value)
	{
		GetStaticText()->SetURL(wxStr(value));
	}

	string LinkLabel::RetrieveText()
	{
		return wxStr(GetStaticText()->GetLabel());
	}

	void LinkLabel::ApplyText(const string& value)
	{
		GetStaticText()->SetLabel(wxStr(value));
	}

}
