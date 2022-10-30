#include "PrintDocument.h"
#include "DrawingContext.h"

namespace Alternet::UI
{
    PrintDocument::Printout::Printout(PrintDocument* owner) : _owner(owner)
    {
    }

    bool PrintDocument::Printout::OnBeginDocument(int startPage, int endPage)
    {
        return true;
    }

    void PrintDocument::Printout::OnEndDocument()
    {
    }

    void PrintDocument::Printout::OnBeginPrinting()
    {
    }

    void PrintDocument::Printout::OnEndPrinting()
    {
    }

    void PrintDocument::Printout::OnPreparePrinting()
    {
    }

    bool PrintDocument::Printout::HasPage(int page)
    {
        return page == 1;
    }

    bool PrintDocument::Printout::OnPrintPage(int page)
    {
        return _owner->OnPrintPage(page);
    }

    void PrintDocument::Printout::GetPageInfo(int* minPage, int* maxPage, int* pageFrom, int* pageTo)
    {
        wxPrintout::GetPageInfo(minPage, maxPage, pageFrom, pageTo);
    }

    // =========================================================

    PrintDocument::PrintDocument()
    {
    }

    PrintDocument::~PrintDocument()
    {
    }

    bool PrintDocument::OnPrintPage(int page)
    {
        return !RaiseEvent(PrintDocumentEvent::PrintPage);
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
        if (_printout != nullptr)
            throwExInvalidOpWithInfo(u"Another printing operation for this PrintDocument is in progress.");

        _printout = new Printout(this);
        wxPrinter printer;
        printer.Print(ParkingWindow::GetWindow(), _printout, /*prompt:*/false);
        delete _printout;
        _printout = nullptr;
    }

    DrawingContext* PrintDocument::GetPrintPage_DrawingContext()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        return new DrawingContext(_printout->GetDC());
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
