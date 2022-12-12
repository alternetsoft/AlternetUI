#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Window.h"

namespace Alternet::UI
{
    class ColorDialog : public Object
    {
#include "Api/ColorDialog.inc"
    public:

    private:
        void RecreateDialog();

        wxColourDialog* GetDialog();

        void DestroyDialog();
        void CreateDialog();

        optional<string> _title;
        Color _color;

        Window* _owner = nullptr;

        wxColourDialog* _dialog = nullptr;
        wxColourData* _data = nullptr;
    };
}
