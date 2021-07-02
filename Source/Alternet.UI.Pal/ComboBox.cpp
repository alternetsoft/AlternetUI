#include "ComboBox.h"

namespace Alternet::UI
{
    constexpr bool UseChoiceControlForReadOnlyComboBox = true;
//#ifdef __WXMSW__
//    constexpr bool UseChoiceControlForReadOnlyComboBox = false;
//#else
//    constexpr bool UseChoiceControlForReadOnlyComboBox = true;
//#endif

    ComboBox::ComboBox() :
        _selectedIndex(*this, -1, &Control::IsWxWindowCreated, &ComboBox::RetrieveSelectedIndex, &ComboBox::ApplySelectedIndex),
        _text(*this, u"", &ComboBox::IsUsingComboBoxControl, &ComboBox::RetrieveText, &ComboBox::ApplyText)
    {
        GetDelayedValues().Add({ &_selectedIndex, &_text });
    }

    ComboBox::~ComboBox()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_COMBOBOX, &ComboBox::OnSelectionChanged, this);
        }
    }

    void ComboBox::InsertItem(int index, const string& value)
    {
        if (IsWxWindowCreated())
            GetItemContainer()->Insert(wxStr(value), index);
        else
            _items.emplace(_items.begin() + index, value);
    }

    void ComboBox::RemoveItemAt(int index)
    {
        if (IsWxWindowCreated())
            GetComboBox()->Delete(index);
        else
            _items.erase(_items.begin() + index);
    }

    void ComboBox::ClearItems()
    {
        if (IsWxWindowCreated())
            GetComboBox()->Clear();
        else
            _items.clear();
    }

    wxWindow* ComboBox::CreateWxWindowCore(wxWindow* parent)
    {
        if (!_isEditable && UseChoiceControlForReadOnlyComboBox)
        {
            // On non-Windows systems wxChoice looks different than read-only wxComboBox.
            auto value = new wxChoice(parent, wxID_ANY);
            value->Bind(wxEVT_CHOICE, &ComboBox::OnSelectionChanged, this);
            return value;
        }
        else
        {
            auto value = new wxComboBox(
                parent,
                wxID_ANY,
                "",
                wxDefaultPosition,
                wxDefaultSize,
                0,
                NULL,
                _isEditable ? wxCB_DROPDOWN : wxCB_READONLY);

            value->Bind(wxEVT_COMBOBOX, &ComboBox::OnSelectionChanged, this);
            return value;
        }

    }

    void ComboBox::OnSelectionChanged(wxCommandEvent& event)
    {
        RaiseEvent(ComboBoxEvent::SelectionChanged);
    }

    bool ComboBox::GetIsEditable()
    {
        return _isEditable;
    }

    void ComboBox::SetIsEditable(bool value)
    {
        if (_isEditable == value)
            return;

        auto oldSelectedIndex = GetSelectedIndex();
        
        _isEditable = value;
        RecreateWxWindowIfNeeded();

        SetSelectedIndex(oldSelectedIndex);
    }

    int ComboBox::GetSelectedIndex()
    {
        return _selectedIndex.Get();
    }

    void ComboBox::SetSelectedIndex(int value)
    {
        _selectedIndex.Set(value);
    }

    string ComboBox::GetText()
    {
        return _text.Get();
    }

    void ComboBox::SetText(const string& value)
    {
        _text.Set(value);
    }

    void ComboBox::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyItems();
    }

    void ComboBox::OnWxWindowDestroying()
    {
        Control::OnWxWindowDestroying();
        ReceiveItems();
    }

    int ComboBox::GetItemsCount()
    {
        if (IsWxWindowCreated())
            return GetItemContainer()->GetCount();
        else
            return _items.size();
    }

    void ComboBox::ApplyItems()
    {
        auto itemContainer = GetItemContainer();
        itemContainer->Clear();

        if (_items.empty())
            return;

        for (auto item : _items)
            itemContainer->Append(wxStr(item));

        _items.clear();
    }

    void ComboBox::ReceiveItems()
    {
        auto itemContainer = GetItemContainer();
        _items.clear();

        if (itemContainer->GetCount() == 0)
            return;

        for (int i = 0; i < (int)itemContainer->GetCount(); i++)
            _items.push_back(wxStr(itemContainer->GetString(i)));
    }

    int ComboBox::RetrieveSelectedIndex()
    {
        if (IsUsingChoiceControl())
            return GetChoice()->GetSelection();
        else
            return GetComboBox()->GetSelection();
    }

    void ComboBox::ApplySelectedIndex(const int& value)
    {
        if (IsUsingChoiceControl())
            GetChoice()->SetSelection(value);
        else
            GetComboBox()->SetSelection(value);
    }

    string ComboBox::RetrieveText()
    {
        return wxStr(GetComboBox()->GetValue());
    }

    void ComboBox::ApplyText(const string& value)
    {
        GetComboBox()->SetValue(wxStr(value));
    }

    wxComboBox* ComboBox::GetComboBox()
    {
        auto value = dynamic_cast<wxComboBox*>(GetWxWindow());
        wxASSERT(value);
        return value;
    }

    wxChoice* ComboBox::GetChoice()
    {
        auto value = dynamic_cast<wxChoice*>(GetWxWindow());
        wxASSERT(value);
        return value;
    }

    wxItemContainer* ComboBox::GetItemContainer()
    {
        auto value = dynamic_cast<wxItemContainer*>(GetWxWindow());
        wxASSERT(value);
        return value;
    }

    bool ComboBox::IsUsingChoiceControl()
    {
        return dynamic_cast<wxChoice*>(GetWxWindow()) != nullptr;
    }

    bool ComboBox::IsUsingComboBoxControl()
    {
        return dynamic_cast<wxComboBox*>(GetWxWindow()) != nullptr;
    }
}
