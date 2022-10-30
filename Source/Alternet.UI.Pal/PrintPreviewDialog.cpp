#include "PrintPreviewDialog.h"

namespace Alternet::UI
{
    PrintPreviewDialog::PrintPreviewDialog()
    {
    }

    PrintPreviewDialog::~PrintPreviewDialog()
    {
    }

    PrintDocument* PrintPreviewDialog::GetDocument()
    {
        return nullptr;
    }

    void PrintPreviewDialog::SetDocument(PrintDocument* value)
    {
    }

    ModalResult PrintPreviewDialog::ShowModal(Window* owner)
    {
        //wxPrintPreview* preview =
        //    new wxPrintPreview(new MyPrintout(this), new MyPrintout(this), &printDialogData);
        //if (!preview->IsOk())
        //{
        //    delete preview;
        //    wxLogError("There was a problem previewing.\nPerhaps your current printer is not set correctly?");
        //    return;
        //}

        //wxPreviewFrame* frame =
        //    new wxPreviewFrame(preview, this, "Demo Print Preview", wxPoint(100, 100), wxSize(600, 650));
        //frame->Centre(wxBOTH);
        //frame->InitializeWithModality(m_previewModality);
        //frame->Show();
    }
}
