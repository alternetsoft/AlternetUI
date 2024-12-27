#pragma once
#include "Common.h"

namespace Alternet::UI
{
    class Control;

    class DropTarget : public wxDropTarget
    {
    public:
        DropTarget(Control* control);
        virtual ~DropTarget();

        virtual wxDragResult OnEnter(wxCoord x, wxCoord y, wxDragResult def) override;
        virtual void OnLeave() override;
        
        virtual wxDragResult OnData(wxCoord x, wxCoord y, wxDragResult def) override;

        virtual wxDragResult OnDragOver(wxCoord x, wxCoord y, wxDragResult def) override;

        Control* _control;

    private:

        void RecreateDataObject();
        wxDataObjectComposite* GetDataObjectComposite();
        void UpdateData();
        wxDataObjectComposite* GetDataObjectWithoutEmptyData();

        BYREF_ONLY(DropTarget);
    };
}
