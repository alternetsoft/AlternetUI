#include "PrintDocument.h"

namespace Alternet::UI
{
    PrintDocument::PrintDocument()
    {
    }
    PrintDocument::~PrintDocument()
    {
    }
    string PrintDocument::GetDocumentName()
    {
        return string();
    }
    void PrintDocument::SetDocumentName(const string& value)
    {
    }
    PrinterSettings* PrintDocument::GetPrinterSettings()
    {
        return nullptr;
    }
    void PrintDocument::SetPrinterSettings(PrinterSettings* value)
    {
    }
    PageSettings* PrintDocument::GetDefaultPageSettings()
    {
        return nullptr;
    }
    void PrintDocument::SetDefaultPageSettings(PageSettings* value)
    {
    }
    DrawingContext* PrintDocument::GetPrintDrawingContext()
    {
        return nullptr;
    }
    bool PrintDocument::GetPrintHasMorePages()
    {
        return false;
    }
    PageSettings* PrintDocument::GetPrintPageSettings()
    {
        return nullptr;
    }
    Thickness PrintDocument::GetPrintPhysicalMarginBounds()
    {
        return Thickness();
    }
    Rect PrintDocument::GetMarginBounds()
    {
        return Rect();
    }
    Rect PrintDocument::GetPhysicalPageBounds()
    {
        return Rect();
    }
    Rect PrintDocument::GetPageBounds()
    {
        return Rect();
    }
    void PrintDocument::Print()
    {
    }
}
