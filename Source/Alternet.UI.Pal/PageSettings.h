#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PrinterSettings;

    class PageSettings : public Object
    {
#include "Api/PageSettings.inc"
    public:

    private:

        bool _color = true;
        Thickness _margins;
    };
}
