#include "Mouse.h"

namespace Alternet::UI
{
    Mouse::Mouse()
    {
    }

    Mouse::~Mouse()
    {
    }

    PointI Mouse::GetPosition()
    {
        return wxGetMousePosition();
    }

    MouseButtonState Mouse::GetButtonState(MouseButton button)
    {
        auto state = wxGetMouseState();
        
        switch (button)
        {
        case MouseButton::Left:
            return state.LeftIsDown() ? MouseButtonState::Pressed : MouseButtonState::Released;
        case MouseButton::Middle:
            return state.MiddleIsDown() ? MouseButtonState::Pressed : MouseButtonState::Released;
        case MouseButton::Right:
            return state.RightIsDown() ? MouseButtonState::Pressed : MouseButtonState::Released;
        case MouseButton::XButton1:
            return state.Aux1IsDown() ? MouseButtonState::Pressed : MouseButtonState::Released;
        case MouseButton::XButton2:
            return state.Aux2IsDown() ? MouseButtonState::Pressed : MouseButtonState::Released;
        default:
            return MouseButtonState::Released;
        }
    }

    Control* Mouse::GetEventTargetControl(wxEvent& e)
    {
        Control* targetControl = nullptr;
        auto eventObject = e.GetEventObject();
        auto window = dynamic_cast<wxWindow*>(eventObject);
        if (window != nullptr)
            targetControl = Control::TryFindControlByWxWindow(window);
        return targetControl;
    }

    void OnMouseNop()
    {
    }

    void Mouse::OnMouse(int eventKind, wxMouseEvent& e, bool& handled)
    {
        auto clickCount = e.GetClickCount();

        if (clickCount > 1)
        {
            OnMouseNop();
        }

        MouseEventData data { eventKind, e.GetTimestamp(), GetEventTargetControl(e), e.GetWheelRotation(), clickCount};
        handled = RaiseStaticEvent(MouseEvent::MouseChanged, &data);
    }
}
