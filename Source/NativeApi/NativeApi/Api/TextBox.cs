#pragma warning disable
using ApiCommon;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class TextBox : Control
    {
        public static IntPtr CreateTextBox(IntPtr validator) => default;

        public event EventHandler? TextChanged;
        public event EventHandler? TextEnter;
        public event EventHandler? TextUrl;
        public event EventHandler? TextMaxLength;

        public string Text { get; set; }

        public string ReportedUrl { get; }

        public bool EditControlOnly { get; set; }

        public bool ReadOnly { get; set; }

        public bool Multiline { get; set; }

        public bool IsRichEdit { get; set; }

        public bool HasSelection { get; }

        public int GetLineLength(long lineNo) => throw new Exception();

        public string GetLineText(long lineNo) => throw new Exception();

        public int GetNumberOfLines() => throw new Exception();

        public Point PositionToXY(long pos) => throw new Exception();

        public Point PositionToCoords(long pos) => throw new Exception();

        public void ShowPosition(long pos) => throw new Exception();

        public long XYToPosition (long x, long y) => throw new Exception();

        public IntPtr GetDefaultStyle() => throw new Exception();

        public IntPtr GetStyle(long position) => throw new Exception();

        public bool SetDefaultStyle(IntPtr style) => throw new Exception();

        public bool SetStyle(long start, long end, IntPtr style) => 
            throw new Exception();

        public bool IsModified { get; set; }
        public bool CanCopy { get; }
        public bool CanCut { get; }
        public bool CanPaste { get; }
        public bool CanRedo { get; }
        public bool CanUndo { get; }
        public bool IsEmpty { get; }
        public void Clear() => throw new Exception();
        public void Copy() => throw new Exception();
        public void Cut() => throw new Exception();

        public void AppendText(string text) => throw new Exception();

        public long GetInsertionPoint() => throw new Exception();

        public void Paste() => throw new Exception();
        public void Redo() => throw new Exception();

        public void Remove(long from, long to) => throw new Exception();

        public void Replace(long from, long to, string value) => throw new Exception();

        public void SetInsertionPoint(long pos) => throw new Exception();

        public void SetInsertionPointEnd() => throw new Exception();

        public void SetMaxLength(ulong len) => throw new Exception();

        public void SetSelection(long from, long to) => throw new Exception();

        public void SelectAll() => throw new Exception();
        public void SelectNone() => throw new Exception();

        public string EmptyTextHint { get; set; }

        public void Undo() => throw new Exception();

        public void WriteText(string text) => throw new Exception();

        public string GetRange(long from, long to) => throw new Exception();

        public string GetStringSelection() => throw new Exception();

        public void EmptyUndoBuffer() => throw new Exception();

        public bool IsValidPosition(long pos) => throw new Exception();

        public long GetLastPosition() => throw new Exception();

        public long GetSelectionStart() => throw new Exception();
        public long GetSelectionEnd() => throw new Exception();
        
        public bool HideSelection { get; set; }

        public bool ProcessTab { get; set; }

        public bool ProcessEnter { get; set; }

        public bool IsPassword { get; set; }

        public bool AutoUrl { get; set; }

        public bool HideVertScrollbar { get; set; }
    }
}