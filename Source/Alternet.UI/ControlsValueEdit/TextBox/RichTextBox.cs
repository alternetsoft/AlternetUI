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
        private bool hasBorder = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        public virtual bool HasBorder
        {
            get
            {
                return hasBorder;
            }

            set
            {
                if (hasBorder == value)
                    return;
                hasBorder = value;
                NativeControl.HasBorder = value;
            }
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

        /// <summary>
        /// Clears the cache of available font names.
        /// </summary>
        public static void ClearAvailableFontNames()
        {
            Native.RichTextBox.ClearAvailableFontNames();
        }

        /// <summary>
        /// Creates new custom rich text style.
        /// </summary>
        /// <returns></returns>
        public static ITextBoxRichAttr CreateRichAttr()
        {
            return new TextBoxRichAttr();
        }

        /// <summary>
        /// <inheritdoc cref="TextBox.GetRange"/>
        /// </summary>
        public string GetRange(long from, long to)
        {
            return NativeControl.GetRange(from, to);
        }

        /// <summary>
        /// Returns the length of the specified line in characters.
        /// </summary>
        public int GetLineLength(long lineNo)
        {
            return NativeControl.GetLineLength(lineNo);
        }

        /// <summary>
        /// Returns the text for the given line.
        /// </summary>
        public string GetLineText(long lineNo)
        {
            return NativeControl.GetLineText(lineNo);
        }

        /// <summary>
        /// Returns the number of lines in the buffer.
        /// </summary>
        public int GetNumberOfLines()
        {
            return NativeControl.GetNumberOfLines();
        }

        /// <summary>
        /// Returns <c>true</c> if the buffer has been modified.
        /// </summary>
        public bool IsModified()
        {
            return NativeControl.IsModified();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is editable.
        /// </summary>
        public bool IsEditable()
        {
            return NativeControl.IsEditable();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is single-line.
        /// Currently <see cref="RichTextBox"/> does not support single-line editing.
        /// </summary>
        public bool IsSingleLine()
        {
            return NativeControl.IsSingleLine();
        }

        /// <summary>
        /// Returns <c>true</c> if the control is multiline.
        /// </summary>
        public bool IsMultiLine()
        {
            return NativeControl.IsMultiLine();
        }

        /// <summary>
        /// Returns the text within the current selection range, if any.
        /// </summary>
        public string GetStringSelection()
        {
            return NativeControl.GetStringSelection();
        }

        /// <summary>
        /// Gets the current filename associated with the control.
        /// </summary>
        public string GetFilename()
        {
            return NativeControl.GetFilename();
        }

        /// <summary>
        /// Sets the current filename.
        /// </summary>
        /// <param name="filename"></param>
        public void SetFilename(string filename)
        {
            NativeControl.SetFilename(filename);
        }

        /// <summary>
        /// Sets the size of the buffer beyond which layout is delayed during resizing.
        /// This optimizes sizing for large buffers. The default is 20000.
        /// </summary>
        /// <param name="threshold"></param>
        public void SetDelayedLayoutThreshold(long threshold)
        {
            NativeControl.SetDelayedLayoutThreshold(threshold);
        }

        /// <summary>
        /// Gets the size of the buffer beyond which layout is delayed during resizing.
        /// This optimizes sizing for large buffers. The default is 20000.
        /// </summary>
        public long GetDelayedLayoutThreshold()
        {
            return NativeControl.GetDelayedLayoutThreshold();
        }

        /// <summary>
        /// Gets the flag indicating that full layout is required.
        /// </summary>
        public bool GetFullLayoutRequired()
        {
            return NativeControl.GetFullLayoutRequired();
        }

        /// <summary>
        /// Sets the flag indicating that full layout is required.
        /// </summary>
        /// <param name="b"></param>
        public void SetFullLayoutRequired(bool b)
        {
            NativeControl.SetFullLayoutRequired(b);
        }

        /// <summary>
        /// Returns the last time full layout was performed.
        /// </summary>
        public long GetFullLayoutTime()
        {
            return NativeControl.GetFullLayoutTime();
        }

        /// <summary>
        /// Sets the last time full layout was performed.
        /// </summary>
        /// <param name="t"></param>
        public void SetFullLayoutTime(long t)
        {
            NativeControl.SetFullLayoutTime(t);
        }

        /// <summary>
        /// Returns the position that should be shown when full (delayed) layout is performed.
        /// </summary>
        /// <returns></returns>
        public long GetFullLayoutSavedPosition()
        {
            return NativeControl.GetFullLayoutSavedPosition();
        }

        /// <summary>
        /// Sets the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// true on success, false if an error occurred(this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        public virtual bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return NativeControl.SetDefaultStyle(s.Handle);
        }

        /// <summary>
        /// Appends text and styles to the end of the text control.
        /// </summary>
        /// <param name="list">List containing strings or
        /// <see cref="ITextBoxTextAttr"/> instances.</param>
        /// <remarks>
        /// After the text is appended, the insertion point will be at the end
        /// of the text control. If this behaviour is not desired,
        /// the programmer should use <see cref="GetInsertionPoint"/>
        /// and <see cref="SetInsertionPoint"/>.
        /// </remarks>
        public virtual void AppendTextAndStyles(IEnumerable<object> list)
        {
            foreach (object item in list)
            {
                var ta = item as ITextBoxTextAttr;
                if (ta is not null)
                {
                    SetDefaultStyle(ta);
                    continue;
                }

                if (item != null)
                {
                    var s = item.ToString()!;
                    if (s == "<b>")
                        BeginBold();
                    else
                    if (s == "</b>")
                        EndBold();
                    else
                    if (s == "<i>")
                        BeginItalic();
                    else
                    if (s == "</i>")
                        EndItalic();
                    else
                    if (s == "<u>")
                        BeginUnderline();
                    else
                    if (s == "</u>")
                        EndUnderline();
                    else
                        WriteText(s);
                }
            }
        }

        /// <summary>
        /// Sets the position that should be shown when full (delayed) layout is performed.
        /// </summary>
        /// <param name="p"></param>
        public void SetFullLayoutSavedPosition(long p)
        {
            NativeControl.SetFullLayoutSavedPosition(p);
        }

        /// <summary>
        /// Forces any pending layout due to delayed, partial layout when the control was resized.
        /// </summary>
        public void ForceDelayedLayout()
        {
            NativeControl.ForceDelayedLayout();
        }

        /// <summary>
        /// Returns <c>true</c> if we are showing the caret position at the start of a line
        /// instead of at the end of the previous one.
        /// </summary>
        /// <returns></returns>
        public bool GetCaretAtLineStart()
        {
            return NativeControl.GetCaretAtLineStart();
        }

        /// <summary>
        /// Sets a flag to remember that we are showing the caret position at the start of a line
        /// instead of at the end of the previous one.
        /// </summary>
        /// <param name="atStart"></param>
        public void SetCaretAtLineStart(bool atStart)
        {
            NativeControl.SetCaretAtLineStart(atStart);
        }

        /// <summary>
        /// Returns <c>true</c> if we are dragging a selection.
        /// </summary>
        /// <returns></returns>
        public bool GetDragging()
        {
            return NativeControl.GetDragging();
        }

        /// <summary>
        /// Sets a flag to remember if we are dragging a selection.
        /// </summary>
        /// <param name="dragging"></param>
        public void SetDragging(bool dragging)
        {
            NativeControl.SetDragging(dragging);
        }

        /// <summary>
        /// Returns an anchor so we know how to extend the selection.
        /// It's a caret position since it's between two characters.
        /// </summary>
        public long GetSelectionAnchor()
        {
            return NativeControl.GetSelectionAnchor();
        }

        /// <summary>
        /// Sets an anchor so we know how to extend the selection.
        /// It's a caret position since it's between two characters.
        /// </summary>
        public void SetSelectionAnchor(long anchor)
        {
            NativeControl.SetSelectionAnchor(anchor);
        }

        /// <summary>
        /// Clears the buffer content, leaving a single empty paragraph. Cannot be undone.
        /// </summary>
        public void Clear()
        {
            NativeControl.Clear();
        }

        /// <summary>
        /// Replaces the content in the specified range with the string specified by @a value.
        /// </summary>
        public void Replace(long from, long to, string value)
        {
            NativeControl.Replace(from, to, value);
        }

        /// <summary>
        /// Removes the content in the specified range.
        /// </summary>
        public void Remove(long from, long to)
        {
            NativeControl.Remove(from, to);
        }

        /// <summary>
        /// Loads content into the control's buffer using the given type.
        /// </summary>
        /// <param name="file">Filename.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        public bool LoadFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return NativeControl.LoadFile(file, (int)type);
        }

        /// <summary>
        /// Saves the buffer content using the given type.
        /// </summary>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        /// <param name="file">Filename.</param>
        /// <param name="type">File type.</param>
        public bool SaveFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return NativeControl.SaveFile(file, (int)type);
        }

        /// <summary>
        /// Sets flags that change the behaviour of loading or saving.
        /// See the documentation for each handler class to see what flags are
        /// relevant for each handler.
        /// </summary>
        public void SetFileHandlerFlags(int flags)
        {
            NativeControl.SetHandlerFlags(flags);
        }

        /// <summary>
        /// Returns flags that change the behaviour of loading or saving.
        /// See the documentation for each handler class to see what flags are
        /// relevant for each handler.
        /// </summary>
        public int GetFileHandlerFlags()
        {
            return NativeControl.GetHandlerFlags();
        }

        /// <summary>
        /// Marks the buffer as modified.
        /// </summary>
        public void MarkDirty()
        {
            NativeControl.MarkDirty();
        }

        /// <summary>
        /// Sets the buffer's modified status to false, and clears the buffer's command history.
        /// </summary>
        public void DiscardEdits()
        {
            NativeControl.DiscardEdits();
        }

        /// <summary>
        /// Sets the maximum number of characters that may be entered in a single line
        /// text control. For compatibility only; currently does nothing.
        /// </summary>
        public void SetMaxLength(ulong len)
        {
            NativeControl.SetMaxLength(len);
        }

        /// <summary>
        /// Writes text at the current position.
        /// </summary>
        public void WriteText(string text)
        {
            NativeControl.WriteText(text);
        }

        /// <summary>
        /// Sets the insertion point to the end of the buffer and writes the text.
        /// </summary>
        public void AppendText(string text)
        {
            NativeControl.AppendText(text);
        }

        /// <summary>
        /// Translates from column and line number to position.
        /// </summary>
        public long XYToPosition(long x, long y)
        {
            return NativeControl.XYToPosition(x, y);
        }

        /// <summary>
        /// Scrolls the buffer so that the given position is in view.
        /// </summary>
        public void ShowPosition(long pos)
        {
            NativeControl.ShowPosition(pos);
        }

        /// <summary>
        /// Copies the selected content (if any) to the clipboard.
        /// </summary>
        public void Copy()
        {
            NativeControl.Copy();
        }

        /// <summary>
        /// Copies the selected content (if any) to the clipboard and deletes the selection.
        /// This is undoable.
        /// </summary>
        public void Cut()
        {
            NativeControl.Cut();
        }

        /// <summary>
        /// Pastes content from the clipboard to the buffer.
        /// </summary>
        public void Paste()
        {
            NativeControl.Paste();
        }

        /// <summary>
        /// Deletes the content in the selection, if any. This is undoable.
        /// </summary>
        public void DeleteSelection()
        {
            NativeControl.DeleteSelection();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be copied to the clipboard.
        /// </summary>
        public bool CanCopy()
        {
            return NativeControl.CanCopy();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be copied to the clipboard and deleted.
        /// </summary>
        public bool CanCut()
        {
            return NativeControl.CanCut();
        }

        /// <summary>
        /// Returns <c>true</c> if the clipboard content can be pasted to the buffer.
        /// </summary>
        public bool CanPaste()
        {
            return NativeControl.CanPaste();
        }

        /// <summary>
        /// Returns <c>true</c> if selected content can be deleted.
        /// </summary>
        public bool CanDeleteSelection()
        {
            return NativeControl.CanDeleteSelection();
        }

        /// <summary>
        /// Undoes the command at the top of the command history, if there is one.
        /// </summary>
        public void Undo()
        {
            NativeControl.Undo();
        }

        /// <summary>
        /// Redoes the current command.
        /// </summary>
        public void Redo()
        {
            NativeControl.Redo();
        }

        /// <summary>
        /// Returns <c>true</c> if there is a command in the command history that can be undone.
        /// </summary>
        public bool CanUndo()
        {
            return NativeControl.CanUndo();
        }

        /// <summary>
        /// Returns <c>true</c> if there is a command in the command history that can be redone.
        /// </summary>
        public bool CanRedo()
        {
            return NativeControl.CanRedo();
        }

        /// <summary>
        /// Sets the insertion point and causes the current editing style to be taken from
        /// the new position (unlike <see cref="SetCaretPosition"/>).
        /// </summary>
        public void SetInsertionPoint(long pos)
        {
            NativeControl.SetInsertionPoint(pos);
        }

        /// <summary>
        /// Sets the insertion point to the end of the text control.
        /// </summary>
        public void SetInsertionPointEnd()
        {
            NativeControl.SetInsertionPointEnd();
        }

        /// <summary>
        /// Returns the current insertion point.
        /// </summary>
        public long GetInsertionPoint()
        {
            return NativeControl.GetInsertionPoint();
        }

        /// <summary>
        /// Sets the selection to the given range.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// So, for example, to set the selection for a character at position 5, use the
        /// range (5,6).
        /// </summary>
        public void SetSelection(long from, long to)
        {
            NativeControl.SetSelection(from, to);
        }

        /// <summary>
        /// Makes the control editable, or not.
        /// </summary>
        public void SetEditable(bool editable)
        {
            NativeControl.SetEditable(editable);
        }

        /// <summary>
        /// Returns <c>true</c> if there is a selection and the object containing the selection
        /// was the same as the current focus object.
        /// </summary>
        public bool HasSelection()
        {
            return NativeControl.HasSelection();
        }

        /// <summary>
        /// Returns <c>true</c> if there was a selection, whether or not the current focus object
        /// is the same as the selection's container object.
        /// </summary>
        public bool HasUnfocusedSelection()
        {
            return NativeControl.HasUnfocusedSelection();
        }

        /// <summary>
        /// Inserts a new paragraph at the current insertion point. See <see cref="LineBreak"/>.
        /// </summary>
        public bool NewLine()
        {
            return NativeControl.Newline();
        }

        /// <summary>
        /// Inserts a line break at the current insertion point.
        /// A line break forces wrapping within a paragraph, and can be introduced by
        /// using this function or by typing Shift-Return.
        /// </summary>
        public bool LineBreak()
        {
            return NativeControl.LineBreak();
        }

        /// <summary>
        /// Ends the current style.
        /// </summary>
        public bool EndStyle()
        {
            return NativeControl.EndStyle();
        }

        /// <summary>
        /// Ends application of all styles in the current style stack.
        /// </summary>
        public bool EndAllStyles()
        {
            return NativeControl.EndAllStyles();
        }

        /// <summary>
        /// Begins using bold.
        /// </summary>
        public bool BeginBold()
        {
            return NativeControl.BeginBold();
        }

        /// <summary>
        /// Ends using bold.
        /// </summary>
        public bool EndBold()
        {
            return NativeControl.EndBold();
        }

        /// <summary>
        /// Begins using italic.
        /// </summary>
        public bool BeginItalic()
        {
            return NativeControl.BeginItalic();
        }

        /// <summary>
        /// Ends using italic.
        /// </summary>
        public bool EndItalic()
        {
            return NativeControl.EndItalic();
        }

        /// <summary>
        /// Begins using underlining.
        /// </summary>
        public bool BeginUnderline()
        {
            return NativeControl.BeginUnderline();
        }

        /// <summary>
        /// End applying underlining.
        /// </summary>
        public bool EndUnderline()
        {
            return NativeControl.EndUnderline();
        }

        /// <summary>
        /// Begins using the given point size.
        /// </summary>
        public bool BeginFontSize(int pointSize)
        {
            return NativeControl.BeginFontSize(pointSize);
        }

        /// <summary>
        /// Ends using a point size.
        /// </summary>
        public bool EndFontSize()
        {
            return NativeControl.EndFontSize();
        }

        /// <summary>
        /// Ends using a font.
        /// </summary>
        public bool EndFont()
        {
            return NativeControl.EndFont();
        }

        /// <summary>
        /// Begins using this color.
        /// </summary>
        public bool BeginTextColor(Color color)
        {
            return NativeControl.BeginTextColour(color);
        }

        /// <summary>
        /// Ends applying a text color.
        /// </summary>
        public bool EndTextColor()
        {
            return NativeControl.EndTextColour();
        }

        /// <summary>
        /// Begins using alignment.
        /// </summary>
        public bool BeginAlignment(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.BeginAlignment((int)alignment);
        }

        /// <summary>
        /// Ends alignment.
        /// </summary>
        public bool EndAlignment()
        {
            return NativeControl.EndAlignment();
        }

        /// <summary>
        /// Begins applying a left indent and subindent in tenths of a millimetre.
        /// </summary>
        /// <param name="leftIndent">
        /// The distance between the margin and the bullet.
        /// </param>
        /// <param name="leftSubIndent">
        /// The distance between the left edge of the bullet and the left edge
        /// of the actual paragraph.
        /// </param>
        /// <returns></returns>
        /// <remarks>
        /// The subindent is an offset from the left edge of the paragraph, and is
        /// used for all but the first line in a paragraph. A positive value will
        /// cause the first line to appear to the left of the subsequent lines, and
        /// a negative value will cause the first line to be indented to the right
        /// of the subsequent lines.
        /// Control uses indentation to render a bulleted item. The
        /// content of the paragraph, including the first line, starts at the
        /// leftIndent plus the leftSubIndent.
        /// </remarks>
        public bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0)
        {
            return NativeControl.BeginLeftIndent(leftIndent, leftSubIndent);
        }

        /// <summary>
        /// Ends left indent.
        /// </summary>
        public bool EndLeftIndent()
        {
            return NativeControl.EndLeftIndent();
        }

        /// <summary>
        /// Begins a right indent, specified in tenths of a millimetre.
        /// </summary>
        public bool BeginRightIndent(int rightIndent)
        {
            return NativeControl.BeginRightIndent(rightIndent);
        }

        /// <summary>
        /// Ends right indent.
        /// </summary>
        public bool EndRightIndent()
        {
            return NativeControl.EndRightIndent();
        }

        /// <summary>
        /// Begins paragraph spacing; pass the before-paragraph and after-paragraph spacing
        /// in tenths of a millimetre.
        /// </summary>
        public bool BeginParagraphSpacing(int before, int after)
        {
            return NativeControl.BeginParagraphSpacing(before, after);
        }

        /// <summary>
        /// Ends paragraph spacing.
        /// </summary>
        public bool EndParagraphSpacing()
        {
            return NativeControl.EndParagraphSpacing();
        }

        /// <summary>
        /// Begins applying line spacing.
        /// </summary>
        /// <param name="lineSpacing">A multiple, where 10 means
        /// single-spacing, 15 means 1.5 spacing, and 20 means double spacing.</param>
        /// <remarks>
        /// The <see cref="TextBoxTextAttrLineSpacing"/> constants are defined for convenience.
        /// </remarks>
        /// <returns></returns>
        public bool BeginLineSpacing(int lineSpacing)
        {
            return NativeControl.BeginLineSpacing(lineSpacing);
        }

        /// <summary>
        /// Ends line spacing.
        /// </summary>
        public bool EndLineSpacing()
        {
            return NativeControl.EndLineSpacing();
        }

        /// <summary>
        /// Begins a numbered bullet.
        /// </summary>
        /// <param name="bulletNumber">A number, usually starting with 1</param>
        /// <param name="leftIndent">A value in tenths of a millimetre.</param>
        /// <param name="leftSubIndent">A value in tenths of a millimetre.</param>
        /// <param name="bulletStyle">Bullet style.</param>
        /// <remarks>
        /// This call will be needed for each item in the list, and the
        /// application should take care of incrementing the numbering.
        /// </remarks>
        /// <remarks>
        /// Control uses indentation to render a bulleted item.
        /// The left indent is the distance between the margin and the bullet.
        /// The content of the paragraph, including the first line, starts
        /// at leftMargin + leftSubIndent.
        /// So the distance between the left edge of the bullet and the
        /// left of the actual paragraph is leftSubIndent.
        /// </remarks>
        /// <returns></returns>
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

        /// <summary>
        /// Ends application of a numbered bullet.
        /// </summary>
        public bool EndNumberedBullet()
        {
            return NativeControl.EndNumberedBullet();
        }

        /// <summary>
        /// Begins applying a symbol bullet, using a character from the current font.
        /// See <see cref="BeginNumberedBullet"/> for an explanation of how indentation is used
        /// to render the bulleted paragraph.
        /// </summary>
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

        /// <summary>
        /// Ends applying a symbol bullet.
        /// </summary>
        public bool EndSymbolBullet()
        {
            return NativeControl.EndSymbolBullet();
        }

        /// <summary>
        /// Begins applying a symbol bullet.
        /// </summary>
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

        /// <summary>
        /// Begins applying a standard bullet.
        /// </summary>
        public bool EndStandardBullet()
        {
            return NativeControl.EndStandardBullet();
        }

        /// <summary>
        /// Begins using the named character style.
        /// </summary>
        public bool BeginCharacterStyle(string characterStyle)
        {
            return NativeControl.BeginCharacterStyle(characterStyle);
        }

        /// <summary>
        /// Ends application of a named character style.
        /// </summary>
        public bool EndCharacterStyle()
        {
            return NativeControl.EndCharacterStyle();
        }

        /// <summary>
        /// Begins applying the named paragraph style.
        /// </summary>
        public bool BeginParagraphStyle(string paragraphStyle)
        {
            return NativeControl.BeginParagraphStyle(paragraphStyle);
        }

        /// <summary>
        /// Ends application of a named paragraph style.
        /// </summary>
        public bool EndParagraphStyle()
        {
            return NativeControl.EndParagraphStyle();
        }

        /// <summary>
        /// Begins using a specified list style.
        /// Optionally, you can also pass a level and a number.
        /// </summary>
        public bool BeginListStyle(string listStyle, int level = 1, int number = 1)
        {
            return NativeControl.BeginListStyle(listStyle, level, number);
        }

        /// <summary>
        /// Ends using a specified list style.
        /// </summary>
        public bool EndListStyle()
        {
            return NativeControl.EndListStyle();
        }

        //!!!!!!!!!!!!!!!!!!!!

        /// <summary>
        /// 
        /// </summary>
        /// Begins applying wxTEXT_ATTR_URL to the content.
        /// Pass a URL and optionally, a character style to apply, since it is common
        /// to mark a URL with a familiar style such as blue text with underlining.
        public bool BeginURL(string url, string? characterStyle = default)
        {
            characterStyle ??= string.Empty;
            return NativeControl.BeginURL(url, characterStyle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Ends applying a URL.
        public bool EndURL()
        {
            return NativeControl.EndURL();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is bold.
        public bool IsSelectionBold()
        {
            return NativeControl.IsSelectionBold();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is italic.
        public bool IsSelectionItalics()
        {
            return NativeControl.IsSelectionItalics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is underlined.
        public bool IsSelectionUnderlined()
        {
            return NativeControl.IsSelectionUnderlined();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the current caret position, has the supplied effects flag(s).
        public bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag)
        {
            return NativeControl.DoesSelectionHaveTextEffectFlag((int)flag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is aligned according to the specified flag.
        public bool IsSelectionAligned(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.IsSelectionAligned((int)alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Apples bold to the selection or default style (undoable).
        public bool ApplyBoldToSelection()
        {
            return NativeControl.ApplyBoldToSelection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Applies italic to the selection or default style (undoable).
        public bool ApplyItalicToSelection()
        {
            return NativeControl.ApplyItalicToSelection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Applies underline to the selection or default style (undoable).
        public bool ApplyUnderlineToSelection()
        {
            return NativeControl.ApplyUnderlineToSelection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Applies one or more wxTextAttrEffects flags to the selection (undoable).
        /// If there is no selection, it is applied to the default style.
        public bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags)
        {
            return NativeControl.ApplyTextEffectToSelection((int)flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Applies the given alignment to the selection or the default style (undoable).
        /// For alignment values, see wxTextAttr.
        public bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.ApplyAlignmentToSelection((int)alignment);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Sets the default style to the style under the cursor.
        public bool SetDefaultStyleToCursorStyle()
        {
            return NativeControl.SetDefaultStyleToCursorStyle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Cancels any selection.
        public void SelectNone()
        {
            NativeControl.SelectNone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// Selects the word at the given character position.
        public bool SelectWord(long position)
        {
            return NativeControl.SelectWord(position);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Lays out the buffer, which must be done before certain operations, such as
        /// setting the caret position.
        /// This function should not normally be required by the application.
        public bool LayoutContent(bool onlyVisibleRect = false)
        {
            return NativeControl.LayoutContent(onlyVisibleRect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Moves right.
        public bool MoveRight(int noPositions = 1, int flags = 0)
        {
            return NativeControl.MoveRight(noPositions, flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Moves left.
        public bool MoveLeft(int noPositions = 1, int flags = 0)
        {
            return NativeControl.MoveLeft(noPositions, flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Moves to the start of the paragraph.
        public bool MoveUp(int noLines = 1, int flags = 0)
        {
            return NativeControl.MoveUp(noLines, flags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// Moves the caret down.
        public bool MoveDown(int noLines = 1, int flags = 0)
        {
            return NativeControl.MoveDown(noLines, flags);
        }

        /// Moves to the end of the line.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveToLineEnd(int flags = 0)
        {
            return NativeControl.MoveToLineEnd(flags);
        }

        /// Moves to the start of the line.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveToLineStart(int flags = 0)
        {
            return NativeControl.MoveToLineStart(flags);
        }

        /// Moves to the end of the paragraph.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveToParagraphEnd(int flags = 0)
        {
            return NativeControl.MoveToParagraphEnd(flags);
        }

        /// Moves to the start of the paragraph.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveToParagraphStart(int flags = 0)
        {
            return NativeControl.MoveToParagraphStart(flags);
        }

        /// Moves to the start of the buffer.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveHome(int flags = 0)
        {
            return NativeControl.MoveHome(flags);
        }

        /// Moves to the end of the buffer.
        /// <summary>
        /// 
        /// </summary>
        public bool MoveEnd(int flags = 0)
        {
            return NativeControl.MoveEnd(flags);
        }

        /// Moves one or more pages up.
        /// <summary>
        /// 
        /// </summary>
        public bool PageUp(int noPages = 1, int flags = 0)
        {
            return NativeControl.PageUp(noPages, flags);
        }

        /// Moves one or more pages down.
        /// <summary>
        /// 
        /// </summary>
        public bool PageDown(int noPages = 1, int flags = 0)
        {
            return NativeControl.PageDown(noPages, flags);
        }

        /// Moves a number of words to the left.
        /// <summary>
        /// 
        /// </summary>
        public bool WordLeft(int noPages = 1, int flags = 0)
        {
            return NativeControl.WordLeft(noPages, flags);
        }

        /// Move a number of words to the right.
        /// <summary>
        /// 
        /// </summary>
        public bool WordRight(int noPages = 1, int flags = 0)
        {
            return NativeControl.WordRight(noPages, flags);
        }

        /// Starts batching undo history for commands.
        /// <summary>
        /// 
        /// </summary>
        public bool BeginBatchUndo(string cmdName)
        {
            return NativeControl.BeginBatchUndo(cmdName);
        }

        /// Ends batching undo command history.
        /// <summary>
        /// 
        /// </summary>
        public bool EndBatchUndo()
        {
            return NativeControl.EndBatchUndo();
        }

        /// Returns <c>true</c> if undo commands are being batched.
        /// <summary>
        /// 
        /// </summary>
        public bool BatchingUndo()
        {
            return NativeControl.BatchingUndo();
        }

        /// Starts suppressing undo history for commands.
        /// <summary>
        /// 
        /// </summary>
        public bool BeginSuppressUndo()
        {
            return NativeControl.BeginSuppressUndo();
        }

        /// Ends suppressing undo command history.
        /// <summary>
        /// 
        /// </summary>
        public bool EndSuppressUndo()
        {
            return NativeControl.EndSuppressUndo();
        }

        /// Returns <c>true</c> if undo history suppression is on.
        /// <summary>
        /// 
        /// </summary>
        public bool SuppressingUndo()
        {
            return NativeControl.SuppressingUndo();
        }

        /// Enable or disable the vertical scrollbar.
        /// <summary>
        /// 
        /// </summary>
        public void EnableVerticalScrollbar(bool enable)
        {
            NativeControl.EnableVerticalScrollbar(enable);
        }

        /// Returns <c>true</c> if the vertical scrollbar is enabled.
        /// <summary>
        /// 
        /// </summary>
        public bool GetVerticalScrollbarEnabled()
        {
            return NativeControl.GetVerticalScrollbarEnabled();
        }

        /// Sets the scale factor for displaying fonts, for example for more comfortableediting.
        /// <summary>
        /// 
        /// </summary>
        public void SetFontScale(double fontScale, bool refresh = false)
        {
            NativeControl.SetFontScale(fontScale, refresh);
        }

        /// Returns the scale factor for displaying fonts, for example for more comfortable editing.
        /// <summary>
        /// 
        /// </summary>
        public double GetFontScale()
        {
            return NativeControl.GetFontScale();
        }

        /// Returns <c>true</c> if this control can use attributes and text. The default is @false.
        /// <summary>
        /// 
        /// </summary>
        public bool GetVirtualAttributesEnabled()
        {
            return NativeControl.GetVirtualAttributesEnabled();
        }

        /// Pass <c>true</c> to let the control use attributes. The default is @false.
        /// <summary>
        /// 
        /// </summary>
        public void EnableVirtualAttributes(bool b)
        {
            NativeControl.EnableVirtualAttributes(b);
        }

        /// Write text
        /// <summary>
        /// 
        /// </summary>
        public void DoWriteText(string value, int flags = 0)
        {
            NativeControl.DoWriteText(value, flags);
        }

        /// Helper function for extending the selection, returning <c>true</c> if the selection
        /// was changed. Selections are in caret positions.
        /// <summary>
        /// 
        /// </summary>
        public bool ExtendSelection(long oldPosition, long newPosition, int flags)
        {
            return NativeControl.ExtendSelection(oldPosition, newPosition, flags);
        }

        /// Sets the caret position.
        /// The caret position is the character position just before the caret.
        /// A value of -1 means the caret is at the start of the buffer.
        /// Please note that this does not update the current editing style
        /// from the new position or cause the actual caret to be refreshed; to do that,
        /// call SetInsertionPoint instead.
        /// <summary>
        /// 
        /// </summary>
        public void SetCaretPosition(long position, bool showAtLineStart = false)
        {
            NativeControl.SetCaretPosition(position, showAtLineStart);
        }

        /// Returns the current caret position.
        /// <summary>
        /// 
        /// </summary>
        public long GetCaretPosition()
        {
            return NativeControl.GetCaretPosition();
        }

        /// The adjusted caret position is the character position adjusted to take
        /// into account whether we're at the start of a paragraph, in which case
        /// style information should be taken from the next position, not current one.
        /// <summary>
        /// 
        /// </summary>
        public long GetAdjustedCaretPosition(long caretPos)
        {
            return NativeControl.GetAdjustedCaretPosition(caretPos);
        }

        /// Move the caret one visual step forward: this may mean setting a flag
        /// and keeping the same position if we're going from the end of one line
        /// to the start of the next, which may be the exact same caret position.
        /// <summary>
        /// 
        /// </summary>
        public void MoveCaretForward(long oldPosition)
        {
            NativeControl.MoveCaretForward(oldPosition);
        }

        /// Transforms logical (unscrolled) position to physical window position.
        /// <summary>
        /// 
        /// </summary>
        public Int32Point GetPhysicalPoint(Int32Point ptLogical)
        {
            return NativeControl.GetPhysicalPoint(ptLogical);
        }

        /// Transforms physical window position to logical (unscrolled) position.
        /// <summary>
        /// 
        /// </summary>
        public Int32Point GetLogicalPoint(Int32Point ptPhysical)
        {
            return NativeControl.GetLogicalPoint(ptPhysical);
        }

        /// Helper function for finding the caret position for the next word.
        /// Direction is 1 (forward) or -1 (backwards).
        /// <summary>
        /// 
        /// </summary>
        public long FindNextWordPosition(int direction = 1)
        {
            return NativeControl.FindNextWordPosition(direction);
        }

        /// Returns <c>true</c> if the given position is visible on the screen.
        /// <summary>
        /// 
        /// </summary>
        public bool IsPositionVisible(long pos)
        {
            return NativeControl.IsPositionVisible(pos);
        }

        /// Returns the first visible position in the current view.
        /// <summary>
        /// 
        /// </summary>
        public long GetFirstVisiblePosition()
        {
            return NativeControl.GetFirstVisiblePosition();
        }

        /// Returns the caret position since the default formatting was changed. As
        /// soon as this position changes, we no longer reflect the default style
        /// in the UI. A value of -2 means that we should only reflect the style of the
        /// content under the caret.
        /// <summary>
        /// 
        /// </summary>
        public long GetCaretPositionForDefaultStyle()
        {
            return NativeControl.GetCaretPositionForDefaultStyle();
        }

        /// Set the caret position for the default style that the user is selecting.
        /// <summary>
        /// 
        /// </summary>
        public void SetCaretPositionForDefaultStyle(long pos)
        {
            NativeControl.SetCaretPositionForDefaultStyle(pos);
        }

        /// Move the caret one visual step forward: this may mean setting a flag
        /// and keeping the same position if we're going from the end of one line
        /// to the start of the next, which may be the exact same caret position.
        /// <summary>
        /// 
        /// </summary>
        public void MoveCaretBack(long oldPosition)
        {
            NativeControl.MoveCaretBack(oldPosition);
        }

        /// Returns <c>true</c> if the user has recently set the default style without moving
        /// the caret, and therefore the UI needs to reflect the default style and not
        /// the style at the caret.
        /// <summary>
        /// 
        /// </summary>
        public bool IsDefaultStyleShowing()
        {
            return NativeControl.IsDefaultStyleShowing();
        }

        /// Returns the first visible point in the window.
        /// <summary>
        /// 
        /// </summary>
        public Int32Point GetFirstVisiblePoint()
        {
            return NativeControl.GetFirstVisiblePoint();
        }

        /// Enable or disable images
        /// <summary>
        /// 
        /// </summary>
        public void EnableImages(bool b)
        {
            NativeControl.EnableImages(b);
        }

        /// Returns <c>true</c> if images are enabled.
        /// <summary>
        /// 
        /// </summary>
        public bool GetImagesEnabled()
        {
            return NativeControl.GetImagesEnabled();
        }

        /// Enable or disable delayed image loading.
        /// <summary>
        /// 
        /// </summary>
        public void EnableDelayedImageLoading(bool b)
        {
            NativeControl.EnableDelayedImageLoading(b);
        }

        /// Returns <c>true</c> if delayed image loading is enabled.
        /// <summary>
        /// 
        /// </summary>
        public bool GetDelayedImageLoading()
        {
            return NativeControl.GetDelayedImageLoading();
        }

        /// Gets the flag indicating that delayed image processing is required.
        /// <summary>
        /// 
        /// </summary>
        public bool GetDelayedImageProcessingRequired()
        {
            return NativeControl.GetDelayedImageProcessingRequired();
        }

        /// Sets the flag indicating that delayed image processing is required.
        /// <summary>
        /// 
        /// </summary>
        public void SetDelayedImageProcessingRequired(bool b)
        {
            NativeControl.SetDelayedImageProcessingRequired(b);
        }

        /// Returns the last time delayed image processing was performed.
        /// <summary>
        /// 
        /// </summary>
        public long GetDelayedImageProcessingTime()
        {
            return NativeControl.GetDelayedImageProcessingTime();
        }

        /// Sets the last time delayed image processing was performed.
        /// <summary>
        /// 
        /// </summary>
        public void SetDelayedImageProcessingTime(long t)
        {
            NativeControl.SetDelayedImageProcessingTime(t);
        }

        /// Returns the content of the entire control as a string.
        /// <summary>
        /// 
        /// </summary>
        public string GetValue()
        {
            return NativeControl.GetValue();
        }

        /// Replaces existing content with the given text.
        /// <summary>
        /// 
        /// </summary>
        public void SetValue(string value)
        {
            NativeControl.SetValue(value);
        }

        /// Set the line increment height in pixels
        /// <summary>
        /// 
        /// </summary>
        public void SetLineHeight(int height)
        {
            NativeControl.SetLineHeight(height);
        }

        public int GetLineHeight()
        {
            return NativeControl.GetLineHeight();
        }

        /// Do delayed image loading and garbage-collect other images.
        /// <summary>
        /// 
        /// </summary>
        public bool ProcessDelayedImageLoading(bool refresh)
        {
            return NativeControl.ProcessDelayedImageLoading(refresh);
        }

        /// Request delayed image processing.
        /// <summary>
        /// 
        /// </summary>
        public void RequestDelayedImageProcessing()
        {
            NativeControl.RequestDelayedImageProcessing();
        }

        /// Returns the last position in the buffer.
        /// <summary>
        /// 
        /// </summary>
        public long GetLastPosition()
        {
            return NativeControl.GetLastPosition();
        }

        internal bool SetListStyle(
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
        } /// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Clears the list style from the given range, clearing list-related attributes
        /// and applying any named paragraph style associated with each paragraph.
        /// @a flags is a bit list of the following:
        /// - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        internal bool ClearListStyle(long startRange, long endRange, int flags)
        {
            return NativeControl.ClearListStyle(startRange, endRange, flags);
        }/// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        internal bool NumberList(
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
        }/// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        internal bool PromoteList(
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
        } /// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Deletes the content within the given range.
        /// <summary>
        /// 
        /// </summary>
        public bool Delete(long startRange, long endRange)
        {
            return NativeControl.Delete(startRange, endRange);
        }

        /// <summary>
        /// Begins applying a style.
        /// </summary>
        public bool BeginStyle(ITextBoxRichAttr style)
        {
            if (style is not TextBoxRichAttr s)
                return false;
            return NativeControl.BeginStyle(s.Handle);
        }

        /// Sets the selection to the given range.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// So, for example, to set the selection for a character at position 5, use the
        /// range (5,6).
        /// <summary>
        /// 
        /// </summary>
        public void SetSelectionRange(long startRange, long endRange)
        {
            NativeControl.SetSelectionRange(startRange, endRange);
        }

        /// Converts a text position to zero-based column and line numbers.
        /// <summary>
        /// 
        /// </summary>
        public Int32Point PositionToXY(long pos)
        {
            return NativeControl.PositionToXY(pos);
        }

        /// Deletes content if there is a selection, e.g. when pressing a key.
        /// Returns the new caret position in @e newPos, or leaves it if there
        /// was no action. This is undoable.
        /// <summary>
        /// 
        /// </summary>
        public long DeleteSelectedContent()
        {
            return NativeControl.DeleteSelectedContent();
        }

        /// Returns the current context menu.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetContextMenu()
        {
            return NativeControl.GetContextMenu();
        }

        /// Sets the current context menu.
        /// <summary>
        /// 
        /// </summary>
        internal void SetContextMenu(IntPtr menu)
        {
            NativeControl.SetContextMenu(menu);
        }

        /// Returns the anchor object if selecting multiple containers.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetSelectionAnchorObject()
        {
            return NativeControl.GetSelectionAnchorObject();
        }

        /// Sets the anchor object if selecting multiple containers.
        /// <summary>
        /// 
        /// </summary>
        internal void SetSelectionAnchorObject(IntPtr anchor)
        {
            NativeControl.SetSelectionAnchorObject(anchor);
        }

        /// Returns the wxRichTextObject object that currently has the editing focus.
        /// If there are no composite objects, this will be the top-level buffer.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetFocusObject()
        {
            return NativeControl.GetFocusObject();
        }

        /// Sets m_focusObject without making any alterations.
        /// <summary>
        /// 
        /// </summary>
        internal void StoreFocusObject(IntPtr richObj)
        {
            NativeControl.StoreFocusObject(richObj);
        }

        /// Begins using this font.
        /// <summary>
        /// 
        /// </summary>
        internal bool BeginFont(Font? font)
        {
            return NativeControl.BeginFont(font?.NativeFont);
        }

        /// Applies the style sheet to the buffer, matching paragraph styles in the sheet
        /// against named styles in the buffer.
        /// This might be useful if the styles have changed.
        /// If @a sheet is @NULL, the sheet set with SetStyleSheet() is used.
        /// Currently this applies paragraph styles only.
        /// <summary>
        /// 
        /// </summary>
        internal bool ApplyStyle(IntPtr def)
        {
            return NativeControl.ApplyStyle(def);
        }

        /// Sets the style sheet associated with the control.
        /// A style sheet allows named character and paragraph styles to be applied.
        /// <summary>
        /// 
        /// </summary>
        internal void SetStyleSheet(IntPtr styleSheet)
        {
            NativeControl.SetStyleSheet(styleSheet);
        }

        /// Move the caret to the given character position.
        /// Please note that this does not update the current editing style
        /// from the new position; to do that, call SetInsertionPoint instead.
        /// <summary>
        /// 
        /// </summary>
        internal bool MoveCaret(long pos, bool showAtLineStart = false, IntPtr container = default)
        {
            return NativeControl.MoveCaret(pos, showAtLineStart, container);
        }

        /// Push the style sheet to top of stack.
        /// <summary>
        /// 
        /// </summary>
        internal bool PushStyleSheet(IntPtr styleSheet)
        {
            return NativeControl.PushStyleSheet(styleSheet);
        }

        /// Pops the style sheet from top of stack.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr PopStyleSheet()
        {
            return NativeControl.PopStyleSheet();
        }

        /// Applies the style sheet to the buffer, for example if the styles have changed.
        /// <summary>
        /// 
        /// </summary>
        internal bool ApplyStyleSheet(IntPtr styleSheet = default)
        {
            return NativeControl.ApplyStyleSheet(styleSheet);
        }

        /// Shows the given context menu, optionally adding appropriate property-editing commands for the current position in the object hierarchy.
        /// <summary>
        /// 
        /// </summary>
        internal bool ShowContextMenu(IntPtr menu, Int32Point pt, bool addPropertyCommands = true)
        {
            return NativeControl.ShowContextMenu(menu, pt, addPropertyCommands);
        }

        /// Prepares the context menu, optionally adding appropriate property-editing commands.
        /// Returns the number of property commands added.
        /// <summary>
        /// 
        /// </summary>
        internal int PrepareContextMenu(IntPtr menu, Int32Point pt, bool addPropertyCommands = true)
        {
            return NativeControl.PrepareContextMenu(menu, pt, addPropertyCommands);
        }

        /// Returns <c>true</c> if we can edit the object's properties via a GUI.
        /// <summary>
        /// 
        /// </summary>
        internal bool CanEditProperties(IntPtr richObj)
        {
            return NativeControl.CanEditProperties(richObj);
        }

        /// Edits the object's properties via a GUI.
        /// <summary>
        /// 
        /// </summary>
        internal bool EditProperties(IntPtr richObj, IntPtr parentWindow)
        {
            return NativeControl.EditProperties(richObj, parentWindow);
        }

        /// Extends a table selection in the given direction.
        /// <summary>
        /// 
        /// </summary>
        internal bool ExtendCellSelection(IntPtr table, int noRowSteps, int noColSteps)
        {
            return NativeControl.ExtendCellSelection(table, noRowSteps, noColSteps);
        }

        /// Starts selecting table cells.
        /// <summary>
        /// 
        /// </summary>
        internal bool StartCellSelection(IntPtr table, IntPtr newCell)
        {
            return NativeControl.StartCellSelection(table, newCell);
        }

        /// Scrolls @a position into view. This function takes a caret position.
        /// <summary>
        /// 
        /// </summary>
        internal bool ScrollIntoView(long position, int keyCode)
        {
            return NativeControl.ScrollIntoView(position, keyCode);
        }

        /// Returns the caret height and position for the given character position.
        /// If container is null, the current focus object will be used.
        /// <summary>
        /// 
        /// </summary>
        internal bool GetCaretPositionForIndex(
            long position,
            Int32Rect rect,
            IntPtr container = default)
        {
            return NativeControl.GetCaretPositionForIndex(position, rect, container);
        }

        /// Internal helper function returning the line for the visible caret position.
        /// If the caret is shown at the very end of the line, it means the next character
        /// is actually on the following line.
        /// So this function gets the line we're expecting to find if this is the case.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetVisibleLineForCaretPosition(long caretPosition)
        {
            return NativeControl.GetVisibleLineForCaretPosition(caretPosition);
        }

        /// Gets the command processor associated with the control's buffer.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetCommandProcessor()
        {
            return NativeControl.GetCommandProcessor();
        }

        /// Sets up the caret for the given position and container, after a mouse click.
        /// <summary>
        /// 
        /// </summary>
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

        /// Gets the attributes at the given position.
        /// This function gets the @e uncombined style - that is, the attributes associated
        /// with the paragraph or character content, and not necessarily the combined
        /// attributes you see on the screen.
        /// To get the combined attributes, use GetStyle().
        /// If you specify (any) paragraph attribute in @e style's flags, this function
        /// will fetch the paragraph attributes.
        /// Otherwise, it will return the character attributes.
        /// <summary>
        /// 
        /// </summary>
        internal bool GetUncombinedStyle(long position, IntPtr style)
        {
            return NativeControl.GetUncombinedStyle(position, style);
        }

        internal bool GetUncombinedStyle(long position, IntPtr style, IntPtr container)
        {
            return NativeControl.GetUncombinedStyle2(position, style, container);
        }

        internal bool SetDefaultRichStyle(IntPtr style)
        {
            return NativeControl.SetDefaultRichStyle(style);
        }

        /// Returns the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetDefaultStyleEx()
        {
            return NativeControl.GetDefaultStyleEx();
        }

        /// Gets the attributes at the given position.
        /// This function gets the combined style - that is, the style you see on the
        /// screen as a result of combining base style, paragraph style and character
        /// style attributes.
        /// To get the character or paragraph style alone, use GetUncombinedStyle().
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        /// Sets the attributes for the given range.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        internal bool SetStyle(long start, long end, IntPtr style)
        {
            return NativeControl.SetStyle(start, end, style);
        }

        internal bool SetRichStyle(long start, long end, IntPtr style)
        {
            return NativeControl.SetRichStyle(start, end, style);
        }

        /// Sets the attributes for a single object
        /// <summary>
        /// 
        /// </summary>
        internal void SetStyle(IntPtr richObj, IntPtr textAttr, int flags)
        {
            NativeControl.SetStyle2(richObj, textAttr, flags);
        }/// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Gets the attributes common to the specified range.
        /// Attributes that differ in value within the range will not be included
        /// in @a style flags.
        /// <summary>
        /// 
        /// </summary>
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

        /// Sets the attributes for the given range, passing flags to determine how the
        /// attributes are set.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// @a flags may contain a bit list of the following values:
        /// - wxRICHTEXT_SETSTYLE_NONE: no style flag.
        /// - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this operation should be
        ///   undoable.
        /// - wxRICHTEXT_SETSTYLE_OPTIMIZE: specifies that the style should not be applied
        ///   if the combined style at this point is already the style in question.
        /// - wxRICHTEXT_SETSTYLE_PARAGRAPHS_ONLY: specifies that the style should only be
        ///   applied to paragraphs, and not the content.
        ///   This allows content styling to be preserved independently from that
        ///   of e.g. a named paragraph style.
        /// - wxRICHTEXT_SETSTYLE_CHARACTERS_ONLY: specifies that the style should only be
        ///   applied to characters, and not the paragraph.
        ///   This allows content styling to be preserved independently from that
        ///   of e.g. a named paragraph style.
        /// - wxRICHTEXT_SETSTYLE_RESET: resets (clears) the existing style before applying
        ///   the new style.
        /// - wxRICHTEXT_SETSTYLE_REMOVE: removes the specified style. Only the style flags
        ///   are used in this operation.
        /// <summary>
        /// 
        /// </summary>
        internal bool SetStyleEx(long startRange, long endRange, IntPtr style, int flags)
        {
            return NativeControl.SetStyleEx(startRange, endRange, style, flags);
        } /// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Sets the list attributes for the given range, passing flags to determine how
        /// the attributes are set.
        /// Either the style definition or the name of the style definition (in the current
        /// sheet) can be passed.
        /// @a flags is a bit list of the following:
        /// - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        /// - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        ///   @a startFrom, otherwise existing attributes are used.
        /// - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        ///   as the level for all paragraphs, otherwise the current indentation will be used.
        /// <summary>
        /// 
        /// </summary>
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
        } /// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Numbers the paragraphs in the given range.
        /// Pass flags to determine how the attributes are set.
        /// Either the style definition or the name of the style definition (in the current
        /// sheet) can be passed.
        /// @a flags is a bit list of the following:
        /// - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        /// - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        ///   @a startFrom, otherwise existing attributes are used.
        /// - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// <summary>
        /// 
        /// </summary>
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
        }/// = wxRICHTEXT_SETSTYLE_WITH_UNDO, def = default

        /// Promotes or demotes the paragraphs in the given range.
        /// A positive @a promoteBy produces a smaller indent, and a negative number
        /// produces a larger indent. Pass flags to determine how the attributes are set.
        /// Either the style definition or the name of the style definition (in the current
        /// sheet) can be passed.
        /// @a flags is a bit list of the following:
        /// - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        /// - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
        ///   @a startFrom, otherwise existing attributes are used.
        /// - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// <summary>
        /// 
        /// </summary>
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
        } /// def = default, = wxRICHTEXT_SETSTYLE_WITH_UNDO

        /// Write a table at the current insertion point, returning the table.
        /// You can then call SetFocusObject() to set the focus to the new object.
        /// <summary>
        /// 
        /// </summary>
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

        /// Sets the basic (overall) style.
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// <summary>
        /// 
        /// </summary>
        internal void SetBasicStyle(IntPtr style)
        {
            NativeControl.SetBasicStyle(style);
        }

        /// Gets the basic (overall) style.
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetBasicStyle()
        {
            return NativeControl.GetBasicStyle();
        }

        /// Test if this whole range has character attributes of the specified kind.
        /// If any of the attributes are different within the range, the test fails.
        /// You can use this to implement, for example, bold button updating.
        /// @a style must have flags indicating which attributes are of interest.
        /// <summary>
        /// 
        /// </summary>
        internal bool HasCharacterAttributes(long startRange, long endRange, IntPtr style)
        {
            return NativeControl.HasCharacterAttributes(startRange, endRange, style);
        }

        /// Returns the style sheet associated with the control, if any.
        /// A style sheet allows named character and paragraph styles to be applied.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetStyleSheet()
        {
            return NativeControl.GetStyleSheet();
        }

        /// Sets @a attr as the default style and tells the control that the UI should
        /// reflect this attribute until the user moves the caret.
        /// <summary>
        /// 
        /// </summary>
        internal void SetAndShowDefaultStyle(IntPtr attr)
        {
            NativeControl.SetAndShowDefaultStyle(attr);
        }

        /// Write a text box at the current insertion point, returning the text box.
        /// You can then call SetFocusObject() to set the focus to the new object.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr WriteTextBox(IntPtr textAttr = default)
        {
            return NativeControl.WriteTextBox(textAttr);
        }

        /// Test if this whole range has paragraph attributes of the specified kind.
        /// If any of the attributes are different within the range, the test fails.
        /// You can use this to implement, for example, centering button updating.
        /// @a style must have flags indicating which attributes are of interest.
        /// <summary>
        /// 
        /// </summary>
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

        /// Sets the properties for the given range, passing flags to determine how the
        /// attributes are set. You can merge properties or replace them.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the properties for a character at
        /// position 5, use the range (5,6).
        /// @a flags may contain a bit list of the following values:
        /// - wxRICHTEXT_SETSPROPERTIES_NONE: no flag.
        /// - wxRICHTEXT_SETPROPERTIES_WITH_UNDO: specifies that this operation should be
        ///   undoable.
        /// - wxRICHTEXT_SETPROPERTIES_PARAGRAPHS_ONLY: specifies that the properties should only be
        ///   applied to paragraphs, and not the content.
        /// - wxRICHTEXT_SETPROPERTIES_CHARACTERS_ONLY: specifies that the properties should only be
        ///   applied to characters, and not the paragraph.
        /// - wxRICHTEXT_SETPROPERTIES_RESET: resets (clears) the existing properties before applying
        ///   the new properties.
        /// - wxRICHTEXT_SETPROPERTIES_REMOVE: removes the specified properties.
        /// <summary>
        /// 
        /// </summary>
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
        } /// = wxRICHTEXT_SETPROPERTIES_WITH_UNDO

        /// Sets the text (normal) cursor.
        /// <summary>
        /// 
        /// </summary>
        internal void SetTextCursor(IntPtr cursor)
        {
            NativeControl.SetTextCursor(cursor);
        }

        /// Returns the text (normal) cursor.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetTextCursor()
        {
            return NativeControl.GetTextCursor();
        }

        /// Sets the cursor to be used over URLs.
        /// <summary>
        /// 
        /// </summary>
        internal void SetURLCursor(IntPtr cursor)
        {
            NativeControl.SetURLCursor(cursor);
        }

        /// Returns the cursor to be used over URLs.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetURLCursor()
        {
            return NativeControl.GetURLCursor();
        }

        /// Returns the range of the current selection.
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// If the return values @a from and @a to are the same, there is no selection.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetSelection()
        {
            return NativeControl.GetSelection();
        }

        /// Returns an object that stores information about context menu property item(s),
        /// in order to communicate between the context menu event handler and the code
        /// that responds to it. The wxRichTextContextMenuPropertiesInfo stores one
        /// item for each object that could respond to a property-editing event. If
        /// objects are nested, several might be editable.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetContextMenuPropertiesInfo()
        {
            return NativeControl.GetContextMenuPropertiesInfo();
        }

        internal void SetSelection(IntPtr sel)
        {
            NativeControl.SetSelection2(sel);
        }

        public bool WriteImage(
            Image bitmap,
            BitmapType bitmapType = BitmapType.Png)
        {
            if (bitmap is null)
                return false;
            return NativeControl.WriteImage(
                bitmap.NativeImage,
                (int)bitmapType,
                default);
        }

        /// Write a bitmap or image at the current insertion point.
        /// Supply an optional type to use for internal and file storage of the raw data.
        /// <summary>
        /// 
        /// </summary>
        internal bool WriteImage(
            Image bitmap,
            BitmapType bitmapType /*= BitmapType.Png*/,
            IntPtr textAttr)
        {
            if (bitmap is null)
                return false;
            return NativeControl.WriteImage(
                bitmap.NativeImage,
                (int)bitmapType,
                textAttr);
        }

        /// Loads an image from a file and writes it at the current insertion point.
        /// <summary>
        /// 
        /// </summary>
        internal bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            IntPtr textAttr = default)
        {
            return NativeControl.WriteImage2(
                filename,
                (int)bitmapType,
                textAttr);
        }

        /// Writes an image block at the current insertion point.
        /// <summary>
        /// 
        /// </summary>
        internal bool WriteImage(IntPtr imageBlock, IntPtr textAttr = default)
        {
            return NativeControl.WriteImage3(imageBlock, textAttr);
        }

        /// Writes a field at the current insertion point.
        ///   @param fieldType
        ///       The field type, matching an existing field type definition.
        ///   @param properties
        ///       Extra data for the field.
        ///  @param textAttr
        ///      Optional attributes.
        /// <summary>
        /// 
        /// </summary>
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

        /// Can we delete this range?
        /// Sends an event to the control.
        /// <summary>
        /// 
        /// </summary>
        internal bool CanDeleteRange(IntPtr container, long startRange, long endRange)
        {
            return NativeControl.CanDeleteRange(container, startRange, endRange);
        }

        /// Can we insert content at this position?
        /// Sends an event to the control.
        /// <summary>
        /// 
        /// </summary>
        internal bool CanInsertContent(IntPtr container, long pos)
        {
            return NativeControl.CanInsertContent(container, pos);
        }

        /// Returns the buffer associated with the control.
        /// <summary>
        /// 
        /// </summary>
        internal IntPtr GetBuffer()
        {
            return NativeControl.GetBuffer();
        }

        /// Gets the object's properties menu label.
        /// <summary>
        /// 
        /// </summary>
        internal string GetPropertiesMenuLabel(IntPtr richObj)
        {
            return NativeControl.GetPropertiesMenuLabel(richObj);
        }

        /// Sets the wxRichTextObject object that currently has the editing focus.
        /// <summary>
        /// 
        /// </summary>
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