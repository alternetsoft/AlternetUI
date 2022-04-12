#include "Mouse.h"

namespace Alternet::UI
{
    Mouse::Mouse()
    {
    }

    Mouse::~Mouse()
    {
    }

    Point Mouse::GetMousePosition()
    {
        return toDip(wxGetMousePosition(), nullptr);
    }

    void Mouse::OnMouseMove(wxMouseEvent& e, bool& handled)
    {
        MouseEventData data { };
        handled = RaiseEvent(MouseEvent::MouseMove, &data);
    }
}
