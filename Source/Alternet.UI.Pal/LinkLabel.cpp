#include "LinkLabel.h"

namespace Alternet::UI
{
#ifdef  __WXMSW__
	bool UseGenericLinkLabel = false;
#endif
#ifdef __WXOSX__
	bool UseGenericLinkLabel = false;
#endif
#ifdef __WXGTK__
	// Error if FALSE:
	//  On Ubuntu 23 non generic version shows an error when clicked.
	// Error if TRUE:
	// EXEC : libEGL warning : DRI2: failed to authenticate 
	// ATTENTION: default value of option mesa_glthread overridden by environment.
	bool UseGenericLinkLabel = false;
#endif

	bool LinkLabel::GetUseGenericControl()
	{
		return UseGenericLinkLabel;
	}

	void LinkLabel::SetUseGenericControl(bool value)
	{
		UseGenericLinkLabel = value;
	}

	LinkLabel::LinkLabel()
	{
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
		return wxStr(GetStaticText()->GetLabel());
	}

	void LinkLabel::SetText(const string& value)
	{
		GetStaticText()->SetLabel(wxStr(value));
	}

	string LinkLabel::GetUrl() 
	{
		return wxStr(GetStaticText()->GetURL());
	}

	void LinkLabel::SetUrl(const string& value)
	{
		GetStaticText()->SetURL(wxStr(value)); 
	}

	class wxHyperlinkCtrl2 : public wxHyperlinkCtrl, public wxWidgetExtender
	{
	public:
		wxHyperlinkCtrl2(){}
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

	class wxGenericHyperlinkCtrl2 : public wxGenericHyperlinkCtrl, public wxWidgetExtender
	{
	public:
		wxGenericHyperlinkCtrl2(){}
		wxGenericHyperlinkCtrl2(wxWindow* parent,
			wxWindowID id,
			const wxString& label, const wxString& url,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxHL_DEFAULT_STYLE,
			const wxString& name = wxASCII_STR(wxHyperlinkCtrlNameStr))
		{
			Create(parent, id, label, url, pos, size, style, name);
		}
	};

	wxWindow* LinkLabel::CreateWxWindowUnparented()
	{
		if (UseGenericLinkLabel)
		{
			return new wxGenericHyperlinkCtrl2();
		}
		else
		{
			return new wxHyperlinkCtrl2();
		}
	}

	wxWindow* LinkLabel::CreateWxWindowCore(wxWindow* parent)
	{
		wxWindow* staticText;

		// Text must be " " (space) to avoid exception on Linux
		if (UseGenericLinkLabel)
		{
			staticText = new wxGenericHyperlinkCtrl2(
				parent, wxID_ANY, " ", wxEmptyString);
		}
		else
		{
			staticText = new wxHyperlinkCtrl2(
				parent, wxID_ANY, " ", wxEmptyString);
		}

		staticText->Bind(wxEVT_HYPERLINK,
			&LinkLabel::OnHyperlinkClick, this);
		return staticText;
	}

	wxHyperlinkCtrlBase* LinkLabel::GetStaticText()
	{
		return dynamic_cast<wxHyperlinkCtrlBase*>(GetWxWindow());
	}

	Color LinkLabel::GetHoverColor()
	{
		return GetStaticText()->GetHoverColour();
	}

	void LinkLabel::SetHoverColor(const Color& value)
	{
		GetStaticText()->SetHoverColour(value);
	}

	Color LinkLabel::GetNormalColor()
	{
		return GetStaticText()->GetNormalColour();
	}

	void LinkLabel::SetNormalColor(const Color& value)
	{
		GetStaticText()->SetNormalColour(value);
	}

	Color LinkLabel::GetVisitedColor()
	{
		return GetStaticText()->GetVisitedColour();
	}

	void LinkLabel::SetVisitedColor(const Color& value)
	{
		GetStaticText()->SetVisitedColour(value);
	}

	bool LinkLabel::GetVisited()
	{
		return GetStaticText()->GetVisited();
	}

	void LinkLabel::SetVisited(bool value)
	{
		GetStaticText()->SetVisited(value);
	}
}
