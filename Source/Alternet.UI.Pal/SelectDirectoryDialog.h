#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

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

        string _initialDirectory;
        string _title;
        string _directoryName;

        wxDirDialog* _dialog = nullptr;
    };
}
