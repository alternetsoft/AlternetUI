#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Control.h"

#include <wx/treectrl.h>

namespace Alternet::UI
{
    class WxTreeViewFactory : public Object
    {
#include "Api/WxTreeViewFactory.inc"
    public:
    
    private:
    
    };
}
