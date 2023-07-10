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

    ListView::~ListView()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
                window->Unbind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);
                window->Unbind(wxEVT_LIST_COL_CLICK, &ListView::OnColumnHeaderClicked, this);
                window->Unbind(wxEVT_LIST_BEGIN_LABEL_EDIT, &ListView::OnBeginLabelEdit, this);
                window->Unbind(wxEVT_LIST_END_LABEL_EDIT, &ListView::OnEndLabelEdit, this);
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

    void ListView::InsertColumnAt(int index, const string& header, double width, ListViewColumnWidthMode widthMode)
    {
        /*Column column;
        column.column.SetText(wxStr(header));
        column.column.SetColumn(index);
        column.width = GetWxColumnWidth(width, widthMode);*/

        //_columns.emplace(_columns.begin() + index, column);

        if (IsWxWindowCreated() && _view == ListViewView::Details)
            GetListView()->InsertColumn(index, wxStr(header), 0,
                GetWxColumnWidth(width, widthMode));

            //InsertColumn(GetListView(), column);
    }

    void ListView::RemoveColumnAt(int index)
    {
        //_columns.erase(_columns.begin() + index);

        if (IsWxWindowCreated() && _view == ListViewView::Details)
            GetListView()->DeleteColumn(index);
    }

    void ListView::InsertItemAt(int index, const string& value, int columnIndex, 
        int imageIndex)
    {
        //Row& row = GetRow(index);
        //row.SetCell(columnIndex, item);

        if (IsWxWindowCreated()) 
        {
            wxListItem item;
            item.SetText(wxStr(value));
            item.SetColumn(columnIndex);
            item.SetId(index);
            item.SetImage(imageIndex);
            item.SetMask(wxLIST_MASK_TEXT | wxLIST_MASK_IMAGE);
            InsertItem(GetListView(), item);
        }
    }

    /*ListView::Row& ListView::GetRow(int index)
    {
        for (int i = _rows.size(); i <= index; i++)
            _rows.push_back(Row(index));

        return _rows[index];
    }*/

    void ListView::InsertItem(wxListView* listView, wxListItem& item)
    {
        auto lView = GetListView();

        #pragma warning(suppress: 4018)
        if (_view == ListViewView::Details && 
            item.GetColumn() >= lView->GetColumnCount())
            return;

        if (_view == ListViewView::Details || item.GetColumn() == 0)
        {
            if (item.GetColumn() > 0)
            {
                wxListItem i(item);
                lView->SetItem(i);
            }
            else
                lView->InsertItem(item);
        }
    }

    void ListView::RemoveItemAt(int index)
    {
        //_rows.erase(_rows.begin() + index);

        if (IsWxWindowCreated())
            GetListView()->DeleteItem(index);
    }

    void ListView::ClearItems()
    {
        //_rows.clear();

        if (IsWxWindowCreated())
            GetListView()->DeleteAllItems();
    }

    ListViewView ListView::GetCurrentView()
    {
        return _view;
    }

    void ListView::RecreateListView()
    {
        RecreateWxWindowIfNeeded();
        RaiseEvent(ListViewEvent::ControlRecreated);
    }

    void ListView::SetCurrentView(ListViewView value)
    {
        if (_view == value)
            return;

        //auto oldSelection = GetSelectedIndices();

        _view = value;
        RecreateListView();

        //SetSelectedIndices(oldSelection);
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
        if (IsWxWindowCreated())
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
        if (IsWxWindowCreated())
            ApplyLargeImageList(GetListView());
    }

    wxWindow* ListView::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetStyle() | GetBorderStyle();

        auto value = new wxListView(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            style);

#ifdef __WXMSW__
        SetWindowTheme((HWND)value->GetHWND(), L"", NULL); // turn off "explorer style" item hover effects.
