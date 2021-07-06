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
            window->Unbind(wxEVT_LISTBOX, &ListBox::OnSelectionChanged, this);
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

        value->Bind(wxEVT_LISTBOX, &ListBox::OnSelectionChanged, this);

        return value;
    }

    void ListBox::OnSelectionChanged(wxCommandEvent& event)
    {
        RaiseEvent(ListBoxEvent::SelectionChanged);
    }

    void* ListBox::OpenSelectedIndicesArray()
    {
        auto array = new wxArrayInt();
        if (IsWxWindowCreated())
        {
            GetListBox()->GetSelections(*array);
        }
        else
        {
            for (int i = 0; i < _selectedIndices.size(); i++)
                array->Add(_selectedIndices[i]);
        }

        return array;
    }

    void ListBox::CloseSelectedIndicesArray(void* array)
    {
        delete ((wxArrayInt*)array);
    }

    int ListBox::GetSelectedIndicesItemCount(void* array)
    {
        return ((wxArrayInt*)array)->GetCount();
    }

    int ListBox::GetSelectedIndicesItemAt(void* array, int index)
    {
        return (*((wxArrayInt*)array))[index];
    }

    void ListBox::ClearSelected()
    {
        if (IsWxWindowCreated())
        {
            GetListBox()->DeselectAll();
        }
        else
            _selectedIndices.clear();
    }

    void ListBox::SetSelected(int index, bool value)
    {
        if (IsWxWindowCreated())
        {
            auto listBox = GetListBox();
            if (value)
                listBox->Select(index);
            else
                listBox->Deselect(index);
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

    ListBoxSelectionMode ListBox::GetSelectionMode()
    {
        return _selectionMode;
    }

    void ListBox::SetSelectionMode(ListBoxSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        auto oldSelection = GetSelectedIndices();
        
        _selectionMode = value;
        RecreateWxWindowIfNeeded();

        SetSelectedIndices(oldSelection);
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

    void ListBox::ApplySelectedIndices()
    {
        auto listBox = GetListBox();
        listBox->DeselectAll();

        if (_selectedIndices.empty())
            return;

        for (auto index : _selectedIndices)
            listBox->Select(index);

        _selectedIndices.clear();
    }

    void ListBox::ReceiveSelectedIndices()
    {
        auto listBox = GetListBox();
        _selectedIndices.clear();

        wxArrayInt selections;
        if (listBox->GetSelections(selections) == 0)
            return;

        for (int i = 0; i < (int)selections.GetCount(); i++)
            _selectedIndices.push_back(selections[i]);
    }

    wxListBox* ListBox::GetListBox()
    {
        return dynamic_cast<wxListBox*>(GetWxWindow());
    }

    std::vector<int> ListBox::GetSelectedIndices()
    {
        auto array = OpenSelectedIndicesArray();
        int count = GetSelectedIndicesItemCount(array);
        
        std::vector<int> indices(count);
        for (int i = 0; i < count; i++)
            indices[i] = GetSelectedIndicesItemAt(array, i);
        
        CloseSelectedIndicesArray(array);

        return indices;
    }

    void ListBox::SetSelectedIndices(const std::vector<int>& value)
    {
        ClearSelected();
        for (auto index : value)
            SetSelected(index, true);
    }
}
