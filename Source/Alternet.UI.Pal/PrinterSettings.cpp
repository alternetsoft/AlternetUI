#include "PrinterSettings.h"

namespace Alternet::UI
{
    PrinterSettings::PrinterSettings()
    {
    }
    
    PrinterSettings::~PrinterSettings()
    {
    }

    Duplex PrinterSettings::GetDuplex()
    {
        return _duplex;
    }

    void PrinterSettings::SetDuplex(Duplex value)
    {
        _duplex = value;
    }

    int PrinterSettings::GetFromPage()
    {
        return _fromPage;
    }

    void PrinterSettings::SetFromPage(int value)
    {
        _fromPage = value;
    }

    int PrinterSettings::GetToPage()
    {
        return _toPage;
    }

    void PrinterSettings::SetToPage(int value)
    {
        _toPage = value;
    }

    int PrinterSettings::GetMinimumPage()
    {
        return _minimumPage;
    }

    void PrinterSettings::SetMinimumPage(int value)
    {
        _minimumPage = value;
    }

    int PrinterSettings::GetMaximumPage()
    {
        return _maximumPage;
    }

    void PrinterSettings::SetMaximumPage(int value)
    {
        _maximumPage = value;
    }

    PrintRange PrinterSettings::GetPrintRange()
    {
        return _printRange;
    }

    void PrinterSettings::SetPrintRange(PrintRange value)
    {
        _printRange = value;
    }

    bool PrinterSettings::GetCollate()
    {
        return _collate;
    }

    void PrinterSettings::SetCollate(bool value)
    {
        _collate = value;
    }

    int PrinterSettings::GetCopies()
    {
        return _copies;
    }

    void PrinterSettings::SetCopies(int value)
    {
        _copies = value;
    }

    bool PrinterSettings::GetPrintToFile()
    {
        return _printToFile;
    }

    void PrinterSettings::SetPrintToFile(bool value)
    {
        _printToFile = value;
    }

    optional<string> PrinterSettings::GetPrinterName()
    {
        return _printerName;
    }

    void PrinterSettings::SetPrinterName(optional<string> value)
    {
        _printerName = value;
    }

    bool PrinterSettings::GetIsValid()
    {
        if (_printerName == nullopt)
            return true;

        wxPrintData data;
        data.SetPrinterName(wxStr(_printerName.value()));
        return data.IsOk();
    }

    bool PrinterSettings::GetIsDefaultPrinter()
    {
        return _printerName == nullopt;
    }

    optional<string> PrinterSettings::GetPrintFileName()
    {
        return _printFileName;
    }

    void PrinterSettings::SetPrintFileName(optional<string> value)
    {
        _printFileName = value;
    }
}