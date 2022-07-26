#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Window.h"

namespace Alternet::UI
{
    class SelectDirectoryDialog : public Object
    {
#include "Api/SelectDirectoryDialog.inc"
    public:

    private:
        void RecreateDialog();

        wxDirDialog* GetDialog();

        void DestroyDialog();
        void CreateDialog();

        long GetStyle();

        optional<string> _initialDirectory;
        optional<string> _title;
        optional<string> _directoryName;

        Window* _owner = nullptr;

        wxDirDialog* _dialog = nullptr;
    };
}
