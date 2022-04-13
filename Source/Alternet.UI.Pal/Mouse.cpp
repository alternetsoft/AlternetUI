#include "Mouse.h"

namespace Alternet::UI
{
    Mouse::Mouse()
    {
    }

    Mouse::~Mouse()
    {
    }

    Point Mouse::GetPosition()
    {
        return toDip(wxGetMousePosition(), nullptr);
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
            wxASSERT(false);
            throw 0;
        }
    }

    void Mouse::OnMouseMove(wxMouseEvent& e, bool& handled)
    {
        MouseEventData data { e.GetTimestamp() };
        handled = RaiseEvent(MouseEvent::MouseMove, &data);
    }
    
    void Mouse::OnMouseDown(wxMouseEvent& e, MouseButton changedButton, bool& handled)
    {
        MouseButtonEventData data { e.GetTimestamp(), changedButton };
        handled = RaiseEvent(MouseEvent::MouseDown, &data);
    }
    
    void Mouse::OnMouseUp(wxMouseEvent& e, MouseButton changedButton, bool& handled)
    {
        MouseButtonEventData data{ e.GetTimestamp(), changedButton };
        handled = RaiseEvent(MouseEvent::MouseUp, &data);
    }
    
    void Mouse::OnMouseWheel(wxMouseEvent& e, bool& handled)
    {
        MouseWheelEventData data{ e.GetTimestamp(), e.GetWheelRotation()};
        handled = RaiseEvent(MouseEvent::MouseWheel, &data);
    }

    void Mouse::OnMouseDoubleClick(wxMouseEvent& e, MouseButton changedButton, bool& handled)
    {
        MouseButtonEventData data{ e.GetTimestamp(), changedButton };
        handled = RaiseEvent(MouseEvent::MouseDoubleClick, &data);
    }
}
