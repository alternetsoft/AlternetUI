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

        FileDialogMode _mode = FileDialogMode::Open;
        optional<string> _initialDirectory;
        optional<string> _title;
        optional<string> _filter;
        int _selectedFilterIndex = 0;
        optional<string> _fileName;
        bool _allowMultipleSelection = false;
        Window* _owner = nullptr;

        wxFileDialog* _dialog = nullptr;
    };
}
