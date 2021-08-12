#include "Font.h"

namespace Alternet::UI
{
    Font::Font()
    {
    }

    Font::~Font()
    {
    }

    void Font::Initialize(GenericFontFamily genericFamily, optional<string> familyName, float emSize)
    {
        wxFontInfo fontInfo(emSize);

        if (genericFamily != GenericFontFamily::None)
            fontInfo.Family(GetWxFontFamily(genericFamily));

        if (familyName.has_value())
            fontInfo.FaceName(wxStr(familyName.value()));

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

    wxFontFamily Font::GetWxFontFamily(GenericFontFamily genericFamily)
    {
        switch (genericFamily)
        {
        case GenericFontFamily::SansSerif:
            return wxFontFamily::wxFONTFAMILY_SWISS;
        case GenericFontFamily::Serif:
            return wxFontFamily::wxFONTFAMILY_ROMAN;
        case GenericFontFamily::Monospace:
            return wxFontFamily::wxFONTFAMILY_TELETYPE;
        default:
            wxASSERT(false);
            return wxFontFamily::wxFONTFAMILY_DEFAULT;
        }
    }

    void* Font::OpenFamiliesArray()
    {
        auto facenames = wxFontEnumerator::GetFacenames();
        return new wxArrayString(facenames.begin(), facenames.end());
    }

    int Font::GetFamiliesItemCount(void* array)
    {
        return ((wxArrayString*)array)->GetCount();
    }

    string Font::GetFamiliesItemAt(void* array, int index)
    {
        return wxStr(((wxArrayString*)array)->Item(index));
    }

    void Font::CloseFamiliesArray(void* array)
    {
        delete (wxArrayString*)array;
    }

    bool Font::IsFamilyValid(const string& fontFamily)
    {
        return wxFontEnumerator::IsValidFacename(wxStr(fontFamily));
    }

    string Font::GetGenericFamilyName(GenericFontFamily genericFamily)
    {
        wxASSERT(genericFamily != GenericFontFamily::None);

        wxFontInfo fontInfo;
        fontInfo.Family(GetWxFontFamily(genericFamily));
        return wxStr(wxFont(fontInfo).GetFaceName());
    }
}
