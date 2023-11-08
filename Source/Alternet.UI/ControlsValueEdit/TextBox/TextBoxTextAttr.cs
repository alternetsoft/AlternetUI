using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TextBoxTextAttr : DisposableObject, ITextBoxTextAttr
    {
        public TextBoxTextAttr()
            : base(Native.TextBoxTextAttr.CreateTextAttr(), true)
        {
        }

        public TextBoxTextAttr(IntPtr handle)
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

        public void SetFontInfo(FontInfo value)
        {
            SetFontStyle(value.Style);
            SetFontFaceName(value.Name);
            SetFontPointSize(value.SizeInPoints);
    }

        public void SetTextColor(Color colText)
        {
            Native.TextBoxTextAttr.SetTextColor(Handle, colText);
        }

        public void SetBackgroundColor(Color colBack)
        {
            Native.TextBoxTextAttr.SetBackgroundColor(Handle, colBack);
        }

        public bool GetFontItalic()
        {
            const int wxNORMAL = 90;

            var fs = Native.TextBoxTextAttr.GetFontStyle(Handle);
            return fs != wxNORMAL;
        }

        public void SetFontItalic(bool italic)
        {
            const int wxNORMAL = 90;
            const int wxITALIC = 93;

            if (italic)
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxITALIC);
            else
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxNORMAL);
        }

        public void SetFontSlanted(bool slanted)
        {
            const int wxNORMAL = 90;
            const int wxSLANT = 94;

            if (slanted)
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxSLANT);
            else
                Native.TextBoxTextAttr.SetFontStyle(Handle, wxNORMAL);
        }

        public void Copy(ITextBoxTextAttr fromAttr)
        {
            if (fromAttr is not TextBoxTextAttr s)
                return;

            Native.TextBoxTextAttr.Copy(Handle, s.Handle);
        }

        public void SetFontPointSize(double pointSize)
        {
            SetFontPointSize((int)pointSize);
        }

        public void SetFontPointSize(int pointSize)
        {
            Native.TextBoxTextAttr.SetFontPointSize(Handle, pointSize);
        }

        public void SetFontFaceName(string faceName)
        {
            Native.TextBoxTextAttr.SetFontFaceName(Handle, faceName);
        }

        public void SetFontUnderlined(bool underlined)
        {
            Native.TextBoxTextAttr.SetFontUnderlined(Handle, underlined);
        }

        public void SetFontStrikethrough(bool strikethrough)
        {
            Native.TextBoxTextAttr.SetFontStrikethrough(Handle, strikethrough);
        }

        public void SetBulletNumber(int n)
        {
            Native.TextBoxTextAttr.SetBulletNumber(Handle, n);
        }

        public void SetBulletText(string text)
        {
            Native.TextBoxTextAttr.SetBulletText(Handle, text);
        }

        public void SetPageBreak(bool pageBreak)
        {
            Native.TextBoxTextAttr.SetPageBreak(Handle, pageBreak);
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

        public void SetURL(string url)
        {
            Native.TextBoxTextAttr.SetURL(Handle, url);
        }

        public void SetParagraphSpacingAfter(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingAfter(Handle, spacing);
        }

        public void SetParagraphSpacingBefore(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingBefore(Handle, spacing);
        }

        public void SetLineSpacing(int spacing)
        {
            Native.TextBoxTextAttr.SetLineSpacing(Handle, spacing);
        }

        public void SetCharacterStyleName(string name)
        {
            Native.TextBoxTextAttr.SetCharacterStyleName(Handle, name);
        }

        public void SetParagraphStyleName(string name)
        {
            Native.TextBoxTextAttr.SetParagraphStyleName(Handle, name);
        }

        public void SetListStyleName(string name)
        {
            Native.TextBoxTextAttr.SetListStyleName(Handle, name);
        }

        public void SetBulletFont(string bulletFont)
        {
            Native.TextBoxTextAttr.SetBulletFont(Handle, bulletFont);
        }

        public void SetBulletName(string name)
        {
            Native.TextBoxTextAttr.SetBulletName(Handle, name);
        }

        public void SetOutlineLevel(int level)
        {
            Native.TextBoxTextAttr.SetOutlineLevel(Handle, level);
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

        public void SetFontUnderlinedEx(
            TextBoxTextAttrUnderlineType type,
            Color color)
        {
            Native.TextBoxTextAttr.SetFontUnderlinedEx(Handle, (int)type, color);
        }

        public TextBoxTextAttrUnderlineType GetUnderlineType()
        {
            return
                (TextBoxTextAttrUnderlineType)Native.TextBoxTextAttr.
                    GetUnderlineType(Handle);
        }

        public void SetFontWeight(FontWeight fontWeight)
        {
            Native.TextBoxTextAttr.SetFontWeight(Handle, (int)fontWeight);
        }

        public FontWeight GetFontWeight()
        {
            return (FontWeight)Native.TextBoxTextAttr.GetFontWeight(Handle);
        }

        public void SetTextEffects(TextBoxTextAttrEffects effects)
        {
            Native.TextBoxTextAttr.SetTextEffects(Handle, (int)effects);
        }

        public TextBoxTextAttrEffects GetTextEffects()
        {
            return
                (TextBoxTextAttrEffects)Native.TextBoxTextAttr.GetTextEffects(Handle);
        }

        public void SetAlignment(TextBoxTextAttrAlignment alignment)
        {
            Native.TextBoxTextAttr.SetAlignment(Handle, (int)alignment);
        }

        public TextBoxTextAttrAlignment GetAlignment()
        {
            return
                (TextBoxTextAttrAlignment)Native.TextBoxTextAttr.GetAlignment(Handle);
        }

        public void SetBulletStyle(TextBoxTextAttrBulletStyle style)
        {
            Native.TextBoxTextAttr.SetBulletStyle(Handle, (int)style);
        }

        public TextBoxTextAttrBulletStyle GetBulletStyle()
        {
            return
                (TextBoxTextAttrBulletStyle)Native.TextBoxTextAttr.GetBulletStyle(Handle);
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
            if (fontStyle.HasFlag(FontStyle.Bold))
                SetFontWeight(FontWeight.Bold);
            else
                SetFontWeight(FontWeight.Normal);
            SetFontItalic(fontStyle.HasFlag(FontStyle.Italic));
            SetFontStrikethrough(fontStyle.HasFlag(FontStyle.Strikethrough));
            SetFontUnderlined(fontStyle.HasFlag(FontStyle.Underlined));
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
                result |= FontStyle.Underlined;
            if (isStrikeOut)
                result |= FontStyle.Strikethrough;
            return result;
        }

        public bool HasFlag(TextBoxTextAttrFlags flag)
        {
            return Native.TextBoxTextAttr.HasFlag(Handle, (int)flag);
        }

        public void RemoveFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.RemoveFlag(Handle, (int)flag);
        }

        public void AddFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.AddFlag(Handle, (int)flag);
        }

        public void SetFontFamily(GenericFontFamily family)
        {
            Native.TextBoxTextAttr.SetFontFamily(Handle, (int)family);
        }

        public TextBoxTextAttrFlags GetFlags()
        {
            return
                (TextBoxTextAttrFlags)Native.TextBoxTextAttr.GetFlags(Handle);
        }

        public void SetFlags(TextBoxTextAttrFlags flags)
        {
            Native.TextBoxTextAttr.SetFlags(Handle, (int)flags);
        }

        protected override void DisposeUnmanagedResources()
        {
            Native.TextBoxTextAttr.Delete(Handle);
        }
    }
}
