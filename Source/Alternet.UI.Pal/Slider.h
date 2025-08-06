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
        wxWindow* CreateWxWindowUnparented() override;

        void OnSliderValueChanged(wxCommandEvent& event);

        Size GetPreferredSize(const Size& availableSize) override;

        int _value;
        int _minimum;
        int _maximum;
        int _smallChange;
        int _largeChange;
        int _tickFrequency;
    private:
        wxSlider* GetSlider();

        SliderOrientation _orientation = SliderOrientation::Horizontal;
        SliderTickStyle _tickStyle = SliderTickStyle::None;

        long GetStyle();
    };
}
