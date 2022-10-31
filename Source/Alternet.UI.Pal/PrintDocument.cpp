#include "PrintDocument.h"
#include "DrawingContext.h"

namespace Alternet::UI
{
    PrintDocument::Printout::Printout(PrintDocument* owner) : _owner(owner)
    {
    }

    bool PrintDocument::Printout::OnBeginDocument(int startPage, int endPage)
    {
        if (!wxPrintout::OnBeginDocument(startPage, endPage))
            return false;

        return !_owner->RaiseEvent(PrintDocumentEvent::BeginPrint);
    }

    void PrintDocument::Printout::OnEndDocument()
    {
        wxPrintout::OnEndDocument();
    }

    void PrintDocument::Printout::OnBeginPrinting()
    {
        wxPrintout::OnBeginPrinting();
    }

    void PrintDocument::Printout::OnEndPrinting()
    {
        _owner->RaiseEvent(PrintDocumentEvent::EndPrint);
    }

    void PrintDocument::Printout::OnPreparePrinting()
    {
        wxPrintout::OnPreparePrinting();
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
        *minPage = 1;
        *maxPage = 1;
        *pageFrom = 1;
        *pageTo = 1;

        //wxPrintout::GetPageInfo(minPage, maxPage, pageFrom, pageTo);
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

        _printout = dynamic_cast<Printout*>(CreatePrintout());

        ScopeGuard scope([&]
            {
                DeletePrintout();
            });

        wxPrintData printData;
        wxPrintDialogData printDialogData(printData);
        wxPrinter printer(&printDialogData);

        printer.Print(nullptr, _printout, /*prompt:*/false);
    }

    wxPrintout* PrintDocument::CreatePrintout()
    {
        if (_printout != nullptr)
            throwExNoInfo;

        return _printout = new Printout(this);
    }

    void PrintDocument::DeletePrintout()
    {
        if (_printout == nullptr)
            throwExNoInfo;

        delete _printout;
        _printout = nullptr;
    }

    void PrintDocument::ClearPrintout()
    {
        if (_printout == nullptr)
            throwExNoInfo;

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
