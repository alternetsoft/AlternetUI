using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class WxTextBoxTextAttr : DisposableObject<IntPtr>, ITextBoxTextAttr
    {
        public WxTextBoxTextAttr()
            : base(Native.TextBoxTextAttr.CreateTextAttr(), true)
        {
        }

        public WxTextBoxTextAttr(IntPtr handle)
            : base(handle, true)
        {
        }

        public FontInfo GetFontInfo()
        {
            var fontStyle = GetFontStyle();
            var size = GetFontSize();
            var name = GetFontFaceName();
            return new(name, size, fontStyle);
        }

        public ITextBoxTextAttr SetFontInfo(FontInfo value)
        {
            SetFontStyle(value.Style);
            SetFontFaceName(value.Name);
            SetFontPointSize(value.SizeInPoints);
            return this;
        }

        public ITextBoxTextAttr SetTextColor(Color colText)
        {
            Native.TextBoxTextAttr.SetTextColor(Handle, colText);
            return this;
        }

        public ITextBoxTextAttr SetBackgroundColor(Color colBack)
        {
            Native.TextBoxTextAttr.SetBackgroundColor(Handle, colBack);
            return this;
        }

        public bool GetFontItalic()
        {
            const int wxNORMAL = 90;

            var fs = Native.TextBoxTextAttr.GetFontStyle(Handle);
            return fs != wxNORMAL;
        }

        public ITextBoxTextAttr SetFontItalic(bool italic)
        {
            const int wxNORMAL = 90;
            const int wxITALIC = 93;

            if (italic)
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxITALIC);
            else
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxNORMAL);
            return this;
        }

        public ITextBoxTextAttr SetFontSlanted(bool slanted)
        {
            const int wxNORMAL = 90;
            const int wxSLANT = 94;

            if (slanted)
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxSLANT);
            else
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxNORMAL);
            return this;
        }

        public void Copy(ITextBoxTextAttr fromAttr)
        {
            if (fromAttr is not WxTextBoxTextAttr s)
                return;

            Native.TextBoxTextAttr.Copy(Handle, s.Handle);
        }

        public ITextBoxTextAttr SetFontPointSize(Coord pointSize)
        {
            SetFontPointSize((int)pointSize);
            return this;
        }

        public ITextBoxTextAttr SetFontPointSize(int pointSize)
        {
            Native.TextBoxTextAttr.SetFontPointSize(Handle, pointSize);
            return this;
        }

        public ITextBoxTextAttr SetFontFaceName(string faceName)
        {
            Native.TextBoxTextAttr.SetFontFaceName(Handle, faceName);
            return this;
        }

        public ITextBoxTextAttr SetFontUnderlined(bool underlined)
        {
            Native.TextBoxTextAttr.SetFontUnderlined(Handle, underlined);
            return this;
        }

        public ITextBoxTextAttr SetFontStrikethrough(bool strikethrough)
        {
            Native.TextBoxTextAttr.SetFontStrikethrough(Handle, strikethrough);
            return this;
        }

        public ITextBoxTextAttr SetBulletNumber(int n)
        {
            Native.TextBoxTextAttr.SetBulletNumber(Handle, n);
            return this;
        }

        public ITextBoxTextAttr SetBulletText(string text)
        {
            Native.TextBoxTextAttr.SetBulletText(Handle, text);
            return this;
        }

        public ITextBoxTextAttr SetPageBreak(bool pageBreak)
        {
            Native.TextBoxTextAttr.SetPageBreak(Handle, pageBreak);
            return this;
        }

        public int GetOutlineLevel()
        {
            return Native.TextBoxTextAttr.GetOutlineLevel(Handle);
        }

        public bool IsCharacterStyle()
        {
            return Native.TextBoxTextAttr.IsCharacterStyle(Handle);
        }

        public bool IsParagraphStyle()
        {
            return Native.TextBoxTextAttr.IsParagraphStyle(Handle);
        }

        public bool GetFontUnderlined()
        {
            return Native.TextBoxTextAttr.GetFontUnderlined(Handle);
        }

        public Color GetUnderlineColor()
        {
            return Native.TextBoxTextAttr.GetUnderlineColor(Handle);
        }

        public bool GetFontStrikethrough()
        {
            return Native.TextBoxTextAttr.GetFontStrikethrough(Handle);
        }

        public string GetFontFaceName()
        {
            return Native.TextBoxTextAttr.GetFontFaceName(Handle);
        }

        public int GetParagraphSpacingAfter()
        {
            return Native.TextBoxTextAttr.GetParagraphSpacingAfter(Handle);
        }

        public int GetParagraphSpacingBefore()
        {
            return Native.TextBoxTextAttr.GetParagraphSpacingBefore(Handle);
        }

        public int GetLineSpacing()
        {
            return Native.TextBoxTextAttr.GetLineSpacing(Handle);
        }

        public int GetBulletNumber()
        {
            return Native.TextBoxTextAttr.GetBulletNumber(Handle);
        }

        public string GetBulletText()
        {
            return Native.TextBoxTextAttr.GetBulletText(Handle);
        }

        public string GetURL()
        {
            return Native.TextBoxTextAttr.GetURL(Handle);
        }

        public Color GetTextColor()
        {
            return Native.TextBoxTextAttr.GetTextColor(Handle);
        }

        public Color GetBackgroundColor()
        {
            return Native.TextBoxTextAttr.GetBackgroundColor(Handle);
        }

        public ITextBoxTextAttr SetURL(string url)
        {
            Native.TextBoxTextAttr.SetURL(Handle, url);
            return this;
        }

        public ITextBoxTextAttr SetParagraphSpacingAfter(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingAfter(Handle, spacing);
            return this;
        }

        public ITextBoxTextAttr SetParagraphSpacingBefore(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingBefore(Handle, spacing);
            return this;
        }

        public ITextBoxTextAttr SetLineSpacing(int spacing)
        {
            Native.TextBoxTextAttr.SetLineSpacing(Handle, spacing);
            return this;
        }

        public ITextBoxTextAttr SetCharacterStyleName(string name)
        {
            Native.TextBoxTextAttr.SetCharacterStyleName(Handle, name);
            return this;
        }

        public ITextBoxTextAttr SetParagraphStyleName(string name)
        {
            Native.TextBoxTextAttr.SetParagraphStyleName(Handle, name);
            return this;
        }

        public ITextBoxTextAttr SetListStyleName(string name)
        {
            Native.TextBoxTextAttr.SetListStyleName(Handle, name);
            return this;
        }

        public ITextBoxTextAttr SetBulletFont(string bulletFont)
        {
            Native.TextBoxTextAttr.SetBulletFont(Handle, bulletFont);
            return this;
        }

        public ITextBoxTextAttr SetBulletName(string name)
        {
            Native.TextBoxTextAttr.SetBulletName(Handle, name);
            return this;
        }

        public ITextBoxTextAttr SetOutlineLevel(int level)
        {
            Native.TextBoxTextAttr.SetOutlineLevel(Handle, level);
            return this;
        }

        public int GetFontSize()
        {
            return Native.TextBoxTextAttr.GetFontSize(Handle);
        }

        public bool IsDefault()
        {
            return Native.TextBoxTextAttr.IsDefault(Handle);
        }

        public bool HasTextColor()
        {
            return Native.TextBoxTextAttr.HasTextColor(Handle);
        }

        public bool HasBackgroundColor()
        {
            return Native.TextBoxTextAttr.HasBackgroundColor(Handle);
        }

        public bool HasAlignment()
        {
            return Native.TextBoxTextAttr.HasAlignment(Handle);
        }

        public bool HasTabs()
        {
            return Native.TextBoxTextAttr.HasTabs(Handle);
        }

        public bool HasLeftIndent()
        {
            return Native.TextBoxTextAttr.HasLeftIndent(Handle);
        }

        public bool HasRightIndent()
        {
            return Native.TextBoxTextAttr.HasRightIndent(Handle);
        }

        public bool HasFontWeight()
        {
            return Native.TextBoxTextAttr.HasFontWeight(Handle);
        }

        public bool HasFontSize()
        {
            return Native.TextBoxTextAttr.HasFontSize(Handle);
        }

        public bool HasFontPointSize()
        {
            return Native.TextBoxTextAttr.HasFontPointSize(Handle);
        }

        public bool HasFontPixelSize()
        {
            return Native.TextBoxTextAttr.HasFontPixelSize(Handle);
        }

        public bool HasFontItalic()
        {
            return Native.TextBoxTextAttr.HasFontItalic(Handle);
        }

        public bool HasFontUnderlined()
        {
            return Native.TextBoxTextAttr.HasFontUnderlined(Handle);
        }

        public bool HasFontStrikethrough()
        {
            return Native.TextBoxTextAttr.HasFontStrikethrough(Handle);
        }

        public bool HasFontFaceName()
        {
            return Native.TextBoxTextAttr.HasFontFaceName(Handle);
        }

        public bool HasFontEncoding()
        {
            return Native.TextBoxTextAttr.HasFontEncoding(Handle);
        }

        public bool HasFontFamily()
        {
            return Native.TextBoxTextAttr.HasFontFamily(Handle);
        }

        public bool HasFont()
        {
            return Native.TextBoxTextAttr.HasFont(Handle);
        }

        public bool HasParagraphSpacingAfter()
        {
            return Native.TextBoxTextAttr.HasParagraphSpacingAfter(Handle);
        }

        public bool HasParagraphSpacingBefore()
        {
            return Native.TextBoxTextAttr.HasParagraphSpacingBefore(Handle);
        }

        public bool HasLineSpacing()
        {
            return Native.TextBoxTextAttr.HasLineSpacing(Handle);
        }

        public bool HasCharacterStyleName()
        {
            return Native.TextBoxTextAttr.HasCharacterStyleName(Handle);
        }

        public bool HasParagraphStyleName()
        {
            return Native.TextBoxTextAttr.HasParagraphStyleName(Handle);
        }

        public bool HasListStyleName()
        {
            return Native.TextBoxTextAttr.HasListStyleName(Handle);
        }

        public bool HasBulletStyle()
        {
            return Native.TextBoxTextAttr.HasBulletStyle(Handle);
        }

        public bool HasBulletNumber()
        {
            return Native.TextBoxTextAttr.HasBulletNumber(Handle);
        }

        public bool HasBulletText()
        {
            return Native.TextBoxTextAttr.HasBulletText(Handle);
        }

        public bool HasBulletName()
        {
            return Native.TextBoxTextAttr.HasBulletName(Handle);
        }

        public bool HasURL()
        {
            return Native.TextBoxTextAttr.HasURL(Handle);
        }

        public bool HasPageBreak()
        {
            return Native.TextBoxTextAttr.HasPageBreak(Handle);
        }

        public bool HasTextEffects()
        {
            return Native.TextBoxTextAttr.HasTextEffects(Handle);
        }

        public bool HasTextEffect(TextBoxTextAttrEffects effect)
        {
            return Native.TextBoxTextAttr.HasTextEffect(Handle, (int)effect);
        }

        public bool HasOutlineLevel()
        {
            return Native.TextBoxTextAttr.HasOutlineLevel(Handle);
        }

        public ITextBoxTextAttr SetFontUnderlinedEx(
            TextBoxTextAttrUnderlineType type,
            Color color)
        {
            Native.TextBoxTextAttr.SetFontUnderlinedEx(Handle, (int)type, color);
            return this;
        }

        public TextBoxTextAttrUnderlineType GetUnderlineType()
        {
            return
                (TextBoxTextAttrUnderlineType)Native.TextBoxTextAttr.
                    GetUnderlineType(Handle);
        }

        public ITextBoxTextAttr SetFontWeight(FontWeight fontWeight)
        {
            Native.TextBoxTextAttr.SetFontWeight(Handle, (int)fontWeight);
            return this;
        }

        public FontWeight GetFontWeight()
        {
            return (FontWeight)Native.TextBoxTextAttr.GetFontWeight(Handle);
        }

        public ITextBoxTextAttr SetTextEffects(TextBoxTextAttrEffects effects)
        {
            Native.TextBoxTextAttr.SetTextEffects(Handle, (int)effects);
            return this;
        }

        public TextBoxTextAttrEffects GetTextEffects()
        {
            return
                (TextBoxTextAttrEffects)Native.TextBoxTextAttr.GetTextEffects(Handle);
        }

        public ITextBoxTextAttr SetAlignment(TextBoxTextAttrAlignment alignment)
        {
            Native.TextBoxTextAttr.SetAlignment(Handle, (int)alignment);
            return this;
        }

        public TextBoxTextAttrAlignment GetAlignment()
        {
            return
                (TextBoxTextAttrAlignment)Native.TextBoxTextAttr.GetAlignment(Handle);
        }

        public ITextBoxTextAttr SetBulletStyle(TextBoxTextAttrBulletStyle style)
        {
            Native.TextBoxTextAttr.SetBulletStyle(Handle, (int)style);
            return this;
        }

        public TextBoxTextAttrBulletStyle GetBulletStyle()
        {
            return
                (TextBoxTextAttrBulletStyle)Native.TextBoxTextAttr.GetBulletStyle(Handle);
        }

        public ITextBoxTextAttr SetFontStyle(FontStyle fontStyle)
        {
            if (fontStyle.HasFlag(FontStyle.Bold))
                SetFontWeight(FontWeight.Bold);
            else
                SetFontWeight(FontWeight.Normal);
            SetFontItalic(fontStyle.HasFlag(FontStyle.Italic));
            SetFontStrikethrough(fontStyle.HasFlag(FontStyle.Strikeout));
            SetFontUnderlined(fontStyle.HasFlag(FontStyle.Underline));
            return this;
        }

        public FontStyle GetFontStyle()
        {
            var fontWeight = GetFontWeight();
            FontStyle result = FontStyle.Regular;

            var isBold = fontWeight > FontWeight.Normal;
            var isItalic = GetFontItalic();
            var isUnderlined = GetFontUnderlined();
            var isStrikeOut = GetFontStrikethrough();
            if (isBold)
                result |= FontStyle.Bold;
            if (isItalic)
                result |= FontStyle.Italic;
            if (isUnderlined)
                result |= FontStyle.Underline;
            if (isStrikeOut)
                result |= FontStyle.Strikeout;
            return result;
        }

        public bool HasFlag(TextBoxTextAttrFlags flag)
        {
            return Native.TextBoxTextAttr.HasFlag(Handle, (int)flag);
        }

        public ITextBoxTextAttr RemoveFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.RemoveFlag(Handle, (int)flag);
            return this;
        }

        public ITextBoxTextAttr AddFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.AddFlag(Handle, (int)flag);
            return this;
        }

        public ITextBoxTextAttr SetFontFamily(GenericFontFamily family)
        {
            Native.TextBoxTextAttr.SetFontFamily(Handle, (int)family);
            return this;
        }

        public TextBoxTextAttrFlags GetFlags()
        {
            return
                (TextBoxTextAttrFlags)Native.TextBoxTextAttr.GetFlags(Handle);
        }

        public ITextBoxTextAttr SetFlags(TextBoxTextAttrFlags flags)
        {
            Native.TextBoxTextAttr.SetFlags(Handle, (int)flags);
            return this;
        }

        protected override void DisposeUnmanaged()
        {
            Native.TextBoxTextAttr.Delete(Handle);
        }
    }
}
