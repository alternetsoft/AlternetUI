#include "RadioButton.h"

namespace Alternet::UI
{
    RadioButton::RadioButton() :
        _text(*this, u"", &Control::IsWxWindowCreated, &RadioButton::RetrieveText,
            &RadioButton::ApplyText),
        _flags(
            *this,
            RadioButtonFlags::Checked,
            &Control::IsWxWindowCreated,
            {
                {RadioButtonFlags::Checked, std::make_tuple(&RadioButton::RetrieveChecked,
                &RadioButton::ApplyChecked)},
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

        // check with wxRB_GROUP?
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
#ifdef __WXGTK20__
        // On Linux, it seems wxGTK seem to not support radio groups with no selection.
        // So wxRadioButton::GetValue() will return true when a radio button is the only one in the group
        // So while control is a child of the parking window a wrong value is returned. This is a workaround:
        if (_isRecreating)
            return _isCheckedWhileRecreating;
#endif
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
        _firstInGroup = GetChildRadioButtonsCount(parent) == 0;
        _isRecreating = true;
        _isCheckedWhileRecreating = _flags.GetDelayed(RadioButtonFlags::Checked);
        RecreateWxWindowIfNeeded();
        _isRecreating = false;

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
        return _flags.Get(RadioButtonFlags::Checked);
    }

    void RadioButton::SetIsChecked(bool value)
    {
        _flags.Set(RadioButtonFlags::Checked, value);
    }
}
