#include "DropTarget.h"
#include "Control.h"

namespace Alternet::UI
{
    DropTarget::DropTarget(Control* control) : _control(control), wxDropTarget(new wxDataObjectComposite())
    {
    }
    
    DropTarget::~DropTarget()
    {
    }

    wxDragResult DropTarget::OnEnter(wxCoord x, wxCoord y, wxDragResult def)
    {
        return wxDragResult();
    }
    
    void DropTarget::OnLeave()
    {
    }
    
    wxDragResult DropTarget::OnData(wxCoord x, wxCoord y, wxDragResult def)
    {
        return wxDragResult();
    }
}
