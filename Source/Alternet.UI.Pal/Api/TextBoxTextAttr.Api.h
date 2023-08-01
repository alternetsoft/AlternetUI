// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>

#pragma once

#include "TextBoxTextAttr.h"
#include "ApiUtils.h"
#include "Exceptions.h"

using namespace Alternet::UI;

ALTERNET_UI_API TextBoxTextAttr* TextBoxTextAttr_Create_()
{
    return MarshalExceptions<TextBoxTextAttr*>([&](){
            return new TextBoxTextAttr();
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasPageBreak_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasPageBreak(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasTextEffects_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasTextEffects(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasTextEffect_(void* attr, int effect)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasTextEffect(attr, effect);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasOutlineLevel_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasOutlineLevel(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFlag_(void* attr, int64_t flag)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFlag(attr, flag);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_RemoveFlag_(void* attr, int64_t flag)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::RemoveFlag(attr, flag);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_AddFlag_(void* attr, int64_t flag)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::AddFlag(attr, flag);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_Delete_(void* attr)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::Delete(attr);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_Copy_(void* toAttr, void* fromAttr)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::Copy(toAttr, fromAttr);
        });
}

ALTERNET_UI_API void* TextBoxTextAttr_CreateTextAttr_()
{
    return MarshalExceptions<void*>([&](){
            return TextBoxTextAttr::CreateTextAttr();
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetTextColor_(void* attr, Color colText)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetTextColor(attr, colText);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBackgroundColor_(void* attr, Color colBack)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBackgroundColor(attr, colBack);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetAlignment_(void* attr, int alignment)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetAlignment(attr, alignment);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontPointSize_(void* attr, int pointSize)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontPointSize(attr, pointSize);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontStyle_(void* attr, int fontStyle)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontStyle(attr, fontStyle);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontWeight_(void* attr, int fontWeight)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontWeight(attr, fontWeight);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontFaceName_(void* attr, const char16_t* faceName)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontFaceName(attr, faceName);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontUnderlined_(void* attr, c_bool underlined)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontUnderlined(attr, underlined);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontUnderlinedEx_(void* attr, int type, Color colour)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontUnderlinedEx(attr, type, colour);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontStrikethrough_(void* attr, c_bool strikethrough)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontStrikethrough(attr, strikethrough);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFontFamily_(void* attr, int family)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFontFamily(attr, family);
        });
}

ALTERNET_UI_API Color_C TextBoxTextAttr_GetTextColor_(void* attr)
{
    return MarshalExceptions<Color_C>([&](){
            return TextBoxTextAttr::GetTextColor(attr);
        });
}

ALTERNET_UI_API Color_C TextBoxTextAttr_GetBackgroundColor_(void* attr)
{
    return MarshalExceptions<Color_C>([&](){
            return TextBoxTextAttr::GetBackgroundColor(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetAlignment_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetAlignment(attr);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetURL_(void* attr, const char16_t* url)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetURL(attr, url);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetFlags_(void* attr, int64_t flags)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetFlags(attr, flags);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetParagraphSpacingAfter_(void* attr, int spacing)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetParagraphSpacingAfter(attr, spacing);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetParagraphSpacingBefore_(void* attr, int spacing)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetParagraphSpacingBefore(attr, spacing);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetLineSpacing_(void* attr, int spacing)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetLineSpacing(attr, spacing);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBulletStyle_(void* attr, int style)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBulletStyle(attr, style);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBulletNumber_(void* attr, int n)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBulletNumber(attr, n);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBulletText_(void* attr, const char16_t* text)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBulletText(attr, text);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetPageBreak_(void* attr, c_bool pageBreak)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetPageBreak(attr, pageBreak);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetCharacterStyleName_(void* attr, const char16_t* name)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetCharacterStyleName(attr, name);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetParagraphStyleName_(void* attr, const char16_t* name)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetParagraphStyleName(attr, name);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetListStyleName_(void* attr, const char16_t* name)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetListStyleName(attr, name);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBulletFont_(void* attr, const char16_t* bulletFont)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBulletFont(attr, bulletFont);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetBulletName_(void* attr, const char16_t* name)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetBulletName(attr, name);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetTextEffects_(void* attr, int effects)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetTextEffects(attr, effects);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetTextEffectFlags_(void* attr, int effects)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetTextEffectFlags(attr, effects);
        });
}

ALTERNET_UI_API void TextBoxTextAttr_SetOutlineLevel_(void* attr, int level)
{
    MarshalExceptions<void>([&](){
            TextBoxTextAttr::SetOutlineLevel(attr, level);
        });
}

ALTERNET_UI_API int64_t TextBoxTextAttr_GetFlags_(void* attr)
{
    return MarshalExceptions<int64_t>([&](){
            return TextBoxTextAttr::GetFlags(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetFontSize_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetFontSize(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetFontStyle_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetFontStyle(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetFontWeight_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetFontWeight(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_GetFontUnderlined_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::GetFontUnderlined(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetUnderlineType_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetUnderlineType(attr);
        });
}

ALTERNET_UI_API Color_C TextBoxTextAttr_GetUnderlineColor_(void* attr)
{
    return MarshalExceptions<Color_C>([&](){
            return TextBoxTextAttr::GetUnderlineColor(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_GetFontStrikethrough_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::GetFontStrikethrough(attr);
        });
}

ALTERNET_UI_API char16_t* TextBoxTextAttr_GetFontFaceName_(void* attr)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(TextBoxTextAttr::GetFontFaceName(attr));
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetFontFamily_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetFontFamily(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetParagraphSpacingAfter_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetParagraphSpacingAfter(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetParagraphSpacingBefore_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetParagraphSpacingBefore(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetLineSpacing_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetLineSpacing(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetBulletStyle_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetBulletStyle(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetBulletNumber_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetBulletNumber(attr);
        });
}

ALTERNET_UI_API char16_t* TextBoxTextAttr_GetBulletText_(void* attr)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(TextBoxTextAttr::GetBulletText(attr));
        });
}

ALTERNET_UI_API char16_t* TextBoxTextAttr_GetURL_(void* attr)
{
    return MarshalExceptions<char16_t*>([&](){
            return AllocPInvokeReturnString(TextBoxTextAttr::GetURL(attr));
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetTextEffects_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetTextEffects(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetTextEffectFlags_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetTextEffectFlags(attr);
        });
}

ALTERNET_UI_API int TextBoxTextAttr_GetOutlineLevel_(void* attr)
{
    return MarshalExceptions<int>([&](){
            return TextBoxTextAttr::GetOutlineLevel(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_IsCharacterStyle_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::IsCharacterStyle(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_IsParagraphStyle_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::IsParagraphStyle(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_IsDefault_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::IsDefault(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasTextColor_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasTextColor(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasBackgroundColor_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasBackgroundColor(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasAlignment_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasAlignment(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasTabs_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasTabs(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasLeftIndent_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasLeftIndent(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasRightIndent_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasRightIndent(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontWeight_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontWeight(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontSize_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontSize(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontPointSize_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontPointSize(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontPixelSize_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontPixelSize(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontItalic_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontItalic(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontUnderlined_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontUnderlined(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontStrikethrough_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontStrikethrough(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontFaceName_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontFaceName(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontEncoding_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontEncoding(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFontFamily_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFontFamily(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasFont_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasFont(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasParagraphSpacingAfter_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasParagraphSpacingAfter(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasParagraphSpacingBefore_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasParagraphSpacingBefore(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasLineSpacing_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasLineSpacing(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasCharacterStyleName_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasCharacterStyleName(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasParagraphStyleName_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasParagraphStyleName(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasListStyleName_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasListStyleName(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasBulletStyle_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasBulletStyle(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasBulletNumber_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasBulletNumber(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasBulletText_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasBulletText(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasBulletName_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasBulletName(attr);
        });
}

ALTERNET_UI_API c_bool TextBoxTextAttr_HasURL_(void* attr)
{
    return MarshalExceptions<c_bool>([&](){
            return TextBoxTextAttr::HasURL(attr);
        });
}

