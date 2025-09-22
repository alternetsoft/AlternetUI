#include "PageSetupDialog.h"
#include "Window.h"
#include "PrintDocument.h"

namespace Alternet::UI
{
    PageSetupDialog::PageSetupDialog()
    {
    }

    PageSetupDialog::~PageSetupDialog()
    {
    }

    PrintDocument* PageSetupDialog::GetDocument()
    {
        _document->AddRef();
        return _document;
    }

    void PageSetupDialog::SetDocument(PrintDocument* value)
    {
        if (_document != nullptr)
            _document->Release();

        _document = value;

        if (_document != nullptr)
            _document->AddRef();
    }

    Coord PageSetupDialog::GetMinMarginBottom()
    {
        return _minMargins.Bottom;
    }

    Coord PageSetupDialog::GetMinMarginTop()
    {
        return _minMargins.Top;
    }

    Coord PageSetupDialog::GetMinMarginRight()
    {
        return _minMargins.Right;
    }

    Coord PageSetupDialog::GetMinMarginLeft()
    {
        return _minMargins.Left;
    }

    void PageSetupDialog::SetMinMarginBottom(Coord value)
    {
        _minMargins.Bottom = value;
    }

    void PageSetupDialog::SetMinMarginTop(Coord value)
    {
        _minMargins.Top = value;
    }

    void PageSetupDialog::SetMinMarginRight(Coord value)
    {
        _minMargins.Right = value;
    }

    void PageSetupDialog::SetMinMarginLeft(Coord value)
    {
        _minMargins.Left = value;
    }

    bool PageSetupDialog::GetMinMarginsValueSet()
    {
        return _minMarginsValueSet;
    }

    void PageSetupDialog::SetMinMarginsValueSet(bool value)
    {
        _minMarginsValueSet = value;
    }

    bool PageSetupDialog::GetAllowMargins()
    {
        return _allowMargins;
    }

    void PageSetupDialog::SetAllowMargins(bool value)
    {
        _allowMargins = value;
    }

    bool PageSetupDialog::GetAllowOrientation()
    {
        return _allowOrientation;
    }

    void PageSetupDialog::SetAllowOrientation(bool value)
    {
        _allowOrientation = value;
    }

    bool PageSetupDialog::GetAllowPaper()
    {
        return _allowPaper;
    }

    void PageSetupDialog::SetAllowPaper(bool value)
    {
        _allowPaper = value;
    }

    bool PageSetupDialog::GetAllowPrinter()
    {
        return _allowPrinter;
    }

    void PageSetupDialog::SetAllowPrinter(bool value)
    {
        _allowPrinter = value;
    }

    void PageSetupDialog::ApplyProperties(wxPageSetupDialogData& data)
    {
        data.SetDefaultMinMargins(!_minMarginsValueSet);
        if (_minMarginsValueSet)
        {
            auto minMargins = Thickness(
                GetMinMarginLeft(), GetMinMarginTop(), GetMinMarginRight(), GetMinMarginBottom());

            data.SetMinMarginTopLeft(wxPoint(fromDip(minMargins.Left, nullptr),
                fromDip(minMargins.Top, nullptr)));
            data.SetMinMarginBottomRight(wxPoint(fromDip(minMargins.Right, nullptr),
                fromDip(minMargins.Bottom, nullptr)));
        }

        data.EnableMargins(_allowMargins);
        data.EnableOrientation(_allowOrientation);
        data.EnablePaper(_allowPaper);
        data.EnablePrinter(_allowPrinter);
    }

    ModalResult PageSetupDialog::ShowModal(Window* owner)
    {
        if (_document == nullptr)
            throwExInvalidOpWithInfo(u"Cannot show the page setup dialog when the document is null.");

        auto data = _document->GetPageSetupDialogData();
        ApplyProperties(data);

        wxPageSetupDialog dialog(owner == nullptr ? nullptr : owner->GetWxWindow(), &data);

        auto result = dialog.ShowModal();

        bool accepted = result == wxID_OK;

        if (accepted)
            _document->ApplyData(dialog.GetPageSetupDialogData());

        return accepted ? ModalResult::Accepted : ModalResult::Canceled;
    }
}
