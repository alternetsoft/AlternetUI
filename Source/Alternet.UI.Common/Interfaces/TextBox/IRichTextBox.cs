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

        /// <inheritdoc cref="RichTextBox.BeginFontSize(double)"/>
        bool BeginFontSize(double pointSize);

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
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period);

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

        /// <inheritdoc cref="RichTextBox."/>
        bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag);

        /// <inheritdoc cref="RichTextBox."/>
        bool IsSelectionAligned(TextBoxTextAttrAlignment alignment);

        /// <inheritdoc cref="RichTextBox."/>
        bool ApplyBoldToSelection();

        /// <inheritdoc cref="RichTextBox."/>
        bool ApplyItalicToSelection();

        /// <inheritdoc cref="RichTextBox."/>
        bool ApplyUnderlineToSelection();

        /// <inheritdoc cref="RichTextBox."/>
        bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags);

        /// <inheritdoc cref="RichTextBox."/>
        bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment);

        /// <inheritdoc cref="RichTextBox."/>
        bool SetDefaultStyleToCursorStyle();

        /// <inheritdoc cref="RichTextBox."/>
        void SelectNone();

        /// <inheritdoc cref="RichTextBox."/>
        bool SelectWord(long position);

        /// <inheritdoc cref="RichTextBox."/>
        bool LayoutContent(bool onlyVisibleRect = false);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveRight(int noPositions = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveLeft(int noPositions = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveUp(int noLines = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveDown(int noLines = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveToLineEnd(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveToLineStart(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveToParagraphEnd(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveToParagraphStart(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveHome(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool MoveEnd(int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool PageUp(int noPages = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool PageDown(int noPages = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool WordLeft(int noPages = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool WordRight(int noPages = 1, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool BeginBatchUndo(string cmdName);

        /// <inheritdoc cref="RichTextBox."/>
        bool EndBatchUndo();

        /// <inheritdoc cref="RichTextBox."/>
        bool BatchingUndo();

        /// <inheritdoc cref="RichTextBox."/>
        bool BeginSuppressUndo();

        /// <inheritdoc cref="RichTextBox."/>
        bool EndSuppressUndo();

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxRichAttr GetDefaultStyleEx();

        /// <inheritdoc cref="RichTextBox."/>
        bool SuppressingUndo();

        /// <inheritdoc cref="RichTextBox."/>
        void EnableVerticalScrollbar(bool enable);

        /// <inheritdoc cref="RichTextBox."/>
        bool GetVerticalScrollbarEnabled();

        /// <inheritdoc cref="RichTextBox."/>
        void SetFontScale(double fontScale, bool refresh = false);

        /// <inheritdoc cref="RichTextBox."/>
        double GetFontScale();

        /// <inheritdoc cref="RichTextBox."/>
        bool GetVirtualAttributesEnabled();

        /// <inheritdoc cref="RichTextBox."/>
        void EnableVirtualAttributes(bool b);

        /// <inheritdoc cref="RichTextBox."/>
        void DoWriteText(string value, int flags = 0);

        /// <inheritdoc cref="RichTextBox."/>
        bool ExtendSelection(long oldPosition, long newPosition, int flags);

        /// <inheritdoc cref="RichTextBox."/>
        void SetCaretPosition(long position, bool showAtLineStart = false);

        /// <inheritdoc cref="RichTextBox."/>
        long GetCaretPosition();

        /// <inheritdoc cref="RichTextBox."/>
        long GetAdjustedCaretPosition(long caretPos);

        /// <inheritdoc cref="RichTextBox."/>
        void MoveCaretForward(long oldPosition);

        /// <inheritdoc cref="RichTextBox."/>
        PointI GetPhysicalPoint(PointI ptLogical);

        /// <inheritdoc cref="RichTextBox."/>
        PointI GetLogicalPoint(PointI ptPhysical);

        /// <inheritdoc cref="RichTextBox."/>
        long FindNextWordPosition(int direction = 1);

        /// <inheritdoc cref="RichTextBox."/>
        bool IsPositionVisible(long pos);

        /// <inheritdoc cref="RichTextBox."/>
        long GetFirstVisiblePosition();

        /// <inheritdoc cref="RichTextBox."/>
        long GetCaretPositionForDefaultStyle();

        /// <inheritdoc cref="RichTextBox."/>
        void SetCaretPositionForDefaultStyle(long pos);

        /// <inheritdoc cref="RichTextBox."/>
        void MoveCaretBack(long oldPosition);

        /// <inheritdoc cref="RichTextBox."/>
        bool BeginFont(Font? font);

        /// <inheritdoc cref="RichTextBox."/>
        bool IsDefaultStyleShowing();

        /// <inheritdoc cref="RichTextBox."/>
        PointI GetFirstVisiblePoint();

        /// <inheritdoc cref="RichTextBox."/>
        void EnableImages(bool b);

        /// <inheritdoc cref="RichTextBox."/>
        bool GetImagesEnabled();

        /// <inheritdoc cref="RichTextBox."/>
        void EnableDelayedImageLoading(bool b);

        /// <inheritdoc cref="RichTextBox."/>
        bool GetDelayedImageLoading();

        /// <inheritdoc cref="RichTextBox."/>
        bool GetDelayedImageProcessingRequired();

        /// <inheritdoc cref="RichTextBox."/>
        void SetDelayedImageProcessingRequired(bool b);

        /// <inheritdoc cref="RichTextBox."/>
        long GetDelayedImageProcessingTime();

        /// <inheritdoc cref="RichTextBox."/>
        void SetDelayedImageProcessingTime(long t);

        /// <inheritdoc cref="RichTextBox."/>
        void SetLineHeight(int height);

        /// <inheritdoc cref="RichTextBox."/>
        int GetLineHeight();

        /// <inheritdoc cref="RichTextBox."/>
        bool ProcessDelayedImageLoading(bool refresh);

        /// <inheritdoc cref="RichTextBox."/>
        void RequestDelayedImageProcessing();

        /// <inheritdoc cref="RichTextBox."/>
        long GetLastPosition();

        /// <inheritdoc cref="RichTextBox."/>
        bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox."/>
        bool ClearListStyle(
            long startRange,
            long endRange,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        /// <inheritdoc cref="RichTextBox."/>
        object WriteTable(
            int rows,
            int cols,
            ITextBoxRichAttr? tableAttr = default,
            ITextBoxRichAttr? cellAttr = default);

        /// <inheritdoc cref="RichTextBox."/>
        bool NumberList(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxRichAttr CreateUrlAttr();

        /// <inheritdoc cref="RichTextBox."/>
        bool SetStyleEx(
            long startRange,
            long endRange,
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxTextAttr GetStyle(long position);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxRichAttr GetRichStyle(long position);

        /// <inheritdoc cref="RichTextBox."/>
        bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1);

        /// <inheritdoc cref="RichTextBox."/>
        void SetTextCursor(Cursor? cursor);

        /// <inheritdoc cref="RichTextBox."/>
        Cursor GetTextCursor();

        /// <inheritdoc cref="RichTextBox."/>
        void SetURLCursor(Cursor? cursor);

        /// <inheritdoc cref="RichTextBox."/>
        Cursor GetURLCursor();

        /// <inheritdoc cref="RichTextBox."/>
        void SetAndShowDefaultStyle(ITextBoxRichAttr attr);

        /// <inheritdoc cref="RichTextBox."/>
        void SetBasicStyle(ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox."/>
        bool HasParagraphAttributes(
            long startRange,
            long endRange,
            ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox."/>
        bool BeginTextColor(Color color);

        /// <inheritdoc cref="RichTextBox."/>
        bool EndTextColor();

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxRichAttr GetBasicStyle();

        /// <inheritdoc cref="RichTextBox."/>
        bool WriteImage(
            Image? bitmap,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        /// <inheritdoc cref="RichTextBox."/>
        bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        /// <inheritdoc cref="RichTextBox."/>
        bool Delete(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox."/>
        bool BeginStyle(ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox."/>
        bool SetStyle(long start, long end, ITextBoxTextAttr style);

        /// <inheritdoc cref="RichTextBox."/>
        bool SetRichStyle(long start, long end, ITextBoxRichAttr style);

        /// <inheritdoc cref="RichTextBox."/>
        void SetSelectionRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox."/>
        PointI PositionToXY(long pos);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxTextAttr GetStyleForRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange);

        /// <inheritdoc cref="RichTextBox."/>
        ITextBoxTextAttr CreateTextAttr();

        /// <inheritdoc cref="RichTextBox."/>
        long DeleteSelectedContent();
    }
}