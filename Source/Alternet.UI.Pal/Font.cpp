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

    string Font::GetName()
    {
        return string();
    }

    float Font::GetSize()
    {
        return 0.0f;
    }

    string Font::ToString_()
    {
        return string();
    }

    bool Font::IsEqualTo(Font* other)
    {
        return false;
    }

    int Font::GetHashCode_()
    {
        return 0;
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
        default:
            wxASSERT(false);
            return wxFontFamily::wxFONTFAMILY_DEFAULT;
        }
    }

    /*static*/ void* Font::OpenFamiliesArray()
    {
        auto facenames = wxFontEnumerator::GetFacenames();
        return new wxArrayString(facenames.begin(), facenames.end());
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
        return wxFontEnumerator::IsValidFacename(wxStr(fontFamily));
    }

    /*static*/ string Font::GetGenericFamilyName(GenericFontFamily genericFamily)
    {
        wxASSERT(genericFamily != GenericFontFamily::None);

        wxFontInfo fontInfo;
        fontInfo.Family(GetWxFontFamily(genericFamily));
        return wxStr(wxFont(fontInfo).GetFaceName());
    }
}
