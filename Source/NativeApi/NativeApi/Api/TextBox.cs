#pragma warning disable
using ApiCommon;
using System;
using Alternet.Drawing;

namespace NativeApi.Api
{
    public class TextBox : Control
    {
        public event EventHandler? TextChanged { 
            add => throw new Exception(); remove => throw new Exception(); }

        public string Text { 
            get => throw new Exception(); set => throw new Exception(); }

        public bool EditControlOnly { get; set; }

        public bool ReadOnly { get; set; }

        public bool Multiline { get; set; }

        public bool IsRichEdit { get; set; }

        //Gets the length of the specified line, not including any trailing
        //newline character(s).
        public int GetLineLength(long lineNo) => throw new Exception();

        //Returns the contents of a given line in the text control, not
        //including any trailing newline character(s).
        public string GetLineText(long lineNo) => throw new Exception();

        //Returns the number of lines in the text control buffer.
        public int GetNumberOfLines() => throw new Exception();

        //Converts given position to a zero-based column, line number pair.
        public Point PositionToXY(long pos) => throw new Exception();

        //Converts given text position to client coordinates in pixels.
        public Point PositionToCoords(long pos) => throw new Exception();

        //Makes the line containing the given position visible.
        public void ShowPosition(long pos) => throw new Exception();

        //Converts the given zero based column and line number to a position.
        public long XYToPosition (long x, long y) => throw new Exception();

        //Returns the style currently used for the new text.
        public IntPtr GetDefaultStyle() => throw new Exception();

        //Returns the style at this position in the text control.
        public bool GetStyle(long position, IntPtr style) => throw new Exception();

        //Changes the default style to use for the new text which is
        //going to be added to the control using WriteText() or AppendText().
        public bool SetDefaultStyle(IntPtr style) => throw new Exception();

        //Changes the style of the given range.
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

        //Appends the text to the end of the text control.
        public void AppendText(string text) => throw new Exception();

        //Returns the insertion point, or cursor, position.
        public long GetInsertionPoint() => throw new Exception();

        public void Paste() => throw new Exception();
        public void Redo() => throw new Exception();

        // Removes the text starting at the first given position
        // up to (but not including) the character at the last position.
        public void Remove(long from, long to) => throw new Exception();

        // Replaces the text starting at the first position up
        // to (but not including) the character at the last position
        // with the given text.
        public void Replace(long from, long to, string value) => throw new Exception();

        // Sets the insertion point at the given position.
        public void SetInsertionPoint(long pos) => throw new Exception();

        // Sets the insertion point at the end of the text control.
        public void SetInsertionPointEnd() => throw new Exception();

        // This function sets the maximum number of characters 
        // the user can enter into the control.
        public void SetMaxLength(ulong len) => throw new Exception();

        // Selects the text starting at the first position up to
        // (but not including) the character at the last position.
        public void SetSelection(long from, long to) => throw new Exception();

        public void SelectAll() => throw new Exception();
        public void SelectNone() => throw new Exception();

        // Gets or sets a hint shown in an empty unfocused text control. 
        public string EmptyTextHint { get; set; }

        public void Undo() => throw new Exception();

        //Writes the text into the text control at the current insertion position.  
        public void WriteText(string text) => throw new Exception();

        // Returns the string containing the text starting in the
        // positions from and up to to in the control. 
        public string GetRange(long from, long to) => throw new Exception();

        //Gets the text currently selected in the control.
        public string GetStringSelection() => throw new Exception();

        public void EmptyUndoBuffer() => throw new Exception();

        // Return true if the given position is valid, i.e. positive and less than
        // the last position.
        public bool IsValidPosition(long pos) => throw new Exception();

        // Returns the zero based index of the last position in
        //the text control, which is equal to the number of characters in the control. 
        public long GetLastPosition() => throw new Exception();

        //Gets the current selection span.  
        public long GetSelectionStart() => throw new Exception();
        public long GetSelectionEnd() => throw new Exception();

    }
}