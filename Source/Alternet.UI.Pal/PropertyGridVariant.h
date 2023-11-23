#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

#include <wx/variant.h>
#include <wx/propgrid/advprops.h>

namespace Alternet::UI
{
    class PropertyGridVariant : public Object
    {
#include "Api/PropertyGridVariant.inc"
    public:
        wxVariant variant = wxVariant();

        static wxVariant& ToVar(void* handle);
        static void FromVariant(void* handle, wxVariant& value);
        static uint32_t _lastColorKind;

        PropertyGridVariant(wxVariant value);
    private:
    
    };
}
