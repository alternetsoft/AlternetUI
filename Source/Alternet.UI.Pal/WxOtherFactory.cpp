#include "WxOtherFactory.h"
#include "Application.h"
#include "WxAlternet/wxAlternetRichToolTip.h"

#include <wx/numdlg.h>
#include <string.h>

namespace Alternet::UI
{
	bool WxOtherFactory::GetRichToolTipUseGeneric()
	{
		return wxRichToolTip2::AlternetRichTooltip;
	}
	
	void WxOtherFactory::SetRichToolTipUseGeneric(bool value)
	{
		wxRichToolTip2::AlternetRichTooltip = value;
	}

	void WxOtherFactory::RichToolTipSetTitleFgColor(void* handle, const Color& color)
	{
		((wxRichToolTip2*)handle)->SetTitleForegroundColour(color);
	}

	void WxOtherFactory::RichToolTipSetFgColor(void* handle, const Color& color)
	{
		((wxRichToolTip2*)handle)->SetForegroundColour(color);
	}

	void* WxOtherFactory::CreateRichToolTip(const string& title, const string& message)
	{
		return new wxRichToolTip2(wxStr(title), wxStr(message));
	}

	void WxOtherFactory::DeleteRichToolTip(void* handle)
	{
		delete (wxRichToolTip2*)handle;
	}

	void WxOtherFactory::RichToolTipSetBkColor(void* handle, const Color& color, const Color& endColor)
	{
		if(endColor.IsEmpty())
			((wxRichToolTip2*)handle)->SetBackgroundColour(color);
		else
			((wxRichToolTip2*)handle)->SetBackgroundColour(color, endColor);
	}

	void WxOtherFactory::RichToolTipSetIcon(void* handle, ImageSet* bitmapBundle)
	{
		((wxRichToolTip2*)handle)->SetIcon(ImageSet::BitmapBundle(bitmapBundle));
	}

	void WxOtherFactory::RichToolTipSetIcon2(void* handle, int icon)
	{
		((wxRichToolTip2*)handle)->SetIcon(icon);
	}

	void WxOtherFactory::RichToolTipSetTimeout(void* handle, uint32_t milliseconds, uint32_t millisecondsShowdelay)
	{
		((wxRichToolTip2*)handle)->SetTimeout(milliseconds, millisecondsShowdelay);
	}

	void WxOtherFactory::RichToolTipSetTipKind(void* handle, int tipKind)
	{
		((wxRichToolTip2*)handle)->SetTipKind((wxTipKind)tipKind);
	}

	void WxOtherFactory::RichToolTipSetTitleFont(void* handle, Font* font)
	{
		if (font == nullptr)
			((wxRichToolTip2*)handle)->SetTitleFont(wxNullFont);
		else
			((wxRichToolTip2*)handle)->SetTitleFont(font->GetWxFont());
	}

	void* WxOtherFactory::CreateToolTip(const string& tip)
	{
		return new wxToolTip(wxStr(tip));
	}

	void WxOtherFactory::DeleteToolTip(void* handle)
	{
		delete (wxToolTip*)handle;
	}

	string WxOtherFactory::ToolTipGetTip(void* handle)
	{
		return wxStr(((wxToolTip*)handle)->GetTip());
	}

	void* WxOtherFactory::ToolTipGetWindow(void* handle)
	{
		return ((wxToolTip*)handle)->GetWindow();
	}

	void WxOtherFactory::ToolTipSetTip(void* handle, const string& tip)
	{
		((wxToolTip*)handle)->SetTip(wxStr(tip));
	}

	void WxOtherFactory::ToolTipEnable(bool flag)
	{
		wxToolTip::Enable(flag);
	}

	void WxOtherFactory::ToolTipSetAutoPop(int64_t msecs)
	{
		wxToolTip::SetAutoPop(msecs);
	}

	void WxOtherFactory::ToolTipSetDelay(int64_t msecs)
	{
		wxToolTip::SetDelay(msecs);
	}

