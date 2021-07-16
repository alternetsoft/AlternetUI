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
            //window->Unbind(wxEVT_LISt, &ListView::OnSelectionChanged, this);
        }
    }

    void ListView::InsertColumnAt(int index, const string& header)
    {
        _columns.emplace(_columns.begin() + index, header);

        if (IsWxWindowCreated())
            InsertColumn(GetListCtrl(), header, index);
    }

    void ListView::RemoveColumnAt(int index)
    {
        _columns.erase(_columns.begin() + index);

        if (IsWxWindowCreated())
            GetListCtrl()->DeleteColumn(index);
    }

    void ListView::InsertItemAt(int index, const string& value, int columnIndex)
    {
        wxListItem item;
        item.SetText(wxStr(value));
        item.SetColumn(columnIndex);
        item.SetId(index);


        Row& row = GetRow(index);
        row.SetCell(columnIndex, item);

        if (IsWxWindowCreated())
            InsertItem(GetListCtrl(), item);
    }

    ListView::Row& ListView::GetRow(int index)
    {
        for (int i = _rows.size(); i <= index; i++)
            _rows.push_back(Row(index));

        return _rows[index];
    }

    void ListView::InsertItem(wxListCtrl* listCtrl, const wxListItem& item)
    {
        if (_view == ListViewView::Details || item.GetColumn() == 0)
        {
            if (item.GetColumn() > 0)
            {
                wxListItem i(item);
                GetListCtrl()->SetItem(i);
            }
            else
                GetListCtrl()->InsertItem(item);
        }
    }

    void ListView::RemoveItemAt(int index)
    {
        _rows.erase(_rows.begin() + index);

        if (IsWxWindowCreated())
            GetListCtrl()->DeleteItem(index);
    }

    void ListView::ClearItems()
    {
        _rows.clear();

        if (IsWxWindowCreated())
            GetListCtrl()->DeleteAllItems();
    }

    ListViewView ListView::GetCurrentView()
    {
        return _view;
    }

    void ListView::SetCurrentView(ListViewView value)
    {
        if (_view == value)
            return;

        //auto oldSelectedIndex = GetSelectedIndex(); // todo

        _view = value;
        RecreateWxWindowIfNeeded();

        //SetSelectedIndex(oldSelectedIndex);
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

        //value->Bind(wxEVT_ListView, &ListView::OnSelectionChanged, this);

        return value;
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

        auto listCtrl = GetListCtrl();
        listCtrl->DeleteAllItems();

        int index = 0;
        for (auto row : _rows)
        {
            for (int columnIndex = 0; columnIndex < row.GetCellCount(); columnIndex++)
            {
                wxListItem cell;
                row.GetCell(columnIndex, cell);
                InsertItem(listCtrl, cell);
            }
        }

        EndUpdate();
    }

    void ListView::ApplyColumns()
    {
        BeginUpdate();

        auto listCtrl = GetListCtrl();
        listCtrl->DeleteAllColumns();

        int index = 0;
        for (auto item : _columns)
            InsertColumn(listCtrl, item, index++);

        EndUpdate();
    }

    wxListCtrl* ListView::GetListCtrl()
    {
        return dynamic_cast<wxListCtrl*>(GetWxWindow());
    }

    void ListView::InsertColumn(wxListCtrl* listCtrl, const string& title, int index)
    {
        listCtrl->InsertColumn(index, wxStr(title));
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
                return wxLC_ICON;
            default:
                wxASSERT(false);
                throw 0;
            }
        };

        return getViewStyle();
    }
}
