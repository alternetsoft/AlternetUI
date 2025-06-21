#include "Slider.h"

namespace Alternet::UI
{
    Slider::Slider()
    {
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
        _tickStyle = value;
        RecreateWxWindowIfNeeded();
    }

    int Slider::GetMinimum()
    {
        return GetSlider()->GetMin();
    }

    void Slider::SetMinimum(int value)
    {
        GetSlider()->SetMin(value);
    }

    int Slider::GetMaximum()
    {
        return GetSlider()->GetMax();
    }

    void Slider::SetMaximum(int value)
    {
        GetSlider()->SetMax(value);
    }

    int Slider::GetValue()
    {
        return GetSlider()->GetValue();
    }

    void Slider::SetValue(int value)
    {
        GetSlider()->SetValue(value);
    }

    int Slider::GetSmallChange()
    {
        return GetSlider()->GetLineSize();
    }

    void Slider::SetSmallChange(int value)
    {
        GetSlider()->SetLineSize(value);
    }

    int Slider::GetLargeChange()
    {
        return GetSlider()->GetPageSize();
    }

    void Slider::SetLargeChange(int value)
    {
        GetSlider()->SetPageSize(value);
    }

    int Slider::GetTickFrequency()
    {
        return GetSlider()->GetTickFreq();
    }

    void Slider::SetTickFrequency(int value)
    {
        GetSlider()->SetTickFreq(value);
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
            0,
            0,
            100,
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

    void Slider::ClearTicks()
    {
        GetSlider()->ClearTicks();
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
            default:
                return wxSL_AUTOTICKS | (isHorizontal ? wxSL_TOP : wxSL_LEFT);
            case SliderTickStyle::BottomRight:
                return wxSL_AUTOTICKS | (isHorizontal ? wxSL_BOTTOM : wxSL_RIGHT);
            case SliderTickStyle::Both:
                return wxSL_AUTOTICKS | wxSL_BOTH;
            }
        };

        return orientation | getTickStyle();
    }
}
