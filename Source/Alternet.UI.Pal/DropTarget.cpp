#include "DropTarget.h"
#include "Control.h"

namespace Alternet::UI
{
    //DropTarget::DropTarget(Control* control) : _control(control), wxDropTarget(new wxFileDataObject())
    DropTarget::DropTarget(Control* control) : _control(control)
    {
        auto composite = new wxDataObjectComposite();
        composite->Add(new wxTextDataObject());
        composite->Add(new wxFileDataObject());
        composite->Add(new wxImageDataObject());
        SetDataObject(composite);

        SetDefaultAction(wxDragResult::wxDragMove);
    }
    
    DropTarget::~DropTarget()
    {
    }

    wxDragResult DropTarget::OnEnter(wxCoord x, wxCoord y, wxDragResult def)
    {
        return wxDropTarget::OnEnter(x, y, def);
    }
    
    void DropTarget::OnLeave()
    {
        return wxDropTarget::OnLeave();
    }
    
    wxDragResult DropTarget::OnData(wxCoord x, wxCoord y, wxDragResult def)
    {
        bool r = GetData();
        //auto fn = ((wxFileDataObject*)GetDataObject())->GetFilenames();
        return wxDragResult::wxDragMove;
    }

    wxDragResult DropTarget::OnDragOver(wxCoord x, wxCoord y, wxDragResult def)
    {
        return wxDropTarget::OnDragOver(x, y, def);
    }
    
    bool DropTarget::OnDrop(wxCoord x, wxCoord y)
    {
        return wxDropTarget::OnDrop(x, y);
    }
}
