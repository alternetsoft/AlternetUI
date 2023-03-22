#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class DateTimePicker : public Control
    {
#include "Api/DateTimePicker.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        //void OnDateTimePickerValueChanged(wxDateTimePickerEvent& event);

    private:
        //wxDateTimePickerCtrl* GetDateTimePickerCtrl();

        //DelayedValue<DateTimePicker, DateTime> _value;

        //DateTime RetrieveValue();
        //void ApplyValue(const DateTime& value);

    };
}
