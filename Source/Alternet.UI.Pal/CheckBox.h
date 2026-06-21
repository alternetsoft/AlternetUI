#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class CheckBox : public Control
    {
#include "Api/CheckBox.inc"

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;
        void OnCheckedChanged(wxCommandEvent& event);

    public:
        NativeStringSpan GetText() override;
        void SetText(const NativeStringSpan& value) override;

        virtual void RecreateWxWindowIfNeeded() override;
    private:

        wxCheckBox* GetCheckBox();

        bool _threeState = false;
        bool _alignRight = false;
        bool _allowAllStatesForUser = false;
    };
}
