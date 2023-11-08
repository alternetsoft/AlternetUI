#include "WxOtherFactory.h"

namespace Alternet::UI
{
	void* WxOtherFactory::CreateRichToolTip(const string& title, const string& message)
	{
		return new wxRichToolTip(wxStr(title), wxStr(message));
	}

	void WxOtherFactory::DeleteRichToolTip(void* handle)
	{
		delete (wxRichToolTip*)handle;
	}

	void WxOtherFactory::RichToolTipSetBkColor(void* handle, const Color& color, const Color& endColor)
	{
		if(endColor.IsEmpty())
			((wxRichToolTip*)handle)->SetBackgroundColour(color);
		else
			((wxRichToolTip*)handle)->SetBackgroundColour(color, endColor);
	}

	void WxOtherFactory::RichToolTipSetIcon(void* handle, ImageSet* bitmapBundle)
	{
		((wxRichToolTip*)handle)->SetIcon(ImageSet::BitmapBundle(bitmapBundle));
	}

	void WxOtherFactory::RichToolTipSetIcon2(void* handle, int icon)
	{
		((wxRichToolTip*)handle)->SetIcon(icon);
	}

	void WxOtherFactory::RichToolTipSetTimeout(void* handle, uint32_t milliseconds, uint32_t millisecondsShowdelay)
	{
		((wxRichToolTip*)handle)->SetTimeout(milliseconds, millisecondsShowdelay);
	}

	void WxOtherFactory::RichToolTipSetTipKind(void* handle, int tipKind)
	{
		((wxRichToolTip*)handle)->SetTipKind((wxTipKind)tipKind);
	}

	void WxOtherFactory::RichToolTipSetTitleFont(void* handle, Font* font)
	{
		if (font == nullptr)
			((wxRichToolTip*)handle)->SetTitleFont(wxNullFont);
		else
			((wxRichToolTip*)handle)->SetTitleFont(font->GetWxFont());
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
			((wxRichToolTip*)handle)->ShowFor((wxWindow*)window);
		else
		{
			auto wxr = new wxRect(rect.X, rect.Y, rect.Width, rect.Height);
			((wxRichToolTip*)handle)->ShowFor((wxWindow*)window, wxr);
			delete wxr;
		}
	}

	void* WxOtherFactory::CreateCursor()
	{
		return new wxCursor();
	}
	
	void* WxOtherFactory::CreateCursor2(int cursorId)
	{
		return new wxCursor((wxStockCursor)cursorId);
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

	bool WxOtherFactory::SystemAppearanceIsDark()
	{
		return wxSystemSettings::GetAppearance().IsDark();
	}

	bool WxOtherFactory::SystemAppearanceIsUsingDarkBackground()
	{
		return wxSystemSettings::GetAppearance().IsUsingDarkBackground();
	}
}
