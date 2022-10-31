#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PrinterSettings;
    class PageSettings;
    class DrawingContext;

    class PrintDocument : public Object
    {
#include "Api/PrintDocument.inc"
    public:
        wxPrintout* CreatePrintout();
        void DeletePrintout();
        void ClearPrintout();

        wxPageSetupDialogData GetPageSetupDialogData();

        wxPrintDialogData GetPrintDialogData();
        wxPrintData GetPrintData();

        void ApplyData(wxPrintDialogData& data);
        void ApplyData(wxPrintData& data);
        void ApplyData(wxPageSetupDialogData& data);

    private:
        class Printout : public wxPrintout
        {
        public:
            Printout(PrintDocument* owner);

            virtual bool OnBeginDocument(int startPage, int endPage) override;
            virtual void OnEndDocument() override;
            virtual void OnBeginPrinting() override;
            virtual void OnEndPrinting() override;

            virtual void OnPreparePrinting() override;

            virtual bool HasPage(int page) override;
            virtual bool OnPrintPage(int page) override;
            virtual void GetPageInfo(int* minPage, int* maxPage, int* pageFrom, int* pageTo) override;

            bool GetHasMorePages();
            void SetHasMorePages(bool value);

            void SetDCMapping();
        private:
            PrintDocument* _owner;

            bool _hasMorePages = false;
            int _lastPrintedPageNumber = 0;
        };

    private:
        bool OnPrintPage(int page);
        int _currentPageNumber = 0;
        Printout* _printout = nullptr;
        string _documentName = u"Print Document";
        bool _originAtMargins = false;

        PrinterSettings* _printerSettings = nullptr;
        PageSettings* _pageSettings = nullptr;

        PageSettings* GetPageSettingsCore();
        PrinterSettings* GetPrinterSettingsCore();

        wxPrintQuality GetWxPrintQuality(PrinterResolutionKind value);
        PrinterResolutionKind GetPrintQuality(wxPrintQuality value);

        wxDuplexMode GetWxDuplexMode(Duplex value);
        Duplex GetDuplexMode(wxDuplexMode value);

        void SetPrintRange(wxPrintDialogData& data, PrintRange range);
        PrintRange GetPrintRangeFromData(wxPrintDialogData& data);
    };
}
