using System;
using System.Runtime.InteropServices;

using Alternet.Drawing;
using Alternet.UI.Extensions;

namespace Alternet.UI
{
    internal class TextBoxHandler
        : WxControlHandler<TextBox, Native.TextBox>, ITextBoxHandler, IWxTextBoxHandler
    {
        public bool ReadOnly
        {
            get
            {
                return NativeControl.ReadOnly;
            }

            set
            {
                NativeControl.ReadOnly = value;
            }
        }

        public override bool HasBorder
        {
            get
            {
                return !NativeControl.EditControlOnly;
            }

            set
            {
                NativeControl.EditControlOnly = !value;
            }
        }

        public bool Multiline
        {
            get
            {
                return NativeControl.Multiline;
            }

            set
            {
                NativeControl.Multiline = value;
            }
        }

        public string ReportedUrl
        {
            get
            {
                return NativeControl.ReportedUrl;
            }
        }

        public bool HideSelection
        {
            get
            {
                return NativeControl.HideSelection;
            }

            set
            {
                NativeControl.HideSelection = value;
            }
        }

        public bool ProcessTab
        {
            get
            {
                return NativeControl.ProcessTab;
            }

            set
            {
                NativeControl.ProcessTab = value;
            }
        }

        public bool ProcessEnter
        {
            get
            {
                return NativeControl.ProcessEnter;
            }

            set
            {
                NativeControl.ProcessEnter = value;
            }
        }

        public bool IsPassword
        {
            get
            {
                return NativeControl.IsPassword;
            }

            set
            {
                NativeControl.IsPassword = value;
            }
        }

        public bool AutoUrl
        {
            get
            {
                return NativeControl.AutoUrl;
            }

            set
            {
                NativeControl.AutoUrl = value;
            }
        }

        public bool HideVertScrollbar
        {
            get
            {
                return NativeControl.HideVertScrollbar;
            }

            set
            {
                NativeControl.HideVertScrollbar = value;
            }
        }

        public bool HasSelection
        {
            get
            {
                return NativeControl.HasSelection;
            }
        }

        public bool IsModified
        {
            get
            {
                return NativeControl.IsModified;
            }

            set
            {
                NativeControl.IsModified = value;
            }
        }

        public bool CanCopy
        {
            get
            {
                return NativeControl.CanCopy;
            }
        }

        public bool CanCut
        {
            get
            {
                return NativeControl.CanCut;
            }
        }

        public bool CanPaste
        {
            get
            {
                return NativeControl.CanPaste;
            }
        }

        public bool CanRedo
        {
            get
            {
                return NativeControl.CanRedo;
            }
        }

        public bool CanUndo
        {
            get
            {
                return NativeControl.CanUndo;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return NativeControl.IsEmpty;
            }
        }

        public string EmptyTextHint
        {
            get
            {
                return NativeControl.EmptyTextHint;
            }

            set
            {
                NativeControl.EmptyTextHint = value;
            }
        }

        public TextBoxTextWrap TextWrap
        {
            get
            {
                return (TextBoxTextWrap)NativeControl.TextWrap;
            }

            set
            {
                NativeControl.TextWrap = (int)value;
            }
        }

        public TextHorizontalAlignment TextAlign
        {
            get
            {
                return ((GenericAlignment)NativeControl.TextAlign).AsTextHorizontalAlignment();
            }

            set
            {
                NativeControl.TextAlign = (int)(value.AsGenericAlignment());
            }
        }

        public bool IsRichEdit
        {
            get
            {
                return NativeControl.IsRichEdit;
            }

            set
            {
                NativeControl.IsRichEdit = value;
            }
        }

        public int GetLineLength(long lineNo)
        {
            return NativeControl.GetLineLength(lineNo);
        }

        public string GetLineText(long lineNo)
        {
            return NativeControl.GetLineText(lineNo);
        }

        public int GetNumberOfLines()
        {
            return NativeControl.GetNumberOfLines();
        }

        public PointI PositionToXY(long pos)
        {
            return NativeControl.PositionToXY(pos);
        }

        public PointD PositionToCoord(long pos)
        {
            return NativeControl.PositionToCoords(pos);
        }

        public void ShowPosition(long pos)
        {
            NativeControl.ShowPosition(pos);
        }

        public long XYToPosition(long x, long y)
        {
            return NativeControl.XYToPosition(x, y);
        }

        public System.IntPtr GetDefaultStyle()
        {
            return NativeControl.GetDefaultStyle();
        }

        public IntPtr GetStyle(long position)
        {
            return NativeControl.GetStyle(position);
        }

        public bool SetDefaultStyle(System.IntPtr style)
        {
            return NativeControl.SetDefaultStyle(style);
        }

        public bool SetStyle(long start, long end, System.IntPtr style)
        {
            return NativeControl.SetStyle(start, end, style);
        }

        public void Clear()
        {
            NativeControl.Clear();
        }

        public void Copy()
        {
            NativeControl.Copy();
        }

        public void Cut()
        {
            NativeControl.Cut();
        }

        public void AppendText(string text)
        {
            NativeControl.AppendText(text);
        }

        public long GetInsertionPoint()
        {
            return NativeControl.GetInsertionPoint();
        }

        public void Paste()
        {
            NativeControl.Paste();
        }

        public void Redo()
        {
            NativeControl.Redo();
        }

        public void Remove(long from, long to)
        {
            NativeControl.Remove(from, to);
        }

        public void Replace(long from, long to, string value)
        {
            NativeControl.Replace(from, to, value);
        }

        public void SetInsertionPoint(long pos)
        {
            NativeControl.SetInsertionPoint(pos);
        }

        public void SetInsertionPointEnd()
        {
            NativeControl.SetInsertionPointEnd();
        }

        public ITextBoxTextAttr CreateTextAttr()
        {
            return new TextBoxTextAttr();
        }

        ITextBoxTextAttr IWxTextBoxHandler.GetDefaultStyle()
        {
            return new TextBoxTextAttr(NativeControl.GetDefaultStyle());
        }

        bool IWxTextBoxHandler.SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return SetStyle(start, end, s.Handle);
        }

