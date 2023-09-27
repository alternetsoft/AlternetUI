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
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				window->Unbind(wxEVT_HYPERLINK,
					&LinkLabel::OnHyperlinkClick, this);
			}
		}

	}

	void LinkLabel::OnHyperlinkClick(wxHyperlinkEvent& event)
	{
		bool c = RaiseEvent(LinkLabelEvent::HyperlinkClick);

		if (!c)
			event.Skip();
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

	class wxHyperlinkCtrl2 : public wxHyperlinkCtrl, public wxWidgetExtender
	{
	public:
		wxHyperlinkCtrl2(wxWindow* parent,
			wxWindowID id,
			const wxString& label, const wxString& url,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxHL_DEFAULT_STYLE,
			const wxString& name = wxASCII_STR(wxHyperlinkCtrlNameStr))
		{
			Create(parent, id, label, url, pos, size, style, name);
		}
		void OnEraseBackGround(wxEraseEvent& event) {};
		DECLARE_EVENT_TABLE()
	};
	BEGIN_EVENT_TABLE(wxHyperlinkCtrl2, wxHyperlinkCtrl)
		EVT_ERASE_BACKGROUND(wxHyperlinkCtrl2::OnEraseBackGround)
		END_EVENT_TABLE()

	wxWindow* LinkLabel::CreateWxWindowCore(wxWindow* parent)
	{
		// Text must be " " (space) to avoid exception on Linux
		auto staticText = new wxHyperlinkCtrl2(
			parent, wxID_ANY, " ", wxEmptyString);
		staticText->Bind(wxEVT_HYPERLINK,
			&LinkLabel::OnHyperlinkClick, this);
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
