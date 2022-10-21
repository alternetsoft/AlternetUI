#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Region : public Object
    {
#include "Api/Region.inc"
    public:
        wxRegion GetRegion();

    private:
        wxWindow* GetWindow();

        wxRegion _region;
    };
}