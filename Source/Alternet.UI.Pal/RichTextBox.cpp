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
		Int32Rect r = rect;
		wxRect wxr = r;
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
		return GetTextCtrl()->GetDelayedImageProcessingTime().ToLong();
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
		return GetTextCtrl()->IsSelectionAligned((wxTextAttrAlignment)alignment);
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
		return GetTextCtrl()->ApplyAlignmentToSelection((wxTextAttrAlignment)alignment);
	}

	bool RichTextBox::ApplyStyle(void* def)
	{
		// !!!!!
		return false;
	}

	void RichTextBox::SetStyleSheet(void* styleSheet)
	{
	}

	bool RichTextBox::SetDefaultStyleToCursorStyle()
	{
		return GetTextCtrl()->SetDefaultStyleToCursorStyle();
	}

	void RichTextBox::SelectNone()
	{

	}

	bool RichTextBox::SelectWord(int64_t position)
	{
		return GetTextCtrl()->SelectWord(position);
	}

	bool RichTextBox::LayoutContent(bool onlyVisibleRect)
	{
		return GetTextCtrl()->LayoutContent(onlyVisibleRect);
	}

	bool RichTextBox::MoveCaret(int64_t pos, bool showAtLineStart, void* container)
	{
		// !!!!!
		return false;
	}

	bool RichTextBox::MoveRight(int noPositions, int flags)
	{
		return GetTextCtrl()->MoveRight(noPositions, flags);
	}

	bool RichTextBox::MoveLeft(int noPositions, int flags)
	{
		return GetTextCtrl()->MoveLeft(noPositions, flags);
	}

	bool RichTextBox::MoveUp(int noLines, int flags)
	{
		return GetTextCtrl()->MoveUp(noLines, flags);
	}

	bool RichTextBox::MoveDown(int noLines, int flags)
	{
		return GetTextCtrl()->MoveDown(noLines, flags);
	}

	bool RichTextBox::MoveToLineEnd(int flags)
	{
		return GetTextCtrl()->MoveToLineEnd(flags);
	}

	bool RichTextBox::MoveToLineStart(int flags)
	{
		return GetTextCtrl()->MoveToLineStart(flags);
	}

	bool RichTextBox::MoveToParagraphEnd(int flags)
	{
		return GetTextCtrl()->MoveToParagraphEnd(flags);
	}

	bool RichTextBox::MoveToParagraphStart(int flags)
	{
		return GetTextCtrl()->MoveToParagraphStart(flags);
	}

	bool RichTextBox::MoveHome(int flags)
	{
		return GetTextCtrl()->MoveHome(flags);
	}

	bool RichTextBox::MoveEnd(int flags)
	{
		return GetTextCtrl()->MoveEnd(flags);
	}

	bool RichTextBox::PageUp(int noPages, int flags)
	{
		return GetTextCtrl()->PageUp(noPages, flags);
	}

	bool RichTextBox::PageDown(int noPages, int flags)
	{
		return GetTextCtrl()->PageDown(noPages, flags);
	}

	bool RichTextBox::WordLeft(int noPages, int flags)
	{
		return GetTextCtrl()->WordLeft(noPages, flags);
	}

	bool RichTextBox::WordRight(int noPages, int flags)
	{
		return GetTextCtrl()->WordRight(noPages, flags);
	}

	bool RichTextBox::PushStyleSheet(void* styleSheet)
	{
		return false;
	}

	void* RichTextBox::PopStyleSheet()
	{
		return GetTextCtrl()->PopStyleSheet();
	}

	bool RichTextBox::ApplyStyleSheet(void* styleSheet)
	{
		return false;
	}

	bool RichTextBox::ShowContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return GetTextCtrl()->ShowContextMenu((wxMenu*)menu, pt, addPropertyCommands);
	}

	int RichTextBox::PrepareContextMenu(void* menu, const Int32Point& pt, bool addPropertyCommands)
	{
		return GetTextCtrl()->PrepareContextMenu((wxMenu*) menu, pt, addPropertyCommands);
	}

	bool RichTextBox::CanEditProperties(void* richObj)
	{
		return GetTextCtrl()->CanEditProperties((wxRichTextObject*)richObj);
	}

	bool RichTextBox::EditProperties(void* richObj, void* parentWindow)
	{
		return GetTextCtrl()->EditProperties((wxRichTextObject*)richObj, (wxWindow*)parentWindow);
	}

	string RichTextBox::GetPropertiesMenuLabel(void* richObj)
	{
		return wxStr(GetTextCtrl()->GetPropertiesMenuLabel((wxRichTextObject*)richObj));
	}

	bool RichTextBox::BeginBatchUndo(const string& cmdName)
	{
		return GetTextCtrl()->BeginBatchUndo(wxStr(cmdName));
	}

	bool RichTextBox::EndBatchUndo()
	{
		return GetTextCtrl()->EndBatchUndo();
	}

	bool RichTextBox::BatchingUndo()
	{
		return GetTextCtrl()->BatchingUndo();
	}

	bool RichTextBox::BeginSuppressUndo()
	{
		return GetTextCtrl()->BeginSuppressUndo();
	}

	bool RichTextBox::EndSuppressUndo()
	{
		return GetTextCtrl()->EndSuppressUndo();
	}

	bool RichTextBox::SuppressingUndo()
	{
		return GetTextCtrl()->SuppressingUndo();
	}

	void RichTextBox::EnableVerticalScrollbar(bool enable)
	{

	}

	bool RichTextBox::GetVerticalScrollbarEnabled()
	{
		return GetTextCtrl()->GetVerticalScrollbarEnabled();
	}

	void RichTextBox::SetFontScale(double fontScale, bool refresh)
	{

	}

	double RichTextBox::GetFontScale()
	{
		return GetTextCtrl()->GetFontScale();
	}

	bool RichTextBox::GetVirtualAttributesEnabled()
	{
		return GetTextCtrl()->GetVirtualAttributesEnabled();
	}

	void RichTextBox::EnableVirtualAttributes(bool b)
	{

	}

	void RichTextBox::DoWriteText(const string& value, int flags)
	{

	}

	bool RichTextBox::ExtendSelection(int64_t oldPosition, int64_t newPosition, int flags)
	{
		return GetTextCtrl()->ExtendSelection(oldPosition, newPosition, flags);
	}

	bool RichTextBox::ExtendCellSelection(void* table, int noRowSteps, int noColSteps)
	{
		// !!!!!!!!!!
		return false;
	}

	bool RichTextBox::StartCellSelection(void* table, void* newCell)
	{
		// !!!!!!!!!!
		return false;
	}

	bool RichTextBox::ScrollIntoView(int64_t position, int keyCode)
	{
		return GetTextCtrl()->ScrollIntoView(position, keyCode);
	}

	void RichTextBox::SetCaretPosition(int64_t position, bool showAtLineStart)
	{

	}

	int64_t RichTextBox::GetCaretPosition()
	{
		return GetTextCtrl()->GetCaretPosition();
	}

	int64_t RichTextBox::GetAdjustedCaretPosition(int64_t caretPos)
	{
		return GetTextCtrl()->GetAdjustedCaretPosition(caretPos);
	}

	void RichTextBox::MoveCaretForward(int64_t oldPosition)
	{

	}

	Int32Point RichTextBox::GetPhysicalPoint(const Int32Point& ptLogical)
	{
		return GetTextCtrl()->GetPhysicalPoint(ptLogical);
	}

	Int32Point RichTextBox::GetLogicalPoint(const Int32Point& ptPhysical)
	{
		return GetTextCtrl()->GetLogicalPoint(ptPhysical);
	}

	int64_t RichTextBox::FindNextWordPosition(int direction)
	{
		return GetTextCtrl()->FindNextWordPosition(direction);
	}

	string RichTextBox::GetRange(int64_t from, int64_t to)
	{
		return wxStr(GetTextCtrl()->GetRange(from, to));
	}

	int RichTextBox::GetLineLength(int64_t lineNo)
	{
		return GetTextCtrl()->GetLineLength(lineNo);
	}

	string RichTextBox::GetLineText(int64_t lineNo)
	{
		return wxStr(GetTextCtrl()->GetLineText(lineNo));
	}

	int RichTextBox::GetNumberOfLines()
	{
		return GetTextCtrl()->GetNumberOfLines();
	}

	bool RichTextBox::IsModified()
	{
		return GetTextCtrl()->IsModified();
	}

	bool RichTextBox::IsEditable()
	{
		return GetTextCtrl()->IsEditable();
	}

	bool RichTextBox::IsSingleLine()
	{
		return GetTextCtrl()->IsSingleLine();
	}

	bool RichTextBox::IsMultiLine()
	{
		return GetTextCtrl()->IsMultiLine();
	}

	string RichTextBox::GetStringSelection()
	{
		return wxStr(GetTextCtrl()->GetStringSelection());
	}

	string RichTextBox::GetFilename()
	{
		return wxStr(GetTextCtrl()->GetFilename());
	}

	void RichTextBox::SetFilename(const string& filename)
	{

	}

	void RichTextBox::SetDelayedLayoutThreshold(int64_t threshold)
	{

	}

	int64_t RichTextBox::GetDelayedLayoutThreshold()
	{
		return GetTextCtrl()->GetDelayedLayoutThreshold();
	}

	bool RichTextBox::GetFullLayoutRequired()
	{
		return GetTextCtrl()->GetFullLayoutRequired();
	}

	void RichTextBox::SetFullLayoutRequired(bool b)
	{

	}

	int64_t RichTextBox::GetFullLayoutTime()
	{
		return GetTextCtrl()->GetFullLayoutTime().ToLong();
	}

	void RichTextBox::SetFullLayoutTime(int64_t t)
	{

	}

	int64_t RichTextBox::GetFullLayoutSavedPosition()
	{
		return GetTextCtrl()->GetFullLayoutSavedPosition();
	}

	void RichTextBox::SetFullLayoutSavedPosition(int64_t p)
	{

	}

	void RichTextBox::ForceDelayedLayout()
	{

	}

	bool RichTextBox::GetCaretAtLineStart()
	{
		return GetTextCtrl()->GetCaretAtLineStart();
	}

	void RichTextBox::SetCaretAtLineStart(bool atStart)
	{

	}

	bool RichTextBox::GetDragging()
	{
		return GetTextCtrl()->GetDragging();
	}

	void RichTextBox::SetDragging(bool dragging)
	{

	}

	void* RichTextBox::GetContextMenu()
	{
		return GetTextCtrl()->GetContextMenu();
	}

	void RichTextBox::SetContextMenu(void* menu)
	{

	}

	int64_t RichTextBox::GetSelectionAnchor()
	{
		return GetTextCtrl()->GetSelectionAnchor();
	}

	void RichTextBox::SetSelectionAnchor(int64_t anchor)
	{

	}

	void* RichTextBox::GetSelectionAnchorObject()
	{
		return GetTextCtrl()->GetSelectionAnchorObject();
	}

	void RichTextBox::SetSelectionAnchorObject(void* anchor)
	{

	}

	void* RichTextBox::GetFocusObject()
	{
		return GetTextCtrl()->GetFocusObject();
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
		return GetTextCtrl()->LoadFile(wxStr(file), type);
	}

	bool RichTextBox::SaveFile(const string& file, int type)
	{
		return GetTextCtrl()->SaveFile(wxStr(file), type);
	}

	void RichTextBox::SetHandlerFlags(int flags)
	{

	}

	int RichTextBox::GetHandlerFlags()
	{
		return GetTextCtrl()->GetHandlerFlags();
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
		return GetTextCtrl()->XYToPosition(x, y);
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
		return GetTextCtrl()->CanCopy();
	}

	bool RichTextBox::CanCut()
	{
		return GetTextCtrl()->CanCut();
	}

	bool RichTextBox::CanPaste()
	{
		return GetTextCtrl()->CanPaste();
	}

	bool RichTextBox::CanDeleteSelection()
	{
		return GetTextCtrl()->CanDeleteSelection();
	}

	void RichTextBox::Undo()
	{

	}

	void RichTextBox::Redo()
	{

	}

	bool RichTextBox::CanUndo()
	{
		return GetTextCtrl()->CanUndo();
	}

	bool RichTextBox::CanRedo()
	{
		return GetTextCtrl()->CanRedo();
	}

	void RichTextBox::SetInsertionPoint(int64_t pos)
	{

	}

	void RichTextBox::SetInsertionPointEnd()
	{

	}

	int64_t RichTextBox::GetInsertionPoint()
	{
		return GetTextCtrl()->GetInsertionPoint();
	}

	void RichTextBox::SetSelection(int64_t from, int64_t to)
	{

	}

	void RichTextBox::SetEditable(bool editable)
	{

	}

	bool RichTextBox::HasSelection()
	{
		return GetTextCtrl()->HasSelection();
	}

	bool RichTextBox::HasUnfocusedSelection()
	{
		return GetTextCtrl()->HasUnfocusedSelection();
	}

	bool RichTextBox::Newline()
	{
		return GetTextCtrl()->Newline();
	}

	bool RichTextBox::LineBreak()
	{
		return GetTextCtrl()->LineBreak();
	}

	bool RichTextBox::EndStyle()
	{
		return GetTextCtrl()->EndStyle();
	}

	bool RichTextBox::EndAllStyles()
	{
		return GetTextCtrl()->EndAllStyles();
	}

	bool RichTextBox::BeginBold()
	{
		return GetTextCtrl()->BeginBold();
	}

	bool RichTextBox::EndBold()
	{
		return GetTextCtrl()->EndBold();
	}

	bool RichTextBox::BeginItalic()
	{
		return GetTextCtrl()->BeginItalic();
	}

	bool RichTextBox::EndItalic()
	{
		return GetTextCtrl()->EndItalic();
	}

	bool RichTextBox::BeginUnderline()
	{
		return GetTextCtrl()->BeginUnderline();
	}

	bool RichTextBox::EndUnderline()
	{
		return GetTextCtrl()->EndUnderline();
	}

	bool RichTextBox::BeginFontSize(int pointSize)
	{
		return GetTextCtrl()->BeginFontSize(pointSize);
	}

	bool RichTextBox::EndFontSize()
	{
		return GetTextCtrl()->EndFontSize();
	}

	bool RichTextBox::BeginFont(Font* font)
	{
		return false;
	}

	bool RichTextBox::EndFont()
	{
		return GetTextCtrl()->EndFont();
	}

	bool RichTextBox::BeginTextColour(const Color& colour)
	{
		return GetTextCtrl()->BeginTextColour(colour);
	}

	bool RichTextBox::EndTextColour()
	{
		return GetTextCtrl()->EndTextColour();
	}

	bool RichTextBox::BeginAlignment(int alignment)
	{
		return GetTextCtrl()->BeginAlignment((wxTextAttrAlignment)alignment);
	}

	bool RichTextBox::EndAlignment()
	{
		return GetTextCtrl()->EndAlignment();
	}
}
