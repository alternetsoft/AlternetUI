#include "ColorPicker.h"

namespace Alternet::UI
{
    ColorPicker::ColorPicker():
        _value(*this, *wxBLACK, &Control::IsWxWindowCreated, &ColorPicker::RetrieveValue, &ColorPicker::ApplyValue)
    {
        GetDelayedValues().Add(&_value);
    }

    ColorPicker::~ColorPicker()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_COLOURPICKER_CHANGED,
                    &ColorPicker::OnColourPickerValueChanged, this);
            }
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

    class wxColourPickerCtrl2 : public wxColourPickerCtrl, public wxWidgetExtender
    {
    public:
        wxColourPickerCtrl2(){}

        wxColourPickerCtrl2(wxWindow* parent, wxWindowID id,
            const wxColour& col = *wxBLACK, const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize, long style = wxCLRP_DEFAULT_STYLE,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxColourPickerCtrlNameStr))
        {
            Create(parent, id, col, pos, size, style, validator, name);
        }
    protected:
        long GetPickerStyle(long style) const wxOVERRIDE
        {
            return (style & (/*wxCLRP_SHOW_LABEL | */wxCLRP_SHOW_ALPHA));
        }

    };

    wxWindow* ColorPicker::CreateWxWindowUnparented()
    {
        return new wxColourPickerCtrl2();
    }

    wxWindow* ColorPicker::CreateWxWindowCore(wxWindow* parent)
    {
        long style = wxCLRP_DEFAULT_STYLE;

        auto value = new wxColourPickerCtrl2(parent, wxID_ANY,
            *wxBLACK,
            wxDefaultPosition,
            wxDefaultSize,
            style,
            wxDefaultValidator,
            wxASCII_STR(wxColourPickerCtrlNameStr));

        value->Bind(wxEVT_COLOURPICKER_CHANGED, &ColorPicker::OnColourPickerValueChanged, this);
        return value;
    }
    
    void ColorPicker::OnColourPickerValueChanged(wxColourPickerEvent& event)
    {
        event.Skip();
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
