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
        NativeStringSpan GetText() override;
        void SetText(const NativeStringSpan& value) override;
        virtual void RecreateWxWindowIfNeeded() override;

    private:
        wxRadioButton* GetRadioButton();

    protected:
        void SetWxWindowParent(wxWindow* parent) override;
    };
}