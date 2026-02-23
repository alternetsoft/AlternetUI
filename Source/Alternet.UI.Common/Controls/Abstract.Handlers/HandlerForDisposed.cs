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
    public class HandlerForDisposed : PlessControlHandler,
        IAnimationPlayerHandler,
        IButtonHandler,
        ICalendarHandler,
        ICheckBoxHandler,
        IComboBoxHandler,
        IListViewHandler,
        IRadioButtonHandler,
        IRichTextBox,
        IScrollBarHandler,
        ITextBoxHandler,
        IWebBrowserLite,
        IWindowHandler
    {
        /// <summary>
        /// Gets dummy control handler.
        /// </summary>
        public static readonly PlessControlHandler Default = new HandlerForDisposed();

#pragma warning disable
        public event EventHandler? CurrentPositionChanged;
#pragma warning enable

        public uint FrameCount { get; }

        public SizeI AnimationSize { get; }

        public bool IsOk { get; }

        public Image? NormalImage { get; set; }

        public Image? HoveredImage { get; set; }

        public Image? PressedImage { get; set; }

        public Image? DisabledImage { get; set; }

        public Image? FocusedImage { get; set; }

        public Action? Click { get; set; }

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

        public ICalendarDateAttr? MarkDateAttr { get; set; }

        public CheckState CheckState { get; set; }

        public bool AllowAllStatesForUser { get; set; }

        public bool AlignRight { get; set; }

        public bool ThreeState { get; set; }

        public VirtualListBox? PopupControl { get; set; }

        public ComboBox.OwnerDrawFlags OwnerDrawStyle { get; set; }

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

        public ListViewItem? TopItem { get; }

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

        bool IWebBrowserLite.HasSelection { get; }

        bool ITextBoxHandler.IsModified { get; set; }

        bool ITextBoxHandler.CanCopy { get; }

        bool IWebBrowserLite.CanCopy { get; }

        bool ITextBoxHandler.CanCut { get; }

        bool IWebBrowserLite.CanCut { get; }

        bool ITextBoxHandler.CanPaste { get; }

        bool IWebBrowserLite.CanPaste { get; }

        bool ITextBoxHandler.CanRedo { get; }

        bool IWebBrowserLite.CanRedo { get; }

        bool ITextBoxHandler.CanUndo { get; }

        bool IWebBrowserLite.CanUndo { get; }

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

        public bool ApplyStyleToSelection(ITextBoxRichAttr style, RichTextSetStyleFlags flags)
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

        public bool BeginStyle(ITextBoxRichAttr style)
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

        public ICalendarDateAttr CreateDateAttr(CalendarDateBorder border = CalendarDateBorder.None)
        {
            return new PlessCalendarDateAttr();
        }

        public ITextBoxRichAttr CreateRichAttr()
        {
            return new PlessTextBoxRichAttr();
        }

        public ITextBoxTextAttr CreateTextAttr()
        {
            return new PlessTextBoxRichAttr();
        }

        public ITextBoxRichAttr CreateUrlAttr()
        {
            return new PlessTextBoxRichAttr();
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

        public int Find(string text, WebBrowserFindParams? prm = null)
        {
            return -1;
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

        public ICalendarDateAttr? GetAttr(int day)
        {
            return default;
        }

        public ITextBoxRichAttr GetBasicStyle()
        {
            return PlessTextBoxRichAttr.Empty;
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

        public ITextBoxTextAttr GetDefaultStyle()
        {
            return PlessTextBoxRichAttr.Empty;
        }

        public ITextBoxRichAttr GetDefaultStyleEx()
        {
            return PlessTextBoxRichAttr.Empty;
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

        public GenericImage GetFrame(uint i)
        {
            return GenericImage.Empty;
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

        public ITextBoxRichAttr GetRichStyle(long position)
        {
            return PlessTextBoxRichAttr.Empty;
        }

        public ITextBoxRichAttr GetRichStyleForRange(long startRange, long endRange)
        {
            return PlessTextBoxRichAttr.Empty;
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

        public ITextBoxTextAttr GetStyle(long position)
        {
            return PlessTextBoxRichAttr.Empty;
        }

        public ITextBoxTextAttr GetStyleForRange(long startRange, long endRange)
        {
            return PlessTextBoxRichAttr.Empty;
        }

        public Cursor GetTextCursor()
        {
            return Cursors.Default;
        }

        public int GetTopBorderForSizer()
        {
            return default;
        }

        public Cursor GetURLCursor()
        {
            return Cursors.Default;
        }

        public string GetValue()
        {
            return string.Empty;
        }

        public bool GetVerticalScrollbarEnabled()
        {
            return default;
        }

        public bool GetVirtualAttributesEnabled()
        {
            return default;
        }

        public int GetVisibleBegin()
        {
            return default;
        }

        public int GetVisibleEnd()
        {
            return default;
        }

        public bool GoBack()
        {
            return default;
        }

        public bool GoForward()
        {
            return default;
        }

        public bool HasParagraphAttributes(long startRange, long endRange, ITextBoxRichAttr style)
        {
            return default;
        }

        public bool HasSelection()
        {
            return default;
        }

        public bool HasUnfocusedSelection()
        {
            return default;
        }

        public Calendar.HitTestResult HitTest(PointD point)
        {
            return default;
        }

        public bool HitTest(PointD point, out TreeViewItem? item, out TreeViewHitTestLocations locations, bool needItem = true)
        {
            locations = default;
            item = null;
            return default;
        }

        public bool IsCurrent(int current)
        {
            return default;
        }

        public bool IsDefaultStyleShowing()
        {
            return default;
        }

        public bool IsEditable()
        {
            return default;
        }

        public bool IsItemFocused(TreeViewItem item)
        {
            return default;
        }

        public bool IsItemSelected(TreeViewItem item)
        {
            return default;
        }

        public bool IsModified()
        {
            return default;
        }

        public bool IsMultiLine()
        {
            return default;
        }

        public bool IsPlaying()
        {
            return default;
        }

        public bool IsPositionVisible(long pos)
        {
            return default;
        }

        public bool IsSelected(int line)
        {
            return default;
        }

        public bool IsSelectionAligned(TextBoxTextAttrAlignment alignment)
        {
            return default;
        }

        public bool IsSelectionBold()
        {
            return default;
        }

        public bool IsSelectionItalics()
        {
            return default;
        }

        public bool IsSelectionUnderlined()
        {
            return default;
        }

        public bool IsSingleLine()
        {
            return default;
        }

        public bool IsValidPosition(long pos)
        {
            return default;
        }

        public bool IsVisible(int line)
        {
            return default;
        }

        public int ItemHitTest(PointD position)
        {
            return default;
        }

        public bool LayoutContent(bool onlyVisibleRect = false)
        {
            return default;
        }

        public bool LineBreak()
        {
            return default;
        }

        public bool Load(Stream stream, AnimationType type = AnimationType.Any)
        {
            return default;
        }

        public bool LoadFile(string filename, AnimationType type = AnimationType.Any)
        {
            return default;
        }

        public bool LoadFromFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return default;
        }

        public bool LoadFromStream(Stream stream, RichTextFileType type)
        {
            return default;
        }

        public void LoadURL(string url)
        {
        }

        public void MakeAsListBox()
        {
        }

        public void Mark(int day, bool mark)
        {
        }

        public void MarkDirty()
        {
        }

        public void MoveCaretBack(long oldPosition)
        {
        }

        public void MoveCaretForward(long oldPosition)
        {
        }

        public bool MoveDown(int noLines = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveEnd(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveHome(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveLeft(int noPositions = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveRight(int noPositions = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveToLineEnd(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveToLineStart(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveToParagraphEnd(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveToParagraphStart(RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool MoveUp(int noLines = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public void NavigateToString(string html, string? baseUrl = null)
        {
        }

        public bool NewLine()
        {
            return default;
        }

        public bool NumberList(long startRange, long endRange, string defName, RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo, int startFrom = 1, int specifiedLevel = -1)
        {
            return default;
        }

        public bool PageDown(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool PageUp(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public void Paste()
        {
        }

        public bool Play()
        {
            return default;
        }

        public PointD PositionToCoord(long pos)
        {
            return default;
        }

        public PointI PositionToXY(long pos)
        {
            return default;
        }

        public void Print()
        {
        }

        public bool ProcessDelayedImageLoading(bool refresh)
        {
            return default;
        }

        public bool PromoteList(int promoteBy, long startRange, long endRange, string defName, RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo, int specifiedLevel = -1)
        {
            return default;
        }

        public void Redo()
        {
        }

        public void RefreshRow(int row)
        {
        }

        public void RefreshRows(int from, int to)
        {
        }

        public void Reload()
        {
        }

        public void Reload(bool noCache)
        {
        }

        public void Remove(long from, long to)
        {
        }

        public void RemoveAllUserScripts()
        {
        }

        public bool RemoveScriptMessageHandler(string name)
        {
            return default;
        }

        public void Replace(long from, long to, string value)
        {
        }

        public void RequestDelayedImageProcessing()
        {
        }

        public void ResetAttr(int day)
        {
        }

        public void RunScriptAsync(string javaScript, IntPtr? clientData)
        {
        }

        public bool SaveToFile(string file, RichTextFileType type = RichTextFileType.Any)
        {
            return default;
        }

        public bool SaveToStream(Stream stream, RichTextFileType type)
        {
            return default;
        }

        public void ScrollIntoView(TreeViewItem item)
        {
        }

        public bool ScrollRowPages(int pages)
        {
            return default;
        }

        public bool ScrollRows(int rows)
        {
            return default;
        }

        public bool ScrollToRow(int row)
        {
            return default;
        }

        public void SelectAll()
        {
        }

        public void SelectAllText()
        {
        }

        public void SelectNone()
        {
        }

        public void SelectTextRange(int start, int length)
        {
        }

        public bool SelectWord(long position)
        {
            return default;
        }

        public void SetAndShowDefaultStyle(ITextBoxRichAttr attr)
        {
        }

        public void SetAttr(int day, ICalendarDateAttr? dateAttr)
        {
        }

        public void SetBasicStyle(ITextBoxRichAttr style)
        {
        }

        public void SetCaretAtLineStart(bool atStart)
        {
        }

        public void SetCaretPosition(long position, bool showAtLineStart = false)
        {
        }

        public void SetCaretPositionForDefaultStyle(long pos)
        {
        }

        public void SetColumnTitle(long columnIndex, string title)
        {
        }

        public void SetColumnWidth(long columnIndex, Coord width, ListViewColumnWidthMode widthMode)
        {
        }

        public bool SetDefaultRichStyle(ITextBoxRichAttr style)
        {
            return default;
        }

        public bool SetDefaultStyle(ITextBoxTextAttr style)
        {
            return default;
        }

        public bool SetDefaultStyleToCursorStyle()
        {
            return default;
        }

        public void SetDelayedImageProcessingRequired(bool b)
        {
        }

        public void SetDelayedImageProcessingTime(long t)
        {
        }

        public void SetDelayedLayoutThreshold(long threshold)
        {
        }

        public void SetDragging(bool dragging)
        {
        }

        public void SetEditable(bool editable)
        {
        }

        public void SetFileHandlerFlags(RichTextHandlerFlags knownFlags, int customFlags = 0)
        {
        }

        public void SetFileName(string filename)
        {
        }

        public void SetFocused(TreeViewItem item, bool value)
        {
        }

        public void SetFontScale(Coord fontScale, bool refresh = false)
        {
        }

        public void SetFullLayoutRequired(bool b)
        {
        }

        public void SetFullLayoutSavedPosition(long p)
        {
        }

        public void SetFullLayoutTime(long t)
        {
        }

        public void SetHeaderColors(Color colorFg, Color colorBg)
        {
        }

        public void SetHighlightColors(Color colorFg, Color colorBg)
        {
        }

        public void SetHoliday(int day)
        {
        }

        public void SetHolidayColors(Color colorFg, Color colorBg)
        {
        }

        public void SetIcon(IconSet? value)
        {
        }

        public void SetImageMargins(Coord x, Coord y)
        {
        }

        public void SetImagePosition(ElementContentAlign dir)
        {
        }

        public void SetInactiveBitmap(ImageSet? bitmap)
        {
        }

        public void SetInsertionPoint(long pos)
        {
        }

        public void SetInsertionPointEnd()
        {
        }

        public void SetItemBackgroundColor(TreeViewItem item, Color? color)
        {
        }

        public void SetItemImageIndex(long itemIndex, long columnIndex, int? imageIndex)
        {
        }

        public void SetItemImageIndex(TreeViewItem item, int? imageIndex)
        {
        }

        public void SetItemIsBold(TreeViewItem item, bool isBold)
        {
        }

        public void SetItemText(long itemIndex, long columnIndex, string text)
        {
        }

        public void SetItemText(TreeViewItem item, string text)
        {
        }

        public void SetItemTextColor(TreeViewItem item, Color? color)
        {
        }

        public void SetLineHeight(int height)
        {
        }

        public bool SetListStyle(long startRange, long endRange, string defName, RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo, int startFrom = 1, int specifiedLevel = -1)
        {
            return default;
        }

        public void SetMaxLength(ulong len)
        {
        }

        public void SetMenu(DisposableObject? value)
        {
        }

        public bool SetRange(bool useMinValue, bool useMaxValue)
        {
            return default;
        }

        public void SetRange(DateTime min, DateTime max, bool useMin, bool useMax)
        {
        }

        public bool SetRichStyle(long start, long end, ITextBoxRichAttr style)
        {
            return default;
        }

        public void SetScrollbar(int? position, int? range, int? pageSize, bool refresh = true)
        {
        }

        public void SetSelected(int index, bool value)
        {
        }

        public void SetSelection(long from, long to)
        {

        }

        public void SetSelection(int selection)
        {
        }

        public void SetSelectionAnchor(long anchor)
        {
        }

        public void SetSelectionBackground(Color color)
        {
        }

        public void SetSelectionRange(long startRange, long endRange)
        {
        }

        public bool SetStyle(long start, long end, ITextBoxTextAttr style)
        {
            return default;
        }

        public bool SetStyleEx(long startRange, long endRange, ITextBoxRichAttr style, RichTextSetStyleFlags flags = RichTextSetStyleFlags.WithUndo)
        {
            return default;
        }

        public void SetTextCursor(Cursor? cursor)
        {
        }

        public void SetURLCursor(Cursor? cursor)
        {
        }

        public void SetValue(string value)
        {
        }

        public void SetVirtualHostNameToFolderMapping(string hostName, string folderPath, WebBrowserHostResourceAccessKind accessKind)
        {
        }

        public void Show(
            AbstractControl control,
            PointD? position = null,
            Action? onClose = null)
        {
        }

        public ModalResult ShowModal(IWindow? owner)
        {
            return default;
        }

        public void ShowPopup()
        {
        }

        public void ShowPosition(long pos)
        {
        }

        public void Stop()
        {

        }

        public bool SuppressingUndo()
        {
            return default;
        }

        public void Undo()
        {
        }

        public bool WordLeft(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool WordRight(int noPages = 1, RichTextMoveCaretFlags flags = 0)
        {
            return default;
        }

        public bool WriteImage(Image? bitmap, BitmapType bitmapType = BitmapType.Png, ITextBoxRichAttr? textAttr = null)
        {
            return default;
        }

        public bool WriteImage(string filename, BitmapType bitmapType = BitmapType.Png, ITextBoxRichAttr? textAttr = null)
        {
            return default;
        }

        public object WriteTable(int rows, int cols, ITextBoxRichAttr? tableAttr = null, ITextBoxRichAttr? cellAttr = null)
        {
            return default;
        }

        public void WriteText(string text)
        {
        }

        public long XYToPosition(long x, long y)
        {
            return default;
        }

        ListViewHitTestInfo IListViewHandler.HitTest(PointD point)
        {
            return default;
        }

        public void ShowModalAsync(Window? owner, Action<ModalResult> onResult)
        {
        }

        public void SetMinSize(SizeD size)
        {
        }

        public void SetMaxSize(SizeD size)
        {
        }
    }
}
