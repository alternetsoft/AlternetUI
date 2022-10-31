#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PrinterSettings;
    class PageSettings;
    class PrintDocument;
    class Window;

    class PageSetupDialog : public Object
    {
#include "Api/PageSetupDialog.inc"
    public:

    private:
        void ApplyProperties(wxPageSetupDialogData& data);

        PrintDocument* _document = nullptr;

        Thickness _minMargins;
        bool _minMarginsValueSet = false;
        bool _allowMargins = true;
        bool _allowOrientation = true;
        bool _allowPaper = true;
        bool _allowPrinter = true;
    };
}
