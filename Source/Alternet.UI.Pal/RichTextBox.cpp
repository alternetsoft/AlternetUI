#include "RichTextBox.h"

namespace Alternet::UI
{
	class wxRichTextCtrl2 : public wxRichTextCtrl, public wxWidgetExtender
	{
	public:
		wxRichTextCtrl2(wxWindow* parent, wxWindowID id = -1,
			const wxString& value = wxEmptyString, const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxRE_MULTILINE, const wxValidator& validator = wxDefaultValidator,
			const wxString& name = wxASCII_STR(wxTextCtrlNameStr))
			: wxRichTextCtrl(parent, id, value, pos, size, style, validator, name)
		{}

	};

	wxWindow* RichTextBox::CreateWxWindowCore(wxWindow* parent)
	{
/*
#define wxRE_READONLY          0x0010
#define wxRE_CENTRE_CARET      0x8000
*/
		long style = wxRE_MULTILINE;

		auto result = new wxRichTextCtrl2(parent, -1,
			wxEmptyString, wxDefaultPosition,
			wxDefaultSize,
			style);

		//result->Bind(wxEVT_TEXT, &RichTextBox::OnTextChanged, this);
		return result;
	}

	wxRichTextCtrl* RichTextBox::GetTextCtrl()
	{
		return dynamic_cast<wxRichTextCtrl*>(GetWxWindow());
	}

	RichTextBox::RichTextBox()
	{

	}