	void WxOtherFactory::ToolTipSetMaxWidth(int width)
	{
#ifdef __WXMSW__
		wxToolTip::SetMaxWidth(width);
#endif
	}

	void WxOtherFactory::ToolTipSetReshow(int64_t msecs)
	{
		wxToolTip::SetReshow(msecs);
	}

	WxOtherFactory::WxOtherFactory()
	{

	}

	WxOtherFactory::~WxOtherFactory()
	{
	}

	void WxOtherFactory::RichToolTipShowFor(void* handle, void* window, const Int32Rect& rect)
	{
		if(rect.IsZero())
			((wxRichToolTip2*)handle)->ShowFor((wxWindow*)window);
		else
		{
			auto wxr = new wxRect(rect.X, rect.Y, rect.Width, rect.Height);
			((wxRichToolTip2*)handle)->ShowFor((wxWindow*)window, wxr);
			delete wxr;
		}
	}

	void* WxOtherFactory::CreateCursor()
	{
		return new wxCursor();
	}

#ifdef __WXMSW__
	HMODULE GetCurrentModule()
	{ // NB: XP+ solution!
		HMODULE hModule = NULL;
		GetModuleHandleEx(
			GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS,
			(LPCTSTR)GetCurrentModule,
			&hModule);

		return hModule;
	}
#endif

	void* WxOtherFactory::CreateCursor2(int cursorId)
	{
#ifdef __WXMSW__
		//auto instance = wxGetInstance();
		//auto newHandle = GetCurrentModule();
		//wxSetInstance(newHandle);
#endif
		try
		{
			return new wxCursor((wxStockCursor)cursorId);
		}
		catch (...)
		{
#ifdef __WXMSW__
			//wxSetInstance(instance);
#endif
			throw;
		}
	}

	void* WxOtherFactory::CreateCursor3(const string& cursorName, int type,
		int hotSpotX, int hotSpotY)
	{
		return new wxCursor(wxStr(cursorName), (wxBitmapType)type, hotSpotX, hotSpotY);
	}

	void* WxOtherFactory::CreateCursor4(Image* image)
	{
		if (image == nullptr)
			return new wxCursor();
		return new wxCursor(image->GetBitmap().ConvertToImage());
	}

	void WxOtherFactory::DeleteCursor(void* handle)
	{
		delete (wxCursor*)handle;
	}

	bool WxOtherFactory::CursorIsOk(void* handle)
	{
		return ((wxCursor*)handle)->IsOk();
	}

	Int32Point WxOtherFactory::CursorGetHotSpot(void* handle)
	{
		return ((wxCursor*)handle)->GetHotSpot();
	}

	void WxOtherFactory::DeleteCaret(void* handle)
	{
		delete (wxCaret*)handle;
	}

	Int32Point WxOtherFactory::CaretGetPosition(void* handle)
	{
		return ((wxCaret*)handle)->GetPosition();
	}

	Int32Size WxOtherFactory::CaretGetSize(void* handle)
	{
		return ((wxCaret*)handle)->GetSize();
	}

	void WxOtherFactory::CaretMove(void* handle, int x, int y)
	{
		((wxCaret*)handle)->Move(x, y);
	}

	void WxOtherFactory::CaretSetSize(void* handle, int width, int height)
	{
		return ((wxCaret*)handle)->SetSize(width, height);
	}

	void* WxOtherFactory::CreateCaret()
	{
		return new wxCaret();
	}

	void* WxOtherFactory::CreateCaret2(void* window, int width, int height)
	{
		return new wxCaret((wxWindow*)window, width, height);
	}

	void* WxOtherFactory::CaretGetWindow(void* handle)
	{
		return ((wxCaret*)handle)->GetWindow();
	}

	void WxOtherFactory::CaretHide(void* handle)
	{
		((wxCaret*)handle)->Hide();
	}

	int WxOtherFactory::CaretGetBlinkTime()
	{
		return wxCaret::GetBlinkTime();
	}
	
