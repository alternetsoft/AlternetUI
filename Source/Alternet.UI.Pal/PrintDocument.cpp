#include "PrintDocument.h"
#include "DrawingContext.h"
#include "PrinterSettings.h"
#include "PageSettings.h"
#include "Api/DrawingTypes.h"

namespace Alternet::UI
{
    PrintDocument::Printout::Printout(PrintDocument* owner) :
        wxPrintout::wxPrintout(wxStr(owner->GetDocumentName())),
        _owner(owner)
    {
    }

    bool PrintDocument::Printout::OnBeginDocument(int startPage, int endPage)
    {
        if (!wxPrintout::OnBeginDocument(startPage, endPage))
            return false;

        SetDCMapping();

        _lastPrintedPageNumber = 0;

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
        if (page == 1)
            return true;

        if (_lastPrintedPageNumber == page - 1)
            return _hasMorePages;

        return false;
    }

    bool PrintDocument::Printout::OnPrintPage(int page)
    {
        SetDCMapping();
        _lastPrintedPageNumber = page;
        return _owner->OnPrintPage(page);
    }

    void PrintDocument::Printout::SetDCMapping()
    {
        MapScreenSizeToDevice();

        if (_owner->GetOriginAtMargins())
            MapScreenSizeToPageMargins(_owner->GetPageSetupDialogData());
        else
            MapScreenSizeToPage();
    }

    void PrintDocument::Printout::GetPageInfo(int* minPage, int* maxPage, int* pageFrom, int* pageTo)
    {
        wxPrintout::GetPageInfo(minPage, maxPage, pageFrom, pageTo);
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
        _currentPageNumber = page;
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
        auto settings = GetDefaultPageSettingsCore();
        settings->AddRef();
        return settings;
    }

    PageSettings* PrintDocument::GetDefaultPageSettingsCore()
    {
        if (_defaultPageSettings == nullptr)
            _defaultPageSettings = new PageSettings();

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

        auto printDialogData = GetPrintDialogData();
        wxPrinter printer(& printDialogData);

        printer.Print(nullptr, _printout, /*prompt:*/false);
    }

    bool PrintDocument::GetOriginAtMargins()
    {
        return _originAtMargins;
    }

    void PrintDocument::SetOriginAtMargins(bool value)
    {
        _originAtMargins = value;
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
        ClearPrintout();
    }

    void PrintDocument::ClearPrintout()
    {
        if (_printout == nullptr)
            throwExNoInfo;

        _printout = nullptr;
    }

    int PrintDocument::GetPrintPage_PageNumber()
    {
        if (_printout == nullptr)
            throwExNoInfo;

        return _currentPageNumber;
    }

    DrawingContext* PrintDocument::GetPrintPage_DrawingContext()
    {
        if (_printout == nullptr)
            throwExInvalidOp;

        //return new DrawingContext(_printout->GetDC(), [&]() { _printout->SetDCMapping(); });
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

        auto rect = _printout->GetLogicalPageMarginsRect(GetPageSetupDialogData());
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

    wxPageSetupDialogData PrintDocument::GetPageSetupDialogData()
    {
        wxPageSetupDialogData data;

        auto settings = GetDefaultPageSettingsCore();

        auto m = settings->GetMargins();

        data.SetMarginTopLeft(wxPoint(fromDip(m.Left, nullptr), fromDip(m.Top, nullptr)));
        data.SetMarginBottomRight(wxPoint(fromDip(m.Right, nullptr), fromDip(m.Bottom, nullptr)));

        return data;
    }

    wxPrintDialogData PrintDocument::GetPrintDialogData()
    {
        wxPrintDialogData data(GetPrintData());
        return data;
    }

    wxPrintData PrintDocument::GetPrintData()
    {
        wxPrintData data;

        auto settings = GetDefaultPageSettingsCore();
        data.SetColour(settings->GetColor());

        return data;
    }
}
