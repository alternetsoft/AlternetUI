#include "FileDialog.h"

namespace Alternet::UI
{
    FileDialog::FileDialog()
    {
    }

    FileDialog::~FileDialog()
    {
        DestroyDialog();
    }

    void FileDialog::CreateDialog()
    {
        _dialog = new wxFileDialog(
            nullptr,
            wxASCII_STR(wxFileSelectorPromptStr),
            wxStr(_initialDirectory),
            wxEmptyString,
            wxStr(_filter),
            GetStyle(),
            wxDefaultPosition,
            wxDefaultSize,
            wxASCII_STR(wxFileDialogNameStr));
    }

    long FileDialog::GetStyle()
    {
        long style = 0;

        if (_mode == FileDialogMode::Open)
            style |= wxFD_OPEN;
        else if (_mode == FileDialogMode::Save)
            style |= wxFD_SAVE;
        else
            throwExNoInfo;

        if (_allowMultipleSelection)
            style |= wxFD_MULTIPLE;

        return style;
    }

    FileDialogMode FileDialog::GetMode()
    {
        return _mode;
    }

    void FileDialog::SetMode(FileDialogMode value)
    {
        _mode = value;
        RecreateDialog();
    }

    string FileDialog::GetInitialDirectory()
    {
        return _initialDirectory;
    }

    void FileDialog::SetInitialDirectory(const string& value)
    {
        _initialDirectory = value;
        GetDialog()->SetDirectory(wxStr(value));
    }

    string FileDialog::GetTitle()
    {
        return _title;
    }

    void FileDialog::SetTitle(const string& value)
    {
        _title = value;
        GetDialog()->SetTitle(wxStr(value));
    }

    string FileDialog::GetFilter()
    {
        return _filter;
    }

    void FileDialog::SetFilter(const string& value)
    {
        _filter = value;
        GetDialog()->SetWildcard(wxStr(value));
    }

    int FileDialog::GetSelectedFilterIndex()
    {
        return _selectedFilterIndex;
    }

    void FileDialog::SetSelectedFilterIndex(int value)
    {
        _selectedFilterIndex = value;
        GetDialog()->SetFilterIndex(value);
    }

    string FileDialog::GetFileName()
    {
        return wxStr(GetDialog()->GetPath());
    }

    void FileDialog::SetFileName(const string& value)
    {
        _fileName = value;
        GetDialog()->SetPath(wxStr(value));
    }

    bool FileDialog::GetAllowMultipleSelection()
    {
        return _allowMultipleSelection;
    }

    void FileDialog::SetAllowMultipleSelection(bool value)
    {
        _allowMultipleSelection = value;
        RecreateDialog();
    }

    void* FileDialog::OpenFileNamesArray()
    {
        auto paths = new wxArrayString();
        GetDialog()->GetPaths(*paths);
        return paths;
    }

    int FileDialog::GetFileNamesItemCount(void* array)
    {
        auto paths = (wxArrayString*)array;
        return paths->GetCount();
    }

    string FileDialog::GetFileNamesItemAt(void* array, int index)
    {
        auto paths = (wxArrayString*)array;
        return wxStr((*paths)[index]);
    }

    void FileDialog::CloseFileNamesArray(void* array)
    {
        auto paths = (wxArrayString*)array;
        delete paths;
    }

    ModalResult FileDialog::ShowModal()
    {
        auto result = GetDialog()->ShowModal();
        
        if (result == wxID_OK)
            return ModalResult::Accepted;
        else if (result == wxID_CANCEL)
            return ModalResult::Canceled;
        else
            throwExNoInfo;
    }

    void FileDialog::DestroyDialog()
    {
        if (_dialog != nullptr)
        {
            delete _dialog;
            _dialog = nullptr;
        }
    }

    void FileDialog::RecreateDialog()
    {
        DestroyDialog();
        CreateDialog();
    }

    wxFileDialog* FileDialog::GetDialog()
    {
        if (_dialog == nullptr)
            RecreateDialog();

        return _dialog;
    }
}
