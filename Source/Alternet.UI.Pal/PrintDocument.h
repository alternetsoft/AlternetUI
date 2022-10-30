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
            virtual bool OnPrintPage(int page)  override;
            virtual void GetPageInfo(int* minPage, int* maxPage, int* pageFrom, int* pageTo) override;

        private:
            PrintDocument* _owner;
        };

    private:
        bool OnPrintPage(int page);
        Printout* _printout = nullptr;

    };
}
