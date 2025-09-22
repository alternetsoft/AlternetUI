using System;
using System.IO;

using Alternet.Drawing;

namespace Alternet.UI
{
    internal class RichTextBoxHandler : WxControlHandler, ISimpleRichTextBox, IRichTextBox
    {
        static RichTextBoxHandler()
        {
            Native.RichTextBox.InitFileHandlers();
        }

        public RichTextBoxHandler()
        {
        }

        event EventHandler? ISimpleRichTextBox.CurrentPositionChanged
        {
            add
            {
                if(Control is not null)
                    Control.CurrentPositionChanged += value;
            }

            remove
            {
                if (Control is not null)
                    Control.CurrentPositionChanged -= value;
            }
        }

        public override bool HasBorder
        {
            get => NativeControl.HasBorder;
            set => NativeControl.HasBorder = value;
        }

        string? ISimpleRichTextBox.Name
        {
            get => Control?.Name ?? string.Empty;
            set
            {
                if (Control is not null)
                    Control.Name = value;
            }
        }

        PointI? ISimpleRichTextBox.CurrentPosition
        {
            get => Control?.CurrentPosition ?? null;
            set
            {
                if (Control is not null)
                    Control.CurrentPosition = value;
            }
        }

        long ISimpleRichTextBox.LastLineNumber
        {
            get => Control?.LastLineNumber ?? 0;
        }

        long ISimpleRichTextBox.InsertionPointLineNumber
        {
            get => Control?.InsertionPointLineNumber ?? 0;
        }

        public new Native.RichTextBox NativeControl => (Native.RichTextBox)base.NativeControl!;

        public new RichTextBox? Control => (RichTextBox?)base.Control;

        public string ReportedUrl
        {
            get
            {
                return NativeControl.ReportedUrl;
            }
        }

        /// <summary>
        /// Clears the cache of available font names.
        /// </summary>
        public static void ClearAvailableFontNames()
        {
            Native.RichTextBox.ClearAvailableFontNames();
        }

        void IRichTextBox.SetDragging(bool dragging)
        {
            NativeControl.SetDragging(dragging);
        }

        long IRichTextBox.GetSelectionAnchor()
        {
            return NativeControl.GetSelectionAnchor();
        }

        void IRichTextBox.SetSelectionAnchor(long anchor)
        {
            NativeControl.SetSelectionAnchor(anchor);
        }

        string IRichTextBox.GetValue()
        {
            return NativeControl.GetValue();
        }

        void IRichTextBox.SetValue(string value)
        {
            NativeControl.SetValue(value);
        }

        bool IRichTextBox.IsModified()
        {
            return NativeControl.IsModified();
        }

        bool IRichTextBox.IsEditable()
        {
            return NativeControl.IsEditable();
        }

        bool IRichTextBox.IsSingleLine()
        {
            return NativeControl.IsSingleLine();
        }

        bool IRichTextBox.IsMultiLine()
        {
            return NativeControl.IsMultiLine();
        }

        void IRichTextBox.SetDelayedLayoutThreshold(long threshold)
        {
            NativeControl.SetDelayedLayoutThreshold(threshold);
        }

        long IRichTextBox.GetDelayedLayoutThreshold()
        {
            return NativeControl.GetDelayedLayoutThreshold();
        }

        bool IRichTextBox.GetFullLayoutRequired()
        {
            return NativeControl.GetFullLayoutRequired();
        }

        void IRichTextBox.SetFullLayoutRequired(bool b)
        {
            NativeControl.SetFullLayoutRequired(b);
        }

        long IRichTextBox.GetFullLayoutTime()
        {
            return NativeControl.GetFullLayoutTime();
        }

        void IRichTextBox.SetFullLayoutTime(long t)
        {
            NativeControl.SetFullLayoutTime(t);
        }

        long IRichTextBox.GetFullLayoutSavedPosition()
        {
            return NativeControl.GetFullLayoutSavedPosition();
        }

        void IRichTextBox.SetFullLayoutSavedPosition(long p)
        {
            NativeControl.SetFullLayoutSavedPosition(p);
        }

        void IRichTextBox.ForceDelayedLayout()
        {
            NativeControl.ForceDelayedLayout();
        }

        bool IRichTextBox.GetCaretAtLineStart()
        {
            return NativeControl.GetCaretAtLineStart();
        }

        void IRichTextBox.SetCaretAtLineStart(bool atStart)
        {
            NativeControl.SetCaretAtLineStart(atStart);
        }

        bool IRichTextBox.GetDragging()
        {
            return NativeControl.GetDragging();
        }

        int IRichTextBox.GetFileHandlerFlags()
        {
            return NativeControl.GetHandlerFlags();
        }

        void IRichTextBox.MarkDirty()
        {
            NativeControl.MarkDirty();
        }

        void IRichTextBox.DiscardEdits()
        {
            NativeControl.DiscardEdits();
        }

        void IRichTextBox.DeleteSelection()
        {
            NativeControl.DeleteSelection();
        }

        bool IRichTextBox.CanCopy()
        {
            return NativeControl.CanCopy();
        }

        bool IRichTextBox.CanCut()
        {
            return NativeControl.CanCut();
        }

        bool IRichTextBox.CanPaste()
        {
            return NativeControl.CanPaste();
        }

        bool IRichTextBox.CanDeleteSelection()
        {
            return NativeControl.CanDeleteSelection();
        }

        bool IRichTextBox.CanUndo()
        {
            return NativeControl.CanUndo();
        }

        bool IRichTextBox.CanRedo()
        {
            return NativeControl.CanRedo();
        }

        void IRichTextBox.SetEditable(bool editable)
        {
            NativeControl.SetEditable(editable);
        }

        bool IRichTextBox.HasSelection()
        {
            return NativeControl.HasSelection();
        }

        bool IRichTextBox.HasUnfocusedSelection()
        {
            return NativeControl.HasUnfocusedSelection();
        }

        bool IRichTextBox.NewLine()
        {
            return NativeControl.Newline();
        }