	void WxOtherFactory::CaretSetBlinkTime(int milliseconds)
	{
		wxCaret::SetBlinkTime(milliseconds);
	}

	bool WxOtherFactory::CaretIsOk(void* handle)
	{
		return ((wxCaret*)handle)->IsOk();
	}

	bool WxOtherFactory::CaretIsVisible(void* handle)
	{
		return ((wxCaret*)handle)->IsVisible();
	}

	void WxOtherFactory::CaretShow(void* handle, bool show)
	{
		((wxCaret*)handle)->Show(show);
	}

	void* WxOtherFactory::CreateDisplay()
	{
		return new wxDisplay();
	}

	void* WxOtherFactory::CreateDisplay2(uint32_t index)
	{
		return new wxDisplay(index);
	}

	void* WxOtherFactory::CreateDisplay3(void* window)
	{
		return new wxDisplay((wxWindow*)window);
	}

	void WxOtherFactory::DeleteDisplay(void* handle)
	{
		delete (wxDisplay*)handle;
	}

	uint32_t WxOtherFactory::DisplayGetCount()
	{
		return wxDisplay::GetCount();
	}

	int WxOtherFactory::DisplayGetFromPoint(const Int32Point& pt)
	{
		return wxDisplay::GetFromPoint(pt);
	}

	int WxOtherFactory::DisplayGetFromWindow(void* win)
	{
		return wxDisplay::GetFromWindow((wxWindow*) win);
	}

	int WxOtherFactory::DisplayGetStdPPIValue()
	{
		return wxDisplay::GetStdPPIValue();
	}

	Int32Size WxOtherFactory::DisplayGetStdPPI()
	{
		return wxDisplay::GetStdPPI();
	}

	string WxOtherFactory::DisplayGetName(void* handle)
	{
		return wxStr(wxEmptyString);
	}

	Int32Size WxOtherFactory::DisplayGetPPI(void* handle)
	{
		return ((wxDisplay*)handle)->GetPPI();
	}

	double WxOtherFactory::DisplayGetScaleFactor(void* handle)
	{
		return ((wxDisplay*)handle)->GetScaleFactor();
	}

	bool WxOtherFactory::DisplayIsPrimary(void* handle)
	{
		return ((wxDisplay*)handle)->IsPrimary();
	}

	Int32Rect WxOtherFactory::DisplayGetClientArea(void* handle)
	{
		return ((wxDisplay*)handle)->GetClientArea();
	}

	Int32Rect WxOtherFactory::DisplayGetGeometry(void* handle)
	{
		return ((wxDisplay*)handle)->GetGeometry();
	}

	bool WxOtherFactory::SystemSettingsHasFeature(int index)
	{
		return wxSystemSettings::HasFeature((wxSystemFeature)index);
	}

	Color WxOtherFactory::SystemSettingsGetColor(int index)
	{
		return wxSystemSettings::GetColour((wxSystemColour)index);
	}

	int WxOtherFactory::SystemSettingsGetMetric(int index, void* win)
	{
		return wxSystemSettings::GetMetric((wxSystemMetric)index, (wxWindow*)win);
	}

	string WxOtherFactory::SystemAppearanceGetName()
	{
		return wxStr(wxSystemSettings::GetAppearance().GetName());
	}

	Font* WxOtherFactory::SystemSettingsGetFont(int index)
	{
		auto wxfont = wxSystemSettings::GetFont((wxSystemFont)index);
		auto _font = new Font();
		_font->SetWxFont(wxfont);
		return _font;
	}

	bool WxOtherFactory::SystemAppearanceIsDark()
	{
		return wxSystemSettings::GetAppearance().IsDark();
	}

	bool WxOtherFactory::SystemAppearanceIsUsingDarkBackground()
	{
		return wxSystemSettings::GetAppearance().IsUsingDarkBackground();
	}

	bool WxOtherFactory::IsBusyCursor()
	{
		return wxIsBusy();
	}

