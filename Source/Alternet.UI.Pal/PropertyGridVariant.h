#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "wx/variant.h"

namespace Alternet::UI
{
    class PropertyGridVariant : public Object
    {
#include "Api/PropertyGridVariant.inc"
    public:
        wxVariant variant = wxVariant();

        static wxVariant& ToVar(void* handle);
        static void FromVariant(void* handle, wxVariant& value);
    private:
    
    };
}
