#include "DateTimePicker.h"

namespace Alternet::UI
{
    //DateTimePicker::DateTimePicker() :
    //    _value(*this, *wxBLACK, &Control::IsWxWindowCreated, &DateTimePicker::RetrieveValue, &DateTimePicker::ApplyValue)
    //{
    //    GetDelayedValues().Add({ &_value });
    //}

    //DateTimePicker::~DateTimePicker()
    //{
    //    if (IsWxWindowCreated())
    //    {
    //        auto window = GetWxWindow();
    //        if (window != nullptr)
    //        {
    //            window->Unbind(wxEVT_COLOURPICKER_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
    //        }
    //    }
    //}

    //DateTime DateTimePicker::GetValue()
    //{
    //    return _value.Get();
    //}

    //void DateTimePicker::SetValue(const DateTime& value)
    //{
    //    _value.Set(value);
    //}

    //wxWindow* DateTimePicker::CreateWxWindowCore(wxWindow* parent)
    //{
    //    auto value = new wxColourPickerCtrl(parent, wxID_ANY);
    //    value->Bind(wxEVT_COLOURPICKER_CHANGED, &DateTimePicker::OnDateTimePickerValueChanged, this);
    //    return value;
    //}

    //void DateTimePicker::OnDateTimePickerValueChanged(wxDateTimePickerEvent& event)
    //{
    //    RaiseEvent(DateTimePickerEvent::ValueChanged);
    //}

    //wxDateTimePickerCtrl* DateTimePicker::GetDateTimePickerCtrl()
    //{
    //    return dynamic_cast<wxDateTimePickerCtrl*>(GetWxWindow());
    //}

    //DateTime DateTimePicker::RetrieveValue()
    //{
    //    return GetDateTimePickerCtrl()->GetValue();
    //}

    //void DateTimePicker::ApplyValue(const DateTime& value)
    //{
    //    GetDateTimePickerCtrl()->SetValue(value);
    //}
}
