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

	int ScrollBar::GetEventTypeID()
	{
		return _eventType;
	}

	int ScrollBar::GetEventOldPos()
	{
		return _eventPos;
	}
	
	int ScrollBar::GetEventNewPos()
	{
		return _eventPos;
	}

	void ScrollBar::OnScrollInternal(ScrollEventType evType, wxScrollEvent& event)
	{
		event.Skip();
		_eventType = evType;
		_eventPos = event.GetPosition();
		RaiseEvent(ScrollBarEvent::Scroll);
	}

	void ScrollBar::OnEventScrollTop(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::First, event);
	}

	void ScrollBar::OnEventScrollBottom(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::Last, event);
	}

	void ScrollBar::OnEventScrollLineUp(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::SmallDecrement, event);
	}

	void ScrollBar::OnEventScrollLineDown(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::SmallIncrement, event);
	}

	void ScrollBar::OnEventScrollPageUp(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::LargeDecrement, event);
	}

	void ScrollBar::OnEventScrollPageDown(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::LargeIncrement, event);
	}

	void ScrollBar::OnEventScrollThumbTrack(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::ThumbTrack, event);
	}

	void ScrollBar::OnEventScrollThumbRelease(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::ThumbPosition, event);
	}

	void ScrollBar::OnEventScrollChanged(wxScrollEvent& event)
	{
		OnScrollInternal(ScrollEventType::EndScroll, event);
	}

	ScrollBar::~ScrollBar()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				window->Unbind(wxEVT_SCROLL_TOP, &ScrollBar::OnEventScrollTop, this);
				window->Unbind(wxEVT_SCROLL_BOTTOM, &ScrollBar::OnEventScrollBottom, this);
				window->Unbind(wxEVT_SCROLL_LINEUP, &ScrollBar::OnEventScrollLineUp, this);
				window->Unbind(wxEVT_SCROLL_LINEDOWN, &ScrollBar::OnEventScrollLineDown, this);
				window->Unbind(wxEVT_SCROLL_PAGEUP, &ScrollBar::OnEventScrollPageUp, this);
				window->Unbind(wxEVT_SCROLL_PAGEDOWN, &ScrollBar::OnEventScrollPageDown, this);
				window->Unbind(wxEVT_SCROLL_THUMBTRACK, &ScrollBar::OnEventScrollThumbTrack, this);
				window->Unbind(wxEVT_SCROLL_THUMBRELEASE, &ScrollBar::OnEventScrollThumbRelease, this);
				window->Unbind(wxEVT_SCROLL_CHANGED, &ScrollBar::OnEventScrollChanged, this);
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

		window->Bind(wxEVT_SCROLL_TOP, &ScrollBar::OnEventScrollTop, this);
		window->Bind(wxEVT_SCROLL_BOTTOM, &ScrollBar::OnEventScrollBottom, this);
		window->Bind(wxEVT_SCROLL_LINEUP, &ScrollBar::OnEventScrollLineUp, this);
		window->Bind(wxEVT_SCROLL_LINEDOWN, &ScrollBar::OnEventScrollLineDown, this);
		window->Bind(wxEVT_SCROLL_PAGEUP, &ScrollBar::OnEventScrollPageUp, this);
		window->Bind(wxEVT_SCROLL_PAGEDOWN, &ScrollBar::OnEventScrollPageDown, this);
		window->Bind(wxEVT_SCROLL_THUMBTRACK, &ScrollBar::OnEventScrollThumbTrack, this);
		window->Bind(wxEVT_SCROLL_THUMBRELEASE, &ScrollBar::OnEventScrollThumbRelease, this);
		window->Bind(wxEVT_SCROLL_CHANGED, &ScrollBar::OnEventScrollChanged, this);
        return window;
    }

    wxScrollBarBase* ScrollBar::GetScrollBar()
    {
        return dynamic_cast<wxScrollBarBase*>(GetWxWindow());
    }
}
