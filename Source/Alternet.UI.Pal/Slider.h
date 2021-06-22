#pragma once
#include "Common.h"
#include "Control.h"

namespace Alternet::UI
{
    class Slider : public Control
    {
#include "Api/Slider.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void OnSliderValueChanged(wxCommandEvent& event);

    private:
        wxSlider* GetSlider();

        DelayedValue<Slider, int> _value;
        DelayedValue<Slider, int> _minimum;
        DelayedValue<Slider, int> _maximum;
        DelayedValue<Slider, int> _smallChange;
        DelayedValue<Slider, int> _largeChange;
        DelayedValue<Slider, int> _tickFrequency;

        int RetrieveValue();
        void ApplyValue(const int& value);

        int RetrieveMaximum();
        void ApplyMaximum(const int& value);

        int RetrieveMinimum();
        void ApplyMinimum(const int& value);

        int RetrieveSmallChange();
        void ApplySmallChange(const int& value);

        int RetrieveLargeChange();
        void ApplyLargeChange(const int& value);

        int RetrieveTickFrequency();
        void ApplyTickFrequency(const int& value);
    };
}
