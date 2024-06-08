#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include "ImageSet.h"

#include <wx/aui/aui.h>

namespace Alternet::UI
{                          
    class AuiToolBar : public Control
    {
#include "Api/AuiToolBar.inc"
    public:                  
        wxAuiToolBar* GetToolbar();
        AuiToolBar(long styles);
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;
        int _eventToolId = 0;
        bool _eventIsDropDownClicked = false;
        Int32Point _eventClickPoint = Int32Point();
        Int32Rect _eventItemRect = Int32Rect();
    private:
        long _createStyle = wxAUI_TB_DEFAULT_STYLE;

        void OnToolbarCommand(wxCommandEvent& event);
        void OnLeftDown(wxMouseEvent& evt);
        void FromEventData(wxAuiToolBarEvent& event);
        void OnToolDropDown(wxAuiToolBarEvent& event);
        void OnBeginDrag(wxAuiToolBarEvent& event);
        void OnMiddleClick(wxAuiToolBarEvent& event);
        void OnOverflowClick(wxAuiToolBarEvent& event);
        void OnRightClick(wxAuiToolBarEvent& event);
    };
}
