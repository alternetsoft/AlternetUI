#include "ComboBox.h"

namespace Alternet::UI
{
    ComboBox::ComboBox() :
        _selectedIndex(*this, -1, &Control::IsWxWindowCreated, &ComboBox::RetrieveSelectedIndex, &ComboBox::ApplySelectedIndex),
        _text(*this, u"", &Control::IsWxWindowCreated, &ComboBox::RetrieveText, &ComboBox::ApplyText)
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
            GetComboBox()->Insert(wxStr(value), index);
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
        auto value = new wxComboBox(
            parent,
            wxID_ANY,
            "",
            wxDefaultPosition,
            wxDefaultSize,
            0,
            NULL,
            GetComboBoxStyle());

        value->Bind(wxEVT_COMBOBOX, &ComboBox::OnSelectionChanged, this);

        return value;
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

        _isEditable = value;
        RecreateWxWindowIfNeeded();
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
            return GetComboBox()->GetCount();
        else
            return _items.size();
    }

    long ComboBox::GetComboBoxStyle()
    {
        return _isEditable ? wxCB_DROPDOWN : wxCB_READONLY;
    }

    void ComboBox::ApplyItems()
    {
        auto comboBox = GetComboBox();
        comboBox->Clear();

        if (_items.empty())
            return;

        for (auto item : _items)
            comboBox->Append(wxStr(item));

        _items.clear();
    }

    void ComboBox::ReceiveItems()
    {
        auto comboBox = GetComboBox();
        _items.clear();

        if (comboBox->GetCount() == 0)
            return;

        for (int i = 0; i < (int)comboBox->GetCount(); i++)
            _items.push_back(wxStr(comboBox->GetString(i)));
    }

    int ComboBox::RetrieveSelectedIndex()
    {
        return GetComboBox()->GetSelection();
    }

    void ComboBox::ApplySelectedIndex(const int& value)
    {
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
        return dynamic_cast<wxComboBox*>(GetWxWindow());
    }
}
