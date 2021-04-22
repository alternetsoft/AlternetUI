#include "CheckBox.h"

namespace Alternet::UI
{
    CheckBox::CheckBox() :
        _text(*this, u"", &Control::IsWxWindowCreated, &CheckBox::RetrieveText, &CheckBox::ApplyText)
    {
        GetDelayedValues().Add(&_text);
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

    void CheckBox::OnCheckedChanged(wxCommandEvent& event)
    {
        RaiseEvent(CheckBoxEvent::CheckedChanged);
    }
    
    bool CheckBox::GetIsChecked()
    {
        return GetCheckBox()->GetValue();
    }
    
    void CheckBox::SetIsChecked(bool value)
    {
        GetCheckBox()->SetValue(value);
    }
}
