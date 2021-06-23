#include "NumericUpDown.h"

namespace Alternet::UI
{
    NumericUpDown::NumericUpDown():
        _value(*this, 0, &Control::IsWxWindowCreated, &NumericUpDown::RetrieveValue, &NumericUpDown::ApplyValue),
        _maximum(*this, 100, &Control::IsWxWindowCreated, &NumericUpDown::RetrieveMaximum, &NumericUpDown::ApplyMaximum),
        _minimum(*this, 0, &Control::IsWxWindowCreated, &NumericUpDown::RetrieveMinimum, &NumericUpDown::ApplyMinimum)
    {
    }

    NumericUpDown::~NumericUpDown()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_SPINCTRL, &NumericUpDown::OnSpinCtrlValueChanged, this);
        }
    }

    int NumericUpDown::GetMinimum()
    {
        return _minimum.Get();
    }

    void NumericUpDown::SetMinimum(int value)
    {
        _minimum.Set(value);
    }

    int NumericUpDown::GetMaximum()
    {
        return _maximum.Get();
    }

    void NumericUpDown::SetMaximum(int value)
    {
        _maximum.Set(value);
    }

    int NumericUpDown::GetValue()
    {
        return _value.Get();
    }

    void NumericUpDown::SetValue(int value)
    {
        _value.Set(value);
    }

    wxWindow* NumericUpDown::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxSpinCtrl(parent);
        value->Bind(wxEVT_SPINCTRL, &NumericUpDown::OnSpinCtrlValueChanged, this);

        return value;
    }

    void NumericUpDown::OnSpinCtrlValueChanged(wxCommandEvent& event)
    {
        RaiseEvent(NumericUpDownEvent::ValueChanged);
    }

    wxSpinCtrl* NumericUpDown::GetSpinCtrl()
    {
        return dynamic_cast<wxSpinCtrl*>(GetWxWindow());
    }

    int NumericUpDown::RetrieveValue()
    {
        return GetSpinCtrl()->GetValue();
    }

    void NumericUpDown::ApplyValue(const int& value)
    {
        GetSpinCtrl()->SetValue(value);
    }

    int NumericUpDown::RetrieveMaximum()
    {
        return GetSpinCtrl()->GetMax();
    }

    void NumericUpDown::ApplyMaximum(const int& value)
    {
        GetSpinCtrl()->SetMax(value);
    }

    int NumericUpDown::RetrieveMinimum()
    {
        return GetSpinCtrl()->GetMin();
    }
    void NumericUpDown::ApplyMinimum(const int& value)
    {
        GetSpinCtrl()->SetMin(value);
    }
}
