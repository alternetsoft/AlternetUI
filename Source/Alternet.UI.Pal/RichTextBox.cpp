#include "RichTextBox.h"

namespace Alternet::UI
{
	class wxRichTextCtrl2 : public wxRichTextCtrl, public wxWidgetExtender
	{
	public:
		wxRichTextCtrl2(){}
		wxRichTextCtrl2(wxWindow* parent, wxWindowID id = -1,
			const wxString& value = wxEmptyString, const wxPoint& pos = wxDefaultPosition,
			const wxSize& size = wxDefaultSize,
			long style = wxRE_MULTILINE, const wxValidator& validator = wxDefaultValidator,
			const wxString& name = wxASCII_STR(wxTextCtrlNameStr))
			: wxRichTextCtrl(parent, id, value, pos, size, style, validator, name)
		{}

	};

	wxWindow* RichTextBox::CreateWxWindowUnparented()
	{
		return new wxRichTextCtrl2();
	}

	bool RichTextBox::GetHasBorder()
	{
		return hasBorder;
	}

	void RichTextBox::SetHasBorder(bool value)
	{
		if (hasBorder == value)
			return;
		hasBorder = value;
		RecreateWxWindowIfNeeded();
	}

	wxWindow* RichTextBox::CreateWxWindowCore(wxWindow* parent)
	{
/*
#define wxRE_READONLY          0x0010
#define wxRE_CENTRE_CARET      0x8000
*/
		long style = wxRE_MULTILINE;

		if (!hasBorder)
			style = style | wxBORDER_NONE;

		auto result = new wxRichTextCtrl2(parent, -1,
			wxEmptyString, wxDefaultPosition,
			wxDefaultSize,
			style);

		result->Bind(wxEVT_TEXT_ENTER, &RichTextBox::OnTextEnter, this);
		result->Bind(wxEVT_TEXT_URL, &RichTextBox::OnTextUrl, this);

		return result;
	}

	string RichTextBox::GetReportedUrl()
	{
		return _eventUrl;
	}

	void RichTextBox::OnTextUrl(wxTextUrlEvent& event)
	{
		event.Skip();
		const wxMouseEvent& ev = event.GetMouseEvent();

		// filter out mouse moves, too many of them
		if (ev.Moving())
			return;

	/*	if (!ev.LeftDown())
			return;*/
/*
		long start = event.GetURLStart();
		long end = event.GetURLEnd();
		long delta = end - start;
  */
		/*LogEvent(event);*/

	/*	auto url = GetTextCtrl()->GetValue().Mid(start, delta).Clone();*/

		_eventUrl = wxStr(event.GetString());

		RaiseEvent(RichTextBoxEvent::TextUrl);
	}

	void RichTextBox::OnTextEnter(wxCommandEvent& event)
	{
		event.Skip();
		RaiseEvent(RichTextBoxEvent::TextEnter);
	}

	wxRichTextCtrl* RichTextBox::GetTextCtrl()
	{
		return dynamic_cast<wxRichTextCtrl*>(GetWxWindow());
	}

	RichTextBox::RichTextBox()
	{
		bindScrollEvents = false;
	}

