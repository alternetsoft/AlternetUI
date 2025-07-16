#include "WxOtherFactory.h"
#include "Application.h"
#include "Api/InputStream.h"
#include "Api/OutputStream.h"
#include "ManagedInputStream.h"
#include "ManagedOutputStream.h"

#include <wx/numdlg.h>
#include <string.h>
#include <wx/sound.h>
#include <wx/uiaction.h>

namespace Alternet::UI
{
	//----------------------------------------------------------------------------
	// SimpleTransientPopup
	//----------------------------------------------------------------------------
	class SimpleTransientPopup : public wxPopupTransientWindow
	{
	public:
		SimpleTransientPopup(wxWindow* parent, bool scrolled);
		virtual ~SimpleTransientPopup();

		// wxPopupTransientWindow virtual methods are all overridden to log them
		virtual void Popup(wxWindow* focus = NULL) wxOVERRIDE;
		virtual void OnDismiss() wxOVERRIDE;
		virtual bool ProcessLeftDown(wxMouseEvent& event) wxOVERRIDE;
		virtual bool Show(bool show = true) wxOVERRIDE;

	private:
		wxScrolledWindow* m_panel;
		wxButton* m_button;
		wxSpinCtrl* m_spinCtrl;
		wxStaticText* m_mouseText;

	private:
		void OnMouse(wxMouseEvent& event);
		void OnSize(wxSizeEvent& event);
		void OnSetFocus(wxFocusEvent& event);
		void OnKillFocus(wxFocusEvent& event);
		void OnButton(wxCommandEvent& event);
		void OnSpinCtrl(wxSpinEvent& event);

	private:
		wxDECLARE_ABSTRACT_CLASS(SimpleTransientPopup);
		wxDECLARE_EVENT_TABLE();
	};

	// IDs for the controls and the menu commands
	enum
	{
		Minimal_Quit = wxID_EXIT,
		Minimal_About = wxID_ABOUT,
		Minimal_TestDialog,
		Minimal_StartSimplePopup,
		Minimal_StartScrolledPopup,
		Minimal_LogWindow,
		Minimal_PopupButton,
		Minimal_PopupSpinctrl
	};

	SimpleTransientPopup::SimpleTransientPopup(wxWindow* parent, bool scrolled)
		:wxPopupTransientWindow(parent,
			wxBORDER_NONE |
			wxPU_CONTAINS_CONTROLS)
	{
		m_panel = new wxScrolledWindow(this, wxID_ANY);
		m_panel->SetBackgroundColour(*wxLIGHT_GREY);

		// Keep this code to verify if mouse events work, they're required if
		// you're making a control like a combobox where the items are highlighted
		// under the cursor, the m_panel is set focus in the Popup() function
		m_panel->Bind(wxEVT_MOTION, &SimpleTransientPopup::OnMouse, this);

		wxStaticText* text = new wxStaticText(m_panel, wxID_ANY,
			"wxPopupTransientWindow is a\n"
			"wxPopupWindow which disappears\n"
			"automatically when the user\n"
			"clicks the mouse outside it or if it\n"
			"(or its first child) loses focus in \n"
			"any other way.");

		m_button = new wxButton(m_panel, Minimal_PopupButton, "Press Me");
		m_spinCtrl = new wxSpinCtrl(m_panel, Minimal_PopupSpinctrl, "Hello");
		m_mouseText = new wxStaticText(m_panel, wxID_ANY,
			"<- Test Mouse ->");

		wxBoxSizer* topSizer = new wxBoxSizer(wxVERTICAL);
		topSizer->Add(text, 0, wxALL, 5);
		topSizer->Add(m_button, 0, wxALL, 5);
		topSizer->Add(m_spinCtrl, 0, wxALL, 5);
		topSizer->Add(new wxTextCtrl(m_panel, wxID_ANY, "Try to type here"),
			0, wxEXPAND | wxALL, 5);
		topSizer->Add(m_mouseText, 0, wxCENTRE | wxALL, 5);

		if (scrolled)
		{
			// Add a big window to ensure that scrollbars are shown when we set the
			// panel size to a lesser size below.
			topSizer->Add(new wxPanel(m_panel, wxID_ANY, wxDefaultPosition,
				wxSize(600, 900)));
		}

		m_panel->SetSizer(topSizer);
		if (scrolled)
		{
			// Set the fixed size to ensure that the scrollbars are shown.
			m_panel->SetSize(300, 300);

			// And also actually enable them.
			m_panel->SetScrollRate(10, 10);
		}
		else
		{
			// Use the fitting size for the panel if we don't need scrollbars.
			topSizer->Fit(m_panel);
		}

		SetClientSize(m_panel->GetSize());
	}

