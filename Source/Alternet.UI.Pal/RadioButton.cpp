#include "RadioButton.h"

namespace Alternet::UI
{
    RadioButton::RadioButton()
    {
    }

    RadioButton::~RadioButton()
    {
    }

    string RadioButton::GetText()
    {
        return wxStr(GetRadioButton()->GetLabel());
    }

    void RadioButton::SetText(const NativeStringSpan& value)
    {
        GetRadioButton()->SetLabel(StringSpanToWx(value));
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
            _firstInGroup ? wxRB_GROUP : 0);

        radioButton->Bind(wxEVT_RADIOBUTTON, &RadioButton::OnCheckedChanged, this);
        return radioButton;
    }

    wxRadioButton* RadioButton::GetRadioButton()
    {
        return dynamic_cast<wxRadioButton*>(GetWxWindow());
    }

    std::vector<RadioButton*> RadioButton::GetRadioButtonsInGroup()
    {
        std::vector<RadioButton*> result;

        auto parent = GetParent();
        if (parent == nullptr)
            return result;

        // check with wxRB_GROUP?
        for (auto child : parent->GetChildren())
        {
            auto radioButton = dynamic_cast<RadioButton*>(child);
            if (radioButton != nullptr)
                result.push_back(radioButton);
        }

        return result;
    }

    int RadioButton::GetChildRadioButtonsCount(wxWindow* parent)
    {
        int result = 0;

        if (parent == nullptr)
            return result;

        for (auto child : parent->GetChildren())
        {
            auto radioButton = dynamic_cast<wxRadioButton*>(child);
            if (radioButton != nullptr)
                result++;
        }

        return result;
    }

    void RadioButton::RecreateWxWindowIfNeeded()
    {
        auto text = GetText();
        auto state = GetIsChecked();
        Control::RecreateWxWindowIfNeeded();
        GetRadioButton()->SetLabel(wxStr(text));
        SetIsChecked(state);
    }

    void RadioButton::SetWxWindowParent(wxWindow* parent)
    {
        _firstInGroup = GetChildRadioButtonsCount(parent) == 0;
        Control::SetWxWindowParent(parent);
    }

    void RadioButton::OnCheckedChanged(wxCommandEvent& event)
    {
        event.Skip();
        auto group = GetRadioButtonsInGroup();
        if (group.size() > 0)
        {
            // wxEVT_RADIOBUTTON is not fired on unchecked, only on "click".
            // So we need to create an illusion of that.
            for (auto rb : group)
                rb->RaiseEvent(RadioButtonEvent::CheckedChanged);
        }
        else
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
