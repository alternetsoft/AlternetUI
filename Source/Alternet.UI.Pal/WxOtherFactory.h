#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "ImageSet.h"
#include "Font.h"

#include <wx/richtooltip.h>
#include <wx/tooltip.h>            
#include <wx/display.h>
#include <wx/cursor.h>
#include <wx/caret.h>
#include <wx/private/richtooltip.h>
#include <wx/generic/private/richtooltip.h>
#include <wx/renderer.h>
#include <wx/fswatcher.h>

namespace Alternet::UI
{
    class WxOtherFactory : public Object
    {
#include "Api/WxOtherFactory.inc"
    public:
    
    private:
    
    };
}
