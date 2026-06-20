#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class RadioButton : public Control
    {
#include "Api/RadioButton.inc"

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;
        void OnCheckedChanged(wxCommandEvent& event);

    public:
        string GetText() override;
        void SetText(const NativeStringSpan& value) override;
        virtual void RecreateWxWindowIfNeeded() override;

    private:
        enum class RadioButtonFlags
        {
            None = 0,
            Checked = 1 << 0,
        };

        wxRadioButton* GetRadioButton();

        std::vector<RadioButton*> GetRadioButtonsInGroup();

        int GetChildRadioButtonsCount(wxWindow* parent);

        bool _firstInGroup = false;

    protected:
        void SetWxWindowParent(wxWindow* parent) override;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::RadioButton::RadioButtonFlags> { static const bool enable = true; };