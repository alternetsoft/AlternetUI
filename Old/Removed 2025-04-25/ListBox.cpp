#include "ListBox.h"

namespace Alternet::UI
{
    void* ListBox::CreateEx(int64_t styles)
    {
        return new ListBox(styles);
    }

    bool ListBox::GetHasBorder() 
    {
        return hasBorder;
    }
    
    void ListBox::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWindow();
    }

    ListBox::ListBox(int64_t styles)
    {
        bindScrollEvents = false;
    }

    ListBox::ListBox()
    {
        bindScrollEvents = false;
    }

    ListBox::~ListBox()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_LISTBOX, &ListBox::OnSelectionChanged, this);
            }
        }
    }

    void ListBox::SetItem(int index, const string& value)
    {
        if (IsWxWindowCreated())
            GetListBox()->SetString(index, wxStr(value));
        else
            _items.at(index) = value;
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

    class wxListBox2 : public wxListBox, public wxWidgetExtender
    {
    public:
        wxListBox2() {}
        wxListBox2(wxWindow* parent, wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            int n = 0, const wxString choices[] = NULL,
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxListBoxNameStr))
        {
            Create(parent, id, pos, size, n, choices, style, validator, name);
        }
    };

    wxWindow* ListBox::CreateWxWindowUnparented()
    {
        return new wxListBox2();
    }

    wxWindow* ListBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetSelectionStyle() | GetBorderStyle();

        if (!hasBorder)
            style = style | wxBORDER_NONE;
        
        auto value = new wxListBox2(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            0,
            NULL,
            style);

        value->Bind(wxEVT_LISTBOX, &ListBox::OnSelectionChanged, this);

        return value;
    }

    Size ListBox::GetPreferredSize(const Size& availableSize)
    {
        auto size = Control::GetPreferredSize(availableSize);

#ifdef __WXOSX_COCOA__
        // Hacky workaround to fix macOS ListBox measurement.
        size.Width += 4;
        size.Height += 4;
#endif

        return size;
    }

    void ListBox::EnsureVisible(int itemIndex)
    {
        GetListBox()->EnsureVisible(itemIndex);
    }

    int ListBox::ItemHitTest(const Point& position)
    {
        auto listBox = GetListBox();
        return listBox->HitTest(fromDip(position, listBox));
    }

    void ListBox::OnSelectionChanged(wxCommandEvent& event)
    {
        event.Skip();
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
            for (size_t i = 0; i < _selectedIndices.size(); i++)
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

    void ListBox::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();
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
        default:
            return wxLB_SINGLE;
        case ListBoxSelectionMode::Multiple:
            return wxLB_EXTENDED;
        }
    }

    void ListBox::ApplyItems()
    {
        BeginUpdate();

        auto listBox = GetListBox();
        listBox->Clear();

        for (auto& item : _items) 
        {
            listBox->Append(wxStr(item));
        }

        EndUpdate();

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
