using System;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class RichTextBoxHandler : WxControlHandler, IRichTextBoxHandler
    {
        public RichTextBoxHandler()
        {
        }

        public new Native.RichTextBox NativeControl => (Native.RichTextBox)base.NativeControl!;

        public new RichTextBox Control => (RichTextBox)base.Control;

        public string ReportedUrl
        {
            get
            {
                return NativeControl.ReportedUrl;
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

        public void ShowPosition(long pos)
        {
            NativeControl.ShowPosition(pos);
        }

        public long XYToPosition(long x, long y)
        {
            return NativeControl.XYToPosition(x, y);
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

        public bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return NativeControl.SetStyle(start, end, s.Handle);
        }

        public bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return NativeControl.SetDefaultStyle(s.Handle);
        }

        public ITextBoxTextAttr GetStyle(long pos)
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

        public long GetLastPosition()
        {
            return NativeControl.GetLastPosition();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.RichTextBox();
        }

        protected override void OnAttach()
        {
            base.OnAttach();
            NativeControl.TextChanged = NativeControl_TextChanged;
            NativeControl.TextEnter = NativeControl_TextEnter;
            NativeControl.TextUrl = NativeControl_TextUrl;
        }

        protected override void OnDetach()
        {
            base.OnDetach();
            NativeControl.TextChanged = null;
            NativeControl.TextEnter = null;
            NativeControl.TextUrl = null;
        }

        private void NativeControl_TextUrl()
        {
            var url = NativeControl.ReportedUrl;
            Control.OnTextUrl(new UrlEventArgs(url));
        }

        private void NativeControl_TextEnter()
        {
            Control.OnEnterPressed(EventArgs.Empty);
        }

        private void NativeControl_TextChanged()
        {
            Control.RaiseTextChanged(EventArgs.Empty);
        }
    }
}