	void WxOtherFactory::BeginBusyCursor()
	{
		wxBeginBusyCursor();
	}

	void WxOtherFactory::EndBusyCursor()
	{
		wxEndBusyCursor();
	}
	
	void WxOtherFactory::Bell()
	{
		wxBell();
	}

	static wxString wxGetTextFromUser2(const wxString& message, const wxString& caption,
		const wxString& defaultValue, wxWindow* parent,
		wxCoord x, wxCoord y, bool centre)
	{
		wxString str;
		long style = wxTextEntryDialogStyle;

		if (centre)
			style |= wxCENTRE;
		else
			style &= ~wxCENTRE;

		wxTextEntryDialog dialog(parent, message, caption, defaultValue, style, wxPoint(x, y));

		auto font = ParkingWindow::GetWindow()->GetFont();
		dialog.SetFont(font);

		if (dialog.ShowModal() == wxID_OK)
		{
			str = dialog.GetValue();
		}
		else
			str = DialogCancelGuid;

		return str;
	}

	long wxGetNumberFromUser2(const wxString& msg,
		const wxString& prompt,
		const wxString& title,
		long value,
		long min,
		long max,
		wxWindow* parent,
		const wxPoint& pos)
	{
		wxNumberEntryDialog dialog(parent, msg, prompt, title,
			value, min, max, pos);
		auto font = ParkingWindow::GetWindow()->GetFont();
		dialog.SetFont(font);
		Application::Log(font.GetNativeFontInfoUserDesc());

		if (dialog.ShowModal() == wxID_OK)
			return dialog.GetValue();

		return -1;
	}

	string WxOtherFactory::GetTextFromUser(const string& message,
		const string& caption, const string& defaultValue, void* parent, int x, int y, bool centre)
	{
		return wxStr(wxGetTextFromUser2(wxStr(message),
			wxStr(caption), wxStr(defaultValue), (wxWindow*)parent, x, y, centre));
	}

	int64_t WxOtherFactory::GetNumberFromUser(const string& message, const string& prompt,
		const string& caption, int64_t value, int64_t min, int64_t max, void* parent,
		const Int32Point& pos)
	{
		return wxGetNumberFromUser2(wxStr(message), wxStr(prompt),
			wxStr(caption), value, min, max, (wxWindow*)parent, pos);
	}

// ============================================

	Int32Size WxOtherFactory::RendererGetExpanderSize(void* renderer, void* win)
	{
		return wxRendererNative::Get().GetExpanderSize((wxWindow*)win);
	}
	
