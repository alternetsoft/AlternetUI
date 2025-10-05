#include "TextBox.h"
#include "Application.h"

namespace Alternet::UI
{
	TextBox::TextBox() :
		_text(*this, u"", &Control::IsWxWindowCreated, &TextBox::RetrieveText, &TextBox::ApplyText)
	{
		bindScrollEvents = false;
		GetDelayedValues().Add(&_text);
	}

	TextBox::~TextBox()
	{
		if (IsWxWindowCreated())
		{
			auto window = GetWxWindow();
			if (window != nullptr)
			{
				if (_processEnter)
					window->Unbind(wxEVT_TEXT_ENTER, &TextBox::OnTextEnter, this);
				window->Unbind(wxEVT_TEXT_URL, &TextBox::OnTextUrl, this);
				window->Unbind(wxEVT_TEXT_MAXLEN, &TextBox::OnTextMaxLength, this);
			}
		}
	}

	string TextBox::GetText()
	{
		return _text.Get();
	}

	void TextBox::SetText(const string& value)
	{
		_text.Set(value);
	}

	TextBox::TextBox(void* validator) : TextBox()
	{
		_validator = validator;
	}

	void* TextBox::CreateTextBox(void* validator)
	{
		return new TextBox(validator);
	}

	void* TextBox::GetValidator()
	{
		return _validator;
	}

	void TextBox::SetValidator(void* value)
	{
		if (_validator == value)
			return;
		_validator = value;
		GetTextCtrl()->SetValidator(*(wxValidator*)_validator);
	}

	wxWindow* TextBox::CreateWxWindowUnparented()
	{
		return new TextCtrlEx();
	}

	wxWindow* TextBox::CreateWxWindowCore(wxWindow* parent)
	{
		long style = GetCreateStyle();

		TextCtrlEx* textCtrl;

		if (_validator == nullptr)
		{
			textCtrl = new TextCtrlEx(
				parent, wxID_ANY, wxEmptyString, wxDefaultPosition,
				wxDefaultSize, style, wxDefaultValidator);
		}
		else
		{
			textCtrl = new TextCtrlEx(
				parent, wxID_ANY, wxEmptyString, wxDefaultPosition,
				wxDefaultSize, style, *((wxValidator*)_validator));
		}

#ifdef __WXOSX__
		// port all platforms to the latest wx version, and then the ifdef
		// can be removed.
		// EnableVisibleFocus is implemented only on macOS at the moment through.
		if (_editControlOnly)
			textCtrl->EnableVisibleFocus(false);
#endif

		if (_processEnter)
			textCtrl->Bind(wxEVT_TEXT_ENTER, &TextBox::OnTextEnter, this);
		textCtrl->Bind(wxEVT_TEXT_URL, &TextBox::OnTextUrl, this);
		textCtrl->Bind(wxEVT_TEXT_MAXLEN, &TextBox::OnTextMaxLength, this);
		return textCtrl;
	}

	int TextBox::GetTextWrap()
	{
		return _textWrap;
	}

	void TextBox::SetTextWrap(int value)
	{
		if (_textWrap == value)
			return;
		_textWrap = value;
		RecreateWxWindowIfNeeded();
	}

	int TextBox::GetTextAlign()
	{
		return _textAlign;
	}

	void TextBox::SetTextAlign(int value)
	{
		if (_textAlign == value)
			return;
		_textAlign = value;
		RecreateWxWindowIfNeeded();
	}

#define TextBoxTextWrapBest 0
#define TextBoxTextWrapWord 1
#define TextBoxTextWrapChar 2
#define TextBoxTextWrapNone 3

