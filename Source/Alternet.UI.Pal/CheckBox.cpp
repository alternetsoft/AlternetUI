#include "CheckBox.h"

namespace Alternet::UI
{
    CheckBox::CheckBox() :
        _text(*this, u"", &Control::IsWxWindowCreated, &CheckBox::RetrieveText, &CheckBox::ApplyText),
        _state(*this, wxCheckBoxState::wxCHK_UNCHECKED, &Control::IsWxWindowCreated, &CheckBox::RetrieveState, &CheckBox::ApplyState)
    {
        GetDelayedValues().Add(&_text);
        GetDelayedValues().Add(&_state);
    }

    CheckBox::~CheckBox()
    {
    }

    string CheckBox::GetText()
    {
        return _text.Get();
    }

    void CheckBox::SetText(const string& value)
    {
        _text.Set(value);
    }

    wxWindow* CheckBox::CreateWxWindowCore(wxWindow* parent)
    {
        auto checkBox = new wxCheckBox(parent, wxID_ANY, wxStr(_text.Get()));
        checkBox->Bind(wxEVT_CHECKBOX, &CheckBox::OnCheckedChanged, this);
        return checkBox;
    }

    wxCheckBox* CheckBox::GetCheckBox()
    {
        return dynamic_cast<wxCheckBox*>(GetWxWindow());
    }

    string CheckBox::RetrieveText()
    {
        return wxStr(GetCheckBox()->GetLabel());
    }

    void CheckBox::ApplyText(const string& value)
    {
        GetCheckBox()->SetLabel(wxStr(value));
    }

    wxCheckBoxState CheckBox::RetrieveState()
    {
        return GetCheckBox()->Get3StateValue();
    }

    void CheckBox::ApplyState(const wxCheckBoxState& value)
    {
        GetCheckBox()->Set3StateValue(value);
    }

    void CheckBox::OnCheckedChanged(wxCommandEvent& event)
    {
        RaiseEvent(CheckBoxEvent::CheckedChanged);
    }
    
    bool CheckBox::GetIsChecked()
    {
        return _state.Get() == wxCheckBoxState::wxCHK_CHECKED;
    }
    
    void CheckBox::SetIsChecked(bool value)
    {
        _state.Set(value ? wxCheckBoxState::wxCHK_CHECKED : wxCheckBoxState::wxCHK_UNCHECKED);
    }
}
