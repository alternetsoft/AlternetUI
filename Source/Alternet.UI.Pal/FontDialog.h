#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Font.h"
#include "Window.h"
#include "wx/fontdlg.h"

namespace Alternet::UI
{
    class FontDialog : public Object
    {
#include "Api/FontDialog.inc"
    public:
    
    private:
        void RecreateDialog();

        wxFontDialog* GetDialog();

        void DestroyDialog();
        void CreateDialog();

        optional<string> _title;

        Window* _owner = nullptr;
        Font* _font = nullptr;

        wxFontDialog* _dialog = nullptr;

        wxFontData& GetFontData();
    };
}
