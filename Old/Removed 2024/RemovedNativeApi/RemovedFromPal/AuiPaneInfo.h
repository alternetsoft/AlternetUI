#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "ImageSet.h"

#include <wx/aui/aui.h>

namespace Alternet::UI
{
    class AuiPaneInfo : public Object
    {
#include "Api/AuiPaneInfo.inc"
    public:
        wxAuiPaneInfo _paneInfoV = wxAuiPaneInfo();
        wxAuiPaneInfo& _paneInfo = _paneInfoV;
        
        static wxAuiPaneInfo& PaneInfo(void* paneInfo);
    private:
    
    };
}