        bool IWxTextBoxHandler.SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return SetDefaultStyle(s.Handle);
        }

        ITextBoxTextAttr IWxTextBoxHandler.GetStyle(long pos)
        {
            return new TextBoxTextAttr(NativeControl.GetStyle(pos));
        }

        public void SetMaxLength(ulong len)
        {
            NativeControl.SetMaxLength(len);
        }

        public void SetSelection(long from, long to)
        {
            NativeControl.SetSelection(from, to);
        }

        public void SelectAll()
        {
            if (IsRichEdit)
            {
                BeginUpdate();
                try
                {
                    SetSelection(-1, -1);
                }
                finally
                {
                    EndUpdate();
                }
            }
            else
            {
                NativeControl.SelectAll();
            }
        }

        public void SelectNone()
        {
            NativeControl.SelectNone();
        }

        public void Undo()
        {
            NativeControl.Undo();
        }

        public void WriteText(string text)
        {
            NativeControl.WriteText(text);
        }

        public string GetRange(long from, long to)
        {
            return NativeControl.GetRange(from, to);
        }

        public string GetStringSelection()
        {
            return NativeControl.GetStringSelection();
        }

        public void EmptyUndoBuffer()
        {
            NativeControl.EmptyUndoBuffer();
        }

        public bool IsValidPosition(long pos)
        {
            return NativeControl.IsValidPosition(pos);
        }

        public long GetLastPosition()
        {
            return NativeControl.GetLastPosition();
        }

        public long GetSelectionStart()
        {
            return NativeControl.GetSelectionStart();
        }

        public long GetSelectionEnd()
        {
            return NativeControl.GetSelectionEnd();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new NativeTextBox(Control);
        }

        public override void OnSystemColorsChanged()
        {
            base.OnSystemColorsChanged();

            if (App.IsWindowsOS)
                RecreateWindow();
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            if (App.IsWindowsOS)
                UserPaint = true;
        }

        internal class NativeTextBox : Native.TextBox
        {
            public NativeTextBox(TextBox? control)
            {
                IntPtr ptr = default;
                SetNativePointer(NativeApi.TextBox_CreateTextBox_(ptr));
            }
        }
    }
}