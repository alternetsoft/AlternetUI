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

    private:

        Control* _control;

        BYREF_ONLY(DropTarget);
    };
}
