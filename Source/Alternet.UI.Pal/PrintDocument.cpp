#include "PrintDocument.h"
#include "DrawingContext.h"
#include "PrinterSettings.h"
#include "PageSettings.h"
#include "Api/DrawingTypes.h"
#include "PaperSizes.h"

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
        auto printerSettings = _owner->GetPrinterSettings();

        *minPage = printerSettings->GetMinimumPage();
        *maxPage = printerSettings->GetMaximumPage();
        *pageFrom = printerSettings->GetFromPage();
        *pageTo = printerSettings->GetToPage();

        // wxPrintout::GetPageInfo(minPage, maxPage, pageFrom, pageTo);
    }

    bool PrintDocument::Printout::GetHasMorePages()
    {
        return _hasMorePages;
    }

    void PrintDocument::Printout::SetHasMorePages(bool value)
    {
        _hasMorePages = value;
    }

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
        auto settings = GetPrinterSettingsCore();
        settings->AddRef();
        return settings;
    }

    PageSettings* PrintDocument::GetPageSettings()
    {
        auto settings = GetPageSettingsCore();
        settings->AddRef();
        return settings;
    }

    PageSettings* PrintDocument::GetPageSettingsCore()
    {
        if (_pageSettings == nullptr)
            _pageSettings = new PageSettings();

        return _pageSettings;
    }

    PrinterSettings* PrintDocument::GetPrinterSettingsCore()
    {
        if (_printerSettings == nullptr)
            _printerSettings = new PrinterSettings();

        return _printerSettings;
    }

    void PrintDocument::Print()
    {
        if (_printout != nullptr)
            throwExInvalidOpWithInfo(
                u"Another printing operation for this PrintDocument is in progress.");

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
        auto dc = new DrawingContext(_printout->GetDC(), false);
        dc->SetDoNotDeleteDC(true); // wxPrintout deletes the wxDC.
        return dc;
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

        auto pageSettings = GetPageSettingsCore();

        auto margins = Thickness(pageSettings->GetMarginLeft(), pageSettings->GetMarginTop(),
            pageSettings->GetMarginRight(), pageSettings->GetMarginBottom());

        data.SetMarginTopLeft(wxPoint(fromDip(margins.Left, nullptr), fromDip(margins.Top, nullptr)));
        data.SetMarginBottomRight(wxPoint(fromDip(margins.Right, nullptr),
            fromDip(margins.Bottom, nullptr)));

        return data;
    }

    wxPrintDialogData PrintDocument::GetPrintDialogData()
    {
        wxPrintDialogData data(GetPrintData());
        
        auto printerSettings = GetPrinterSettingsCore();

        data.SetFromPage(printerSettings->GetFromPage());
        data.SetToPage(printerSettings->GetToPage());

        data.SetMinPage(printerSettings->GetMinimumPage());
        data.SetMaxPage(printerSettings->GetMaximumPage());

        SetPrintRange(data, printerSettings->GetPrintRange());

        data.SetCollate(printerSettings->GetCollate());
        data.SetNoCopies(printerSettings->GetCopies());
        data.SetPrintToFile(printerSettings->GetPrintToFile());

        auto isOk = data.IsOk();

        return data;
    }

    wxPrintData PrintDocument::GetPrintData()
    {
        wxPrintData data;

        auto printerSettings = GetPrinterSettingsCore();

        auto printerName = wxStr(printerSettings->GetPrinterName().value_or(u""));

        data.SetPrinterName(printerName);

        auto pageSettings = GetPageSettingsCore();

        if (printerSettings->GetPrintFileName() != nullopt)
        {
            auto printFileName = wxStr(printerSettings->GetPrintFileName().value());
            data.SetFilename(printFileName);
        }

        auto duplexMode = GetWxDuplexMode(printerSettings->GetDuplex());

        data.SetDuplex(duplexMode);

		auto isColor = pageSettings->GetColor();

        data.SetColour(isColor);

        auto orientation = pageSettings->GetLandscape()
            ? wxPrintOrientation::wxLANDSCAPE
			: wxPrintOrientation::wxPORTRAIT;

        data.SetOrientation(orientation);

        if (pageSettings->GetUseCustomPaperSize())
        {
            data.SetPaperId(wxPaperSize::wxPAPER_NONE);

            auto customPaperSize = pageSettings->GetCustomPaperSize();

            auto paperWidth = (int)customPaperSize.Width;
			auto paperHeight = (int)customPaperSize.Height;

            data.SetPaperSize(wxSize(paperWidth, paperHeight));
        }
        else
        {
            auto paperSize = pageSettings->GetPaperSize();
            auto wxPaperSize = GetWxPaperSize(paperSize);

            data.SetPaperId(wxPaperSize);
        }

		auto quality = pageSettings->GetPrinterResolution();
		auto wxQuality = GetWxPrintQuality(quality);

        data.SetQuality(wxQuality);

		auto isOk = data.IsOk();

        return data;
    }

    wxPrintQuality PrintDocument::GetWxPrintQuality(PrinterResolutionKind value)
    {
        switch (value)
        {
        case PrinterResolutionKind::Draft:
            return wxPRINT_QUALITY_DRAFT;
        case PrinterResolutionKind::Low:
            return wxPRINT_QUALITY_LOW;
        case PrinterResolutionKind::Medium:
            return wxPRINT_QUALITY_MEDIUM;
        case PrinterResolutionKind::High:
            return wxPRINT_QUALITY_HIGH;
        default:
            return wxPRINT_QUALITY_DRAFT;
        }
    }

    PrinterResolutionKind PrintDocument::GetPrintQuality(wxPrintQuality value)
    {
        switch (value)
        {
        case wxPRINT_QUALITY_DRAFT:
            return PrinterResolutionKind::Draft;
        case wxPRINT_QUALITY_LOW:
            return PrinterResolutionKind::Low;
        case wxPRINT_QUALITY_MEDIUM:
            return PrinterResolutionKind::Medium;
        case wxPRINT_QUALITY_HIGH:
            return PrinterResolutionKind::High;
        default:
            return PrinterResolutionKind::High; // For values like "600". This is a DPI.
        }
    }

    wxDuplexMode PrintDocument::GetWxDuplexMode(Duplex value)
    {
        switch (value)
        {
        case Duplex::Simplex:
            return wxDUPLEX_SIMPLEX;
        case Duplex::Horizontal:
            return wxDUPLEX_HORIZONTAL;
        case Duplex::Vertical:
            return wxDUPLEX_VERTICAL;
        default:
            return wxDUPLEX_SIMPLEX;
        }
    }

    Duplex PrintDocument::GetDuplexMode(wxDuplexMode value)
    {
        switch (value)
        {
        case wxDUPLEX_SIMPLEX:
            return Duplex::Simplex;
        case wxDUPLEX_HORIZONTAL:
            return Duplex::Horizontal;
        case wxDUPLEX_VERTICAL:
            return Duplex::Vertical;
        default:
            return Duplex::Simplex;
        }
    }

    void PrintDocument::SetPrintRange(wxPrintDialogData& data, PrintRange range)
    {
        switch (range)
        {
        case PrintRange::AllPages:
            data.SetAllPages(true);
            data.SetSelection(false);
            break;
        case PrintRange::Selection:
            data.SetAllPages(false);
            data.SetSelection(true);
            break;
        case PrintRange::SomePages:
            data.SetAllPages(false);
            data.SetSelection(false);
            break;
        }
    }

    void PrintDocument::ApplyData(wxPrintDialogData& data)
    {
        ApplyData(data.GetPrintData());

        auto printerSettings = GetPrinterSettingsCore();

        printerSettings->SetFromPage(data.GetFromPage());
        printerSettings->SetToPage(data.GetToPage());
        printerSettings->SetMinimumPage(data.GetMinPage());
        printerSettings->SetMaximumPage(data.GetMaxPage());
        printerSettings->SetPrintRange(GetPrintRangeFromData(data));
        printerSettings->SetCollate(data.GetCollate());
        printerSettings->SetCopies(data.GetNoCopies());
        printerSettings->SetPrintToFile(data.GetPrintToFile());
    }

    void PrintDocument::ApplyData(wxPageSetupDialogData& data)
    {
        auto pageSettings = GetPageSettingsCore();

        auto topLeft = data.GetMarginTopLeft();
        auto bottomRight = data.GetMarginBottomRight();

        pageSettings->SetMarginLeft(toDip(topLeft.x, nullptr));
        pageSettings->SetMarginRight(toDip(bottomRight.x, nullptr));
        pageSettings->SetMarginTop(toDip(topLeft.y, nullptr));
        pageSettings->SetMarginBottom(toDip(bottomRight.y, nullptr));
    }

    PrintRange PrintDocument::GetPrintRangeFromData(wxPrintDialogData& data)
    {
        if (data.GetAllPages())
            return PrintRange::AllPages;
        if (data.GetSelection())
            return PrintRange::Selection;
        return PrintRange::SomePages;
    }

    void PrintDocument::ApplyData(wxPrintData& data)
    {
        auto printerSettings = GetPrinterSettingsCore();
        auto pageSettings = GetPageSettingsCore();

        printerSettings->SetPrinterName(data.GetPrinterName() == ""
            ? nullopt : optional<string>(wxStr(data.GetPrinterName())));
        printerSettings->SetPrintFileName(data.GetFilename() == ""
            ? nullopt : optional<string>(wxStr(data.GetFilename())));
        printerSettings->SetDuplex(GetDuplexMode(data.GetDuplex()));
        pageSettings->SetColor(data.GetColour());
        pageSettings->SetLandscape(data.GetOrientation() == wxPrintOrientation::wxLANDSCAPE);

        if (data.GetPaperId() == wxPaperSize::wxPAPER_NONE)
        {
            pageSettings->SetUseCustomPaperSize(true);
            auto& paperSize = data.GetPaperSize();
            pageSettings->SetCustomPaperSize(Size(paperSize.x, paperSize.y));
        }
        else
        {
            pageSettings->SetUseCustomPaperSize(false);
            pageSettings->SetPaperSize(GetPaperSize(data.GetPaperId()));
        }

        pageSettings->SetPrinterResolution(GetPrintQuality(data.GetQuality()));
    }
}
