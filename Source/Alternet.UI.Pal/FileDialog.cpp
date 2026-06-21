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
        auto owner = _owner != nullptr ? _owner->GetWxWindow() : nullptr;

        _dialog = new wxFileDialog(
            owner,
            _title,
            _initialDirectory,
            _fileName,
            _filter,
            GetStyle(),
            wxDefaultPosition,
            wxDefaultSize,
            wxASCII_STR(wxFileDialogNameStr));
    }

    bool FileDialog::GetOverwritePrompt()
    {
        return _overwritePrompt;
    }

    void FileDialog::SetOverwritePrompt(bool value)
    {
        if (_overwritePrompt == value)
            return;
        _overwritePrompt = value;
        RecreateDialog();
    }

    bool FileDialog::GetNoShortcutFollow()
    {
        return _noShortcutFollow;
    }

    void FileDialog::SetNoShortcutFollow(bool value)
    {
        if (_noShortcutFollow == value)
            return;
        _noShortcutFollow = value;
        RecreateDialog();
    }

    bool FileDialog::GetFileMustExist()
    {
        return _fileMustExist;
    }

    void FileDialog::SetFileMustExist(bool value)
    {
        if (_fileMustExist == value)
            return;
        _fileMustExist = value;
        RecreateDialog();
    }

    bool FileDialog::GetChangeDir()
    {
        return _changeDir;
    }

    void FileDialog::SetChangeDir(bool value)
    {
        if (_changeDir == value)
            return;
        _changeDir = value;
        RecreateDialog();
    }

    bool FileDialog::GetPreviewFiles()
    {
        return _previewFiles;
    }

    void FileDialog::SetPreviewFiles(bool value)
    {
        if (_previewFiles == value)
            return;
        _previewFiles = value;
        RecreateDialog();
    }

    bool FileDialog::GetShowHiddenFiles()
    {
        return _showHiddenFiles;
    }

    void FileDialog::SetShowHiddenFiles(bool value)
    {
        if (_showHiddenFiles == value)
            return;
        _showHiddenFiles = value;
        RecreateDialog();
    }

    long FileDialog::GetStyle()
    {
        long style = 0;

        if (_mode == FileDialogMode::Open)
            style |= wxFD_OPEN;
        else if (_mode == FileDialogMode::Save)
            style |= wxFD_SAVE;

        if (_overwritePrompt)
            style |= wxFD_OVERWRITE_PROMPT;
        if (_noShortcutFollow)
            style |= wxFD_NO_FOLLOW;
        if (_fileMustExist)
            style |= wxFD_FILE_MUST_EXIST;
        if (_changeDir)
            style |= wxFD_CHANGE_DIR;
        if (_previewFiles)
            style |= wxFD_PREVIEW;
        if (_showHiddenFiles)
            style |= wxFD_SHOW_HIDDEN;
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

    NativeStringSpan FileDialog::GetInitialDirectory()
    {
        return wxStr(_initialDirectory);
    }

    void FileDialog::SetInitialDirectory(const NativeStringSpan& value)
    {
        auto _newInitialDirectory = wxStr(value);
        if (_initialDirectory == _newInitialDirectory)
            return;
        _initialDirectory = _newInitialDirectory;
        GetDialog()->SetDirectory(_initialDirectory);
    }

    NativeStringSpan FileDialog::GetTitle()
    {
        return wxStr(_title);
    }

    void FileDialog::SetTitle(const NativeStringSpan& value)
    {
        auto _newTitle = wxStr(value);
        if (_title == _newTitle)
            return;
        _title = _newTitle;
        RecreateDialog();
    }

    NativeStringSpan FileDialog::GetFilter()
    {
        return wxStr(_filter);
    }

    void FileDialog::SetFilter(const NativeStringSpan& value)
    {
        auto _newFilter = wxStr(value);
        if (_filter == _newFilter)
            return;
        _filter = _newFilter;
        GetDialog()->SetWildcard(_filter);
    }

    int FileDialog::GetSelectedFilterIndex()
    {
        return _selectedFilterIndex;
    }

    void FileDialog::SetSelectedFilterIndex(int value)
    {
        if (_selectedFilterIndex == value)
            return;
        _selectedFilterIndex = value;
        GetDialog()->SetFilterIndex(value);
    }

    NativeStringSpan FileDialog::GetFileName()
    {
        if (_allowMultipleSelection)
        {
            wxArrayString paths;
            GetDialog()->GetPaths(paths);
            _fileNameResult = paths.GetCount() == 0 ? wxString("") : paths[0];
        }
        else
        {
            _fileNameResult = GetDialog()->GetPath();
        }

        return wxStr(_fileNameResult);
    }

    void FileDialog::SetFileName(const NativeStringSpan& value)
    {
        auto _newFileName = wxStr(value);
        if (_fileName == _newFileName)
            return;
        _fileName = _newFileName;
        GetDialog()->SetPath(_fileName);
    }

    bool FileDialog::GetAllowMultipleSelection()
    {
        return _allowMultipleSelection;
    }

    void FileDialog::SetAllowMultipleSelection(bool value)
    {
        if (_allowMultipleSelection == value)
            return;
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

    NativeStringSpan FileDialog::GetFileNamesItemAt(void* array, int index)
    {
        auto paths = (wxArrayString*)array;
        _fileNameResult = (*paths)[index];
        return wxStr(_fileNameResult);
    }

    void FileDialog::CloseFileNamesArray(void* array)
    {
        auto paths = (wxArrayString*)array;
        delete paths;
    }

    ModalResult FileDialog::ShowModal(Window* owner)
    {
        bool ownerChanged = _owner != owner;
        _owner = owner;
        if (ownerChanged)
            RecreateDialog();

        auto result = GetDialog()->ShowModal();
        
        if (result == wxID_OK)
            return ModalResult::Accepted;
        else
            return ModalResult::Canceled;
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