        bool IRichTextBox.LineBreak()
        {
            return NativeControl.LineBreak();
        }

        bool IRichTextBox.EndStyle()
        {
            return NativeControl.EndStyle();
        }

        bool IRichTextBox.EndAllStyles()
        {
            return NativeControl.EndAllStyles();
        }

        bool IRichTextBox.BeginBold()
        {
            return NativeControl.BeginBold();
        }

        bool IRichTextBox.EndBold()
        {
            return NativeControl.EndBold();
        }

        bool IRichTextBox.BeginItalic()
        {
            return NativeControl.BeginItalic();
        }

        bool IRichTextBox.EndItalic()
        {
            return NativeControl.EndItalic();
        }

        bool IRichTextBox.BeginUnderline()
        {
            return NativeControl.BeginUnderline();
        }

        bool IRichTextBox.EndUnderline()
        {
            return NativeControl.EndUnderline();
        }

        bool IRichTextBox.EndFontSize()
        {
            return NativeControl.EndFontSize();
        }

        bool IRichTextBox.EndFont()
        {
            return NativeControl.EndFont();
        }

        bool IRichTextBox.BeginAlignment(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.BeginAlignment((int)alignment);
        }

        bool IRichTextBox.EndAlignment()
        {
            return NativeControl.EndAlignment();
        }

        bool IRichTextBox.BeginLeftIndent(int leftIndent, int leftSubIndent)
        {
            return NativeControl.BeginLeftIndent(leftIndent, leftSubIndent);
        }

        bool IRichTextBox.EndLeftIndent()
        {
            return NativeControl.EndLeftIndent();
        }

        bool IRichTextBox.BeginRightIndent(int rightIndent)
        {
            return NativeControl.BeginRightIndent(rightIndent);
        }

        bool IRichTextBox.EndRightIndent()
        {
            return NativeControl.EndRightIndent();
        }

        bool IRichTextBox.BeginParagraphSpacing(int before, int after)
        {
            return NativeControl.BeginParagraphSpacing(before, after);
        }

        bool IRichTextBox.EndParagraphSpacing()
        {
            return NativeControl.EndParagraphSpacing();
        }

        bool IRichTextBox.BeginLineSpacing(int lineSpacing)
        {
            return NativeControl.BeginLineSpacing(lineSpacing);
        }

        bool IRichTextBox.EndLineSpacing()
        {
            return NativeControl.EndLineSpacing();
        }

        bool IRichTextBox.EndNumberedBullet()
        {
            return NativeControl.EndNumberedBullet();
        }

        bool IRichTextBox.EndSymbolBullet()
        {
            return NativeControl.EndSymbolBullet();
        }

        bool IRichTextBox.EndStandardBullet()
        {
            return NativeControl.EndStandardBullet();
        }

        bool IRichTextBox.BeginCharacterStyle(string characterStyle)
        {
            return NativeControl.BeginCharacterStyle(characterStyle);
        }

        bool IRichTextBox.EndCharacterStyle()
        {
            return NativeControl.EndCharacterStyle();
        }

        bool IRichTextBox.BeginParagraphStyle(string paragraphStyle)
        {
            return NativeControl.BeginParagraphStyle(paragraphStyle);
        }

        bool IRichTextBox.EndParagraphStyle()
        {
            return NativeControl.EndParagraphStyle();
        }

        bool IRichTextBox.BeginListStyle(string listStyle, int level, int number)
        {
            return NativeControl.BeginListStyle(listStyle, level, number);
        }

        bool IRichTextBox.EndListStyle()
        {
            return NativeControl.EndListStyle();
        }

        bool IRichTextBox.BeginURL(string url, string? characterStyle)
        {
            return NativeControl.BeginURL(url, characterStyle ?? string.Empty);
        }

        bool IRichTextBox.EndURL()
        {
            return NativeControl.EndURL();
        }

        bool IRichTextBox.IsSelectionBold()
        {
            return NativeControl.IsSelectionBold();
        }

        bool IRichTextBox.IsSelectionItalics()
        {
            return NativeControl.IsSelectionItalics();
        }

        bool IRichTextBox.IsSelectionUnderlined()
        {
            return NativeControl.IsSelectionUnderlined();
        }

        bool IRichTextBox.ApplyBoldToSelection()
        {
            return NativeControl.ApplyBoldToSelection();
        }

        bool IRichTextBox.ApplyItalicToSelection()
        {
            return NativeControl.ApplyItalicToSelection();
        }

        bool IRichTextBox.ApplyUnderlineToSelection()
        {
            return NativeControl.ApplyUnderlineToSelection();
        }

        bool IRichTextBox.SetDefaultStyleToCursorStyle()
        {
            return NativeControl.SetDefaultStyleToCursorStyle();
        }

        bool IRichTextBox.SelectWord(long position)
        {
            return NativeControl.SelectWord(position);
        }

        bool IRichTextBox.LayoutContent(bool onlyVisibleRect)
        {
            return NativeControl.LayoutContent(onlyVisibleRect);
        }

        bool IRichTextBox.MoveRight(int noPositions, RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveRight(noPositions, (int)flags);
        }

        bool IRichTextBox.MoveLeft(int noPositions, RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveLeft(noPositions, (int)flags);
        }

        bool IRichTextBox.MoveUp(int noLines, RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveUp(noLines, (int)flags);
        }

        bool IRichTextBox.MoveDown(int noLines, RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveDown(noLines, (int)flags);
        }

        bool IRichTextBox.MoveToLineEnd(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveToLineEnd((int)flags);
        }

        bool IRichTextBox.MoveToLineStart(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveToLineStart((int)flags);
        }

        bool IRichTextBox.MoveToParagraphEnd(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveToParagraphEnd((int)flags);
        }

        bool IRichTextBox.MoveToParagraphStart(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveToParagraphStart((int)flags);
        }

        bool IRichTextBox.MoveHome(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveHome((int)flags);
        }

