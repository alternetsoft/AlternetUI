#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class ColorPicker : public Control
    {
#include "Api/ColorPicker.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void OnColourPickerValueChanged(wxColourPickerEvent& event);

    private:
        wxColourPickerCtrl* GetColourPickerCtrl();

        DelayedValue<ColorPicker, Color> _value;

        Color RetrieveValue();
        void ApplyValue(const Color& value);
    };
}
