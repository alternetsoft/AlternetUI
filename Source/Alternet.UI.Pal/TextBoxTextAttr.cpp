#include "TextBoxTextAttr.h"

namespace Alternet::UI
{
	TextBoxTextAttr::TextBoxTextAttr()
	{
	}

	TextBoxTextAttr::~TextBoxTextAttr()
	{
	}

	wxTextAttr* TextBoxTextAttr::Attr(void* attr) 
	{
		return (wxTextAttr*)attr;
	}

	void TextBoxTextAttr::Delete(void* attr)
	{
		delete Attr(attr);
	}

	/*void TextBoxTextAttr::Copy(void* toAttr, void* fromAttr)
	{
	}*/

	void* TextBoxTextAttr::CreateTextAttr()
	{
		wxTextAttr* attr = new wxTextAttr();
		attr->SetFlags(0);
		return attr;
	}

	void TextBoxTextAttr::SetTextColor(void* attr, const Color& colText)
	{
		Attr(attr)->SetTextColour(colText);
	}

	void TextBoxTextAttr::SetBackgroundColor(void* attr, const Color& colBack)
	{
		Attr(attr)->SetBackgroundColour(colBack);
	}

	void TextBoxTextAttr::SetAlignment(void* attr, int alignment)
	{
		Attr(attr)->SetAlignment((wxTextAttrAlignment)alignment);
	}

	void TextBoxTextAttr::SetFontPointSize(void* attr, int pointSize)
	{
		Attr(attr)->SetFontPointSize(pointSize);
	}

	void TextBoxTextAttr::SetFontStyle(void* attr, int fontStyle)
	{
		Attr(attr)->SetFontStyle((wxFontStyle)fontStyle);
	}

	void TextBoxTextAttr::SetFontWeight(void* attr, int fontWeight)
	{
		Attr(attr)->SetFontWeight((wxFontWeight)fontWeight);
	}

	void TextBoxTextAttr::SetFontFaceName(void* attr, const string& faceName)
	{
		Attr(attr)->SetFontFaceName(wxStr(faceName));
	}

	void TextBoxTextAttr::SetFontUnderlined(void* attr, bool underlined)
	{
		Attr(attr)->SetFontUnderlined(underlined);
	}

	void TextBoxTextAttr::SetFontUnderlinedEx(void* attr, int type,
		const Color& colour)
	{	
		Attr(attr)->SetFontUnderlined((wxTextAttrUnderlineType)type, colour);
	}

	void TextBoxTextAttr::SetFontStrikethrough(void* attr, bool strikethrough)
	{
		Attr(attr)->SetFontStrikethrough(strikethrough);
	}

	void TextBoxTextAttr::SetFontFamily(void* attr, int family)
	{
		Attr(attr)->SetFontFamily((wxFontFamily)family);
	}

	Color TextBoxTextAttr::GetTextColor(void* attr)
	{
		return Attr(attr)->GetTextColour();
	}

	Color TextBoxTextAttr::GetBackgroundColor(void* attr)
	{
		return Attr(attr)->GetBackgroundColour();
	}

	int TextBoxTextAttr::GetAlignment(void* attr)
	{
		return Attr(attr)->GetAlignment();
	}

	void TextBoxTextAttr::SetURL(void* attr, const string& url)
	{
		Attr(attr)->SetURL(wxStr(url));
	}

	void TextBoxTextAttr::SetFlags(void* attr, int64_t flags)
	{
		Attr(attr)->SetFlags(flags);
	}

	void TextBoxTextAttr::SetParagraphSpacingAfter(void* attr, int spacing)
	{
		Attr(attr)->SetParagraphSpacingBefore(spacing);
	}

	void TextBoxTextAttr::SetParagraphSpacingBefore(void* attr, int spacing)
	{
		Attr(attr)->SetParagraphSpacingBefore(spacing);
	}

	void TextBoxTextAttr::SetLineSpacing(void* attr, int spacing)
	{
		Attr(attr)->SetLineSpacing(spacing);
	}

	void TextBoxTextAttr::SetBulletStyle(void* attr, int style)
	{
		Attr(attr)->SetBulletStyle(style);
	}

	void TextBoxTextAttr::SetBulletNumber(void* attr, int n)
	{
		Attr(attr)->SetBulletNumber(n);
	}

	void TextBoxTextAttr::SetBulletText(void* attr, const string& text)
	{
		Attr(attr)->SetBulletText(wxStr(text));
	}

	void TextBoxTextAttr::SetPageBreak(void* attr, bool pageBreak)
	{
		Attr(attr)->SetPageBreak(pageBreak);
	}

