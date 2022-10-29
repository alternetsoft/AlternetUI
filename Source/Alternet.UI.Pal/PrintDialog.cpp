#include "PrintDialog.h"

namespace Alternet::UI
{
    PrintDialog::PrintDialog()
    {
    }
    PrintDialog::~PrintDialog()
    {
    }
    bool PrintDialog::GetAllowSomePages()
    {
        return false;
    }
    void PrintDialog::SetAllowSomePages(bool value)
    {
    }
    bool PrintDialog::GetAllowSelection()
    {
        return false;
    }
    void PrintDialog::SetAllowSelection(bool value)
    {
    }
    bool PrintDialog::GetAllowPrintToFile()
    {
        return false;
    }
    void PrintDialog::SetAllowPrintToFile(bool value)
    {
    }
    bool PrintDialog::GetShowHelp()
    {
        return false;
    }
    void PrintDialog::SetShowHelp(bool value)
    {
    }
    PrintDocument* PrintDialog::GetDocument()
    {
        return nullptr;
    }
    void PrintDialog::SetDocument(PrintDocument* value)
    {
    }
    PrinterSettings* PrintDialog::GetPrinterSettings()
    {
        return nullptr;
    }
    void PrintDialog::SetPrinterSettings(PrinterSettings* value)
    {
    }
    ModalResult PrintDialog::ShowModal(Window* owner)
    {
        return ModalResult();
    }
}