        bool IRichTextBox.MoveEnd(RichTextMoveCaretFlags flags)
        {
            return NativeControl.MoveEnd((int)flags);
        }

        bool IRichTextBox.PageUp(int noPages, RichTextMoveCaretFlags flags)
        {
            return NativeControl.PageUp(noPages, (int)flags);
        }

        bool IRichTextBox.PageDown(int noPages, RichTextMoveCaretFlags flags)
        {
            return NativeControl.PageDown(noPages, (int)flags);
        }

        bool IRichTextBox.WordLeft(int noPages, RichTextMoveCaretFlags flags)
        {
            return NativeControl.WordLeft(noPages, (int)flags);
        }

        bool IRichTextBox.WordRight(int noPages, RichTextMoveCaretFlags flags)
        {
            return NativeControl.WordRight(noPages, (int)flags);
        }

        bool IRichTextBox.BeginBatchUndo(string cmdName)
        {
            return NativeControl.BeginBatchUndo(cmdName);
        }

        bool IRichTextBox.EndBatchUndo()
        {
            return NativeControl.EndBatchUndo();
        }

        bool IRichTextBox.BatchingUndo()
        {
            return NativeControl.BatchingUndo();
        }

        bool IRichTextBox.BeginSuppressUndo()
        {
            return NativeControl.BeginSuppressUndo();
        }

        bool IRichTextBox.EndSuppressUndo()
        {
            return NativeControl.EndSuppressUndo();
        }

        bool IRichTextBox.SuppressingUndo()
        {
            return NativeControl.SuppressingUndo();
        }

        void IRichTextBox.EnableVerticalScrollbar(bool enable)
        {
            NativeControl.EnableVerticalScrollbar(enable);
        }

        bool IRichTextBox.GetVerticalScrollbarEnabled()
        {
            return NativeControl.GetVerticalScrollbarEnabled();
        }

        void IRichTextBox.SetFontScale(Coord fontScale, bool refresh)
        {
            NativeControl.SetFontScale(fontScale, refresh);
        }

        Coord IRichTextBox.GetFontScale()
        {
            return NativeControl.GetFontScale();
        }

        bool IRichTextBox.GetVirtualAttributesEnabled()
        {
            return NativeControl.GetVirtualAttributesEnabled();
        }

        void IRichTextBox.EnableVirtualAttributes(bool b)
        {
            NativeControl.EnableVirtualAttributes(b);
        }

        void IRichTextBox.DoWriteText(string value, TextBoxSetValueFlags flags)
        {
            NativeControl.DoWriteText(value, (int)flags);
        }

        bool IRichTextBox.ExtendSelection(
            long oldPosition,
            long newPosition,
            RichTextMoveCaretFlags flags)
        {
            return NativeControl.ExtendSelection(oldPosition, newPosition, (int)flags);
        }

        void IRichTextBox.SetCaretPosition(long position, bool showAtLineStart)
        {
            NativeControl.SetCaretPosition(position, showAtLineStart);
        }

        long IRichTextBox.GetCaretPosition()
        {
            return NativeControl.GetCaretPosition();
        }

        long IRichTextBox.GetAdjustedCaretPosition(long caretPos)
        {
            return NativeControl.GetAdjustedCaretPosition(caretPos);
        }

        void IRichTextBox.MoveCaretForward(long oldPosition)
        {
            NativeControl.MoveCaretForward(oldPosition);
        }

        PointI IRichTextBox.GetPhysicalPoint(PointI ptLogical)
        {
            return NativeControl.GetPhysicalPoint(ptLogical);
        }

        PointI IRichTextBox.GetLogicalPoint(PointI ptPhysical)
        {
            return NativeControl.GetLogicalPoint(ptPhysical);
        }

        long IRichTextBox.FindNextWordPosition(int direction)
        {
            return NativeControl.FindNextWordPosition(direction);
        }

        bool IRichTextBox.IsPositionVisible(long pos)
        {
            return NativeControl.IsPositionVisible(pos);
        }

        long IRichTextBox.GetFirstVisiblePosition()
        {
            return NativeControl.GetFirstVisiblePosition();
        }

        long IRichTextBox.GetCaretPositionForDefaultStyle()
        {
            return NativeControl.GetCaretPositionForDefaultStyle();
        }

        void IRichTextBox.SetCaretPositionForDefaultStyle(long pos)
        {
            NativeControl.SetCaretPositionForDefaultStyle(pos);
        }

        void IRichTextBox.MoveCaretBack(long oldPosition)
        {
            NativeControl.MoveCaretBack(oldPosition);
        }

        bool IRichTextBox.IsDefaultStyleShowing()
        {
            return NativeControl.IsDefaultStyleShowing();
        }

        PointI IRichTextBox.GetFirstVisiblePoint()
        {
            return NativeControl.GetFirstVisiblePoint();
        }

        void IRichTextBox.EnableImages(bool b)
        {
            NativeControl.EnableImages(b);
        }

        bool IRichTextBox.GetImagesEnabled()
        {
            return NativeControl.GetImagesEnabled();
        }

        void IRichTextBox.EnableDelayedImageLoading(bool b)
        {
            NativeControl.EnableDelayedImageLoading(b);
        }

        bool IRichTextBox.GetDelayedImageLoading()
        {
            return NativeControl.GetDelayedImageLoading();
        }

        bool IRichTextBox.GetDelayedImageProcessingRequired()
        {
            return NativeControl.GetDelayedImageProcessingRequired();
        }

        void IRichTextBox.SetDelayedImageProcessingRequired(bool b)
        {
            NativeControl.SetDelayedImageProcessingRequired(b);
        }

        long IRichTextBox.GetDelayedImageProcessingTime()
        {
            return NativeControl.GetDelayedImageProcessingTime();
        }

        void IRichTextBox.SetDelayedImageProcessingTime(long t)
        {
            NativeControl.SetDelayedImageProcessingTime(t);
        }

        void IRichTextBox.SetLineHeight(int height)
        {
            NativeControl.SetLineHeight(height);
        }

