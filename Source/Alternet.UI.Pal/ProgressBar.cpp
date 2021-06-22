#include "ProgressBar.h"

namespace Alternet::UI
{
    ProgressBar::ProgressBar():
        _value(*this, 0, &Control::IsWxWindowCreated, &ProgressBar::RetrieveValue, &ProgressBar::ApplyValue),
        _maximum(*this, 100, &Control::IsWxWindowCreated, &ProgressBar::RetrieveMaximum, &ProgressBar::ApplyMaximum)
    {
        GetDelayedValues().Add(&_value);
        GetDelayedValues().Add(&_maximum);
    }
    
    ProgressBar::~ProgressBar()
    {
    }

    int ProgressBar::GetMinimum()
    {
        return _minimum;
    }

    void ProgressBar::SetMinimum(int value)
    {
        if (_minimum == value)
            return;

        _minimum = value;
        SetMaximum(GetMaximum() + _minimum);
        SetValue(GetValue() + _minimum);
    }

    int ProgressBar::GetMaximum()
    {
        return _maximum.Get() + _minimum;
    }

    void ProgressBar::SetMaximum(int value)
    {
        _maximum.Set(value - _minimum);
    }

    int ProgressBar::GetValue()
    {
        return _value.Get() + _minimum;
    }

    void ProgressBar::SetValue(int value)
    {
        _value.Set(value - _minimum);
    }

    bool ProgressBar::GetIsIndeterminate()
    {
        return false;
    }

    void ProgressBar::SetIsIndeterminate(bool value)
    {
    }

    wxWindow* ProgressBar::CreateWxWindowCore(wxWindow* parent)
    {
        return new wxGauge(parent, wxID_ANY, _maximum.Get() - _minimum);
    }

    wxGauge* ProgressBar::GetGauge()
    {
        return dynamic_cast<wxGauge*>(GetWxWindow());
    }
    
    int ProgressBar::RetrieveValue()
    {
        return GetGauge()->GetValue();
    }

    void ProgressBar::ApplyValue(const int& value)
    {
        GetGauge()->SetValue(value);
    }

    int ProgressBar::RetrieveMaximum()
    {
        return GetGauge()->GetRange();
    }

    void ProgressBar::ApplyMaximum(const int& value)
    {
        GetGauge()->SetRange(value);
    }
}