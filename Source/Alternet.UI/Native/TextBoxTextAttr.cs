// <auto-generated>This code was generated by a tool, DO NOT MODIFY MANUALLY. Copyright AlterNET, 2023.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal class TextBoxTextAttr : NativeObject
    {
        static TextBoxTextAttr()
        {
        }
        
        public TextBoxTextAttr()
        {
            SetNativePointer(NativeApi.TextBoxTextAttr_Create_());
        }
        
        public TextBoxTextAttr(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public static void Delete(System.IntPtr attr)
        {
            NativeApi.TextBoxTextAttr_Delete_(attr);
        }
        
        public static void Copy(System.IntPtr toAttr, System.IntPtr fromAttr2)
        {
            NativeApi.TextBoxTextAttr_Copy_(toAttr, fromAttr2);
        }
        
        public static System.IntPtr CreateTextAttr()
        {
            var n = NativeApi.TextBoxTextAttr_CreateTextAttr_();
            var m = n;
            return m;
        }
        
        public static void SetTextColor(System.IntPtr attr, Alternet.Drawing.Color colText)
        {
            NativeApi.TextBoxTextAttr_SetTextColor_(attr, colText);
        }
        
        public static void SetBackgroundColor(System.IntPtr attr, Alternet.Drawing.Color colBack)
        {
            NativeApi.TextBoxTextAttr_SetBackgroundColor_(attr, colBack);
        }
        
        public static void SetAlignment(System.IntPtr attr, int alignment)
        {
            NativeApi.TextBoxTextAttr_SetAlignment_(attr, alignment);
        }
        
        public static void SetFontPointSize(System.IntPtr attr, int pointSize)
        {
            NativeApi.TextBoxTextAttr_SetFontPointSize_(attr, pointSize);
        }
        
        public static void SetFontStyle(System.IntPtr attr, int fontStyle)
        {
            NativeApi.TextBoxTextAttr_SetFontStyle_(attr, fontStyle);
        }
        
        public static void SetFontWeight(System.IntPtr attr, int fontWeight)
        {
            NativeApi.TextBoxTextAttr_SetFontWeight_(attr, fontWeight);
        }
        
        public static void SetFontFaceName(System.IntPtr attr, string faceName)
        {
            NativeApi.TextBoxTextAttr_SetFontFaceName_(attr, faceName);
        }
        
        public static void SetFontUnderlined(System.IntPtr attr, bool underlined)
        {
            NativeApi.TextBoxTextAttr_SetFontUnderlined_(attr, underlined);
        }
        
        public static void SetFontUnderlinedEx(System.IntPtr attr, int type, Alternet.Drawing.Color colour)
        {
            NativeApi.TextBoxTextAttr_SetFontUnderlinedEx_(attr, type, colour);
        }
        
        public static void SetFontStrikethrough(System.IntPtr attr, bool strikethrough)
        {
            NativeApi.TextBoxTextAttr_SetFontStrikethrough_(attr, strikethrough);
        }
        
        public static void SetFontFamily(System.IntPtr attr, int family)
        {
            NativeApi.TextBoxTextAttr_SetFontFamily_(attr, family);
        }
        
        public static Alternet.Drawing.Color GetTextColor(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetTextColor_(attr);
            var m = n;
            return m;
        }
        
        public static Alternet.Drawing.Color GetBackgroundColor(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetBackgroundColor_(attr);
            var m = n;
            return m;
        }
        
        public static int GetAlignment(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetAlignment_(attr);
            var m = n;
            return m;
        }
        
        public static void SetURL(System.IntPtr attr, string url)
        {
            NativeApi.TextBoxTextAttr_SetURL_(attr, url);
        }
        
        public static void SetFlags(System.IntPtr attr, long flags)
        {
            NativeApi.TextBoxTextAttr_SetFlags_(attr, flags);
        }
        
        public static void SetParagraphSpacingAfter(System.IntPtr attr, int spacing)
        {
            NativeApi.TextBoxTextAttr_SetParagraphSpacingAfter_(attr, spacing);
        }
        
        public static void SetParagraphSpacingBefore(System.IntPtr attr, int spacing)
        {
            NativeApi.TextBoxTextAttr_SetParagraphSpacingBefore_(attr, spacing);
        }
        
        public static void SetLineSpacing(System.IntPtr attr, int spacing)
        {
            NativeApi.TextBoxTextAttr_SetLineSpacing_(attr, spacing);
        }
        
        public static void SetBulletStyle(System.IntPtr attr, int style)
        {
            NativeApi.TextBoxTextAttr_SetBulletStyle_(attr, style);
        }
        
        public static void SetBulletNumber(System.IntPtr attr, int n)
        {
            NativeApi.TextBoxTextAttr_SetBulletNumber_(attr, n);
        }
        
        public static void SetBulletText(System.IntPtr attr, string text)
        {
            NativeApi.TextBoxTextAttr_SetBulletText_(attr, text);
        }
        
        public static void SetPageBreak(System.IntPtr attr, bool pageBreak)
        {
            NativeApi.TextBoxTextAttr_SetPageBreak_(attr, pageBreak);
        }
        
        public static void SetCharacterStyleName(System.IntPtr attr, string name)
        {
            NativeApi.TextBoxTextAttr_SetCharacterStyleName_(attr, name);
        }
        
        public static void SetParagraphStyleName(System.IntPtr attr, string name)
        {
            NativeApi.TextBoxTextAttr_SetParagraphStyleName_(attr, name);
        }
        
        public static void SetListStyleName(System.IntPtr attr, string name)
        {
            NativeApi.TextBoxTextAttr_SetListStyleName_(attr, name);
        }
        
        public static void SetBulletFont(System.IntPtr attr, string bulletFont)
        {
            NativeApi.TextBoxTextAttr_SetBulletFont_(attr, bulletFont);
        }
        
        public static void SetBulletName(System.IntPtr attr, string name)
        {
            NativeApi.TextBoxTextAttr_SetBulletName_(attr, name);
        }
        
        public static void SetTextEffects(System.IntPtr attr, int effects)
        {
            NativeApi.TextBoxTextAttr_SetTextEffects_(attr, effects);
        }
        
        public static void SetTextEffectFlags(System.IntPtr attr, int effects)
        {
            NativeApi.TextBoxTextAttr_SetTextEffectFlags_(attr, effects);
        }
        
        public static void SetOutlineLevel(System.IntPtr attr, int level)
        {
            NativeApi.TextBoxTextAttr_SetOutlineLevel_(attr, level);
        }
        
        public static long GetFlags(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFlags_(attr);
            var m = n;
            return m;
        }
        
        public static int GetFontSize(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontSize_(attr);
            var m = n;
            return m;
        }
        
        public static int GetFontStyle(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontStyle_(attr);
            var m = n;
            return m;
        }
        
        public static int GetFontWeight(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontWeight_(attr);
            var m = n;
            return m;
        }
        
        public static bool GetFontUnderlined(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontUnderlined_(attr);
            var m = n;
            return m;
        }
        
        public static int GetUnderlineType(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetUnderlineType_(attr);
            var m = n;
            return m;
        }
        
        public static Alternet.Drawing.Color GetUnderlineColor(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetUnderlineColor_(attr);
            var m = n;
            return m;
        }
        
        public static bool GetFontStrikethrough(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontStrikethrough_(attr);
            var m = n;
            return m;
        }
        
        public static string GetFontFaceName(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontFaceName_(attr);
            var m = n;
            return m;
        }
        
        public static int GetFontFamily(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetFontFamily_(attr);
            var m = n;
            return m;
        }
        
        public static int GetParagraphSpacingAfter(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetParagraphSpacingAfter_(attr);
            var m = n;
            return m;
        }
        
        public static int GetParagraphSpacingBefore(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetParagraphSpacingBefore_(attr);
            var m = n;
            return m;
        }
        
        public static int GetLineSpacing(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetLineSpacing_(attr);
            var m = n;
            return m;
        }
        
        public static int GetBulletStyle(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetBulletStyle_(attr);
            var m = n;
            return m;
        }
        
        public static int GetBulletNumber(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetBulletNumber_(attr);
            var m = n;
            return m;
        }
        
        public static string GetBulletText(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetBulletText_(attr);
            var m = n;
            return m;
        }
        
        public static string GetURL(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetURL_(attr);
            var m = n;
            return m;
        }
        
        public static int GetTextEffects(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetTextEffects_(attr);
            var m = n;
            return m;
        }
        
        public static int GetTextEffectFlags(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetTextEffectFlags_(attr);
            var m = n;
            return m;
        }
        
        public static int GetOutlineLevel(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_GetOutlineLevel_(attr);
            var m = n;
            return m;
        }
        
        public static bool IsCharacterStyle(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_IsCharacterStyle_(attr);
            var m = n;
            return m;
        }
        
        public static bool IsParagraphStyle(System.IntPtr attr)
        {
            var n = NativeApi.TextBoxTextAttr_IsParagraphStyle_(attr);
            var m = n;
            return m;
        }
        
        public bool IsDefault(System.IntPtr attr)
        {
            CheckDisposed();
            var n = NativeApi.TextBoxTextAttr_IsDefault_(NativePointer, attr);
            var m = n;
            return m;
        }
        
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TextBoxTextAttr_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_Delete_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_Copy_(System.IntPtr toAttr, System.IntPtr fromAttr2);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TextBoxTextAttr_CreateTextAttr_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetTextColor_(System.IntPtr attr, NativeApiTypes.Color colText);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBackgroundColor_(System.IntPtr attr, NativeApiTypes.Color colBack);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetAlignment_(System.IntPtr attr, int alignment);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontPointSize_(System.IntPtr attr, int pointSize);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontStyle_(System.IntPtr attr, int fontStyle);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontWeight_(System.IntPtr attr, int fontWeight);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontFaceName_(System.IntPtr attr, string faceName);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontUnderlined_(System.IntPtr attr, bool underlined);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontUnderlinedEx_(System.IntPtr attr, int type, NativeApiTypes.Color colour);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontStrikethrough_(System.IntPtr attr, bool strikethrough);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFontFamily_(System.IntPtr attr, int family);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color TextBoxTextAttr_GetTextColor_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color TextBoxTextAttr_GetBackgroundColor_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetAlignment_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetURL_(System.IntPtr attr, string url);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetFlags_(System.IntPtr attr, long flags);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetParagraphSpacingAfter_(System.IntPtr attr, int spacing);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetParagraphSpacingBefore_(System.IntPtr attr, int spacing);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetLineSpacing_(System.IntPtr attr, int spacing);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBulletStyle_(System.IntPtr attr, int style);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBulletNumber_(System.IntPtr attr, int n);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBulletText_(System.IntPtr attr, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetPageBreak_(System.IntPtr attr, bool pageBreak);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetCharacterStyleName_(System.IntPtr attr, string name);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetParagraphStyleName_(System.IntPtr attr, string name);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetListStyleName_(System.IntPtr attr, string name);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBulletFont_(System.IntPtr attr, string bulletFont);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetBulletName_(System.IntPtr attr, string name);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetTextEffects_(System.IntPtr attr, int effects);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetTextEffectFlags_(System.IntPtr attr, int effects);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBoxTextAttr_SetOutlineLevel_(System.IntPtr attr, int level);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBoxTextAttr_GetFlags_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetFontSize_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetFontStyle_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetFontWeight_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBoxTextAttr_GetFontUnderlined_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetUnderlineType_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern NativeApiTypes.Color TextBoxTextAttr_GetUnderlineColor_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBoxTextAttr_GetFontStrikethrough_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBoxTextAttr_GetFontFaceName_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetFontFamily_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetParagraphSpacingAfter_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetParagraphSpacingBefore_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetLineSpacing_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetBulletStyle_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetBulletNumber_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBoxTextAttr_GetBulletText_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBoxTextAttr_GetURL_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetTextEffects_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetTextEffectFlags_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBoxTextAttr_GetOutlineLevel_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBoxTextAttr_IsCharacterStyle_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBoxTextAttr_IsParagraphStyle_(System.IntPtr attr);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBoxTextAttr_IsDefault_(IntPtr obj, System.IntPtr attr);
            
        }
    }
}