        int IRichTextBox.GetLineHeight()
        {
            return NativeControl.GetLineHeight();
        }

        bool IRichTextBox.ProcessDelayedImageLoading(bool refresh)
        {
            return NativeControl.ProcessDelayedImageLoading(refresh);
        }

        void IRichTextBox.RequestDelayedImageProcessing()
        {
            NativeControl.RequestDelayedImageProcessing();
        }

        ITextBoxRichAttr IRichTextBox.CreateUrlAttr()
        {
            return Control?.CreateUrlAttr() ?? new PlessTextBoxRichAttr();
        }

        bool IRichTextBox.Delete(long startRange, long endRange)
        {
            return NativeControl.Delete(startRange, endRange);
        }

        void IRichTextBox.SetSelectionRange(long startRange, long endRange)
        {
            NativeControl.SetSelectionRange(startRange, endRange);
        }

        long IRichTextBox.DeleteSelectedContent()
        {
            return NativeControl.DeleteSelectedContent();
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

        /// <summary>
        /// Creates text attributes object.
        /// </summary>
        /// <returns></returns>
        public ITextBoxTextAttr CreateTextAttr()
        {
            return new TextBoxTextAttr();
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

        /// <summary>
        /// Sets the attributes for the given range.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// </remarks>
        public bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return NativeControl.SetStyle(start, end, s.Handle);
        }

        /// <summary>
        /// Gets the basic (overall) style.
        /// </summary>
        /// <remarks>
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// </remarks>
        public ITextBoxRichAttr GetBasicStyle()
        {
            var result = NativeControl.GetBasicStyle();
            return new TextBoxRichAttr(result);
        }

        /// <summary>
        /// Sets the basic (overall) style.
        /// </summary>
        /// <remarks>
        /// This is the style of the whole buffer before further styles are applied,
        /// unlike the default style, which only affects the style currently being
        /// applied (for example, setting the default style to bold will cause
        /// subsequently inserted text to be bold).
        /// </remarks>
        public void SetBasicStyle(ITextBoxRichAttr style)
        {
            if (style is TextBoxRichAttr s)
                NativeControl.SetBasicStyle(s.Handle);
        }

        /// <summary>
        /// Sets the text (normal) cursor.
        /// </summary>
        public void SetTextCursor(Cursor? cursor)
        {
            NativeControl.SetTextCursor(WxCursorHandler.CursorToPtr(cursor));
        }

        /// <summary>
        /// Returns the text (normal) cursor.
        /// </summary>
        public Cursor GetTextCursor()
        {
            var handler = new WxCursorHandler(NativeControl.GetTextCursor(), true);
            return new Cursor(handler);
        }

        /// <summary>
        /// Sets the cursor to be used over URLs.
        /// </summary>
        public void SetURLCursor(Cursor? cursor)
        {
            NativeControl.SetURLCursor(WxCursorHandler.CursorToPtr(cursor));
        }

        /// <summary>
        /// Sets the attributes for the given range, passing flags to determine how the
        /// attributes are set.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// <paramref name="flags"/> may contain a bit list of the following values:
        /// <see cref="RichTextSetStyleFlags.None"/>,
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>,
        /// <see cref="RichTextSetStyleFlags.Optimize"/>,
        /// <see cref="RichTextSetStyleFlags.ParagraphsOnly"/>,
        /// <see cref="RichTextSetStyleFlags.CharactersOnly"/>,
        /// <see cref="RichTextSetStyleFlags.Reset"/>,
        /// <see cref="RichTextSetStyleFlags.Remove"/>.
        /// </remarks>
        public bool SetStyleEx(
            long startRange,
            long endRange,
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            if (style is not TextBoxRichAttr s)
                return false;
            return NativeControl.SetStyleEx(startRange, endRange, s.Handle, (int)flags);
        }

        /// <summary>
        /// Gets the attributes at the given position.
        /// </summary>
        /// <remarks>
        /// This function gets the combined style - that is, the style you see on the
        /// screen as a result of combining base style, paragraph style and character
        /// style attributes.
        /// To get the character or paragraph style alone, use GetUncombinedStyle.
        /// </remarks>
        public ITextBoxTextAttr GetStyle(long position)
        {
            var result = NativeControl.GetStyle(position);
            return new TextBoxTextAttr(result);
        }

        /// <summary>
        /// Applies one or more <see cref="TextBoxTextAttrEffects"/> flags to the selection (undoable).
        /// If there is no selection, it is applied to the default style.
        /// </summary>
        public bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags)
        {
            return NativeControl.ApplyTextEffectToSelection((int)flags);
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
        /// Begins a numbered bullet.
        /// </summary>
        /// <param name="bulletNumber">A number, usually starting with 1</param>
        /// <param name="leftIndent">A value in tenths of a millimeter.</param>
        /// <param name="leftSubIndent">A value in tenths of a millimeter.</param>
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
        /// Saves the buffer content using the given type.
        /// </summary>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        /// <param name="file">File name.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        public bool SaveToFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return NativeControl.SaveFile(file, (int)type);
        }

        /// <summary>
        /// Sets the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// </summary>
        /// <param name="style">The style for the new text.</param>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred (this may
        /// also mean that the styles are not supported under this platform).
        /// </returns>
        public virtual bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            if (style is not TextBoxTextAttr s)
                return false;
            return NativeControl.SetDefaultStyle(s.Handle);
        }

        /// <inheritdoc cref="SetDefaultStyle(ITextBoxTextAttr)"/>
        public bool SetDefaultRichStyle(ITextBoxRichAttr style)
        {
            if (style is not TextBoxRichAttr s)
                return false;
            return NativeControl.SetDefaultRichStyle(s.Handle);
        }

