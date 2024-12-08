#include "VListBox.h"

namespace Alternet::UI
{
    void VListBox::ProcessScrollEvent(wxScrollWinEvent& event)
    {
        wxEventType evType = event.GetEventType();

        if (evType == wxEVT_SCROLLWIN_TOP)
        {
            OnScrollTop(event);
            return;
        }

        if (evType == wxEVT_SCROLLWIN_BOTTOM)
        {
            OnScrollBottom(event);
            return;
        }

        if (evType == wxEVT_SCROLLWIN_LINEUP)
        {
            OnScrollLineUp(event);
            return;
        }
        
        if (evType == wxEVT_SCROLLWIN_LINEDOWN)
        {
            OnScrollLineDown(event);
            return;
        }
        
        if (evType == wxEVT_SCROLLWIN_PAGEUP)
        {
            OnScrollPageUp(event);
            return;
        }
        
        if (evType == wxEVT_SCROLLWIN_PAGEDOWN)
        {
            OnScrollPageDown(event);
            return;
        }
        
        if (evType == wxEVT_SCROLLWIN_THUMBTRACK)
        {
            OnScrollThumbTrack(event);
            return;
        }
        
        if (evType == wxEVT_SCROLLWIN_THUMBRELEASE)
        {
            OnScrollThumbRelease(event);
            return;
        }     
    }

    bool VListBox::GetVScrollBarVisible()
    {
        return _vScrollBarVisible;
    }

    void VListBox::SetVScrollBarVisible(bool value)
    {
        if (_vScrollBarVisible == value)
            return;
        _vScrollBarVisible = value;
        RecreateWindow();
    }

    bool VListBox::GetHScrollBarVisible()
    {
        return _hScrollBarVisible;
    }

    void VListBox::SetHScrollBarVisible(bool value)
    {
        if (_hScrollBarVisible == value)
            return;
        _hScrollBarVisible = value;
        RecreateWindow();
    }

    RectI VListBox::GetItemRectI(int index)
    {
        return GetListBox()->GetItemRect(index);
    }

    void* VListBox::CreateEx(int64_t styles)
    {
        return new VListBox(styles);
    }

    bool VListBox::IsSelected(int line)
    {
        return GetListBox()->IsSelected(line);
    }

    VListBox::VListBox(int64_t styles)
    {
        bindScrollEvents = false;
    }

    void* VListBox::GetEventDcHandle()
    {
        return eventDc->GetHandle();
    }

    DrawingContext* VListBox::GetEventDc()
    {
        return eventDc;
    }

    int VListBox::GetEventHeight()
    {
        return eventItemHeight;
    }

    void VListBox::SetEventHeight(int value)
    {
        eventItemHeight = value;
    }

    RectI VListBox::GetEventRect()
    {
        return eventRect;
    }

    int VListBox::GetEventItem()
    {
        return eventItemIndex;
    }

    VListBox::VListBox()
	{
        bindScrollEvents = false;
	}

