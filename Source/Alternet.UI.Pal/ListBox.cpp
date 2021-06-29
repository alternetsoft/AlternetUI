#include "ListBox.h"

namespace Alternet::UI
{
    ListBox::ListBox():
        _selectionMode(*this, ListBoxSelectionMode::Single, &Control::IsWxWindowCreated, &ListBox::RetrieveSelectionMode, &ListBox::ApplySelectionMode)
    {
        GetDelayedValues().Add(&_selectionMode);
    }

    ListBox::~ListBox()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            //window->Unbind(wxEVT_SLIDER, &Slider::OnSliderValueChanged, this);
        }
    }

    void ListBox::InsertItem(int index, const string& value)
    {
        if (IsWxWindowCreated())
            GetListBox()->Insert(wxStr(value), index);
        else
            _items.emplace(_items.begin() + index, value);
    }

    void ListBox::RemoveItemAt(int index)
    {
        if (IsWxWindowCreated())
            GetListBox()->Delete(index);
        else
            _items.erase(_items.begin() + index);
    }

    void ListBox::ClearItems()
    {
        if (IsWxWindowCreated())
            GetListBox()->Clear();
        else
            _items.clear();
    }

    wxWindow* ListBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxListBox(parent, wxID_ANY);
        //value->Bind(wxEVT_SLIDER, &Slider::OnSliderValueChanged, this);

        return value;
    }

    ListBoxSelectionMode ListBox::GetSelectionMode()
    {
        return ListBoxSelectionMode();
    }

    void ListBox::SetSelectionMode(ListBoxSelectionMode value)
    {
    }

    void ListBox::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyItems();
    }

    int ListBox::GetItemsCount()
    {
        if (IsWxWindowCreated())
            return GetListBox()->GetCount();
        else
            return _items.size();
    }

    void ListBox::ApplyItems()
    {
        auto listBox = GetListBox();
        listBox->Clear();

        if (_items.empty())
            return;

        for (auto item : _items)
            listBox->Append(wxStr(item));

        _items.clear();
    }

    ListBoxSelectionMode ListBox::RetrieveSelectionMode()
    {
        auto isSingle = (GetListBox()->GetWindowStyle() & wxLB_SINGLE) != 0;
        return isSingle ? ListBoxSelectionMode::Single : ListBoxSelectionMode::Multiple;
    }

    void ListBox::ApplySelectionMode(const ListBoxSelectionMode& value)
    {
        auto style = GetListBox()->GetWindowStyle();

        switch (value)
        {
        case ListBoxSelectionMode::Single:
            style |= wxLB_SINGLE;
            style &= ~(wxLB_EXTENDED | wxLB_MULTIPLE);
            break;
        case ListBoxSelectionMode::Multiple:
            style |= wxLB_EXTENDED;
            style &= ~wxLB_SINGLE;
            break;
        default:
            wxASSERT(false);
        }

        GetListBox()->SetWindowStyle(style);
    }

    wxListBox* ListBox::GetListBox()
    {
        return dynamic_cast<wxListBox*>(GetWxWindow());
    }
}
