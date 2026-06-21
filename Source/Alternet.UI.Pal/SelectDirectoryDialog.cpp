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
            _title,
            _initialDirectory,
            GetStyle(),
            wxDefaultPosition,
            wxDefaultSize,
            wxASCII_STR(wxDirDialogNameStr));
    }

    long SelectDirectoryDialog::GetStyle()
    {
        return wxDD_DEFAULT_STYLE;
    }

    NativeStringSpan SelectDirectoryDialog::GetInitialDirectory()
    {
        return wxStr(_initialDirectory);
    }

    void SelectDirectoryDialog::SetInitialDirectory(const NativeStringSpan& value)
    {
        _initialDirectory = wxStr(value);
        RecreateDialog();
    }

    NativeStringSpan SelectDirectoryDialog::GetTitle()
    {
        return wxStr(_title);
    }

    void SelectDirectoryDialog::SetTitle(const NativeStringSpan& value)
    {
        _title = wxStr(value);
        RecreateDialog();
    }

    NativeStringSpan SelectDirectoryDialog::GetDirectoryName()
    {
        _directoryName = GetDialog()->GetPath();
        return wxStr(_directoryName);
    }

    void SelectDirectoryDialog::SetDirectoryName(const NativeStringSpan& value)
    {
        _directoryName = wxStr(value);
        GetDialog()->SetPath(_directoryName);
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