	RichTextBox::~RichTextBox()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				// window->Unbind(wxEVT_TEXT, &TextBox::OnTextChanged, this);
			}
		}
	}

	bool RichTextBox::IsPositionVisible(int64_t pos)
	{
		return false;
	}

	int64_t RichTextBox::GetFirstVisiblePosition()
	{
		return 0;
	}

	int64_t RichTextBox::GetCaretPositionForDefaultStyle()
	{
		return 0;
	}

	void RichTextBox::SetCaretPositionForDefaultStyle(int64_t pos)
	{

	}

	void RichTextBox::MoveCaretBack(int64_t oldPosition)
	{

	}

	bool RichTextBox::GetCaretPositionForIndex(int64_t position, const Int32Rect& rect, void* container)
	{
		return false;
	}

	void* RichTextBox::GetVisibleLineForCaretPosition(int64_t caretPosition)
	{
		return nullptr;
	}

	void* RichTextBox::GetCommandProcessor()
	{
		return nullptr;
	}

	bool RichTextBox::IsDefaultStyleShowing()
	{
		return false;
	}

	Int32Point RichTextBox::GetFirstVisiblePoint()
	{
		return Int32Point();
	}

	void RichTextBox::EnableImages(bool b)
	{

	}

	bool RichTextBox::GetImagesEnabled()
	{
		return false;
	}

	void RichTextBox::EnableDelayedImageLoading(bool b)
	{

	}

	bool RichTextBox::GetDelayedImageLoading()
	{
		return false;
	}

	bool RichTextBox::GetDelayedImageProcessingRequired()
	{
		return false;
	}

	void RichTextBox::SetDelayedImageProcessingRequired(bool b)
	{

	}

	int64_t RichTextBox::GetDelayedImageProcessingTime()
	{
		return 0;
	}

	void RichTextBox::SetDelayedImageProcessingTime(int64_t t)
	{

	}

	string RichTextBox::GetValue()
	{
		return wxStr(wxEmptyString);
	}

	void RichTextBox::SetValue(const string& value)
	{

	}

	void RichTextBox::SetLineHeight(int height)
	{

	}

	int RichTextBox::GetLineHeight()
	{
		return 0;
	}

	bool RichTextBox::SetCaretPositionAfterClick(void* container, int64_t position,
		int hitTestFlags, bool extendSelection)
	{
		return false;
	}

	/*static*/ void RichTextBox::ClearAvailableFontNames()
	{

	}

	bool RichTextBox::ProcessDelayedImageLoading(bool refresh)
	{
		return false;
	}

	void RichTextBox::RequestDelayedImageProcessing()
	{

	}

	bool RichTextBox::GetUncombinedStyle(int64_t position, void* style)
	{
		return false;
	}

	bool RichTextBox::GetUncombinedStyle2(int64_t position, void* style, void* container)
	{
		return false;
	}

	bool RichTextBox::SetDefaultStyle(void* style)
	{
		return false;
	}

	bool RichTextBox::SetDefaultRichStyle(void* style)
	{
		return false;
	}

	void* RichTextBox::GetDefaultStyleEx()
	{
		return nullptr;
	}

	int64_t RichTextBox::GetLastPosition()
	{
		return 0;
	}

	void* RichTextBox::GetStyle(int64_t position)
	{
		return nullptr;
	}

	void* RichTextBox::GetRichStyle(int64_t position)
	{
		return nullptr;
	}

	void* RichTextBox::GetStyleInContainer(int64_t position, void* container)
	{
		return nullptr;
	}

	bool RichTextBox::SetStyle(int64_t start, int64_t end, void* style)
	{
		return false;
	}

	bool RichTextBox::SetRichStyle(int64_t start, int64_t end, void* style)
	{
		return false;
	}

	void RichTextBox::SetStyle2(void* richObj, void* textAttr, int flags)
	{

	}

	void* RichTextBox::GetStyleForRange(int64_t startRange, int64_t endRange)
	{
		return nullptr;
	}

	void* RichTextBox::GetStyleForRange2(int64_t startRange, int64_t endRange)
	{
		return nullptr;
	}

	void* RichTextBox::GetStyleForRange3(int64_t startRange, int64_t endRange, void* container)
	{
		return nullptr;
	}

	bool RichTextBox::SetStyleEx(int64_t startRange, int64_t endRange, void* style, int flags)
	{
		return false;
	}

	bool RichTextBox::SetListStyle(int64_t startRange, int64_t endRange,
		void* def, int flags, int startFrom, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::SetListStyle2(int64_t startRange, int64_t endRange,
		const string& defName, int flags, int startFrom, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::ClearListStyle(int64_t startRange, int64_t endRange, int flags)
	{
		return false;
	}

	bool RichTextBox::NumberList(int64_t startRange, int64_t endRange, void* def,
		int flags, int startFrom, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::NumberList2(int64_t startRange, int64_t endRange,
		const string& defName, int flags, int startFrom, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::PromoteList(int promoteBy, int64_t startRange,
		int64_t endRange, void* def, int flags, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::PromoteList2(int promoteBy, int64_t startRange, int64_t endRange,
		const string& defName, int flags, int specifiedLevel)
	{
		return false;
	}

	bool RichTextBox::Delete(int64_t startRange, int64_t endRange)
	{
		return false;
	}

	void* RichTextBox::WriteTable(int rows, int cols, void* tableAttr, void* cellAttr)
	{
		return nullptr;
	}

	void RichTextBox::SetBasicStyle(void* style)
	{

	}

	void* RichTextBox::GetBasicStyle()
	{
		return nullptr;
	}

	bool RichTextBox::BeginStyle(void* style)
	{
		return false;
	}

	bool RichTextBox::HasCharacterAttributes(int64_t startRange, int64_t endRange, void* style)
	{
		return false;
	}

	void* RichTextBox::GetStyleSheet()
	{
		return nullptr;
	}

	void RichTextBox::SetAndShowDefaultStyle(void* attr)
	{

	}

	void RichTextBox::SetSelectionRange(int64_t startRange, int64_t endRange)
	{

	}

	Int32Point RichTextBox::PositionToXY(int64_t pos)
	{
		return Int32Point();
	}

	void* RichTextBox::WriteTextBox(void* textAttr)
	{
		return nullptr;
	}

	bool RichTextBox::HasParagraphAttributes(int64_t startRange, int64_t endRange, void* style)
	{
		return false;
	}

	bool RichTextBox::SetProperties(int64_t startRange, int64_t endRange, void* properties, int flags)
	{
		return false;
	}

	void RichTextBox::SetTextCursor(void* cursor)
	{

	}

	void* RichTextBox::GetTextCursor()
	{
		return nullptr;
	}

	void RichTextBox::SetURLCursor(void* cursor)
	{

	}

	void* RichTextBox::GetURLCursor()
	{
		return nullptr;
	}

	void* RichTextBox::GetSelection()
	{
		return nullptr;
	}

	void* RichTextBox::GetContextMenuPropertiesInfo()
	{
		return nullptr;
	}

	void RichTextBox::SetSelection2(void* sel)
	{

	}

	bool RichTextBox::WriteImage(Image* bitmap, int bitmapType, void* textAttr)
	{
		return false;
	}

	bool RichTextBox::WriteImage2(const string& filename, int bitmapType, void* textAttr)
	{
		return false;
	}

	bool RichTextBox::WriteImage3(void* imageBlock, void* textAttr)
	{
		return false;
	}

	void* RichTextBox::WriteField(const string& fieldType, void* properties, void* textAttr)
	{
		return nullptr;
	}

	bool RichTextBox::CanDeleteRange(void* container, int64_t startRange, int64_t endRange)
	{
		return false;
	}

	bool RichTextBox::CanInsertContent(void* container, int64_t pos)
	{
		return false;
	}

	void* RichTextBox::GetBuffer()
	{
		return nullptr;
	}

	int64_t RichTextBox::DeleteSelectedContent()
	{
		return 0;
	}

	bool RichTextBox::BeginLeftIndent(int leftIndent, int leftSubIndent)
	{
		return false;
	}

	bool RichTextBox::EndLeftIndent()
	{
		return false;
	}

	bool RichTextBox::BeginRightIndent(int rightIndent)
	{
		return false;
	}

	bool RichTextBox::EndRightIndent()
	{
		return false;
	}

	bool RichTextBox::BeginParagraphSpacing(int before, int after)
	{
		return false;
	}

	bool RichTextBox::EndParagraphSpacing()
	{
		return false;
	}

	bool RichTextBox::BeginLineSpacing(int lineSpacing)
	{
		return false;
	}

	bool RichTextBox::EndLineSpacing()
	{
		return false;
	}

	bool RichTextBox::BeginNumberedBullet(int bulletNumber, int leftIndent,
		int leftSubIndent, int bulletStyle)
	{
		return false;
	}

	bool RichTextBox::EndNumberedBullet()
	{
		return false;
	}

	bool RichTextBox::BeginSymbolBullet(const string& symbol, int leftIndent,
		int leftSubIndent, int bulletStyle)
	{
		return false;
	}

	bool RichTextBox::EndSymbolBullet()
	{
		return false;
	}

	bool RichTextBox::BeginStandardBullet(const string& bulletName,
		int leftIndent, int leftSubIndent, int bulletStyle)
	{
		return false;
	}

	bool RichTextBox::EndStandardBullet()
	{
		return false;
	}

	bool RichTextBox::BeginCharacterStyle(const string& characterStyle)
	{
		return false;
	}

	bool RichTextBox::EndCharacterStyle()
	{
		return false;
	}

	bool RichTextBox::BeginParagraphStyle(const string& paragraphStyle)
	{
		return false;
	}

	bool RichTextBox::EndParagraphStyle()
	{
		return false;
	}

	bool RichTextBox::BeginListStyle(const string& listStyle, int level, int number)
	{
		return false;
	}

	bool RichTextBox::EndListStyle()
	{
		return false;
	}

	bool RichTextBox::BeginURL(const string& url, const string& characterStyle)
	{
		return false;
	}

	bool RichTextBox::EndURL()
	{
		return false;
	}

	bool RichTextBox::IsSelectionBold()
	{
		return false;
	}

	bool RichTextBox::IsSelectionItalics()
	{
		return false;
	}

	bool RichTextBox::IsSelectionUnderlined()
	{
		return false;
	}

	bool RichTextBox::DoesSelectionHaveTextEffectFlag(int flag)
	{
		return false;
	}

	bool RichTextBox::IsSelectionAligned(int alignment)
	{
		return false;
	}

	bool RichTextBox::ApplyBoldToSelection()
	{
		return false;
	}

	bool RichTextBox::ApplyItalicToSelection()
	{
		return false;
	}

	bool RichTextBox::ApplyUnderlineToSelection()
	{
		return false;
	}

	bool RichTextBox::ApplyTextEffectToSelection(int flags)
	{
		return false;
	}

	bool RichTextBox::ApplyAlignmentToSelection(int alignment)
	{
		return false;
	}

	bool RichTextBox::ApplyStyle(void* def)
	{
		return false;
	}

	void RichTextBox::SetStyleSheet(void* styleSheet)
	{
	}

	bool RichTextBox::SetDefaultStyleToCursorStyle()
	{
		return false;
	}

	void RichTextBox::SelectNone()
	{

	}

	bool RichTextBox::SelectWord(int64_t position)
	{
		return false;
	}

	bool RichTextBox::LayoutContent(bool onlyVisibleRect)
	{
		return false;
	}

	bool RichTextBox::MoveCaret(int64_t pos, bool showAtLineStart, void* container)
	{
		return false;
	}

	bool RichTextBox::MoveRight(int noPositions, int flags)
	{
		return false;
	}

	bool RichTextBox::MoveLeft(int noPositions, int flags)
	{
		return false;
	}

	bool RichTextBox::MoveUp(int noLines, int flags)
	{
		return false;
	}

	bool RichTextBox::MoveDown(int noLines, int flags)
	{
		return false;
	}

	bool RichTextBox::MoveToLineEnd(int flags)
	{
		return false;
	}

	bool RichTextBox::MoveToLineStart(int flags)
	{
		return false;
	}

	bool RichTextBox::MoveToParagraphEnd(int flags)
	{
		return false;
	}

	bool RichTextBox::MoveToParagraphStart(int flags)
	{
		return false;
	}

	bool RichTextBox::MoveHome(int flags)
	{
		return false;
	}

	bool RichTextBox::MoveEnd(int flags)
	{
		return false;
	}

	bool RichTextBox::PageUp(int noPages, int flags)
	{
		return false;
	}

	bool RichTextBox::PageDown(int noPages, int flags)
	{
		return false;
	}

	bool RichTextBox::WordLeft(int noPages, int flags)
	{
		return false;
	}

	bool RichTextBox::WordRight(int noPages, int flags)
	{
		return false;
	}

	bool RichTextBox::PushStyleSheet(void* styleSheet)
	{
		return false;
	}

	void* RichTextBox::PopStyleSheet()
	{
		return nullptr;
	}

	bool RichTextBox::ApplyStyleSheet(void* styleSheet)
	{
		return false;
	}

	bool RichTextBox::ShowContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return false;
	}

	int RichTextBox::PrepareContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return 0;
	}

	bool RichTextBox::CanEditProperties(void* richObj)
	{
		return false;
	}

	bool RichTextBox::EditProperties(void* richObj, void* parentWindow)
	{
		return false;
	}

	string RichTextBox::GetPropertiesMenuLabel(void* richObj)
	{
		return wxStr(wxEmptyString);
	}

	bool RichTextBox::BeginBatchUndo(const string& cmdName)
	{
		return false;
	}

	bool RichTextBox::EndBatchUndo()
	{
		return false;
	}

	bool RichTextBox::BatchingUndo()
	{
		return false;
	}

	bool RichTextBox::BeginSuppressUndo()
	{
		return false;
	}

	bool RichTextBox::EndSuppressUndo()
	{
		return false;
	}

	bool RichTextBox::SuppressingUndo()
	{
		return false;
	}

	void RichTextBox::EnableVerticalScrollbar(bool enable)
	{

	}

	bool RichTextBox::GetVerticalScrollbarEnabled()
	{
		return false;
	}

	void RichTextBox::SetFontScale(double fontScale, bool refresh)
	{

	}

	double RichTextBox::GetFontScale()
	{
		return 0;
	}

	bool RichTextBox::GetVirtualAttributesEnabled()
	{
		return false;
	}

	void RichTextBox::EnableVirtualAttributes(bool b)
	{

	}

	void RichTextBox::DoWriteText(const string& value, int flags)
	{

	}

	bool RichTextBox::ExtendSelection(int64_t oldPosition, int64_t newPosition, int flags)
	{
		return false;
	}

	bool RichTextBox::ExtendCellSelection(void* table, int noRowSteps, int noColSteps)
	{
		return false;
	}

	bool RichTextBox::StartCellSelection(void* table, void* newCell)
	{
		return false;
	}

	bool RichTextBox::ScrollIntoView(int64_t position, int keyCode)
	{
		return false;
	}

	void RichTextBox::SetCaretPosition(int64_t position, bool showAtLineStart)
	{

	}

	int64_t RichTextBox::GetCaretPosition()
	{
		return 0;
	}

	int64_t RichTextBox::GetAdjustedCaretPosition(int64_t caretPos)
	{
		return 0;
	}

	void RichTextBox::MoveCaretForward(int64_t oldPosition)
	{

	}

	Int32Point RichTextBox::GetPhysicalPoint(const Int32Point& ptLogical)
	{
		return Int32Point();
	}

	Int32Point RichTextBox::GetLogicalPoint(const Int32Point& ptPhysical)
	{
		return Int32Point();
	}

	int64_t RichTextBox::FindNextWordPosition(int direction)
	{
		return 0;
	}

	string RichTextBox::GetRange(int64_t from, int64_t to)
	{
		return wxStr(wxEmptyString);
	}

	int RichTextBox::GetLineLength(int64_t lineNo)
	{
		return 0;
	}

	string RichTextBox::GetLineText(int64_t lineNo)
	{
		return wxStr(wxEmptyString);
	}

	int RichTextBox::GetNumberOfLines()
	{
		return 0;
	}

	bool RichTextBox::IsModified()
	{
		return false;
	}

	bool RichTextBox::IsEditable()
	{
		return false;
	}

	bool RichTextBox::IsSingleLine()
	{
		return false;
	}

	bool RichTextBox::IsMultiLine()
	{
		return false;
	}

	string RichTextBox::GetStringSelection()
	{
		return wxStr(wxEmptyString);
	}

	string RichTextBox::GetFilename()
	{
		return wxStr(wxEmptyString);
	}

	void RichTextBox::SetFilename(const string& filename)
	{

	}

	void RichTextBox::SetDelayedLayoutThreshold(int64_t threshold)
	{

	}

	int64_t RichTextBox::GetDelayedLayoutThreshold()
	{
		return 0;
	}

	bool RichTextBox::GetFullLayoutRequired()
	{
		return false;
	}

	void RichTextBox::SetFullLayoutRequired(bool b)
	{

	}

	int64_t RichTextBox::GetFullLayoutTime()
	{
		return 0;
	}

	void RichTextBox::SetFullLayoutTime(int64_t t)
	{

	}

	int64_t RichTextBox::GetFullLayoutSavedPosition()
	{
		return 0;
	}

	void RichTextBox::SetFullLayoutSavedPosition(int64_t p)
	{

	}

	void RichTextBox::ForceDelayedLayout()
	{

	}

	bool RichTextBox::GetCaretAtLineStart()
	{
		return false;
	}

	void RichTextBox::SetCaretAtLineStart(bool atStart)
	{

	}

	bool RichTextBox::GetDragging()
	{
		return false;
	}

	void RichTextBox::SetDragging(bool dragging)
	{

	}

	void* RichTextBox::GetContextMenu()
	{
		return nullptr;
	}

	void RichTextBox::SetContextMenu(void* menu)
	{

	}

	int64_t RichTextBox::GetSelectionAnchor()
	{
		return 0;
	}

	void RichTextBox::SetSelectionAnchor(int64_t anchor)
	{

	}

	void* RichTextBox::GetSelectionAnchorObject()
	{
		return nullptr;
	}

	void RichTextBox::SetSelectionAnchorObject(void* anchor)
	{

	}

	void* RichTextBox::GetFocusObject()
	{
		return nullptr;
	}

	void RichTextBox::StoreFocusObject(void* richObj)
	{

	}

	bool RichTextBox::SetFocusObject(void* richObj, bool setCaretPosition)
	{
		return false;
	}

	void RichTextBox::Invalidate()
	{

	}

	void RichTextBox::Clear()
	{

	}

	void RichTextBox::Replace(int64_t from, int64_t to, const string& value)
	{

	}

	void RichTextBox::Remove(int64_t from, int64_t to)
	{

	}

	bool RichTextBox::LoadFile(const string& file, int type)
	{
		return false;
	}

	bool RichTextBox::SaveFile(const string& file, int type)
	{
		return false;
	}

	void RichTextBox::SetHandlerFlags(int flags)
	{

	}

	int RichTextBox::GetHandlerFlags()
	{
		return 0;
	}

	void RichTextBox::MarkDirty()
	{

	}

	void RichTextBox::DiscardEdits()
	{

	}

	void RichTextBox::SetMaxLength(uint64_t len)
	{

	}

	void RichTextBox::WriteText(const string& text)
	{

	}

	void RichTextBox::AppendText(const string& text)
	{

	}

	int64_t RichTextBox::XYToPosition(int64_t x, int64_t y)
	{
		return 0;
	}

	void RichTextBox::ShowPosition(int64_t pos)
	{

	}

	void RichTextBox::Copy()
	{

	}

	void RichTextBox::Cut()
	{

	}

	void RichTextBox::Paste()
	{

	}

	void RichTextBox::DeleteSelection()
	{

	}

	bool RichTextBox::CanCopy()
	{
		return false;
	}

	bool RichTextBox::CanCut()
	{
		return false;
	}

	bool RichTextBox::CanPaste()
	{
		return false;
	}

	bool RichTextBox::CanDeleteSelection()
	{
		return false;
	}

	void RichTextBox::Undo()
	{

	}

	void RichTextBox::Redo()
	{

	}

	bool RichTextBox::CanUndo()
	{
		return false;
	}

	bool RichTextBox::CanRedo()
	{
		return false;
	}

	void RichTextBox::SetInsertionPoint(int64_t pos)
	{

	}

	void RichTextBox::SetInsertionPointEnd()
	{

	}

	int64_t RichTextBox::GetInsertionPoint()
	{
		return 0;
	}

	void RichTextBox::SetSelection(int64_t from, int64_t to)
	{

	}

	void RichTextBox::SetEditable(bool editable)
	{

	}

	bool RichTextBox::HasSelection()
	{
		return false;
	}

	bool RichTextBox::HasUnfocusedSelection()
	{
		return false;
	}

	bool RichTextBox::Newline()
	{
		return false;
	}

	bool RichTextBox::LineBreak()
	{
		return false;
	}

	bool RichTextBox::EndStyle()
	{
		return false;
	}

	bool RichTextBox::EndAllStyles()
	{
		return false;
	}

	bool RichTextBox::BeginBold()
	{
		return false;
	}

	bool RichTextBox::EndBold()
	{
		return false;
	}

	bool RichTextBox::BeginItalic()
	{
		return false;
	}

	bool RichTextBox::EndItalic()
	{
		return false;
	}

	bool RichTextBox::BeginUnderline()
	{
		return false;
	}

	bool RichTextBox::EndUnderline()
	{
		return false;
	}

	bool RichTextBox::BeginFontSize(int pointSize)
	{
		return false;
	}

	bool RichTextBox::EndFontSize()
	{
		return false;
	}

	bool RichTextBox::BeginFont(Font* font)
	{
		return false;
	}

	bool RichTextBox::EndFont()
	{
		return false;
	}

	bool RichTextBox::BeginTextColour(const Color& colour)
	{
		return false;
	}

	bool RichTextBox::EndTextColour()
	{
		return false;
	}

	bool RichTextBox::BeginAlignment(int alignment)
	{
		return false;
	}

	bool RichTextBox::EndAlignment()
	{
		return false;
	}
}
