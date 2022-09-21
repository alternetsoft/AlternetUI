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
    }
    
    DropTarget::~DropTarget()
    {
    }

    wxDragResult DropTarget::OnEnter(wxCoord x, wxCoord y, wxDragResult def)
    {
        GetData();
        return _control->RaiseDragEnter(wxPoint(x, y), def, (wxDataObjectComposite*)GetDataObject());
    }
    
    void DropTarget::OnLeave()
    {
        _control->RaiseDragLeave();
    }
    
    wxDragResult DropTarget::OnData(wxCoord x, wxCoord y, wxDragResult def)
    {
        GetData();
        return _control->RaiseDragDrop(wxPoint(x, y), def, (wxDataObjectComposite*)GetDataObject());
    }

    wxDragResult DropTarget::OnDragOver(wxCoord x, wxCoord y, wxDragResult def)
    {
        GetData();
        return _control->RaiseDragOver(wxPoint(x, y), def, (wxDataObjectComposite*)GetDataObject());
    }
}
