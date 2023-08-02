using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class TextBoxTextAttr : ITextBoxTextAttr, IDisposable
    {
        private IntPtr handle;

        public TextBoxTextAttr()
        {
            handle = Native.TextBoxTextAttr.CreateTextAttr();
        }

        public TextBoxTextAttr(IntPtr handle)
        {
            this.handle = handle;
        }

        ~TextBoxTextAttr()
        {
            Dispose();
        }

        public IntPtr Handle => handle;

        public void SetTextColor(Color colText)
        {
            Native.TextBoxTextAttr.SetTextColor(handle, colText);
        }

        public void SetBackgroundColor(Color colBack)
        {
            Native.TextBoxTextAttr.SetBackgroundColor(handle, colBack);
        }

        public void SetFontItalic(bool italic)
        {
            const int wxNORMAL = 90;
            const int wxITALIC = 93;

            if (italic)
                Native.TextBoxTextAttr.SetFontStyle(handle, wxITALIC);
            else
                Native.TextBoxTextAttr.SetFontStyle(handle, wxNORMAL);
        }

        public void SetFontSlanted(bool slanted)
        {
            const int wxNORMAL = 90;
            const int wxSLANT = 94;

            if (slanted)
                Native.TextBoxTextAttr.SetFontStyle(handle, wxSLANT);
            else
                Native.TextBoxTextAttr.SetFontStyle(handle, wxNORMAL);
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                Native.TextBoxTextAttr.Delete(handle);
                handle = IntPtr.Zero;
            }
        }

        public void Copy(ITextBoxTextAttr fromAttr)
        {
            if (fromAttr is not TextBoxTextAttr s)
                return;

            Native.TextBoxTextAttr.Copy(handle, s.Handle);
        }

        public void SetFontPointSize(int pointSize)
        {
            Native.TextBoxTextAttr.SetFontPointSize(handle, pointSize);
        }

        public void SetFontFaceName(string faceName)
        {
            Native.TextBoxTextAttr.SetFontFaceName(handle, faceName);
        }

        public void SetFontUnderlined(bool underlined)
        {
            Native.TextBoxTextAttr.SetFontUnderlined(handle, underlined);
        }

        public void SetFontStrikethrough(bool strikethrough)
        {
            Native.TextBoxTextAttr.SetFontStrikethrough(handle, strikethrough);
        }

        public void SetBulletNumber(int n)
        {
            Native.TextBoxTextAttr.SetBulletNumber(handle, n);
        }

        public void SetBulletText(string text)
        {
            Native.TextBoxTextAttr.SetBulletText(handle, text);
        }

        public void SetPageBreak(bool pageBreak)
        {
            Native.TextBoxTextAttr.SetPageBreak(handle, pageBreak);
        }

        public int GetOutlineLevel()
        {
            return Native.TextBoxTextAttr.GetOutlineLevel(handle);
        }

        public bool IsCharacterStyle()
        {
            return Native.TextBoxTextAttr.IsCharacterStyle(handle);
        }

        public bool IsParagraphStyle()
        {
            return Native.TextBoxTextAttr.IsParagraphStyle(handle);
        }

        public bool GetFontUnderlined()
        {
            return Native.TextBoxTextAttr.GetFontUnderlined(handle);
        }

        public Color GetUnderlineColor()
        {
            return Native.TextBoxTextAttr.GetUnderlineColor(handle);
        }

        public bool GetFontStrikethrough()
        {
            return Native.TextBoxTextAttr.GetFontStrikethrough(handle);
        }

        public string GetFontFaceName()
        {
            return Native.TextBoxTextAttr.GetFontFaceName(handle);
        }

        public int GetParagraphSpacingAfter()
        {
            return Native.TextBoxTextAttr.GetParagraphSpacingAfter(handle);
        }

        public int GetParagraphSpacingBefore()
        {
            return Native.TextBoxTextAttr.GetParagraphSpacingBefore(handle);
        }

        public int GetLineSpacing()
        {
            return Native.TextBoxTextAttr.GetLineSpacing(handle);
        }

        public int GetBulletNumber()
        {
            return Native.TextBoxTextAttr.GetBulletNumber(handle);
        }

        public string GetBulletText()
        {
            return Native.TextBoxTextAttr.GetBulletText(handle);
        }

        public string GetURL()
        {
            return Native.TextBoxTextAttr.GetURL(handle);
        }

        public Color GetTextColor()
        {
            return Native.TextBoxTextAttr.GetTextColor(handle);
        }

        public Color GetBackgroundColor()
        {
            return Native.TextBoxTextAttr.GetBackgroundColor(handle);
        }

        public void SetURL(string url)
        {
            Native.TextBoxTextAttr.SetURL(handle, url);
        }

        public void SetParagraphSpacingAfter(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingAfter(handle, spacing);
        }

        public void SetParagraphSpacingBefore(int spacing)
        {
            Native.TextBoxTextAttr.SetParagraphSpacingBefore(handle, spacing);
        }

        public void SetLineSpacing(int spacing)
        {
            Native.TextBoxTextAttr.SetLineSpacing(handle, spacing);
        }

        public void SetCharacterStyleName(string name)
        {
            Native.TextBoxTextAttr.SetCharacterStyleName(handle, name);
        }

        public void SetParagraphStyleName(string name)
        {
            Native.TextBoxTextAttr.SetParagraphStyleName(handle, name);
        }

        public void SetListStyleName(string name)
        {
            Native.TextBoxTextAttr.SetListStyleName(handle, name);
        }

        public void SetBulletFont(string bulletFont)
        {
            Native.TextBoxTextAttr.SetBulletFont(handle, bulletFont);
        }

        public void SetBulletName(string name)
        {
            Native.TextBoxTextAttr.SetBulletName(handle, name);
        }

        public void SetOutlineLevel(int level)
        {
            Native.TextBoxTextAttr.SetOutlineLevel(handle, level);
        }

        public int GetFontSize()
        {
            return Native.TextBoxTextAttr.GetFontSize(handle);
        }

        public bool IsDefault()
        {
            return Native.TextBoxTextAttr.IsDefault(handle);
        }

        public bool HasTextColor()
        {
            return Native.TextBoxTextAttr.HasTextColor(handle);
        }

        public bool HasBackgroundColor()
        {
            return Native.TextBoxTextAttr.HasBackgroundColor(handle);
        }

        public bool HasAlignment()
        {
            return Native.TextBoxTextAttr.HasAlignment(handle);
        }

        public bool HasTabs()
        {
            return Native.TextBoxTextAttr.HasTabs(handle);
        }

        public bool HasLeftIndent()
        {
            return Native.TextBoxTextAttr.HasLeftIndent(handle);
        }

        public bool HasRightIndent()
        {
            return Native.TextBoxTextAttr.HasRightIndent(handle);
        }

        public bool HasFontWeight()
        {
            return Native.TextBoxTextAttr.HasFontWeight(handle);
        }

        public bool HasFontSize()
        {
            return Native.TextBoxTextAttr.HasFontSize(handle);
        }

        public bool HasFontPointSize()
        {
            return Native.TextBoxTextAttr.HasFontPointSize(handle);
        }

        public bool HasFontPixelSize()
        {
            return Native.TextBoxTextAttr.HasFontPixelSize(handle);
        }

        public bool HasFontItalic()
        {
            return Native.TextBoxTextAttr.HasFontItalic(handle);
        }

        public bool HasFontUnderlined()
        {
            return Native.TextBoxTextAttr.HasFontUnderlined(handle);
        }

        public bool HasFontStrikethrough()
        {
            return Native.TextBoxTextAttr.HasFontStrikethrough(handle);
        }

        public bool HasFontFaceName()
        {
            return Native.TextBoxTextAttr.HasFontFaceName(handle);
        }

        public bool HasFontEncoding()
        {
            return Native.TextBoxTextAttr.HasFontEncoding(handle);
        }

        public bool HasFontFamily()
        {
            return Native.TextBoxTextAttr.HasFontFamily(handle);
        }

        public bool HasFont()
        {
            return Native.TextBoxTextAttr.HasFont(handle);
        }

        public bool HasParagraphSpacingAfter()
        {
            return Native.TextBoxTextAttr.HasParagraphSpacingAfter(handle);
        }

        public bool HasParagraphSpacingBefore()
        {
            return Native.TextBoxTextAttr.HasParagraphSpacingBefore(handle);
        }

        public bool HasLineSpacing()
        {
            return Native.TextBoxTextAttr.HasLineSpacing(handle);
        }

        public bool HasCharacterStyleName()
        {
            return Native.TextBoxTextAttr.HasCharacterStyleName(handle);
        }

        public bool HasParagraphStyleName()
        {
            return Native.TextBoxTextAttr.HasParagraphStyleName(handle);
        }

        public bool HasListStyleName()
        {
            return Native.TextBoxTextAttr.HasListStyleName(handle);
        }

        public bool HasBulletStyle()
        {
            return Native.TextBoxTextAttr.HasBulletStyle(handle);
        }

        public bool HasBulletNumber()
        {
            return Native.TextBoxTextAttr.HasBulletNumber(handle);
        }

        public bool HasBulletText()
        {
            return Native.TextBoxTextAttr.HasBulletText(handle);
        }

        public bool HasBulletName()
        {
            return Native.TextBoxTextAttr.HasBulletName(handle);
        }

        public bool HasURL()
        {
            return Native.TextBoxTextAttr.HasURL(handle);
        }

        public bool HasPageBreak()
        {
            return Native.TextBoxTextAttr.HasPageBreak(handle);
        }

        public bool HasTextEffects()
        {
            return Native.TextBoxTextAttr.HasTextEffects(handle);
        }

        public bool HasTextEffect(TextBoxTextAttrEffects effect)
        {
            return Native.TextBoxTextAttr.HasTextEffect(handle, (int)effect);
        }

        public bool HasOutlineLevel()
        {
            return Native.TextBoxTextAttr.HasOutlineLevel(handle);
        }

        public void SetFontUnderlinedEx(
            TextBoxTextAttrUnderlineType type,
            Color color)
        {
            Native.TextBoxTextAttr.SetFontUnderlinedEx(handle, (int)type, color);
        }

        public TextBoxTextAttrUnderlineType GetUnderlineType()
        {
            return (TextBoxTextAttrUnderlineType)
                Native.TextBoxTextAttr.GetUnderlineType(handle);
        }

        public void SetFontWeight(FontWeight fontWeight)
        {
            Native.TextBoxTextAttr.SetFontWeight(handle, (int)fontWeight);
        }

        public FontWeight GetFontWeight()
        {
            return (FontWeight)Native.TextBoxTextAttr.GetFontWeight(handle);
        }

        public void SetTextEffects(TextBoxTextAttrEffects effects)
        {
            Native.TextBoxTextAttr.SetTextEffects(handle, (int)effects);
        }

        public TextBoxTextAttrEffects GetTextEffects()
        {
            return
                (TextBoxTextAttrEffects)Native.TextBoxTextAttr.GetTextEffects(handle);
        }

        public void SetAlignment(TextBoxTextAttrAlignment alignment)
        {
            Native.TextBoxTextAttr.SetAlignment(handle, (int)alignment);
        }

        public TextBoxTextAttrAlignment GetAlignment()
        {
            return
                (TextBoxTextAttrAlignment)Native.TextBoxTextAttr.GetAlignment(handle);
        }

        public void SetBulletStyle(TextBoxTextAttrBulletStyle style)
        {
            Native.TextBoxTextAttr.SetBulletStyle(handle, (int)style);
        }

        public TextBoxTextAttrBulletStyle GetBulletStyle()
        {
            return
                (TextBoxTextAttrBulletStyle)Native.TextBoxTextAttr.GetBulletStyle(handle);
        }

        /*public void SetFontStyle(FontStyle fontStyle)
        {
            throw new NotImplementedException();
        }

        public FontStyle GetFontStyle()
        {
            throw new NotImplementedException();
        }*/

        public bool HasFlag(TextBoxTextAttrFlags flag)
        {
            return Native.TextBoxTextAttr.HasFlag(handle, (int)flag);
        }

        public void RemoveFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.RemoveFlag(handle, (int)flag);
        }

        public void AddFlag(TextBoxTextAttrFlags flag)
        {
            Native.TextBoxTextAttr.AddFlag(handle, (int)flag);
        }

        public void SetFontFamily(GenericFontFamily family)
        {
            Native.TextBoxTextAttr.SetFontFamily(handle, (int)family);
        }

        public TextBoxTextAttrFlags GetFlags()
        {
            return
                (TextBoxTextAttrFlags)Native.TextBoxTextAttr.GetFlags(handle);
        }

        public void SetFlags(TextBoxTextAttrFlags flags)
        {
            Native.TextBoxTextAttr.SetFlags(handle, (int)flags);
        }
    }
}
