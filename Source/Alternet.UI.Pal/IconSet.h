#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Image.h"

namespace Alternet::UI
{
    class IconSet : public Object
    {
#include "Api/IconSet.inc"
    public:
    private:
        wxIconBundle _iconBundle;
    };
}
