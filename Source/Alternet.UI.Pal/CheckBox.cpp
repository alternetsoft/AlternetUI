#include "CheckBox.h"

namespace Alternet::UI
{
    CheckBox::CheckBox() :
        _text(*this, u"", &Control::IsWxWindowCreated,
            &CheckBox::RetrieveText, &CheckBox::ApplyText),
        _state(*this, wxCheckBoxState::wxCHK_UNCHECKED,
            &Control::IsWxWindowCreated, &CheckBox::RetrieveState, &CheckBox::ApplyState)
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

    class wxCheckBox2 : public wxCheckBox, public wxWidgetExtender
    {
    public:
        wxCheckBox2(wxWindow* parent,
            wxWindowID id,
            const wxString& label,
            const wxPoint& pos,
            const wxSize& size,
            long style,
            const wxValidator& validator,
            const wxString& name);
    };

    wxCheckBox2::wxCheckBox2(wxWindow* parent,
        wxWindowID id,
        const wxString& label,
        const wxPoint& pos = wxDefaultPosition,
        const wxSize& size = wxDefaultSize,
        long style = 0,
        const wxValidator& validator = wxDefaultValidator,
        const wxString& name = wxASCII_STR(wxCheckBoxNameStr))
    {
        Create(parent, id, label, pos, size, style, validator, name);
    }

    wxWindow* CheckBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = 0;

        if (_threeState)
            style |= wxCHK_3STATE;
        
        if(_alignRight)
            style |= wxALIGN_RIGHT;

        if (_allowAllStatesForUser)
            style |= wxCHK_ALLOW_3RD_STATE_FOR_USER;

        auto checkBox = new wxCheckBox2(parent,
            wxID_ANY,
            wxStr(_text.Get()),
            wxDefaultPosition,
            wxDefaultSize,
            style,
            wxDefaultValidator,
            wxASCII_STR(wxCheckBoxNameStr));

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
        event.Skip();
        RaiseEvent(CheckBoxEvent::CheckedChanged);
    }
    
    bool CheckBox::GetIsChecked()
    {
        return _state.Get() == wxCheckBoxState::wxCHK_CHECKED;
    }

    int CheckBox::GetCheckState()
    {
        return _state.Get();
    }

    void CheckBox::SetCheckState(int value)
    {
        _state.Set((wxCheckBoxState)value);
    }

    bool CheckBox::GetThreeState()
    {
        return _threeState;
    }

    void CheckBox::SetThreeState(bool value)
    {
        if (_threeState == value)
            return;
        _threeState = value;
        RecreateWxWindowIfNeeded();
    }

    bool CheckBox::GetAlignRight()
    {
        return _alignRight;
    }

    void CheckBox::SetAlignRight(bool value)
    {
        if (_alignRight == value)
            return;
        _alignRight = value;
        RecreateWxWindowIfNeeded();
    }

    bool CheckBox::GetAllowAllStatesForUser()
    {
        return _allowAllStatesForUser;
    }

    void CheckBox::SetAllowAllStatesForUser(bool value)
    {
        if (_allowAllStatesForUser == value)
            return;
        _allowAllStatesForUser = value;
        RecreateWxWindowIfNeeded();
    }
    
    void CheckBox::SetIsChecked(bool value)
    {
        _state.Set(value ? wxCheckBoxState::wxCHK_CHECKED : wxCheckBoxState::wxCHK_UNCHECKED);
    }
}