	void WxOtherFactory::RendererDrawPushButton(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawPushButton((wxWindow*)win, *(wxDC*) dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawCollapseButton(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCollapseButton((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	Int32Size WxOtherFactory::RendererGetCollapseButtonSize(void* renderer, void* win, void* dc)
	{
		return wxRendererNative::Get().GetCollapseButtonSize((wxWindow*)win, *(wxDC*)dc);
	}

	void WxOtherFactory::RendererDrawItemSelectionRect(void* renderer, void* win,
		void* dc, const Int32Rect& rect,
		int flags)
	{
		wxRendererNative::Get().DrawItemSelectionRect((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawFocusRect(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawFocusRect((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawChoice(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawChoice((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawComboBox(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawComboBox((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawTextCtrl(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawTextCtrl((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawRadioBitmap(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawRadioBitmap((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawGauge(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int value,
		int max, int flags)
	{
		wxRendererNative::Get().DrawGauge((wxWindow*)win, *(wxDC*)dc, rect, value, max,flags);
	}

	void WxOtherFactory::RendererDrawItemText(void* renderer, void* win, void* dc, const string& text,
		const Int32Rect& rect, int align, int flags, int ellipsizeMode)
	{
		wxRendererNative::Get().DrawItemText((wxWindow*)win, *(wxDC*)dc, wxStr(text), rect, align,
			flags,(wxEllipsizeMode) ellipsizeMode);
	}

	string WxOtherFactory::RendererGetVersion(void* renderer)
	{
		auto version = wxRendererNative::Get().GetVersion();
		auto result = std::to_string(version.version) + "." + std::to_string(version.age);
		return wxStr(result);
	}

	int WxOtherFactory::RendererDrawHeaderButton(void* renderer, void* win, void* dc,
		const Int32Rect& rect,
		int flags, int sortArrow, void* headerButtonParams)
	{
		return wxRendererNative::Get().DrawHeaderButton((wxWindow*)win, *(wxDC*)dc, rect, flags,
			(wxHeaderSortIconType)sortArrow, (wxHeaderButtonParams*) headerButtonParams);
	}

	int WxOtherFactory::RendererDrawHeaderButtonContents(void* renderer, void* win,
		void* dc, const Int32Rect& rect,
		int flags, int sortArrow, void* headerButtonParams)
	{
		return wxRendererNative::Get().DrawHeaderButtonContents((wxWindow*)win, *(wxDC*)dc, rect, flags,
			(wxHeaderSortIconType)sortArrow, (wxHeaderButtonParams*)headerButtonParams);
	}

	int WxOtherFactory::RendererGetHeaderButtonHeight(void* renderer, void* win)
	{
		return wxRendererNative::Get().GetHeaderButtonHeight((wxWindow*)win);
	}

	int WxOtherFactory::RendererGetHeaderButtonMargin(void* renderer, void* win)
	{
		return wxRendererNative::Get().GetHeaderButtonMargin((wxWindow*)win);
	}

	void WxOtherFactory::RendererDrawTreeItemButton(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawTreeItemButton((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawSplitterBorder(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawSplitterBorder((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawSplitterSash(void* renderer, void* win, void* dcReal,
		const Int32Size& sizeReal,
		int position, int orientation, int flags)
	{
		wxRendererNative::Get().DrawSplitterSash((wxWindow*)win, *(wxDC*)dcReal,
			sizeReal, position, (wxOrientation)orientation, flags);
	}

	void WxOtherFactory::RendererDrawComboBoxDropButton(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawComboBoxDropButton((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawDropArrow(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawDropArrow((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawCheckBox(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCheckBox((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	void WxOtherFactory::RendererDrawCheckMark(void* renderer, void* win, void* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCheckMark((wxWindow*)win, *(wxDC*)dc, rect, flags);
	}

	Int32Size WxOtherFactory::RendererGetCheckBoxSize(void* renderer, void* win, int flags)
	{
		return wxRendererNative::Get().GetCheckBoxSize((wxWindow*)win, flags);
	}

	Int32Size WxOtherFactory::RendererGetCheckMarkSize(void* renderer, void* win)
	{
		return wxRendererNative::Get().GetCheckMarkSize((wxWindow*)win);
	}

	// ============================================

	void* WxOtherFactory::MemoryAlloc(uint64_t size)
	{
		const auto sizet = static_cast<size_t>(size);
		return malloc(sizet);
	}

	void* WxOtherFactory::MemoryRealloc(void* memory, uint64_t newSize)
	{
		const auto sizet = static_cast<size_t>(newSize);
		return realloc(memory, sizet);
	}

	void WxOtherFactory::MemoryFree(void* memory)
	{
		free(memory);
	}

	void* WxOtherFactory::MemoryCopy(void* dest, void* src, uint64_t count)
	{
		const auto countt = static_cast<size_t>(count);
		return memcpy(dest, src, countt);
	}

	void* WxOtherFactory::MemoryMove(void* dest, void* src, uint64_t count)
	{
		const auto countt = static_cast<size_t>(count);
		return memmove(dest, src, countt);
	}

	void* WxOtherFactory::MemorySet(void* dest, int fillByte, uint64_t count)
	{
		const auto countt = static_cast<size_t>(count);
		return memset(dest, fillByte, countt);
	}

	// ============================================

}
