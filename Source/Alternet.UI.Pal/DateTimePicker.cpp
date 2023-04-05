#include "DateTimePicker.h"
#include "wx/datetime.h"

namespace Alternet::UI
{
    DateTimePicker::DateTimePicker() :
        _value(*this, DateTime(), &Control::IsWxWindowCreated, &DateTimePicker::RetrieveValue, &DateTimePicker::ApplyValue)
    {
        GetDelayedValues().Add({ &_value });
    }

    DateTimePicker::~DateTimePicker()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_DATE_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
                window->Unbind(wxEVT_TIME_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
            }
        }
    }

    DateTime DateTimePicker::GetValue()
    {
        return _value.Get();
    }

    void DateTimePicker::SetValue(const DateTime& value)
    {
        _value.Set(value);
    }

    wxWindow* DateTimePicker::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxDatePickerCtrl(parent, wxID_ANY);
        value->Bind(wxEVT_DATE_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
        value->Bind(wxEVT_TIME_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
        return value;
    }

    void DateTimePicker::OnDateTimePickerValueChanged(wxDateEvent& event)
    {
        RaiseEvent(DateTimePickerEvent::ValueChanged);
    }

    wxDatePickerCtrl* DateTimePicker::GetDateTimePickerCtrl()
    {
        return dynamic_cast<wxDatePickerCtrl*>(GetWxWindow());
    }

    wxDateTime DateTimePicker::RetrieveValue()
    {
        return GetDateTimePickerCtrl()->GetValue();
    }

    void DateTimePicker::ApplyValue(const DateTime& value)
    {
        GetDateTimePickerCtrl()->SetValue(value);
    }
}