	VListBox::~VListBox()
	{
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_LISTBOX, &VListBox::OnSelectionChanged, this);
            }
        }
    }

    bool VListBox::GetHasBorder()
    {
        return hasBorder;
    }

    void VListBox::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWindow();
    }

    void VListBox::ClearItems()
    {
        GetListBox()->Clear();
    }

    wxWindow* VListBox::CreateWxWindowUnparented()
    {
        return new wxVListBox2();
    }

    wxWindow* VListBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetSelectionStyle() | GetBorderStyle();

        style = BuildStyle(style, wxBORDER_NONE, !hasBorder);
        style = BuildStyle(style, wxHSCROLL, _hScrollBarVisible);
        style = BuildStyle(style, wxVSCROLL, _vScrollBarVisible);

        auto value = new wxVListBox2(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        value->SetDoubleBuffered(true);

        value->Bind(wxEVT_LISTBOX, &VListBox::OnSelectionChanged, this);

        return value;
    }

    Size VListBox::GetPreferredSize(const Size& availableSize)
    {
        auto size = Control::GetPreferredSize(availableSize);

#ifdef __WXOSX_COCOA__
        // Hacky workaround to fix macOS ListBox measurement.
        size.Width += 4;
        size.Height += 4;
#endif

        return size;
    }

    void VListBox::SetItemsCount(int value)
    {
        if (_itemsCount == value)
            return;
        _itemsCount = value;
        GetListBox()->SetItemCount(value);
    }

    int VListBox::GetItemsCount()
    {
        return _itemsCount;
    }

    bool VListBox::IsCurrent(int current)
    {
        return GetListBox()->IsCurrent(current);
    }

    bool VListBox::DoSetCurrent(int current)
    {
        return GetListBox()->SetCurrent(current);
    }

    long VListBox::GetSelectionStyle()
    {
        switch (_selectionMode)
        {
        case ListBoxSelectionMode::Single:
        default:
            return wxLB_SINGLE;
        case ListBoxSelectionMode::Multiple:
            return wxLB_MULTIPLE;
        }
    }

    ListBoxSelectionMode VListBox::GetSelectionMode()
    {
        return _selectionMode;
    }

    void VListBox::SetSelectionMode(ListBoxSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        _selectionMode = value;
        RecreateWxWindowIfNeeded();
    }

    void VListBox::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        GetListBox()->SetItemCount(_itemsCount);
    }

    wxVListBox2* VListBox::GetListBox()
    {
        return dynamic_cast<wxVListBox2*>(GetWxWindow());
    }

    void VListBox::OnSelectionChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(VListBoxEvent::SelectionChanged);
    }

    void VListBox::OnDrawItem(DrawingContext* dc, const wxRect& rect, size_t n)
    {
        eventDc = dc;
        eventRect = rect;
        eventItemIndex = n;
        RaiseEvent(VListBoxEvent::DrawItem);
        eventDc = nullptr;
    }

    wxCoord VListBox::OnMeasureItem(size_t n)
    {
        eventItemIndex = n;
        eventItemHeight = 26;
        RaiseEvent(VListBoxEvent::MeasureItem);
        return eventItemHeight;
    }

    void wxVListBox2::OnDrawBackground(wxDC& dc, const wxRect& rect, size_t n) const
    {
        /*wxVListBox::OnDrawBackground(dc, rect, n);*/
    }

    int VListBox::GetVisibleEnd()
    {
        return GetListBox()->GetVisibleEnd();
    }

    int VListBox::GetVisibleBegin()
    {
        return GetListBox()->GetVisibleBegin();
    }

    int VListBox::GetRowHeight(int line)
    {
        return GetListBox()->OnMeasureItem(line);
    }

    void VListBox::OnPaint(wxPaintEvent& event)
    {
        event.Skip(false);
        Control::RaiseEvent(ControlEvent::Paint);
    }

    // the derived class must implement this function to actually draw the item
    // with the given index on the provided DC
    void wxVListBox2::OnDrawItem(wxDC& dc, const wxRect& rect, size_t n) const
    {
    }

    void wxVListBox2::OnDrawItem(DrawingContext* dc, const wxRect& rect, size_t n) const
    {
        ((VListBox*)_palControl)->OnDrawItem(dc, rect, n);
    }

    // the derived class must implement this method to return the height of the
    // specified item
    wxCoord wxVListBox2::OnMeasureItem(size_t n) const
    {
        return ((VListBox*)_palControl)->OnMeasureItem(n);
    }

    void wxVListBox2::ProcessScrollEvent(wxScrollWinEvent& event)
    {
        if (event.GetOrientation() == wxVERTICAL)
            return;
        ((VListBox*)_palControl)->ProcessScrollEvent(event);
    }

    int VListBox::GetFirstSelected()
    {
        return GetListBox()->GetFirstSelected(selectedCookie);
    }

    int VListBox::GetNextSelected()
    {
        return GetListBox()->GetNextSelected(selectedCookie);
    }

    int VListBox::GetSelectedCount()
    {
        return GetListBox()->GetSelectedCount();
    }

    int VListBox::GetSelection()
    {
        return GetListBox()->GetSelection();
    }

    void VListBox::ClearSelected()
    {
        auto listBox = GetListBox();

        if(_selectionMode == ListBoxSelectionMode::Multiple)
            listBox->DeselectAll();
        else
        {
            listBox->SetSelection(-1);
        }
    }

    void VListBox::SetSelectionBackground(const Color& color)
    {
        GetListBox()->SetSelectionBackground(color);
    }

    void VListBox::SetSelection(int selection)
    {
        GetListBox()->SetSelection(selection);
    }

    void VListBox::SetSelected(int index, bool value)
    {
        auto listBox = GetListBox();

        if (_selectionMode == ListBoxSelectionMode::Multiple)
            listBox->Select(index, value);
        else
        {
            if(value)
                listBox->SetSelection(index);
            else
            {
                if (listBox->GetSelection() == index)
                    listBox->SetSelection(-1);
            }
        }
    }

    bool VListBox::ScrollRows(int rows)
    {
        return GetListBox()->ScrollRows(rows);
    }

    bool VListBox::ScrollRowPages(int pages)
    {
        return GetListBox()->ScrollRowPages(pages);
    }

    void VListBox::RefreshRow(int row)
    {
        GetListBox()->RefreshRow(row);
    }

    bool VListBox::ScrollToRow(int row)
    {
        return GetListBox()->ScrollToRow(row);
    }

    void VListBox::RefreshRows(int from, int to)
    {
        GetListBox()->RefreshRows(from, to);
    }

    bool VListBox::IsVisible(int line)
    {
        return GetListBox()->IsRowVisible(line);
    }

    void VListBox::EnsureVisible(int itemIndex)
    {
        GetListBox()->ScrollToRow(itemIndex);
    }

    int VListBox::ItemHitTest(const PointD& position)
    {
        auto listBox = GetListBox();
        auto y = fromDip(position.Y, listBox);
        auto result = listBox->VirtualHitTest(y);
        return result;
    }
}
