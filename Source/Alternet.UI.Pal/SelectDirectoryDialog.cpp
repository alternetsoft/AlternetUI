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
        _dialog = new wxDirDialog(
            nullptr,
            wxASCII_STR(wxDirSelectorPromptStr),
            wxStr(_initialDirectory),
            GetStyle(),
            wxDefaultPosition,
            wxDefaultSize,
            wxASCII_STR(wxDirDialogNameStr));
    }

    long SelectDirectoryDialog::GetStyle()
    {
        return wxDD_DEFAULT_STYLE;
    }

    string SelectDirectoryDialog::GetInitialDirectory()
    {
        return _initialDirectory;
    }

    void SelectDirectoryDialog::SetInitialDirectory(const string& value)
    {
        _initialDirectory = value;
        RecreateDialog();
    }

    string SelectDirectoryDialog::GetTitle()
    {
        return _title;
    }

    void SelectDirectoryDialog::SetTitle(const string& value)
    {
        _title = value;
        GetDialog()->SetTitle(wxStr(value));
    }

    string SelectDirectoryDialog::GetDirectoryName()
    {
        return wxStr(GetDialog()->GetPath());
    }

    void SelectDirectoryDialog::SetDirectoryName(const string& value)
    {
        _directoryName = value;
        GetDialog()->SetPath(wxStr(value));
    }

    ModalResult SelectDirectoryDialog::ShowModal()
    {
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