	void TextBoxTextAttr::SetCharacterStyleName(void* attr, const string& name)
	{
		Attr(attr)->SetCharacterStyleName(wxStr(name));
	}
	
	void TextBoxTextAttr::SetParagraphStyleName(void* attr, const string& name)
	{
		Attr(attr)->SetParagraphStyleName(wxStr(name));
	}

	void TextBoxTextAttr::SetListStyleName(void* attr, const string& name)
	{
		Attr(attr)->SetListStyleName(wxStr(name));
	}

	void TextBoxTextAttr::SetBulletFont(void* attr, const string& bulletFont)
	{
		Attr(attr)->SetBulletFont(wxStr(bulletFont));
	}

	void TextBoxTextAttr::SetBulletName(void* attr, const string& name)
	{
		Attr(attr)->SetBulletName(wxStr(name));
	}

	void TextBoxTextAttr::SetTextEffects(void* attr, int effects)
	{
		Attr(attr)->SetTextEffects(effects);
	}

	void TextBoxTextAttr::SetTextEffectFlags(void* attr, int effects)
	{
		Attr(attr)->SetTextEffectFlags(effects);
	}

	void TextBoxTextAttr::SetOutlineLevel(void* attr, int level)
	{
		Attr(attr)->SetOutlineLevel(level);
	}

	int64_t TextBoxTextAttr::GetFlags(void* attr)
	{
		return Attr(attr)->GetFlags();
	}

	int TextBoxTextAttr::GetFontSize(void* attr)
	{
		return Attr(attr)->GetFontSize();
	}

	int TextBoxTextAttr::GetFontStyle(void* attr)
	{
		return Attr(attr)->GetFontStyle();
	}

	int TextBoxTextAttr::GetFontWeight(void* attr)
	{
		return Attr(attr)->GetFontWeight();
	}

	bool TextBoxTextAttr::GetFontUnderlined(void* attr)
	{
		return Attr(attr)->GetFontUnderlined();
	}
	
	int TextBoxTextAttr::GetUnderlineType(void* attr)
	{
		return Attr(attr)->GetUnderlineType();
	}

	Color TextBoxTextAttr::GetUnderlineColor(void* attr)
	{
		return Attr(attr)->GetUnderlineColour();
	}

	bool TextBoxTextAttr::GetFontStrikethrough(void* attr)
	{
		return Attr(attr)->GetFontStrikethrough();
	}

	string TextBoxTextAttr::GetFontFaceName(void* attr)
	{
		return wxStr(Attr(attr)->GetFontFaceName());
	}

	int TextBoxTextAttr::GetFontFamily(void* attr)
	{
		return Attr(attr)->GetFontFamily();
	}

	int TextBoxTextAttr::GetParagraphSpacingAfter(void* attr)
	{
		return Attr(attr)->GetParagraphSpacingAfter();
	}

	int TextBoxTextAttr::GetParagraphSpacingBefore(void* attr)
	{
		return Attr(attr)->GetParagraphSpacingBefore();
	}

	int TextBoxTextAttr::GetLineSpacing(void* attr)
	{
		return Attr(attr)->GetLineSpacing();
	}

	int TextBoxTextAttr::GetBulletStyle(void* attr)
	{
		return Attr(attr)->GetBulletStyle();
	}

	int TextBoxTextAttr::GetBulletNumber(void* attr)
	{
		return Attr(attr)->GetBulletNumber();
	}

	string TextBoxTextAttr::GetBulletText(void* attr)
	{
		return wxStr(Attr(attr)->GetBulletText());
	}

	string TextBoxTextAttr::GetURL(void* attr)
	{
		return wxStr(Attr(attr)->GetURL());
	}

	int TextBoxTextAttr::GetTextEffects(void* attr)
	{
		return Attr(attr)->GetTextEffects();
	}

	int TextBoxTextAttr::GetTextEffectFlags(void* attr)
	{
		return Attr(attr)->GetTextEffectFlags();
	}
	
	int TextBoxTextAttr::GetOutlineLevel(void* attr)
	{
		return Attr(attr)->GetOutlineLevel();
	}

	bool TextBoxTextAttr::IsCharacterStyle(void* attr)
	{
		return Attr(attr)->IsCharacterStyle();
	}

	bool TextBoxTextAttr::IsParagraphStyle(void* attr)
	{
		return Attr(attr)->IsParagraphStyle();
	}

	bool TextBoxTextAttr::IsDefault(void* attr)
	{
		return Attr(attr)->IsDefault();
	}
}
