#include "RadioButton.h"

namespace Alternet::UI
{
    RadioButton::RadioButton()
    {
    }

    RadioButton::~RadioButton()
    {
    }

    NativeStringSpan RadioButton::GetText()
    {
        _textValue = GetRadioButton()->GetLabel();
        return wxStr(_textValue);
    }

    void RadioButton::SetText(const NativeStringSpan& value)
    {
        GetRadioButton()->SetLabel(wxStr(value));
    }

    class wxRadioButton2 : public wxRadioButton, public wxWidgetExtender
    {
    public:
        wxRadioButton2(wxWindow* parent,
            wxWindowID id,
            const wxString& label,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = 0,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxRadioButtonNameStr))
        {
            Create(parent, id, label, pos, size, style, validator, name);
        }

        wxRadioButton2()
        {
        }
    };

    wxWindow* RadioButton::CreateWxWindowUnparented()
    {
        return new wxRadioButton2();
    }

    wxWindow* RadioButton::CreateWxWindowCore(wxWindow* parent)
    {
        auto radioButton = new wxRadioButton2(
            parent,
            wxID_ANY,
            "",
            wxDefaultPosition,
            wxDefaultSize,
            wxRB_SINGLE);

        radioButton->Bind(wxEVT_RADIOBUTTON, &RadioButton::OnCheckedChanged, this);
        return radioButton;
    }

    wxRadioButton* RadioButton::GetRadioButton()
    {
        return dynamic_cast<wxRadioButton*>(GetWxWindow());
    }

    void RadioButton::RecreateWxWindowIfNeeded()
    {
        auto text = GetRadioButton()->GetLabel();
        auto state = GetIsChecked();
        Control::RecreateWxWindowIfNeeded();
        GetRadioButton()->SetLabel(text);
        SetIsChecked(state);
    }

    void RadioButton::SetWxWindowParent(wxWindow* parent)
    {
        Control::SetWxWindowParent(parent);
    }

    void RadioButton::OnCheckedChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(RadioButtonEvent::CheckedChanged);
    }

    bool RadioButton::GetIsChecked()
    {
        return GetRadioButton()->GetValue();
    }

    void RadioButton::SetIsChecked(bool value)
    {
        GetRadioButton()->SetValue(value);
    }
}
