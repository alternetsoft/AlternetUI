#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include <wx/aui/aui.h>

namespace Alternet::UI
{
    class AuiManager : public Object
    {
#include "Api/AuiManager.inc"
    public:
        wxAuiManager* Manager(void* handle);

    private:
    
    };
}
