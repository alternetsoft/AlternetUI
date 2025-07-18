#include "Label.h"

namespace Alternet::UI
{
	//https://docs.wxwidgets.org/3.2/classwx_static_text.html

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

	class wxStaticText2 : public wxStaticText, public wxWidgetExtender
	{
	public:
		wxStaticText2(){}
		wxStaticText2(wxWindow* parent,
			wxWindowID id,
			const wxString& label,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = 0,
			const wxString& name = wxASCII_STR(wxStaticTextNameStr))
		{
			Create(parent, id, label, pos, size, style, name);
		}

		void OnEraseBackGround(wxEraseEvent& event) {};
		DECLARE_EVENT_TABLE()
	};

	BEGIN_EVENT_TABLE(wxStaticText2, wxStaticText)
		EVT_ERASE_BACKGROUND(wxStaticText2::OnEraseBackGround)
		END_EVENT_TABLE()

	wxWindow* Label::CreateWxWindowUnparented()
	{
		return new wxStaticText2();
	}

	wxWindow* Label::CreateWxWindowCore(wxWindow* parent)
	{
		long style = 0;
		auto staticText = new wxStaticText2(
			parent, wxID_ANY, wxEmptyString, wxDefaultPosition, wxDefaultSize, style);
		return staticText;
	}
/*

enum wxAlignment
{
	wxALIGN_INVALID = -1,

		wxALIGN_NOT = 0x0000,
		wxALIGN_CENTER_HORIZONTAL = 0x0100,
		wxALIGN_CENTRE_HORIZONTAL = wxALIGN_CENTER_HORIZONTAL,
		wxALIGN_LEFT = wxALIGN_NOT,
		wxALIGN_TOP = wxALIGN_NOT,
		wxALIGN_RIGHT = 0x0200,
		wxALIGN_BOTTOM = 0x0400,
		wxALIGN_CENTER_VERTICAL = 0x0800,
		wxALIGN_CENTRE_VERTICAL = wxALIGN_CENTER_VERTICAL,

		wxALIGN_CENTER = (wxALIGN_CENTER_HORIZONTAL | wxALIGN_CENTER_VERTICAL),
		wxALIGN_CENTRE = wxALIGN_CENTER,

		wxALIGN_MASK = 0x0f00
};

#define wxST_NO_AUTORESIZE         0x0001
// free 0x0002 bit
#define wxST_ELLIPSIZE_START       0x0004
#define wxST_ELLIPSIZE_MIDDLE      0x0008
#define wxST_ELLIPSIZE_END         0x0010


wxALIGN_LEFT:
Align the text to the left.
wxALIGN_RIGHT:
Align the text to the right.
wxALIGN_CENTRE_HORIZONTAL:
Center the text (horizontally).

wxST_NO_AUTORESIZE:
By default, the control will adjust its size to exactly fit to the size of the text when SetLabel() is called. If this style flag is given, the control will not change its size (this style is especially useful with controls which also have the wxALIGN_RIGHT or the wxALIGN_CENTRE_HORIZONTAL style because otherwise they won't make sense any longer after a call to SetLabel()).
--------
wxST_ELLIPSIZE_START:
If the labeltext width exceeds the control width, replace the beginning of the label with an ellipsis; uses wxControl::Ellipsize.
wxST_ELLIPSIZE_MIDDLE:
If the label text width exceeds the control width, replace the middle of the label with an ellipsis; uses wxControl::Ellipsize.
wxST_ELLIPSIZE_END:
If the label text width exceeds the control width, replace the end of the label with an ellipsis; uses wxControl::Ellipsize.
-------
void wxStaticText::Wrap	(	int 	width	)
This functions wraps the controls label so that each of its lines becomes at most width pixels wide if possible (the lines are broken at words boundaries so it might not be the case if words are too long).

If width is negative, no wrapping is done. Note that this width is not necessarily the total width of the control, since a few pixels for the border (depending on the controls border style) may be added.
*/

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
