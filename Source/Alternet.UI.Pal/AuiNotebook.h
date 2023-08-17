#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "ImageSet.h"
#include <wx/aui/aui.h>

namespace Alternet::UI
{
    class AuiNotebook : public Control
    {
#include "Api/AuiNotebook.inc"
    public:
        wxAuiNotebook* GetNotebook();
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void FromEventData(wxAuiNotebookEvent& event);
        void OnPageClose(wxAuiNotebookEvent& event);
        void OnPageClosed(wxAuiNotebookEvent& event);
        void OnPageChanged(wxAuiNotebookEvent& event);
        void OnPageChanging(wxAuiNotebookEvent& event);
        void OnWindowListButton(wxAuiNotebookEvent& event);
        void OnBeginDrag(wxAuiNotebookEvent& event);
        void OnEndDrag(wxAuiNotebookEvent& event);
        void OnDragMotion(wxAuiNotebookEvent& event);
        void OnAllowTabDrop(wxAuiNotebookEvent& event);
        void OnDragDone(wxAuiNotebookEvent& event);
        void OnTabMiddleMouseDown(wxAuiNotebookEvent& event);
        void OnTabMiddleMouseUp(wxAuiNotebookEvent& event);
        void OnTabRightMouseDown(wxAuiNotebookEvent& event);
        void OnTabRightMouseUp(wxAuiNotebookEvent& event);
        void OnBgDclickMouse(wxAuiNotebookEvent& event);
    private:
    
    };
}
