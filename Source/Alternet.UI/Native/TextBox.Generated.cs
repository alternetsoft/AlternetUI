// <auto-generated> DO NOT MODIFY MANUALLY. Copyright (c) 2025 AlterNET Software.</auto-generated>
#nullable enable
#pragma warning disable

using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Security;
namespace Alternet.UI.Native
{
    internal partial class TextBox : Control
    {
        static TextBox()
        {
            SetEventCallback();
        }
        
        public TextBox()
        {
            SetNativePointer(NativeApi.TextBox_Create_());
        }
        
        public TextBox(IntPtr nativePointer) : base(nativePointer)
        {
        }
        
        public int TextWrap
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetTextWrap_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetTextWrap_(NativePointer, value);
            }
        }
        
        public int TextAlign
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetTextAlign_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetTextAlign_(NativePointer, value);
            }
        }
        
        public string ReportedUrl
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetReportedUrl_(NativePointer);
            }
            
        }
        
        public bool EditControlOnly
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetEditControlOnly_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetEditControlOnly_(NativePointer, value);
            }
        }
        
        public bool ReadOnly
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetReadOnly_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetReadOnly_(NativePointer, value);
            }
        }
        
        public System.IntPtr Validator
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetValidator_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetValidator_(NativePointer, value);
            }
        }
        
        public bool Multiline
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetMultiline_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetMultiline_(NativePointer, value);
            }
        }
        
        public bool IsRichEdit
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetIsRichEdit_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetIsRichEdit_(NativePointer, value);
            }
        }
        
        public bool HasSelection
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetHasSelection_(NativePointer);
            }
            
        }
        
        public bool IsModified
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetIsModified_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetIsModified_(NativePointer, value);
            }
        }
        
        public bool CanCopy
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetCanCopy_(NativePointer);
            }
            
        }
        
        public bool CanCut
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetCanCut_(NativePointer);
            }
            
        }
        
        public bool CanPaste
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetCanPaste_(NativePointer);
            }
            
        }
        
        public bool CanRedo
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetCanRedo_(NativePointer);
            }
            
        }
        
        public bool CanUndo
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetCanUndo_(NativePointer);
            }
            
        }
        
        public bool IsEmpty
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetIsEmpty_(NativePointer);
            }
            
        }
        
        public string EmptyTextHint
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetEmptyTextHint_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetEmptyTextHint_(NativePointer, value);
            }
        }
        
        public bool HideSelection
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetHideSelection_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetHideSelection_(NativePointer, value);
            }
        }
        
        public bool ProcessTab
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetProcessTab_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetProcessTab_(NativePointer, value);
            }
        }
        
        public bool ProcessEnter
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetProcessEnter_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetProcessEnter_(NativePointer, value);
            }
        }
        
        public bool IsPassword
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetIsPassword_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetIsPassword_(NativePointer, value);
            }
        }
        
        public bool AutoUrl
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetAutoUrl_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetAutoUrl_(NativePointer, value);
            }
        }
        
        public bool HideVertScrollbar
        {
            get
            {
                CheckDisposed();
                return NativeApi.TextBox_GetHideVertScrollbar_(NativePointer);
            }
            
            set
            {
                CheckDisposed();
                NativeApi.TextBox_SetHideVertScrollbar_(NativePointer, value);
            }
        }
        
        public static System.IntPtr CreateTextBox(System.IntPtr validator)
        {
            return NativeApi.TextBox_CreateTextBox_(validator);
        }
        
        public int GetLineLength(long lineNo)
        {
            CheckDisposed();
            return NativeApi.TextBox_GetLineLength_(NativePointer, lineNo);
        }
        
        public string GetLineText(long lineNo)
        {
            CheckDisposed();
            return NativeApi.TextBox_GetLineText_(NativePointer, lineNo);
        }
        
        public int GetNumberOfLines()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetNumberOfLines_(NativePointer);
        }
        
        public Alternet.Drawing.PointI PositionToXY(long pos)
        {
            CheckDisposed();
            return NativeApi.TextBox_PositionToXY_(NativePointer, pos);
        }
        
        public Alternet.Drawing.PointD PositionToCoords(long pos)
        {
            CheckDisposed();
            return NativeApi.TextBox_PositionToCoords_(NativePointer, pos);
        }
        
        public void ShowPosition(long pos)
        {
            CheckDisposed();
            NativeApi.TextBox_ShowPosition_(NativePointer, pos);
        }
        
        public long XYToPosition(long x, long y)
        {
            CheckDisposed();
            return NativeApi.TextBox_XYToPosition_(NativePointer, x, y);
        }
        
        public System.IntPtr GetDefaultStyle()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetDefaultStyle_(NativePointer);
        }
        
        public System.IntPtr GetStyle(long position)
        {
            CheckDisposed();
            return NativeApi.TextBox_GetStyle_(NativePointer, position);
        }
        
        public bool SetDefaultStyle(System.IntPtr style)
        {
            CheckDisposed();
            return NativeApi.TextBox_SetDefaultStyle_(NativePointer, style);
        }
        
        public bool SetStyle(long start, long end, System.IntPtr style)
        {
            CheckDisposed();
            return NativeApi.TextBox_SetStyle_(NativePointer, start, end, style);
        }
        
        public void Clear()
        {
            CheckDisposed();
            NativeApi.TextBox_Clear_(NativePointer);
        }
        
        public void Copy()
        {
            CheckDisposed();
            NativeApi.TextBox_Copy_(NativePointer);
        }
        
        public void Cut()
        {
            CheckDisposed();
            NativeApi.TextBox_Cut_(NativePointer);
        }
        
        public void AppendText(string text)
        {
            CheckDisposed();
            NativeApi.TextBox_AppendText_(NativePointer, text);
        }
        
        public long GetInsertionPoint()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetInsertionPoint_(NativePointer);
        }
        
        public void Paste()
        {
            CheckDisposed();
            NativeApi.TextBox_Paste_(NativePointer);
        }
        
        public void Redo()
        {
            CheckDisposed();
            NativeApi.TextBox_Redo_(NativePointer);
        }
        
        public void Remove(long from, long to)
        {
            CheckDisposed();
            NativeApi.TextBox_Remove_(NativePointer, from, to);
        }
        
        public void Replace(long from, long to, string value)
        {
            CheckDisposed();
            NativeApi.TextBox_Replace_(NativePointer, from, to, value);
        }
        
        public void SetInsertionPoint(long pos)
        {
            CheckDisposed();
            NativeApi.TextBox_SetInsertionPoint_(NativePointer, pos);
        }
        
        public void SetInsertionPointEnd()
        {
            CheckDisposed();
            NativeApi.TextBox_SetInsertionPointEnd_(NativePointer);
        }
        
        public void SetMaxLength(ulong len)
        {
            CheckDisposed();
            NativeApi.TextBox_SetMaxLength_(NativePointer, len);
        }
        
        public void SetSelection(long from, long to)
        {
            CheckDisposed();
            NativeApi.TextBox_SetSelection_(NativePointer, from, to);
        }
        
        public void SelectAll()
        {
            CheckDisposed();
            NativeApi.TextBox_SelectAll_(NativePointer);
        }
        
        public void SelectNone()
        {
            CheckDisposed();
            NativeApi.TextBox_SelectNone_(NativePointer);
        }
        
        public void Undo()
        {
            CheckDisposed();
            NativeApi.TextBox_Undo_(NativePointer);
        }
        
        public void WriteText(string text)
        {
            CheckDisposed();
            NativeApi.TextBox_WriteText_(NativePointer, text);
        }
        
        public string GetRange(long from, long to)
        {
            CheckDisposed();
            return NativeApi.TextBox_GetRange_(NativePointer, from, to);
        }
        
        public string GetStringSelection()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetStringSelection_(NativePointer);
        }
        
        public void EmptyUndoBuffer()
        {
            CheckDisposed();
            NativeApi.TextBox_EmptyUndoBuffer_(NativePointer);
        }
        
        public bool IsValidPosition(long pos)
        {
            CheckDisposed();
            return NativeApi.TextBox_IsValidPosition_(NativePointer, pos);
        }
        
        public long GetLastPosition()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetLastPosition_(NativePointer);
        }
        
        public long GetSelectionStart()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetSelectionStart_(NativePointer);
        }
        
        public long GetSelectionEnd()
        {
            CheckDisposed();
            return NativeApi.TextBox_GetSelectionEnd_(NativePointer);
        }
        
        static GCHandle eventCallbackGCHandle;
        public static TextBox? GlobalObject;
        
        static void SetEventCallback()
        {
            if (!eventCallbackGCHandle.IsAllocated)
            {
                var sink = new NativeApi.TextBoxEventCallbackType((obj, e, parameter) =>
                    UI.Application.HandleThreadExceptions(() =>
                    {
                        var w = NativeObject.GetFromNativePointer<TextBox>(obj, p => new TextBox(p));
                        w ??= GlobalObject;
                        if (w == null) return IntPtr.Zero;
                        return w.OnEvent(e, parameter);
                    }
                    )
                );
                eventCallbackGCHandle = GCHandle.Alloc(sink);
                NativeApi.TextBox_SetEventCallback_(sink);
            }
        }
        
        IntPtr OnEvent(NativeApi.TextBoxEvent e, IntPtr parameter)
        {
            switch (e)
            {
                case NativeApi.TextBoxEvent.TextEnter:
                {
                    TextEnter?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.TextBoxEvent.TextUrl:
                {
                    TextUrl?.Invoke(); return IntPtr.Zero;
                }
                case NativeApi.TextBoxEvent.TextMaxLength:
                {
                    TextMaxLength?.Invoke(); return IntPtr.Zero;
                }
                default: throw new Exception("Unexpected TextBoxEvent value: " + e);
            }
        }
        
        public Action? TextEnter;
        public Action? TextUrl;
        public Action? TextMaxLength;
        
        [SuppressUnmanagedCodeSecurity]
        public class NativeApi : NativeApiProvider
        {
            static NativeApi() => Initialize();
            
            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            public delegate IntPtr TextBoxEventCallbackType(IntPtr obj, TextBoxEvent e, IntPtr param);
            
            public enum TextBoxEvent
            {
                TextEnter,
                TextUrl,
                TextMaxLength,
            }
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEventCallback_(TextBoxEventCallbackType callback);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr TextBox_Create_();
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBox_GetTextWrap_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetTextWrap_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBox_GetTextAlign_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetTextAlign_(IntPtr obj, int value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetReportedUrl_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetEditControlOnly_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEditControlOnly_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetReadOnly_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetReadOnly_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TextBox_GetValidator_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetValidator_(IntPtr obj, System.IntPtr value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetMultiline_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetMultiline_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetIsRichEdit_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetIsRichEdit_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetHasSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetIsModified_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetIsModified_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetCanCopy_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetCanCut_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetCanPaste_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetCanRedo_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetCanUndo_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetIsEmpty_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetEmptyTextHint_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetEmptyTextHint_(IntPtr obj, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetHideSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetHideSelection_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetProcessTab_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetProcessTab_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetProcessEnter_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetProcessEnter_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetIsPassword_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetIsPassword_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetAutoUrl_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetAutoUrl_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_GetHideVertScrollbar_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetHideVertScrollbar_(IntPtr obj, bool value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TextBox_CreateTextBox_(System.IntPtr validator);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBox_GetLineLength_(IntPtr obj, long lineNo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetLineText_(IntPtr obj, long lineNo);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern int TextBox_GetNumberOfLines_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.PointI TextBox_PositionToXY_(IntPtr obj, long pos);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern Alternet.Drawing.PointD TextBox_PositionToCoords_(IntPtr obj, long pos);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_ShowPosition_(IntPtr obj, long pos);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBox_XYToPosition_(IntPtr obj, long x, long y);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TextBox_GetDefaultStyle_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern System.IntPtr TextBox_GetStyle_(IntPtr obj, long position);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_SetDefaultStyle_(IntPtr obj, System.IntPtr style);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_SetStyle_(IntPtr obj, long start, long end, System.IntPtr style);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Clear_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Copy_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Cut_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_AppendText_(IntPtr obj, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBox_GetInsertionPoint_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Paste_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Redo_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Remove_(IntPtr obj, long from, long to);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Replace_(IntPtr obj, long from, long to, string value);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetInsertionPoint_(IntPtr obj, long pos);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetInsertionPointEnd_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetMaxLength_(IntPtr obj, ulong len);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SetSelection_(IntPtr obj, long from, long to);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SelectAll_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_SelectNone_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_Undo_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_WriteText_(IntPtr obj, string text);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetRange_(IntPtr obj, long from, long to);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern string TextBox_GetStringSelection_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern void TextBox_EmptyUndoBuffer_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool TextBox_IsValidPosition_(IntPtr obj, long pos);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBox_GetLastPosition_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBox_GetSelectionStart_(IntPtr obj);
            
            [DllImport(NativeModuleName, CallingConvention = CallingConvention.Cdecl)]
            public static extern long TextBox_GetSelectionEnd_(IntPtr obj);
            
        }
    }
}