	RichTextBox::~RichTextBox()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				window->Unbind(wxEVT_TEXT_ENTER, &RichTextBox::OnTextEnter, this);
				window->Unbind(wxEVT_TEXT_URL, &RichTextBox::OnTextUrl, this);
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
		GetTextCtrl()->EnableImages(b);
	}

	bool RichTextBox::GetImagesEnabled()
	{
		return GetTextCtrl()->GetImagesEnabled();
	}

	void RichTextBox::EnableDelayedImageLoading(bool b)
	{
		GetTextCtrl()->EnableDelayedImageLoading(b);
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
		GetTextCtrl()->SetDelayedImageProcessingRequired(b);
	}

	int64_t RichTextBox::GetDelayedImageProcessingTime()
	{
		return GetTextCtrl()->GetDelayedImageProcessingTime().ToLong();
	}

	void RichTextBox::SetDelayedImageProcessingTime(int64_t t)
	{
		GetTextCtrl()->SetDelayedImageProcessingTime(t);
	}

	string RichTextBox::GetValue()
	{
		return wxStr(GetTextCtrl()->GetValue());
	}

	void RichTextBox::SetValue(const string& value)
	{
		GetTextCtrl()->SetValue(wxStr(value));
	}

	void RichTextBox::SetLineHeight(int height)
	{
		GetTextCtrl()->SetLineHeight(height);
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

	void RichTextBox::ClearAvailableFontNames()
	{
		wxRichTextCtrl::ClearAvailableFontNames();
	}

	bool RichTextBox::ProcessDelayedImageLoading(bool refresh)
	{
		return GetTextCtrl()->ProcessDelayedImageLoading(refresh);
	}

	void RichTextBox::RequestDelayedImageProcessing()
	{
		GetTextCtrl()->RequestDelayedImageProcessing();
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
		wxRichTextAttr result = GetTextCtrl()->GetDefaultStyleEx();
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
		auto wxtableAttr = (wxRichTextAttr*)tableAttr;
		auto wxcellAttr = (wxRichTextAttr*)cellAttr;

		wxRichTextAttr ta;
		wxRichTextAttr tc;

		if (wxtableAttr != nullptr)
			ta.Copy(*wxtableAttr);
		if (wxcellAttr != nullptr)
			tc.Copy(*wxcellAttr);

		return GetTextCtrl()->WriteTable(rows, cols, ta, tc);
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
		wxRichTextAttr* s = (wxRichTextAttr*)attr;
		GetTextCtrl()->SetAndShowDefaultStyle(*s);
	}

	void RichTextBox::SetSelectionRange(int64_t startRange, int64_t endRange)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		GetTextCtrl()->SetSelectionRange(range);
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

	bool RichTextBox::WriteImage2(const string& filename, int bitmapType, void* textAttr)
	{
		if(textAttr == nullptr)
			return GetTextCtrl()->WriteImage(wxStr(filename), (wxBitmapType)bitmapType);
		else
		{
			wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
			return GetTextCtrl()->WriteImage(wxStr(filename), (wxBitmapType)bitmapType, *s);
		}
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

	bool RichTextBox::ApplyStyleToSelection(void* style, int flags)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)style;

		if (GetTextCtrl()->HasSelection())
			return GetTextCtrl()->SetStyleEx(GetTextCtrl()->GetSelectionRange(), *s,
				wxRICHTEXT_SETSTYLE_WITH_UNDO | wxRICHTEXT_SETSTYLE_OPTIMIZE | flags);
		else
		{
			wxRichTextAttr current = GetTextCtrl()->GetDefaultStyleEx();
			current.Apply(*s);
			GetTextCtrl()->SetAndShowDefaultStyle(current);
		}
		return true;
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
		return GetTextCtrl()->ApplyStyle((wxRichTextStyleDefinition*)def);
	}

	void RichTextBox::SetStyleSheet(void* styleSheet)
	{
		GetTextCtrl()->SetStyleSheet((wxRichTextStyleSheet*)styleSheet);
	}

	bool RichTextBox::SetDefaultStyleToCursorStyle()
	{
		return GetTextCtrl()->SetDefaultStyleToCursorStyle();
	}

	void RichTextBox::SelectNone()
	{
		GetTextCtrl()->SelectNone();
	}

	bool RichTextBox::SelectWord(int64_t position)
	{
		return GetTextCtrl()->SelectWord(position);
	}

	bool RichTextBox::LayoutContent(bool onlyVisibleRect)
	{
		return GetTextCtrl()->LayoutContent(onlyVisibleRect);
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
		return GetTextCtrl()->PushStyleSheet((wxRichTextStyleSheet*)styleSheet);
	}

	void* RichTextBox::PopStyleSheet()
	{
		return GetTextCtrl()->PopStyleSheet();
	}

	bool RichTextBox::ApplyStyleSheet(void* styleSheet)
	{
		return GetTextCtrl()->ApplyStyleSheet((wxRichTextStyleSheet*)styleSheet);
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
		GetTextCtrl()->EnableVerticalScrollbar(enable);
	}

	bool RichTextBox::GetVerticalScrollbarEnabled()
	{
		return GetTextCtrl()->GetVerticalScrollbarEnabled();
	}

	void RichTextBox::SetFontScale(Coord fontScale, bool refresh)
	{
		GetTextCtrl()->SetFontScale(fontScale, refresh);
	}

	Coord RichTextBox::GetFontScale()
	{
		return GetTextCtrl()->GetFontScale();
	}

	bool RichTextBox::GetVirtualAttributesEnabled()
	{
		return GetTextCtrl()->GetVirtualAttributesEnabled();
	}

	void RichTextBox::EnableVirtualAttributes(bool b)
	{
		GetTextCtrl()->EnableVirtualAttributes(b);
	}

	void RichTextBox::DoWriteText(const string& value, int flags)
	{
		GetTextCtrl()->DoWriteText(wxStr(value), flags);
	}

	bool RichTextBox::ExtendSelection(int64_t oldPosition, int64_t newPosition, int flags)
	{
		return GetTextCtrl()->ExtendSelection(oldPosition, newPosition, flags);
	}

	bool RichTextBox::ScrollIntoView(int64_t position, int keyCode)
	{
		return GetTextCtrl()->ScrollIntoView(position, keyCode);
	}

	void RichTextBox::SetCaretPosition(int64_t position, bool showAtLineStart)
	{
		GetTextCtrl()->SetCaretPosition(position, showAtLineStart);
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
		GetTextCtrl()->MoveCaretForward(oldPosition);
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
		GetTextCtrl()->SetFilename(wxStr(filename));
	}

	void RichTextBox::SetDelayedLayoutThreshold(int64_t threshold)
	{
		GetTextCtrl()->SetDelayedLayoutThreshold(threshold);
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
		GetTextCtrl()->SetFullLayoutRequired(b);
	}

	int64_t RichTextBox::GetFullLayoutTime()
	{
		return GetTextCtrl()->GetFullLayoutTime().ToLong();
	}

	void RichTextBox::SetFullLayoutTime(int64_t t)
	{
		GetTextCtrl()->SetFullLayoutTime(t);
	}

	int64_t RichTextBox::GetFullLayoutSavedPosition()
	{
		return GetTextCtrl()->GetFullLayoutSavedPosition();
	}

	void RichTextBox::SetFullLayoutSavedPosition(int64_t p)
	{
		GetTextCtrl()->SetFullLayoutSavedPosition(p);
	}

	void RichTextBox::ForceDelayedLayout()
	{
		GetTextCtrl()->ForceDelayedLayout();
	}

	bool RichTextBox::GetCaretAtLineStart()
	{
		return GetTextCtrl()->GetCaretAtLineStart();
	}

	void RichTextBox::SetCaretAtLineStart(bool atStart)
	{
		GetTextCtrl()->SetCaretAtLineStart(atStart);
	}

	bool RichTextBox::GetDragging()
	{
		return GetTextCtrl()->GetDragging();
	}

	void RichTextBox::SetDragging(bool dragging)
	{
		GetTextCtrl()->SetDragging(dragging);
	}

	void* RichTextBox::GetContextMenu()
	{
		return GetTextCtrl()->GetContextMenu();
	}

	void RichTextBox::SetContextMenu(void* menu)
	{
		GetTextCtrl()->SetContextMenu((wxMenu*) menu);
	}

	int64_t RichTextBox::GetSelectionAnchor()
	{
		return GetTextCtrl()->GetSelectionAnchor();
	}

	void RichTextBox::SetSelectionAnchor(int64_t anchor)
	{
		GetTextCtrl()->SetSelectionAnchor(anchor);
	}

	void* RichTextBox::GetSelectionAnchorObject()
	{
		return GetTextCtrl()->GetSelectionAnchorObject();
	}

	void RichTextBox::SetSelectionAnchorObject(void* anchor)
	{
		GetTextCtrl()->SetSelectionAnchorObject((wxRichTextObject*)anchor);
	}

	void* RichTextBox::GetFocusObject()
	{
		return GetTextCtrl()->GetFocusObject();
	}

	void RichTextBox::StoreFocusObject(void* richObj)
	{
		GetTextCtrl()->StoreFocusObject((wxRichTextParagraphLayoutBox*)richObj);
	}

	bool RichTextBox::SetFocusObject(void* richObj, bool setCaretPosition)
	{
		return GetTextCtrl()->SetFocusObject((wxRichTextParagraphLayoutBox*)richObj, setCaretPosition);
	}

	void RichTextBox::Invalidate()
	{
		GetTextCtrl()->Invalidate();
	}

	void RichTextBox::Clear()
	{
		GetTextCtrl()->Clear();
	}

	void RichTextBox::Replace(int64_t from, int64_t to, const string& value)
	{
		GetTextCtrl()->Replace(from, to, wxStr(value));
	}

	void RichTextBox::Remove(int64_t from, int64_t to)
	{
		GetTextCtrl()->Remove(from, to);
	}

	bool RichTextBox::LoadFile(const string& file, int type)
	{
		return GetTextCtrl()->LoadFile(wxStr(file), type);
	}

	void RichTextBox::InitFileHandlers()
	{
		/*
		Disabled as requires compat library on macos and it is not supported there properly
		wxRichTextBuffer::AddHandler(new wxRichTextXMLHandler);*/
		wxRichTextBuffer::AddHandler(new wxRichTextHTMLHandler);
	}

	bool RichTextBox::SaveFile(const string& file, int type)
	{
		return GetTextCtrl()->SaveFile(wxStr(file), type);
	}

	void RichTextBox::SetHandlerFlags(int flags)
	{
		GetTextCtrl()->SetHandlerFlags(flags);
	}

	int RichTextBox::GetHandlerFlags()
	{
		return GetTextCtrl()->GetHandlerFlags();
	}

	void RichTextBox::MarkDirty()
	{
		GetTextCtrl()->MarkDirty();
	}

	void RichTextBox::DiscardEdits()
	{
		GetTextCtrl()->DiscardEdits();
	}

	void RichTextBox::SetMaxLength(uint64_t len)
	{
		GetTextCtrl()->SetMaxLength(len);
	}

	void RichTextBox::WriteText(const string& text)
	{
		GetTextCtrl()->WriteText(wxStr(text));
	}

	void RichTextBox::AppendText(const string& text)
	{
		GetTextCtrl()->AppendText(wxStr(text));
	}

	int64_t RichTextBox::XYToPosition(int64_t x, int64_t y)
	{
		return GetTextCtrl()->XYToPosition(x, y);
	}

	void RichTextBox::ShowPosition(int64_t pos)
	{
		GetTextCtrl()->ShowPosition(pos);
	}

	void RichTextBox::Copy()
	{
		GetTextCtrl()->Copy();
	}

	void RichTextBox::Cut()
	{
		GetTextCtrl()->Cut();
	}

	void RichTextBox::Paste()
	{
		GetTextCtrl()->Paste();
	}

	void RichTextBox::DeleteSelection()
	{
		GetTextCtrl()->DeleteSelection();
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
		GetTextCtrl()->Undo();
	}

	void RichTextBox::Redo()
	{
		GetTextCtrl()->Redo();
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
		GetTextCtrl()->SetInsertionPoint(pos);
	}

	void RichTextBox::SetInsertionPointEnd()
	{
		GetTextCtrl()->SetInsertionPointEnd();
	}

	int64_t RichTextBox::GetInsertionPoint()
	{
		return GetTextCtrl()->GetInsertionPoint();
	}

	void RichTextBox::SetSelection(int64_t from, int64_t to)
	{
		GetTextCtrl()->SetSelection(from, to);
	}

	void RichTextBox::SetEditable(bool editable)
	{
		GetTextCtrl()->SetEditable(editable);
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

	bool RichTextBox::MoveCaret(int64_t pos, bool showAtLineStart, void* container)
	{
		return GetTextCtrl()->MoveCaret(pos, showAtLineStart,
			(wxRichTextParagraphLayoutBox*)container);
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

	void* RichTextBox::GetSelection()
	{
		return new wxRichTextSelection(GetTextCtrl()->GetSelection());
	}

	bool RichTextBox::EndAlignment()
	{
		return GetTextCtrl()->EndAlignment();
	}

	bool RichTextBox::BeginFont(Font* font)
	{
		if (font == nullptr)
			return GetTextCtrl()->BeginFont(wxNullFont);
		return GetTextCtrl()->BeginFont(font->GetWxFont());
	}

	bool RichTextBox::WriteImage(Image* bitmap, int bitmapType, void* textAttr)
	{
		if (bitmap == nullptr)
			return false;
		if(textAttr == nullptr)
			return GetTextCtrl()->WriteImage(bitmap->GetBitmap(), (wxBitmapType)bitmapType);
		else
		{
			wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
			return GetTextCtrl()->WriteImage(bitmap->GetBitmap(), (wxBitmapType)bitmapType, *s);
		}
	}

	bool RichTextBox::WriteImage3(void* imageBlock, void* textAttr)
	{
		wxRichTextImageBlock* block = (wxRichTextImageBlock*)imageBlock;
		if (textAttr == nullptr)
			return GetTextCtrl()->WriteImage(*block);
		else
		{
			wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
			return GetTextCtrl()->WriteImage(*block, *s);
		}
	}

	bool RichTextBox::CanDeleteRange(void* container, int64_t startRange, int64_t endRange)
	{
		wxRichTextRange range = wxRichTextRange(startRange, endRange);
		wxRichTextParagraphLayoutBox* s = (wxRichTextParagraphLayoutBox*)container;
		return GetTextCtrl()->CanDeleteRange(*s, range);
	}

	bool RichTextBox::CanInsertContent(void* container, int64_t pos)
	{
		wxRichTextParagraphLayoutBox* s = (wxRichTextParagraphLayoutBox*)container;
		return GetTextCtrl()->CanInsertContent(*s, pos);
	}

	bool RichTextBox::ExtendCellSelection(void* table, int noRowSteps, int noColSteps)
	{
		return GetTextCtrl()->ExtendCellSelection((wxRichTextTable*)table, noRowSteps, noColSteps);;
	}

	bool RichTextBox::StartCellSelection(void* table, void* newCell)
	{
		return GetTextCtrl()->StartCellSelection((wxRichTextTable*)table,
			(wxRichTextParagraphLayoutBox*)newCell);
	}

	void RichTextBox::SetSelection2(void* sel)
	{
		wxRichTextSelection* s = (wxRichTextSelection*)sel;
		GetTextCtrl()->SetSelection(*s);
	}

	void* RichTextBox::WriteField(const string& fieldType, void* properties, void* textAttr)
	{
		wxRichTextAttr* s = (wxRichTextAttr*)textAttr;
		wxRichTextProperties* p = (wxRichTextProperties*)properties;
		return GetTextCtrl()->WriteField(wxStr(fieldType), *p, *s);
	}

	void RichTextBox::SetTextCursor(void* cursor)
	{
		if (cursor == nullptr)
			GetTextCtrl()->SetTextCursor(wxCursor());
		else
		{
			auto wx = (wxCursor*)cursor;
			auto wcursor = wxCursor(*wx);			
			GetTextCtrl()->SetTextCursor(wcursor);
		}
	}

	void* RichTextBox::GetTextCursor()
	{
		return new wxCursor(GetTextCtrl()->GetTextCursor());
	}

	void RichTextBox::SetURLCursor(void* cursor)
	{
		if (cursor == nullptr)
			GetTextCtrl()->SetURLCursor(wxCursor());
		else
		{
			auto wx = (wxCursor*)cursor;
			auto wcursor = wxCursor(*wx);
			GetTextCtrl()->SetURLCursor(wcursor);
		}
	}

	bool RichTextBox::LoadFromStream(void* stream, int type)
	{
		InputStream inputStream(stream);
		ManagedInputStream managedInputStream(&inputStream);
		auto flags = GetHandlerFlags();

		auto handler = wxRichTextBuffer::FindHandler((wxRichTextFileType)type);
		if (handler == nullptr)
			return false;
		handler->SetFlags(flags);
		auto result = handler->LoadFile(&GetTextCtrl()->GetBuffer(), managedInputStream);
		return result;
	}

	bool RichTextBox::SaveToStream(void* stream, int type)
	{
		OutputStream outputStream(stream);
		ManagedOutputStream managedOutputStream(&outputStream);
		auto flags = GetHandlerFlags();

		auto handler = wxRichTextBuffer::FindHandler((wxRichTextFileType) type);
		if (handler == nullptr)
			return false;
		handler->SetFlags(flags);
		auto result = handler->SaveFile(&GetTextCtrl()->GetBuffer(), managedOutputStream);
		return result;
	}

	void* RichTextBox::GetURLCursor()
	{
		return new wxCursor(GetTextCtrl()->GetURLCursor());
	}

	void* RichTextBox::GetContextMenuPropertiesInfo()
	{
		//!!!!!!!!!!!!
		//wxRichTextContextMenuPropertiesInfo& 
		return nullptr;
	}

	void* RichTextBox::GetBuffer()
	{
		// !!!!!!!!!!!!!
		// wxRichTextBuffer&
		return nullptr;
	}
}

