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
        return Duplex();
    }
    void PrinterSettings::SetDuplex(Duplex value)
    {
    }
    int PrinterSettings::GetFromPage()
    {
        return 0;
    }
    void PrinterSettings::SetFromPage(int value)
    {
    }
    int PrinterSettings::GetToPage()
    {
        return 0;
    }
    void PrinterSettings::SetToPage(int value)
    {
    }
    int PrinterSettings::GetMinimumPage()
    {
        return 0;
    }
    void PrinterSettings::SetMinimumPage(int value)
    {
    }
    int PrinterSettings::GetMaximumPage()
    {
        return 0;
    }
    void PrinterSettings::SetMaximumPage(int value)
    {
    }
    PrintRange PrinterSettings::GetPrintRange()
    {
        return PrintRange();
    }
    void PrinterSettings::SetPrintRange(PrintRange value)
    {
    }
    bool PrinterSettings::GetCollate()
    {
        return false;
    }
    void PrinterSettings::SetCollate(bool value)
    {
    }
    int PrinterSettings::GetCopies()
    {
        return 0;
    }
    void PrinterSettings::SetCopies(int value)
    {
    }
    PageSettings* PrinterSettings::GetDefaultPageSettings()
    {
        return nullptr;
    }
    void PrinterSettings::SetDefaultPageSettings(PageSettings* value)
    {
    }
    bool PrinterSettings::GetPrintToFile()
    {
        return false;
    }
    void PrinterSettings::SetPrintToFile(bool value)
    {
    }
    optional<string> PrinterSettings::GetPrinterName()
    {
        return optional<string>();
    }
    void PrinterSettings::SetPrinterName(optional<string> value)
    {
    }
    bool PrinterSettings::GetIsValid()
    {
        return false;
    }
    bool PrinterSettings::GetIsDefaultPrinter()
    {
        return false;
    }
    optional<string> PrinterSettings::GetPrintFileName()
    {
        return optional<string>();
    }
    void PrinterSettings::SetPrintFileName(optional<string> value)
    {
    }
}
