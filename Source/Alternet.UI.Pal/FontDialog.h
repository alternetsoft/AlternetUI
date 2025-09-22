#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"
#include "Font.h"
#include "Window.h"

#include <wx/fontdlg.h>

namespace Alternet::UI
{
    class FontDialog : public Object
    {
#include "Api/FontDialog.inc"
    public:
    
    private:
        GenericFontFamily _genericFamily = GenericFontFamily::None;
        optional<string> _familyName;
        double _fontSizeInPoints = 12.0;
        FontStyle _fontStyle = FontStyle::Regular;

        void RecreateDialog();

        wxFontDialog* GetDialog();

        void DestroyDialog();
        void CreateDialog();

        optional<string> _title;

        Window* _owner = nullptr;

        wxFontDialog* _dialog = nullptr;

        wxFontData& GetFontData();

        bool _allowSymbols = true;
        bool _showHelp = false;
        bool _enableEffects = true;
        int _restrictSelection = 0;
        int _minRange = 0;
        int _maxRange = 0;
        Color _color = Color(0, 0, 0, 0);;

        bool DialogGetAllowSymbols();
        void DialogSetAllowSymbols(bool value);
        bool DialogGetShowHelp();
        void DialogSetShowHelp(bool value);
        bool DialogGetEnableEffects();
        void DialogSetEnableEffects(bool value);
        int DialogGetRestrictSelection();
        void DialogSetRestrictSelection(int value);
        void DialogSetRange(int minRange, int maxRange);
        Color DialogGetColor();
        void DialogSetColor(const Color& value);
        wxFont DialogGetFont();
    };
}