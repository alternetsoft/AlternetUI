#include "PrintDocument.h"
#include "DrawingContext.h"
#include "PrinterSettings.h"
#include "PageSettings.h"
#include "Api/DrawingTypes.h"

namespace Alternet::UI
{
    PrintDocument::Printout::Printout(PrintDocument* owner) : _owner(owner)
    {
    }

    bool PrintDocument::Printout::OnBeginDocument(int startPage, int endPage)
    {
        if (!wxPrintout::OnBeginDocument(startPage, endPage))
            return false;

        MapScreenSizeToPaper();

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

    bool PrintDocument::Printout::GetHasMorePages()
    {
        return _hasMorePages;
    }

    void PrintDocument::Printout::SetHasMorePages(bool value)
    {
        _hasMorePages = value;
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
        return _documentName;
    }

    void PrintDocument::SetDocumentName(const string& value)
    {
        _documentName = value;
    }

    PrinterSettings* PrintDocument::GetPrinterSettings()
    {
        if (_printerSettings == nullptr)
            _printerSettings = new PrinterSettings();

        _printerSettings->AddRef();
        return _printerSettings;
    }

    void PrintDocument::SetPrinterSettings(PrinterSettings* value)
    {
        if (_printerSettings == value)
            return;

        if (_printerSettings != nullptr)
            _printerSettings->Release();

        _printerSettings = value;

        if (_printerSettings != nullptr)
            _printerSettings->AddRef();
    }

    PageSettings* PrintDocument::GetDefaultPageSettings()
    {
        if (_defaultPageSettings == nullptr)
            _defaultPageSettings = new PageSettings();

        _defaultPageSettings->AddRef();
        return _defaultPageSettings;
    }

    void PrintDocument::SetDefaultPageSettings(PageSettings* value)
    {
        if (_defaultPageSettings == value)
            return;

        if (_defaultPageSettings != nullptr)
            _defaultPageSettings->Release();

        _defaultPageSettings = value;

        if (_defaultPageSettings != nullptr)
            _defaultPageSettings->AddRef();
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
        if (_printout == nullptr)
            throwExInvalidOp;

        return _printout->GetHasMorePages();
    }

    void PrintDocument::SetPrintPage_HasMorePages(bool value)
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        _printout->SetHasMorePages(value);
    }

    PageSettings* PrintDocument::GetPrintPage_PageSettings()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        return GetDefaultPageSettings();
    }

    Rect PrintDocument::GetPrintPage_MarginBounds()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        wxPageSetupData data;
        data.SetMarginTopLeft(wxPoint(10, 10));
        data.SetMarginBottomRight(wxPoint(10, 10));
        auto rect = _printout->GetLogicalPageMarginsRect(data);
        return toDip(rect, nullptr);
    }

    Rect PrintDocument::GetPrintPage_PhysicalPageBounds()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        int w = 0, h = 0;
        _printout->GetPageSizeMM(&w, &h);
        return Rect(0, 0, w, h);
    }

    Rect PrintDocument::GetPrintPage_PageBounds()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        auto rect = _printout->GetLogicalPaperRect();
        return toDip(rect, nullptr);
    }

    Rect PrintDocument::GetPrintPage_PrintablePageBounds()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        auto rect = _printout->GetLogicalPageRect();
        return toDip(rect, nullptr);
    }
}
