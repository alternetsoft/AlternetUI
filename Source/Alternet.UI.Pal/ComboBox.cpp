#include "ComboBox.h"

namespace Alternet::UI
{
    // Linux performance note: combo box with many items is really slow on Linux.
    // This seems to be a known problem, 
    // see https://gitlab.gnome.org/GNOME/gtk/-/issues/1910

    constexpr bool UseChoiceControlForReadOnlyComboBox = false;
//#ifdef __WXMSW__
//    constexpr bool UseChoiceControlForReadOnlyComboBox = false;
//#else
//    constexpr bool UseChoiceControlForReadOnlyComboBox = true;
//#endif

    ComboBox::ComboBox() :
        _selectedIndex(*this, -1, &Control::IsWxWindowCreated, 
            &ComboBox::RetrieveSelectedIndex, &ComboBox::ApplySelectedIndex),
        _text(*this, u"", &ComboBox::IsUsingComboBoxControl, &ComboBox::RetrieveText, 
            &ComboBox::ApplyText)
    {
        GetDelayedValues().Add({ &_selectedIndex, &_text });
    }

    bool ComboBox::GetHasBorder()
    {
        return hasBorder;
    }

    void ComboBox::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
    }

    ComboBox::~ComboBox()
    {
        if (IsWxWindowCreated())
        {
            if (IsUsingChoiceControl())
            {
                auto window = GetChoice();
                window->Unbind(wxEVT_CHOICE, &ComboBox::OnSelectedItemChanged, this);
            }
            else if (IsUsingComboBoxControl())
            {
                auto window = GetComboBox();
                window->Unbind(wxEVT_COMBOBOX, 
                    &ComboBox::OnSelectedItemChanged, this);
                window->Unbind(wxEVT_TEXT, &ComboBox::OnTextChanged, this);
            }
        }
    }

    void ComboBox::InsertItem(int index, const string& value)
    {
        if (IsWxWindowCreated())
            GetItemContainer()->Insert(wxStr(value), index);
        else
            _items.emplace(_items.begin() + index, value);
    }

    void* ComboBox::CreateItemsInsertion()
    {
        return new wxArrayString();
    }

    void ComboBox::AddItemToInsertion(void* insertion, const string& item)
    {
        auto strings = (wxArrayString*)insertion;
        strings->Add(wxStr(item));
    }

    void ComboBox::CommitItemsInsertion(void* insertion, int index)
    {
        auto strings = (wxArrayString*)insertion;

        if (IsWxWindowCreated())
        {
            if(strings->GetCount()>0)
                GetItemContainer()->Insert(*strings, index);
        }
        else
        {
            for (size_t i = 0; i < strings->GetCount(); i++)
                _items.emplace(_items.begin() + index + i, wxStr((*strings)[i]));
        }

        delete strings;
    }

    void ComboBox::RemoveItemAt(int index)
    {
        if (IsWxWindowCreated())
            GetItemContainer()->Delete(index);
        else
            _items.erase(_items.begin() + index);
    }

    void ComboBox::ClearItems()
    {
        if (IsWxWindowCreated())
            GetItemContainer()->Clear();
        else
            _items.clear();
    }

    class wxChoice2 : public wxChoice, public wxWidgetExtender
    {
    public:
        wxChoice2(wxWindow* parent,
            wxWindowID id,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            int n = 0, const wxString choices[] = NULL,
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxChoiceNameStr))
        {
            Create(parent, id, pos, size, n, choices, style, validator, name);
        }
    };

    class wxComboBox2 : public wxComboBox, public wxWidgetExtender
    {
    public:
        wxComboBox2(wxWindow* parent, wxWindowID id,
            const wxString& value = wxEmptyString,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            int n = 0, const wxString choices[] = NULL,
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxComboBoxNameStr))
        {
            Create(parent, id, value, pos, size, n, choices, style, validator, name);

        }
    };

    wxWindow* ComboBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = GetBorderStyle();

        if (!hasBorder)
            style = style | wxBORDER_NONE;

        if (!_isEditable && UseChoiceControlForReadOnlyComboBox)
        {
            // On non-Windows systems wxChoice looks different than 
            // read-only wxComboBox.
            auto value = new wxChoice2(
                parent,
                wxID_ANY,
                wxDefaultPosition,
                wxDefaultSize,
                0,
                NULL,
                style,
                wxDefaultValidator);

            value->Bind(wxEVT_CHOICE, &ComboBox::OnSelectedItemChanged, this);
            return value;
        }
        else
        {
            auto comboStyle = _isEditable ? wxCB_DROPDOWN : wxCB_READONLY;
            style = style | comboStyle;
            auto value = new wxComboBox2(
                parent,
                wxID_ANY,
                "",
                wxDefaultPosition,
                wxDefaultSize,
                0,
                NULL,
                style);

            value->Bind(wxEVT_COMBOBOX, &ComboBox::OnSelectedItemChanged, this);
            value->Bind(wxEVT_TEXT, &ComboBox::OnTextChanged, this);
            return value;
        }

    }

    void ComboBox::OnSelectedItemChanged(wxCommandEvent& event)
    {
        RaiseEvent(ComboBoxEvent::SelectedItemChanged);
    }

    void ComboBox::OnTextChanged(wxCommandEvent& event)
    {
        RaiseEvent(ComboBoxEvent::TextChanged);
    }

    int ComboBox::GetTextSelectionStart()
    {
        if (!_isEditable)
            return 0;
        
        long from = 0, to = 0;
        GetComboBox()->GetSelection(& from, & to);
        return from;
    }

    int ComboBox::GetTextSelectionLength()
    {
        if (!_isEditable)
            return 0;

        long from = 0, to = 0;
        GetComboBox()->GetSelection(&from, &to);
        return to - from;
    }

    void ComboBox::SelectTextRange(int start, int length)
    {
        if (!_isEditable)
            return;

        GetComboBox()->SetSelection(start, start + length);
    }

    void ComboBox::SelectAllText()
    {
        if (IsUsingChoiceControl())
            return;

        GetComboBox()->SelectAll();
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

    void ComboBox::OnBeforeDestroyWxWindow()
    {
        Control::OnBeforeDestroyWxWindow();
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
        BeginUpdate();

        auto itemContainer = GetItemContainer();
        itemContainer->Clear();

        wxArrayString wxStrings;
        for (auto item : _items)
            wxStrings.Add(wxStr(item));

        itemContainer->Append(wxStrings);

        _items.clear();
        
        EndUpdate();
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

        // wxEVT_CHOICE / wxEVT_COMBOBOX are not raised on programmatic selection change.
        RaiseEvent(ComboBoxEvent::SelectedItemChanged);
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
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxChoice* ComboBox::GetChoice()
    {
        auto value = dynamic_cast<wxChoice*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
        return value;
    }

    wxItemContainer* ComboBox::GetItemContainer()
    {
        auto value = dynamic_cast<wxItemContainer*>(GetWxWindow());
        if (value == nullptr)
            throwExInvalidOp;
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
