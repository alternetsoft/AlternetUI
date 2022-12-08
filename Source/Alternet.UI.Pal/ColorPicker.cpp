#include "ColorPicker.h"

namespace Alternet::UI
{
    ColorPicker::ColorPicker():
        _value(*this, *wxBLACK, &Control::IsWxWindowCreated, &ColorPicker::RetrieveValue, &ColorPicker::ApplyValue)
    {
        GetDelayedValues().Add({ &_value });
    }

    ColorPicker::~ColorPicker()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_COLOURPICKER_CHANGED, &ColorPicker::OnColourPickerValueChanged, this);
        }
    }

    Color ColorPicker::GetValue()
    {
        return _value.Get();
    }

    void ColorPicker::SetValue(const Color& value)
    {
        _value.Set(value);
    }

    wxWindow* ColorPicker::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxColourPickerCtrl(parent, wxID_ANY);
        value->Bind(wxEVT_COLOURPICKER_CHANGED, &ColorPicker::OnColourPickerValueChanged, this);
        return value;
    }
    
    void ColorPicker::OnColourPickerValueChanged(wxColourPickerEvent& event)
    {
        RaiseEvent(ColorPickerEvent::ValueChanged);
    }
    
    wxColourPickerCtrl* ColorPicker::GetColourPickerCtrl()
    {
        return dynamic_cast<wxColourPickerCtrl*>(GetWxWindow());
    }
    
    Color ColorPicker::RetrieveValue()
    {
        return GetColourPickerCtrl()->GetColour();
    }
    
    void ColorPicker::ApplyValue(const Color& value)
    {
        GetColourPickerCtrl()->SetColour(value);
    }
}
