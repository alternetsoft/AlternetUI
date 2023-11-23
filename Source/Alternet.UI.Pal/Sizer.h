#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/sizer.h>

namespace Alternet::UI
{
    class Sizer : public Object
    {
#include "Api/Sizer.inc"
    public:
        wxSizer* sizer = nullptr;
    private:
    
    };
}
