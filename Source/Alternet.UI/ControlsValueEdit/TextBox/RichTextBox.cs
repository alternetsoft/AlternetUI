using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements rich text editor functionality.
    /// </summary>
    public class RichTextBox : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.RichTextBox;

        [Browsable(false)]
        internal new NativeRichTextBoxHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeRichTextBoxHandler)base.Handler;
            }
        }

        internal Native.RichTextBox NativeControl => Handler.NativeControl;

        // Clears the cache of available font names.
        public static void ClearAvailableFontNames()
        {
            Native.RichTextBox.ClearAvailableFontNames();
        }

        public string GetRange(long from, long to)
        {
            return NativeControl.GetRange(from, to);
        }

        // Returns the length of the specified line in characters.
        public int GetLineLength(long lineNo)
        {
            return NativeControl.GetLineLength(lineNo);
        }

        // Returns the text for the given line.
        public string GetLineText(long lineNo)
        {
            return NativeControl.GetLineText(lineNo);
        }

        // Returns the number of lines in the buffer.
        public int GetNumberOfLines()
        {
            return NativeControl.GetNumberOfLines();
        }

        // Returns <c>true</c> if the buffer has been modified.
        public bool IsModified()
        {
            return NativeControl.IsModified();
        }

        // Returns <c>true</c> if the control is editable.
        public bool IsEditable()
        {
            return NativeControl.IsEditable();
        }

        // Returns <c>true</c> if the control is single-line.
        // Currently wxRichTextCtrl does not support single-line editing.
        public bool IsSingleLine()
        {
            return NativeControl.IsSingleLine();
        }

        // Returns <c>true</c> if the control is multiline.
        public bool IsMultiLine()
        {
            return NativeControl.IsMultiLine();
        }

        // Returns the text within the current selection range, if any.
        public string GetStringSelection()
        {
            return NativeControl.GetStringSelection();
        }

        // Gets the current filename associated with the control.
        public string GetFilename()
        {
            return NativeControl.GetFilename();
        }

        // Sets the current filename.
        public void SetFilename(string filename)
        {
            NativeControl.SetFilename(filename);
        }

        // Sets the size of the buffer beyond which layout is delayed during resizing.
        // This optimizes sizing for large buffers. The default is 20000.
        public void SetDelayedLayoutThreshold(long threshold)
        {
            NativeControl.SetDelayedLayoutThreshold(threshold);
        }

        // Gets the size of the buffer beyond which layout is delayed during resizing.
        // This optimizes sizing for large buffers. The default is 20000.
        public long GetDelayedLayoutThreshold()
        {
            return NativeControl.GetDelayedLayoutThreshold();
        }

        // Gets the flag indicating that full layout is required.
        public bool GetFullLayoutRequired()
        {
            return NativeControl.GetFullLayoutRequired();
        }

        // Sets the flag indicating that full layout is required.
        public void SetFullLayoutRequired(bool b)
        {
            NativeControl.SetFullLayoutRequired(b);
        }

        // Returns the last time full layout was performed.
        public long GetFullLayoutTime()
        {
            return NativeControl.GetFullLayoutTime();
        }

        // Sets the last time full layout was performed.
        public void SetFullLayoutTime(long t)
        {
            NativeControl.SetFullLayoutTime(t);
        }

        // Returns the position that should be shown when full (delayed) layout is performed.
        public long GetFullLayoutSavedPosition()
        {
            return NativeControl.GetFullLayoutSavedPosition();
        }

        // Sets the position that should be shown when full (delayed) layout is performed.
        public void SetFullLayoutSavedPosition(long p)
        {
            NativeControl.SetFullLayoutSavedPosition(p);
        }

        // Forces any pending layout due to delayed, partial layout when the control was resized.
        public void ForceDelayedLayout()
        {
            NativeControl.ForceDelayedLayout();
        }

        // Returns <c>true</c> if we are showing the caret position at the start of a line
        // instead of at the end of the previous one.
        public bool GetCaretAtLineStart()
        {
            return NativeControl.GetCaretAtLineStart();
        }

        // Sets a flag to remember that we are showing the caret position at the start of a line
        // instead of at the end of the previous one.
        public void SetCaretAtLineStart(bool atStart)
        {
            NativeControl.SetCaretAtLineStart(atStart);
        }

        // Returns <c>true</c> if we are dragging a selection.
        public bool GetDragging()
        {
            return NativeControl.GetDragging();
        }

        // Sets a flag to remember if we are dragging a selection.
        public void SetDragging(bool dragging)
        {
            NativeControl.SetDragging(dragging);
        }

        // Returns an anchor so we know how to extend the selection.
        // It's a caret position since it's between two characters.
        public long GetSelectionAnchor()
        {
            return NativeControl.GetSelectionAnchor();
        }

        // Sets an anchor so we know how to extend the selection.
        // It's a caret position since it's between two characters.
        public void SetSelectionAnchor(long anchor)
        {
            NativeControl.SetSelectionAnchor(anchor);
        }

        // Clears the buffer content, leaving a single empty paragraph. Cannot be undone.
        public void Clear()
        {
            NativeControl.Clear();
        }

        // Replaces the content in the specified range with the string specified by @a value.
        public void Replace(long from, long to, string value)
        {
            NativeControl.Replace(from, to, value);
        }

        // Removes the content in the specified range.
        public void Remove(long from, long to)
        {
            NativeControl.Remove(from, to);
        }

        public bool LoadFile(string file, int type)
        {
            return NativeControl.LoadFile(file, type);
        }

        // Saves the buffer content using the given type.
        // If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        // the filename extension.
        public bool SaveFile(string file, int type)
        {
            return NativeControl.SaveFile(file, type);
        }

        // Sets flags that change the behaviour of loading or saving.
        // See the documentation for each handler class to see what flags are
        // relevant for each handler.
        public void SetFileHandlerFlags(int flags)
        {
            NativeControl.SetHandlerFlags(flags);
        }

        // Returns flags that change the behaviour of loading or saving.
        // See the documentation for each handler class to see what flags are
        // relevant for each handler.
        public int GetFileHandlerFlags()
        {
            return NativeControl.GetHandlerFlags();
        }

        // Marks the buffer as modified.
        public void MarkDirty()
        {
            NativeControl.MarkDirty();
        }

        // Sets the buffer's modified status to @false, and clears the buffer's command history.
        public void DiscardEdits()
        {
            NativeControl.DiscardEdits();
        }

        // Sets the maximum number of characters that may be entered in a single line
        // text control.For compatibility only; currently does nothing.
        public void SetMaxLength(ulong len)
        {
            NativeControl.SetMaxLength(len);
        }

        // Writes text at the current position.
        public void WriteText(string text)
        {
            NativeControl.WriteText(text);
        }

        // Sets the insertion point to the end of the buffer and writes the text.
        public void AppendText(string text)
        {
            NativeControl.AppendText(text);
        }

        // Translates from column and line number to position.
        public long XYToPosition(long x, long y)
        {
            return NativeControl.XYToPosition(x, y);
        }

        // Scrolls the buffer so that the given position is in view.
        public void ShowPosition(long pos)
        {
            NativeControl.ShowPosition(pos);
        }

        // Copies the selected content (if any) to the clipboard.
        public void Copy()
        {
            NativeControl.Copy();
        }

        // Copies the selected content (if any) to the clipboard and deletes the selection.
        // This is undoable.
        public void Cut()
        {
            NativeControl.Cut();
        }

        // Pastes content from the clipboard to the buffer.
        public void Paste()
        {
            NativeControl.Paste();
        }

        // Deletes the content in the selection, if any. This is undoable.
        public void DeleteSelection()
        {
            NativeControl.DeleteSelection();
        }

        // Returns <c>true</c> if selected content can be copied to the clipboard.
        public bool CanCopy()
        {
            return NativeControl.CanCopy();
        }

        // Returns <c>true</c> if selected content can be copied to the clipboard and deleted.
        public bool CanCut()
        {
            return NativeControl.CanCut();
        }

        // Returns <c>true</c> if the clipboard content can be pasted to the buffer.
        public bool CanPaste()
        {
            return NativeControl.CanPaste();
        }

        // Returns <c>true</c> if selected content can be deleted.
        public bool CanDeleteSelection()
        {
            return NativeControl.CanDeleteSelection();
        }

        // Undoes the command at the top of the command history, if there is one.
        public void Undo()
        {
            NativeControl.Undo();
        }

        // Redoes the current command.
        public void Redo()
        {
            NativeControl.Redo();
        }

        // Returns <c>true</c> if there is a command in the command history that can be undone.
        public bool CanUndo()
        {
            return NativeControl.CanUndo();
        }

        // Returns <c>true</c> if there is a command in the command history that can be redone.
        public bool CanRedo()
        {
            return NativeControl.CanRedo();
        }

        // Sets the insertion point and causes the current editing style to be taken from
        // the new position (unlike wxRichTextCtrl::SetCaretPosition).
        public void SetInsertionPoint(long pos)
        {
            NativeControl.SetInsertionPoint(pos);
        }

        // Sets the insertion point to the end of the text control.
        public void SetInsertionPointEnd()
        {
            NativeControl.SetInsertionPointEnd();
        }

        // Returns the current insertion point.
        public long GetInsertionPoint()
        {
            return NativeControl.GetInsertionPoint();
        }

        // Sets the selection to the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // So, for example, to set the selection for a character at position 5, use the
        // range (5,6).
        public void SetSelection(long from, long to)
        {
            NativeControl.SetSelection(from, to);
        }

        // Makes the control editable, or not.
        public void SetEditable(bool editable)
        {
            NativeControl.SetEditable(editable);
        }

        // Returns <c>true</c> if there is a selection and the object containing the selection
        // was the same as the current focus object.
        public bool HasSelection()
        {
            return NativeControl.HasSelection();
        }

        // Returns <c>true</c> if there was a selection, whether or not the current focus object
        // is the same as the selection's container object.
        public bool HasUnfocusedSelection()
        {
            return NativeControl.HasUnfocusedSelection();
        }

        // Inserts a new paragraph at the current insertion point. @see LineBreak().
        public bool Newline()
        {
            return NativeControl.Newline();
        }

        // Inserts a line break at the current insertion point.
        // A line break forces wrapping within a paragraph, and can be introduced by
        // using this function, by appending the wxChar value @b  wxRichTextLineBreakChar
        // to text content, or by typing Shift-Return.
        public bool LineBreak()
        {
            return NativeControl.LineBreak();
        }

        // Ends the current style.
        public bool EndStyle()
        {
            return NativeControl.EndStyle();
        }

        // Ends application of all styles in the current style stack.
        public bool EndAllStyles()
        {
            return NativeControl.EndAllStyles();
        }

        // Begins using bold.
        public bool BeginBold()
        {
            return NativeControl.BeginBold();
        }

        // Ends using bold.
        public bool EndBold()
        {
            return NativeControl.EndBold();
        }

        // Begins using italic.
        public bool BeginItalic()
        {
            return NativeControl.BeginItalic();
        }

        // Ends using italic.
        public bool EndItalic()
        {
            return NativeControl.EndItalic();
        }

        // Begins using underlining.
        public bool BeginUnderline()
        {
            return NativeControl.BeginUnderline();
        }

        // End applying underlining.
        public bool EndUnderline()
        {
            return NativeControl.EndUnderline();
        }

        // Begins using the given point size.
        public bool BeginFontSize(int pointSize)
        {
            return NativeControl.BeginFontSize(pointSize);
        }

        // Ends using a point size.
        public bool EndFontSize()
        {
            return NativeControl.EndFontSize();
        }

        // Ends using a font.
        public bool EndFont()
        {
            return NativeControl.EndFont();
        }

        // Begins using this color.
        public bool BeginTextColor(Color color)
        {
            return NativeControl.BeginTextColour(color);
        }

        // Ends applying a text color.
        public bool EndTextColor()
        {
            return NativeControl.EndTextColour();
        }

        // Begins using alignment.
        public bool BeginAlignment(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.BeginAlignment((int)alignment);
        }

        // Ends alignment.
        public bool EndAlignment()
        {
            return NativeControl.EndAlignment();
        }

        // Begins applying a left indent and subindent in tenths of a millimetre.
        // The subindent is an offset from the left edge of the paragraph, and is
        // used for all but the first line in a paragraph. A positive value will
        // cause the first line to appear to the left of the subsequent lines, and
        // a negative value will cause the first line to be indented to the right
        // of the subsequent lines.
        // wxRichTextBuffer uses indentation to render a bulleted item. The
        // content of the paragraph, including the first line, starts at the
        // @a leftIndent plus the @a leftSubIndent.
        // @param leftIndent
        // The distance between the margin and the bullet.
        // @param leftSubIndent
        // The distance between the left edge of the bullet and the left edge
        // of the actual paragraph.
        public bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0)
        {
            return NativeControl.BeginLeftIndent(leftIndent, leftSubIndent);
        }

        // Ends left indent.
        public bool EndLeftIndent()
        {
            return NativeControl.EndLeftIndent();
        }

        // Begins a right indent, specified in tenths of a millimetre.
        public bool BeginRightIndent(int rightIndent)
        {
            return NativeControl.BeginRightIndent(rightIndent);
        }

        // Ends right indent.
        public bool EndRightIndent()
        {
            return NativeControl.EndRightIndent();
        }

        // Begins paragraph spacing; pass the before-paragraph and after-paragraph spacing
        // in tenths of a millimetre.
        public bool BeginParagraphSpacing(int before, int after)
        {
            return NativeControl.BeginParagraphSpacing(before, after);
        }

        // Ends paragraph spacing.
        public bool EndParagraphSpacing()
        {
            return NativeControl.EndParagraphSpacing();
        }

        // Begins applying line spacing. @e spacing is a multiple, where 10 means
        // single-spacing, 15 means 1.5 spacing, and 20 means double spacing.
        // The ::wxTextAttrLineSpacing constants are defined for convenience.
        public bool BeginLineSpacing(int lineSpacing)
        {
            return NativeControl.BeginLineSpacing(lineSpacing);
        }

        // Ends line spacing.
        public bool EndLineSpacing()
        {
            return NativeControl.EndLineSpacing();
        }

        // Begins a numbered bullet.
        // This call will be needed for each item in the list, and the
        // application should take care of incrementing the numbering.
        // @a bulletNumber is a number, usually starting with 1.
        // @a leftIndent and @a leftSubIndent are values in tenths of a millimetre.
        // @a bulletStyle is a bitlist of the  ::wxTextAttrBulletStyle values.
        // wxRichTextBuffer uses indentation to render a bulleted item.
        // The left indent is the distance between the margin and the bullet.
        // The content of the paragraph, including the first line, starts
        // at leftMargin + leftSubIndent.
        // So the distance between the left edge of the bullet and the
        // left of the actual paragraph is leftSubIndent.
        public bool BeginNumberedBullet(
            int bulletNumber,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period)
        {
            return NativeControl.BeginNumberedBullet(
                bulletNumber,
                leftIndent,
                leftSubIndent,
                (int)bulletStyle);
        }

        // Ends application of a numbered bullet.
        public bool EndNumberedBullet()
        {
            return NativeControl.EndNumberedBullet();
        }

        // Begins applying a symbol bullet, using a character from the current font.
        // See BeginNumberedBullet() for an explanation of how indentation is used
        // to render the bulleted paragraph.
        public bool BeginSymbolBullet(
            string symbol,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Symbol)
        {
            return NativeControl.BeginSymbolBullet(
                symbol,
                leftIndent,
                leftSubIndent,
                (int)bulletStyle);
        }

        // Ends applying a symbol bullet.
        public bool EndSymbolBullet()
        {
            return NativeControl.EndSymbolBullet();
        }

        // Begins applying a symbol bullet.
        public bool BeginStandardBullet(
            string bulletName,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Standard)
        {
            return NativeControl.BeginStandardBullet(
                bulletName,
                leftIndent,
                leftSubIndent,
                (int)bulletStyle);
        }

        // Begins applying a standard bullet.
        public bool EndStandardBullet()
        {
            return NativeControl.EndStandardBullet();
        }

        // Begins using the named character style.
        public bool BeginCharacterStyle(string characterStyle)
        {
            return NativeControl.BeginCharacterStyle(characterStyle);
        }

        // Ends application of a named character style.
        public bool EndCharacterStyle()
        {
            return NativeControl.EndCharacterStyle();
        }

        // Begins applying the named paragraph style.
        public bool BeginParagraphStyle(string paragraphStyle)
        {
            return NativeControl.BeginParagraphStyle(paragraphStyle);
        }

        // Ends application of a named paragraph style.
        public bool EndParagraphStyle()
        {
            return NativeControl.EndParagraphStyle();
        }

        // Begins using a specified list style.
        // Optionally, you can also pass a level and a number.
        public bool BeginListStyle(string listStyle, int level = 1, int number = 1)
        {
            return NativeControl.BeginListStyle(listStyle, level, number);
        }

        // Ends using a specified list style.
        public bool EndListStyle()
        {
            return NativeControl.EndListStyle();
        }

        // Begins applying wxTEXT_ATTR_URL to the content.
        // Pass a URL and optionally, a character style to apply, since it is common
        // to mark a URL with a familiar style such as blue text with underlining.
        public bool BeginURL(string url, string? characterStyle = default)
        {
            characterStyle ??= string.Empty;
            return NativeControl.BeginURL(url, characterStyle);
        }

        // Ends applying a URL.
        public bool EndURL()
        {
            return NativeControl.EndURL();
        }

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is bold.
        public bool IsSelectionBold()
        {
            return NativeControl.IsSelectionBold();
        }

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is italic.
        public bool IsSelectionItalics()
        {
            return NativeControl.IsSelectionItalics();
        }

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is underlined.
        public bool IsSelectionUnderlined()
        {
            return NativeControl.IsSelectionUnderlined();
        }

        // Returns <c>true</c> if all of the selection, or the content
        // at the current caret position, has the supplied effects flag(s).
        public bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag)
        {
            return NativeControl.DoesSelectionHaveTextEffectFlag((int)flag);
        }

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is aligned according to the specified flag.
        public bool IsSelectionAligned(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.IsSelectionAligned((int)alignment);
        }

        // Apples bold to the selection or default style (undoable).
        public bool ApplyBoldToSelection()
        {
            return NativeControl.ApplyBoldToSelection();
        }

        // Applies italic to the selection or default style (undoable).
        public bool ApplyItalicToSelection()
        {
            return NativeControl.ApplyItalicToSelection();
        }

        // Applies underline to the selection or default style (undoable).
        public bool ApplyUnderlineToSelection()
        {
            return NativeControl.ApplyUnderlineToSelection();
        }

        // Applies one or more wxTextAttrEffects flags to the selection (undoable).
        // If there is no selection, it is applied to the default style.
        public bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags)
        {
            return NativeControl.ApplyTextEffectToSelection((int)flags);
        }

        // Applies the given alignment to the selection or the default style (undoable).
        // For alignment values, see wxTextAttr.
        public bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.ApplyAlignmentToSelection((int)alignment);
        }

        // Sets the default style to the style under the cursor.
        public bool SetDefaultStyleToCursorStyle()
        {
            return NativeControl.SetDefaultStyleToCursorStyle();
        }

        // Cancels any selection.
        public void SelectNone()
        {
            NativeControl.SelectNone();
        }

        // Selects the word at the given character position.
        public bool SelectWord(long position)
        {
            return NativeControl.SelectWord(position);
        }

        // Lays out the buffer, which must be done before certain operations, such as
        // setting the caret position.
        // This function should not normally be required by the application.
        public bool LayoutContent(bool onlyVisibleRect = false)
        {
            return NativeControl.LayoutContent(onlyVisibleRect);
        }

        // Moves right.
        public bool MoveRight(int noPositions = 1, int flags = 0)
        {
            return NativeControl.MoveRight(noPositions, flags);
        }

        // Moves left.
        public bool MoveLeft(int noPositions = 1, int flags = 0)
        {
            return NativeControl.MoveLeft(noPositions, flags);
        }

        // Moves to the start of the paragraph.
        public bool MoveUp(int noLines = 1, int flags = 0)
        {
            return NativeControl.MoveUp(noLines, flags);
        }

        // Moves the caret down.
        public bool MoveDown(int noLines = 1, int flags = 0)
        {
            return NativeControl.MoveDown(noLines, flags);
        }

        // Moves to the end of the line.
        public bool MoveToLineEnd(int flags = 0)
        {
            return NativeControl.MoveToLineEnd(flags);
        }

        // Moves to the start of the line.
        public bool MoveToLineStart(int flags = 0)
        {
            return NativeControl.MoveToLineStart(flags);
        }

        // Moves to the end of the paragraph.
        public bool MoveToParagraphEnd(int flags = 0)
        {
            return NativeControl.MoveToParagraphEnd(flags);
        }

        // Moves to the start of the paragraph.
        public bool MoveToParagraphStart(int flags = 0)
        {
            return NativeControl.MoveToParagraphStart(flags);
        }

        // Moves to the start of the buffer.
        public bool MoveHome(int flags = 0)
        {
            return NativeControl.MoveHome(flags);
        }

        // Moves to the end of the buffer.
        public bool MoveEnd(int flags = 0)
        {
            return NativeControl.MoveEnd(flags);
        }

        // Moves one or more pages up.
        public bool PageUp(int noPages = 1, int flags = 0)
        {
            return NativeControl.PageUp(noPages, flags);
        }

        // Moves one or more pages down.
        public bool PageDown(int noPages = 1, int flags = 0)
        {
            return NativeControl.PageDown(noPages, flags);
        }

        // Moves a number of words to the left.
        public bool WordLeft(int noPages = 1, int flags = 0)
        {
            return NativeControl.WordLeft(noPages, flags);
        }

        // Move a number of words to the right.
        public bool WordRight(int noPages = 1, int flags = 0)
        {
            return NativeControl.WordRight(noPages, flags);
        }

        // Starts batching undo history for commands.
        public bool BeginBatchUndo(string cmdName)
        {
            return NativeControl.BeginBatchUndo(cmdName);
        }

        // Ends batching undo command history.
        public bool EndBatchUndo()
        {
            return NativeControl.EndBatchUndo();
        }

        // Returns <c>true</c> if undo commands are being batched.
        public bool BatchingUndo()
        {
            return NativeControl.BatchingUndo();
        }

        // Starts suppressing undo history for commands.
        public bool BeginSuppressUndo()
        {
            return NativeControl.BeginSuppressUndo();
        }

        // Ends suppressing undo command history.
        public bool EndSuppressUndo()
        {
            return NativeControl.EndSuppressUndo();
        }

        // Returns <c>true</c> if undo history suppression is on.
        public bool SuppressingUndo()
        {
            return NativeControl.SuppressingUndo();
        }

        // Enable or disable the vertical scrollbar.
        public void EnableVerticalScrollbar(bool enable)
        {
            NativeControl.EnableVerticalScrollbar(enable);
        }

        // Returns <c>true</c> if the vertical scrollbar is enabled.
        public bool GetVerticalScrollbarEnabled()
        {
            return NativeControl.GetVerticalScrollbarEnabled();
        }

        // Sets the scale factor for displaying fonts, for example for more comfortableediting.
        public void SetFontScale(double fontScale, bool refresh = false)
        {
            NativeControl.SetFontScale(fontScale, refresh);
        }

        // Returns the scale factor for displaying fonts, for example for more comfortable editing.
        public double GetFontScale()
        {
            return NativeControl.GetFontScale();
        }

        // Returns <c>true</c> if this control can use attributes and text. The default is @false.
        public bool GetVirtualAttributesEnabled()
        {
            return NativeControl.GetVirtualAttributesEnabled();
        }

        // Pass <c>true</c> to let the control use attributes. The default is @false.
        public void EnableVirtualAttributes(bool b)
        {
            NativeControl.EnableVirtualAttributes(b);
        }

        // Write text
        public void DoWriteText(string value, int flags = 0)
        {
            NativeControl.DoWriteText(value, flags);
        }

        // Helper function for extending the selection, returning <c>true</c> if the selection
        // was changed. Selections are in caret positions.
        public bool ExtendSelection(long oldPosition, long newPosition, int flags)
        {
            return NativeControl.ExtendSelection(oldPosition, newPosition, flags);
        }

        // Sets the caret position.
        // The caret position is the character position just before the caret.
        // A value of -1 means the caret is at the start of the buffer.
        // Please note that this does not update the current editing style
        // from the new position or cause the actual caret to be refreshed; to do that,
        // call wxRichTextCtrl::SetInsertionPoint instead.
        public void SetCaretPosition(long position, bool showAtLineStart = false)
        {
            NativeControl.SetCaretPosition(position, showAtLineStart);
        }

        // Returns the current caret position.
        public long GetCaretPosition()
        {
            return NativeControl.GetCaretPosition();
        }

        // The adjusted caret position is the character position adjusted to take
        // into account whether we're at the start of a paragraph, in which case
        // style information should be taken from the next position, not current one.
        public long GetAdjustedCaretPosition(long caretPos)
        {
            return NativeControl.GetAdjustedCaretPosition(caretPos);
        }

        // Move the caret one visual step forward: this may mean setting a flag
        // and keeping the same position if we're going from the end of one line
        // to the start of the next, which may be the exact same caret position.
        public void MoveCaretForward(long oldPosition)
        {
            NativeControl.MoveCaretForward(oldPosition);
        }

        // Transforms logical (unscrolled) position to physical window position.
        public Int32Point GetPhysicalPoint(Int32Point ptLogical)
        {
            return NativeControl.GetPhysicalPoint(ptLogical);
        }

        // Transforms physical window position to logical (unscrolled) position.
        public Int32Point GetLogicalPoint(Int32Point ptPhysical)
        {
            return NativeControl.GetLogicalPoint(ptPhysical);
        }

        // Helper function for finding the caret position for the next word.
        // Direction is 1 (forward) or -1 (backwards).
        public long FindNextWordPosition(int direction = 1)
        {
            return NativeControl.FindNextWordPosition(direction);
        }

        // Returns <c>true</c> if the given position is visible on the screen.
        public bool IsPositionVisible(long pos)
        {
            return NativeControl.IsPositionVisible(pos);
        }

        // Returns the first visible position in the current view.
        public long GetFirstVisiblePosition()
        {
            return NativeControl.GetFirstVisiblePosition();
        }

        // Returns the caret position since the default formatting was changed. As
        // soon as this position changes, we no longer reflect the default style
        // in the UI. A value of -2 means that we should only reflect the style of the
        // content under the caret.
        public long GetCaretPositionForDefaultStyle()
        {
            return NativeControl.GetCaretPositionForDefaultStyle();
        }

        // Set the caret position for the default style that the user is selecting.
        public void SetCaretPositionForDefaultStyle(long pos)
        {
            NativeControl.SetCaretPositionForDefaultStyle(pos);
        }

        // Move the caret one visual step forward: this may mean setting a flag
        // and keeping the same position if we're going from the end of one line
        // to the start of the next, which may be the exact same caret position.
        public void MoveCaretBack(long oldPosition)
        {
            NativeControl.MoveCaretBack(oldPosition);
        }

        // Returns <c>true</c> if the user has recently set the default style without moving
        // the caret, and therefore the UI needs to reflect the default style and not
        // the style at the caret.
        public bool IsDefaultStyleShowing()
        {
            return NativeControl.IsDefaultStyleShowing();
        }

        // Returns the first visible point in the window.
        public Int32Point GetFirstVisiblePoint()
        {
            return NativeControl.GetFirstVisiblePoint();
        }

        // Enable or disable images
        public void EnableImages(bool b)
        {
            NativeControl.EnableImages(b);
        }

        // Returns <c>true</c> if images are enabled.
        public bool GetImagesEnabled()
        {
            return NativeControl.GetImagesEnabled();
        }

        // Enable or disable delayed image loading.
        public void EnableDelayedImageLoading(bool b)
        {
            NativeControl.EnableDelayedImageLoading(b);
        }

        // Returns <c>true</c> if delayed image loading is enabled.
        public bool GetDelayedImageLoading()
        {
            return NativeControl.GetDelayedImageLoading();
        }

        // Gets the flag indicating that delayed image processing is required.
        public bool GetDelayedImageProcessingRequired()
        {
            return NativeControl.GetDelayedImageProcessingRequired();
        }

        // Sets the flag indicating that delayed image processing is required.
        public void SetDelayedImageProcessingRequired(bool b)
        {
            NativeControl.SetDelayedImageProcessingRequired(b);
        }

        // Returns the last time delayed image processing was performed.
        public long GetDelayedImageProcessingTime()
        {
            return NativeControl.GetDelayedImageProcessingTime();
        }

        // Sets the last time delayed image processing was performed.
        public void SetDelayedImageProcessingTime(long t)
        {
            NativeControl.SetDelayedImageProcessingTime(t);
        }

        // Returns the content of the entire control as a string.
        public string GetValue()
        {
            return NativeControl.GetValue();
        }

        // Replaces existing content with the given text.
        public void SetValue(string value)
        {
            NativeControl.SetValue(value);
        }

        // Set the line increment height in pixels
        public void SetLineHeight(int height)
        {
            NativeControl.SetLineHeight(height);
        }

        public int GetLineHeight()
        {
            return NativeControl.GetLineHeight();
        }

        // Do delayed image loading and garbage-collect other images.
        public bool ProcessDelayedImageLoading(bool refresh)
        {
            return NativeControl.ProcessDelayedImageLoading(refresh);
        }

        // Request delayed image processing.
        public void RequestDelayedImageProcessing()
        {
            NativeControl.RequestDelayedImageProcessing();
        }

        // Returns the last position in the buffer.
        public long GetLastPosition()
        {
            return NativeControl.GetLastPosition();
        }

        public bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            int flags,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.SetListStyle2(
                startRange,
                endRange,
                defName,
                flags,
                startFrom,
                specifiedLevel);
        } // = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Clears the list style from the given range, clearing list-related attributes
        // and applying any named paragraph style associated with each paragraph.
        // @a flags is a bit list of the following:
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        public bool ClearListStyle(long startRange, long endRange, int flags)
        {
            return NativeControl.ClearListStyle(startRange, endRange, flags);
        }// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        public bool NumberList(
            long startRange,
            long endRange,
            string defName,
            int flags,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.NumberList2(
                startRange,
                endRange,
                defName,
                flags,
                startFrom,
                specifiedLevel);
        }// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        public bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            int flags,
            int specifiedLevel = -1)
        {
            return NativeControl.PromoteList2(
                promoteBy,
                startRange,
                endRange,
                defName,
                flags,
                specifiedLevel);
        } // = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Deletes the content within the given range.
        public bool Delete(long startRange, long endRange)
        {
            return NativeControl.Delete(startRange, endRange);
        }

        // Sets the selection to the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // So, for example, to set the selection for a character at position 5, use the
        // range (5,6).
        public void SetSelectionRange(long startRange, long endRange)
        {
            NativeControl.SetSelectionRange(startRange, endRange);
        }

        // Converts a text position to zero-based column and line numbers.
        public Int32Point PositionToXY(long pos)
        {
            return NativeControl.PositionToXY(pos);
        }

        // Deletes content if there is a selection, e.g. when pressing a key.
        // Returns the new caret position in @e newPos, or leaves it if there
        // was no action. This is undoable.
        public long DeleteSelectedContent()
        {
            return NativeControl.DeleteSelectedContent();
        }

        // Returns the current context menu.
        internal IntPtr GetContextMenu()
        {
            return NativeControl.GetContextMenu();
        }

        // Sets the current context menu.
        internal void SetContextMenu(IntPtr menu)
        {
            NativeControl.SetContextMenu(menu);
        }

        // Returns the anchor object if selecting multiple containers.
        internal IntPtr GetSelectionAnchorObject()
        {
            return NativeControl.GetSelectionAnchorObject();
        }

        // Sets the anchor object if selecting multiple containers.
        internal void SetSelectionAnchorObject(IntPtr anchor)
        {
            NativeControl.SetSelectionAnchorObject(anchor);
        }

        // Returns the wxRichTextObject object that currently has the editing focus.
        // If there are no composite objects, this will be the top-level buffer.
        internal IntPtr GetFocusObject()
        {
            return NativeControl.GetFocusObject();
        }

        // Sets m_focusObject without making any alterations.
        internal void StoreFocusObject(IntPtr richObj)
        {
            NativeControl.StoreFocusObject(richObj);
        }

        // Begins using this font.
        internal bool BeginFont(Font? font)
        {
            return NativeControl.BeginFont(font?.NativeFont);
        }

        // Applies the style sheet to the buffer, matching paragraph styles in the sheet
        // against named styles in the buffer.
        // This might be useful if the styles have changed.
        // If @a sheet is @NULL, the sheet set with SetStyleSheet() is used.
        // Currently this applies paragraph styles only.
        internal bool ApplyStyle(IntPtr def)
        {
            return NativeControl.ApplyStyle(def);
        }

        // Sets the style sheet associated with the control.
        // A style sheet allows named character and paragraph styles to be applied.
        internal void SetStyleSheet(IntPtr styleSheet)
        {
            NativeControl.SetStyleSheet(styleSheet);
        }

        // Move the caret to the given character position.
        // Please note that this does not update the current editing style
        // from the new position; to do that, call wxRichTextCtrl::SetInsertionPoint instead.
        internal bool MoveCaret(long pos, bool showAtLineStart = false, IntPtr container = default)
        {
            return NativeControl.MoveCaret(pos, showAtLineStart, container);
        }

        // Push the style sheet to top of stack.
        internal bool PushStyleSheet(IntPtr styleSheet)
        {
            return NativeControl.PushStyleSheet(styleSheet);
        }

        // Pops the style sheet from top of stack.
        internal IntPtr PopStyleSheet()
        {
            return NativeControl.PopStyleSheet();
        }

        // Applies the style sheet to the buffer, for example if the styles have changed.
        internal bool ApplyStyleSheet(IntPtr styleSheet = default)
        {
            return NativeControl.ApplyStyleSheet(styleSheet);
        }

        // Shows the given context menu, optionally adding appropriate property-editing commands for the current position in the object hierarchy.
        internal bool ShowContextMenu(IntPtr menu, Int32Point pt, bool addPropertyCommands = true)
        {
            return NativeControl.ShowContextMenu(menu, pt, addPropertyCommands);
        }

        // Prepares the context menu, optionally adding appropriate property-editing commands.
        // Returns the number of property commands added.
        internal int PrepareContextMenu(IntPtr menu, Int32Point pt, bool addPropertyCommands = true)
        {
            return NativeControl.PrepareContextMenu(menu, pt, addPropertyCommands);
        }

        // Returns <c>true</c> if we can edit the object's properties via a GUI.
        internal bool CanEditProperties(IntPtr richObj)
        {
            return NativeControl.CanEditProperties(richObj);
        }

        // Edits the object's properties via a GUI.
        internal bool EditProperties(IntPtr richObj, IntPtr parentWindow)
        {
            return NativeControl.EditProperties(richObj, parentWindow);
        }

        // Extends a table selection in the given direction.
        internal bool ExtendCellSelection(IntPtr table, int noRowSteps, int noColSteps)
        {
            return NativeControl.ExtendCellSelection(table, noRowSteps, noColSteps);
        }

        // Starts selecting table cells.
        internal bool StartCellSelection(IntPtr table, IntPtr newCell)
        {
            return NativeControl.StartCellSelection(table, newCell);
        }

        // Scrolls @a position into view. This function takes a caret position.
        internal bool ScrollIntoView(long position, int keyCode)
        {
            return NativeControl.ScrollIntoView(position, keyCode);
        }

        // Returns the caret height and position for the given character position.
        // If container is null, the current focus object will be used.
        internal bool GetCaretPositionForIndex(
            long position,
            Int32Rect rect,
            IntPtr container = default)
        {
            return NativeControl.GetCaretPositionForIndex(position, rect, container);
        }

        // Internal helper function returning the line for the visible caret position.
        // If the caret is shown at the very end of the line, it means the next character
        // is actually on the following line.
        // So this function gets the line we're expecting to find if this is the case.
        internal IntPtr GetVisibleLineForCaretPosition(long caretPosition)
        {
            return NativeControl.GetVisibleLineForCaretPosition(caretPosition);
        }

        // Gets the command processor associated with the control's buffer.
        internal IntPtr GetCommandProcessor()
        {
            return NativeControl.GetCommandProcessor();
        }

        // Sets up the caret for the given position and container, after a mouse click.
        internal bool SetCaretPositionAfterClick(
            IntPtr container,
            long position,
            int hitTestFlags,
            bool extendSelection = false)
        {
            return NativeControl.SetCaretPositionAfterClick(
                container,
                position,
                hitTestFlags,
                extendSelection);
        }

        // Gets the attributes at the given position.
        // This function gets the @e uncombined style - that is, the attributes associated
        // with the paragraph or character content, and not necessarily the combined
        // attributes you see on the screen.
        // To get the combined attributes, use GetStyle().
        // If you specify (any) paragraph attribute in @e style's flags, this function
        // will fetch the paragraph attributes.
        // Otherwise, it will return the character attributes.
        internal bool GetUncombinedStyle(long position, IntPtr style)
        {
            return NativeControl.GetUncombinedStyle(position, style);
        }

        internal bool GetUncombinedStyle(long position, IntPtr style, IntPtr container)
        {
            return NativeControl.GetUncombinedStyle2(position, style, container);
        }

        // Sets the current default style, which can be used to change how subsequently
        // inserted text is displayed.
        internal bool SetDefaultStyle(IntPtr style)
        {
            return NativeControl.SetDefaultStyle(style);
        }

        internal bool SetDefaultRichStyle(IntPtr style)
        {
            return NativeControl.SetDefaultRichStyle(style);
        }

        // Returns the current default style, which can be used to change how subsequently
        // inserted text is displayed.
        internal IntPtr GetDefaultStyleEx()
        {
            return NativeControl.GetDefaultStyleEx();
        }

        // Gets the attributes at the given position.
        // This function gets the combined style - that is, the style you see on the
        // screen as a result of combining base style, paragraph style and character
        // style attributes.
        // To get the character or paragraph style alone, use GetUncombinedStyle().
        internal IntPtr GetStyle(long position)
        {
            return NativeControl.GetStyle(position);
        }

        internal IntPtr GetRichStyle(long position)
        {
            return NativeControl.GetRichStyle(position);
        }

        internal IntPtr GetStyleInContainer(long position, IntPtr container)
        {
            return NativeControl.GetStyleInContainer(position, container);
        }

        // Sets the attributes for the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one. So, for example, to set the style for a character at
        // position 5, use the range (5,6).
        internal bool SetStyle(long start, long end, IntPtr style)
        {
            return NativeControl.SetStyle(start, end, style);
        }

        internal bool SetRichStyle(long start, long end, IntPtr style)
        {
            return NativeControl.SetRichStyle(start, end, style);
        }

        // Sets the attributes for a single object
        internal void SetStyle(IntPtr richObj, IntPtr textAttr, int flags)
        {
            NativeControl.SetStyle2(richObj, textAttr, flags);
        }// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Gets the attributes common to the specified range.
        // Attributes that differ in value within the range will not be included
        // in @a style flags.
        internal IntPtr GetStyleForRange(long startRange, long endRange)
        {
            return NativeControl.GetStyleForRange(startRange, endRange);
        }

        internal IntPtr GetRichStyleForRange(long startRange, long endRange)
        {
            return NativeControl.GetStyleForRange2(startRange, endRange);
        }

        internal IntPtr GetStyleForRange(long startRange, long endRange, IntPtr container)
        {
            return NativeControl.GetStyleForRange3(startRange, endRange, container);
        }

        // Sets the attributes for the given range, passing flags to determine how the
        // attributes are set.
        // The end point of range is specified as the last character position of the span
        // of text, plus one. So, for example, to set the style for a character at
        // position 5, use the range (5,6).
        // @a flags may contain a bit list of the following values:
        // - wxRICHTEXT_SETSTYLE_NONE: no style flag.
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this operation should be
        //   undoable.
        // - wxRICHTEXT_SETSTYLE_OPTIMIZE: specifies that the style should not be applied
        //   if the combined style at this point is already the style in question.
        // - wxRICHTEXT_SETSTYLE_PARAGRAPHS_ONLY: specifies that the style should only be
        //   applied to paragraphs, and not the content.
        //   This allows content styling to be preserved independently from that
        //   of e.g. a named paragraph style.
        // - wxRICHTEXT_SETSTYLE_CHARACTERS_ONLY: specifies that the style should only be
        //   applied to characters, and not the paragraph.
        //   This allows content styling to be preserved independently from that
        //   of e.g. a named paragraph style.
        // - wxRICHTEXT_SETSTYLE_RESET: resets (clears) the existing style before applying
        //   the new style.
        // - wxRICHTEXT_SETSTYLE_REMOVE: removes the specified style. Only the style flags
        //   are used in this operation.
        internal bool SetStyleEx(long startRange, long endRange, IntPtr style, int flags)
        {
            return NativeControl.SetStyleEx(startRange, endRange, style, flags);
        } // = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Sets the list attributes for the given range, passing flags to determine how
        // the attributes are set.
        // Either the style definition or the name of the style definition (in the current
        // sheet) can be passed.
        // @a flags is a bit list of the following:
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        // - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        //   @a startFrom, otherwise existing attributes are used.
        // - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        //   as the level for all paragraphs, otherwise the current indentation will be used.
        internal bool SetListStyle(
            long startRange,
            long endRange,
            IntPtr def,
            int flags,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.SetListStyle(
                startRange,
                endRange,
                def,
                flags,
                startFrom,
                specifiedLevel);
        } // = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Numbers the paragraphs in the given range.
        // Pass flags to determine how the attributes are set.
        // Either the style definition or the name of the style definition (in the current
        // sheet) can be passed.
        // @a flags is a bit list of the following:
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        // - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        //   @a startFrom, otherwise existing attributes are used.
        // - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        // as the level for all paragraphs, otherwise the current indentation will be used.
        internal bool NumberList(
            long startRange,
            long endRange,
            IntPtr def,
            int flags,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.NumberList(
                startRange,
                endRange,
                def,
                flags,
                startFrom,
                specifiedLevel);
        }// = wxRICHTEXT_SETSTYLE_WITH_UNDO, def = default

        // Promotes or demotes the paragraphs in the given range.
        // A positive @a promoteBy produces a smaller indent, and a negative number
        // produces a larger indent. Pass flags to determine how the attributes are set.
        // Either the style definition or the name of the style definition (in the current
        // sheet) can be passed.
        // @a flags is a bit list of the following:
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        // - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        //   @a startFrom, otherwise existing attributes are used.
        // - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        // as the level for all paragraphs, otherwise the current indentation will be used.
        internal bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            IntPtr def,
            int flags,
            int specifiedLevel = -1)
        {
            return NativeControl.PromoteList(
                promoteBy,
                startRange,
                endRange,
                def,
                flags,
                specifiedLevel);
        } // def = default, = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Write a table at the current insertion point, returning the table.
        // You can then call SetFocusObject() to set the focus to the new object.
        internal IntPtr WriteTable(
            int rows,
            int cols,
            IntPtr tableAttr = default,
            IntPtr cellAttr = default)
        {
            return NativeControl.WriteTable(
                rows,
                cols,
                tableAttr,
                cellAttr);
        }

        // Sets the basic (overall) style.
        // This is the style of the whole buffer before further styles are applied,
        // unlike the default style, which only affects the style currently being
        // applied (for example, setting the default style to bold will cause
        // subsequently inserted text to be bold).
        internal void SetBasicStyle(IntPtr style)
        {
            NativeControl.SetBasicStyle(style);
        }

        // Gets the basic (overall) style.
        // This is the style of the whole buffer before further styles are applied,
        // unlike the default style, which only affects the style currently being
        // applied (for example, setting the default style to bold will cause
        // subsequently inserted text to be bold).
        internal IntPtr GetBasicStyle()
        {
            return NativeControl.GetBasicStyle();
        }

        // Begins applying a style.
        internal bool BeginStyle(IntPtr style)
        {
            return NativeControl.BeginStyle(style);
        }

        // Test if this whole range has character attributes of the specified kind.
        // If any of the attributes are different within the range, the test fails.
        // You can use this to implement, for example, bold button updating.
        // @a style must have flags indicating which attributes are of interest.
        internal bool HasCharacterAttributes(long startRange, long endRange, IntPtr style)
        {
            return NativeControl.HasCharacterAttributes(startRange, endRange, style);
        }

        // Returns the style sheet associated with the control, if any.
        // A style sheet allows named character and paragraph styles to be applied.
        internal IntPtr GetStyleSheet()
        {
            return NativeControl.GetStyleSheet();
        }

        // Sets @a attr as the default style and tells the control that the UI should
        // reflect this attribute until the user moves the caret.
        internal void SetAndShowDefaultStyle(IntPtr attr)
        {
            NativeControl.SetAndShowDefaultStyle(attr);
        }

        // Write a text box at the current insertion point, returning the text box.
        // You can then call SetFocusObject() to set the focus to the new object.
        internal IntPtr WriteTextBox(IntPtr textAttr = default)
        {
            return NativeControl.WriteTextBox(textAttr);
        }

        // Test if this whole range has paragraph attributes of the specified kind.
        // If any of the attributes are different within the range, the test fails.
        // You can use this to implement, for example, centering button updating.
        // @a style must have flags indicating which attributes are of interest.
        internal bool HasParagraphAttributes(
            long startRange,
            long endRange,
            IntPtr style)
        {
            return NativeControl.HasParagraphAttributes(
                startRange,
                endRange,
                style);
        }

        // Sets the properties for the given range, passing flags to determine how the
        // attributes are set. You can merge properties or replace them.
        // The end point of range is specified as the last character position of the span
        // of text, plus one. So, for example, to set the properties for a character at
        // position 5, use the range (5,6).
        // @a flags may contain a bit list of the following values:
        // - wxRICHTEXT_SETSPROPERTIES_NONE: no flag.
        // - wxRICHTEXT_SETPROPERTIES_WITH_UNDO: specifies that this operation should be
        //   undoable.
        // - wxRICHTEXT_SETPROPERTIES_PARAGRAPHS_ONLY: specifies that the properties should only be
        //   applied to paragraphs, and not the content.
        // - wxRICHTEXT_SETPROPERTIES_CHARACTERS_ONLY: specifies that the properties should only be
        //   applied to characters, and not the paragraph.
        // - wxRICHTEXT_SETPROPERTIES_RESET: resets (clears) the existing properties before applying
        //   the new properties.
        // - wxRICHTEXT_SETPROPERTIES_REMOVE: removes the specified properties.
        internal bool SetProperties(
            long startRange,
            long endRange,
            IntPtr properties,
            int flags)
        {
            return NativeControl.SetProperties(
                startRange,
                endRange,
                properties,
                flags);
        } // = wxRICHTEXT_SETPROPERTIES_WITH_UNDO

        // Sets the text (normal) cursor.
        internal void SetTextCursor(IntPtr cursor)
        {
            NativeControl.SetTextCursor(cursor);
        }

        // Returns the text (normal) cursor.
        internal IntPtr GetTextCursor()
        {
            return NativeControl.GetTextCursor();
        }

        // Sets the cursor to be used over URLs.
        internal void SetURLCursor(IntPtr cursor)
        {
            NativeControl.SetURLCursor(cursor);
        }

        // Returns the cursor to be used over URLs.
        internal IntPtr GetURLCursor()
        {
            return NativeControl.GetURLCursor();
        }

        // Returns the range of the current selection.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // If the return values @a from and @a to are the same, there is no selection.
        internal IntPtr GetSelection()
        {
            return NativeControl.GetSelection();
        }

        // Returns an object that stores information about context menu property item(s),
        // in order to communicate between the context menu event handler and the code
        // that responds to it. The wxRichTextContextMenuPropertiesInfo stores one
        // item for each object that could respond to a property-editing event. If
        // objects are nested, several might be editable.
        internal IntPtr GetContextMenuPropertiesInfo()
        {
            return NativeControl.GetContextMenuPropertiesInfo();
        }

        internal void SetSelection(IntPtr sel)
        {
            NativeControl.SetSelection2(sel);
        }

        // Write a bitmap or image at the current insertion point.
        // Supply an optional type to use for internal and file storage of the raw data.
        internal bool WriteImage(
            Image bitmap,
            int bitmapType,
            IntPtr textAttr = default)
        {
            if (bitmap is null)
                return false;
            return NativeControl.WriteImage(
                bitmap.NativeImage,
                bitmapType,
                textAttr);
        } // = wxBITMAP_TYPE_PNG

        // Loads an image from a file and writes it at the current insertion point.
        internal bool WriteImage(
            string filename,
            int bitmapType,
            IntPtr textAttr = default)
        {
            return NativeControl.WriteImage2(
                filename,
                bitmapType,
                textAttr);
        }

        // Writes an image block at the current insertion point.
        internal bool WriteImage(IntPtr imageBlock, IntPtr textAttr = default)
        {
            return NativeControl.WriteImage3(imageBlock, textAttr);
        }

        // Writes a field at the current insertion point.
        //   @param fieldType
        //       The field type, matching an existing field type definition.
        //   @param properties
        //       Extra data for the field.
        //  @param textAttr
        //      Optional attributes.
        internal IntPtr WriteField(
            string fieldType,
            IntPtr properties,
            IntPtr textAttr = default)
        {
            return NativeControl.WriteField(
                fieldType,
                properties,
                textAttr);
        }

        // Can we delete this range?
        // Sends an event to the control.
        internal bool CanDeleteRange(IntPtr container, long startRange, long endRange)
        {
            return NativeControl.CanDeleteRange(container, startRange, endRange);
        }

        // Can we insert content at this position?
        // Sends an event to the control.
        internal bool CanInsertContent(IntPtr container, long pos)
        {
            return NativeControl.CanInsertContent(container, pos);
        }

        // Returns the buffer associated with the control.
        internal IntPtr GetBuffer()
        {
            return NativeControl.GetBuffer();
        }

        // Gets the object's properties menu label.
        internal string GetPropertiesMenuLabel(IntPtr richObj)
        {
            return NativeControl.GetPropertiesMenuLabel(richObj);
        }

        // Sets the wxRichTextObject object that currently has the editing focus.
        internal bool SetFocusObject(IntPtr richObj, bool setCaretPosition = true)
        {
            return NativeControl.SetFocusObject(richObj, setCaretPosition);
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeRichTextBoxHandler();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
    }
}
