#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "wx/aui/aui.h"
#include "AuiPaneInfo.h"

namespace Alternet::UI
{
    class AuiManager : public Object
    {
#include "Api/AuiManager.inc"
    public:
        static inline wxAuiManager* Manager(void* handle);

        static wxAuiPaneInfo& PaneInfo(void* paneInfo);
        static void* FromPaneInfo(wxAuiPaneInfo& paneInfo);
    private:
    
    };
}
