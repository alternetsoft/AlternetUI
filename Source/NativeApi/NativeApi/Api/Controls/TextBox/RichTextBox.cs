#pragma warning disable
using NativeApi.Api.ManagedServers;
using ApiCommon;
using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    // https://docs.wxwidgets.org/3.2/classwx_rich_text_ctrl.html
    // https://docs.wxwidgets.org/3.2/classwx_rich_text_buffer.html
    // https://docs.wxwidgets.org/3.2/classwx_rich_text_file_handler.html
    // https://docs.wxwidgets.org/3.2/classwx_rich_text_x_m_l_handler.html
    // https://docs.wxwidgets.org/3.2/classwx_rich_text_h_t_m_l_handler.html
    public class RichTextBox : Control
    {
        public bool LoadFromStream(InputStream stream, int type) => default;
        public bool SaveToStream(OutputStream stream, int type) => default;

        public bool ApplyStyleToSelection(IntPtr style, int flags) => default;

        public static void InitFileHandlers() {}
        
        public bool HasBorder { get; set; }

        public string ReportedUrl { get; }

        public event EventHandler? TextEnter;
        public event EventHandler? TextUrl;

        public string GetRange(long from, long to) => default;

        // Returns the length of the specified line in characters.
        public int GetLineLength(long lineNo) => default;

        // Returns the text for the given line.
        public string GetLineText(long lineNo) => default;

        // Returns the number of lines in the buffer.
        public int GetNumberOfLines() => default;

        // Returns <c>true</c> if the buffer has been modified.
        public bool IsModified() => default;

        // Returns <c>true</c> if the control is editable.
        public bool IsEditable() => default;

        // Returns <c>true</c> if the control is single-line.
        // Currently wxRichTextCtrl does not support single-line editing.
        public bool IsSingleLine() => default;

        // Returns <c>true</c> if the control is multiline.
        public bool IsMultiLine() => default;

        // Returns the text within the current selection range, if any.    
        public string GetStringSelection() => default;

        // Gets the current filename associated with the control.    
        public string GetFilename() => default;

        // Sets the current filename.    
        public void SetFilename(string filename) { }

        // Sets the size of the buffer beyond which layout is delayed during resizing.
        // This optimizes sizing for large buffers. The default is 20000.
        public void SetDelayedLayoutThreshold(long threshold) { }

        // Gets the size of the buffer beyond which layout is delayed during resizing.
        // This optimizes sizing for large buffers. The default is 20000.
        public long GetDelayedLayoutThreshold() => default;

        // Gets the flag indicating that full layout is required.    
        public bool GetFullLayoutRequired() => default;

        // Sets the flag indicating that full layout is required.    
        public void SetFullLayoutRequired(bool b) { }

        // Returns the last time full layout was performed.    
        public long GetFullLayoutTime() => default;

        // Sets the last time full layout was performed.    
        public void SetFullLayoutTime(long t) { }

        // Returns the position that should be shown when full (delayed) layout is performed.    
        public long GetFullLayoutSavedPosition() => default;

        // Sets the position that should be shown when full (delayed) layout is performed.    
        public void SetFullLayoutSavedPosition(long p) { }

        // Forces any pending layout due to delayed, partial layout when the control was resized.
        public void ForceDelayedLayout() { }

        // Returns <c>true</c> if we are showing the caret position at the start of a line
        // instead of at the end of the previous one.
        public bool GetCaretAtLineStart() => default;

        // Sets a flag to remember that we are showing the caret position at the start of a line
        // instead of at the end of the previous one.    
        public void SetCaretAtLineStart(bool atStart) { }

        // Returns <c>true</c> if we are dragging a selection.    
        public bool GetDragging() => default;

        // Sets a flag to remember if we are dragging a selection.
        public void SetDragging(bool dragging) { }

        // Returns the current context menu.    
        public IntPtr GetContextMenu() => default;

        // Sets the current context menu.    
        public void SetContextMenu(IntPtr menu) { }

        // Returns an anchor so we know how to extend the selection.
        // It's a caret position since it's between two characters.    
        public long GetSelectionAnchor() => default;

        // Sets an anchor so we know how to extend the selection.
        // It's a caret position since it's between two characters.    
        public void SetSelectionAnchor(long anchor) { }

        // Returns the anchor object if selecting multiple containers.
        public IntPtr GetSelectionAnchorObject() => default;

        // Sets the anchor object if selecting multiple containers.
        public void SetSelectionAnchorObject(IntPtr anchor) { }

        // Returns the wxRichTextObject object that currently has the editing focus.
        // If there are no composite objects, this will be the top-level buffer.
        public IntPtr GetFocusObject() => default;

        // Sets m_focusObject without making any alterations.
        public void StoreFocusObject(IntPtr richObj) { }

        // Sets the wxRichTextObject object that currently has the editing focus.
        public bool SetFocusObject(IntPtr richObj, bool setCaretPosition = true) => default;

        // Invalidates the whole buffer to trigger painting later.    
        public void Invalidate() { }

        // Clears the buffer content, leaving a single empty paragraph. Cannot be undone.    
        public void Clear() { }

        // Replaces the content in the specified range with the string specified by @a value.
        public void Replace(long from, long to, string value) { }

        // Removes the content in the specified range.    
        public void Remove(long from, long to) { }

        public bool LoadFile(string file, int type) => default;

        // Saves the buffer content using the given type.
        // If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        // the filename extension.    
        public bool SaveFile(string file, int type) => default;

        // Sets flags that change the behaviour of loading or saving.
        // See the documentation for each handler class to see what flags are
        // relevant for each handler.    
        public void SetHandlerFlags(int flags) { }

        // Returns flags that change the behaviour of loading or saving.
        // See the documentation for each handler class to see what flags are
        // relevant for each handler.
        public int GetHandlerFlags() => default;

        // Marks the buffer as modified.    
        public void MarkDirty() { }

        // Sets the buffer's modified status to @false, and clears the buffer's command history.    
        public void DiscardEdits() { }

        // Sets the maximum number of characters that may be entered in a single line
        // text control.For compatibility only; currently does nothing.
        public void SetMaxLength(ulong len) { }

        // Writes text at the current position.    
        public void WriteText(string text) { }

        // Sets the insertion point to the end of the buffer and writes the text.    
        public void AppendText(string text) { }

        // Translates from column and line number to position.    
        public long XYToPosition(long x, long y) => default;

        // Scrolls the buffer so that the given position is in view.    
        public void ShowPosition(long pos) { }

        // Copies the selected content (if any) to the clipboard.    
        public void Copy() { }

        // Copies the selected content (if any) to the clipboard and deletes the selection.
        // This is undoable.    
        public void Cut() { }

        // Pastes content from the clipboard to the buffer.    
        public void Paste() { }

        // Deletes the content in the selection, if any. This is undoable.    
        public void DeleteSelection() { }

        // Returns <c>true</c> if selected content can be copied to the clipboard.    
        public bool CanCopy() => default;

        // Returns <c>true</c> if selected content can be copied to the clipboard and deleted.    
        public bool CanCut() => default;

        // Returns <c>true</c> if the clipboard content can be pasted to the buffer.    
        public bool CanPaste() => default;

        // Returns <c>true</c> if selected content can be deleted.    
        public bool CanDeleteSelection() => default;

        // Undoes the command at the top of the command history, if there is one.    
        public void Undo() { }

        // Redoes the current command.    
        public void Redo() { }

        // Returns <c>true</c> if there is a command in the command history that can be undone.    
        public bool CanUndo() => default;

        // Returns <c>true</c> if there is a command in the command history that can be redone.    
        public bool CanRedo() => default;

        // Sets the insertion point and causes the current editing style to be taken from
        // the new position (unlike wxRichTextCtrl::SetCaretPosition).    
        public void SetInsertionPoint(long pos) { }

        // Sets the insertion point to the end of the text control.    
        public void SetInsertionPointEnd() { }

        // Returns the current insertion point.    
        public long GetInsertionPoint() => default;

        // Sets the selection to the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // So, for example, to set the selection for a character at position 5, use the
        // range (5,6).
        public void SetSelection(long from, long to) { }

        // Makes the control editable, or not.
        public void SetEditable(bool editable) { }

        // Returns <c>true</c> if there is a selection and the object containing the selection
        // was the same as the current focus object.    
        public bool HasSelection() => default;

        // Returns <c>true</c> if there was a selection, whether or not the current focus object
        // is the same as the selection's container object.    
        public bool HasUnfocusedSelection() => default;

        // Inserts a new paragraph at the current insertion point. @see LineBreak().    
        public bool Newline() => default;

        // Inserts a line break at the current insertion point.
        //A line break forces wrapping within a paragraph, and can be introduced by
        //using this function, by appending the wxChar value @b  wxRichTextLineBreakChar
        //to text content, or by typing Shift-Return.    
        public bool LineBreak() => default;

        // Ends the current style.    
        public bool EndStyle() => default;

        // Ends application of all styles in the current style stack.
        public bool EndAllStyles() => default;

        // Begins using bold.
        public bool BeginBold() => default;

        // Ends using bold.
        public bool EndBold() => default;

        // Begins using italic.    
        public bool BeginItalic() => default;

        // Ends using italic.    
        public bool EndItalic() => default;

        // Begins using underlining.    
        public bool BeginUnderline() => default;

        // End applying underlining.    
        public bool EndUnderline() => default;

        // Begins using the given point size.    
        public bool BeginFontSize(int pointSize) => default;

        // Ends using a point size.    
        public bool EndFontSize() => default;

        // Begins using this font.    
        public bool BeginFont(Font? font) => default;

        // Ends using a font.    
        public bool EndFont() => default;

        // Begins using this colour.    
        public bool BeginTextColour(Color colour) => default;

        // Ends applying a text colour.    
        public bool EndTextColour() => default;

        // Begins using alignment.
        // For alignment values, see wxTextAttr.
        public bool BeginAlignment(int alignment) => default;

        // Ends alignment.    
        public bool EndAlignment() => default;

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
        public bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0) => default;

        // Ends left indent.    
        public bool EndLeftIndent() => default;

        // Begins a right indent, specified in tenths of a millimetre.    
        public bool BeginRightIndent(int rightIndent) => default;

        // Ends right indent.    
        public bool EndRightIndent() => default;

        // Begins paragraph spacing; pass the before-paragraph and after-paragraph spacing
        // in tenths of a millimetre.    
        public bool BeginParagraphSpacing(int before, int after) => default;

        // Ends paragraph spacing.    
        public bool EndParagraphSpacing() => default;

        // Begins applying line spacing. @e spacing is a multiple, where 10 means
        // single-spacing, 15 means 1.5 spacing, and 20 means double spacing.
        // The ::wxTextAttrLineSpacing constants are defined for convenience.
        public bool BeginLineSpacing(int lineSpacing) => default;

        // Ends line spacing.    
        public bool EndLineSpacing() => default;

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
        public bool BeginNumberedBullet(int bulletNumber, int leftIndent, int leftSubIndent,
            int bulletStyle) => default;// = wxTEXT_ATTR_BULLET_STYLE_ARABIC|wxTEXT_ATTR_BULLET_STYLE_PERIOD

        // Ends application of a numbered bullet.    
        public bool EndNumberedBullet() => default;

        // Begins applying a symbol bullet, using a character from the current font.
        // See BeginNumberedBullet() for an explanation of how indentation is used
        // to render the bulleted paragraph.
        public bool BeginSymbolBullet(string symbol, int leftIndent, int leftSubIndent,
            int bulletStyle) => default;//= wxTEXT_ATTR_BULLET_STYLE_SYMBOL

        // Ends applying a symbol bullet.
        public bool EndSymbolBullet() => default;

        // Begins applying a symbol bullet.
        public bool BeginStandardBullet(string bulletName, int leftIndent,
            int leftSubIndent, int bulletStyle) => default; // = wxTEXT_ATTR_BULLET_STYLE_STANDARD

        // Begins applying a standard bullet.    
        public bool EndStandardBullet() => default;

        // Begins using the named character style.
        public bool BeginCharacterStyle(string characterStyle) => default;

        // Ends application of a named character style.    
        public bool EndCharacterStyle() => default;

        // Begins applying the named paragraph style.    
        public bool BeginParagraphStyle(string paragraphStyle) => default;

        // Ends application of a named paragraph style.    
        public bool EndParagraphStyle() => default;

        // Begins using a specified list style.
        // Optionally, you can also pass a level and a number.    
        public bool BeginListStyle(string listStyle, int level = 1, int number = 1) => default;

        // Ends using a specified list style.    
        public bool EndListStyle() => default;

        // Begins applying wxTEXT_ATTR_URL to the content.
        // Pass a URL and optionally, a character style to apply, since it is common
        // to mark a URL with a familiar style such as blue text with underlining.
        public bool BeginURL(string url, string characterStyle = default) => default;

        // Ends applying a URL.
        public bool EndURL() => default;

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is bold.
        public bool IsSelectionBold() => default;

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is italic.
        public bool IsSelectionItalics() => default;

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is underlined.
        public bool IsSelectionUnderlined() => default;

        // Returns <c>true</c> if all of the selection, or the content
        // at the current caret position, has the supplied wxTextAttrEffects flag(s).
        public bool DoesSelectionHaveTextEffectFlag(int flag) => default;

        // Returns <c>true</c> if all of the selection, or the content
        // at the caret position, is aligned according to the specified flag.
        public bool IsSelectionAligned(int alignment) => default;

        // Apples bold to the selection or default style (undoable).
        public bool ApplyBoldToSelection() => default;

        // Applies italic to the selection or default style (undoable).
        public bool ApplyItalicToSelection() => default;

        // Applies underline to the selection or default style (undoable).
        public bool ApplyUnderlineToSelection() => default;

        // Applies one or more wxTextAttrEffects flags to the selection (undoable).
        // If there is no selection, it is applied to the default style.    
        public bool ApplyTextEffectToSelection(int flags) => default;

        // Applies the given alignment to the selection or the default style (undoable).
        // For alignment values, see wxTextAttr.    
        public bool ApplyAlignmentToSelection(int alignment) => default;

        // Applies the style sheet to the buffer, matching paragraph styles in the sheet
        // against named styles in the buffer.
        // This might be useful if the styles have changed.
        // If @a sheet is @NULL, the sheet set with SetStyleSheet() is used.
        //Currently this applies paragraph styles only.    
        public bool ApplyStyle(IntPtr def) => default;

        // Sets the style sheet associated with the control.
        // A style sheet allows named character and paragraph styles to be applied.    
        public void SetStyleSheet(IntPtr styleSheet) { }

        // Sets the default style to the style under the cursor.    
        public bool SetDefaultStyleToCursorStyle() => default;

        // Cancels any selection.    
        public void SelectNone() { }

        // Selects the word at the given character position.    
        public bool SelectWord(long position) => default;

        // Lays out the buffer, which must be done before certain operations, such as
        // setting the caret position.
        // This function should not normally be required by the application.    
        public bool LayoutContent(bool onlyVisibleRect = false) => default;

        // Move the caret to the given character position.
        // Please note that this does not update the current editing style
        // from the new position; to do that, call wxRichTextCtrl::SetInsertionPoint instead.
        public bool MoveCaret(long pos, bool showAtLineStart = false, IntPtr container = default) => default;

        // Moves right.    
        public bool MoveRight(int noPositions = 1, int flags = 0) => default;

        // Moves left.    
        public bool MoveLeft(int noPositions = 1, int flags = 0) => default;

        // Moves to the start of the paragraph.    
        public bool MoveUp(int noLines = 1, int flags = 0) => default;

        // Moves the caret down.    
        public bool MoveDown(int noLines = 1, int flags = 0) => default;

        // Moves to the end of the line.    
        public bool MoveToLineEnd(int flags = 0) => default;

        // Moves to the start of the line.    
        public bool MoveToLineStart(int flags = 0) => default;

        // Moves to the end of the paragraph.    
        public bool MoveToParagraphEnd(int flags = 0) => default;

        // Moves to the start of the paragraph.    
        public bool MoveToParagraphStart(int flags = 0) => default;

        // Moves to the start of the buffer.    
        public bool MoveHome(int flags = 0) => default;

        // Moves to the end of the buffer.    
        public bool MoveEnd(int flags = 0) => default;

        // Moves one or more pages up.    
        public bool PageUp(int noPages = 1, int flags = 0) => default;

        // Moves one or more pages down.    
        public bool PageDown(int noPages = 1, int flags = 0) => default;

        // Moves a number of words to the left.    
        public bool WordLeft(int noPages = 1, int flags = 0) => default;

        // Move a number of words to the right.    
        public bool WordRight(int noPages = 1, int flags = 0) => default;

        // Push the style sheet to top of stack.    
        public bool PushStyleSheet(IntPtr styleSheet) => default;

        // Pops the style sheet from top of stack.    
        public IntPtr PopStyleSheet() => default;

        // Applies the style sheet to the buffer, for example if the styles have changed.    
        public bool ApplyStyleSheet(IntPtr styleSheet = default) => default;

        // Shows the given context menu, optionally adding appropriate property-editing commands for the current position in the object hierarchy.    
        public bool ShowContextMenu(IntPtr menu, PointI pt, bool addPropertyCommands = true) => default;

        // Prepares the context menu, optionally adding appropriate property-editing commands.
        // Returns the number of property commands added.    
        public int PrepareContextMenu(IntPtr menu, PointI pt, bool addPropertyCommands = true) => default;

        // Returns <c>true</c> if we can edit the object's properties via a GUI.
        public bool CanEditProperties(IntPtr richObj) => default;

        // Edits the object's properties via a GUI.
        public bool EditProperties(IntPtr richObj, IntPtr parentWindow) => default;

        // Gets the object's properties menu label.    
        public string GetPropertiesMenuLabel(IntPtr richObj) => default;

        // Starts batching undo history for commands.
        public bool BeginBatchUndo(string cmdName) => default;

        // Ends batching undo command history.    
        public bool EndBatchUndo() => default;

        // Returns <c>true</c> if undo commands are being batched.    
        public bool BatchingUndo() => default;

        // Starts suppressing undo history for commands.    
        public bool BeginSuppressUndo() => default;

        // Ends suppressing undo command history.    
        public bool EndSuppressUndo() => default;

        // Returns <c>true</c> if undo history suppression is on.    
        public bool SuppressingUndo() => default;

        // Enable or disable the vertical scrollbar.
        public void EnableVerticalScrollbar(bool enable) { }

        // Returns <c>true</c> if the vertical scrollbar is enabled.
        public bool GetVerticalScrollbarEnabled() => default;

        // Sets the scale factor for displaying fonts, for example for more comfortable editing.
        public void SetFontScale(float fontScale, bool refresh = false) { }

        // Returns the scale factor for displaying fonts, for example for more comfortable editing.
        public float GetFontScale() => default;

        // Returns <c>true</c> if this control can use attributes and text. The default is False.    
        public bool GetVirtualAttributesEnabled() => default;

        // Pass <c>true</c> to let the control use attributes. The default is False.
        public void EnableVirtualAttributes(bool b) { }

        // Write text
        public void DoWriteText(string value, int flags = 0) { }

        // Helper function for extending the selection, returning <c>true</c> if the selection
        // was changed. Selections are in caret positions.    
        public bool ExtendSelection(long oldPosition, long newPosition, int flags) => default;

        // Extends a table selection in the given direction.    
        public bool ExtendCellSelection(IntPtr table, int noRowSteps, int noColSteps) => default;

        // Starts selecting table cells.    
        public bool StartCellSelection(IntPtr table, IntPtr newCell) => default;

        // Scrolls @a position into view. This function takes a caret position.
        public bool ScrollIntoView(long position, int keyCode) => default;

        // Sets the caret position.
        // The caret position is the character position just before the caret.
        // A value of -1 means the caret is at the start of the buffer.
        // Please note that this does not update the current editing style
        // from the new position or cause the actual caret to be refreshed; to do that,
        // call wxRichTextCtrl::SetInsertionPoint instead.   
        public void SetCaretPosition(long position, bool showAtLineStart = false) { }

        // Returns the current caret position.    
        public long GetCaretPosition() => default;

        // The adjusted caret position is the character position adjusted to take
        // into account whether we're at the start of a paragraph, in which case
        // style information should be taken from the next position, not current one.    
        public long GetAdjustedCaretPosition(long caretPos) => default;

        // Move the caret one visual step forward: this may mean setting a flag
        // and keeping the same position if we're going from the end of one line
        // to the start of the next, which may be the exact same caret position.    
        public void MoveCaretForward(long oldPosition) { }

        // Transforms logical (unscrolled) position to physical window position.    
        public PointI GetPhysicalPoint(PointI ptLogical) => default;

        // Transforms physical window position to logical (unscrolled) position.
        public PointI GetLogicalPoint(PointI ptPhysical) => default;

        // Helper function for finding the caret position for the next word.
        // Direction is 1 (forward) or -1 (backwards).    
        public long FindNextWordPosition(int direction = 1) => default;

        // Returns <c>true</c> if the given position is visible on the screen.    
        public bool IsPositionVisible(long pos) => default;

        // Returns the first visible position in the current view.    
        public long GetFirstVisiblePosition() => default;

        // Returns the caret position since the default formatting was changed. As
        // soon as this position changes, we no longer reflect the default style
        // in the UI. A value of -2 means that we should only reflect the style of the
        //content under the caret.
        public long GetCaretPositionForDefaultStyle() => default;

        // Set the caret position for the default style that the user is selecting.
        public void SetCaretPositionForDefaultStyle(long pos) { }

        //Move the caret one visual step forward: this may mean setting a flag
        //and keeping the same position if we're going from the end of one line
        //to the start of the next, which may be the exact same caret position.    
        public void MoveCaretBack(long oldPosition) { }

        // Returns the caret height and position for the given character position.
        // If container is null, the current focus object will be used.
        public bool GetCaretPositionForIndex(long position, RectI rect,
            IntPtr container = default) => default;

        // Internal helper function returning the line for the visible caret position.
        // If the caret is shown at the very end of the line, it means the next character
        // is actually on the following line.
        // So this function gets the line we're expecting to find if this is the case.
        public IntPtr GetVisibleLineForCaretPosition(long caretPosition) => default;

        // Gets the command processor associated with the control's buffer.    
        public IntPtr GetCommandProcessor() => default;

        // Returns <c>true</c> if the user has recently set the default style without moving
        // the caret, and therefore the UI needs to reflect the default style and not
        // the style at the caret.
        public bool IsDefaultStyleShowing() => default;

        // Returns the first visible point in the window.    
        public PointI GetFirstVisiblePoint() => default;

        // Enable or disable images
        public void EnableImages(bool b) { }

        // Returns <c>true</c> if images are enabled.   
        public bool GetImagesEnabled() => default;

        // Enable or disable delayed image loading   
        public void EnableDelayedImageLoading(bool b) { }

        // Returns <c>true</c> if delayed image loading is enabled.
        public bool GetDelayedImageLoading() => default;

        // Gets the flag indicating that delayed image processing is required.
        public bool GetDelayedImageProcessingRequired() => default;

        // Sets the flag indicating that delayed image processing is required.    
        public void SetDelayedImageProcessingRequired(bool b) { }

        // Returns the last time delayed image processing was performed.    
        public long GetDelayedImageProcessingTime() => default;

        // Sets the last time delayed image processing was performed.    
        public void SetDelayedImageProcessingTime(long t) { }

        // Returns the content of the entire control as a string.    
        public string GetValue() => default;

        // Replaces existing content with the given text.    
        public void SetValue(string value) { }

        // Set the line increment height in pixels
        public void SetLineHeight(int height) { }

        public int GetLineHeight() => default;

        // Sets up the caret for the given position and container, after a mouse click.
        public bool SetCaretPositionAfterClick(IntPtr container, long position,
            int hitTestFlags, bool extendSelection = false) => default;

        // Clears the cache of available font names.    
        public static void ClearAvailableFontNames() { }

        // Do delayed image loading and garbage-collect other images    
        public bool ProcessDelayedImageLoading(bool refresh) => default;

        // Request delayed image processing.    
        public void RequestDelayedImageProcessing() { }

        // Gets the attributes at the given position.
        // This function gets the @e uncombined style - that is, the attributes associated
        // with the paragraph or character content, and not necessarily the combined
        // attributes you see on the screen.
        // To get the combined attributes, use GetStyle().
        // If you specify (any) paragraph attribute in @e style's flags, this function
        // will fetch the paragraph attributes.
        // Otherwise, it will return the character attributes.    
        public bool GetUncombinedStyle(long position, IntPtr style) => default;
        public bool GetUncombinedStyle2(long position, IntPtr style,
            IntPtr container) => default;

        //  Sets the current default style, which can be used to change how subsequently
        //  inserted text is displayed.    
        public bool SetDefaultStyle(IntPtr style) => default;
        public bool SetDefaultRichStyle(IntPtr style) => default;

        // Returns the current default style, which can be used to change how subsequently
        // inserted text is displayed.    
        public IntPtr GetDefaultStyleEx() => default;

        // Returns the last position in the buffer.    
        public long GetLastPosition() => default;

        // Gets the attributes at the given position.
        // This function gets the combined style - that is, the style you see on the
        // screen as a result of combining base style, paragraph style and character
        // style attributes.
        // To get the character or paragraph style alone, use GetUncombinedStyle().    
        public IntPtr GetStyle(long position) => default;
        public IntPtr GetRichStyle(long position) => default;
        public IntPtr GetStyleInContainer(long position, IntPtr container) => default;

        // Sets the attributes for the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one. So, for example, to set the style for a character at
        // position 5, use the range (5,6).
        public bool SetStyle(long start, long end, IntPtr style) => default;
        public bool SetRichStyle(long start, long end, IntPtr style) => default;

        // Sets the attributes for a single object    
        public void SetStyle2(IntPtr richObj, IntPtr textAttr, int flags) { }// = wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Gets the attributes common to the specified range.
        // Attributes that differ in value within the range will not be included
        // in @a style flags.
        public IntPtr GetStyleForRange(long startRange, long endRange) => default;
        public IntPtr GetStyleForRange2(long startRange, long endRange) => default;
        public IntPtr GetStyleForRange3(long startRange, long endRange, IntPtr container) => default;

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
        public bool SetStyleEx(long startRange, long endRange, IntPtr style,
            int flags) => default; // = wxRICHTEXT_SETSTYLE_WITH_UNDO


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
        public bool SetListStyle(long startRange, long endRange, IntPtr def, int flags,
            int startFrom = 1, int specifiedLevel = -1) => default; //= wxRICHTEXT_SETSTYLE_WITH_UNDO
        public bool SetListStyle2(long startRange, long endRange, string defName,
            int flags, int startFrom = 1, int specifiedLevel = -1) => default;//= wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Clears the list style from the given range, clearing list-related attributes
        // and applying any named paragraph style associated with each paragraph.
        // @a flags is a bit list of the following:
        // - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        public bool ClearListStyle(long startRange, long endRange, int flags) => default;// = wxRICHTEXT_SETSTYLE_WITH_UNDO

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
        public bool NumberList(long startRange, long endRange,
            IntPtr def, int flags, int startFrom = 1,
            int specifiedLevel = -1) => default;// = wxRICHTEXT_SETSTYLE_WITH_UNDO, def = default
        public bool NumberList2(long startRange, long endRange,
            string defName, int flags, int startFrom = 1,
            int specifiedLevel = -1) => default;// = wxRICHTEXT_SETSTYLE_WITH_UNDO

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
        public bool PromoteList(int promoteBy, long startRange, long endRange,
            IntPtr def, int flags, int specifiedLevel = -1) => default; // def = default, = wxRICHTEXT_SETSTYLE_WITH_UNDO
        public bool PromoteList2(int promoteBy, long startRange, long endRange,
            string defName, int flags, int specifiedLevel = -1) => default; //= wxRICHTEXT_SETSTYLE_WITH_UNDO

        // Deletes the content within the given range.
        public bool Delete(long startRange, long endRange) => default;

        // Write a table at the current insertion point, returning the table.
        // You can then call SetFocusObject() to set the focus to the new object.
        public IntPtr WriteTable(int rows, int cols,
            IntPtr tableAttr = default, IntPtr cellAttr = default) => default;

        // Sets the basic (overall) style.
        // This is the style of the whole buffer before further styles are applied,
        // unlike the default style, which only affects the style currently being
        // applied (for example, setting the default style to bold will cause
        // subsequently inserted text to be bold).
        public void SetBasicStyle(IntPtr style) { }

        // Gets the basic (overall) style.
        // This is the style of the whole buffer before further styles are applied,
        // unlike the default style, which only affects the style currently being
        // applied (for example, setting the default style to bold will cause
        // subsequently inserted text to be bold).    
        public IntPtr GetBasicStyle() => default;

        // Begins applying a style.    
        public bool BeginStyle(IntPtr style) => default;

        // Test if this whole range has character attributes of the specified kind.
        //If any of the attributes are different within the range, the test fails.
        //You can use this to implement, for example, bold button updating.
        //@a style must have flags indicating which attributes are of interest.
        public bool HasCharacterAttributes(long startRange, long endRange, IntPtr style) => default;

        // Returns the style sheet associated with the control, if any.
        // A style sheet allows named character and paragraph styles to be applied.    
        public IntPtr GetStyleSheet() => default;

        // Sets @a attr as the default style and tells the control that the UI should
        // reflect this attribute until the user moves the caret.
        public void SetAndShowDefaultStyle(IntPtr attr) { }

        // Sets the selection to the given range.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // So, for example, to set the selection for a character at position 5, use the
        // range (5,6).    
        public void SetSelectionRange(long startRange, long endRange) { }

        // Converts a text position to zero-based column and line numbers.    
        public PointI PositionToXY(long pos) => default;

        // Write a text box at the current insertion point, returning the text box.
        // You can then call SetFocusObject() to set the focus to the new object.    
        public IntPtr WriteTextBox(IntPtr textAttr = default) => default;

        // Test if this whole range has paragraph attributes of the specified kind.
        // If any of the attributes are different within the range, the test fails.
        // You can use this to implement, for example, centering button updating.
        // @a style must have flags indicating which attributes are of interest.
        public bool HasParagraphAttributes(long startRange, long endRange,
            IntPtr style) => default;

        // Sets the properties for the given range, passing flags to determine how the
        // attributes are set. You can merge properties or replace them.
        // The end point of range is specified as the last character position of the span
        // of text, plus one. So, for example, to set the properties for a character at
        // position 5, use the range (5,6).
        // @a flags may contain a bit list of the following values:
        // - wxRICHTEXT_SETSPROPERTIES_NONE: no flag.
        //- wxRICHTEXT_SETPROPERTIES_WITH_UNDO: specifies that this operation should be
        //   undoable.
        // - wxRICHTEXT_SETPROPERTIES_PARAGRAPHS_ONLY: specifies that the properties should only be
        //   applied to paragraphs, and not the content.
        // - wxRICHTEXT_SETPROPERTIES_CHARACTERS_ONLY: specifies that the properties should only be
        //   applied to characters, and not the paragraph.
        // - wxRICHTEXT_SETPROPERTIES_RESET: resets (clears) the existing properties before applying
        //   the new properties.
        // - wxRICHTEXT_SETPROPERTIES_REMOVE: removes the specified properties.    
        public bool SetProperties(long startRange, long endRange, IntPtr properties,
            int flags) => default; //= wxRICHTEXT_SETPROPERTIES_WITH_UNDO

        // Sets the text (normal) cursor.    
        public void SetTextCursor(IntPtr cursor) { }

        // Returns the text (normal) cursor.    
        public IntPtr GetTextCursor() => default;

        // Sets the cursor to be used over URLs.    
        public void SetURLCursor(IntPtr cursor) { }

        // Returns the cursor to be used over URLs.    
        public IntPtr GetURLCursor() => default;

        // Returns the range of the current selection.
        // The end point of range is specified as the last character position of the span
        // of text, plus one.
        // If the return values @a from and @a to are the same, there is no selection.    
        public IntPtr GetSelection() => default;

        // Returns an object that stores information about context menu property item(s),
        // in order to communicate between the context menu event handler and the code
        // that responds to it. The wxRichTextContextMenuPropertiesInfo stores one
        // item for each object that could respond to a property-editing event. If
        // objects are nested, several might be editable.    
        public IntPtr GetContextMenuPropertiesInfo() => default;

        public void SetSelection2(IntPtr sel) { }

        // Write a bitmap or image at the current insertion point.
        // Supply an optional type to use for internal and file storage of the raw data.    
        public bool WriteImage(Image bitmap, int bitmapType,
                                IntPtr textAttr = default) => default;   // =  = wxBITMAP_TYPE_PNG

        // Loads an image from a file and writes it at the current insertion point.    
        public bool WriteImage2(string filename, int bitmapType,
                                IntPtr textAttr = default) => default;

        // Writes an image block at the current insertion point.
        public bool WriteImage3(IntPtr imageBlock, IntPtr textAttr = default) => default;

        // Writes a field at the current insertion point.
        //   @param fieldType
        //       The field type, matching an existing field type definition.
        //   @param properties
        //       Extra data for the field.
        //  @param textAttr
        //      Optional attributes.
        public IntPtr WriteField(string fieldType, IntPtr properties,
                                IntPtr textAttr = default) => default;

        // Can we delete this range?
        // Sends an event to the control.    
        public bool CanDeleteRange(IntPtr container, long startRange, long endRange) => default;

        // Can we insert content at this position?
        // Sends an event to the control.    
        public bool CanInsertContent(IntPtr container, long pos) => default;

        // Returns the buffer associated with the control.    
        public IntPtr GetBuffer() => default;

        // Deletes content if there is a selection, e.g. when pressing a key.
        // Returns the new caret position in @e newPos, or leaves it if there
        // was no action. This is undoable.
        public long DeleteSelectedContent() => default;
    }
}

