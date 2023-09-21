#include "WxStatusBarFactory.h"

namespace Alternet::UI
{
	WxStatusBarFactory::WxStatusBarFactory()
	{

	}

	WxStatusBarFactory::~WxStatusBarFactory()
	{
	}

	int WxStatusBarFactory::GetFieldsCount(void* handle)
	{
		return ((wxStatusBar*)handle)->GetFieldsCount();
	}

	void WxStatusBarFactory::SetStatusText(void* handle, const string& text, int number)
	{
		((wxStatusBar*)handle)->SetStatusText(wxStr(text), number);
	}

	string WxStatusBarFactory::GetStatusText(void* handle, int number)
	{
		return wxStr(((wxStatusBar*)handle)->GetStatusText(number));
	}

	void WxStatusBarFactory::PushStatusText(void* handle, const string& text, int number)
	{
		((wxStatusBar*)handle)->PushStatusText(wxStr(text), number);
	}

	void WxStatusBarFactory::PopStatusText(void* handle, int number)
	{
		((wxStatusBar*)handle)->PopStatusText(number);
	}

	int WxStatusBarFactory::GetStatusWidth(void* handle, int n)
	{
		return ((wxStatusBar*)handle)->GetStatusWidth(n);
	}

	int WxStatusBarFactory::GetStatusStyle(void* handle, int n)
	{
		return ((wxStatusBar*)handle)->GetStatusStyle(n);
	}

	void WxStatusBarFactory::SetStatusWidths(void* handle, int* widths, int widthsCount)
	{
		((wxStatusBar*)handle)->SetStatusWidths(widthsCount, widths);
	}

	void WxStatusBarFactory::SetStatusStyles(void* handle, int* styles, int stylesCount)
	{
		((wxStatusBar*)handle)->SetStatusStyles(stylesCount, styles);
	}

	void WxStatusBarFactory::SetFieldsCount(void* handle, int number)
	{
		auto sb = (wxStatusBar*)handle;

		auto panesCount = sb->GetFieldsCount();

		if (panesCount != number)
		{
			auto newCount = number;
			if (newCount < 1)
				newCount = 1;
			sb->SetFieldsCount(newCount);
		}
	}

	Int32Rect WxStatusBarFactory::GetFieldRect(void* handle, int i)
	{
		wxRect rect;
		auto result = ((wxStatusBar*)handle)->GetFieldRect(i, rect);
		if (result)
			return rect;
		else
			return Int32Rect();
	}

	void WxStatusBarFactory::SetMinHeight(void* handle, int height)
	{
		((wxStatusBar*)handle)->SetMinHeight(height);
	}

	int WxStatusBarFactory::GetBorderX(void* handle)
	{
		return ((wxStatusBar*)handle)->GetBorderX();
	}

	int WxStatusBarFactory::GetBorderY(void* handle)
	{
		return ((wxStatusBar*)handle)->GetBorderY();
	}
}
