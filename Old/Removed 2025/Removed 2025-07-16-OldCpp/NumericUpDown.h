#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class NumericUpDown : public Control
    {
#include "Api/NumericUpDown.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        void OnSpinCtrlValueChanged(wxCommandEvent& event);

    private:
        wxSpinCtrl* GetSpinCtrl();
        bool hasBorder = true;

        DelayedValue<NumericUpDown, int> _value;
        DelayedValue<NumericUpDown, int> _minimum;
        DelayedValue<NumericUpDown, int> _maximum;

        int RetrieveValue();
        void ApplyValue(const int& value);

        int RetrieveMaximum();
        void ApplyMaximum(const int& value);

        int RetrieveMinimum();
        void ApplyMinimum(const int& value);
    };
}
