#include "CheckBox.h"

namespace Alternet::UI
{
    CheckBox::CheckBox()
    {
    }

    CheckBox::~CheckBox()
    {
    }

    NativeStringSpan CheckBox::GetText()
    {
        _textValue = GetCheckBox()->GetLabel();
        return wxStr(_textValue);
    }

    void CheckBox::SetText(const NativeStringSpan& value)
    {
        auto wx = wxStr(value);
        GetCheckBox()->SetLabel(wx);
    }

    int CheckBox::GetCheckState()
    {
        return GetCheckBox()->Get3StateValue();
    }

    void CheckBox::SetCheckState(int value)
    {
        GetCheckBox()->Set3StateValue((wxCheckBoxState)value);
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
            const wxString& name)
        {
            Create(parent, id, label, pos, size, style, validator, name);
        }

        wxCheckBox2()
        {

        }
    };

    wxWindow* CheckBox::CreateWxWindowUnparented()
    {
        return new wxCheckBox2();
    }

    wxWindow* CheckBox::CreateWxWindowCore(wxWindow* parent)
    {
        long style = 0;

        if (_threeState)
            style |= wxCHK_3STATE;
        else
            style |= wxCHK_2STATE;
        
        if(_alignRight)
            style |= wxALIGN_RIGHT;

        if (_allowAllStatesForUser && _threeState)
            style |= wxCHK_ALLOW_3RD_STATE_FOR_USER;

        auto checkBox = new wxCheckBox2(parent,
            wxID_ANY,
            "",
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

    void CheckBox::OnCheckedChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(CheckBoxEvent::CheckedChanged);
    }
    
    bool CheckBox::GetIsChecked()
    {
        return GetCheckState() == wxCheckBoxState::wxCHK_CHECKED;
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

    void CheckBox::RecreateWxWindowIfNeeded()
    {
		auto text = GetCheckBox()->GetLabel();
		auto state = GetCheckState();
        Control::RecreateWxWindowIfNeeded();
        GetCheckBox()->SetLabel(text);
        SetCheckState(state);
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
        SetCheckState(value ? wxCheckBoxState::wxCHK_CHECKED : wxCheckBoxState::wxCHK_UNCHECKED);
    }
}
