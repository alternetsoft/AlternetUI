#include "PageSetupDialog.h"
#include "Window.h"

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
        return nullptr;
    }

    void PageSetupDialog::SetDocument(PrintDocument* value)
    {
    }

    Thickness PageSetupDialog::GetMinMargins()
    {
        return Thickness();
    }

    void PageSetupDialog::SetMinMargins(const Thickness& value)
    {
    }

    bool PageSetupDialog::GetMinMarginsValueSet()
    {
        return false;
    }

    void PageSetupDialog::SetMinMarginsValueSet(bool value)
    {
    }

    bool PageSetupDialog::GetAllowMargins()
    {
        return false;
    }

    void PageSetupDialog::SetAllowMargins(bool value)
    {
    }

    bool PageSetupDialog::GetAllowOrientation()
    {
        return false;
    }

    void PageSetupDialog::SetAllowOrientation(bool value)
    {
    }

    bool PageSetupDialog::GetAllowPaper()
    {
        return false;
    }

    void PageSetupDialog::SetAllowPaper(bool value)
    {
    }

    bool PageSetupDialog::GetAllowPrinter()
    {
        return false;
    }

    void PageSetupDialog::SetAllowPrinter(bool value)
    {
    }

    ModalResult PageSetupDialog::ShowModal(Window* owner)
    {
        wxPageSetupDialogData data;
        wxPageSetupDialog dialog(owner == nullptr ? nullptr : owner->GetWxWindow(), &data);

        auto result = dialog.ShowModal();

        return result == wxID_OK ? ModalResult::Accepted : ModalResult::Canceled;
    }
}
