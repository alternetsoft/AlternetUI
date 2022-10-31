#include "PrintPreviewDialog.h"
#include "PrintDocument.h"
#include "Window.h"

namespace Alternet::UI
{
    PrintPreviewDialog::PrintPreviewDialog()
    {
    }

    PrintPreviewDialog::~PrintPreviewDialog()
    {
        SetDocument(nullptr);
    }

    PrintDocument* PrintPreviewDialog::GetDocument()
    {
        _document->AddRef();
        return _document;
    }

    void PrintPreviewDialog::SetDocument(PrintDocument* value)
    {
        if (_document != nullptr)
            _document->Release();

        _document = value;

        if (_document != nullptr)
            _document->AddRef();
    }

    optional<string> PrintPreviewDialog::GetTitle()
    {
        return _title;
    }

    void PrintPreviewDialog::SetTitle(optional<string> value)
    {
        _title = value;
    }

    void PrintPreviewDialog::Show(Window* owner)
    {
        if (_document == nullptr)
            throwExInvalidOpWithInfo(u"Cannot show the print preview dialog when the document is null.");


        auto previewPrintout = _document->CreatePrintout();

        ScopeGuard scope([&]
            {
                delete previewPrintout;
            });

        wxPrintData printData;
        wxPrintDialogData printDialogData(printData);

        wxPrintPreview preview(previewPrintout, nullptr, &printDialogData);
        if (!preview.IsOk())
            throwEx(u"Print preview failed.");

        auto frame = new wxPreviewFrame(
            &preview,
            owner == nullptr ? ParkingWindow::GetWindow() : owner->GetWxWindow(),
            wxStr(_title.value_or(u"Print Preview")));
        
        frame->InitializeWithModality(wxPreviewFrameModalityKind::wxPreviewFrame_NonModal);
        frame->Show();
    }
}
