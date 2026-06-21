#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Window.h"

namespace Alternet::UI
{
    class FileDialog : public Object
    {
#include "Api/FileDialog.inc"
    public:

    private:

        void RecreateDialog();

        wxFileDialog* GetDialog();

        void DestroyDialog();
        void CreateDialog();

        long GetStyle();

        bool _overwritePrompt = false;
        bool _noShortcutFollow = false;
        bool _fileMustExist = false;
        bool _changeDir = false;
        bool _previewFiles = false;
        bool _showHiddenFiles = false;

        FileDialogMode _mode = FileDialogMode::Open;
        wxString _initialDirectory;
        wxString _title;
        wxString _filter;
        int _selectedFilterIndex = 0;
        wxString _fileName;
        wxString _fileNameResult;
        bool _allowMultipleSelection = false;
        Window* _owner = nullptr;

        wxFileDialog* _dialog = nullptr;
    };
}