/*
    //   Returns the selection range in character positions. -1, -1 means no selection.
    //   The range is in API convention, i.e. a single character selection is denoted
    //   by (n, n+1)    
    wxRichTextRange GetSelectionRange()=> default;

    // Adds a new paragraph of text to the end of the buffer.    
    wxRichTextRange AddParagraph(string text)=> default;

    // Adds an image to the control's buffer.    
    wxRichTextRange AddImage(wxImage& image)=> default;

====================

    bool ProcessDelayedImageLoading(Int32Rect screenRect,
        IntPtr box, int& loadCount);

    // Find the caret position for the combination of hit-test flags and character position.
    // Returns the caret position and also an indication of where to place the caret (caretLineStart)
    // since this is ambiguous (same position used for end of line and start of next).    
    long FindCaretPositionForCharacterPosition(long position, int hitTestFlags, 
        IntPtr container, bool& caretLineStart);

    // Finds the character at the given position in pixels.
    // @a pt is in device coords (not adjusted for the client area origin nor for
    // scrolling).    
    wxTextCtrlHitTestResult HitTest(Int32Point pt, long *pos)
    wxTextCtrlHitTestResult HitTest2(Int32Point pt, long *col, long *row)

    // Finds the container at the given point, which is in screen coordinates.
    IntPtr FindContainerAtPoint(Int32Point pt,
        long& position, int& hit, IntPtr hitObj, int flags = 0);

    // Given a character position at which there is a list style, find the range
    // encompassing the same list style by looking backwards and forwards.    
    wxRichTextRange FindRangeForList(long pos, bool& isNumberedList);
*/
