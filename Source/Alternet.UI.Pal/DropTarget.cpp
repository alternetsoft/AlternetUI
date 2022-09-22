#include "DropTarget.h"
#include "Control.h"
#include "UnmanagedDataObject.h"

namespace Alternet::UI
{
    DropTarget::DropTarget(Control* control) : _control(control)
    {
        RecreateDataObject();
    }
    
    DropTarget::~DropTarget()
    {
    }

    void DropTarget::RecreateDataObject()
    {
        auto composite = new wxDataObjectComposite();
        composite->Add(new wxTextDataObject());
        composite->Add(new wxFileDataObject());
        composite->Add(new wxImageDataObject());
        SetDataObject(composite);
    }

    wxDataObjectComposite* DropTarget::GetDataObjectComposite()
    {
        return (wxDataObjectComposite*)GetDataObject();
    }

    void DropTarget::UpdateData()
    {
        RecreateDataObject();
        GetData();
    }

    wxDataObjectComposite* DropTarget::GetDataObjectWithoutEmptyData()
    {
        auto composite = GetDataObjectComposite();

        auto newComposite = new wxDataObjectComposite();
        
        auto text = UnmanagedDataObject::TryGetText(composite);
        if (text.has_value() && text.value().size() > 0)
            newComposite->Add(new wxTextDataObject(wxStr(text.value())));

        auto files = UnmanagedDataObject::TryGetFiles(composite);
        if (files.has_value() && files.value().Count() > 0)
        {
            auto fileObject = new wxFileDataObject();
            for (auto file : files.value())
                fileObject->AddFile(file);
            newComposite->Add(fileObject);
        }

#ifdef __WXMSW__ // The code below crashes on macOS/Linux for unknown reason.
        auto bitmap = UnmanagedDataObject::TryGetBitmap(composite);
        if (bitmap.has_value())
            newComposite->Add(new wxImageDataObject(bitmap.value().ConvertToImage()));
#endif

        return newComposite;
    }

    wxDragResult DropTarget::OnEnter(wxCoord x, wxCoord y, wxDragResult def)
    {
        UpdateData();
        return _control->RaiseDragEnter(wxPoint(x, y), def, GetDataObjectWithoutEmptyData());
    }
    
    void DropTarget::OnLeave()
    {
        _control->RaiseDragLeave();
    }
    
    wxDragResult DropTarget::OnData(wxCoord x, wxCoord y, wxDragResult def)
    {
        UpdateData();
        return _control->RaiseDragDrop(wxPoint(x, y), def, GetDataObjectWithoutEmptyData());
    }

    wxDragResult DropTarget::OnDragOver(wxCoord x, wxCoord y, wxDragResult def)
    {
        UpdateData();
        return _control->RaiseDragOver(wxPoint(x, y), def, GetDataObjectWithoutEmptyData());
    }
}
