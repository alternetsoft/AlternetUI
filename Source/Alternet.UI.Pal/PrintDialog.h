#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class Window;
    class PageSettings;
    class PrintDocument;
    class PrinterSettings;

    class PrintDialog : public Object
    {
#include "Api/PrintDialog.inc"
    public:

    private:
        PrintDocument* _document = nullptr;

        bool _allowSomePages = true;
        bool _allowSelection = true;
        bool _allowPrintToFile = true;
        bool _showHelp = true;

        void ApplyProperties(wxPrintDialogData& data);
    };
}