	SimpleTransientPopup::~SimpleTransientPopup()
	{
	}

	void SimpleTransientPopup::Popup(wxWindow* WXUNUSED(focus))
	{
		/*wxLogMessage("%p SimpleTransientPopup::Popup", this);*/
		wxPopupTransientWindow::Popup();
	}

	void SimpleTransientPopup::OnDismiss()
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnDismiss", this);*/
		wxPopupTransientWindow::OnDismiss();
	}

	bool SimpleTransientPopup::ProcessLeftDown(wxMouseEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::ProcessLeftDown pos(%d, %d)", this,
			event.GetX(), event.GetY());*/
		return wxPopupTransientWindow::ProcessLeftDown(event);
	}
	bool SimpleTransientPopup::Show(bool show)
	{
		/*wxLogMessage("%p SimpleTransientPopup::Show %d", this, int(show));*/
		return wxPopupTransientWindow::Show(show);
	}

	void SimpleTransientPopup::OnSize(wxSizeEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnSize", this);*/
		event.Skip();
	}

	void SimpleTransientPopup::OnSetFocus(wxFocusEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnSetFocus", this);*/
		event.Skip();
	}

	void SimpleTransientPopup::OnKillFocus(wxFocusEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnKillFocus", this);*/
		event.Skip();
	}

	void SimpleTransientPopup::OnMouse(wxMouseEvent& event)
	{
		wxRect rect(m_mouseText->GetRect());
		rect.SetX(-100000);
		rect.SetWidth(1000000);
		wxColour colour(*wxLIGHT_GREY);

		if (rect.Contains(event.GetPosition()))
		{
			colour = wxSystemSettings::GetColour(wxSYS_COLOUR_HIGHLIGHT);
			/*wxLogMessage("%p SimpleTransientPopup::OnMouse pos(%d, %d)",
				event.GetEventObject(), event.GetX(), event.GetY());*/
		}

		if (colour != m_mouseText->GetBackgroundColour())
		{
			m_mouseText->SetBackgroundColour(colour);
			m_mouseText->Refresh();
		}
		event.Skip();
	}

	void SimpleTransientPopup::OnButton(wxCommandEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnButton ID %d", this, event.GetId());*/

		wxButton* button = wxDynamicCast(event.GetEventObject(), wxButton);
		if (button->GetLabel() == "Press Me")
			button->SetLabel("Pressed");
		else
			button->SetLabel("Press Me");

		event.Skip();
	}

	void SimpleTransientPopup::OnSpinCtrl(wxSpinEvent& event)
	{
		/*wxLogMessage("%p SimpleTransientPopup::OnSpinCtrl ID %d Value %d",
			this, event.GetId(), event.GetInt());*/
		event.Skip();
	}

	//----------------------------------------------------------------------------
	// SimpleTransientPopup
	//----------------------------------------------------------------------------
	wxIMPLEMENT_CLASS(SimpleTransientPopup, wxPopupTransientWindow);

	wxBEGIN_EVENT_TABLE(SimpleTransientPopup, wxPopupTransientWindow)
		EVT_MOUSE_EVENTS(SimpleTransientPopup::OnMouse)
		EVT_SIZE(SimpleTransientPopup::OnSize)
		EVT_SET_FOCUS(SimpleTransientPopup::OnSetFocus)
		EVT_KILL_FOCUS(SimpleTransientPopup::OnKillFocus)
		EVT_BUTTON(Minimal_PopupButton, SimpleTransientPopup::OnButton)
		EVT_SPINCTRL(Minimal_PopupSpinctrl, SimpleTransientPopup::OnSpinCtrl)
		wxEND_EVENT_TABLE()

