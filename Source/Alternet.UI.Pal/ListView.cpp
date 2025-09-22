#include "ListView.h"

#ifdef __WXMSW__
#include <uxtheme.h>
#endif

namespace Alternet::UI
{
    ListView::ListView()
    {
        bindScrollEvents = false;
    }

    bool ListView::GetHasBorder()
    {
        return hasBorder;
    }

    void ListView::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    ListView::~ListView()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_LIST_ITEM_SELECTED, 
                    &ListView::OnItemSelected, this);
                window->Unbind(wxEVT_LIST_ITEM_DESELECTED, 
                    &ListView::OnItemDeselected, this);
                window->Unbind(wxEVT_LIST_COL_CLICK, 
                    &ListView::OnColumnHeaderClicked, this);
                window->Unbind(wxEVT_LIST_BEGIN_LABEL_EDIT, 
                    &ListView::OnBeginLabelEdit, this);
                window->Unbind(wxEVT_LIST_END_LABEL_EDIT, 
                    &ListView::OnEndLabelEdit, this);
            }
        }

        if (_smallImageList != nullptr)
        {
            _smallImageList->Release();
            _smallImageList = nullptr;
        }

        if (_largeImageList != nullptr)
        {
            _largeImageList->Release();
            _largeImageList = nullptr;
        }
    }

    void ListView::InsertColumnAt(int64_t index, const string& header, 
        Coord width, ListViewColumnWidthMode widthMode)
    {
        if (_view == ListViewView::Details)
        {
            auto listView = GetListView();
            if (index > listView->GetColumnCount())
                return;

            auto w = GetWxColumnWidth(width, widthMode);
            listView->InsertColumn(index, wxStr(header), 0, w);
        }
    }

    void ListView::RemoveColumnAt(int64_t index)
    {
        if (_view == ListViewView::Details)
        {
            auto listView = GetListView();

            if (index >= listView->GetColumnCount())
                return;
            listView->DeleteColumn(index);
        }
    }

    void ListView::InsertItemAt(int64_t index, const string& value, int64_t columnIndex,
        int imageIndex)
    {
        wxListItem item;
        item.SetText(wxStr(value));
        item.SetColumn(columnIndex);
        item.SetId(index);
        item.SetImage(imageIndex);
        item.SetMask(wxLIST_MASK_TEXT | wxLIST_MASK_IMAGE);
        InsertItem(GetListView(), item);
    }

    void ListView::InsertItem(wxListView2* listView, wxListItem& item)
    {
        auto col = item.GetColumn();

        #pragma warning(suppress: 4018)
        if (_view == ListViewView::Details && col >= listView->GetColumnCount() && col > 0)
            return;

        if (_view == ListViewView::Details || col == 0)
        {
            if (col > 0)
            {
                wxListItem i(item);
                listView->SetItem(i);
            }
            else
                listView->InsertItem(item);
        }
    }

    void ListView::RemoveItemAt(int64_t index)
    {
        GetListView()->DeleteItem(index);
    }

    void ListView::ClearItems()
    {
        GetListView()->DeleteAllItems();
    }

    ListViewView ListView::GetCurrentView()
    {
        return _view;
    }

    void ListView::RecreateWxWindowIfNeeded()
    {
        if (_ignoreRecreate)
            return;
        Control::RecreateWxWindowIfNeeded();
        RaiseEvent(ListViewEvent::ControlRecreated);
    }

    void ListView::SetCurrentView(ListViewView value)
    {
        if (_view == value)
            return;
        _view = value;
        RecreateWxWindowIfNeeded();
    }

    ImageList* ListView::GetSmallImageList()
    {
        if (_smallImageList != nullptr)
            _smallImageList->AddRef();
        return _smallImageList;
    }

    void ListView::SetSmallImageList(ImageList* value)
    {
        if (_smallImageList != nullptr)
            _smallImageList->Release();
        _smallImageList = value;
        if (_smallImageList != nullptr)
            _smallImageList->AddRef();
        ApplySmallImageList(GetListView());
    }

    ImageList* ListView::GetLargeImageList()
    {
        if (_largeImageList != nullptr)
            _largeImageList->AddRef();
        return _largeImageList;
    }

    void ListView::SetLargeImageList(ImageList* value)
    {
        if (_largeImageList != nullptr)
            _largeImageList->Release();
        _largeImageList = value;
        if (_largeImageList != nullptr)
            _largeImageList->AddRef();
        ApplyLargeImageList(GetListView());
    }

    wxWindow* ListView::CreateWxWindowUnparented()
    {
        return new wxListView2();
    }

    wxWindow* ListView::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetStyle();

        if (!hasBorder)
            style = style | wxBORDER_NONE;
        else
            style |= wxBORDER_THEME;

        auto value = new wxListView2(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

        /* This is commented out as in dark mode we have problems with this code
#ifdef __WXMSW__
        auto hWnd = (HWND)value->GetHWND();
        // turn off "explorer style" item hover effects.
        SetWindowTheme(hWnd, L"", NULL); 
#endif
*/

        ApplySmallImageList(value);
        ApplyLargeImageList(value);

        value->Bind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
        value->Bind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);
        value->Bind(wxEVT_LIST_COL_CLICK, &ListView::OnColumnHeaderClicked, this);
        value->Bind(wxEVT_LIST_BEGIN_LABEL_EDIT, &ListView::OnBeginLabelEdit, this);
        value->Bind(wxEVT_LIST_END_LABEL_EDIT, &ListView::OnEndLabelEdit, this);

        return value;
    }

    void ListView::OnItemSelected(wxListEvent& event)
    {
        event.Skip();
        RaiseSelectionChanged();
    }

    void ListView::OnItemDeselected(wxListEvent& event)
    {
        event.Skip();
        RaiseSelectionChanged();
    }

    void ListView::OnColumnHeaderClicked(wxListEvent& event)
    {
        event.Skip();
        ListViewColumnEventData data = { 0 };
        data.columnIndex = event.GetColumn();
        RaiseEvent(ListViewEvent::ColumnClick, &data);
    }

    void ListView::OnBeginLabelEdit(wxListEvent& event)
    {
        OnLabelEditEvent(event, ListViewEvent::BeforeItemLabelEdit);
    }

    void ListView::OnLabelEditEvent(wxListEvent& event, ListViewEvent e)
    {
        ListViewItemLabelEditEventData data{ 0 };

        data.editCancelled = event.IsEditCancelled();
        data.itemIndex = event.GetItem().m_itemId;
        auto label = wxStr(event.GetLabel());
        data.label = const_cast<char16_t*>(label.c_str());

        auto result = RaiseEventWithPointerResult(e, &data);

        if (result != 0)
            event.Veto();
        else
            event.Skip();
    }

    void ListView::OnEndLabelEdit(wxListEvent& event)
    {
        auto window = GetWxWindow();
        window->Refresh();
        window->Update();

        OnLabelEditEvent(event, ListViewEvent::AfterItemLabelEdit);
    }

    int ListView::GetWxColumnWidth(double width, ListViewColumnWidthMode widthMode)
    {
        switch (widthMode)
        {
        case ListViewColumnWidthMode::Fixed:
            return fromDip(width, GetListView());
        case ListViewColumnWidthMode::AutoSize:
        default:
            return wxLIST_AUTOSIZE;
        case ListViewColumnWidthMode::AutoSizeHeader:
            return wxLIST_AUTOSIZE_USEHEADER;
        }
    }

    void ListView::SetColumnWidth(int64_t columnIndex, Coord fixedWidth,
        ListViewColumnWidthMode widthMode)
    {
        if (_view != ListViewView::Details)
            return;

        auto listView = GetListView();
        int width = GetWxColumnWidth(fixedWidth, widthMode);
        listView->SetColumnWidth(columnIndex, width);
    }

    void ListView::SetColumnTitle(int64_t columnIndex, const string& title)
    {
        if (_view != ListViewView::Details)
            return;

        wxListItem item;
        item.SetMask(wxLIST_MASK_TEXT);
        item.SetText(wxStr(title));

        GetListView()->SetColumn(columnIndex, item);
    }

    void ListView::SetItemText(int64_t itemIndex, int64_t columnIndex, const string& text)
    {
        auto listView = GetListView();

        if (itemIndex >= listView->GetItemCount() || itemIndex < 0)
            return;

        if (_view == ListViewView::Details || columnIndex == 0)
        {
            if (columnIndex >= listView->GetColumnCount() && columnIndex > 0)
                return;

            wxListItem item;
            item.SetColumn(columnIndex);
            item.SetId(itemIndex);
            item.SetMask(wxLIST_MASK_TEXT);
            item.SetText(wxStr(text));
            listView->SetItem(item);
        }
    }

    void ListView::SetItemImageIndex(int64_t itemIndex,
        int64_t columnIndex, int imageIndex)
    {
        auto listView = GetListView();

        if (itemIndex >= listView->GetItemCount() || itemIndex < 0)
            return;

        if (_view == ListViewView::Details)
        {
            if (columnIndex >= listView->GetColumnCount() && columnIndex > 0)
                return;

            wxListItem item;
            item.SetId(itemIndex);
            item.SetColumn(columnIndex);
            item.SetImage(imageIndex);
            item.SetMask(wxLIST_MASK_IMAGE);
            listView->SetItem(item);
        }
    }

    void ListView::SetFocusedItemIndex(int64_t value)
    {
        GetListView()->Focus(value);
    }

    bool ListView::GetAllowLabelEdit()
    {
        return _allowLabelEdit;
    }

    void ListView::SetAllowLabelEdit(bool value)
    {
        if (_allowLabelEdit == value)
            return;
        _allowLabelEdit = value;
        RecreateWxWindowIfNeeded();
    }

    int64_t ListView::GetTopItemIndex()
    {
        return GetListView()->GetTopItem();
    }

    ListViewGridLinesDisplayMode ListView::GetGridLinesDisplayMode()
    {
        return _gridLinesDisplayMode;
    }

    void ListView::SetGridLinesDisplayMode(ListViewGridLinesDisplayMode value)
    {
        if (_gridLinesDisplayMode == value)
            return;
        _gridLinesDisplayMode = value;
        RecreateWxWindowIfNeeded();
    }

    ListViewSortMode ListView::GetSortMode()
    {
        return _sortMode;
    }

    void ListView::SetSortMode(ListViewSortMode value)
    {
        if (_sortMode == value)
            return;

        _sortMode = value;
        RecreateWxWindowIfNeeded();
    }

    bool ListView::GetColumnHeaderVisible()
    {
        return _columnHeaderVisible;
    }

    void ListView::SetColumnHeaderVisible(bool value)
    {
        if (_columnHeaderVisible == value)
            return;
        _columnHeaderVisible = value;
        RecreateWxWindowIfNeeded();
    }

    int64_t ListView::GetFocusedItemIndex()
    {
        return GetListView()->GetFocusedItem();
    }

    void* ListView::ItemHitTest(const Point& point)
    {
        auto listView = GetListView();
        int flags = 0;
        long subItemIndex = 0;
        auto itemIndex = listView->HitTest(fromDip(point, listView),
            flags, &subItemIndex);

        auto result = new HitTestResult();
        result->itemIndex = itemIndex;
        result->columnIndex = subItemIndex;
        result->locations = GetHitTestLocationsFromWxFlags(flags);
        return result;
    }

    /*static*/ ListViewHitTestLocations ListView::GetHitTestLocationsFromWxFlags(int flags)
    {
        ListViewHitTestLocations result = (ListViewHitTestLocations)0;

        if ((flags & wxLIST_HITTEST_ABOVE) != 0) 
            result |= ListViewHitTestLocations::AboveClientArea;
        if ((flags & wxLIST_HITTEST_BELOW) != 0) 
            result |= ListViewHitTestLocations::BelowClientArea;
        if ((flags & wxLIST_HITTEST_NOWHERE) != 0) 
            result |= ListViewHitTestLocations::None;
        if ((flags & wxLIST_HITTEST_ONITEMICON) != 0) 
            result |= ListViewHitTestLocations::ItemImage;
        if ((flags & wxLIST_HITTEST_ONITEMLABEL) != 0) 
            result |= ListViewHitTestLocations::ItemLabel;
        if ((flags & wxLIST_HITTEST_ONITEMRIGHT) != 0) 
            result |= ListViewHitTestLocations::RightOfItem;
        if ((flags & wxLIST_HITTEST_TOLEFT) != 0) 
            result |= ListViewHitTestLocations::LeftOfClientArea;
        if ((flags & wxLIST_HITTEST_TORIGHT) != 0) 
            result |= ListViewHitTestLocations::RightOfClientArea;

        if (result == (ListViewHitTestLocations)0)
            result = ListViewHitTestLocations::None;

        return result;
    }

    ListViewHitTestLocations ListView::GetHitTestResultLocations(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->locations;
    }

    int64_t ListView::GetHitTestResultItemIndex(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->itemIndex;
    }

    int64_t ListView::GetHitTestResultColumnIndex(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->columnIndex;
    }

    void ListView::FreeHitTestResult(void* hitTestResult)
    {
        delete (HitTestResult*)hitTestResult;
    }

    void ListView::BeginLabelEdit(int64_t itemIndex)
    {
        GetListView()->EditLabel(itemIndex);
    }

    Rect ListView::GetItemBounds(int64_t itemIndex, ListViewItemBoundsPortion portion)
    {
        auto getWxPortionCode = [&]()
        {
            switch (portion)
            {
            case ListViewItemBoundsPortion::EntireItem:
                return wxLIST_RECT_BOUNDS;
            case ListViewItemBoundsPortion::Icon:
                return wxLIST_RECT_ICON;
            case ListViewItemBoundsPortion::Label:
                return wxLIST_RECT_LABEL;
            default:
                throwExNoInfo;
            }
        };

        auto listView = GetListView();
        wxRect rect;
        if (!listView->GetItemRect(itemIndex, rect, getWxPortionCode()))
            return Rect();
        return toDip(rect, listView);
    }

    void ListView::Clear()
    {
        GetListView()->ClearAll();
    }

    void ListView::EnsureItemVisible(int64_t itemIndex)
    {
        GetListView()->EnsureVisible(itemIndex);
    }

    void* ListView::OpenSelectedIndicesArray()
    {
        auto array = new std::vector<int64_t>();
        auto listView = GetListView();
        array->resize(listView->GetSelectedItemCount());
        int64_t index = listView->GetFirstSelected();
        int64_t i = 0;
        while (index != -1)
        {
            (*array)[i++] = index;
            index = listView->GetNextSelected(index);
        }
        return array;
    }

    void ListView::CloseSelectedIndicesArray(void* array)
    {
        delete ((std::vector<int64_t>*)array);
    }

    int ListView::GetSelectedIndicesItemCount(void* array)
    {
        return ((std::vector<int64_t>*)array)->size();
    }

    int64_t ListView::GetSelectedIndicesItemAt(void* array, int index)
    {
        return (*((std::vector<int64_t>*)array))[index];
    }

    void ListView::ClearSelected()
    {
        DeselectAll(GetListView());
    }

    void ListView::DeselectAll(wxListView2* listView)
    {
        auto selectedIndices = (std::vector<int64_t>*)OpenSelectedIndicesArray();
        for (auto index : *selectedIndices)
            listView->Select(index, false);
        delete selectedIndices;
    }

    void ListView::SetSelected(int64_t index, bool value)
    {
        GetListView()->Select(index, value);
    }

    ListViewSelectionMode ListView::GetSelectionMode()
    {
        return _selectionMode;
    }

    void ListView::SetSelectionMode(ListViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;
        _selectionMode = value;
        RecreateWxWindowIfNeeded();
    }

    void ListView::ApplyLargeImageList(wxListView2* value)
    {
        value->SetImageList(_largeImageList == nullptr ? nullptr :
            _largeImageList->GetImageList(), wxIMAGE_LIST_NORMAL);
    }

    void ListView::ApplySmallImageList(wxListView2* value)
    {
        value->SetImageList(_smallImageList == nullptr ? nullptr :
            _smallImageList->GetImageList(), wxIMAGE_LIST_SMALL);
    }

    void ListView::OnSizeChanged(wxSizeEvent& event)
    {
        Control::OnSizeChanged(event);
    }

    void ListView::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        /*
        #ifdef __WXMSW__
                auto wnd = GetListView();
                auto hWnd = (HWND)wnd->GetHWND();

                wnd->SetDoubleBuffered(false);
                wnd->SetBackgroundStyle(wxBackgroundStyle::wxBG_STYLE_ERASE);

                DWORD dwStyle = SendMessage(hWnd, LVM_GETEXTENDEDLISTVIEWSTYLE, 0, 0);

                // Remove the LVS_EX_DOUBLEBUFFER flag
                dwStyle &= ~LVS_EX_DOUBLEBUFFER;

                SendMessage(hWnd, LVM_SETEXTENDEDLISTVIEWSTYLE, 0, dwStyle);

                RedrawWindow(hWnd, nullptr, nullptr, RDW_FRAME | RDW_INVALIDATE | RDW_UPDATENOW);
                SetWindowPos(hWnd, nullptr, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOZORDER);
                SendMessage(hWnd, WM_NCPAINT, 0, 0);
                SendMessage(hWnd, WM_PAINT, 0, 0);
        #endif
        */
    }

    int64_t ListView::GetItemsCount()
    {
        return GetListView()->GetItemCount();
    }

    wxListView2* ListView::GetListView()
    {
        return dynamic_cast<wxListView2*>(GetWxWindow());
    }

    long ListView::GetStyle()
    {
        auto getViewStyle = [&]()
        {
            switch (_view)
            {
            case ListViewView::List:
            default:
                return wxLC_LIST;
            case ListViewView::Details:
                return wxLC_REPORT;
            case ListViewView::SmallIcon:
                return wxLC_SMALL_ICON;
            case ListViewView::LargeIcon:
                return wxLC_ICON | wxLC_AUTOARRANGE;
            }
        };

        auto getSelectionStyle = [&]()
        {
            switch (_selectionMode)
            {
            case ListViewSelectionMode::Single:
            default:
                return wxLC_SINGLE_SEL;
            case ListViewSelectionMode::Multiple:
                return 0;
            }
        };

        auto getGridLinesStyle = [&]()
        {
            if (_view != ListViewView::Details)
            {
                return 0;
            }

            switch (_gridLinesDisplayMode)
            {
            case ListViewGridLinesDisplayMode::None:
#ifdef __WXMSW__
                return wxLC_VRULES;
#else
                return 0;
#endif
            case ListViewGridLinesDisplayMode::Horizontal:
                return wxLC_HRULES;
            default:
            case ListViewGridLinesDisplayMode::Vertical:
                return wxLC_VRULES;
            case ListViewGridLinesDisplayMode::VerticalAndHorizontal:
                return wxLC_HRULES | wxLC_VRULES;
            }
        };

        auto getSortStyle = [&]()
        {
            switch (_sortMode)
            {
            case ListViewSortMode::None:
            default:
                return 0;
            case ListViewSortMode::Ascending:
                return wxLC_SORT_ASCENDING;
            case ListViewSortMode::Descending:
                return wxLC_SORT_DESCENDING;
            case ListViewSortMode::Custom:
                return 0;
            }
        };

        auto style = getViewStyle() | getSelectionStyle() | getGridLinesStyle() | getSortStyle();

        style |= (_allowLabelEdit ? wxLC_EDIT_LABELS : 0);
        style |= (_columnHeaderVisible ? 0 : wxLC_NO_HEADER);

        return style;
    }

    std::vector<int64_t> ListView::GetSelectedIndices()
    {
        auto array = OpenSelectedIndicesArray();
        int count = GetSelectedIndicesItemCount(array);

        std::vector<int64_t> indices(count);
        for (int i = 0; i < count; i++)
            indices[i] = GetSelectedIndicesItemAt(array, i);

        CloseSelectedIndicesArray(array);

        return indices;
    }

    void ListView::SetSelectedIndices(const std::vector<int64_t>& value)
    {
        ClearSelected();
        for (auto index : value)
            SetSelected(index, true);
    }

    void ListView::RaiseSelectionChanged()
    {
        RaiseEvent(ListViewEvent::SelectionChanged);
    }
}