	long TextBox::GetCreateStyle()
	{
		long style = _editControlOnly ? wxNO_BORDER : 0;

		if (_readOnly)
			style |= wxTE_READONLY;

		if (_multiline)
		{
			style |= wxTE_MULTILINE;

			switch (_textWrap)
			{
			case TextBoxTextWrapWord:
				style |= wxTE_WORDWRAP;
				break;
			case TextBoxTextWrapChar:
				style |= wxTE_CHARWRAP;
				break;
			case TextBoxTextWrapNone:
				style |= wxTE_DONTWRAP;
				break;
			}
		}

		if (_textAlign == wxALIGN_CENTER_HORIZONTAL || _textAlign == wxALIGN_RIGHT)
		{
			style |= _textAlign;
		}

		if (_isRichEdit)
			style |= wxTE_RICH2;

		if (_processTab)
			style |= wxTE_PROCESS_TAB;

		if (_processEnter)
			style |= wxTE_PROCESS_ENTER;

		if (_password)
			style |= wxTE_PASSWORD;

		if (_noVScroll)
			style |= wxTE_NO_VSCROLL;

		if (_autoUrl)
			style |= wxTE_AUTO_URL;

		if (_noHideSel)
			style |= wxTE_NOHIDESEL;

		return style;
	}

	bool TextBox::GetProcessTab()
	{
		return _processTab;
	}

