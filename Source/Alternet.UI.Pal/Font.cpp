#include "Font.h"

namespace Alternet::UI
{
    Font::Font(wxFont font)
    {
        _font = font;
    }

    Font::Font()
    {
    }

    Font::~Font()
    {
    }

    void Font::InitializeFromFont(Font* font)
    {
        _font = wxFont(font->_font);
    }

    void Font::Initialize(GenericFontFamily genericFamily,
        optional<string> familyName, Coord emSize, FontStyle style)
    {
        _font = InitializeWxFont(genericFamily, familyName, emSize, style);
    }

    wxFont Font::InitializeWxFont(GenericFontFamily genericFamily, 
        optional<string> familyName, Coord emSize, FontStyle style)
    {
        wxFontInfo fontInfo(emSize);

        if (genericFamily != GenericFontFamily::None)
            fontInfo.Family(GetWxFontFamily(genericFamily));

        if (familyName.has_value())
            fontInfo.FaceName(wxStr(familyName.value()));

        if ((style & FontStyle::Bold) != (FontStyle)0)
            fontInfo.Bold();

        if ((style & FontStyle::Italic) != (FontStyle)0)
            fontInfo.Italic();

        if ((style & FontStyle::Strikeout) != (FontStyle)0)
            fontInfo.Strikethrough();

        if ((style & FontStyle::Underline) != (FontStyle)0)
            fontInfo.Underlined();

        return wxFont(fontInfo);
    }

    void Font::InitializeWithDefaultFont()
    {
        _font = wxSystemSettings::GetFont(wxSystemFont::wxSYS_DEFAULT_GUI_FONT);
    }

    void Font::InitializeWithDefaultMonoFont()
    {
        _font = wxSystemSettings::GetFont(wxSystemFont::wxSYS_ANSI_FIXED_FONT);
    }

    void Font::SetWxFontInfo(wxFontInfo fontInfo)
    {
        _font = wxFont(fontInfo);
    }

    void Font::SetWxFont(wxFont font)
    {
        _font = font;
    }

    wxFont Font::GetWxFont()
    {
        return _font;
    }

    string Font::GetName()
    {
        return wxStr(_font.GetFaceName());
    }

    FontStyle Font::GetStyle()
    {
        return GetFontStyle(_font);
    }

    FontStyle Font::GetFontStyle(wxFont font)
    {
        auto style = FontStyle::Regular;
        auto wxStyle = font.GetStyle();
        if (wxStyle == wxFontStyle::wxFONTSTYLE_ITALIC)
            style |= FontStyle::Italic;

        auto weight = font.GetWeight();
        if (weight > wxFontWeight::wxFONTWEIGHT_NORMAL)
            style |= FontStyle::Bold;

        if (font.GetStrikethrough())
            style |= FontStyle::Strikeout;

        if (font.GetUnderlined())
            style |= FontStyle::Underline;

        return style;
    }

    Int32Size Font::GetPixelSize()
    {
        return _font.GetPixelSize();
    }

    bool Font::IsUsingSizeInPixels()
    {
        return _font.IsUsingSizeInPixels();
    }

    int Font::GetNumericWeight()
    {
        return _font.GetNumericWeight();
    }

    bool Font::GetUnderlined()
    {
        return _font.GetUnderlined();
    }

    bool Font::GetItalic()
    {
        auto style = _font.GetStyle();
        return style == wxFontStyle::wxFONTSTYLE_ITALIC;
    }

    bool Font::GetStrikethrough()
    {
        return _font.GetStrikethrough();
    }

    int Font::GetEncoding()
    {
        return _font.GetEncoding();
    }

    bool Font::IsFixedWidth()
    {
        return _font.IsFixedWidth();
    }

    int Font::GetDefaultEncoding()
    {
        return wxFont::GetDefaultEncoding();
    }

    void Font::SetDefaultEncoding(int encoding)
    {
        wxFont::SetDefaultEncoding((wxFontEncoding)encoding);
    }

    int Font::GetWeight()
    {
        return _font.GetWeight();
    }

    Coord Font::GetSizeInPoints()
    {
        return _font.GetFractionalPointSize();
    }

    string Font::GetDescription()
    {
        return wxStr(_font.GetNativeFontInfoUserDesc());
    }

    bool Font::IsEqualTo(Font* other)
    {
        if (other == nullptr)
            return false;
        
        return _font == other->_font;
    }

    string Font::Serialize()
    {
        return wxStr(_font.GetNativeFontInfoDesc());
    }

    /*static*/ wxFontFamily Font::GetWxFontFamily(GenericFontFamily genericFamily)
    {
        switch (genericFamily)
        {
        case GenericFontFamily::SansSerif:
            return wxFontFamily::wxFONTFAMILY_SWISS;
        case GenericFontFamily::Serif:
            return wxFontFamily::wxFONTFAMILY_ROMAN;
        case GenericFontFamily::Monospace:
            return wxFontFamily::wxFONTFAMILY_TELETYPE;
        case GenericFontFamily::Default:
        default:
            return wxFontFamily::wxFONTFAMILY_DEFAULT;
        }
    }

    /*static*/ void* Font::OpenFamiliesArray()
    {
        auto facenames = wxFontEnumerator::GetFacenames();
        return new wxArrayString(facenames);
    }

    /*static*/ int Font::GetFamiliesItemCount(void* array)
    {
        return ((wxArrayString*)array)->GetCount();
    }

    /*static*/ string Font::GetFamiliesItemAt(void* array, int index)
    {
        return wxStr(((wxArrayString*)array)->Item(index));
    }

    /*static*/ void Font::CloseFamiliesArray(void* array)
    {
        delete (wxArrayString*)array;
    }

    /*static*/ bool Font::IsFamilyValid(const string& fontFamily)
    {
#ifdef __WXOSX_COCOA__
        // the wx function will return false for this.
        // This is a family name of the macOS system font.
        if (fontFamily == u".AppleSystemUIFont")
            return true; 
#endif

        return wxFontEnumerator::IsValidFacename(wxStr(fontFamily));
    }

    /*static*/ string Font::GetGenericFamilyName(GenericFontFamily genericFamily)
    {
        if (genericFamily == GenericFontFamily::None)
            throwExInvalidArg(genericFamily, u"genericFamily cannot be None");

        wxFontInfo fontInfo;
        fontInfo.Family(GetWxFontFamily(genericFamily));
        return wxStr(wxFont(fontInfo).GetFaceName());
    }
}
