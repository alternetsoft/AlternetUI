using System;
using System.Runtime.InteropServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class NativeTextBoxHandler : NativeControlHandler<TextBox, Native.TextBox>
    {
        private bool handlingNativeControlTextChanged;

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

        public GenericAlignment TextAlign
        {
            get
            {
                return (GenericAlignment)NativeControl.TextAlign;
            }

            set
            {
                NativeControl.TextAlign = (int)value;
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

        public PointD PositionToCoords(long pos)
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
            NativeControl.SelectAll();
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
            return new NativeTextBox(Control)
            {
                Text = Control.Text,
                EditControlOnly = !Control.HasBorder,
            };
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            Control.TextChanged -= Control_TextChanged;
            NativeControl.TextChanged -= NativeControl_TextChanged;
            Control.HasBorderChanged -= Control_HasBorderChanged;
            Control.MultilineChanged -= Control_MultilineChanged;
            Control.ReadOnlyChanged -= Control_ReadOnlyChanged;
            NativeControl.TextEnter -= NativeControl_TextEnter;
            NativeControl.TextUrl -= NativeControl_TextUrl;
            NativeControl.TextMaxLength -= NativeControl_TextMaxLength;
        }

        protected override void OnAttach()
        {
            base.OnAttach();

            ApplyMultiline();
            ApplyReadOnly();
            NativeControl.Text = Control.Text;

            Control.HasBorderChanged += Control_HasBorderChanged;
            Control.TextChanged += Control_TextChanged;
            Control.MultilineChanged += Control_MultilineChanged;
            Control.ReadOnlyChanged += Control_ReadOnlyChanged;
            NativeControl.TextChanged += NativeControl_TextChanged;
            NativeControl.TextEnter += NativeControl_TextEnter;
            NativeControl.TextUrl += NativeControl_TextUrl;
            NativeControl.TextMaxLength += NativeControl_TextMaxLength;
        }

        private void NativeControl_TextMaxLength(object? sender, EventArgs e)
        {
            Control.OnTextMaxLength(e);
        }

        private void NativeControl_TextUrl(object? sender, EventArgs e)
        {
            var url = ReportedUrl;
            Control.OnTextUrl(new UrlEventArgs(url));
        }

        private void NativeControl_TextEnter(object? sender, EventArgs e)
        {
            Control.OnEnterPressed(e);
        }

        private void Control_ReadOnlyChanged(object? sender, EventArgs e)
        {
            ApplyReadOnly();
        }

        private void ApplyReadOnly()
        {
            NativeControl.ReadOnly = Control.ReadOnly;
        }

        private void ApplyMultiline()
        {
            NativeControl.Multiline = Control.Multiline;
        }

        private void Control_MultilineChanged(object? sender, EventArgs e)
        {
            ApplyMultiline();
        }

        private void Control_HasBorderChanged(object? sender, EventArgs e)
        {
            NativeControl.EditControlOnly = !Control.HasBorder;
        }

        private void NativeControl_TextChanged(object? sender, EventArgs e)
        {
            handlingNativeControlTextChanged = true;
            try
            {
                Control.Text = NativeControl.Text!;
            }
            finally
            {
                handlingNativeControlTextChanged = false;
            }
        }

        private void Control_TextChanged(object? sender, EventArgs e)
        {
            if (!handlingNativeControlTextChanged)
            {
                if (NativeControl.Text != Control.Text)
                    NativeControl.Text = Control.Text;
            }
        }

        internal class NativeTextBox : Native.TextBox
        {
            public NativeTextBox(TextBox control)
                : base()
            {
                var validator = control.Validator;
                IntPtr ptr = default;
                if (validator != null)
                    ptr = validator.Handle;

                SetNativePointer(NativeApi.TextBox_CreateTextBox_(ptr));
            }
        }
    }
}