	void TextBox::SetProcessTab(bool value)
	{
		if (_processTab == value)
			return;
		_processTab = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetProcessEnter()
	{
		return _processEnter;
	}

	void TextBox::SetProcessEnter(bool value)
	{
		if (_processEnter == value)
			return;
		_processEnter = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetIsPassword()
	{
		return _password;
	}

	void TextBox::SetIsPassword(bool value)
	{
		if (_password == value)
			return;
		_password = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetAutoUrl()
	{
		return _autoUrl;
	}

	void TextBox::SetAutoUrl(bool value)
	{
		if (_autoUrl == value)
			return;
		_autoUrl = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetHideVertScrollbar()
	{
		return _noVScroll;
	}

	void TextBox::SetHideVertScrollbar(bool value)
	{
		if (_noVScroll == value)
			return;
		_noVScroll = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetHideSelection()
	{
		return !_noHideSel;
	}

	void TextBox::SetHideSelection(bool value)
	{
		value = !value;
		if (_noHideSel == value)
			return;
		_noHideSel = value;
		RecreateWxWindowIfNeeded();
	}

	void TextBox::OnTextEnter(wxCommandEvent& event)
	{
		event.Skip();
		RaiseEvent(TextBoxEvent::TextEnter);
	}

	void LogEvent(wxTextUrlEvent& event)
	{
		const wxMouseEvent& ev = event.GetMouseEvent();
		long start = event.GetURLStart();
		long end = event.GetURLEnd();
		long delta = end - start;

		auto s_start = std::to_string(start);
		auto s_end = std::to_string(end);
		auto s_delta = std::to_string(delta);
		auto s_mouse = Control::GetMouseEventDesc(ev);

		Application::Log("==================");
		Application::Log(s_mouse);
		Application::Log(s_start + " / " + s_end + " / " + s_delta);
		Application::Log("==================");
	}

	void TextBox::OnTextUrl(wxTextUrlEvent& event)
	{
		event.Skip();
		const wxMouseEvent& ev = event.GetMouseEvent();

		// filter out mouse moves, too many of them
		if (ev.Moving())
			return;

		if (!ev.LeftDown())
			return;

		long start = event.GetURLStart();
		long end = event.GetURLEnd();
		long delta = end - start;

		/*LogEvent(event);*/

		auto url = GetTextCtrl()->GetValue().Mid(start, delta).Clone();

		_eventUrl = wxStr(url);

		// Application::Log("OnTextUrl.GetString:" + event.GetString());

		RaiseEvent(TextBoxEvent::TextUrl);
	}

	string TextBox::GetReportedUrl()
	{
		return _eventUrl;
	}

	void TextBox::OnTextMaxLength(wxCommandEvent& event)
	{
		event.Skip();
		RaiseEvent(TextBoxEvent::TextMaxLength);
	}

	bool TextBox::GetEditControlOnly()
	{
		return _editControlOnly;
	}

	void TextBox::SetEditControlOnly(bool value)
	{
		_editControlOnly = value;
		RecreateWindow();
	}

	bool TextBox::GetReadOnly()
	{
		return _readOnly;
	}

	void TextBox::SetReadOnly(bool value)
	{
		if (_readOnly == value)
			return;

		_readOnly = value;
		GetTextCtrl()->SetEditable(!_readOnly);
	}

	bool TextBox::GetMultiline()
	{
		return _multiline;
	}

	void TextBox::SetMultiline(bool value)
	{
		if (_multiline == value)
			return;

		_multiline = value;
		RecreateWxWindowIfNeeded();
	}

	wxTextCtrl* TextBox::GetTextCtrl()
	{
		return dynamic_cast<wxTextCtrl*>(GetWxWindow());
	}

	string TextBox::RetrieveText()
	{
		return wxStr(GetTextCtrl()->GetValue());
	}

	void TextBox::ApplyText(const string& value)
	{
		GetTextCtrl()->SetValue(wxStr(value));
	}

	bool TextBox::GetIsRichEdit()
	{
		return _isRichEdit;
	}

	void TextBox::SetIsRichEdit(bool value)
	{
		if (_isRichEdit == value)
			return;
		_isRichEdit = value;
		RecreateWxWindowIfNeeded();
	}

	bool TextBox::GetIsModified()
	{
		return GetTextCtrl()->IsModified();
	}

	void TextBox::SetIsModified(bool value)
	{
		GetTextCtrl()->SetModified(value);
	}

	bool TextBox::GetCanCopy()
	{
		return GetTextCtrl()->CanCopy();
	}

	bool TextBox::GetCanCut()
	{
		return GetTextCtrl()->CanCut();
	}

	bool TextBox::GetCanPaste()
	{
		return GetTextCtrl()->CanPaste();
	}

	bool TextBox::GetCanRedo()
	{
		return GetTextCtrl()->CanRedo();
	}

	bool TextBox::GetCanUndo()
	{
		return GetTextCtrl()->CanUndo();
	}

	bool TextBox::GetIsEmpty()
	{
		return GetTextCtrl()->IsEmpty();
	}

	int TextBox::GetLineLength(int64_t lineNo)
	{
		return GetTextCtrl()->GetLineLength(lineNo);
	}

	string TextBox::GetLineText(int64_t lineNo)
	{
		return wxStr(GetTextCtrl()->GetLineText(lineNo));
	}

	int TextBox::GetNumberOfLines()
	{
		return GetTextCtrl()->GetNumberOfLines();
	}

	Int32Point TextBox::PositionToXY(int64_t pos)
	{
		long x;
		long y;
		auto result = GetTextCtrl()->PositionToXY(pos, &x, &y);
		if (result)
			return Int32Point(x, y);
		return Int32Point(-1, -1);
	}

	Point TextBox::PositionToCoords(int64_t pos)
	{
		auto p = GetTextCtrl()->PositionToCoords(pos);
		return Point(p.x, p.y);
	}

	void TextBox::ShowPosition(int64_t pos)
	{
		GetTextCtrl()->ShowPosition(pos);
	}

	int64_t TextBox::XYToPosition(int64_t x, int64_t y)
	{
		return GetTextCtrl()->XYToPosition(x, y);
	}

	void TextBox::Clear()
	{
		GetTextCtrl()->Clear();
	}

	void TextBox::Copy()
	{
		GetTextCtrl()->Copy();
	}

	void TextBox::Cut()
	{
		GetTextCtrl()->Cut();
	}

	void TextBox::AppendText(const string& text)
	{
		GetTextCtrl()->AppendText(wxStr(text));
	}

	int64_t TextBox::GetInsertionPoint()
	{
		return GetTextCtrl()->GetInsertionPoint();
	}

	void TextBox::Paste()
	{
		GetTextCtrl()->Paste();
	}

	void TextBox::Redo()
	{
		GetTextCtrl()->Redo();
	}

	void TextBox::Remove(int64_t from, int64_t to)
	{
		GetTextCtrl()->Remove(from, to);
	}

	void TextBox::Replace(int64_t from, int64_t to, const string& value)
	{
		GetTextCtrl()->Replace(from, to, wxStr(value));
	}

	void TextBox::SetInsertionPoint(int64_t pos)
	{
		GetTextCtrl()->SetInsertionPoint(pos);
	}

	void TextBox::SetInsertionPointEnd()
	{
		GetTextCtrl()->SetInsertionPointEnd();
	}

	void TextBox::SetMaxLength(uint64_t len)
	{
		GetTextCtrl()->SetMaxLength(len);
	}

	void TextBox::SetSelection(int64_t from, int64_t to)
	{
		GetTextCtrl()->SetSelection(from, to);
	}

	void TextBox::SelectAll()
	{
		GetTextCtrl()->SelectAll();
	}

	void TextBox::SelectNone()
	{
		GetTextCtrl()->SelectNone();
	}

	void TextBox::TextBox::Undo()
	{
		GetTextCtrl()->Undo();
	}

	void TextBox::WriteText(const string& text)
	{
		GetTextCtrl()->WriteText(wxStr(text));
	}

	string TextBox::GetRange(int64_t from, int64_t to)
	{
		return wxStr(GetTextCtrl()->GetRange(from, to));
	}

	string TextBox::GetStringSelection()
	{
		return wxStr(GetTextCtrl()->GetStringSelection());
	}

	void TextBox::EmptyUndoBuffer()
	{
		GetTextCtrl()->EmptyUndoBuffer();
	}

	bool TextBox::IsValidPosition(int64_t pos)
	{
		return pos >= 0 && pos <= GetTextCtrl()->GetLastPosition();
	}

	int64_t TextBox::GetLastPosition()
	{
		return GetTextCtrl()->GetLastPosition();
	}

	int64_t TextBox::GetSelectionStart()
	{
		long selectionFrom;
		long selectionTo;
		GetTextCtrl()->GetSelection(&selectionFrom, &selectionTo);
		return selectionFrom;
	}

	int64_t TextBox::GetSelectionEnd()
	{
		long selectionFrom;
		long selectionTo;
		GetTextCtrl()->GetSelection(&selectionFrom, &selectionTo);
		return selectionTo;
	}

	string TextBox::GetEmptyTextHint()
	{
		return wxStr(GetTextCtrl()->GetHint());
	}

	void TextBox::SetEmptyTextHint(const string& value)
	{
		GetTextCtrl()->SetHint(wxStr(value));
	}

	void* TextBox::GetStyle(int64_t position)
	{
		wxTextAttr textAttr;

		auto result = GetTextCtrl()->GetStyle(position, textAttr);
		if (!result)
			return nullptr;

		wxTextAttr* style = new wxTextAttr();
		style->Copy(textAttr);
		return style;
	}

	bool TextBox::SetDefaultStyle(void* style)
	{
		wxTextAttr* s = (wxTextAttr*)style;

		return GetTextCtrl()->SetDefaultStyle(*s);
	}

	bool TextBox::SetStyle(int64_t start, int64_t end, void* style)
	{
		wxTextAttr* s = (wxTextAttr*)style;

		return GetTextCtrl()->SetStyle(start, end, *s);
	}

	void* TextBox::GetDefaultStyle()
	{
		wxTextAttr result = GetTextCtrl()->GetDefaultStyle();
		wxTextAttr* style = new wxTextAttr();
		style->Copy(result);
		return style;
	}

	bool TextBox::GetHasSelection()
	{
		long from, to;
		GetTextCtrl()->GetSelection(&from, &to);
		return from != to;
	}

}
