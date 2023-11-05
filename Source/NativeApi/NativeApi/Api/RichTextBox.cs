#pragma warning disable
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
    public class RichTextBox
    {
/*
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
*/
    }
}

/*

// 
Returns the range of the current selection.
        The end point of range is specified as the last character position of the span
        of text, plus one.
        If the return values @a from and @a to are the same, there is no selection.
    
    void GetSelection(long* from, long* to) const wxOVERRIDE;
    const wxRichTextSelection& GetSelection() const { return m_selection; }
    wxRichTextSelection& GetSelection() { return m_selection; }

    // 
        Returns the text within the current selection range, if any.
    
    wxString GetStringSelection() const wxOVERRIDE;

    // 
        Gets the current filename associated with the control.
    
    wxString GetFilename() const { return m_filename; }

    // 
        Sets the current filename.
    
    void SetFilename(const wxString& filename) { m_filename = filename; }

    // 
        Sets the size of the buffer beyond which layout is delayed during resizing.
        This optimizes sizing for large buffers. The default is 20000.
    
    void SetDelayedLayoutThreshold(long threshold) { m_delayedLayoutThreshold = threshold; }

    // 
        Gets the size of the buffer beyond which layout is delayed during resizing.
        This optimizes sizing for large buffers. The default is 20000.
    
    long GetDelayedLayoutThreshold() const { return m_delayedLayoutThreshold; }

    // 
        Gets the flag indicating that full layout is required.
    
    bool GetFullLayoutRequired() const { return m_fullLayoutRequired; }

    // 
        Sets the flag indicating that full layout is required.
    
    void SetFullLayoutRequired(bool b) { m_fullLayoutRequired = b; }

    // 
        Returns the last time full layout was performed.
    
    wxLongLong GetFullLayoutTime() const { return m_fullLayoutTime; }

    // 
        Sets the last time full layout was performed.
    
    void SetFullLayoutTime(wxLongLong t) { m_fullLayoutTime = t; }

    // 
        Returns the position that should be shown when full (delayed) layout is performed.
    
    long GetFullLayoutSavedPosition() const { return m_fullLayoutSavedPosition; }

    // 
        Sets the position that should be shown when full (delayed) layout is performed.
    
    void SetFullLayoutSavedPosition(long p) { m_fullLayoutSavedPosition = p; }

    // 
        Forces any pending layout due to delayed, partial layout when the control
        was resized.
    
    void ForceDelayedLayout();

    // 
        Sets the text (normal) cursor.
    
    void SetTextCursor(const wxCursor& cursor ) { m_textCursor = cursor; }

    // 
        Returns the text (normal) cursor.
    
    wxCursor GetTextCursor() const { return m_textCursor; }

    // 
        Sets the cursor to be used over URLs.
    
    void SetURLCursor(const wxCursor& cursor ) { m_urlCursor = cursor; }

    // 
        Returns the cursor to be used over URLs.
    
    wxCursor GetURLCursor() const { return m_urlCursor; }

    // 
        Returns <c>true</c> if we are showing the caret position at the start of a line
        instead of at the end of the previous one.
    
    bool GetCaretAtLineStart() const { return m_caretAtLineStart; }

    // 
        Sets a flag to remember that we are showing the caret position at the start of a line
        instead of at the end of the previous one.
    
    void SetCaretAtLineStart(bool atStart) { m_caretAtLineStart = atStart; }

    // 
        Returns <c>true</c> if we are dragging a selection.
    
    bool GetDragging() const { return m_dragging; }

    // 
        Sets a flag to remember if we are dragging a selection.
    
    void SetDragging(bool dragging) { m_dragging = dragging; }


#if wxRICHTEXT_BUFFERED_PAINTING
    //@{
    // 
        Returns the buffer bitmap if using buffered painting.
    
    const wxBitmap& GetBufferBitmap() const { return m_bufferBitmap; }
    wxBitmap& GetBufferBitmap() { return m_bufferBitmap; }
    //@}
#endif

    // 
        Returns the current context menu.
    
    wxMenu* GetContextMenu()

    // 
        Sets the current context menu.
    
    void SetContextMenu(wxMenu* menu);

    // 
        Returns an anchor so we know how to extend the selection.
        It's a caret position since it's between two characters.
    
    long GetSelectionAnchor()

    // 
        Sets an anchor so we know how to extend the selection.
        It's a caret position since it's between two characters.
    
    void SetSelectionAnchor(long anchor)

    // 
        Returns the anchor object if selecting multiple containers.
    
    wxRichTextObject* GetSelectionAnchorObject()

    // 
        Sets the anchor object if selecting multiple containers.
    
    void SetSelectionAnchorObject(wxRichTextObject* anchor)

    //@{
    // 
        Returns an object that stores information about context menu property item(s),
        in order to communicate between the context menu event handler and the code
        that responds to it. The wxRichTextContextMenuPropertiesInfo stores one
        item for each object that could respond to a property-editing event. If
        objects are nested, several might be editable.
    
    wxRichTextContextMenuPropertiesInfo& GetContextMenuPropertiesInfo()
    const wxRichTextContextMenuPropertiesInfo& GetContextMenuPropertiesInfo()
    //@}

    // 
        Returns the wxRichTextObject object that currently has the editing focus.
        If there are no composite objects, this will be the top-level buffer.
    
    wxRichTextParagraphLayoutBox* GetFocusObject()

    // 
        Sets m_focusObject without making any alterations.
    
    void StoreFocusObject(wxRichTextParagraphLayoutBox* obj)

    // 
        Sets the wxRichTextObject object that currently has the editing focus.
    
    bool SetFocusObject(wxRichTextParagraphLayoutBox* obj, bool setCaretPosition = true);

    // 
        Invalidates the whole buffer to trigger painting later.
    
    void Invalidate() { GetBuffer().Invalidate(wxRICHTEXT_ALL); }

    // 
        Clears the buffer content, leaving a single empty paragraph. Cannot be undone.
    
    void Clear() wxOVERRIDE;

    // 
        Replaces the content in the specified range with the string specified by
        @a value.
    
    void Replace(long from, long to, const wxString& value) wxOVERRIDE;

    // 
        Removes the content in the specified range.
    
    void Remove(long from, long to) wxOVERRIDE;

#ifdef DOXYGEN
    // 
        Loads content into the control's buffer using the given type.

        If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        the filename extension.

        This function looks for a suitable wxRichTextFileHandler object.
    
    bool LoadFile(const wxString& file,
                  int type = wxRICHTEXT_TYPE_ANY);
#endif

#if wxUSE_FFILE && wxUSE_STREAMS
    // 
        Helper function for LoadFile(). Loads content into the control's buffer using the given type.

        If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        the filename extension.

        This function looks for a suitable wxRichTextFileHandler object.
    
    bool DoLoadFile(const wxString& file, int fileType) wxOVERRIDE;
#endif // wxUSE_FFILE && wxUSE_STREAMS

#ifdef DOXYGEN
    // 
        Saves the buffer content using the given type.

        If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        the filename extension.

        This function looks for a suitable wxRichTextFileHandler object.
    
    bool SaveFile(const wxString& file = wxEmptyString,
                  int type = wxRICHTEXT_TYPE_ANY);
#endif

#if wxUSE_FFILE && wxUSE_STREAMS
    // 
        Helper function for SaveFile(). Saves the buffer content using the given type.

        If the specified type is wxRICHTEXT_TYPE_ANY, the type is deduced from
        the filename extension.

        This function looks for a suitable wxRichTextFileHandler object.
    
    bool DoSaveFile(const wxString& file = wxEmptyString,
                            int fileType = wxRICHTEXT_TYPE_ANY) wxOVERRIDE;
#endif // wxUSE_FFILE && wxUSE_STREAMS

    // 
        Sets flags that change the behaviour of loading or saving.

        See the documentation for each handler class to see what flags are
        relevant for each handler.
    
    void SetHandlerFlags(int flags) { GetBuffer().SetHandlerFlags(flags); }

    // 
        Returns flags that change the behaviour of loading or saving.
        See the documentation for each handler class to see what flags are
        relevant for each handler.
    
    int GetHandlerFlags() const { return GetBuffer().GetHandlerFlags(); }

    // 
        Marks the buffer as modified.
    
    void MarkDirty() wxOVERRIDE;

    // 
        Sets the buffer's modified status to @false, and clears the buffer's command
        history.
    
    void DiscardEdits() wxOVERRIDE;

    // 
        Sets the maximum number of characters that may be entered in a single line
        text control. For compatibility only; currently does nothing.
    
    void SetMaxLength(unsigned long WXUNUSED(len)) wxOVERRIDE { }

    // 
        Writes text at the current position.
    
    void WriteText(const wxString& text) wxOVERRIDE;

    // 
        Sets the insertion point to the end of the buffer and writes the text.
    
    void AppendText(const wxString& text) wxOVERRIDE;

    //@{
    // 
        Gets the attributes at the given position.
        This function gets the combined style - that is, the style you see on the
        screen as a result of combining base style, paragraph style and character
        style attributes.

        To get the character or paragraph style alone, use GetUncombinedStyle().

        @beginWxPerlOnly
        In wxPerl this method is implemented as GetStyle(@a position)
        returning a 2-element list (ok, attr).
        @endWxPerlOnly
    
    bool GetStyle(long position, wxTextAttr& style) wxOVERRIDE;
    bool GetStyle(long position, wxRichTextAttr& style);
    bool GetStyle(long position, wxRichTextAttr& style, wxRichTextParagraphLayoutBox* container);
    //@}

    //@{
    // 
        Sets the attributes for the given range.
        The end point of range is specified as the last character position of the span
        of text, plus one.

        So, for example, to set the style for a character at position 5, use the range
        (5,6).
    
    bool SetStyle(long start, long end, const wxTextAttr& style) wxOVERRIDE;
    bool SetStyle(long start, long end, const wxRichTextAttr& style);
    bool SetStyle(const wxRichTextRange& range, const wxTextAttr& style);
    bool SetStyle(const wxRichTextRange& range, const wxRichTextAttr& style);
    //@}

    // 
        Sets the attributes for a single object
    
    void SetStyle(wxRichTextObject *obj, const wxRichTextAttr& textAttr, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO);

    //@{
    // 
        Gets the attributes common to the specified range.
        Attributes that differ in value within the range will not be included
        in @a style flags.

        @beginWxPerlOnly
        In wxPerl this method is implemented as GetStyleForRange(@a position)
        returning a 2-element list (ok, attr).
        @endWxPerlOnly
    
    bool GetStyleForRange(const wxRichTextRange& range, wxTextAttr& style);
    bool GetStyleForRange(const wxRichTextRange& range, wxRichTextAttr& style);
    bool GetStyleForRange(const wxRichTextRange& range, wxRichTextAttr& style, wxRichTextParagraphLayoutBox* container);
    //@}

    // 
        Sets the attributes for the given range, passing flags to determine how the
        attributes are set.

        The end point of range is specified as the last character position of the span
        of text, plus one. So, for example, to set the style for a character at
        position 5, use the range (5,6).

        @a flags may contain a bit list of the following values:
        - wxRICHTEXT_SETSTYLE_NONE: no style flag.
        - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this operation should be
          undoable.
        - wxRICHTEXT_SETSTYLE_OPTIMIZE: specifies that the style should not be applied
          if the combined style at this point is already the style in question.
        - wxRICHTEXT_SETSTYLE_PARAGRAPHS_ONLY: specifies that the style should only be
          applied to paragraphs, and not the content.
          This allows content styling to be preserved independently from that
          of e.g. a named paragraph style.
        - wxRICHTEXT_SETSTYLE_CHARACTERS_ONLY: specifies that the style should only be
          applied to characters, and not the paragraph.
          This allows content styling to be preserved independently from that
          of e.g. a named paragraph style.
        - wxRICHTEXT_SETSTYLE_RESET: resets (clears) the existing style before applying
          the new style.
        - wxRICHTEXT_SETSTYLE_REMOVE: removes the specified style. Only the style flags
          are used in this operation.
    
    bool SetStyleEx(const wxRichTextRange& range, const wxRichTextAttr& style, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO);

    //@{
    // 
        Gets the attributes at the given position.
        This function gets the @e uncombined style - that is, the attributes associated
        with the paragraph or character content, and not necessarily the combined
        attributes you see on the screen.
        To get the combined attributes, use GetStyle().

        If you specify (any) paragraph attribute in @e style's flags, this function
        will fetch the paragraph attributes.
        Otherwise, it will return the character attributes.

        @beginWxPerlOnly
        In wxPerl this method is implemented as GetUncombinedStyle(@a position)
        returning a 2-element list (ok, attr).
        @endWxPerlOnly
    
    bool GetUncombinedStyle(long position, wxRichTextAttr& style);
    bool GetUncombinedStyle(long position, wxRichTextAttr& style, wxRichTextParagraphLayoutBox* container);
    //@}

    //@{
    // 
        Sets the current default style, which can be used to change how subsequently
        inserted text is displayed.
    
    bool SetDefaultStyle(const wxTextAttr& style) wxOVERRIDE;
    bool SetDefaultStyle(const wxRichTextAttr& style);
    //@}

    // 
        Returns the current default style, which can be used to change how subsequently
        inserted text is displayed.
    
    const wxRichTextAttr& GetDefaultStyleEx() const;

    //const wxTextAttr& GetDefaultStyle() const;

    //@{
    // 
        Sets the list attributes for the given range, passing flags to determine how
        the attributes are set.

        Either the style definition or the name of the style definition (in the current
        sheet) can be passed.
        @a flags is a bit list of the following:
        - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
          @a startFrom, otherwise existing attributes are used.
        - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
          as the level for all paragraphs, otherwise the current indentation will be used.

        @see NumberList(), PromoteList(), ClearListStyle().
    
    bool SetListStyle(const wxRichTextRange& range, wxRichTextListStyleDefinition* def, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int startFrom = 1, int specifiedLevel = -1);
    bool SetListStyle(const wxRichTextRange& range, const wxString& defName, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int startFrom = 1, int specifiedLevel = -1);
    //@}

    // 
        Clears the list style from the given range, clearing list-related attributes
        and applying any named paragraph style associated with each paragraph.

        @a flags is a bit list of the following:
        - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.

        @see SetListStyle(), PromoteList(), NumberList().
    
    bool ClearListStyle(const wxRichTextRange& range, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO);

    //@{
    // 
        Numbers the paragraphs in the given range.
        Pass flags to determine how the attributes are set.

        Either the style definition or the name of the style definition (in the current
        sheet) can be passed.

        @a flags is a bit list of the following:
        - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
          @a startFrom, otherwise existing attributes are used.
        - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
          as the level for all paragraphs, otherwise the current indentation will be used.

        @see SetListStyle(), PromoteList(), ClearListStyle().
    
    bool NumberList(const wxRichTextRange& range, wxRichTextListStyleDefinition* def = NULL, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int startFrom = 1, int specifiedLevel = -1);
    bool NumberList(const wxRichTextRange& range, const wxString& defName, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int startFrom = 1, int specifiedLevel = -1);
    //@}

    //@{
    // 
        Promotes or demotes the paragraphs in the given range.
        A positive @a promoteBy produces a smaller indent, and a negative number
        produces a larger indent. Pass flags to determine how the attributes are set.
        Either the style definition or the name of the style definition (in the current
        sheet) can be passed.

        @a flags is a bit list of the following:
        - wxRICHTEXT_SETSTYLE_WITH_UNDO: specifies that this command will be undoable.
        - wxRICHTEXT_SETSTYLE_RENUMBER: specifies that numbering should start from
          @a startFrom, otherwise existing attributes are used.
        - wxRICHTEXT_SETSTYLE_SPECIFY_LEVEL: specifies that @a listLevel should be used
        as the level for all paragraphs, otherwise the current indentation will be used.

        @see SetListStyle(), @see SetListStyle(), ClearListStyle().
    
    bool PromoteList(int promoteBy, const wxRichTextRange& range, wxRichTextListStyleDefinition* def = NULL, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int specifiedLevel = -1);
    bool PromoteList(int promoteBy, const wxRichTextRange& range, const wxString& defName, int flags = wxRICHTEXT_SETSTYLE_WITH_UNDO, int specifiedLevel = -1);
    //@}

    // 
        Sets the properties for the given range, passing flags to determine how the
        attributes are set. You can merge properties or replace them.

        The end point of range is specified as the last character position of the span
        of text, plus one. So, for example, to set the properties for a character at
        position 5, use the range (5,6).

        @a flags may contain a bit list of the following values:
        - wxRICHTEXT_SETSPROPERTIES_NONE: no flag.
        - wxRICHTEXT_SETPROPERTIES_WITH_UNDO: specifies that this operation should be
          undoable.
        - wxRICHTEXT_SETPROPERTIES_PARAGRAPHS_ONLY: specifies that the properties should only be
          applied to paragraphs, and not the content.
        - wxRICHTEXT_SETPROPERTIES_CHARACTERS_ONLY: specifies that the properties should only be
          applied to characters, and not the paragraph.
        - wxRICHTEXT_SETPROPERTIES_RESET: resets (clears) the existing properties before applying
          the new properties.
        - wxRICHTEXT_SETPROPERTIES_REMOVE: removes the specified properties.
    
    bool SetProperties(const wxRichTextRange& range, const wxRichTextProperties& properties, int flags = wxRICHTEXT_SETPROPERTIES_WITH_UNDO);

    // 
        Deletes the content within the given range.
    
    bool Delete(const wxRichTextRange& range);

    // 
        Translates from column and line number to position.
    
    long XYToPosition(long x, long y) const wxOVERRIDE;

    // 
        Converts a text position to zero-based column and line numbers.
    
    bool PositionToXY(long pos, long *x, long *y) const wxOVERRIDE;

    // 
        Scrolls the buffer so that the given position is in view.
    
    void ShowPosition(long pos) wxOVERRIDE;

    //@{
    // 
        Finds the character at the given position in pixels.
        @a pt is in device coords (not adjusted for the client area origin nor for
        scrolling).
    
    wxTextCtrlHitTestResult HitTest(const wxPoint& pt, long *pos) const wxOVERRIDE;
    wxTextCtrlHitTestResult HitTest(const wxPoint& pt,
                                            wxTextCoord *col,
                                            wxTextCoord *row) const wxOVERRIDE;

    // 
        Finds the container at the given point, which is in screen coordinates.
    
    wxRichTextParagraphLayoutBox* FindContainerAtPoint(const wxPoint& pt, long& position, int& hit, wxRichTextObject* hitObj, int flags = 0);
    //@}

#if wxUSE_DRAG_AND_DROP
    // 
        Does the 'drop' of Drag'n'Drop.
    
    void OnDrop(wxCoord WXUNUSED(x), wxCoord WXUNUSED(y), wxDragResult def, wxDataObject* DataObj);
#endif

// Clipboard operations

    // 
        Copies the selected content (if any) to the clipboard.
    
    void Copy() wxOVERRIDE;

    // 
        Copies the selected content (if any) to the clipboard and deletes the selection.
        This is undoable.
    
    void Cut() wxOVERRIDE;

    // 
        Pastes content from the clipboard to the buffer.
    
    void Paste() wxOVERRIDE;

    // 
        Deletes the content in the selection, if any. This is undoable.
    
    void DeleteSelection();

    // 
        Returns <c>true</c> if selected content can be copied to the clipboard.
    
    bool CanCopy() const wxOVERRIDE;

    // 
        Returns <c>true</c> if selected content can be copied to the clipboard and deleted.
    
    bool CanCut() const wxOVERRIDE;

    // 
        Returns <c>true</c> if the clipboard content can be pasted to the buffer.
    
    bool CanPaste() const wxOVERRIDE;

    // 
        Returns <c>true</c> if selected content can be deleted.
    
    bool CanDeleteSelection() const;

    // 
        Undoes the command at the top of the command history, if there is one.
    
    void Undo() wxOVERRIDE;

    // 
        Redoes the current command.
    
    void Redo() wxOVERRIDE;

    // 
        Returns <c>true</c> if there is a command in the command history that can be undone.
    
    bool CanUndo() const wxOVERRIDE;

    // 
        Returns <c>true</c> if there is a command in the command history that can be redone.
    
    bool CanRedo() const wxOVERRIDE;

    // 
        Sets the insertion point and causes the current editing style to be taken from
        the new position (unlike wxRichTextCtrl::SetCaretPosition).
    
    void SetInsertionPoint(long pos) wxOVERRIDE;

    // 
        Sets the insertion point to the end of the text control.
    
    void SetInsertionPointEnd() wxOVERRIDE;

    // 
        Returns the current insertion point.
    
    long GetInsertionPoint() const wxOVERRIDE;

    // 
        Returns the last position in the buffer.
    
    wxTextPos GetLastPosition() const wxOVERRIDE;

    //@{
    // 
        Sets the selection to the given range.
        The end point of range is specified as the last character position of the span
        of text, plus one.

        So, for example, to set the selection for a character at position 5, use the
        range (5,6).
    
    void SetSelection(long from, long to) wxOVERRIDE;
    void SetSelection(const wxRichTextSelection& sel) { m_selection = sel; }
    //@}

    // 
        Makes the control editable, or not.
    
    void SetEditable(bool editable) wxOVERRIDE;

    // 
        Returns <c>true</c> if there is a selection and the object containing the selection
        was the same as the current focus object.
    
    bool HasSelection() const;

    // 
        Returns <c>true</c> if there was a selection, whether or not the current focus object
        is the same as the selection's container object.
    
    bool HasUnfocusedSelection() const;

    //@{
    // 
        Write a bitmap or image at the current insertion point.
        Supply an optional type to use for internal and file storage of the raw data.
    
    bool WriteImage(const wxImage& image, wxBitmapType bitmapType = wxBITMAP_TYPE_PNG,
                            const wxRichTextAttr& textAttr = wxRichTextAttr());

    bool WriteImage(const wxBitmap& bitmap, wxBitmapType bitmapType = wxBITMAP_TYPE_PNG,
                            const wxRichTextAttr& textAttr = wxRichTextAttr());
    //@}

    // 
        Loads an image from a file and writes it at the current insertion point.
    
    bool WriteImage(const wxString& filename, wxBitmapType bitmapType,
                            const wxRichTextAttr& textAttr = wxRichTextAttr());

    // 
        Writes an image block at the current insertion point.
    
    bool WriteImage(const wxRichTextImageBlock& imageBlock,
                            const wxRichTextAttr& textAttr = wxRichTextAttr());

    // 
        Write a text box at the current insertion point, returning the text box.
        You can then call SetFocusObject() to set the focus to the new object.
    
    wxRichTextBox* WriteTextBox(const wxRichTextAttr& textAttr = wxRichTextAttr());

    // 
        Writes a field at the current insertion point.

        @param fieldType
            The field type, matching an existing field type definition.
        @param properties
            Extra data for the field.
        @param textAttr
            Optional attributes.

        @see wxRichTextField, wxRichTextFieldType, wxRichTextFieldTypeStandard
    
    wxRichTextField* WriteField(const wxString& fieldType, const wxRichTextProperties& properties,
                            const wxRichTextAttr& textAttr = wxRichTextAttr());

    // 
        Write a table at the current insertion point, returning the table.
        You can then call SetFocusObject() to set the focus to the new object.
    
    wxRichTextTable* WriteTable(int rows, int cols, const wxRichTextAttr& tableAttr = wxRichTextAttr(), const wxRichTextAttr& cellAttr = wxRichTextAttr());

    // 
        Inserts a new paragraph at the current insertion point. @see LineBreak().
    
    bool Newline();

    // 
        Inserts a line break at the current insertion point.

        A line break forces wrapping within a paragraph, and can be introduced by
        using this function, by appending the wxChar value @b  wxRichTextLineBreakChar
        to text content, or by typing Shift-Return.
    
    bool LineBreak();

    // 
        Sets the basic (overall) style.

        This is the style of the whole buffer before further styles are applied,
        unlike the default style, which only affects the style currently being
        applied (for example, setting the default style to bold will cause
        subsequently inserted text to be bold).
    
    void SetBasicStyle(const wxRichTextAttr& style) { GetBuffer().SetBasicStyle(style); }

    // 
        Gets the basic (overall) style.

        This is the style of the whole buffer before further styles are applied,
        unlike the default style, which only affects the style currently being
        applied (for example, setting the default style to bold will cause
        subsequently inserted text to be bold).
    
    const wxRichTextAttr& GetBasicStyle() const { return GetBuffer().GetBasicStyle(); }

    // 
        Begins applying a style.
    
    bool BeginStyle(const wxRichTextAttr& style) { return GetBuffer().BeginStyle(style); }

    // 
        Ends the current style.
    
    bool EndStyle() { return GetBuffer().EndStyle(); }

    // 
        Ends application of all styles in the current style stack.
    
    bool EndAllStyles() { return GetBuffer().EndAllStyles(); }

    // 
        Begins using bold.
    
    bool BeginBold() { return GetBuffer().BeginBold(); }

    // 
        Ends using bold.
    
    bool EndBold()  { return GetBuffer().EndBold(); }

    // 
        Begins using italic.
    
    bool BeginItalic() { return GetBuffer().BeginItalic(); }

    // 
        Ends using italic.
    
    bool EndItalic() { return GetBuffer().EndItalic(); }

    // 
        Begins using underlining.
    
    bool BeginUnderline() { return GetBuffer().BeginUnderline(); }

    // 
        End applying underlining.
    
    bool EndUnderline() { return GetBuffer().EndUnderline(); }

    // 
        Begins using the given point size.
    
    bool BeginFontSize(int pointSize) { return GetBuffer().BeginFontSize(pointSize); }

    // 
        Ends using a point size.
    
    bool EndFontSize() { return GetBuffer().EndFontSize(); }

    // 
        Begins using this font.
    
    bool BeginFont(const wxFont& font) { return GetBuffer().BeginFont(font); }

    // 
        Ends using a font.
    
    bool EndFont() { return GetBuffer().EndFont(); }

    // 
        Begins using this colour.
    
    bool BeginTextColour(const wxColour& colour) { return GetBuffer().BeginTextColour(colour); }

    // 
        Ends applying a text colour.
    
    bool EndTextColour() { return GetBuffer().EndTextColour(); }

    // 
        Begins using alignment.
        For alignment values, see wxTextAttr.
    
    bool BeginAlignment(wxTextAttrAlignment alignment) { return GetBuffer().BeginAlignment(alignment); }

    // 
        Ends alignment.
    
    bool EndAlignment() { return GetBuffer().EndAlignment(); }

    // 
        Begins applying a left indent and subindent in tenths of a millimetre.
        The subindent is an offset from the left edge of the paragraph, and is
        used for all but the first line in a paragraph. A positive value will
        cause the first line to appear to the left of the subsequent lines, and
        a negative value will cause the first line to be indented to the right
        of the subsequent lines.

        wxRichTextBuffer uses indentation to render a bulleted item. The
        content of the paragraph, including the first line, starts at the
        @a leftIndent plus the @a leftSubIndent.

        @param leftIndent
            The distance between the margin and the bullet.
        @param leftSubIndent
             The distance between the left edge of the bullet and the left edge
             of the actual paragraph.
    
    bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0) { return GetBuffer().BeginLeftIndent(leftIndent, leftSubIndent); }

    // 
        Ends left indent.
    
    bool EndLeftIndent() { return GetBuffer().EndLeftIndent(); }

    // 
        Begins a right indent, specified in tenths of a millimetre.
    
    bool BeginRightIndent(int rightIndent) { return GetBuffer().BeginRightIndent(rightIndent); }

    // 
        Ends right indent.
    
    bool EndRightIndent() { return GetBuffer().EndRightIndent(); }

    // 
        Begins paragraph spacing; pass the before-paragraph and after-paragraph spacing
        in tenths of a millimetre.
    
    bool BeginParagraphSpacing(int before, int after) { return GetBuffer().BeginParagraphSpacing(before, after); }

    // 
        Ends paragraph spacing.
    
    bool EndParagraphSpacing() { return GetBuffer().EndParagraphSpacing(); }

    // 
        Begins applying line spacing. @e spacing is a multiple, where 10 means
        single-spacing, 15 means 1.5 spacing, and 20 means double spacing.

        The ::wxTextAttrLineSpacing constants are defined for convenience.
    
    bool BeginLineSpacing(int lineSpacing) { return GetBuffer().BeginLineSpacing(lineSpacing); }

    // 
        Ends line spacing.
    
    bool EndLineSpacing() { return GetBuffer().EndLineSpacing(); }

    // 
        Begins a numbered bullet.

        This call will be needed for each item in the list, and the
        application should take care of incrementing the numbering.

        @a bulletNumber is a number, usually starting with 1.
        @a leftIndent and @a leftSubIndent are values in tenths of a millimetre.
        @a bulletStyle is a bitlist of the  ::wxTextAttrBulletStyle values.

        wxRichTextBuffer uses indentation to render a bulleted item.
        The left indent is the distance between the margin and the bullet.
        The content of the paragraph, including the first line, starts
        at leftMargin + leftSubIndent.
        So the distance between the left edge of the bullet and the
        left of the actual paragraph is leftSubIndent.
    
    bool BeginNumberedBullet(int bulletNumber, int leftIndent, int leftSubIndent, int bulletStyle = wxTEXT_ATTR_BULLET_STYLE_ARABIC|wxTEXT_ATTR_BULLET_STYLE_PERIOD)
    { return GetBuffer().BeginNumberedBullet(bulletNumber, leftIndent, leftSubIndent, bulletStyle); }

    // 
        Ends application of a numbered bullet.
    
    bool EndNumberedBullet() { return GetBuffer().EndNumberedBullet(); }

    // 
        Begins applying a symbol bullet, using a character from the current font.
        See BeginNumberedBullet() for an explanation of how indentation is used
        to render the bulleted paragraph.
    
    bool BeginSymbolBullet(const wxString& symbol, int leftIndent, int leftSubIndent, int bulletStyle = wxTEXT_ATTR_BULLET_STYLE_SYMBOL)
    { return GetBuffer().BeginSymbolBullet(symbol, leftIndent, leftSubIndent, bulletStyle); }

    // 
        Ends applying a symbol bullet.
    
    bool EndSymbolBullet() { return GetBuffer().EndSymbolBullet(); }

    // 
        Begins applying a symbol bullet.
    
    bool BeginStandardBullet(const wxString& bulletName, int leftIndent, int leftSubIndent, int bulletStyle = wxTEXT_ATTR_BULLET_STYLE_STANDARD)
    { return GetBuffer().BeginStandardBullet(bulletName, leftIndent, leftSubIndent, bulletStyle); }

    // 
        Begins applying a standard bullet.
    
    bool EndStandardBullet() { return GetBuffer().EndStandardBullet(); }

    // 
        Begins using the named character style.
    
    bool BeginCharacterStyle(const wxString& characterStyle) { return GetBuffer().BeginCharacterStyle(characterStyle); }

    // 
        Ends application of a named character style.
    
    bool EndCharacterStyle() { return GetBuffer().EndCharacterStyle(); }

    // 
        Begins applying the named paragraph style.
    
    bool BeginParagraphStyle(const wxString& paragraphStyle) { return GetBuffer().BeginParagraphStyle(paragraphStyle); }

    // 
        Ends application of a named paragraph style.
    
    bool EndParagraphStyle() { return GetBuffer().EndParagraphStyle(); }

    // 
        Begins using a specified list style.
        Optionally, you can also pass a level and a number.
    
    bool BeginListStyle(const wxString& listStyle, int level = 1, int number = 1) { return GetBuffer().BeginListStyle(listStyle, level, number); }

    // 
        Ends using a specified list style.
    
    bool EndListStyle() { return GetBuffer().EndListStyle(); }

    // 
        Begins applying wxTEXT_ATTR_URL to the content.

        Pass a URL and optionally, a character style to apply, since it is common
        to mark a URL with a familiar style such as blue text with underlining.
    
    bool BeginURL(const wxString& url, const wxString& characterStyle = wxEmptyString) { return GetBuffer().BeginURL(url, characterStyle); }

    // 
        Ends applying a URL.
    
    bool EndURL() { return GetBuffer().EndURL(); }

    // 
        Sets the default style to the style under the cursor.
    
    bool SetDefaultStyleToCursorStyle();

    // 
        Cancels any selection.
    
    void SelectNone() wxOVERRIDE;

    // 
        Selects the word at the given character position.
    
    bool SelectWord(long position);

    // 
        Returns the selection range in character positions. -1, -1 means no selection.

        The range is in API convention, i.e. a single character selection is denoted
        by (n, n+1)
    
    wxRichTextRange GetSelectionRange() const;

    // 
        Sets the selection to the given range.
        The end point of range is specified as the last character position of the span
        of text, plus one.

        So, for example, to set the selection for a character at position 5, use the
        range (5,6).
    
    void SetSelectionRange(const wxRichTextRange& range);

    // 
        Returns the selection range in character positions. -2, -2 means no selection
        -1, -1 means select everything.
        The range is in internal format, i.e. a single character selection is denoted
        by (n, n)
    
    wxRichTextRange GetInternalSelectionRange() const { return m_selection.GetRange(); }

    // 
        Sets the selection range in character positions. -2, -2 means no selection
        -1, -1 means select everything.
        The range is in internal format, i.e. a single character selection is denoted
        by (n, n)
    
    void SetInternalSelectionRange(const wxRichTextRange& range) { m_selection.Set(range, GetFocusObject()); }

    // 
        Adds a new paragraph of text to the end of the buffer.
    
    wxRichTextRange AddParagraph(const wxString& text);

    // 
        Adds an image to the control's buffer.
    
    wxRichTextRange AddImage(const wxImage& image);

    // 
        Lays out the buffer, which must be done before certain operations, such as
        setting the caret position.
        This function should not normally be required by the application.
    
    bool LayoutContent(bool onlyVisibleRect = false);

    // 
        Implements layout. An application may override this to perform operations before or after layout.
    
    void DoLayoutBuffer(wxRichTextBuffer& buffer, wxDC& dc, wxRichTextDrawingContext& context, const wxRect& rect, const wxRect& parentRect, int flags);

    // 
        Move the caret to the given character position.

        Please note that this does not update the current editing style
        from the new position; to do that, call wxRichTextCtrl::SetInsertionPoint instead.
    
    bool MoveCaret(long pos, bool showAtLineStart = false, wxRichTextParagraphLayoutBox* container = NULL);

    // 
        Moves right.
    
    bool MoveRight(int noPositions = 1, int flags = 0);

    // 
        Moves left.
    
    bool MoveLeft(int noPositions = 1, int flags = 0);

    // 
        Moves to the start of the paragraph.
    
    bool MoveUp(int noLines = 1, int flags = 0);

    // 
        Moves the caret down.
    
    bool MoveDown(int noLines = 1, int flags = 0);

    // 
        Moves to the end of the line.
    
    bool MoveToLineEnd(int flags = 0);

    // 
        Moves to the start of the line.
    
    bool MoveToLineStart(int flags = 0);

    // 
        Moves to the end of the paragraph.
    
    bool MoveToParagraphEnd(int flags = 0);

    // 
        Moves to the start of the paragraph.
    
    bool MoveToParagraphStart(int flags = 0);

    // 
        Moves to the start of the buffer.
    
    bool MoveHome(int flags = 0);

    // 
        Moves to the end of the buffer.
    
    bool MoveEnd(int flags = 0);

    // 
        Moves one or more pages up.
    
    bool PageUp(int noPages = 1, int flags = 0);

    // 
        Moves one or more pages down.
    
    bool PageDown(int noPages = 1, int flags = 0);

    // 
        Moves a number of words to the left.
    
    bool WordLeft(int noPages = 1, int flags = 0);

    // 
        Move a number of words to the right.
    
    bool WordRight(int noPages = 1, int flags = 0);

    //@{
    // 
        Returns the buffer associated with the control.
    
    wxRichTextBuffer& GetBuffer() { return m_buffer; }
    const wxRichTextBuffer& GetBuffer() const { return m_buffer; }
    //@}

    // 
        Starts batching undo history for commands.
    
    bool BeginBatchUndo(const wxString& cmdName) { return m_buffer.BeginBatchUndo(cmdName); }

    // 
        Ends batching undo command history.
    
    bool EndBatchUndo() { return m_buffer.EndBatchUndo(); }

    // 
        Returns <c>true</c> if undo commands are being batched.
    
    bool BatchingUndo() const { return m_buffer.BatchingUndo(); }

    // 
        Starts suppressing undo history for commands.
    
    bool BeginSuppressUndo() { return m_buffer.BeginSuppressUndo(); }

    // 
        Ends suppressing undo command history.
    
    bool EndSuppressUndo() { return m_buffer.EndSuppressUndo(); }

    // 
        Returns <c>true</c> if undo history suppression is on.
    
    bool SuppressingUndo() const { return m_buffer.SuppressingUndo(); }

    // 
        Test if this whole range has character attributes of the specified kind.
        If any of the attributes are different within the range, the test fails.

        You can use this to implement, for example, bold button updating.
        @a style must have flags indicating which attributes are of interest.
    
    bool HasCharacterAttributes(const wxRichTextRange& range, const wxRichTextAttr& style) const
    {
        return GetFocusObject()->HasCharacterAttributes(range.ToInternal(), style);
    }

    // 
        Test if this whole range has paragraph attributes of the specified kind.
        If any of the attributes are different within the range, the test fails.
        You can use this to implement, for example, centering button updating.
        @a style must have flags indicating which attributes are of interest.
    
    bool HasParagraphAttributes(const wxRichTextRange& range, const wxRichTextAttr& style) const
    {
        return GetFocusObject()->HasParagraphAttributes(range.ToInternal(), style);
    }

    // 
        Returns <c>true</c> if all of the selection, or the content at the caret position, is bold.
    
    bool IsSelectionBold();

    // 
        Returns <c>true</c> if all of the selection, or the content at the caret position, is italic.
    
    bool IsSelectionItalics();

    // 
        Returns <c>true</c> if all of the selection, or the content at the caret position, is underlined.
    
    bool IsSelectionUnderlined();

    // 
        Returns <c>true</c> if all of the selection, or the content at the current caret position, has the supplied wxTextAttrEffects flag(s).
    
    bool DoesSelectionHaveTextEffectFlag(int flag);

    // 
        Returns <c>true</c> if all of the selection, or the content at the caret position, is aligned according to the specified flag.
    
    bool IsSelectionAligned(wxTextAttrAlignment alignment);

    // 
        Apples bold to the selection or default style (undoable).
    
    bool ApplyBoldToSelection();

    // 
        Applies italic to the selection or default style (undoable).
    
    bool ApplyItalicToSelection();

    // 
        Applies underline to the selection or default style (undoable).
    
    bool ApplyUnderlineToSelection();

    // 
        Applies one or more wxTextAttrEffects flags to the selection (undoable).
        If there is no selection, it is applied to the default style.
    
    bool ApplyTextEffectToSelection(int flags);

    // 
        Applies the given alignment to the selection or the default style (undoable).
        For alignment values, see wxTextAttr.
    
    bool ApplyAlignmentToSelection(wxTextAttrAlignment alignment);

    // 
        Applies the style sheet to the buffer, matching paragraph styles in the sheet
        against named styles in the buffer.

        This might be useful if the styles have changed.
        If @a sheet is @NULL, the sheet set with SetStyleSheet() is used.
        Currently this applies paragraph styles only.
    
    bool ApplyStyle(wxRichTextStyleDefinition* def);

    // 
        Sets the style sheet associated with the control.
        A style sheet allows named character and paragraph styles to be applied.
    
    void SetStyleSheet(wxRichTextStyleSheet* styleSheet) { GetBuffer().SetStyleSheet(styleSheet); }

    // 
        Returns the style sheet associated with the control, if any.
        A style sheet allows named character and paragraph styles to be applied.
    
    wxRichTextStyleSheet* GetStyleSheet() const { return GetBuffer().GetStyleSheet(); }

    // 
        Push the style sheet to top of stack.
    
    bool PushStyleSheet(wxRichTextStyleSheet* styleSheet) { return GetBuffer().PushStyleSheet(styleSheet); }

    // 
        Pops the style sheet from top of stack.
    
    wxRichTextStyleSheet* PopStyleSheet() { return GetBuffer().PopStyleSheet(); }

    // 
        Applies the style sheet to the buffer, for example if the styles have changed.
    
    bool ApplyStyleSheet(wxRichTextStyleSheet* styleSheet = NULL);

    // 
        Shows the given context menu, optionally adding appropriate property-editing commands for the current position in the object hierarchy.
    
    bool ShowContextMenu(wxMenu* menu, const wxPoint& pt, bool addPropertyCommands = true);

    // 
        Prepares the context menu, optionally adding appropriate property-editing commands.
        Returns the number of property commands added.
    
    int PrepareContextMenu(wxMenu* menu, const wxPoint& pt, bool addPropertyCommands = true);

    // 
        Returns <c>true</c> if we can edit the object's properties via a GUI.
    
    bool CanEditProperties(wxRichTextObject* obj) const { return obj->CanEditProperties(); }

    // 
        Edits the object's properties via a GUI.
    
    bool EditProperties(wxRichTextObject* obj, wxWindow* parent) { return obj->EditProperties(parent, & GetBuffer()); }

    // 
        Gets the object's properties menu label.
    
    wxString GetPropertiesMenuLabel(wxRichTextObject* obj) { return obj->GetPropertiesMenuLabel(); }

    // 
        Prepares the content just before insertion (or after buffer reset). Called by the same function in wxRichTextBuffer.
        Currently is only called if undo mode is on.
    
    void PrepareContent(wxRichTextParagraphLayoutBox& WXUNUSED(container)) {}

    // 
        Can we delete this range?
        Sends an event to the control.
    
    bool CanDeleteRange(wxRichTextParagraphLayoutBox& container, const wxRichTextRange& range) const;

    // 
        Can we insert content at this position?
        Sends an event to the control.
    
    bool CanInsertContent(wxRichTextParagraphLayoutBox& container, long pos) const;

    // 
        Enable or disable the vertical scrollbar.
    
    void EnableVerticalScrollbar(bool enable);

    // 
        Returns <c>true</c> if the vertical scrollbar is enabled.
    
    bool GetVerticalScrollbarEnabled() const { return m_verticalScrollbarEnabled; }

    // 
        Sets the scale factor for displaying fonts, for example for more comfortable
        editing.
    
    void SetFontScale(double fontScale, bool refresh = false);

    // 
        Returns the scale factor for displaying fonts, for example for more comfortable
        editing.
    
    double GetFontScale() const { return GetBuffer().GetFontScale(); }

    // 
        Sets the scale factor for displaying certain dimensions such as indentation and
        inter-paragraph spacing. This can be useful when editing in a small control
        where you still want legible text, but a minimum of wasted white space.
    
    void SetDimensionScale(double dimScale, bool refresh = false);

    // 
        Returns the scale factor for displaying certain dimensions such as indentation
        and inter-paragraph spacing.
    
    double GetDimensionScale() const { return GetBuffer().GetDimensionScale(); }

    // 
        Sets an overall scale factor for displaying and editing the content.
    
    void SetScale(double scale, bool refresh = false);

    // 
        Returns an overall scale factor for displaying and editing the content.
    
    double GetScale() const { return m_scale; }

    // 
        Returns an unscaled point.
    
    wxPoint GetUnscaledPoint(const wxPoint& pt) const;

    // 
        Returns a scaled point.
    
    wxPoint GetScaledPoint(const wxPoint& pt) const;

    // 
        Returns an unscaled size.
    
    wxSize GetUnscaledSize(const wxSize& sz) const;

    // 
        Returns a scaled size.
    
    wxSize GetScaledSize(const wxSize& sz) const;

    // 
        Returns an unscaled rectangle.
    
    wxRect GetUnscaledRect(const wxRect& rect) const;

    // 
        Returns a scaled rectangle.
    
    wxRect GetScaledRect(const wxRect& rect) const;

    // 
        Returns <c>true</c> if this control can use attributes and text.
        The default is @false.
    
    bool GetVirtualAttributesEnabled() const { return m_useVirtualAttributes; }

    // 
        Pass <c>true</c> to let the control use attributes.
        The default is @false.
    
    void EnableVirtualAttributes(bool b) { m_useVirtualAttributes = b; }

// Command handlers

    // 
        Sends the event to the control.
    
    void Command(wxCommandEvent& event) wxOVERRIDE;

    // 
        Loads the first dropped file.
    
    void OnDropFiles(wxDropFilesEvent& event);

    void OnCaptureLost(wxMouseCaptureLostEvent& event);
    void OnSysColourChanged(wxSysColourChangedEvent& event);

    // 
        Standard handler for the wxID_CUT command.
    
    void OnCut(wxCommandEvent& event);

    // 
        Standard handler for the wxID_COPY command.
    
    void OnCopy(wxCommandEvent& event);

    // 
        Standard handler for the wxID_PASTE command.
    
    void OnPaste(wxCommandEvent& event);

    // 
        Standard handler for the wxID_UNDO command.
    
    void OnUndo(wxCommandEvent& event);

    // 
        Standard handler for the wxID_REDO command.
    
    void OnRedo(wxCommandEvent& event);

    // 
        Standard handler for the wxID_SELECTALL command.
    
    void OnSelectAll(wxCommandEvent& event);

    // 
        Standard handler for property commands.
    
    void OnProperties(wxCommandEvent& event);

    // 
        Standard handler for the wxID_CLEAR command.
    
    void OnClear(wxCommandEvent& event);

    // 
        Standard update handler for the wxID_CUT command.
    
    void OnUpdateCut(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_COPY command.
    
    void OnUpdateCopy(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_PASTE command.
    
    void OnUpdatePaste(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_UNDO command.
    
    void OnUpdateUndo(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_REDO command.
    
    void OnUpdateRedo(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_SELECTALL command.
    
    void OnUpdateSelectAll(wxUpdateUIEvent& event);

    // 
        Standard update handler for property commands.
    

    void OnUpdateProperties(wxUpdateUIEvent& event);

    // 
        Standard update handler for the wxID_CLEAR command.
    
    void OnUpdateClear(wxUpdateUIEvent& event);

    // 
        Shows a standard context menu with undo, redo, cut, copy, paste, clear, and
        select all commands.
    
    void OnContextMenu(wxContextMenuEvent& event);

// Event handlers

    // Painting
    void OnPaint(wxPaintEvent& event);
    void OnEraseBackground(wxEraseEvent& event);

    // Left-click
    void OnLeftClick(wxMouseEvent& event);

    // Left-up
    void OnLeftUp(wxMouseEvent& event);

    // Motion
    void OnMoveMouse(wxMouseEvent& event);

    // Left-double-click
    void OnLeftDClick(wxMouseEvent& event);

    // Middle-click
    void OnMiddleClick(wxMouseEvent& event);

    // Right-click
    void OnRightClick(wxMouseEvent& event);

    // Key press
    void OnChar(wxKeyEvent& event);

    // Sizing
    void OnSize(wxSizeEvent& event);

    // Setting/losing focus
    void OnSetFocus(wxFocusEvent& event);
    void OnKillFocus(wxFocusEvent& event);

    // Idle-time processing
    void OnIdle(wxIdleEvent& event);

    // Scrolling
    void OnScroll(wxScrollWinEvent& event);

    // 
        Sets the font, and also the basic and default attributes
        (see wxRichTextCtrl::SetDefaultStyle).
    
    bool SetFont(const wxFont& font) wxOVERRIDE;

    // 
        A helper function setting up scrollbars, for example after a resize.
    
    void SetupScrollbars(bool atTop = false, bool fromOnPaint = false);

    // 
        Helper function implementing keyboard navigation.
    
    bool KeyboardNavigate(int keyCode, int flags);

    // 
        Paints the background.
    
    void PaintBackground(wxDC& dc);

    // 
        Other user defined painting after everything else (i.e. all text) is painted.

        @since 2.9.1
    
    void PaintAboveContent(wxDC& WXUNUSED(dc)) {}

#if wxRICHTEXT_BUFFERED_PAINTING
    // 
        Recreates the buffer bitmap if necessary.
    
    bool RecreateBuffer(const wxSize& size = wxDefaultSize);
#endif

    // Write text
    void DoWriteText(const wxString& value, int flags = 0);

    // Should we inherit colours?
    bool ShouldInheritColours() const wxOVERRIDE { return false; }

    // 
        Internal function to position the visible caret according to the current caret
        position.
    
    void PositionCaret(wxRichTextParagraphLayoutBox* container = NULL);

    // 
        Helper function for extending the selection, returning <c>true</c> if the selection
        was changed. Selections are in caret positions.
    
    bool ExtendSelection(long oldPosition, long newPosition, int flags);

    // 
        Extends a table selection in the given direction.
    
    bool ExtendCellSelection(wxRichTextTable* table, int noRowSteps, int noColSteps);

    // 
        Starts selecting table cells.
    
    bool StartCellSelection(wxRichTextTable* table, wxRichTextParagraphLayoutBox* newCell);

    // 
        Scrolls @a position into view. This function takes a caret position.
    
    bool ScrollIntoView(long position, int keyCode);

    // 
        Refreshes the area affected by a selection change.
    
    bool RefreshForSelectionChange(const wxRichTextSelection& oldSelection, const wxRichTextSelection& newSelection);

    // 
        Overrides standard refresh in order to provoke delayed image loading.
    
    void Refresh( bool eraseBackground = true,
                       const wxRect *rect = (const wxRect *) NULL ) wxOVERRIDE;

    // 
        Sets the caret position.

        The caret position is the character position just before the caret.
        A value of -1 means the caret is at the start of the buffer.
        Please note that this does not update the current editing style
        from the new position or cause the actual caret to be refreshed; to do that,
        call wxRichTextCtrl::SetInsertionPoint instead.
    
    void SetCaretPosition(long position, bool showAtLineStart = false) ;

    // 
        Returns the current caret position.
    
    long GetCaretPosition() const { return m_caretPosition; }

    // 
        The adjusted caret position is the character position adjusted to take
        into account whether we're at the start of a paragraph, in which case
        style information should be taken from the next position, not current one.
    
    long GetAdjustedCaretPosition(long caretPos) const;

    // 
        Move the caret one visual step forward: this may mean setting a flag
        and keeping the same position if we're going from the end of one line
        to the start of the next, which may be the exact same caret position.
    
    void MoveCaretForward(long oldPosition) ;

    // 
        Move the caret one visual step forward: this may mean setting a flag
        and keeping the same position if we're going from the end of one line
        to the start of the next, which may be the exact same caret position.
    
    void MoveCaretBack(long oldPosition) ;

    // 
        Returns the caret height and position for the given character position.
        If container is null, the current focus object will be used.

        @beginWxPerlOnly
        In wxPerl this method is implemented as
        GetCaretPositionForIndex(@a position) returning a
        2-element list (ok, rect).
        @endWxPerlOnly
    
    bool GetCaretPositionForIndex(long position, wxRect& rect, wxRichTextParagraphLayoutBox* container = NULL);

    // 
        Internal helper function returning the line for the visible caret position.
        If the caret is shown at the very end of the line, it means the next character
        is actually on the following line.
        So this function gets the line we're expecting to find if this is the case.
    
    wxRichTextLine* GetVisibleLineForCaretPosition(long caretPosition) const;

    // 
        Gets the command processor associated with the control's buffer.
    
    wxCommandProcessor* GetCommandProcessor() const { return GetBuffer().GetCommandProcessor(); }

    // 
        Deletes content if there is a selection, e.g. when pressing a key.
        Returns the new caret position in @e newPos, or leaves it if there
        was no action. This is undoable.

        @beginWxPerlOnly
        In wxPerl this method takes no arguments and returns a 2-element
        list (ok, newPos).
        @endWxPerlOnly
    
    bool DeleteSelectedContent(long* newPos= NULL);

    // 
        Transforms logical (unscrolled) position to physical window position.
    
    wxPoint GetPhysicalPoint(const wxPoint& ptLogical) const;

    // 
        Transforms physical window position to logical (unscrolled) position.
    
    wxPoint GetLogicalPoint(const wxPoint& ptPhysical) const;

    // 
        Helper function for finding the caret position for the next word.
        Direction is 1 (forward) or -1 (backwards).
    
    long FindNextWordPosition(int direction = 1) const;

    // 
        Returns <c>true</c> if the given position is visible on the screen.
    
    bool IsPositionVisible(long pos) const;

    // 
        Returns the first visible position in the current view.
    
    long GetFirstVisiblePosition() const;

    // 
        Returns the caret position since the default formatting was changed. As
        soon as this position changes, we no longer reflect the default style
        in the UI. A value of -2 means that we should only reflect the style of the
        content under the caret.
    
    long GetCaretPositionForDefaultStyle() const { return m_caretPositionForDefaultStyle; }

    // 
        Set the caret position for the default style that the user is selecting.
    
    void SetCaretPositionForDefaultStyle(long pos) { m_caretPositionForDefaultStyle = pos; }

    // 
        Returns <c>true</c> if the user has recently set the default style without moving
        the caret, and therefore the UI needs to reflect the default style and not
        the style at the caret.

        Below is an example of code that uses this function to determine whether the UI
        should show that the current style is bold.

        @see SetAndShowDefaultStyle().
    
    bool IsDefaultStyleShowing() const { return m_caretPositionForDefaultStyle != -2; }

    // 
        Sets @a attr as the default style and tells the control that the UI should
        reflect this attribute until the user moves the caret.

        @see IsDefaultStyleShowing().
    
    void SetAndShowDefaultStyle(const wxRichTextAttr& attr)
    {
        SetDefaultStyle(attr);
        SetCaretPositionForDefaultStyle(GetCaretPosition());
    }

    // 
        Returns the first visible point in the window.
    
    wxPoint GetFirstVisiblePoint() const;

    // 
        Enable or disable images
    

    void EnableImages(bool b) { m_enableImages = b; }

    // 
        Returns <c>true</c> if images are enabled.
    

    bool GetImagesEnabled() const { return m_enableImages; }

    // 
        Enable or disable delayed image loading
    

    void EnableDelayedImageLoading(bool b) { m_enableDelayedImageLoading = b; }

    // 
        Returns <c>true</c> if delayed image loading is enabled.
    

    bool GetDelayedImageLoading() const { return m_enableDelayedImageLoading; }

    // 
        Gets the flag indicating that delayed image processing is required.
    
    bool GetDelayedImageProcessingRequired() const { return m_delayedImageProcessingRequired; }

    // 
        Sets the flag indicating that delayed image processing is required.
    
    void SetDelayedImageProcessingRequired(bool b) { m_delayedImageProcessingRequired = b; }

    // 
        Returns the last time delayed image processing was performed.
    
    wxLongLong GetDelayedImageProcessingTime() const { return m_delayedImageProcessingTime; }

    // 
        Sets the last time delayed image processing was performed.
    
    void SetDelayedImageProcessingTime(wxLongLong t) { m_delayedImageProcessingTime = t; }

#ifdef DOXYGEN
    // 
        Returns the content of the entire control as a string.
    
    wxString GetValue() const;

    // 
        Replaces existing content with the given text.
    
    void SetValue(const wxString& value);

    // 
        Call this function to prevent refresh and allow fast updates, and then Thaw() to
        refresh the control.
    
    void Freeze();

    // 
        Call this function to end a Freeze and refresh the display.
    
    void Thaw();

    // 
        Returns <c>true</c> if Freeze has been called without a Thaw.
    
    bool IsFrozen() const;

#endif

    /// Set the line increment height in pixels
    void SetLineHeight(int height) { m_lineHeight = height; }
    int GetLineHeight() const { return m_lineHeight; }

// Implementation

    // 
        Processes the back key.
    
    bool ProcessBackKey(wxKeyEvent& event, int flags);

    // 
        Given a character position at which there is a list style, find the range
        encompassing the same list style by looking backwards and forwards.
    
    wxRichTextRange FindRangeForList(long pos, bool& isNumberedList);

    // 
        Sets up the caret for the given position and container, after a mouse click.
    
    bool SetCaretPositionAfterClick(wxRichTextParagraphLayoutBox* container, long position, int hitTestFlags, bool extendSelection = false);

    // 
        Find the caret position for the combination of hit-test flags and character position.
        Returns the caret position and also an indication of where to place the caret (caretLineStart)
        since this is ambiguous (same position used for end of line and start of next).
    
    long FindCaretPositionForCharacterPosition(long position, int hitTestFlags, wxRichTextParagraphLayoutBox* container,
                                                   bool& caretLineStart);

    // 
        Processes mouse movement in order to change the cursor
    
    bool ProcessMouseMovement(wxRichTextParagraphLayoutBox* container, wxRichTextObject* obj, long position, const wxPoint& pos);

    // 
        Font names take a long time to retrieve, so cache them (on demand).
    
    static const wxArrayString& GetAvailableFontNames();

    // 
        Clears the cache of available font names.
    
    static void ClearAvailableFontNames();

    WX_FORWARD_TO_SCROLL_HELPER()

    // implement wxTextEntry methods
    wxString DoGetValue() const wxOVERRIDE;

    // 
        Do delayed image loading and garbage-collect other images
    
    bool ProcessDelayedImageLoading(bool refresh);
    bool ProcessDelayedImageLoading(const wxRect& screenRect, wxRichTextParagraphLayoutBox* box, int& loadCount);

    // 
        Request delayed image processing.
    
    void RequestDelayedImageProcessing();

    // 
        Respond to timer events.
    
    void OnTimer(wxTimerEvent& event);

*/
