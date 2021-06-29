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
        auto value = new wxListBox(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            0,
            NULL,
            GetSelectionStyle());

        //value->Bind(wxEVT_SLIDER, &Slider::OnSliderValueChanged, this);

        return value;
    }

    ListBoxSelectionMode ListBox::GetSelectionMode()
    {
        return _selectionMode;
    }

    void ListBox::SetSelectionMode(ListBoxSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        _selectionMode = value;
        RecreateWxWindowIfNeeded();
        // todo: restore selection
    }

    void ListBox::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyItems();
    }

    void ListBox::OnWxWindowDestroying()
    {
        Control::OnWxWindowDestroying();
        ReceiveItems();
    }

    int ListBox::GetItemsCount()
    {
        if (IsWxWindowCreated())
            return GetListBox()->GetCount();
        else
            return _items.size();
    }

    long ListBox::GetSelectionStyle()
    {
        switch (_selectionMode)
        {
        case ListBoxSelectionMode::Single:
            return wxLB_SINGLE;
        case ListBoxSelectionMode::Multiple:
            return wxLB_EXTENDED;
        default:
            wxASSERT(false);
            throw 0;
        }
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

    void ListBox::ReceiveItems()
    {
        auto listBox = GetListBox();
        _items.clear();

        if (listBox->IsEmpty())
            return;

        for (int i = 0; i < (int)listBox->GetCount(); i++)
            _items.push_back(wxStr(listBox->GetString(i)));
    }

    wxListBox* ListBox::GetListBox()
    {
        return dynamic_cast<wxListBox*>(GetWxWindow());
    }
}
