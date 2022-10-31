#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PageSettings;

    class PrinterSettings : public Object
    {
#include "Api/PrinterSettings.inc"
    public:

    private:
        int _fromPage = 0;
        int _toPage = 0;

        int _minimumPage = 0;
        int _maximumPage = 0;

        PrintRange _printRange = PrintRange::AllPages;
        bool _collate = false;
        int _copies = 1;
        bool _printToFile = false;
        optional<string> _printerName;
        Duplex _duplex = Duplex::Simplex;
        optional<string> _printFileName;
    };
}
