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

    private:
    
    };
}
