#include "WxOtherFactory.h"
#include "WxAlternet/wxAlternetRichToolTip.h"

#include <wx/numdlg.h>

namespace Alternet::UI
{

#ifdef __WXGTK__
	bool AlternetRichTooltip = true;
#endif

#ifdef __WXMSW__
	bool AlternetRichTooltip = true;
#endif

#ifdef __WXOSX__
	bool AlternetRichTooltip = true;
#endif

	class wxRichToolTip2
	{
	public:
		wxAlternetRichToolTipImpl* GetAlternetImpl()
		{
			return dynamic_cast<wxAlternetRichToolTipImpl*>(m_impl);
		}

		wxRichToolTip2(const wxString& title, const wxString& message)
		{
			// m_impl = new wxRichToolTipGenericImpl(title, message);
			if(AlternetRichTooltip)
				m_impl = new wxAlternetRichToolTipImpl(title, message);
			else
				m_impl = wxRichToolTipImpl::Create(title, message);
		}

		void SetForegroundColour(const wxColour& col)
		{
			auto impl = GetAlternetImpl();
			if (impl != nullptr)
				impl->SetForegroundColour(col);
		}

		void SetTitleForegroundColour(const wxColour& col)
		{
			auto impl = GetAlternetImpl();
			if (impl != nullptr)
				impl->SetTitleForegroundColour(col);
		}

		// Set the background colour: if two colours are specified, the background
		// is drawn using a gradient from top to bottom, otherwise a single solid
		// colour is used.
		void SetBackgroundColour(const wxColour& col,
			const wxColour& colEnd = wxColour());

		// Set the small icon to show: either one of the standard information/
		// warning/error ones (the question icon doesn't make sense for a tooltip)
		// or a custom icon.
		void SetIcon(int icon = wxICON_INFORMATION);
		void SetIcon(const wxBitmapBundle& icon);

		// Set timeout after which the tooltip should disappear, in milliseconds.
		// By default the tooltip is hidden after system-dependent interval of time
		// elapses but this method can be used to change this or also disable
		// hiding the tooltip automatically entirely by passing 0 in this parameter
		// (but doing this can result in native version not being used).
		// Optionally specify a show delay.
		void SetTimeout(unsigned milliseconds, unsigned millisecondsShowdelay = 0);

		// Choose the tip kind, possibly none. By default the tip is positioned
		// automatically, as if wxTipKind_Auto was used.
		void SetTipKind(wxTipKind tipKind);

		// Set the title text font. By default it's emphasized using the font style
		// or colour appropriate for the current platform.
		void SetTitleFont(const wxFont& font);

		// Show the tooltip for the given window and optionally a specified area.
		void ShowFor(wxWindow* win, const wxRect* rect = NULL);

		// Non-virtual dtor as this class is not supposed to be derived from.
		~wxRichToolTip2();

	private:
		wxRichToolTipImpl* m_impl;

		wxDECLARE_NO_COPY_CLASS(wxRichToolTip2);
	};

	void wxRichToolTip2::SetBackgroundColour(const wxColour& col, const wxColour& colEnd)
	{
		m_impl->SetBackgroundColour(col, colEnd);
	}

	void wxRichToolTip2::SetIcon(int icon)
	{
		m_impl->SetStandardIcon(icon);
	}

	void wxRichToolTip2::SetIcon(const wxBitmapBundle& icon)
	{
		m_impl->SetCustomIcon(icon);
	}

	void wxRichToolTip2::SetTimeout(unsigned milliseconds,
		unsigned millisecondsDelay)
	{
		m_impl->SetTimeout(milliseconds, millisecondsDelay);
	}

	void wxRichToolTip2::SetTipKind(wxTipKind tipKind)
	{
		m_impl->SetTipKind(tipKind);
	}

	void wxRichToolTip2::SetTitleFont(const wxFont& font)
	{
		m_impl->SetTitleFont(font);
	}

	void wxRichToolTip2::ShowFor(wxWindow* win, const wxRect* rect)
	{
		wxCHECK_RET(win, wxS("Must have a valid window"));

		m_impl->ShowFor(win, rect);
	}

	wxRichToolTip2::~wxRichToolTip2()
	{
		delete m_impl;
	}
	
	bool WxOtherFactory::GetRichToolTipUseGeneric()
	{
		return AlternetRichTooltip;
	}
	
	void WxOtherFactory::SetRichToolTipUseGeneric(bool value)
	{
		AlternetRichTooltip = value;
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

		if (dialog.ShowModal() == wxID_OK)
		{
			str = dialog.GetValue();
		}
		else
			str = DialogCancelGuid;

		return str;
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
		return wxGetNumberFromUser(wxStr(message), wxStr(prompt),
			wxStr(caption), value, min, max, (wxWindow*)parent, pos);
	}
}
