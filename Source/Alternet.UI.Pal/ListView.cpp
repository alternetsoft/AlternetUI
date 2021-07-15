#include "ListView.h"

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

    void ListView::InsertItemAt(int index, const string& value)
    {
        if (IsWxWindowCreated())
            GetListCtrl()->InsertItem(index, wxStr(value));
        else
            _items.emplace(_items.begin() + index, value);
    }

    void ListView::RemoveItemAt(int index)
    {
        if (IsWxWindowCreated())
            GetListCtrl()->DeleteItem(index);
        else
            _items.erase(_items.begin() + index);
    }

    void ListView::ClearItems()
    {
        if (IsWxWindowCreated())
            GetListCtrl()->DeleteAllItems();
        else
            _items.clear();
    }

    wxWindow* ListView::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxListView(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            wxLC_LIST);

        //value->Bind(wxEVT_ListView, &ListView::OnSelectionChanged, this);

        return value;
    }


    void ListView::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyItems();
    }

    void ListView::OnWxWindowDestroying()
    {
        Control::OnWxWindowDestroying();
        ReceiveItems();
    }

    int ListView::GetItemsCount()
    {
        if (IsWxWindowCreated())
            return GetListCtrl()->GetItemCount();
        else
            return _items.size();
    }

    void ListView::ApplyItems()
    {
        BeginUpdate();

        auto listCtrl = GetListCtrl();
        listCtrl->DeleteAllItems();

        int index = 0;
        for (auto item : _items)
            listCtrl->InsertItem(index++, wxStr(item));

        EndUpdate();

        _items.clear();
    }

    void ListView::ReceiveItems()
    {
        auto listCtrl = GetListCtrl();
        _items.clear();

        if (listCtrl->IsEmpty())
            return;

        for (int i = 0; i < (int)listCtrl->GetItemCount(); i++)
            _items.push_back(wxStr(listCtrl->GetItemText(i)));
    }

    wxListCtrl* ListView::GetListCtrl()
    {
        return dynamic_cast<wxListCtrl*>(GetWxWindow());
    }
}
