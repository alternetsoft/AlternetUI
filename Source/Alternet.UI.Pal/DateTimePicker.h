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

    private:
        wxDatePickerCtrl* GetDatePickerCtrl();
        wxTimePickerCtrl* GetTimePickerCtrl();
        bool IsTimePicker();

        DelayedValue<DateTimePicker, DateTime> _value;
        DateTime _minValue = DateTime();
        DateTime _maxValue = DateTime();
        int _valueKind;
        int _popupKind;

        DateTime RetrieveValue();
        void ApplyValue(const DateTime& value);
    };
}
