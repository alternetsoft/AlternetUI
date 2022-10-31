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
        bool _landscape = false;
        Thickness _margins;
        PaperKind _paperSize = PaperKind::Letter;
        PrinterResolutionKind _printerResolutionKind = PrinterResolutionKind::High;
        Size _customPaperSize;
        bool _useCustomPaperSize = false;
    };
}
