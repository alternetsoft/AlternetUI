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
		wxToolTip::SetMaxWidth(width);
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
}