        /// <summary>
        /// Loads content into the control's buffer using the given type.
        /// </summary>
        /// <param name="file">File name.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// If the specified type is <see cref="RichTextFileType.Any"/>, the type is deduced from
        /// the filename extension.
        /// </remarks>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup load options.
        /// </remarks>
        public bool LoadFromFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return NativeControl.LoadFile(file, (int)type);
        }

        /// <summary>
        /// Saves the buffer content to <see cref="Stream"/> using the given data type.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred.
        /// </returns>
        public bool SaveToStream(Stream stream, RichTextFileType type)
        {
            using var outputStream = new UI.Native.OutputStream(stream);
            return NativeControl.SaveToStream(outputStream, (int)type);
        }

        /// <summary>
        /// Loads the buffer content from <see cref="Stream"/> using the given data type.
        /// </summary>
        /// <param name="stream">Output stream.</param>
        /// <param name="type">File type.</param>
        /// <remarks>
        /// Use <see cref="SetFileHandlerFlags"/> to setup save options.
        /// </remarks>
        /// <returns>
        /// <c>true</c> on success, <c>false</c> if an error occurred.
        /// </returns>
        public bool LoadFromStream(Stream stream, RichTextFileType type)
        {
            using var inputStream = new UI.Native.InputStream(stream);
            return NativeControl.LoadFromStream(inputStream, (int)type);
        }

        /// <summary>
        /// Sets flags that change the behaviour of loading or saving.
        /// See the documentation for each handler class to see what flags are
        /// relevant for each handler.
        /// </summary>
        public void SetFileHandlerFlags(RichTextHandlerFlags knownFlags, int customFlags = 0)
        {
            var flags = customFlags | (int)knownFlags;
            NativeControl.SetHandlerFlags(flags);
        }

        /// <summary>
        /// Begins using the given point size.
        /// </summary>
        public bool BeginFontSize(int pointSize)
        {
            return NativeControl.BeginFontSize(pointSize);
        }

        /// <summary>
        /// Begins using the given point size.
        /// </summary>
        public bool BeginFontSize(Coord pointSize)
        {
            return NativeControl.BeginFontSize((int)pointSize);
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
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the current caret position, has the supplied effects flag(s).
        /// </summary>
        public bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag)
        {
            return NativeControl.DoesSelectionHaveTextEffectFlag((int)flag);
        }

        /// <summary>
        /// Returns <c>true</c> if all of the selection, or the content
        /// at the caret position, is aligned according to the specified flag.
        /// </summary>
        public bool IsSelectionAligned(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.IsSelectionAligned((int)alignment);
        }

        /// <summary>
        /// Applies the given alignment to the selection or the default style (undoable).
        /// </summary>
        public bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment)
        {
            return NativeControl.ApplyAlignmentToSelection((int)alignment);
        }

        /// <summary>
        /// Gets the attributes at the given position.
        /// </summary>
        /// <remarks>
        /// This function gets the combined style - that is, the style you see on the
        /// screen as a result of combining base style, paragraph style and character
        /// style attributes.
        /// To get the character or paragraph style alone, use GetUncombinedStyle.
        /// </remarks>
        public ITextBoxRichAttr GetRichStyle(long position)
        {
            var result = NativeControl.GetRichStyle(position);
            return new TextBoxRichAttr(result);
        }

        /// <summary>
        /// Write a table at the current insertion point, returning the table.
        /// </summary>
        public object WriteTable(
            int rows,
            int cols,
            ITextBoxRichAttr? tableAttr = default,
            ITextBoxRichAttr? cellAttr = default)
        {
            IntPtr tableAttrPtr = default;
            IntPtr cellAttrPtr = default;

            if (tableAttr is TextBoxRichAttr ta)
                tableAttrPtr = ta.Handle;

            if (cellAttr is TextBoxRichAttr ca)
                cellAttrPtr = ca.Handle;

            return NativeControl.WriteTable(
                rows,
                cols,
                tableAttrPtr,
                cellAttrPtr);
        }

        /// <summary>
        /// Numbers the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool NumberList(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.NumberList2(
                startRange,
                endRange,
                defName,
                (int)flags,
                startFrom,
                specifiedLevel);
        }

        /// <summary>
        /// Sets the list attributes for the given range, passing flags to determine how
        /// the attributes are set.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.SetListStyle2(
                startRange,
                endRange,
                defName,
                (int)flags,
                startFrom,
                specifiedLevel);
        }

        /// <summary>
        /// Returns the current default style, which can be used to change how subsequently
        /// inserted text is displayed.
        /// </summary>
        public ITextBoxRichAttr GetDefaultStyleEx()
        {
            var result = NativeControl.GetDefaultStyleEx();
            return new TextBoxRichAttr(result);
        }

        /// <summary>
        /// Clears the list style from the given range, clearing list-related attributes
        /// and applying any named paragraph style associated with each paragraph.
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="endRange"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will be undoable.
        /// </remarks>
        public bool ClearListStyle(
            long startRange,
            long endRange,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            return NativeControl.ClearListStyle(startRange, endRange, (int)flags);
        }

        /// <summary>
        /// Begins using this font.
        /// </summary>
        public bool BeginFont(Font? font)
        {
            return NativeControl.BeginFont((UI.Native.Font?)font?.Handler);
        }

        /// <summary>
        /// Promotes or demotes the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// A positive <paramref name="promoteBy"/> produces a smaller indent, and a negative number
        /// produces a larger indent. Pass flags to determine how the attributes are set.
        /// </remarks>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from start number, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1)
        {
            return NativeControl.PromoteList2(
                promoteBy,
                startRange,
                endRange,
                defName,
                (int)flags,
                specifiedLevel);
        }

        /// <summary>
        /// Returns the cursor to be used over URLs.
        /// </summary>
        public Cursor GetURLCursor()
        {
            var handler = new WxCursorHandler(NativeControl.GetURLCursor(), true);
            return new Cursor(handler);
        }

        /// <summary>
        /// Sets <paramref name="attr"/> as the default style and tells the control that the UI should
        /// reflect this attribute until the user moves the caret.
        /// </summary>
        public void SetAndShowDefaultStyle(ITextBoxRichAttr attr)
        {
            if (attr is TextBoxRichAttr s)
                NativeControl.SetAndShowDefaultStyle(s.Handle);
        }

        /// <summary>
        /// Test if this whole range has paragraph attributes of the specified kind.
        /// </summary>
        /// <remarks>
        /// If any of the attributes are different within the range, the test fails.
        /// You can use this to implement, for example, centering button updating.
        /// <paramref name="style"/> must have flags indicating which attributes are of interest.
        /// </remarks>
        public bool HasParagraphAttributes(
            long startRange,
            long endRange,
            ITextBoxRichAttr style)
        {
            if (style is TextBoxRichAttr s)
            {
                return NativeControl.HasParagraphAttributes(
                    startRange,
                    endRange,
                    s.Handle);
            }
            else
                return false;
        }

        /// <summary>
        /// Sets the attributes for the given range.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the style for a character at
        /// position 5, use the range (5,6).
        /// </remarks>
        public bool SetRichStyle(long start, long end, ITextBoxRichAttr style)
        {
            if (style is not TextBoxRichAttr s)
                return false;
            return NativeControl.SetRichStyle(start, end, s.Handle);
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

        public bool ApplyStyleToSelection(ITextBoxRichAttr style, RichTextSetStyleFlags flags)
        {
            if (style is not TextBoxRichAttr s)
                return false;
            return NativeControl.ApplyStyleToSelection(s.Handle, (int)flags);
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

        /// <summary>
        /// Returns the range of the current selection.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one.
        /// If the return values 'from' and 'to' are the same, there is no selection.
        /// </remarks>
        public IntPtr GetSelection()
        {
            return NativeControl.GetSelection();
        }

        /// <summary>
        /// Returns an object that stores information about context menu property item(s),
        /// in order to communicate between the context menu event handler and the code
        /// that responds to it.
        /// </summary>
        /// <remarks>
        /// The result stores one
        /// item for each object that could respond to a property-editing event. If
        /// objects are nested, several might be editable.
        /// </remarks>
        public IntPtr GetContextMenuPropertiesInfo()
        {
            return NativeControl.GetContextMenuPropertiesInfo();
        }

        public void SetSelection(IntPtr sel)
        {
            NativeControl.SetSelection2(sel);
        }

        /// <summary>
        /// Writes an image block at the current insertion point.
        /// </summary>
        public bool WriteImage(IntPtr imageBlock, IntPtr textAttr = default)
        {
            return NativeControl.WriteImage3(imageBlock, textAttr);
        }

        /// <summary>
        /// Writes a field at the current insertion point.
        /// </summary>
        /// <param name="fieldType">The field type, matching an existing field type definition.</param>
        /// <param name="properties">Extra data for the field.</param>
        /// <param name="textAttr">Optional attributes.</param>
        /// <returns></returns>
        public IntPtr WriteField(
            string fieldType,
            IntPtr properties,
            IntPtr textAttr = default)
        {
            return NativeControl.WriteField(
                fieldType,
                properties,
                textAttr);
        }

        /// <summary>
        /// Can we delete this range?
        /// Sends an event to the control.
        /// </summary>
        public bool CanDeleteRange(IntPtr container, long startRange, long endRange)
        {
            return NativeControl.CanDeleteRange(container, startRange, endRange);
        }

        /// <summary>
        /// Can we insert content at this position?
        /// Sends an event to the control.
        /// </summary>
        public bool CanInsertContent(IntPtr container, long pos)
        {
            return NativeControl.CanInsertContent(container, pos);
        }

        /// <summary>
        /// Sets the object that currently has the editing focus.
        /// </summary>
        public bool SetFocusObject(IntPtr richObj, bool setCaretPosition = true)
        {
            return NativeControl.SetFocusObject(richObj, setCaretPosition);
        }

        /// <summary>
        /// Returns the current context menu.
        /// </summary>
        public IntPtr GetContextMenu()
        {
            return NativeControl.GetContextMenu();
        }

        /// <summary>
        /// Sets the current context menu.
        /// </summary>
        public void SetContextMenu(IntPtr menu)
        {
            NativeControl.SetContextMenu(menu);
        }

        /// <summary>
        /// Returns the anchor object if selecting multiple containers.
        /// </summary>
        public IntPtr GetSelectionAnchorObject()
        {
            return NativeControl.GetSelectionAnchorObject();
        }

        /// <summary>
        /// Sets the anchor object if selecting multiple containers.
        /// </summary>
        public void SetSelectionAnchorObject(IntPtr anchor)
        {
            NativeControl.SetSelectionAnchorObject(anchor);
        }

        /// <summary>
        /// Returns object that currently has the editing focus.
        /// If there are no composite objects, this will be the top-level buffer.
        /// </summary>
        public IntPtr GetFocusObject()
        {
            return NativeControl.GetFocusObject();
        }

        /// <summary>
        /// Sets focus object without making any alterations.
        /// </summary>
        public void StoreFocusObject(IntPtr richObj)
        {
            NativeControl.StoreFocusObject(richObj);
        }

        /// <summary>
        /// Applies the style sheet to the buffer, matching paragraph styles in the sheet
        /// against named styles in the buffer.
        /// This might be useful if the styles have changed.
        /// If sheet is null, the sheet set with SetStyleSheet() is used.
        /// Currently this applies paragraph styles only.
        /// </summary>
        public bool ApplyStyle(IntPtr def)
        {
            return NativeControl.ApplyStyle(def);
        } // wxRichTextStyleDefinition

        /// <summary>
        /// Sets the style sheet associated with the control.
        /// A style sheet allows named character and paragraph styles to be applied.
        /// </summary>
        public void SetStyleSheet(IntPtr styleSheet)
        {
            NativeControl.SetStyleSheet(styleSheet);
        }

        /// <summary>
        /// Move the caret to the given character position.
        /// Please note that this does not update the current editing style
        /// from the new position; to do that, call <see cref="SetInsertionPoint"/> instead.
        /// </summary>
        public bool MoveCaret(long pos, bool showAtLineStart = false, IntPtr container = default)
        {
            return NativeControl.MoveCaret(pos, showAtLineStart, container);
        }

        /// <summary>
        /// Push the style sheet to top of stack.
        /// </summary>
        public bool PushStyleSheet(IntPtr styleSheet)
        {
            return NativeControl.PushStyleSheet(styleSheet);
        }

        /// <summary>
        /// Pops the style sheet from top of stack.
        /// </summary>
        public IntPtr PopStyleSheet()
        {
            return NativeControl.PopStyleSheet();
        }

        /// <summary>
        /// Applies the style sheet to the buffer, for example if the styles have changed.
        /// </summary>
        public bool ApplyStyleSheet(IntPtr styleSheet = default)
        {
            return NativeControl.ApplyStyleSheet(styleSheet);
        }

        /// <summary>
        /// Shows the given context menu, optionally adding appropriate property-editing
        /// commands for the current position in the object hierarchy.
        /// </summary>
        public bool ShowContextMenu(IntPtr menu, PointI pt, bool addPropertyCommands = true)
        {
            return NativeControl.ShowContextMenu(menu, pt, addPropertyCommands);
        }

        /// <summary>
        /// Prepares the context menu, optionally adding appropriate property-editing commands.
        /// Returns the number of property commands added.
        /// </summary>
        public int PrepareContextMenu(IntPtr menu, PointI pt, bool addPropertyCommands = true)
        {
            return NativeControl.PrepareContextMenu(menu, pt, addPropertyCommands);
        }

        /// <summary>
        /// Returns <c>true</c> if we can edit the object's properties via a GUI.
        /// </summary>
        public bool CanEditProperties(IntPtr richObj)
        {
            return NativeControl.CanEditProperties(richObj);
        }

        /// <summary>
        /// Edits the object's properties via a GUI.
        /// </summary>
        public bool EditProperties(IntPtr richObj, IntPtr parentWindow)
        {
            return NativeControl.EditProperties(richObj, parentWindow);
        }

        /// <summary>
        /// Extends a table selection in the given direction.
        /// </summary>
        public bool ExtendCellSelection(IntPtr table, int noRowSteps, int noColSteps)
        {
            return NativeControl.ExtendCellSelection(table, noRowSteps, noColSteps);
        }

        /// <summary>
        /// Starts selecting table cells.
        /// </summary>
        public bool StartCellSelection(IntPtr table, IntPtr newCell)
        {
            return NativeControl.StartCellSelection(table, newCell);
        }

        /// <summary>
        /// Scrolls <paramref name="position"/> into view. This function takes a caret position.
        /// </summary>
        public bool ScrollIntoView(long position, int keyCode)
        {
            return NativeControl.ScrollIntoView(position, keyCode);
        }

        /// <summary>
        /// Returns the caret height and position for the given character position.
        /// If container is null, the current focus object will be used.
        /// </summary>
        public bool GetCaretPositionForIndex(
            long position,
            RectI rect,
            IntPtr container = default)
        {
            return NativeControl.GetCaretPositionForIndex(position, rect, container);
        }

        /// <summary>
        /// public helper function returning the line for the visible caret position.
        /// If the caret is shown at the very end of the line, it means the next character
        /// is actually on the following line.
        /// So this function gets the line we're expecting to find if this is the case.
        /// </summary>
        public IntPtr GetVisibleLineForCaretPosition(long caretPosition)
        {
            return NativeControl.GetVisibleLineForCaretPosition(caretPosition);
        }

        /// <summary>
        /// Gets the command processor associated with the control's buffer.
        /// </summary>
        public IntPtr GetCommandProcessor()
        {
            return NativeControl.GetCommandProcessor();
        }

        /// <summary>
        /// Sets up the caret for the given position and container, after a mouse click.
        /// </summary>
        public bool SetCaretPositionAfterClick(
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

        /// <summary>
        /// Gets the attributes at the given position.
        /// </summary>
        /// <remarks>
        /// This function gets the uncombined style - that is, the attributes associated
        /// with the paragraph or character content, and not necessarily the combined
        /// attributes you see on the screen.
        /// To get the combined attributes, use GetStyle().
        /// If you specify (any) paragraph attribute in style's flags, this function
        /// will fetch the paragraph attributes.
        /// Otherwise, it will return the character attributes.
        /// </remarks>
        public bool GetUncombinedStyle(long position, IntPtr style)
        {
            return NativeControl.GetUncombinedStyle(position, style);
        } // wxRichTextAttr& style param to result

        public bool GetUncombinedStyle(long position, IntPtr style, IntPtr container)
        {
            return NativeControl.GetUncombinedStyle2(position, style, container);
        } // wxRichTextAttr& style param to result

        public IntPtr GetStyleInContainer(long position, IntPtr container)
        {
            return NativeControl.GetStyleInContainer(position, container);
        }

        /// <summary>
        /// Sets the attributes for a single object.
        /// </summary>
        public void SetStyle(
            IntPtr richObj,
            IntPtr textAttr,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            NativeControl.SetStyle2(richObj, textAttr, (int)flags);
        }

        public IntPtr GetStyleForRange(long startRange, long endRange, IntPtr container)
        {
            return NativeControl.GetStyleForRange3(startRange, endRange, container);
        }

        /// <summary>
        /// Sets the list attributes for the given range, passing flags to determine how
        /// the attributes are set.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool SetListStyle(
            long startRange,
            long endRange,
            IntPtr def,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.SetListStyle(
                startRange,
                endRange,
                def,
                (int)flags,
                startFrom,
                specifiedLevel);
        } // wxRichTextListStyleDefinition

        /// <summary>
        /// Numbers the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from <paramref name="startFrom"/>, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool NumberList(
            long startRange,
            long endRange,
            IntPtr def = default,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1)
        {
            return NativeControl.NumberList(
                startRange,
                endRange,
                def,
                (int)flags,
                startFrom,
                specifiedLevel);
        } // wxRichTextListStyleDefinition

        /// <summary>
        /// Promotes or demotes the paragraphs in the given range.
        /// </summary>
        /// <remarks>
        /// A positive <paramref name="promoteBy"/> produces a smaller indent, and a negative number
        /// produces a larger indent. Pass flags to determine how the attributes are set.
        /// </remarks>
        /// <remarks>
        /// <paramref name="flags"/> is a bit list of the following:
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>: specifies that this command will
        /// be undoable.
        /// <see cref="RichTextSetStyleFlags.Renumber"/>: specifies that numbering
        /// should start from start number, otherwise existing attributes are used.
        /// <see cref="RichTextSetStyleFlags.SpecifyLevel"/>: specifies that
        /// <paramref name="specifiedLevel"/> should be used
        /// as the level for all paragraphs, otherwise the current indentation will be used.
        /// </remarks>
        public bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            IntPtr def = default,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1)
        {
            return NativeControl.PromoteList(
                promoteBy,
                startRange,
                endRange,
                def,
                (int)flags,
                specifiedLevel);
        } // wxRichTextListStyleDefinition

        /// <summary>
        /// Test if this whole range has character attributes of the specified kind.
        /// </summary>
        /// <remarks>
        /// If any of the attributes are different within the range, the test fails.
        /// You can use this to implement, for example, bold button updating.
        /// Style must have flags indicating which attributes are of interest.
        /// </remarks>
        public bool HasCharacterAttributes(long startRange, long endRange, ITextBoxRichAttr style)
        {
            if (style is TextBoxRichAttr s)
                return NativeControl.HasCharacterAttributes(startRange, endRange, s.Handle);
            else
                return false;
        }

        /// <summary>
        /// Returns the style sheet associated with the control, if any.
        /// A style sheet allows named character and paragraph styles to be applied.
        /// </summary>
        public IntPtr GetStyleSheet()
        {
            return NativeControl.GetStyleSheet();
        }

        /// <summary>
        /// Write a text box at the current insertion point, returning the text box.
        /// You can then call SetFocusObject() to set the focus to the new object.
        /// </summary>
        public IntPtr WriteTextBox(IntPtr textAttr = default)
        {
            return NativeControl.WriteTextBox(textAttr);
        }

        /// <summary>
        /// Sets the properties for the given range, passing flags to determine how the
        /// attributes are set. You can merge properties or replace them.
        /// </summary>
        /// <remarks>
        /// The end point of range is specified as the last character position of the span
        /// of text, plus one. So, for example, to set the properties for a character at
        /// position 5, use the range (5,6).
        /// </remarks>
        /// <remarks>
        /// <paramref name="flags"/> may contain a bit list of the following values:
        /// <see cref="RichTextSetStyleFlags.None"/>,
        /// <see cref="RichTextSetStyleFlags.WithUndo"/>,
        /// <see cref="RichTextSetStyleFlags.ParagraphsOnly"/>,
        /// <see cref="RichTextSetStyleFlags.CharactersOnly"/>,
        /// <see cref="RichTextSetStyleFlags.Reset"/>,
        /// <see cref="RichTextSetStyleFlags.Remove"/>.
        /// </remarks>
        public bool SetProperties(
            long startRange,
            long endRange,
            IntPtr properties,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            return NativeControl.SetProperties(
                startRange,
                endRange,
                properties,
                (int)flags);
        }

        /// <summary>
        /// Gets the current filename associated with the control.
        /// </summary>
        public string GetFileName()
        {
            return NativeControl.GetFilename();
        }

        /// <summary>
        /// Sets the current filename.
        /// </summary>
        /// <param name="filename"></param>
        public void SetFileName(string filename)
        {
            NativeControl.SetFilename(filename);
        }

        /// <summary>
        /// Returns the buffer associated with the control.
        /// </summary>
        public IntPtr GetBuffer()
        {
            return NativeControl.GetBuffer();
        }

        /// <summary>
        /// Gets the object's properties menu label.
        /// </summary>
        public string GetPropertiesMenuLabel(IntPtr richObj)
        {
            return NativeControl.GetPropertiesMenuLabel(richObj);
        }

        /// <summary>
        /// Writes a bitmap at the current insertion point.
        /// Supply an optional type to use for internal and file storage of the raw data.
        /// </summary>
        public bool WriteImage(
            Image? bitmap,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null)
        {
            if (bitmap is null)
                return false;
            if (textAttr is TextBoxRichAttr s)
                return NativeControl.WriteImage((UI.Native.Image)bitmap.Handler, (int)bitmapType, s.Handle);
            return NativeControl.WriteImage((UI.Native.Image)bitmap.Handler, (int)bitmapType, default);
        }

        /// <summary>
        /// Gets the attributes common to the specified range.
        /// Attributes that differ in value within the range will not be included
        /// in style flags.
        /// </summary>
        public ITextBoxTextAttr GetStyleForRange(long startRange, long endRange)
        {
            var result = NativeControl.GetStyleForRange(startRange, endRange);
            return new TextBoxTextAttr(result);
        }

        /// <summary>
        /// Gets the attributes common to the specified range.
        /// Attributes that differ in value within the range will not be included
        /// in style flags.
        /// </summary>
        public ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange)
        {
            var result = NativeControl.GetStyleForRange2(startRange, endRange);
            return new TextBoxRichAttr(result);
        }

        /// <summary>
        /// Loads an image from a file and writes it at the current insertion point.
        /// </summary>
        public bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null)
        {
            if (textAttr is TextBoxRichAttr s)
                return NativeControl.WriteImage2(filename, (int)bitmapType, s.Handle);
            return NativeControl.WriteImage2(filename, (int)bitmapType, default);
        }

        /// <summary>
        /// Creates new custom rich text style.
        /// </summary>
        /// <returns></returns>
        public ITextBoxRichAttr CreateRichAttr()
        {
            return new TextBoxRichAttr();
        }

        internal override Native.Control CreateNativeControl()
        {
            return new Native.RichTextBox();
        }
   }
}