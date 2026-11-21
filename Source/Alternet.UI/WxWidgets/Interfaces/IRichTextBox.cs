using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the <see cref="RichTextBox"/> methods and properties.
    /// </summary>
    public interface IRichTextBox : ISimpleRichTextBox
    {
        /// <inheritdoc cref="RichTextBox.HasBorder"/>
        bool HasBorder { get; set; }

        /// <inheritdoc cref="RichTextBox.Remove"/>
        void Remove(long from, long to);

        /// <inheritdoc cref="RichTextBox.SetDragging"/>
        void SetDragging(bool dragging);

        /// <inheritdoc cref="RichTextBox.GetSelectionAnchor"/>
        long GetSelectionAnchor();

        /// <inheritdoc cref="RichTextBox.SetSelectionAnchor"/>
        void SetSelectionAnchor(long anchor);

        /// <inheritdoc cref="RichTextBox.Clear"/>
        void Clear();

        /// <inheritdoc cref="RichTextBox.Replace"/>
        void Replace(long from, long to, string value);

        /// <inheritdoc cref="RichTextBox.GetValue"/>
        string GetValue();

        /// <inheritdoc cref="RichTextBox.SetValue"/>
        void SetValue(string value);

        /// <inheritdoc cref="RichTextBox.CreateRichAttr"/>
        ITextBoxRichAttr CreateRichAttr();

        /// <inheritdoc cref="RichTextBox.GetRange"/>
        string GetRange(long from, long to);

        /// <inheritdoc cref="RichTextBox.GetLineLength"/>
        int GetLineLength(long lineNo);

        /// <inheritdoc cref="RichTextBox.GetLineText"/>
        string GetLineText(long lineNo);

        /// <inheritdoc cref="RichTextBox.GetNumberOfLines"/>
        int GetNumberOfLines();

        /// <inheritdoc cref="RichTextBox.IsModified"/>
        bool IsModified();

        /// <inheritdoc cref="RichTextBox.IsEditable"/>
        bool IsEditable();

        /// <inheritdoc cref="RichTextBox.IsSingleLine"/>
        bool IsSingleLine();

        /// <inheritdoc cref="RichTextBox.IsMultiLine"/>
        bool IsMultiLine();

        /// <inheritdoc cref="RichTextBox.GetStringSelection"/>
        string GetStringSelection();

        /// <inheritdoc cref="RichTextBox.SetDelayedLayoutThreshold"/>
        void SetDelayedLayoutThreshold(long threshold);

        /// <inheritdoc cref="RichTextBox.GetDelayedLayoutThreshold"/>
        long GetDelayedLayoutThreshold();

        /// <inheritdoc cref="RichTextBox.GetFullLayoutRequired"/>
        bool GetFullLayoutRequired();

        /// <inheritdoc cref="RichTextBox.SetFullLayoutRequired"/>
        void SetFullLayoutRequired(bool b);

        /// <inheritdoc cref="RichTextBox.GetFullLayoutTime"/>
        long GetFullLayoutTime();

        /// <inheritdoc cref="RichTextBox.SetFullLayoutTime"/>
        void SetFullLayoutTime(long t);

        /// <inheritdoc cref="RichTextBox.GetFullLayoutSavedPosition"/>
        long GetFullLayoutSavedPosition();

        /// <inheritdoc cref="RichTextBox.SetDefaultStyle"/>
        bool SetDefaultStyle(ITextBoxTextAttr style);

        /// <inheritdoc cref="RichTextBox.SetDefaultRichStyle"/>
        bool SetDefaultRichStyle(ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox.SetFullLayoutSavedPosition"/>
        void SetFullLayoutSavedPosition(long p);

        /// <inheritdoc cref="RichTextBox.ForceDelayedLayout"/>
        void ForceDelayedLayout();

        /// <inheritdoc cref="RichTextBox.GetCaretAtLineStart"/>
        bool GetCaretAtLineStart();

        /// <inheritdoc cref="RichTextBox.SetCaretAtLineStart"/>
        void SetCaretAtLineStart(bool atStart);

        /// <inheritdoc cref="RichTextBox.GetDragging"/>
        bool GetDragging();

        /// <inheritdoc cref="RichTextBox.LoadFromFile"/>
        bool LoadFromFile(string file, RichTextFileType type = RichTextFileType.Any);

        /// <inheritdoc cref="RichTextBox.SaveToFile"/>
        bool SaveToFile(string file, RichTextFileType type = RichTextFileType.Any);

        /// <inheritdoc cref="RichTextBox.SaveToStream"/>
        bool SaveToStream(Stream stream, RichTextFileType type);

        /// <inheritdoc cref="RichTextBox.LoadFromStream"/>
        bool LoadFromStream(Stream stream, RichTextFileType type);

        /// <inheritdoc cref="RichTextBox.SetFileHandlerFlags"/>
        void SetFileHandlerFlags(RichTextHandlerFlags knownFlags, int customFlags = 0);

        /// <inheritdoc cref="RichTextBox.GetFileHandlerFlags"/>
        int GetFileHandlerFlags();

        /// <inheritdoc cref="RichTextBox.MarkDirty"/>
        void MarkDirty();

        /// <inheritdoc cref="RichTextBox.DiscardEdits"/>
        void DiscardEdits();

        /// <inheritdoc cref="RichTextBox.SetMaxLength"/>
        void SetMaxLength(ulong len);

        /// <inheritdoc cref="RichTextBox.WriteText"/>
        void WriteText(string text);

        /// <inheritdoc cref="RichTextBox.AppendText"/>
        void AppendText(string text);

        /// <inheritdoc cref="RichTextBox.Copy"/>
        void Copy();

        /// <inheritdoc cref="RichTextBox.Cut"/>
        void Cut();

        /// <inheritdoc cref="RichTextBox.Paste"/>
        void Paste();

        /// <inheritdoc cref="RichTextBox.DeleteSelection"/>
        void DeleteSelection();

        /// <inheritdoc cref="RichTextBox.CanCopy"/>
        bool CanCopy();

        /// <inheritdoc cref="RichTextBox.CanCut"/>
        bool CanCut();

        /// <inheritdoc cref="RichTextBox.CanPaste"/>
        bool CanPaste();

        /// <inheritdoc cref="RichTextBox.CanDeleteSelection"/>
        bool CanDeleteSelection();

        /// <inheritdoc cref="RichTextBox.Undo"/>
        void Undo();

        /// <inheritdoc cref="RichTextBox.Redo"/>
        void Redo();

        /// <inheritdoc cref="RichTextBox.CanUndo"/>
        bool CanUndo();

        /// <inheritdoc cref="RichTextBox.CanRedo"/>
        bool CanRedo();

        /// <inheritdoc cref="RichTextBox.SetInsertionPointEnd"/>
        void SetInsertionPointEnd();

        /// <inheritdoc cref="RichTextBox.GetInsertionPoint"/>
        long GetInsertionPoint();

        /// <inheritdoc cref="RichTextBox.SetSelection"/>
        void SetSelection(long from, long to);

        /// <inheritdoc cref="RichTextBox.SetEditable"/>
        void SetEditable(bool editable);

        /// <inheritdoc cref="RichTextBox.HasSelection"/>
        bool HasSelection();

        /// <inheritdoc cref="RichTextBox.HasUnfocusedSelection"/>
        bool HasUnfocusedSelection();

        /// <inheritdoc cref="RichTextBox.NewLine()"/>
        bool NewLine();

        /// <inheritdoc cref="RichTextBox.LineBreak"/>
        bool LineBreak();

        /// <inheritdoc cref="RichTextBox.EndStyle"/>
        bool EndStyle();

        /// <inheritdoc cref="RichTextBox.EndAllStyles"/>
        bool EndAllStyles();

        /// <inheritdoc cref="RichTextBox.BeginBold"/>
        bool BeginBold();

        /// <inheritdoc cref="RichTextBox.EndBold"/>
        bool EndBold();

        /// <inheritdoc cref="RichTextBox.BeginItalic"/>
        bool BeginItalic();

        /// <inheritdoc cref="RichTextBox.EndItalic"/>
        bool EndItalic();

        /// <inheritdoc cref="RichTextBox.BeginUnderline"/>
        bool BeginUnderline();

        /// <inheritdoc cref="RichTextBox.EndUnderline"/>
        bool EndUnderline();

        /// <inheritdoc cref="RichTextBox.BeginFontSize(int)"/>
        bool BeginFontSize(int pointSize);

        /// <inheritdoc cref="RichTextBox.BeginFontSize(Coord)"/>
        bool BeginFontSize(Coord pointSize);

        /// <inheritdoc cref="RichTextBox.EndFontSize"/>
        bool EndFontSize();

        /// <inheritdoc cref="RichTextBox.ApplyStyleToSelection"/>
        bool ApplyStyleToSelection(ITextBoxRichAttr style, RichTextSetStyleFlags flags);

        /// <inheritdoc cref="RichTextBox.EndFont"/>
        bool EndFont();

        /// <inheritdoc cref="RichTextBox.GetFileName"/>
        string GetFileName();

        /// <inheritdoc cref="RichTextBox.SetFileName"/>
        void SetFileName(string filename);

        /// <inheritdoc cref="RichTextBox.BeginAlignment"/>
        bool BeginAlignment(TextBoxTextAttrAlignment alignment);

        /// <inheritdoc cref="RichTextBox.EndAlignment"/>
        bool EndAlignment();

        /// <inheritdoc cref="RichTextBox.BeginLeftIndent"/>
        bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0);

        /// <inheritdoc cref="RichTextBox.EndLeftIndent"/>
        bool EndLeftIndent();

        /// <inheritdoc cref="RichTextBox.BeginRightIndent"/>
        bool BeginRightIndent(int rightIndent);

        /// <inheritdoc cref="RichTextBox.EndRightIndent"/>
        bool EndRightIndent();

        /// <inheritdoc cref="RichTextBox.BeginParagraphSpacing"/>
        bool BeginParagraphSpacing(int before, int after);

        /// <inheritdoc cref="RichTextBox.EndParagraphSpacing"/>
        bool EndParagraphSpacing();

        /// <inheritdoc cref="RichTextBox.BeginLineSpacing"/>
        bool BeginLineSpacing(int lineSpacing);

        /// <inheritdoc cref="RichTextBox.EndLineSpacing"/>
        bool EndLineSpacing();

        /// <inheritdoc cref="RichTextBox.BeginNumberedBullet"/>
        bool BeginNumberedBullet(
            int bulletNumber,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle
                = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period);

        /// <inheritdoc cref="RichTextBox.EndNumberedBullet"/>
        bool EndNumberedBullet();

        /// <inheritdoc cref="RichTextBox.BeginSymbolBullet"/>
        bool BeginSymbolBullet(
            string symbol,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Symbol);

        /// <inheritdoc cref="RichTextBox.EndSymbolBullet"/>
        bool EndSymbolBullet();

        /// <inheritdoc cref="RichTextBox.BeginStandardBullet"/>
        bool BeginStandardBullet(
            string bulletName,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Standard);

        /// <inheritdoc cref="RichTextBox.EndStandardBullet"/>
        bool EndStandardBullet();

        /// <inheritdoc cref="RichTextBox.BeginCharacterStyle"/>
        bool BeginCharacterStyle(string characterStyle);

        /// <inheritdoc cref="RichTextBox.EndCharacterStyle"/>
        bool EndCharacterStyle();

        /// <inheritdoc cref="RichTextBox.BeginParagraphStyle"/>
        bool BeginParagraphStyle(string paragraphStyle);

        /// <inheritdoc cref="RichTextBox.EndParagraphStyle"/>
        bool EndParagraphStyle();

        /// <inheritdoc cref="RichTextBox.BeginListStyle"/>
        bool BeginListStyle(string listStyle, int level = 1, int number = 1);

        /// <inheritdoc cref="RichTextBox.EndListStyle"/>
        bool EndListStyle();

        /// <inheritdoc cref="RichTextBox.BeginURL"/>
        bool BeginURL(string url, string? characterStyle = default);

        /// <inheritdoc cref="RichTextBox.EndURL"/>
        bool EndURL();

        /// <inheritdoc cref="RichTextBox.IsSelectionBold"/>
        bool IsSelectionBold();

        /// <inheritdoc cref="RichTextBox.IsSelectionItalics"/>
        bool IsSelectionItalics();

        /// <inheritdoc cref="RichTextBox.IsSelectionUnderlined"/>
        bool IsSelectionUnderlined();

        /// <inheritdoc cref="RichTextBox.DoesSelectionHaveTextEffectFlag"/>
        bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag);

        /// <inheritdoc cref="RichTextBox.IsSelectionAligned"/>
        bool IsSelectionAligned(TextBoxTextAttrAlignment alignment);

        /// <inheritdoc cref="RichTextBox.ApplyBoldToSelection"/>
        bool ApplyBoldToSelection();

        /// <inheritdoc cref="RichTextBox.ApplyItalicToSelection"/>
        bool ApplyItalicToSelection();

        /// <inheritdoc cref="RichTextBox.ApplyUnderlineToSelection"/>
        bool ApplyUnderlineToSelection();

        /// <inheritdoc cref="RichTextBox.ApplyTextEffectToSelection"/>
        bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags);

        /// <inheritdoc cref="RichTextBox.ApplyAlignmentToSelection"/>
        bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment);

        /// <inheritdoc cref="RichTextBox.SetDefaultStyleToCursorStyle"/>
        bool SetDefaultStyleToCursorStyle();

        /// <inheritdoc cref="RichTextBox.SelectNone"/>
        void SelectNone();

        /// <inheritdoc cref="RichTextBox.SelectWord"/>
        bool SelectWord(long position);

        /// <inheritdoc cref="RichTextBox.LayoutContent"/>
        bool LayoutContent(bool onlyVisibleRect = false);

        /// <inheritdoc cref="RichTextBox.MoveRight"/>
        bool MoveRight(int noPositions = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveLeft"/>
        bool MoveLeft(int noPositions = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveUp"/>
        bool MoveUp(int noLines = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveDown"/>
        bool MoveDown(int noLines = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveToLineEnd"/>
        bool MoveToLineEnd(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveToLineStart"/>
        bool MoveToLineStart(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveToParagraphEnd"/>
        bool MoveToParagraphEnd(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveToParagraphStart"/>
        bool MoveToParagraphStart(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveHome"/>
        bool MoveHome(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.MoveEnd"/>
        bool MoveEnd(RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.PageUp"/>
        bool PageUp(int noPages = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.PageDown"/>
        bool PageDown(int noPages = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.WordLeft"/>
        bool WordLeft(int noPages = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.WordRight"/>
        bool WordRight(int noPages = 1, RichTextMoveCaretFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.BeginBatchUndo"/>
        bool BeginBatchUndo(string cmdName);

        /// <inheritdoc cref="RichTextBox.EndBatchUndo"/>
        bool EndBatchUndo();

        /// <inheritdoc cref="RichTextBox.BatchingUndo"/>
        bool BatchingUndo();

        /// <inheritdoc cref="RichTextBox.BeginSuppressUndo"/>
        bool BeginSuppressUndo();

        /// <inheritdoc cref="RichTextBox.EndSuppressUndo"/>
        bool EndSuppressUndo();

        /// <inheritdoc cref="RichTextBox.GetDefaultStyleEx"/>
        ITextBoxRichAttr GetDefaultStyleEx();

        /// <inheritdoc cref="RichTextBox.SuppressingUndo"/>
        bool SuppressingUndo();

        /// <inheritdoc cref="RichTextBox.EnableVerticalScrollbar"/>
        void EnableVerticalScrollbar(bool enable);

        /// <inheritdoc cref="RichTextBox.GetVerticalScrollbarEnabled"/>
        bool GetVerticalScrollbarEnabled();

        /// <inheritdoc cref="RichTextBox.SetFontScale"/>
        void SetFontScale(Coord fontScale, bool refresh = false);

        /// <inheritdoc cref="RichTextBox.GetFontScale"/>
        Coord GetFontScale();

        /// <inheritdoc cref="RichTextBox.GetVirtualAttributesEnabled"/>
        bool GetVirtualAttributesEnabled();

        /// <inheritdoc cref="RichTextBox.EnableVirtualAttributes"/>
        void EnableVirtualAttributes(bool b);

        /// <inheritdoc cref="RichTextBox.DoWriteText"/>
        void DoWriteText(string value, TextBoxSetValueFlags flags = 0);

        /// <inheritdoc cref="RichTextBox.ExtendSelection"/>
        bool ExtendSelection(long oldPosition, long newPosition, RichTextMoveCaretFlags flags);

        /// <inheritdoc cref="RichTextBox.SetCaretPosition"/>
        void SetCaretPosition(long position, bool showAtLineStart = false);

        /// <inheritdoc cref="RichTextBox.GetCaretPosition"/>
        long GetCaretPosition();

        /// <inheritdoc cref="RichTextBox.GetAdjustedCaretPosition"/>
        long GetAdjustedCaretPosition(long caretPos);

        /// <inheritdoc cref="RichTextBox.MoveCaretForward"/>
        void MoveCaretForward(long oldPosition);

        /// <inheritdoc cref="RichTextBox.GetPhysicalPoint"/>
        PointI GetPhysicalPoint(PointI ptLogical);

        /// <inheritdoc cref="RichTextBox.GetLogicalPoint"/>
        PointI GetLogicalPoint(PointI ptPhysical);

        /// <inheritdoc cref="RichTextBox.FindNextWordPosition"/>
        long FindNextWordPosition(int direction = 1);

        /// <inheritdoc cref="RichTextBox.IsPositionVisible"/>
        bool IsPositionVisible(long pos);

        /// <inheritdoc cref="RichTextBox.GetFirstVisiblePosition"/>
        long GetFirstVisiblePosition();

        /// <inheritdoc cref="RichTextBox.GetCaretPositionForDefaultStyle"/>
        long GetCaretPositionForDefaultStyle();

        /// <inheritdoc cref="RichTextBox.SetCaretPositionForDefaultStyle"/>
        void SetCaretPositionForDefaultStyle(long pos);

        /// <inheritdoc cref="RichTextBox.MoveCaretBack"/>
        void MoveCaretBack(long oldPosition);

        /// <inheritdoc cref="RichTextBox.BeginFont"/>
        bool BeginFont(Font? font);

        /// <inheritdoc cref="RichTextBox.IsDefaultStyleShowing"/>
        bool IsDefaultStyleShowing();

        /// <inheritdoc cref="RichTextBox.GetFirstVisiblePoint"/>
        PointI GetFirstVisiblePoint();

        /// <inheritdoc cref="RichTextBox.EnableImages"/>
        void EnableImages(bool b);

        /// <inheritdoc cref="RichTextBox.GetImagesEnabled"/>
        bool GetImagesEnabled();

        /// <inheritdoc cref="RichTextBox.EnableDelayedImageLoading"/>
        void EnableDelayedImageLoading(bool b);

        /// <inheritdoc cref="RichTextBox.GetDelayedImageLoading"/>
        bool GetDelayedImageLoading();

        /// <inheritdoc cref="RichTextBox.GetDelayedImageProcessingRequired"/>
        bool GetDelayedImageProcessingRequired();

        /// <inheritdoc cref="RichTextBox.SetDelayedImageProcessingRequired"/>
        void SetDelayedImageProcessingRequired(bool b);

        /// <inheritdoc cref="RichTextBox.GetDelayedImageProcessingTime"/>
        long GetDelayedImageProcessingTime();

        /// <inheritdoc cref="RichTextBox.SetDelayedImageProcessingTime"/>
        void SetDelayedImageProcessingTime(long t);

        /// <inheritdoc cref="RichTextBox.SetLineHeight"/>
        void SetLineHeight(int height);

        /// <inheritdoc cref="RichTextBox.GetLineHeight"/>
        int GetLineHeight();

        /// <inheritdoc cref="RichTextBox.ProcessDelayedImageLoading"/>
        bool ProcessDelayedImageLoading(bool refresh);

        /// <inheritdoc cref="RichTextBox.RequestDelayedImageProcessing"/>
        void RequestDelayedImageProcessing();

        /// <inheritdoc cref="RichTextBox.GetLastPosition"/>
        long GetLastPosition();

        /// <inheritdoc cref="RichTextBox.SetListStyle"/>
        bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox.ClearListStyle"/>
        bool ClearListStyle(
            long startRange,
            long endRange,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        /// <inheritdoc cref="RichTextBox.WriteTable"/>
        object? WriteTable(
            int rows,
            int cols,
            ITextBoxRichAttr? tableAttr = default,
            ITextBoxRichAttr? cellAttr = default);

        /// <inheritdoc cref="RichTextBox.NumberList"/>
        bool NumberList(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox.CreateUrlAttr"/>
        ITextBoxRichAttr CreateUrlAttr();

        /// <inheritdoc cref="RichTextBox.SetStyleEx"/>
        bool SetStyleEx(
            long startRange,
            long endRange,
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        /// <inheritdoc cref="RichTextBox.GetStyle"/>
        ITextBoxTextAttr GetStyle(long position);

        /// <inheritdoc cref="RichTextBox.GetRichStyle"/>
        ITextBoxRichAttr GetRichStyle(long position);

        /// <inheritdoc cref="RichTextBox.PromoteList"/>
        bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox.SetTextCursor"/>
        void SetTextCursor(Cursor? cursor);

        /// <inheritdoc cref="RichTextBox.GetTextCursor"/>
        Cursor GetTextCursor();

        /// <inheritdoc cref="RichTextBox.SetURLCursor"/>
        void SetURLCursor(Cursor? cursor);

        /// <inheritdoc cref="RichTextBox.GetURLCursor"/>
        Cursor GetURLCursor();

        /// <inheritdoc cref="RichTextBox.SetAndShowDefaultStyle"/>
        void SetAndShowDefaultStyle(ITextBoxRichAttr attr);

        /// <inheritdoc cref="RichTextBox.SetBasicStyle"/>
        void SetBasicStyle(ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox.HasParagraphAttributes"/>
        bool HasParagraphAttributes(
            long startRange,
            long endRange,
            ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox.BeginTextColor"/>
        bool BeginTextColor(Color color);

        /// <inheritdoc cref="RichTextBox.EndTextColor"/>
        bool EndTextColor();

        /// <inheritdoc cref="RichTextBox.GetBasicStyle"/>
        ITextBoxRichAttr GetBasicStyle();

        /// <inheritdoc cref="RichTextBox.WriteImage(Image,BitmapType,ITextBoxRichAttr)"/>
        bool WriteImage(
            Image? bitmap,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        /// <inheritdoc cref="RichTextBox.WriteImage(string,BitmapType,ITextBoxRichAttr)"/>
        bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        /// <inheritdoc cref="RichTextBox.Delete"/>
        bool Delete(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox.BeginStyle"/>
        bool BeginStyle(ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox.SetStyle"/>
        bool SetStyle(long start, long end, ITextBoxTextAttr style);

        /// <inheritdoc cref="RichTextBox.SetRichStyle"/>
        bool SetRichStyle(long start, long end, ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox.SetSelectionRange"/>
        void SetSelectionRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox.PositionToXY"/>
        PointI PositionToXY(long pos);

        /// <inheritdoc cref="RichTextBox.GetStyleForRange"/>
        ITextBoxTextAttr GetStyleForRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox.GetRichStyleForRange"/>
        ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox.CreateTextAttr"/>
        ITextBoxTextAttr CreateTextAttr();

        /// <inheritdoc cref="RichTextBox.DeleteSelectedContent"/>
        long DeleteSelectedContent();
    }
}