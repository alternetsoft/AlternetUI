#include "ScrollBar.h"

namespace Alternet::UI
{

	class wxScrollBar2 : public wxScrollBar, public wxWidgetExtender
	{
	public:
		wxScrollBar2() {}
		wxScrollBar2(wxWindow* parent, wxWindowID id,
			const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxSB_HORIZONTAL,
			const wxValidator& validator = wxDefaultValidator,
			const wxString& name = wxASCII_STR(wxScrollBarNameStr))
			: wxScrollBar(parent, id, pos, size, style, validator, name)
		{
		}
	};

	int ScrollBar::GetThumbPosition()
	{
		return GetScrollBar()->GetThumbPosition();
	}

	void ScrollBar::SetThumbPosition(int value)
	{
		GetScrollBar()->SetThumbPosition(value);
	}

	int ScrollBar::GetRange()
	{
		return GetScrollBar()->GetRange();
	}

	int ScrollBar::GetThumbSize()
	{
		return GetScrollBar()->GetThumbSize();
	}

	int ScrollBar::GetPageSize()
	{
		return GetScrollBar()->GetPageSize();
	}

	void ScrollBar::SetScrollbar(int position, int thumbSize, int range, int pageSize, bool refresh)
	{
		GetScrollBar()->SetScrollbar(position, thumbSize, range, pageSize, refresh);
	}

	bool ScrollBar::GetIsVertical()
	{
		return _isVertical;
	}

	void ScrollBar::SetIsVertical(bool value)
	{
		if (_isVertical == value)
			return;
		_isVertical = value;
		RecreateWxWindowIfNeeded();
	}
	
	ScrollBar::ScrollBar()
	{
	}
	
	ScrollBar::~ScrollBar()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				// window->Unbind(wxEVT_CALENDAR_WEEK_CLICKED, &Calendar::OnEventWeekNumberClick, this);
			}
		}
	}

    wxWindow* ScrollBar::CreateWxWindowUnparented()
    {
        return new wxScrollBar2();
    }

    wxWindow* ScrollBar::CreateWxWindowCore(wxWindow* parent)
    {
        long style = 0;

		if (_isVertical)
			style |= wxSB_VERTICAL;
		else
			style |= wxSB_HORIZONTAL;

        wxWindow* window;

		window = new wxScrollBar2(parent, wxID_ANY,
			wxDefaultPosition,
			wxDefaultSize,
			style,
			wxDefaultValidator,
			wxASCII_STR(wxScrollBarNameStr));

        //window->Bind(wxEVT_CALENDAR_DOUBLECLICKED, &Calendar::OnEventDoubleClick, this);
        return window;
    }

    wxScrollBarBase* ScrollBar::GetScrollBar()
    {
        return dynamic_cast<wxScrollBarBase*>(GetWxWindow());
    }
}
