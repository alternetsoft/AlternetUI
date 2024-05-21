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
        bool HasBorder { get; set; }

        void Remove(long from, long to);

        void SetDragging(bool dragging);

        long GetSelectionAnchor();

        void SetSelectionAnchor(long anchor);

        void Clear();

        void Replace(long from, long to, string value);

        string GetValue();

        void SetValue(string value);

        ITextBoxRichAttr CreateRichAttr();

        string GetRange(long from, long to);

        int GetLineLength(long lineNo);

        string GetLineText(long lineNo);

        int GetNumberOfLines();

        bool IsModified();

        bool IsEditable();

        bool IsSingleLine();

        bool IsMultiLine();

        string GetStringSelection();

        void SetDelayedLayoutThreshold(long threshold);

        long GetDelayedLayoutThreshold();

        bool GetFullLayoutRequired();

        void SetFullLayoutRequired(bool b);

        long GetFullLayoutTime();

        void SetFullLayoutTime(long t);

        long GetFullLayoutSavedPosition();

        bool SetDefaultStyle(ITextBoxTextAttr style);

        bool SetDefaultRichStyle(ITextBoxRichAttr style);

        void SetFullLayoutSavedPosition(long p);

        void ForceDelayedLayout();

        bool GetCaretAtLineStart();

        void SetCaretAtLineStart(bool atStart);

        bool GetDragging();

        bool LoadFromFile(string file, RichTextFileType type = RichTextFileType.Any);

        bool SaveToFile(string file, RichTextFileType type = RichTextFileType.Any);

        bool SaveToStream(Stream stream, RichTextFileType type);

        bool LoadFromStream(Stream stream, RichTextFileType type);

        void SetFileHandlerFlags(RichTextHandlerFlags knownFlags, int customFlags = 0);

        int GetFileHandlerFlags();

        void MarkDirty();

        void DiscardEdits();

        void SetMaxLength(ulong len);

        void WriteText(string text);

        void AppendText(string text);

        void Copy();

        void Cut();

        void Paste();

        void DeleteSelection();

        bool CanCopy();

        bool CanCut();

        bool CanPaste();

        bool CanDeleteSelection();

        void Undo();

        void Redo();

        bool CanUndo();

        bool CanRedo();

        void SetInsertionPointEnd();

        long GetInsertionPoint();

        void SetSelection(long from, long to);

        void SetEditable(bool editable);

        bool HasSelection();

        bool HasUnfocusedSelection();

        bool NewLine();

        bool LineBreak();

        bool EndStyle();

        bool EndAllStyles();

        bool BeginBold();

        bool EndBold();

        bool BeginItalic();

        bool EndItalic();

        bool BeginUnderline();

        bool EndUnderline();

        bool BeginFontSize(int pointSize);

        bool BeginFontSize(double pointSize);

        bool EndFontSize();

        bool ApplyStyleToSelection(ITextBoxRichAttr style, RichTextSetStyleFlags flags);

        bool EndFont();

        string GetFileName();

        void SetFileName(string filename);

        bool BeginAlignment(TextBoxTextAttrAlignment alignment);

        bool EndAlignment();

        bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0);

        bool EndLeftIndent();

        bool BeginRightIndent(int rightIndent);

        bool EndRightIndent();

        bool BeginParagraphSpacing(int before, int after);

        bool EndParagraphSpacing();

        bool BeginLineSpacing(int lineSpacing);

        bool EndLineSpacing();

        bool BeginNumberedBullet(
            int bulletNumber,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period);

        bool EndNumberedBullet();

        bool BeginSymbolBullet(
            string symbol,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Symbol);

        bool EndSymbolBullet();

        bool BeginStandardBullet(
            string bulletName,
            int leftIndent,
            int leftSubIndent,
            TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Standard);

        bool EndStandardBullet();

        bool BeginCharacterStyle(string characterStyle);

        bool EndCharacterStyle();

        bool BeginParagraphStyle(string paragraphStyle);

        bool EndParagraphStyle();

        bool BeginListStyle(string listStyle, int level = 1, int number = 1);

        bool EndListStyle();

        bool BeginURL(string url, string? characterStyle = default);

        bool EndURL();

        bool IsSelectionBold();

        bool IsSelectionItalics();

        bool IsSelectionUnderlined();

        bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag);

        bool IsSelectionAligned(TextBoxTextAttrAlignment alignment);

        bool ApplyBoldToSelection();

        bool ApplyItalicToSelection();

        bool ApplyUnderlineToSelection();

        bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags);

        bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment);

        bool SetDefaultStyleToCursorStyle();

        void SelectNone();

        bool SelectWord(long position);

        bool LayoutContent(bool onlyVisibleRect = false);

        bool MoveRight(int noPositions = 1, int flags = 0);

        bool MoveLeft(int noPositions = 1, int flags = 0);

        bool MoveUp(int noLines = 1, int flags = 0);

        bool MoveDown(int noLines = 1, int flags = 0);

        bool MoveToLineEnd(int flags = 0);

        bool MoveToLineStart(int flags = 0);

        bool MoveToParagraphEnd(int flags = 0);

        bool MoveToParagraphStart(int flags = 0);

        bool MoveHome(int flags = 0);

        bool MoveEnd(int flags = 0);

        bool PageUp(int noPages = 1, int flags = 0);

        bool PageDown(int noPages = 1, int flags = 0);

        bool WordLeft(int noPages = 1, int flags = 0);

        bool WordRight(int noPages = 1, int flags = 0);

        bool BeginBatchUndo(string cmdName);

        bool EndBatchUndo();

        bool BatchingUndo();

        bool BeginSuppressUndo();

        bool EndSuppressUndo();

        ITextBoxRichAttr GetDefaultStyleEx();

        bool SuppressingUndo();

        void EnableVerticalScrollbar(bool enable);

        bool GetVerticalScrollbarEnabled();

        void SetFontScale(double fontScale, bool refresh = false);

        double GetFontScale();

        bool GetVirtualAttributesEnabled();

        void EnableVirtualAttributes(bool b);

        void DoWriteText(string value, int flags = 0);

        bool ExtendSelection(long oldPosition, long newPosition, int flags);

        void SetCaretPosition(long position, bool showAtLineStart = false);

        long GetCaretPosition();

        long GetAdjustedCaretPosition(long caretPos);

        void MoveCaretForward(long oldPosition);

        PointI GetPhysicalPoint(PointI ptLogical);

        PointI GetLogicalPoint(PointI ptPhysical);

        long FindNextWordPosition(int direction = 1);

        bool IsPositionVisible(long pos);

        long GetFirstVisiblePosition();

        long GetCaretPositionForDefaultStyle();

        void SetCaretPositionForDefaultStyle(long pos);

        void MoveCaretBack(long oldPosition);

        bool BeginFont(Font? font);

        bool IsDefaultStyleShowing();

        PointI GetFirstVisiblePoint();

        void EnableImages(bool b);

        bool GetImagesEnabled();

        void EnableDelayedImageLoading(bool b);

        bool GetDelayedImageLoading();

        bool GetDelayedImageProcessingRequired();

        void SetDelayedImageProcessingRequired(bool b);

        long GetDelayedImageProcessingTime();

        void SetDelayedImageProcessingTime(long t);

        void SetLineHeight(int height);

        int GetLineHeight();

        bool ProcessDelayedImageLoading(bool refresh);

        void RequestDelayedImageProcessing();

        long GetLastPosition();

        bool SetListStyle(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        bool ClearListStyle(
            long startRange,
            long endRange,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        object WriteTable(
            int rows,
            int cols,
            ITextBoxRichAttr? tableAttr = default,
            ITextBoxRichAttr? cellAttr = default);

        bool NumberList(
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int startFrom = 1,
            int specifiedLevel = -1);

        ITextBoxRichAttr CreateUrlAttr();

        bool SetStyleEx(
            long startRange,
            long endRange,
            ITextBoxRichAttr style,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo);

        ITextBoxTextAttr GetStyle(long position);

        ITextBoxRichAttr GetRichStyle(long position);

        bool PromoteList(
            int promoteBy,
            long startRange,
            long endRange,
            string defName,
            RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo,
            int specifiedLevel = -1);

        void SetTextCursor(Cursor? cursor);

        Cursor GetTextCursor();

        void SetURLCursor(Cursor? cursor);

        Cursor GetURLCursor();

        void SetAndShowDefaultStyle(ITextBoxRichAttr attr);

        void SetBasicStyle(ITextBoxRichAttr style);

        bool HasParagraphAttributes(
            long startRange,
            long endRange,
            ITextBoxRichAttr style);

        bool BeginTextColor(Color color);

        bool EndTextColor();

        ITextBoxRichAttr GetBasicStyle();

        bool WriteImage(
            Image? bitmap,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        bool WriteImage(
            string filename,
            BitmapType bitmapType = BitmapType.Png,
            ITextBoxRichAttr? textAttr = null);

        bool Delete(long startRange, long endRange);

        bool BeginStyle(ITextBoxRichAttr style);

        bool SetStyle(long start, long end, ITextBoxTextAttr style);

        bool SetRichStyle(long start, long end, ITextBoxRichAttr style);

        void SetSelectionRange(long startRange, long endRange);

        PointI PositionToXY(long pos);

        ITextBoxTextAttr GetStyleForRange(long startRange, long endRange);

        ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange);

        ITextBoxTextAttr CreateTextAttr();

        long DeleteSelectedContent();
    }
}