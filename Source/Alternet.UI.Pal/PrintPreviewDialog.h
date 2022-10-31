#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class PrintDocument;
    class Window;

    class PrintPreviewDialog : public Object
    {
#include "Api/PrintPreviewDialog.inc"
    public:

    private:
        PrintDocument* _document = nullptr;
        optional<string> _title;

        struct State
        {
            PrintDocument* document = nullptr;
            wxPrintout* previewPrintout = nullptr;
            wxPreviewFrame* frame = nullptr;
            wxPrintPreview* printPreview = nullptr;
            wxEventLoop* eventLoop = nullptr;
        };

        State* _state = nullptr;

        void OnClose(wxCloseEvent& event);
    };
}
