#include "ListView.h"

#ifdef __WXMSW__
#include <uxtheme.h>
#endif

namespace Alternet::UI
{
    ListView::ListView()
    {
    }

    ListView::~ListView()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
            window->Unbind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);
        }
    }

    void ListView::InsertColumnAt(int index, const string& header)
    {
        wxListItem column;
        column.SetText(wxStr(header));
        column.SetColumn(index);
        _columns.emplace(_columns.begin() + index, column);

        if (IsWxWindowCreated() && _view == ListViewView::Details)
            InsertColumn(GetListView(),column);
    }

    void ListView::RemoveColumnAt(int index)
    {
        _columns.erase(_columns.begin() + index);

        if (IsWxWindowCreated() && _view == ListViewView::Details)
            GetListView()->DeleteColumn(index);
    }

    void ListView::InsertItemAt(int index, const string& value, int columnIndex, int imageIndex)
    {
        wxListItem item;
        item.SetText(wxStr(value));
        item.SetColumn(columnIndex);
        item.SetId(index);
        
        if (columnIndex == 0)
            item.SetImage(imageIndex); // for now, allow images only for the first column, same as in WinForms.

        Row& row = GetRow(index);
        row.SetCell(columnIndex, item);

        if (IsWxWindowCreated())
            InsertItem(GetListView(), item);
    }

    ListView::Row& ListView::GetRow(int index)
    {
        for (int i = _rows.size(); i <= index; i++)
            _rows.push_back(Row(index));

        return _rows[index];
    }

    void ListView::InsertItem(wxListView* listView, wxListItem& item)
    {
        if (_view == ListViewView::Details || item.GetColumn() == 0)
        {
            if (item.GetColumn() > 0)
            {
                wxListItem i(item);
                GetListView()->SetItem(i);
            }
            else
                GetListView()->InsertItem(item);
        }
    }

    void ListView::RemoveItemAt(int index)
    {
        _rows.erase(_rows.begin() + index);

        if (IsWxWindowCreated())
            GetListView()->DeleteItem(index);
    }

    void ListView::ClearItems()
    {
        _rows.clear();

        if (IsWxWindowCreated())
            GetListView()->DeleteAllItems();
    }

    ListViewView ListView::GetCurrentView()
    {
        return _view;
    }

    void ListView::SetCurrentView(ListViewView value)
    {
        if (_view == value)
            return;

        auto oldSelection = GetSelectedIndices();

        _view = value;
        RecreateWxWindowIfNeeded();

        SetSelectedIndices(oldSelection);
    }

    ImageList* ListView::GetSmallImageList()
    {
        return _smallImageList;
    }

    void ListView::SetSmallImageList(ImageList* value)
    {
        // todo: memory management.
        _smallImageList = value;
        if (IsWxWindowCreated())
            ApplySmallImageList(GetListView());
    }

    ImageList* ListView::GetLargeImageList()
    {
        return _largeImageList;
    }

    void ListView::SetLargeImageList(ImageList* value)
    {
        // todo: memory management.
        _largeImageList = value;
        if (IsWxWindowCreated())
            ApplyLargeImageList(GetListView());
    }

    wxWindow* ListView::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxListView(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            GetStyle());

#ifdef __WXMSW__
        SetWindowTheme((HWND)value->GetHWND(), L"", NULL); // turn off "explorer style" item hover effects.
#endif

        ApplySmallImageList(value);
        ApplyLargeImageList(value);

        value->Bind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
        value->Bind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);

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
        else
        {
            array->resize(_selectedIndices.size());
            for (int i = 0; i < _selectedIndices.size(); i++)
                (*array)[i] = _selectedIndices[i];
        }

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
        else
            _selectedIndices.clear();
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
        else
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
        }
    }

    ListViewSelectionMode ListView::GetSelectionMode()
    {
        return _selectionMode;
    }

    void ListView::SetSelectionMode(ListViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        auto oldSelection = GetSelectedIndices();

        _selectionMode = value;
        RecreateWxWindowIfNeeded();

        SetSelectedIndices(oldSelection);
    }

    void ListView::ApplySelectedIndices()
    {
        auto listView = GetListView();
        DeselectAll(listView);

        if (_selectedIndices.empty())
            return;

        for (auto index : _selectedIndices)
            listView->Select(index, true);

        _selectedIndices.clear();
    }

    void ListView::ReceiveSelectedIndices()
    {
        auto listView = GetListView();
        _selectedIndices.clear();

        auto selectedIndices = (wxArrayInt*)OpenSelectedIndicesArray();
        for (auto index : *selectedIndices)
            _selectedIndices.push_back(index);
        delete selectedIndices;
    }

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
        ApplyColumns();
        ApplyItems();
    }

    int ListView::GetItemsCount()
    {
        return _rows.size();
    }

    void ListView::ApplyItems()
    {
        BeginUpdate();

        auto listView = GetListView();
        listView->DeleteAllItems();

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
    }

    void ListView::ApplyColumns()
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
    }

    wxListView* ListView::GetListView()
    {
        return dynamic_cast<wxListView*>(GetWxWindow());
    }

    void ListView::InsertColumn(wxListView* listView, const wxListItem& column)
    {
        listView->InsertColumn(column.GetColumn(), column.GetText());
    }

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
                wxASSERT(false);
                throw 0;
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
                wxASSERT(false);
                throw 0;
            }
        };

        return getViewStyle() | getSelectionStyle();
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
