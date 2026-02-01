#include "PrintDialog.h"
#include "Window.h"
#include "PrintDocument.h"

namespace Alternet::UI
{
    PrintDialog::PrintDialog()
    {
    }

    PrintDialog::~PrintDialog()
    {
        SetDocument(nullptr);
    }

    bool PrintDialog::GetAllowSomePages()
    {
        return _allowSomePages;
    }

    void PrintDialog::SetAllowSomePages(bool value)
    {
        _allowSomePages = value;
    }

    bool PrintDialog::GetAllowSelection()
    {
        return _allowSelection;
    }

    void PrintDialog::SetAllowSelection(bool value)
    {
        _allowSelection = value;
    }

    bool PrintDialog::GetAllowPrintToFile()
    {
        return _allowPrintToFile;
    }

    void PrintDialog::SetAllowPrintToFile(bool value)
    {
        _allowPrintToFile = value;
    }

    bool PrintDialog::GetShowHelp()
    {
        return _showHelp;
    }

    void PrintDialog::SetShowHelp(bool value)
    {
        _showHelp = value;
    }

    PrintDocument* PrintDialog::GetDocument()
    {
        _document->AddRef();
        return _document;
    }

    void PrintDialog::SetDocument(PrintDocument* value)
    {
        if (_document != nullptr)
            _document->Release();

        _document = value;

        if (_document != nullptr)
            _document->AddRef();
    }

    void PrintDialog::ApplyProperties(wxPrintDialogData& data)
    {
        data.EnablePageNumbers(_allowSomePages);
        data.EnableSelection(_allowSelection);
        data.EnablePrintToFile(_allowPrintToFile);

        /* This is disabled as in Windows 11 dialog fails with this enabled */
        /* data.EnableHelp(_showHelp);*/
    }

    ModalResult PrintDialog::ShowModal(Window* owner)
    {
        if (_document == nullptr)
            throwExInvalidOpWithInfo(u"Cannot show the print dialog when the document is null.");

        auto data = _document->GetPrintDialogData();
        ApplyProperties(data);

        wxPrintDialog dialog(owner == nullptr ? nullptr : owner->GetWxWindow(), &data);

        auto result = dialog.ShowModal();

        bool accepted = result == wxID_OK;

        if (accepted)
            _document->ApplyData(dialog.GetPrintDialogData());

        return accepted ? ModalResult::Accepted : ModalResult::Canceled;
    }
}
