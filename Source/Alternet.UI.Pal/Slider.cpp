#include "Slider.h"

namespace Alternet::UI
{
    Slider::Slider():
        _value(*this, 0, &Control::IsWxWindowCreated, &Slider::RetrieveValue, &Slider::ApplyValue),
        _maximum(*this, 10, &Control::IsWxWindowCreated, &Slider::RetrieveMaximum,
            &Slider::ApplyMaximum),
        _minimum(*this, 0, &Control::IsWxWindowCreated, &Slider::RetrieveMinimum,
            &Slider::ApplyMinimum),
        _smallChange(*this, 1, &Control::IsWxWindowCreated, &Slider::RetrieveSmallChange,
            &Slider::ApplySmallChange),
        _largeChange(*this, 5, &Control::IsWxWindowCreated, &Slider::RetrieveLargeChange,
            &Slider::ApplyLargeChange),
        _tickFrequency(*this, 1, &Control::IsWxWindowCreated, &Slider::RetrieveTickFrequency,
            &Slider::ApplyTickFrequency)
    {
        GetDelayedValues().Add({ &_minimum, &_maximum, &_value, &_smallChange, &_largeChange,
            &_tickFrequency });
    }

    Slider::~Slider()
    {
        if (IsWxWindowCreated())
        {
            auto window = GetWxWindow();
            if (window != nullptr)
            {
                window->Unbind(wxEVT_SLIDER, &Slider::OnSliderValueChanged, this);
            }
        }
    }

    Size Slider::GetPreferredSize(const Size& availableSize)
    {
        auto size = Control::GetPreferredSize(availableSize);

#ifdef __WXOSX_COCOA__
        // Hacky workaround to fix macOS ListBox measurement.
        size.Height += 4;
#endif

        return size;
    }

    SliderOrientation Slider::GetOrientation()
    {
        return _orientation;
    }

    void Slider::SetOrientation(SliderOrientation value)
    {
        if (_orientation == value)
            return;

        _orientation = value;
        RecreateWxWindowIfNeeded();
    }

    SliderTickStyle Slider::GetTickStyle()
    {
        return _tickStyle;
    }

    void Slider::SetTickStyle(SliderTickStyle value)
    {
        if (_tickStyle == value)
            return;

        _tickStyle = value;
        RecreateWxWindowIfNeeded();
    }

    int Slider::GetMinimum()
    {
        return _minimum.Get();
    }

    void Slider::SetMinimum(int value)
    {
        _minimum.Set(value);
    }

    int Slider::GetMaximum()
    {
        return _maximum.Get();
    }

    void Slider::SetMaximum(int value)
    {
        _maximum.Set(value);
    }

    int Slider::GetValue()
    {
        return _value.Get();
    }

    void Slider::SetValue(int value)
    {
        _value.Set(value);
    }

    int Slider::GetSmallChange()
    {
        return _smallChange.Get();
    }

    void Slider::SetSmallChange(int value)
    {
        _smallChange.Set(value);
    }

    int Slider::GetLargeChange()
    {
        return _largeChange.Get();
    }

    void Slider::SetLargeChange(int value)
    {
        _largeChange.Set(value);
    }

    int Slider::GetTickFrequency()
    {
        return _tickFrequency.Get();
    }

    void Slider::SetTickFrequency(int value)
    {
        _tickFrequency.Set(value);
    }

    class wxSlider2 : public wxSlider, public wxWidgetExtender
    {
    public:
        wxSlider2(){}
        wxSlider2(wxWindow* parent,
            wxWindowID id,
            int value,
            int minValue,
            int maxValue,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxSL_HORIZONTAL,
            const wxValidator& validator = wxDefaultValidator,
            const wxString& name = wxASCII_STR(wxSliderNameStr))
        {
            Create(parent, id, value, minValue, maxValue,
                pos, size, style, validator, name);
        }
    };

    wxWindow* Slider::CreateWxWindowUnparented()
    {
        return new wxSlider2();
    }

    wxWindow* Slider::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxSlider2(
            parent,
            wxID_ANY,
            _value.Get(),
            _minimum.Get(),
            _maximum.Get(),
            wxDefaultPosition,
            wxDefaultSize,
            GetStyle());

        value->Bind(wxEVT_SLIDER, &Slider::OnSliderValueChanged, this);

        return value;
    }

    void Slider::OnSliderValueChanged(wxCommandEvent& event)
    {
        event.Skip();
        RaiseEvent(SliderEvent::ValueChanged);
    }

    wxSlider* Slider::GetSlider()
    {
        return dynamic_cast<wxSlider*>(GetWxWindow());
    }

    int Slider::RetrieveValue()
    {
        return GetSlider()->GetValue();
    }

    void Slider::ApplyValue(const int& value)
    {
        GetSlider()->SetValue(value);
    }

    int Slider::RetrieveMaximum()
    {
        return GetSlider()->GetMax();
    }

    void Slider::ApplyMaximum(const int& value)
    {
        GetSlider()->SetMax(value);
    }

    int Slider::RetrieveMinimum()
    {
        return GetSlider()->GetMin();
    }
    void Slider::ApplyMinimum(const int& value)
    {
        GetSlider()->SetMin(value);
    }

    int Slider::RetrieveSmallChange()
    {
        return GetSlider()->GetLineSize();
    }
    
    void Slider::ApplySmallChange(const int& value)
    {
        GetSlider()->SetLineSize(value);
    }

    int Slider::RetrieveLargeChange()
    {
        return GetSlider()->GetPageSize();
    }

    void Slider::ApplyLargeChange(const int& value)
    {
        GetSlider()->SetPageSize(value);
    }

    void Slider::ClearTicks()
    {
        GetSlider()->ClearTicks();
    }

    int Slider::RetrieveTickFrequency()
    {
        return GetSlider()->GetTickFreq();
    }

    void Slider::ApplyTickFrequency(const int& value)
    {
        GetSlider()->SetTickFreq(value);
    }

    long Slider::GetStyle()
    {
        bool isHorizontal = _orientation == SliderOrientation::Horizontal;

        auto orientation = isHorizontal ? wxSL_HORIZONTAL : wxSL_VERTICAL;

        auto getTickStyle = [&]()
        {
            switch (_tickStyle)
            {
            case SliderTickStyle::None:
                return 0;
            case SliderTickStyle::TopLeft:
                return wxSL_AUTOTICKS | (isHorizontal ? wxSL_TOP : wxSL_LEFT);
            case SliderTickStyle::BottomRight:
                return wxSL_AUTOTICKS | (isHorizontal ? wxSL_BOTTOM : wxSL_RIGHT);
            case SliderTickStyle::Both:
                return wxSL_AUTOTICKS | wxSL_BOTH;
            default:
                throwExNoInfo;
            }
        };

        return orientation | getTickStyle();
    }
}
