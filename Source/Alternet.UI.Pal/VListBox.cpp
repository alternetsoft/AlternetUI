#include "VListBox.h"

namespace Alternet::UI
{
    void* VListBox::CreateEx(int64_t styles)
    {
        return new VListBox(styles);
    }

    VListBox::VListBox(int64_t styles)
    {
        bindScrollEvents = false;
    }

    void* VListBox::GetEventDc()
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

        if (!hasBorder)
            style = style | wxBORDER_NONE;

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

    void VListBox::ClearSelected()
    {
        /*auto listBox = GetListBox();
            
        if(_selectionMode == ListBoxSelectionMode::Multiple)
            listBox->DeselectAll();
        else
        {
            listBox->SetSelection(-1);
        }*/
    }

    void VListBox::SetItemsCount(int value)
    {
        return GetListBox()->SetItemCount(value);
    }

    int VListBox::GetItemsCount()
    {
        return GetListBox()->GetItemCount();
    }

    long VListBox::GetSelectionStyle()
    {
        switch (_selectionMode)
        {
        case ListBoxSelectionMode::Single:
        default:
            return wxLB_SINGLE;
        case ListBoxSelectionMode::Multiple:
            return wxLB_EXTENDED;
        }
    }

    void VListBox::SetSelected(int index, bool value)
    {
        /*auto listBox = GetListBox();

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
        }*/
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

    void VListBox::UpdateDc(wxDC& dc)
    {
        eventDc = std::addressof(dc);
    }

    void VListBox::OnDrawItem(wxDC& dc, const wxRect& rect, size_t n)
    {
        UpdateDc(dc);
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
        wxVListBox::OnDrawBackground(dc, rect, n);
    }

    // the derived class must implement this function to actually draw the item
    // with the given index on the provided DC
    void wxVListBox2::OnDrawItem(wxDC& dc, const wxRect& rect, size_t n) const
    {
        ((VListBox*)_palControl)->OnDrawItem(dc, rect, n);
    }

    // the derived class must implement this method to return the height of the
    // specified item
    wxCoord wxVListBox2::OnMeasureItem(size_t n) const
    {
        return ((VListBox*)_palControl)->OnMeasureItem(n);
    }
}
