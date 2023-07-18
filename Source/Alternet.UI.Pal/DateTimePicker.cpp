#include "DateTimePicker.h"

namespace Alternet::UI
{
    DateTimePicker::DateTimePicker() :
        _value(*this, DateTime(), &Control::IsWxWindowCreated,
            &DateTimePicker::RetrieveValue, &DateTimePicker::ApplyValue)
    {
        GetDelayedValues().Add({ &_value });
    }

    int DateTimePicker::GetPopupKind() 
    {
        return _popupKind;
    }

    void DateTimePicker::SetPopupKind(int value)
    {
        if (_popupKind == value)
            return;
        _popupKind = value;
        RecreateWxWindowIfNeeded();
    }

    int DateTimePicker::GetValueKind() 
    {
        return _valueKind;
    }

    void DateTimePicker::SetValueKind(int value)
    {
        if (_valueKind == value)
            return;
        _valueKind = value;
        RecreateWxWindowIfNeeded();
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

    bool DateTimePicker::IsTimePicker() 
    {
        return (_valueKind == 1);
    }

    wxWindow* DateTimePicker::CreateWxWindowCore(wxWindow* parent)
    {
        wxWindow* value = nullptr;
        if(IsTimePicker())
            value = new wxTimePickerCtrl(parent, wxID_ANY);
        else 
        {
            long style = wxDP_DEFAULT;

            if (_popupKind == 1)
                style = wxDP_SPIN;
            if (_popupKind == 2)
                style = wxDP_DROPDOWN;

            style = style | wxDP_SHOWCENTURY;

            wxDateTime v = _value.GetDelayed();
            
            value = new wxDatePickerCtrl(parent,
                wxID_ANY,
                v,
                wxDefaultPosition,
                wxDefaultSize,
                style,
                wxDefaultValidator
            );
        }
        value->Bind(wxEVT_DATE_CHANGED, 
            &DateTimePicker::OnDateTimePickerValueChanged, this);
        value->Bind(wxEVT_TIME_CHANGED, 
            &DateTimePicker::OnDateTimePickerValueChanged, this);
        return value;
    }

    void DateTimePicker::OnDateTimePickerValueChanged(wxDateEvent& event)
    {
        RaiseEvent(DateTimePickerEvent::ValueChanged);
    }

    wxDatePickerCtrl* DateTimePicker::GetDatePickerCtrl()
    {
        return dynamic_cast<wxDatePickerCtrl*>(GetWxWindow());
    }

    wxTimePickerCtrl* DateTimePicker::GetTimePickerCtrl()
    {
        return dynamic_cast<wxTimePickerCtrl*>(GetWxWindow());
    }

    DateTime DateTimePicker::RetrieveValue()
    {
        if(IsTimePicker())
            return GetTimePickerCtrl()->GetValue();
        wxDateTime wxresult = GetDatePickerCtrl()->GetValue();
        DateTime result = DateTime(wxresult);
        return result;
    }

    void DateTimePicker::ApplyValue(const DateTime& value)
    {
        if (IsTimePicker())
            GetTimePickerCtrl()->SetTime(value.Hour,value.Minute,value.Second);
        else
        {
            wxDateTime wxValue = value;
            GetDatePickerCtrl()->SetValue(wxValue);
        }
    }

}
