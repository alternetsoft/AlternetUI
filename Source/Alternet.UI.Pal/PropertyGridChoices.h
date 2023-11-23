#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "ImageSet.h"

#include <wx/propgrid/property.h>

namespace Alternet::UI
{
    class PropertyGridChoices : public Object
    {
#include "Api/PropertyGridChoices.inc"
    public:
        wxPGChoices choices = wxPGChoices();

        static wxPGChoiceEntry& Item(void* handle, uint32_t ind);
        static PropertyGridChoices* Choices(void* handle);
    private:
    
    };
}
