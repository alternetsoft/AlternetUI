#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/validate.h>
#include <wx/valgen.h>
#include <wx/propgrid/props.h>
#include <wx/valnum.h>
#include <wx/valtext.h>

namespace Alternet::UI
{
    class Validator : public Object
    {
#include "Api/Validator.inc"
    public:
    
    private:
    
    };
}