//=============================================

	void WxOtherFactory::TestPopupWindow(void* parent, const Int32Point& pos, const Int32Size& sz)
	{
		static SimpleTransientPopup* m_simplePopup;

		delete m_simplePopup;
		m_simplePopup = new SimpleTransientPopup((wxWindow*)parent, false);
		//wxPoint pos = btn->ClientToScreen(wxPoint(0, 0));
		//wxSize sz = btn->GetSize();
		m_simplePopup->Position(pos, sz);
		/*wxLogMessage("%p Dialog Simple Popup Shown pos(%d, %d) size(%d, %d)",
			m_simplePopup, pos.X, pos.Y, sz.Width, sz.Height);*/
		m_simplePopup->Popup();
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
		if (type == -1)
			type = (int)wxCURSOR_DEFAULT_TYPE;
		return new wxCursor(wxStr(cursorName), (wxBitmapType)type, hotSpotX, hotSpotY);
	}

	void* WxOtherFactory::CreateCursor4(Image* image, int hotSpotX, int hotSpotY)
	{
		if (image == nullptr)
			return new wxCursor();
		auto img = image->GetBitmap().ConvertToImage();

		if (hotSpotX != 0)
			img.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_X, hotSpotX);
		if (hotSpotY != 0)
			img.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_Y, hotSpotY);
		return new wxCursor(img);
	}

	void* WxOtherFactory::CreateCursor5(void* image, int hotSpotX, int hotSpotY)
	{
		if (image == nullptr)
			return new wxCursor();

		auto img = ((GenericImage*)image)->_image;

		if (hotSpotX != 0)
			img.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_X, hotSpotX);
		if (hotSpotY != 0)
			img.SetOption(wxIMAGE_OPTION_CUR_HOTSPOT_Y, hotSpotY);
		return new wxCursor(img);
	}

	void WxOtherFactory::DeleteCursor(void* handle)
	{
		delete (wxCursor*)handle;
	}

	void WxOtherFactory::SetCursor(void* handle)
	{
		if(handle == nullptr)
			wxSetCursor(wxNullCursor);
		else
			wxSetCursor(*(wxCursor*)handle);
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
		return wxStr(((wxDisplay*)handle)->GetName());
	}

	Int32Size WxOtherFactory::DisplayGetPPI(void* handle)
	{
		return ((wxDisplay*)handle)->GetPPI();
	}

	double WxOtherFactory::DisplayGetScaleFactor(void* handle)
	{
		return ((wxDisplay*)handle)->GetScaleFactor();
	}

	bool WxOtherFactory::DisplayIsOk(void* handle)
	{
		return ((wxDisplay*)handle)->IsOk();
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

	class wxTextEntryDialog2 : public wxTextEntryDialog
	{
	public:
		wxTextEntryDialog2(wxWindow* parent,
			const wxString& message,
			const wxString& caption = wxASCII_STR(wxGetTextFromUserPromptStr),
			const wxString& value = wxEmptyString,
			long style = wxTextEntryDialogStyle,
			const wxPoint& pos = wxDefaultPosition)
		{
			if (Window::fontOverride.IsOk())
				SetFont(Window::fontOverride);
			Create(parent, message, caption, value, style, pos);
		}
	};

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

		wxTextEntryDialog2 dialog(parent, message, caption, defaultValue, style, wxPoint(x, y));

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

	class wxNumberEntryDialog2 : public wxNumberEntryDialog
	{
	public:
		wxNumberEntryDialog2(wxWindow* parent,
			const wxString& message,
			const wxString& prompt,
			const wxString& caption,
			long value, long min, long max,
			const wxPoint& pos = wxDefaultPosition)
		{
			if(Window::fontOverride.IsOk())
				SetFont(Window::fontOverride);
			wxNumberEntryDialog::Create(parent, message, prompt, caption, value, min, max, pos);
		}
	};

	long wxGetNumberFromUser2(const wxString& msg,
		const wxString& prompt,
		const wxString& title,
		long value,
		long min,
		long max,
		wxWindow* parent,
		const wxPoint& pos)
	{
		wxNumberEntryDialog2 dialog(parent, msg, prompt, title,
			value, min, max, pos);
/*
		auto font = ParkingWindow::GetWindow()->GetFont();
		dialog.SetFont(font);
*/
		/*Application::Log(dialog.GetFont().GetNativeFontInfoUserDesc());*/

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
	
	void WxOtherFactory::RendererDrawPushButton(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawPushButton((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawCollapseButton(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCollapseButton((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	Int32Size WxOtherFactory::RendererGetCollapseButtonSize(void* renderer, void* win, DrawingContext* dc)
	{
		return wxRendererNative::Get().GetCollapseButtonSize((wxWindow*)win, *dc->GetDC());
	}

	void WxOtherFactory::RendererDrawItemSelectionRect(void* renderer, void* win,
		DrawingContext* dc, const Int32Rect& rect,
		int flags)
	{
		wxRendererNative::Get().DrawItemSelectionRect((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawFocusRect(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawFocusRect((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawChoice(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawChoice((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawComboBox(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawComboBox((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawTextCtrl(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawTextCtrl((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawRadioBitmap(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawRadioBitmap((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawGauge(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int value,
		int max, int flags)
	{
		wxRendererNative::Get().DrawGauge((wxWindow*)win, *dc->GetDC(), rect, value, max,flags);
	}

	void WxOtherFactory::RendererDrawItemText(void* renderer, void* win, DrawingContext* dc, const string& text,
		const Int32Rect& rect, int align, int flags, int ellipsizeMode)
	{
		wxRendererNative::Get().DrawItemText((wxWindow*)win, *dc->GetDC(), wxStr(text), rect, align,
			flags,(wxEllipsizeMode) ellipsizeMode);
	}

	string WxOtherFactory::RendererGetVersion(void* renderer)
	{
		auto version = wxRendererNative::Get().GetVersion();
		auto result = std::to_string(version.version) + "." + std::to_string(version.age);
		return wxStr(result);
	}

	int WxOtherFactory::RendererDrawHeaderButton(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect,
		int flags, int sortArrow, void* headerButtonParams)
	{
		return wxRendererNative::Get().DrawHeaderButton((wxWindow*)win, *dc->GetDC(), rect, flags,
			(wxHeaderSortIconType)sortArrow, (wxHeaderButtonParams*) headerButtonParams);
	}

	int WxOtherFactory::RendererDrawHeaderButtonContents(void* renderer, void* win,
		DrawingContext* dc, const Int32Rect& rect,
		int flags, int sortArrow, void* headerButtonParams)
	{
		return wxRendererNative::Get().DrawHeaderButtonContents((wxWindow*)win, *dc->GetDC(), rect, flags,
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

	void WxOtherFactory::RendererDrawTreeItemButton(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawTreeItemButton((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawSplitterBorder(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawSplitterBorder((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawSplitterSash(void* renderer, void* win, DrawingContext* dcReal,
		const Int32Size& sizeReal,
		int position, int orientation, int flags)
	{
		wxRendererNative::Get().DrawSplitterSash((wxWindow*)win, *dcReal->GetDC(),
			sizeReal, position, (wxOrientation)orientation, flags);
	}

	void WxOtherFactory::RendererDrawComboBoxDropButton(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawComboBoxDropButton((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawDropArrow(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawDropArrow((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawCheckBox(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCheckBox((wxWindow*)win, *dc->GetDC(), rect, flags);
	}

	void WxOtherFactory::RendererDrawCheckMark(void* renderer, void* win, DrawingContext* dc,
		const Int32Rect& rect, int flags)
	{
		wxRendererNative::Get().DrawCheckMark((wxWindow*)win, *dc->GetDC(), rect, flags);
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

	void* WxOtherFactory::FsWatcherCreate()
	{
		return new wxFileSystemWatcher();
	}

	void WxOtherFactory::FsWatcherDelete(void* handle)
	{
		delete (wxFileSystemWatcher*)handle;
	}

	bool WxOtherFactory::FsWatcherAdd(void* handle, const string& path, int events)
	{
		return ((wxFileSystemWatcher*)handle)->Add(wxStr(path), events);
	}

	bool WxOtherFactory::FsWatcherAddTree(
		void* handle, const string& path, int events, const string& filter)
	{
		return ((wxFileSystemWatcher*)handle)->AddTree(wxStr(path), events, wxStr(filter));
	}

	int WxOtherFactory::FsWatcherGetWatchedPathsCount(void* handle)
	{
		return ((wxFileSystemWatcher*)handle)->GetWatchedPathsCount();
	}
	
	bool WxOtherFactory::FsWatcherRemove(void* handle, const string& path)
	{
		return ((wxFileSystemWatcher*)handle)->Remove(wxStr(path));
	}

	bool WxOtherFactory::FsWatcherRemoveAll(void* handle)
	{
		return ((wxFileSystemWatcher*)handle)->RemoveAll();
	}

	bool WxOtherFactory::FsWatcherRemoveTree(void* handle, const string& path)
	{
		return ((wxFileSystemWatcher*)handle)->RemoveTree(wxStr(path));
	}

	void WxOtherFactory::FsWatcherSetOwner(void* handle, void* handler)
	{
		((wxFileSystemWatcher*)handle)->SetOwner((wxEvtHandler*)handler);
	}

	// ============================================

	bool WxOtherFactory::SoundIsOk(void* handle)
	{
		return ((wxSound*)handle)->IsOk();
	}

	void* WxOtherFactory::SoundCreate()
	{
		return new wxSound();
	}
	
	void* WxOtherFactory::SoundCreate2(const string& fileName, bool isResource)
	{
		return new wxSound(wxStr(fileName), isResource);
	}

	void* WxOtherFactory::SoundCreate4(uint64_t size, void* data)
	{
		return new wxSound(size, data);
	}

	void WxOtherFactory::SoundDelete(void* handle)
	{
		delete (wxSound*)handle;
	}

	bool WxOtherFactory::SoundPlay2(const string& filename, uint32_t flags)
	{
		return wxSound::Play(wxStr(filename), flags);
	}

	bool WxOtherFactory::SoundPlay(void* handle, uint32_t flags)
	{
		return ((wxSound*)handle)->Play(flags);
	}

	void WxOtherFactory::SoundStop()
	{
		wxSound::Stop();
	}

	// ============================================

	void WxOtherFactory::UIActionSimulatorDelete(void* handle)
	{
		delete (wxUIActionSimulator*)handle;
	}

	void* WxOtherFactory::UIActionSimulatorCreate()
	{
		return new wxUIActionSimulator();
	}

	bool WxOtherFactory::UIActionSimulatorChar(void* handle, int keycode, int modifiers)
	{
		return ((wxUIActionSimulator*)handle)->Char(keycode, modifiers);
	}

	bool WxOtherFactory::UIActionSimulatorKeyDown(void* handle, int keycode, int modifiers)
	{
		return ((wxUIActionSimulator*)handle)->KeyDown(keycode, modifiers);
	}

	void WxOtherFactory::UIActionSimulatorYield()
	{
		wxYield();
	}

	bool WxOtherFactory::UIActionSimulatorKeyUp(void* handle, int keycode, int modifiers)
	{
		return ((wxUIActionSimulator*)handle)->KeyUp(keycode, modifiers);
	}

	bool WxOtherFactory::UIActionSimulatorMouseClick(void* handle, int button)
	{
		return ((wxUIActionSimulator*)handle)->MouseClick(button);
	}

	bool WxOtherFactory::UIActionSimulatorMouseDblClick(void* handle, int button)
	{
		return ((wxUIActionSimulator*)handle)->MouseDblClick(button);
	}

	bool WxOtherFactory::UIActionSimulatorMouseDown(void* handle, int button)
	{
		return ((wxUIActionSimulator*)handle)->MouseDown(button);
	}

	bool WxOtherFactory::UIActionSimulatorMouseDragDrop(void* handle, int64_t x1,
		int64_t y1, int64_t x2, int64_t y2, int button)
	{
		return ((wxUIActionSimulator*)handle)->MouseDragDrop(x1, y1, x2, y2, button);
	}

	bool WxOtherFactory::UIActionSimulatorMouseMove(void* handle, const PointI& point)
	{
		return ((wxUIActionSimulator*)handle)->MouseMove(point);
	}

	bool WxOtherFactory::UIActionSimulatorMouseUp(void* handle, int button)
	{
		return ((wxUIActionSimulator*)handle)->MouseUp(button);
	}

	bool WxOtherFactory::UIActionSimulatorSelect(void* handle, const string& text)
	{
		return ((wxUIActionSimulator*)handle)->Select(wxStr(text));
	}

	bool WxOtherFactory::UIActionSimulatorText(void* handle, const string& text)
	{
		return ((wxUIActionSimulator*)handle)->Text(wxStr(text));
	}

	// ============================================
}
