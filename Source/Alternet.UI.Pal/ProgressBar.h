#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class ProgressBar : public Control
    {
#include "Api/ProgressBar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
        wxGauge* GetGauge();

        DelayedValue<ProgressBar, int> _value;
        DelayedValue<ProgressBar, int> _maximum;
        int _minimum = 0;

        int RetrieveValue();
        void ApplyValue(const int& value);

        int RetrieveMaximum();
        void ApplyMaximum(const int& value);
    };
}
