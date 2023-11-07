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
		return GetTextCtrl()->IsPositionVisible(pos);
	}

	int64_t RichTextBox::GetFirstVisiblePosition()
	{
		return GetTextCtrl()->GetFirstVisiblePosition();
	}

	int64_t RichTextBox::GetCaretPositionForDefaultStyle()
	{
		return GetTextCtrl()->GetCaretPositionForDefaultStyle();
	}

	void RichTextBox::SetCaretPositionForDefaultStyle(int64_t pos)
	{
		GetTextCtrl()->SetCaretPositionForDefaultStyle(pos);
	}

	void RichTextBox::MoveCaretBack(int64_t oldPosition)
	{
		GetTextCtrl()->MoveCaretBack(oldPosition);
	}

	bool RichTextBox::GetCaretPositionForIndex(int64_t position, const Int32Rect& rect, void* container)
	{
		wxRect wxr = rect;
		return GetTextCtrl()->GetCaretPositionForIndex(position, wxr,
			(wxRichTextParagraphLayoutBox*)container);
	}

	void* RichTextBox::GetVisibleLineForCaretPosition(int64_t caretPosition)
	{
		return GetTextCtrl()->GetVisibleLineForCaretPosition(caretPosition);
	}

	void* RichTextBox::GetCommandProcessor()
	{
		return GetTextCtrl()->GetCommandProcessor();
	}

	bool RichTextBox::IsDefaultStyleShowing()
	{
		return GetTextCtrl()->IsDefaultStyleShowing();
	}

	Int32Point RichTextBox::GetFirstVisiblePoint()
	{
		return GetTextCtrl()->GetFirstVisiblePoint();
	}

	void RichTextBox::EnableImages(bool b)
	{

	}

	bool RichTextBox::GetImagesEnabled()
	{
		return GetTextCtrl()->GetImagesEnabled();
	}

	void RichTextBox::EnableDelayedImageLoading(bool b)
	{

	}

	bool RichTextBox::GetDelayedImageLoading()
	{
		return GetTextCtrl()->GetDelayedImageLoading();
	}

	bool RichTextBox::GetDelayedImageProcessingRequired()
	{
		return GetTextCtrl()->GetDelayedImageProcessingRequired();
	}

	void RichTextBox::SetDelayedImageProcessingRequired(bool b)
	{

	}

	int64_t RichTextBox::GetDelayedImageProcessingTime()
	{
		return GetTextCtrl()->GetDelayedImageProcessingTime();
	}

	void RichTextBox::SetDelayedImageProcessingTime(int64_t t)
	{

	}

	string RichTextBox::GetValue()
	{
		return wxStr(GetTextCtrl()->GetValue());
	}

	void RichTextBox::SetValue(const string& value)
	{

	}

	void RichTextBox::SetLineHeight(int height)
	{

	}

	int RichTextBox::GetLineHeight()
	{
		return GetTextCtrl()->GetLineHeight();
	}

	bool RichTextBox::SetCaretPositionAfterClick(void* container, int64_t position,
		int hitTestFlags, bool extendSelection)
	{
		return GetTextCtrl()->SetCaretPositionAfterClick(
			(wxRichTextParagraphLayoutBox*) container, position,
			hitTestFlags, extendSelection);
	}

	/*static*/ void RichTextBox::ClearAvailableFontNames()
	{
		wxRichTextCtrl::ClearAvailableFontNames();
	}

	bool RichTextBox::ProcessDelayedImageLoading(bool refresh)
	{
		return GetTextCtrl()->ProcessDelayedImageLoading(refresh);
	}

	void RichTextBox::RequestDelayedImageProcessing()
	{

	}

	bool RichTextBox::GetUncombinedStyle(int64_t position, void* style)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->GetUncombinedStyle(position, *s);
	}

	bool RichTextBox::GetUncombinedStyle2(int64_t position, void* style, void* container)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->GetUncombinedStyle(position, *s, (wxRichTextParagraphLayoutBox*)container);
	}

	bool RichTextBox::SetDefaultStyle(void* style)
	{
		wxTextAttr* s = (wxTextAttr*)style;
		return GetTextCtrl()->SetDefaultStyle(*s);
	}

	bool RichTextBox::SetDefaultRichStyle(void* style)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->SetDefaultStyle(*s);
	}

	void* RichTextBox::GetDefaultStyleEx()
	{
		wxRichTextAttr result = GetTextCtrl()->GetDefaultStyle();
		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(result);
		return style;
	}

	int64_t RichTextBox::GetLastPosition()
	{
		return GetTextCtrl()->GetLastPosition();
	}

	void* RichTextBox::GetStyle(int64_t position)
	{
		wxTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyle(position, textAttr);
		if (!result)
			return nullptr;

		wxTextAttr* style = new wxTextAttr();
		style->Copy(textAttr);
		return style;
	}

	void* RichTextBox::GetRichStyle(int64_t position)
	{
		wxRichTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyle(position, textAttr);
		if (!result)
			return nullptr;

		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(textAttr);
		return style;
	}

	void* RichTextBox::GetStyleInContainer(int64_t position, void* container)
	{
		wxRichTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyle(position, textAttr,
			(wxRichTextParagraphLayoutBox*)container);
		if (!result)
			return nullptr;

		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(textAttr);
		return style;
	}

	bool RichTextBox::SetStyle(int64_t start, int64_t end, void* style)
	{
		wxTextAttr* s = (wxTextAttr*)style;
		return GetTextCtrl()->SetStyle(start, end, *s);
	}

	bool RichTextBox::SetRichStyle(int64_t start, int64_t end, void* style)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->SetStyle(start, end, *s);
	}

	void RichTextBox::SetStyle2(void* richObj, void* textAttr, int flags)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
		return GetTextCtrl()->SetStyle((wxRichTextObject*)richObj, *s, flags);
	}

	void* RichTextBox::GetStyleForRange(int64_t startRange, int64_t endRange)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);

		wxTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyleForRange(range, textAttr);
		if (!result)
			return nullptr;

		wxTextAttr* style = new wxTextAttr();
		style->Copy(textAttr);
		return style;
	}

	void* RichTextBox::GetStyleForRange2(int64_t startRange, int64_t endRange)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);

		wxRichTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyleForRange(range, textAttr);
		if (!result)
			return nullptr;

		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(textAttr);
		return style;
	}

	void* RichTextBox::GetStyleForRange3(int64_t startRange, int64_t endRange, void* container)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextAttr textAttr;
		auto result = GetTextCtrl()->GetStyleForRange(range, textAttr,
			(wxRichTextParagraphLayoutBox*)container);
		if (!result)
			return nullptr;
		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(textAttr);
		return style;		
	}

	bool RichTextBox::SetStyleEx(int64_t startRange, int64_t endRange, void* style, int flags)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->SetStyleEx(range, *s, flags);
	}

	bool RichTextBox::SetListStyle(int64_t startRange, int64_t endRange,
		void* def, int flags, int startFrom, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->SetListStyle(range,
			(wxRichTextListStyleDefinition*)def, flags, startFrom, specifiedLevel);
	}

	bool RichTextBox::SetListStyle2(int64_t startRange, int64_t endRange,
		const string& defName, int flags, int startFrom, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->SetListStyle(range,
			wxStr(defName), flags, startFrom, specifiedLevel);
	}

	bool RichTextBox::ClearListStyle(int64_t startRange, int64_t endRange, int flags)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->ClearListStyle(range, flags);
	}

	bool RichTextBox::NumberList(int64_t startRange, int64_t endRange, void* def,
		int flags, int startFrom, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->NumberList(range, (wxRichTextListStyleDefinition*)def,
			flags, startFrom, specifiedLevel);
	}

	bool RichTextBox::NumberList2(int64_t startRange, int64_t endRange,
		const string& defName, int flags, int startFrom, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->NumberList(range,
			wxStr(defName), flags, startFrom, specifiedLevel);
	}

	bool RichTextBox::PromoteList(int promoteBy, int64_t startRange,
		int64_t endRange, void* def, int flags, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->PromoteList(promoteBy, range,
			(wxRichTextListStyleDefinition*)def, flags, specifiedLevel);
	}

	bool RichTextBox::PromoteList2(int promoteBy, int64_t startRange, int64_t endRange,
		const string& defName, int flags, int specifiedLevel)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->PromoteList(promoteBy, range,
			wxStr(defName), flags, specifiedLevel);
	}

	bool RichTextBox::Delete(int64_t startRange, int64_t endRange)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		return GetTextCtrl()->Delete(range);
	}

	void* RichTextBox::WriteTable(int rows, int cols, void* tableAttr, void* cellAttr)
	{
		wxRichTextAttr* wxtableAttr = (wxRichTextAttr*)tableAttr;
		wxRichTextAttr* wxcellAttr = (wxRichTextAttr*)cellAttr;
		return GetTextCtrl()->WriteTable(rows, cols, *wxtableAttr, *wxcellAttr);
	}

	void RichTextBox::SetBasicStyle(void* style)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->SetBasicStyle(*s);
	}

	void* RichTextBox::GetBasicStyle()
	{
		wxRichTextAttr* style = new wxRichTextAttr();
		style->Copy(GetTextCtrl()->GetBasicStyle());
		return style;
	}

	bool RichTextBox::BeginStyle(void* style)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->BeginStyle(*s);
	}

	bool RichTextBox::HasCharacterAttributes(int64_t startRange, int64_t endRange, void* style)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->HasCharacterAttributes(range, *s);
	}

	void* RichTextBox::GetStyleSheet()
	{
		return GetTextCtrl()->GetStyleSheet();
	}

	void RichTextBox::SetAndShowDefaultStyle(void* attr)
	{

	}

	void RichTextBox::SetSelectionRange(int64_t startRange, int64_t endRange)
	{

	}

	Int32Point RichTextBox::PositionToXY(int64_t pos)
	{
		long x;
		long y;
		auto result = GetTextCtrl()->PositionToXY(pos, &x, &y);
		if (result)
			return Int32Point(x, y);
		return Int32Point(-1, -1);
	}

	void* RichTextBox::WriteTextBox(void* textAttr)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
		return GetTextCtrl()->WriteTextBox(*s);
	}

	bool RichTextBox::HasParagraphAttributes(int64_t startRange, int64_t endRange, void* style)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextAttr* s = (wxRichTextAttr*)style;
		return GetTextCtrl()->HasParagraphAttributes(range, *s);
	}

	bool RichTextBox::SetProperties(int64_t startRange, int64_t endRange, void* properties, int flags)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextProperties* s = (wxRichTextProperties*)properties;
		return GetTextCtrl()->SetProperties(range, *s, flags);
	}

	void RichTextBox::SetTextCursor(void* cursor)
	{

	}

	void* RichTextBox::GetTextCursor()
	{
		// !!!!!!!!!!!!!
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
		// wxRichTextSelection&
		return nullptr;
	}

	void* RichTextBox::GetContextMenuPropertiesInfo()
	{
		//wxRichTextContextMenuPropertiesInfo&
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
		wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
		return GetTextCtrl()->WriteImage(wxStr(filename), (wxBitmapType)bitmapType, *s);
	}

	bool RichTextBox::WriteImage3(void* imageBlock, void* textAttr)
	{
		return false;
	}

	void* RichTextBox::WriteField(const string& fieldType, void* properties, void* textAttr)
	{
		return GetTextCtrl()->nullptr;
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
		return false;
	}

	int64_t RichTextBox::DeleteSelectedContent()
	{
		return GetTextCtrl()->DeleteSelectedContent();
	}

	bool RichTextBox::BeginLeftIndent(int leftIndent, int leftSubIndent)
	{
		return GetTextCtrl()->BeginLeftIndent(leftIndent, leftSubIndent);
	}

	bool RichTextBox::EndLeftIndent()
	{
		return GetTextCtrl()->EndLeftIndent();
	}

	bool RichTextBox::BeginRightIndent(int rightIndent)
	{
		return GetTextCtrl()->BeginRightIndent(rightIndent);
	}

	bool RichTextBox::EndRightIndent()
	{
		return GetTextCtrl()->EndRightIndent();
	}

	bool RichTextBox::BeginParagraphSpacing(int before, int after)
	{
		return GetTextCtrl()->BeginParagraphSpacing(before, after);
	}

	bool RichTextBox::EndParagraphSpacing()
	{
		return GetTextCtrl()->EndParagraphSpacing();
	}

	bool RichTextBox::BeginLineSpacing(int lineSpacing)
	{
		return GetTextCtrl()->BeginLineSpacing(lineSpacing);
	}

	bool RichTextBox::EndLineSpacing()
	{
		return GetTextCtrl()->EndLineSpacing();
	}

	bool RichTextBox::BeginNumberedBullet(int bulletNumber, int leftIndent,
		int leftSubIndent, int bulletStyle)
	{
		return GetTextCtrl()->BeginNumberedBullet(bulletNumber, leftIndent,
			leftSubIndent, bulletStyle);
	}

	bool RichTextBox::EndNumberedBullet()
	{
		return GetTextCtrl()->EndNumberedBullet();
	}

	bool RichTextBox::BeginSymbolBullet(const string& symbol, int leftIndent,
		int leftSubIndent, int bulletStyle)
	{
		return GetTextCtrl()->BeginSymbolBullet(wxStr(symbol), leftIndent,
			leftSubIndent, bulletStyle);
	}

	bool RichTextBox::EndSymbolBullet()
	{
		return GetTextCtrl()->EndSymbolBullet();
	}

	bool RichTextBox::BeginStandardBullet(const string& bulletName,
		int leftIndent, int leftSubIndent, int bulletStyle)
	{
		return GetTextCtrl()->BeginStandardBullet(wxStr(bulletName),
			leftIndent, leftSubIndent, bulletStyle);
	}

	bool RichTextBox::EndStandardBullet()
	{
		return GetTextCtrl()->EndStandardBullet();
	}

	bool RichTextBox::BeginCharacterStyle(const string& characterStyle)
	{
		return GetTextCtrl()->BeginCharacterStyle(wxStr(characterStyle));
	}

	bool RichTextBox::EndCharacterStyle()
	{
		return GetTextCtrl()->EndCharacterStyle();
	}

	bool RichTextBox::BeginParagraphStyle(const string& paragraphStyle)
	{
		return GetTextCtrl()->BeginParagraphStyle(wxStr(paragraphStyle));
	}

	bool RichTextBox::EndParagraphStyle()
	{
		return GetTextCtrl()->EndParagraphStyle();
	}

	bool RichTextBox::BeginListStyle(const string& listStyle, int level, int number)
	{
		return GetTextCtrl()->BeginListStyle(wxStr(listStyle), level, number);
	}

	bool RichTextBox::EndListStyle()
	{
		return GetTextCtrl()->EndListStyle();
	}

	bool RichTextBox::BeginURL(const string& url, const string& characterStyle)
	{
		return GetTextCtrl()->BeginURL(wxStr(url), wxStr(characterStyle));
	}

	bool RichTextBox::EndURL()
	{
		return GetTextCtrl()->EndURL();
	}

	bool RichTextBox::IsSelectionBold()
	{
		return GetTextCtrl()->IsSelectionBold();
	}

	bool RichTextBox::IsSelectionItalics()
	{
		return GetTextCtrl()->IsSelectionItalics();
	}

	bool RichTextBox::IsSelectionUnderlined()
	{
		return GetTextCtrl()->IsSelectionUnderlined();
	}

	bool RichTextBox::DoesSelectionHaveTextEffectFlag(int flag)
	{
		return GetTextCtrl()->DoesSelectionHaveTextEffectFlag(flag);
	}

	bool RichTextBox::IsSelectionAligned(int alignment)
	{
		return GetTextCtrl()->IsSelectionAligned(alignment);
	}

	bool RichTextBox::ApplyBoldToSelection()
	{
		return GetTextCtrl()->ApplyBoldToSelection();
	}

	bool RichTextBox::ApplyItalicToSelection()
	{
		return GetTextCtrl()->ApplyItalicToSelection();
	}

	bool RichTextBox::ApplyUnderlineToSelection()
	{
		return GetTextCtrl()->ApplyUnderlineToSelection();
	}

	bool RichTextBox::ApplyTextEffectToSelection(int flags)
	{
		return GetTextCtrl()->ApplyTextEffectToSelection(flags);
	}

	bool RichTextBox::ApplyAlignmentToSelection(int alignment)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::ApplyStyle(void* def)
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetStyleSheet(void* styleSheet)
	{
	}

	bool RichTextBox::SetDefaultStyleToCursorStyle()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SelectNone()
	{

	}

	bool RichTextBox::SelectWord(int64_t position)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::LayoutContent(bool onlyVisibleRect)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveCaret(int64_t pos, bool showAtLineStart, void* container)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveRight(int noPositions, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveLeft(int noPositions, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveUp(int noLines, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveDown(int noLines, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveToLineEnd(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveToLineStart(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveToParagraphEnd(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveToParagraphStart(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveHome(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::MoveEnd(int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::PageUp(int noPages, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::PageDown(int noPages, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::WordLeft(int noPages, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::WordRight(int noPages, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::PushStyleSheet(void* styleSheet)
	{
		return GetTextCtrl()->;
	}

	void* RichTextBox::PopStyleSheet()
	{
		return GetTextCtrl()->nullptr;
	}

	bool RichTextBox::ApplyStyleSheet(void* styleSheet)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::ShowContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return GetTextCtrl()->;
	}

	int RichTextBox::PrepareContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return GetTextCtrl()->0;
	}

	bool RichTextBox::CanEditProperties(void* richObj)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EditProperties(void* richObj, void* parentWindow)
	{
		return GetTextCtrl()->;
	}

	string RichTextBox::GetPropertiesMenuLabel(void* richObj)
	{
		return GetTextCtrl()->wxStr(wxEmptyString);
	}

	bool RichTextBox::BeginBatchUndo(const string& cmdName)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndBatchUndo()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BatchingUndo()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginSuppressUndo()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndSuppressUndo()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::SuppressingUndo()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::EnableVerticalScrollbar(bool enable)
	{

	}

	bool RichTextBox::GetVerticalScrollbarEnabled()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetFontScale(double fontScale, bool refresh)
	{

	}

	double RichTextBox::GetFontScale()
	{
		return GetTextCtrl()->0;
	}

	bool RichTextBox::GetVirtualAttributesEnabled()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::EnableVirtualAttributes(bool b)
	{

	}

	void RichTextBox::DoWriteText(const string& value, int flags)
	{

	}

	bool RichTextBox::ExtendSelection(int64_t oldPosition, int64_t newPosition, int flags)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::ExtendCellSelection(void* table, int noRowSteps, int noColSteps)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::StartCellSelection(void* table, void* newCell)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::ScrollIntoView(int64_t position, int keyCode)
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetCaretPosition(int64_t position, bool showAtLineStart)
	{

	}

	int64_t RichTextBox::GetCaretPosition()
	{
		return GetTextCtrl()->0;
	}

	int64_t RichTextBox::GetAdjustedCaretPosition(int64_t caretPos)
	{
		return GetTextCtrl()->0;
	}

	void RichTextBox::MoveCaretForward(int64_t oldPosition)
	{

	}

	Int32Point RichTextBox::GetPhysicalPoint(const Int32Point& ptLogical)
	{
		return GetTextCtrl()->Int32Point();
	}

	Int32Point RichTextBox::GetLogicalPoint(const Int32Point& ptPhysical)
	{
		return GetTextCtrl()->Int32Point();
	}

	int64_t RichTextBox::FindNextWordPosition(int direction)
	{
		return GetTextCtrl()->0;
	}

	string RichTextBox::GetRange(int64_t from, int64_t to)
	{
		return GetTextCtrl()->wxStr(wxEmptyString);
	}

	int RichTextBox::GetLineLength(int64_t lineNo)
	{
		return GetTextCtrl()->0;
	}

	string RichTextBox::GetLineText(int64_t lineNo)
	{
		return GetTextCtrl()->wxStr(wxEmptyString);
	}

	int RichTextBox::GetNumberOfLines()
	{
		return GetTextCtrl()->0;
	}

	bool RichTextBox::IsModified()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::IsEditable()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::IsSingleLine()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::IsMultiLine()
	{
		return GetTextCtrl()->;
	}

	string RichTextBox::GetStringSelection()
	{
		return GetTextCtrl()->wxStr(wxEmptyString);
	}

	string RichTextBox::GetFilename()
	{
		return GetTextCtrl()->wxStr(wxEmptyString);
	}

	void RichTextBox::SetFilename(const string& filename)
	{

	}

	void RichTextBox::SetDelayedLayoutThreshold(int64_t threshold)
	{

	}

	int64_t RichTextBox::GetDelayedLayoutThreshold()
	{
		return GetTextCtrl()->0;
	}

	bool RichTextBox::GetFullLayoutRequired()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetFullLayoutRequired(bool b)
	{

	}

	int64_t RichTextBox::GetFullLayoutTime()
	{
		return GetTextCtrl()->0;
	}

	void RichTextBox::SetFullLayoutTime(int64_t t)
	{

	}

	int64_t RichTextBox::GetFullLayoutSavedPosition()
	{
		return GetTextCtrl()->0;
	}

	void RichTextBox::SetFullLayoutSavedPosition(int64_t p)
	{

	}

	void RichTextBox::ForceDelayedLayout()
	{

	}

	bool RichTextBox::GetCaretAtLineStart()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetCaretAtLineStart(bool atStart)
	{

	}

	bool RichTextBox::GetDragging()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetDragging(bool dragging)
	{

	}

	void* RichTextBox::GetContextMenu()
	{
		return GetTextCtrl()->nullptr;
	}

	void RichTextBox::SetContextMenu(void* menu)
	{

	}

	int64_t RichTextBox::GetSelectionAnchor()
	{
		return GetTextCtrl()->0;
	}

	void RichTextBox::SetSelectionAnchor(int64_t anchor)
	{

	}

	void* RichTextBox::GetSelectionAnchorObject()
	{
		return GetTextCtrl()->nullptr;
	}

	void RichTextBox::SetSelectionAnchorObject(void* anchor)
	{

	}

	void* RichTextBox::GetFocusObject()
	{
		return GetTextCtrl()->nullptr;
	}

	void RichTextBox::StoreFocusObject(void* richObj)
	{

	}

	bool RichTextBox::SetFocusObject(void* richObj, bool setCaretPosition)
	{
		return GetTextCtrl()->;
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
		return GetTextCtrl()->;
	}

	bool RichTextBox::SaveFile(const string& file, int type)
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetHandlerFlags(int flags)
	{

	}

	int RichTextBox::GetHandlerFlags()
	{
		return GetTextCtrl()->0;
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
		return GetTextCtrl()->0;
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
		return GetTextCtrl()->;
	}

	bool RichTextBox::CanCut()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::CanPaste()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::CanDeleteSelection()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::Undo()
	{

	}

	void RichTextBox::Redo()
	{

	}

	bool RichTextBox::CanUndo()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::CanRedo()
	{
		return GetTextCtrl()->;
	}

	void RichTextBox::SetInsertionPoint(int64_t pos)
	{

	}

	void RichTextBox::SetInsertionPointEnd()
	{

	}

	int64_t RichTextBox::GetInsertionPoint()
	{
		return GetTextCtrl()->0;
	}

	void RichTextBox::SetSelection(int64_t from, int64_t to)
	{

	}

	void RichTextBox::SetEditable(bool editable)
	{

	}

	bool RichTextBox::HasSelection()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::HasUnfocusedSelection()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::Newline()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::LineBreak()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndStyle()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndAllStyles()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginBold()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndBold()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginItalic()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndItalic()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginUnderline()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndUnderline()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginFontSize(int pointSize)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndFontSize()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginFont(Font* font)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndFont()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginTextColour(const Color& colour)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndTextColour()
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::BeginAlignment(int alignment)
	{
		return GetTextCtrl()->;
	}

	bool RichTextBox::EndAlignment()
	{
		return GetTextCtrl()->;
	}
}
