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

    void PrintPreviewDialog::ShowModal(Window* owner)
    {
        if (_state != nullptr)
            return;

        if (_document == nullptr)
            throwExInvalidOpWithInfo(u"Cannot show the print preview dialog when the document is null.");

        _state = new State();

        _state->document = _document;
        _state->document->AddRef();

        _state->previewPrintout = _document->CreatePrintout();

        auto printData = _document->GetPrintData();

        _state->printPreview = new wxPrintPreview(_state->previewPrintout, nullptr, &printData);
        if (!_state->printPreview->IsOk())
            throwEx(u"Print preview failed.");

        _state->frame = new wxPreviewFrame(
            _state->printPreview,
            owner == nullptr ? ParkingWindow::GetWindow() : owner->GetWxWindow(),
            wxStr(_title.value_or(u"Print Preview")),
            wxDefaultPosition,
            wxSize(800, 600));
        
        _state->frame->InitializeWithModality(wxPreviewFrameModalityKind::wxPreviewFrame_AppModal);
        _state->frame->Bind(wxEVT_CLOSE_WINDOW, &PrintPreviewDialog::OnClose, this);
        _state->frame->Show();
        
        auto loop = _state->eventLoop = new wxEventLoop();
        loop->Run();
        delete loop;
    }

    void PrintPreviewDialog::OnClose(wxCloseEvent& event)
    {
        if (_state == nullptr)
            throwExNoInfo;

        event.Skip();

        _state->frame->Unbind(wxEVT_CLOSE_WINDOW, &PrintPreviewDialog::OnClose, this);
        
        _state->document->ClearPrintout();
        _state->document->Release();

        _state->eventLoop->Exit();

        _state = nullptr;
    }
}
