#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class RadioButton : public Control
    {
#include "Api/RadioButton.inc"

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnCheckedChanged(wxCommandEvent& event);

    public:

    private:
        enum class RadioButtonFlags
        {
            None = 0,
            Checked = 1 << 0,
        };

        wxRadioButton* GetRadioButton();

        DelayedValue<RadioButton, string> _text;
        DelayedFlags<RadioButton, RadioButtonFlags> _flags;

        string RetrieveText();
        void ApplyText(const string& value);

        std::vector<RadioButton*> GetRadioButtonsInGroup();

        bool RetrieveChecked();
        void ApplyChecked(bool value);

    protected:
        void SetWxWindowParent(wxWindow* parent) override;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::RadioButton::RadioButtonFlags> { static const bool enable = true; };