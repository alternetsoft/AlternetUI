#include "Font.h"

namespace Alternet::UI
{
    Font::Font()
    {
    }

    Font::~Font()
    {
    }

    void Font::Initialize(const string& familyName, float emSize)
    {
        wxFontInfo fontInfo(emSize);
        fontInfo.FaceName(wxStr(familyName));

        _font = wxFont(fontInfo);
    }

    void Font::InitializeWithDefaultFont()
    {
        _font = wxSystemSettings::GetFont(wxSystemFont::wxSYS_DEFAULT_GUI_FONT);
    }

    wxFont Font::GetWxFont()
    {
        return _font;
    }
}
