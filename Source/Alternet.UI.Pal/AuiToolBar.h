#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"
#include <wx/aui/aui.h>

namespace Alternet::UI
{
    class AuiToolBar : public Control
    {
#include "Api/AuiToolBar.inc"
    public:
        wxAuiToolBar* GetToolbar();
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

    private:
    
    };
}
