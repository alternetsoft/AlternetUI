#include "SelectDirectoryDialog.h"

namespace Alternet::UI
{
    SelectDirectoryDialog::SelectDirectoryDialog()
    {
    }

    SelectDirectoryDialog::~SelectDirectoryDialog()
    {
        DestroyDialog();
    }

    void SelectDirectoryDialog::CreateDialog()
    {
        auto owner = _owner != nullptr ? _owner->GetWxWindow() : nullptr;

        _dialog = new wxDirDialog(
            owner,
            wxASCII_STR(wxDirSelectorPromptStr),
            wxStr(_initialDirectory.value_or(u"")),
            GetStyle(),
            wxDefaultPosition,
            wxDefaultSize,
            wxASCII_STR(wxDirDialogNameStr));
    }

    long SelectDirectoryDialog::GetStyle()
    {
        return wxDD_DEFAULT_STYLE;
    }

    optional<string> SelectDirectoryDialog::GetInitialDirectory()
    {
        return _initialDirectory;
    }

    void SelectDirectoryDialog::SetInitialDirectory(optional<string> value)
    {
        _initialDirectory = value;
        RecreateDialog();
    }

    optional<string> SelectDirectoryDialog::GetTitle()
    {
        return _title;
    }

    void SelectDirectoryDialog::SetTitle(optional<string> value)
    {
        _title = value;
        GetDialog()->SetTitle(wxStr(value.value_or(u"")));
    }

    optional<string> SelectDirectoryDialog::GetDirectoryName()
    {
        return wxStr(GetDialog()->GetPath());
    }

    void SelectDirectoryDialog::SetDirectoryName(optional<string> value)
    {
        _directoryName = value;
        GetDialog()->SetPath(wxStr(value.value_or(u"")));
    }

    ModalResult SelectDirectoryDialog::ShowModal(Window* owner)
    {
        bool ownerChanged = _owner != owner;
        _owner = owner;
        if (ownerChanged)
            RecreateDialog();

        auto result = GetDialog()->ShowModal();

        if (result == wxID_OK)
            return ModalResult::Accepted;
        else if (result == wxID_CANCEL)
            return ModalResult::Canceled;
        else
            throwExNoInfo;
    }

    void SelectDirectoryDialog::DestroyDialog()
    {
        if (_dialog != nullptr)
        {
            delete _dialog;
            _dialog = nullptr;
        }
    }

    void SelectDirectoryDialog::RecreateDialog()
    {
        DestroyDialog();
        CreateDialog();
    }

    wxDirDialog* SelectDirectoryDialog::GetDialog()
    {
        if (_dialog == nullptr)
            RecreateDialog();

        return _dialog;
    }
}
