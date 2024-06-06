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
        wxWindow* CreateWxWindowUnparented() override;
        AuiNotebook(long styles);

        void FromEventData(wxAuiNotebookEvent& event);
        void OnPageClose(wxAuiNotebookEvent& event);
        void OnPageClosed(wxAuiNotebookEvent& event);
        void OnPageChanged(wxAuiNotebookEvent& event);
        void OnPageChanging(wxAuiNotebookEvent& event);
        void OnPageButton(wxAuiNotebookEvent& event);
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
        int _eventSelection = 0;
        int _eventOldSelection = 0;
        long _createStyle = wxAUI_NB_DEFAULT_STYLE;
    };
}
