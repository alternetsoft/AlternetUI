#include "ProgressBar.h"

namespace Alternet::UI
{
    ProgressBar::ProgressBar():
        _maximum(*this, 100, &Control::IsWxWindowCreated, &ProgressBar::RetrieveMaximum, &ProgressBar::ApplyMaximum)
    {
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
        return _value;
    }

    void ProgressBar::SetValue(int value)
    {
        _value = value;
        ApplyValue();
    }

    bool ProgressBar::GetIsIndeterminate()
    {
        return _isIndeterminate;
    }

    void ProgressBar::SetIsIndeterminate(bool value)
    {
        if (_isIndeterminate == value)
            return;

        _isIndeterminate = value;
        ApplyIndeterminate();
    }

    wxWindow* ProgressBar::CreateWxWindowCore(wxWindow* parent)
    {
        auto gauge = new wxGauge(
            parent,
            wxID_ANY,
            _maximum.Get() - _minimum,
            wxDefaultPosition,
            wxDefaultSize,
            GetStyle());

        return gauge;
    }

    void ProgressBar::OnWxWindowCreated()
    {
        Control::OnWxWindowCreated();
        ApplyValue();
        ApplyIndeterminate();
    }

    ProgressBarOrientation ProgressBar::GetOrientation()
    {
        return _orientation;
    }

    void ProgressBar::SetOrientation(ProgressBarOrientation value)
    {
        if (_orientation == value)
            return;

        _orientation = value;
        RecreateWxWindowIfNeeded();
    }

    wxGauge* ProgressBar::GetGauge()
    {
        return dynamic_cast<wxGauge*>(GetWxWindow());
    }

    void ProgressBar::ApplyValue()
    {
        GetGauge()->SetValue(_value - _minimum);
    }

    int ProgressBar::RetrieveMaximum()
    {
        return GetGauge()->GetRange();
    }

    void ProgressBar::ApplyMaximum(const int& value)
    {
        GetGauge()->SetRange(value);
    }

    long ProgressBar::GetStyle()
    {
        return wxGA_SMOOTH | (_orientation == ProgressBarOrientation::Horizontal ? wxGA_HORIZONTAL : wxGA_VERTICAL);
    }

    void ProgressBar::ApplyIndeterminate()
    {
        auto gauge = GetGauge();

        if (_isIndeterminate)
        {
            if (_indeterminedModeTimer == nullptr)
            {
                _indeterminedModeTimer = new IndeterminedModeTimer(this);
                _indeterminedModeTimer->Start(50);
            }
            
            gauge->Pulse();
        }
        else
        {
            if (_indeterminedModeTimer != nullptr)
            {
                _indeterminedModeTimer->Stop();
                delete _indeterminedModeTimer;
                _indeterminedModeTimer = nullptr;
            }

            GetGauge()->SetValue(0);
            ApplyValue();
        }
    }

    // -----------------

    ProgressBar::IndeterminedModeTimer::IndeterminedModeTimer(ProgressBar* owner) : _owner(owner)
    {
    }

    void ProgressBar::IndeterminedModeTimer::Notify()
    {
        auto gauge = _owner->GetGauge();
        if (gauge != nullptr)
            gauge->Pulse();
    }
}