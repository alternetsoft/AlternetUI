#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

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
        string _initialDirectory;
        string _title;
        string _filter;
        int _selectedFilterIndex = 0;
        string _fileName;
        bool _allowMultipleSelection = false;

        wxFileDialog* _dialog = nullptr;
    };
}