#endif

        ApplySmallImageList(value);
        ApplyLargeImageList(value);

        value->Bind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
        value->Bind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);
        value->Bind(wxEVT_LIST_COL_CLICK, &ListView::OnColumnHeaderClicked, this);
        value->Bind(wxEVT_LIST_BEGIN_LABEL_EDIT, &ListView::OnBeginLabelEdit, this);
        value->Bind(wxEVT_LIST_END_LABEL_EDIT, &ListView::OnEndLabelEdit, this);

        return value;
    }

    void ListView::OnItemSelected(wxCommandEvent& event)
    {
        RaiseSelectionChanged();
    }

    void ListView::OnItemDeselected(wxCommandEvent& event)
    {
        RaiseSelectionChanged();
    }

    void ListView::OnColumnHeaderClicked(wxListEvent& event)
    {
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
            return wxLIST_AUTOSIZE;
        case ListViewColumnWidthMode::AutoSizeHeader:
            return wxLIST_AUTOSIZE_USEHEADER;
        default:
            throwExNoInfo;
        }
    }

    void ListView::SetColumnWidth(int columnIndex, double fixedWidth, ListViewColumnWidthMode widthMode)
    {
        if (_view != ListViewView::Details)
            return;

        auto listView = GetListView();

        int width = GetWxColumnWidth(fixedWidth, widthMode);

        listView->SetColumnWidth(columnIndex, width);

        /*auto column = _columns[columnIndex];
        column.width = width;
        _columns[columnIndex] = column;*/
    }

    void ListView::SetColumnTitle(int columnIndex, const string& title)
    {
        /*auto column = _columns[columnIndex];
        column.column.SetText(wxStr(title));
        _columns[columnIndex] = column;*/

            if (_view != ListViewView::Details)
                return;

            wxListItem item;
            item.SetMask(wxLIST_MASK_TEXT);
            item.SetText(wxStr(title));

            GetListView()->SetColumn(columnIndex, item);
    }

    void ListView::SetItemText(int itemIndex, int columnIndex, const string& text)
    {
        /*auto wxText = wxStr(text);
        wxListItem cell;
        auto row = _rows[itemIndex];
        row.GetCell(columnIndex, cell);
        cell.m_text = wxText;
        row.SetCell(columnIndex, cell);
        _rows[itemIndex] = row;*/

        if (_view == ListViewView::Details || columnIndex == 0)
        {
            wxListItem item;
            item.SetColumn(columnIndex);
            item.SetId(itemIndex);
            item.SetMask(wxLIST_MASK_TEXT);
            item.SetText(wxStr(text));

            auto listView = GetListView();
            listView->SetItem(item);
        }
    }

    void ListView::SetItemImageIndex(int itemIndex, int columnIndex, int imageIndex)
    {
        /*wxListItem cell;
        auto row = _rows[itemIndex];
        row.GetCell(columnIndex, cell);
        cell.m_image = imageIndex;
        row.SetCell(columnIndex, cell);
        _rows[itemIndex] = row;*/

        if (_view == ListViewView::Details) 
        {
            wxListItem item;
            item.SetId(itemIndex);
            item.SetColumn(columnIndex);
            item.SetImage(imageIndex);
            item.SetMask(wxLIST_MASK_IMAGE);

            GetListView()->SetItem(item);
        }
    }

    void ListView::SetFocusedItemIndex(int value)
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
        RecreateListView();
    }

    int ListView::GetTopItemIndex()
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
        RecreateListView();
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
        RecreateListView();
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
        RecreateListView();
    }

    int ListView::GetFocusedItemIndex()
    {
        return GetListView()->GetFocusedItem();
    }

    void* ListView::ItemHitTest(const Point& point)
    {
        auto listView = GetListView();
        int flags = 0;
        long subItemIndex = 0;
        auto itemIndex = listView->HitTest(fromDip(point, listView), flags, &subItemIndex);

        auto result = new HitTestResult();
        result->itemIndex = itemIndex;
        result->columnIndex = subItemIndex;
        result->locations = GetHitTestLocationsFromWxFlags(flags);
        return result;
    }

    /*static*/ ListViewHitTestLocations ListView::GetHitTestLocationsFromWxFlags(int flags)
    {
        ListViewHitTestLocations result = (ListViewHitTestLocations)0;

        if ((flags & wxLIST_HITTEST_ABOVE) != 0) result |= ListViewHitTestLocations::AboveClientArea;
        if ((flags & wxLIST_HITTEST_BELOW) != 0) result |= ListViewHitTestLocations::BelowClientArea;
        if ((flags & wxLIST_HITTEST_NOWHERE) != 0) result |= ListViewHitTestLocations::None;
        if ((flags & wxLIST_HITTEST_ONITEMICON) != 0) result |= ListViewHitTestLocations::ItemImage;
        if ((flags & wxLIST_HITTEST_ONITEMLABEL) != 0) result |= ListViewHitTestLocations::ItemLabel;
        if ((flags & wxLIST_HITTEST_ONITEMRIGHT) != 0) result |= ListViewHitTestLocations::RightOfItem;
        if ((flags & wxLIST_HITTEST_TOLEFT) != 0) result |= ListViewHitTestLocations::LeftOfClientArea;
        if ((flags & wxLIST_HITTEST_TORIGHT) != 0) result |= ListViewHitTestLocations::RightOfClientArea;

        if (result == (ListViewHitTestLocations)0)
            result = ListViewHitTestLocations::None;

        return result;
    }

    ListViewHitTestLocations ListView::GetHitTestResultLocations(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->locations;
    }

    int ListView::GetHitTestResultItemIndex(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->itemIndex;
    }

    int ListView::GetHitTestResultColumnIndex(void* hitTestResult)
    {
        return ((HitTestResult*)hitTestResult)->columnIndex;
    }

    void ListView::FreeHitTestResult(void* hitTestResult)
    {
        delete (HitTestResult*)hitTestResult;
    }

    void ListView::BeginLabelEdit(int itemIndex)
    {
        GetListView()->EditLabel(itemIndex);
    }

    Rect ListView::GetItemBounds(int itemIndex, ListViewItemBoundsPortion portion)
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
        //_rows.clear();
        //_columns.clear();
        GetListView()->ClearAll();
    }

    void ListView::EnsureItemVisible(int itemIndex)
    {
        GetListView()->EnsureVisible(itemIndex);
    }

    void* ListView::OpenSelectedIndicesArray()
    {
        auto array = new wxArrayInt();
        if (IsWxWindowCreated())
        {
            auto listView = GetListView();
            array->resize(listView->GetSelectedItemCount());
            int index = listView->GetFirstSelected();
            int i = 0;
            while (index != -1)
            {
                (*array)[i++] = index;
                index = listView->GetNextSelected(index);
            }
        }
        /*else
        {
            array->resize(_selectedIndices.size());
            for (size_t i = 0; i < _selectedIndices.size(); i++)
                (*array)[i] = _selectedIndices[i];
        }*/

        return array;
    }

    void ListView::CloseSelectedIndicesArray(void* array)
    {
        delete ((wxArrayInt*)array);
    }

    int ListView::GetSelectedIndicesItemCount(void* array)
    {
        return ((wxArrayInt*)array)->GetCount();
    }

    int ListView::GetSelectedIndicesItemAt(void* array, int index)
    {
        return (*((wxArrayInt*)array))[index];
    }

    void ListView::ClearSelected()
    {
        if (IsWxWindowCreated())
            DeselectAll(GetListView());
        //else
        //    _selectedIndices.clear();
    }

    void ListView::DeselectAll(wxListView* listView)
    {
        auto selectedIndices = (wxArrayInt*)OpenSelectedIndicesArray();
        for (auto index : *selectedIndices)
            listView->Select(index, false);
        delete selectedIndices;
    }

    void ListView::SetSelected(int index, bool value)
    {
        if (IsWxWindowCreated())
        {
            GetListView()->Select(index, value);
        }
        /*else
        {
            auto existingIndex = std::find(_selectedIndices.begin(), _selectedIndices.end(), index);

            if (value)
            {
                if (existingIndex == _selectedIndices.end())
                    _selectedIndices.push_back(index);
            }
            else
            {
                if (existingIndex != _selectedIndices.end())
                    _selectedIndices.erase(existingIndex);
            }
        }*/
    }

    ListViewSelectionMode ListView::GetSelectionMode()
    {
        return _selectionMode;
    }

    void ListView::SetSelectionMode(ListViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        //auto oldSelection = GetSelectedIndices();

        _selectionMode = value;
        RecreateListView();

        //SetSelectedIndices(oldSelection);
    }

    /*void ListView::ApplySelectedIndices()
    {
        auto listView = GetListView();
        DeselectAll(listView);

        if (_selectedIndices.empty())
            return;

        for (auto index : _selectedIndices)
            listView->Select(index, true);

        _selectedIndices.clear();
    }*/

    /*void ListView::ReceiveSelectedIndices()
    {
        auto listView = GetListView();
        _selectedIndices.clear();

        auto selectedIndices = (wxArrayInt*)OpenSelectedIndicesArray();
        for (auto index : *selectedIndices)
            _selectedIndices.push_back(index);
        delete selectedIndices;
    }*/

    void ListView::ApplyLargeImageList(wxListView* value)
    {
        value->SetImageList(_largeImageList == nullptr ? nullptr : _largeImageList->GetImageList(), wxIMAGE_LIST_NORMAL);
    }

    void ListView::ApplySmallImageList(wxListView* value)
    {
        value->SetImageList(_smallImageList == nullptr ? nullptr : _smallImageList->GetImageList(), wxIMAGE_LIST_SMALL);
    }

    void ListView::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        //ApplyColumns();
        //ApplyItems();
    }

    int ListView::GetItemsCount()
    {
        if (IsWxWindowCreated())
        {
            return GetListView()->GetItemCount();
        }
        else
            return 0;
    }

    /*void ListView::ApplyItems()
    {
        BeginUpdate();

        auto listView = GetListView();
        listView->DeleteAllItems();
        //    Here we have exception. So it is commented for now when Control is recreated and there are items
        int index = 0;
        for (auto row : _rows)
        {
            for (int columnIndex = 0; columnIndex < row.GetCellCount(); columnIndex++)
            {
                wxListItem cell;
                row.GetCell(columnIndex, cell);
                InsertItem(listView, cell);
            }
        }

        EndUpdate();
    }*/

    /*void ListView::ApplyColumns()
    {
        if (_view != ListViewView::Details)
            return;

        BeginUpdate();

        auto listView = GetListView();
        listView->DeleteAllColumns();

        int index = 0;
        for (auto column : _columns)
            InsertColumn(listView, column);

        EndUpdate();
    }*/

    wxListView* ListView::GetListView()
    {
        return dynamic_cast<wxListView*>(GetWxWindow());
    }

    /*void ListView::InsertColumn(wxListView* listView, const Column& column)
    {
        listView->InsertColumn(column.column.GetColumn(), column.column.GetText(), 0, column.width);
    }*/

    long ListView::GetStyle()
    {
        auto getViewStyle = [&]()
        {
            switch (_view)
            {
            case ListViewView::List:
                return wxLC_LIST;
            case ListViewView::Details:
                return wxLC_REPORT;
            case ListViewView::SmallIcon:
                return wxLC_SMALL_ICON;
            case ListViewView::LargeIcon:
                return wxLC_ICON | wxLC_AUTOARRANGE;
            default:
                throwExInvalidOp;
            }
        };

        auto getSelectionStyle = [&]()
        {
            switch (_selectionMode)
            {
            case ListViewSelectionMode::Single:
                return wxLC_SINGLE_SEL;
            case ListViewSelectionMode::Multiple:
                return 0;
            default:
                throwExInvalidOp;
            }
        };

        auto getGridLinesStyle = [&]()
        {
            switch (_gridLinesDisplayMode)
            {
            case ListViewGridLinesDisplayMode::None:
                return 0;
            case ListViewGridLinesDisplayMode::Horizontal:
                return wxLC_HRULES;
            case ListViewGridLinesDisplayMode::Vertical:
                return wxLC_VRULES;
            case ListViewGridLinesDisplayMode::VerticalAndHorizontal:
                return wxLC_HRULES | wxLC_VRULES;
            default:
                throwExInvalidOp;
            }
        };

        auto getSortStyle = [&]()
        {
            switch (_sortMode)
            {
            case ListViewSortMode::None:
                return 0;
            case ListViewSortMode::Ascending:
                return wxLC_SORT_ASCENDING;
            case ListViewSortMode::Descending:
                return wxLC_SORT_DESCENDING;
            case ListViewSortMode::Custom:
                return 0;
            default:
                throwExInvalidOp;
            }
        };

        auto style = getViewStyle() | getSelectionStyle() | getGridLinesStyle() | getSortStyle();

        style |= (_allowLabelEdit ? wxLC_EDIT_LABELS : 0);
        style |= (_columnHeaderVisible ? 0 : wxLC_NO_HEADER);

        return style;
    }

    std::vector<int> ListView::GetSelectedIndices()
    {
        auto array = OpenSelectedIndicesArray();
        int count = GetSelectedIndicesItemCount(array);

        std::vector<int> indices(count);
        for (int i = 0; i < count; i++)
            indices[i] = GetSelectedIndicesItemAt(array, i);

        CloseSelectedIndicesArray(array);

        return indices;
    }

    void ListView::SetSelectedIndices(const std::vector<int>& value)
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
