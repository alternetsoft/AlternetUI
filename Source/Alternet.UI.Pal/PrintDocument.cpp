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
    void PrintDocument::Print()
    {
    }
    DrawingContext* PrintDocument::GetPrintPage_DrawingContext()
    {
        return nullptr;
    }
    bool PrintDocument::GetPrintPage_HasMorePages()
    {
        return false;
    }
    void PrintDocument::SetPrintPage_HasMorePages(bool value)
    {
    }
    PageSettings* PrintDocument::GetPrintPage_PageSettings()
    {
        return nullptr;
    }
    Thickness PrintDocument::GetPrintPage_PhysicalMarginBounds()
    {
        return Thickness();
    }
    Rect PrintDocument::GetPrintPage_MarginBounds()
    {
        return Rect();
    }
    Rect PrintDocument::GetPrintPage_PhysicalPageBounds()
    {
        return Rect();
    }
    Rect PrintDocument::GetPrintPage_PageBounds()
    {
        return Rect();
    }
}
