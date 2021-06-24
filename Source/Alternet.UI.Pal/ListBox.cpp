#include "ListBox.h"

namespace Alternet::UI
{
    ListBox::ListBox()
    {
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

    wxListBox* ListBox::GetListBox()
    {
        return dynamic_cast<wxListBox*>(GetWxWindow());
    }
}
