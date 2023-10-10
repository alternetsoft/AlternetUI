#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "wx/datetime.h"
#include "wx/timectrl.h"

namespace Alternet::UI
{
    class DateTimePicker : public Control
    {
#include "Api/DateTimePicker.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        void OnDateTimePickerValueChanged(wxDateEvent& event);

        // Under Linux without this height of DateTimePicker will be 0
        // But need to find another solution
        void ApplyMinimumSize(const Size& value) override {}
        void ApplyMaximumSize(const Size& value) override {}

    private:
        wxDatePickerCtrl* GetDatePickerCtrl();
        wxTimePickerCtrl* GetTimePickerCtrl();
        bool IsTimePicker();

        bool hasBorder = true;
        DelayedValue<DateTimePicker, DateTime> _value;
        DateTime _minValue = DateTime();
        DateTime _maxValue = DateTime();
        int _valueKind = 0;
        int _popupKind = 0;

        DateTime RetrieveValue();
        void ApplyValue(const DateTime& value);
    };
}
