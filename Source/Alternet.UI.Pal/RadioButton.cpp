#include "RadioButton.h"

namespace Alternet::UI
{
    RadioButton::RadioButton() :
        _text(*this, u"", &Control::IsWxWindowCreated, &RadioButton::RetrieveText, &RadioButton::ApplyText),
        _flags(
            *this,
            RadioButtonFlags::Checked,
            &Control::IsWxWindowCreated,
            {
                {RadioButtonFlags::Checked, std::make_tuple(&RadioButton::RetrieveChecked, &RadioButton::ApplyChecked)},
            })
    {
        GetDelayedValues().Add(&_text);
        GetDelayedValues().Add(&_flags);
    }

    RadioButton::~RadioButton()
    {
    }

    string RadioButton::GetText()
    {
        return _text.Get();
    }

    void RadioButton::SetText(const string& value)
    {
        _text.Set(value);
    }

    wxWindow* RadioButton::CreateWxWindowCore(wxWindow* parent)
    {
        auto radioButton = new wxRadioButton(
            parent,
            wxID_ANY,
            wxStr(_text.Get()),
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

    string RadioButton::RetrieveText()
    {
        return wxStr(GetRadioButton()->GetLabel());
    }

    void RadioButton::ApplyText(const string& value)
    {
        GetRadioButton()->SetLabel(wxStr(value));
    }

    std::vector<RadioButton*> RadioButton::GetRadioButtonsInGroup()
    {
        std::vector<RadioButton*> result;

        auto parent = GetParent();
        if (parent == nullptr)
            return result;

        // todo: check with wxRB_GROUP
        for (auto child : parent->GetChildren())
        {
            auto radioButton = dynamic_cast<RadioButton*>(child);
            if (radioButton != nullptr)
                result.push_back(radioButton);
        }

        return result;
    }

    bool RadioButton::RetrieveChecked()
    {
        return GetRadioButton()->GetValue();
    }

    void RadioButton::ApplyChecked(bool value)
    {
        GetRadioButton()->SetValue(value);
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

    void RadioButton::SetWxWindowParent(wxWindow* parent)
    {
        auto group = GetChildRadioButtonsCount(parent);
        if (group == 0)
        {
            _firstInGroup = true;
            RecreateWxWindowIfNeeded();
        }
        else
            _firstInGroup = false;

        Control::SetWxWindowParent(parent);
    }

    void RadioButton::OnCheckedChanged(wxCommandEvent& event)
    {
        auto group = GetRadioButtonsInGroup();
        if (group.size() > 0)
        {
            // wxEVT_RADIOBUTTON is not fired on unchecked, only on "click". So we need to create an illusion of that.
            for (auto rb : group)
                rb->RaiseEvent(RadioButtonEvent::CheckedChanged);
        }
        else
            RaiseEvent(RadioButtonEvent::CheckedChanged);
    }

    bool RadioButton::GetIsChecked()
    {
        return _flags.Get(RadioButtonFlags::Checked);
    }

    void RadioButton::SetIsChecked(bool value)
    {
        _flags.Set(RadioButtonFlags::Checked, value);
    }
}
