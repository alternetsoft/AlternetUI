#include "DateTimePicker.h"

namespace Alternet::UI
{
    DateTimePicker::DateTimePicker() :
        _value(*this, DateTime(), &Control::IsWxWindowCreated,
            &DateTimePicker::RetrieveValue, &DateTimePicker::ApplyValue)
    {
        GetDelayedValues().Add({ &_value });
    }

    bool DateTimePicker::GetHasBorder()
    {
        return hasBorder;
    }

    void DateTimePicker::SetHasBorder(bool value)
    {
        if (hasBorder == value)
            return;
        hasBorder = value;
        RecreateWxWindowIfNeeded();
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
        long style = GetBorderStyle();

        if (!hasBorder)
            style = style | wxBORDER_NONE;

        wxDateTime v = _value.GetDelayed();
        wxWindow* value = nullptr;

        if (IsTimePicker()) 
        {
            style = style | wxTP_DEFAULT;
            value = new wxTimePickerCtrl(parent,
                wxID_ANY,
                wxDefaultDateTime,
                wxDefaultPosition,
                wxDefaultSize,
                style,
                wxDefaultValidator);
        }
        else 
        {
            long popupStyle = wxDP_DEFAULT;

            if (_popupKind == 1)
                popupStyle = wxDP_SPIN;
            if (_popupKind == 2)
                popupStyle = wxDP_DROPDOWN;

            style = style | wxDP_SHOWCENTURY | popupStyle;

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

    DateTime DateTimePicker::GetMinValue()
    {
        return _minValue;
    }
    
    void DateTimePicker::SetMinValue(const DateTime& value)
    {
        _minValue = value;
    }
    
    DateTime DateTimePicker::GetMaxValue()
    {
        return _maxValue;
    }

    void DateTimePicker::SetMaxValue(const DateTime& value)
    {
        _maxValue = value;
    }

    void DateTimePicker::SetRange(bool useMinValue, bool useMaxValue)
    {
        if (IsTimePicker())
            return;

        wxDateTime wxdt1 = wxInvalidDateTime;
        if (useMinValue)
            wxdt1 = _minValue;

        wxDateTime wxdt2 = wxInvalidDateTime;
        if (useMaxValue)
            wxdt2 = _maxValue;

        GetDatePickerCtrl()->SetRange(wxdt1, wxdt2);        
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
