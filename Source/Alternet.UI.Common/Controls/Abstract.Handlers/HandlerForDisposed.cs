using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// This is dummy handler used for the disposed controls in some cases.
    /// </summary>
    public class HandlerForDisposed : PlessControlHandler, ITextBoxHandler, IWindowHandler
    {
        /// <summary>
        /// Gets dummy control handler.
        /// </summary>
        public static readonly PlessControlHandler Default = new HandlerForDisposed();

#pragma warning disable
        public event EventHandler? CurrentPositionChanged;
#pragma warning enable

        public bool HasBorder { get; set; }

        public bool IsDefault { get; set; }

        public bool ExactFit { get; set; }

        public bool IsCancel { get; set; }

        public bool TextVisible { get; set; }

        public ElementContentAlign TextAlign { get; set; }

        public bool SundayFirst { get; set; }

        public bool MondayFirst { get; set; }

        public bool ShowHolidays { get; set; }

        public bool NoYearChange { get; set; }

        public bool NoMonthChange { get; set; }

        public bool SequentialMonthSelect { get; set; }

        public bool ShowSurroundWeeks { get; set; }

        public bool ShowWeekNumbers { get; set; }

        public bool UseGeneric { get; set; }

        public DateTime Value { get; set; }

        public DateTime MinValue { get; set; }

        public DateTime MaxValue { get; set; }

        public CheckState CheckState { get; set; }

        public bool AllowAllStatesForUser { get; set; }

        public bool AlignRight { get; set; }

        public bool ThreeState { get; set; }

        public VirtualListBox? PopupControl { get; set; }

        public PointI TextMargins { get; }

#pragma warning disable
        public string? EmptyTextHint { get; set; } = string.Empty;
#pragma warning enabled

        public int TextSelectionStart { get; }

        public int TextSelectionLength { get; }

        public DateTimePickerPopupKind PopupKind { get; set; }

        public DateTimePickerKind Kind { get; set; }

        public string Url { get; set; } = string.Empty;

        public Color HoverColor { get; set; } = Color.Empty;

        public Color NormalColor { get; set; } = Color.Empty;

        public Color VisitedColor { get; set; } = Color.Empty;

        public bool Visited { get; set; }

        public long[] SelectedIndices => [];

        public bool ColumnHeaderVisible { get; set; }

        public long? FocusedItemIndex { get; set; }

        public bool AllowLabelEdit { get; set; }

        public ListViewGridLinesDisplayMode GridLinesDisplayMode { get; set; }

        public bool IsChecked { get; set; }

        public string? Name { get; set; }

        public PointI? CurrentPosition { get; set; }

        public long LastLineNumber { get; }

        public long InsertionPointLineNumber { get; }

        public string ReportedUrl { get; }

        public bool HideSelection { get; set; }

        public bool ProcessTab { get; set; }

        public bool ProcessEnter { get; set; }

        public bool IsPassword { get; set; }

        public bool AutoUrl { get; set; }

        public bool HideVertScrollbar { get; set; }

        public bool IsEmpty { get; }

        public bool Multiline { get; set; }

        public bool ReadOnly { get; set; }

        public TextBoxTextWrap TextWrap { get; set; }

        public bool IsRichEdit { get; set; }

        public bool HideRoot { get; set; }

        public bool VariableRowHeight { get; set; }

        public bool TwistButtons { get; set; }

        public uint StateImageSpacing { get; set; }

        public uint Indentation { get; set; }

        public bool RowLines { get; set; }

        public bool ShowLines { get; set; }

        public bool ShowRootLines { get; set; }

        public bool ShowExpandButtons { get; set; }

        public bool FullRowSelect { get; set; }

        public int ItemsCount { get; set; }

        public bool HScrollBarVisible { get; set; }

        public bool VScrollBarVisible { get; set; }

        public ListBoxSelectionMode SelectionMode { get; set; }

        public bool IsEdgeBackend { get; }

        public WebBrowserPreferredColorScheme PreferredColorScheme { get; set; }

        public bool Editable { get; set; }

        public bool CanGoBack { get; }

        public bool CanGoForward { get; }

        public bool IsBusy { get; }

        public float ZoomFactor { get; set; }

        public WebBrowserZoomType ZoomType { get; set; }

        public WebBrowserZoom Zoom { get; set; }

        public string SelectedSource { get; }

        public string SelectedText { get; }

        public string PageSource { get; }

        public string PageText { get; }

        public bool AccessToDevToolsEnabled { get; set; }

        public string UserAgent { get; set; }

        public bool ContextMenuEnabled { get; set; }

        public WebBrowserBackend Backend { get; }

        public bool ShowInTaskbar { get; set; }

        public bool MaximizeEnabled { get; set; }

        public bool MinimizeEnabled { get; set; }

        public bool CloseEnabled { get; set; }

        public bool AlwaysOnTop { get; set; }

        public bool IsToolWindow { get; set; }

        public bool Resizable { get; set; }

        public bool HasTitleBar { get; set; }

        public bool HasSystemMenu { get; set; }

        public string Title { get; set; }

        public bool IsModal { get; }

        public bool IsPopupWindow { get; set; }

        public bool IsActive { get; }

        public WindowState State { get; set; }

        public ModalResult ModalResult { get; set; }

        public DisposableObject? StatusBar { get; set; }

        public int ThumbPosition { get; set; }

        public int Range { get; }

        public int PageSize { get; }

        public bool IsVertical { get; set; }

        public ScrollEventType EventTypeID { get; }

        public int EventOldPos { get; }

        public int EventNewPos { get; }

        bool ITextBoxHandler.HasSelection { get; }

        bool ITextBoxHandler.IsModified { get; set; }

        bool ITextBoxHandler.CanCopy { get; }

        bool ITextBoxHandler.CanCut { get; }

        bool ITextBoxHandler.CanPaste { get; }

        bool ITextBoxHandler.CanRedo { get; }

        bool ITextBoxHandler.CanUndo { get; }

        TextHorizontalAlignment ITextBoxHandler.TextAlign { get; set; }

        Window? IWindowHandler.Control { get; }

        public bool AllowMouseWheel { get; set; }

        public void Activate()
        {
        }

        public bool AddScriptMessageHandler(string name)
        {
            return default;
        }

        public bool AddUserScript(string javaScript, bool injectDocStart)
        {
            return default;
        }

        public bool AllowMonthChange()
        {
            return default;
        }

        public void AppendText(string text)
        {
        }

        public bool ApplyAlignmentToSelection(TextBoxTextAttrAlignment alignment)
        {
            return default;
        }

        public bool ApplyBoldToSelection()
        {
            return default;
        }

        public bool ApplyItalicToSelection()
        {
            return default;
        }

        public bool ApplyTextEffectToSelection(TextBoxTextAttrEffects flags)
        {
            return default;
        }

        public bool ApplyUnderlineToSelection()
        {
            return default;
        }

        public bool BatchingUndo()
        {
            return default;
        }

        public bool BeginAlignment(TextBoxTextAttrAlignment alignment)
        {
            return default;
        }

        public bool BeginBatchUndo(string cmdName)
        {
            return default;
        }

        public bool BeginBold()
        {
            return default;
        }

        public bool BeginCharacterStyle(string characterStyle)
        {
            return default;
        }

        public bool BeginFont(Font? font)
        {
            return default;
        }

        public bool BeginFontSize(int pointSize)
        {
            return default;
        }

        public bool BeginFontSize(Coord pointSize)
        {
            return default;
        }

        public bool BeginItalic()
        {
            return default;
        }

        public void BeginLabelEdit(long itemIndex)
        {
            return;
        }

        public void BeginLabelEdit(TreeViewItem item)
        {
            return;
        }

        public bool BeginLeftIndent(int leftIndent, int leftSubIndent = 0)
        {
            return default;
        }

        public bool BeginLineSpacing(int lineSpacing)
        {
            return default;
        }

        public bool BeginListStyle(string listStyle, int level = 1, int number = 1)
        {
            return default;
        }

        public bool BeginNumberedBullet(int bulletNumber, int leftIndent, int leftSubIndent, TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Arabic | TextBoxTextAttrBulletStyle.Period)
        {
            return default;
        }

        public bool BeginParagraphSpacing(int before, int after)
        {
            return default;
        }

        public bool BeginParagraphStyle(string paragraphStyle)
        {
            return default;
        }

        public bool BeginRightIndent(int rightIndent)
        {
            return default;
        }

        public bool BeginStandardBullet(string bulletName, int leftIndent, int leftSubIndent, TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Standard)
        {
            return default;
        }

        public bool BeginSuppressUndo()
        {
            return default;
        }

        public bool BeginSymbolBullet(string symbol, int leftIndent, int leftSubIndent, TextBoxTextAttrBulletStyle bulletStyle = TextBoxTextAttrBulletStyle.Symbol)
        {
            return default;
        }

        public bool BeginTextColor(Color color)
        {
            return default;
        }

        public bool BeginUnderline()
        {
            return default;
        }

        public bool BeginURL(string url, string? characterStyle = null)
        {
            return default;
        }

        public bool CanCopy()
        {
            return default;
        }

        public bool CanCut()
        {
            return default;
        }

        public bool CanDeleteSelection()
        {
            return default;
        }

        public bool CanPaste()
        {
            return default;
        }

        public bool CanRedo()
        {
            return default;
        }

        public bool CanSetZoomType(WebBrowserZoomType zoomType)
        {
            return default;
        }

        public bool CanUndo()
        {
            return default;
        }

        public void Clear()
        {
            return;
        }

        public void ClearHistory()
        {
            return;
        }

        public void ClearItems()
        {
            return;
        }

        public bool ClearListStyle(long startRange, long endRange, RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            return default;
        }

        public void ClearSelected()
        {
        }

        public void ClearSelection()
        {
        }

        public void ClearTicks()
        {
        }

        public void Close()
        {
        }

        public void CollapseAll()
        {
        }

        public void CollapseAllChildren(TreeViewItem item)
        {
        }

        public void Copy()
        {
        }

        public void Cut()
        {
        }

        public bool Delete(long startRange, long endRange)
        {
            return default;
        }

        public long DeleteSelectedContent()
        {
            return default;
        }

        public void DeleteSelection()
        {
        }

        public void DiscardEdits()
        {
        }

        public void DismissPopup()
        {
        }

        public string? DoCommand(string cmdName, params object?[] args)
        {
            return default;
        }

        public bool DoesSelectionHaveTextEffectFlag(TextBoxTextAttrEffects flag)
        {
            return default;
        }

        public bool DoSetCurrent(int current)
        {
            return default;
        }

        public void DoWriteText(string value, TextBoxSetValueFlags flags = TextBoxSetValueFlags.NoEvent)
        {
        }

        public void EmptyUndoBuffer()
        {
        }

        public void EnableDelayedImageLoading(bool b)
        {
        }

        public void EnableHistory(bool enable = true)
        {
        }

        public void EnableHolidayDisplay(bool display)
        {
        }

        public void EnableImages(bool b)
        {
        }

        public bool SetNoMonthChange(bool enable)
        {
            return default;
        }

        public void EnableVerticalScrollbar(bool enable)
        {
        }

        public void EnableVirtualAttributes(bool b)
        {
        }

        public bool EndAlignment()
        {
            return default;
        }

        public bool EndAllStyles()
        {
            return default;
        }

        public bool EndBatchUndo()
        {
            return default;
        }

        public bool EndBold()
        {
            return default;
        }

        public bool EndCharacterStyle()
        {
            return default;
        }

        public bool EndFont()
        {
            return default;
        }

        public bool EndFontSize()
        {
            return default;
        }

        public bool EndItalic()
        {
            return default;
        }

        public void EndLabelEdit(TreeViewItem item, bool cancel)
        {
        }

        public bool EndLeftIndent()
        {
            return default;
        }

        public bool EndLineSpacing()
        {
            return default;
        }

        public bool EndListStyle()
        {
            return default;
        }

        public bool EndNumberedBullet()
        {
            return default;
        }

        public bool EndParagraphSpacing()
        {
            return default;
        }

        public bool EndParagraphStyle()
        {
            return default;
        }

        public bool EndRightIndent()
        {
            return default;
        }

        public bool EndStandardBullet()
        {
            return default;
        }

        public bool EndStyle()
        {
            return default;
        }

        public bool EndSuppressUndo()
        {
            return default;
        }

        public bool EndSymbolBullet()
        {
            return default;
        }

        public bool EndTextColor()
        {
            return default;
        }

        public bool EndUnderline()
        {
            return default;
        }

        public bool EndURL()
        {
            return default;
        }

        public void EnsureItemVisible(long itemIndex)
        {
        }

        public void EnsureVisible(int itemIndex)
        {
        }

        public void EnsureVisible(TreeViewItem item)
        {
        }

        public void ExpandAll()
        {
        }

        public void ExpandAllChildren(TreeViewItem item)
        {
        }

        public bool ExtendSelection(long oldPosition, long newPosition, RichTextMoveCaretFlags flags)
        {
            return default;
        }

        public void FindClearResult()
        {
        }

        public long FindNextWordPosition(int direction = 1)
        {
            return default;
        }

        public void ForceDelayedLayout()
        {
        }

        public long GetAdjustedCaretPosition(long caretPos)
        {
            return default;
        }

        public bool GetCaretAtLineStart()
        {
            return default;
        }

        public long GetCaretPosition()
        {
            return default;
        }

        public long GetCaretPositionForDefaultStyle()
        {
            return default;
        }

        public string GetCurrentTitle()
        {
            return string.Empty;
        }

        public string GetCurrentURL()
        {
            return string.Empty;
        }

        public int GetDelay(uint i)
        {
            return default;
        }

        public bool GetDelayedImageLoading()
        {
            return default;
        }

        public bool GetDelayedImageProcessingRequired()
        {
            return default;
        }

        public long GetDelayedImageProcessingTime()
        {
            return default;
        }

        public long GetDelayedLayoutThreshold()
        {
            return default;
        }

        public bool GetDragging()
        {
            return default;
        }

        public int GetFileHandlerFlags()
        {
            return default;
        }

        public string GetFileName()
        {
            return default;
        }

        public int GetFirstSelected()
        {
            return default;
        }

        public PointI GetFirstVisiblePoint()
        {
            return default;
        }

        public long GetFirstVisiblePosition()
        {
            return default;
        }

        public Coord GetFontScale()
        {
            return default;
        }

        public bool GetFullLayoutRequired()
        {
            return default;
        }

        public long GetFullLayoutSavedPosition()
        {
            return default;
        }

        public long GetFullLayoutTime()
        {
            return default;
        }

        public Color GetHeaderColorBg()
        {
            return Color.Empty;
        }

        public Color GetHeaderColorFg()
        {
            return Color.Empty;
        }

        public Color GetHighlightColorBg()
        {
            return Color.Empty;
        }

        public Color GetHighlightColorFg()
        {
            return Color.Empty;
        }

        public Color GetHolidayColorBg()
        {
            return Color.Empty;
        }

        public Color GetHolidayColorFg()
        {
            return Color.Empty;
        }

        public bool GetImagesEnabled()
        {
            return default;
        }

        public long GetInsertionPoint()
        {
            return default;
        }

        public RectD GetItemBounds(long itemIndex, ListViewItemBoundsPortion portion)
        {
            return default;
        }

        public RectD? GetItemRect(int index)
        {
            return default;
        }

        public long GetLastPosition()
        {
            return default;
        }

        public int GetLineHeight()
        {
            return default;
        }

        public int GetLineLength(long lineNo)
        {
            return default;
        }

        public string GetLineText(long lineNo)
        {
            return string.Empty;
        }

        public PointI GetLogicalPoint(PointI ptPhysical)
        {
            return default;
        }

        public IntPtr GetNativeBackend()
        {
            return default;
        }

        public int GetNextSelected()
        {
            return default;
        }

        public int GetNumberOfLines()
        {
            return default;
        }

        public int GetOtherBorderForSizer()
        {
            return default;
        }

        public PointI GetPhysicalPoint(PointI ptLogical)
        {
            return default;
        }

        public string GetRange(long from, long to)
        {
            return string.Empty;
        }

        public int GetSelectedCount()
        {
            return default;
        }

        public int GetSelection()
        {
            return default;
        }

        public long GetSelectionAnchor()
        {
            return default;
        }

        public long GetSelectionEnd()
        {
            return default;
        }

        public long GetSelectionStart()
        {
            return default;
        }

        public string GetStringSelection()
        {
            return string.Empty;
        }

        public bool IsValidPosition(long pos)
        {
            return default;
        }

        public void Paste()
        {
        }

        public PointD PositionToCoord(long pos)
        {
            return default;
        }

        public PointI PositionToXY(long pos)
        {
            return default;
        }

        public void Redo()
        {
        }

        public void Remove(long from, long to)
        {
        }

        public bool RemoveScriptMessageHandler(string name)
        {
            return default;
        }

        public void Replace(long from, long to, string value)
        {
        }

        public void SelectAll()
        {
        }

        public void SelectNone()
        {
        }

        public void SetIcon(IconSet? value)
        {
        }

        public void SetInsertionPoint(long pos)
        {
        }

        public void SetInsertionPointEnd()
        {
        }

        public void SetMaxLength(ulong len)
        {
        }

        public void SetSelection(long from, long to)
        {

        }

        public void ShowPosition(long pos)
        {
        }

        public void Undo()
        {
        }

        public void WriteText(string text)
        {
        }

        public long XYToPosition(long x, long y)
        {
            return default;
        }

        public void SetMinSize(SizeD size)
        {
        }

        public void SetMaxSize(SizeD size)
        {
        }
    }
}
