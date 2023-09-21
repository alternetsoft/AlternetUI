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
		return 0;
	}

	void WxStatusBarFactory::SetStatusText(void* handle, const string& text, int number)
	{
	}

	string WxStatusBarFactory::GetStatusText(void* handle, int number)
	{
		return wxStr(wxEmptyString);
	}

	void WxStatusBarFactory::PushStatusText(void* handle, const string& text, int number)
	{
	}

	void WxStatusBarFactory::PopStatusText(void* handle, int number)
	{
	}

	void WxStatusBarFactory::SetStatusWidths(void* handle, int n, int* widths, int widthsCount)
	{
	}

	void WxStatusBarFactory::SetFieldsCount(void* handle, int number, int* widths, int widthsCount)
	{
	}

	int WxStatusBarFactory::GetStatusWidth(void* handle, int n)
	{
		return 0;
	}

	int WxStatusBarFactory::GetStatusStyle(void* handle, int n)
	{
		return 0;
	}

	void WxStatusBarFactory::SetStatusStyles(void* handle, int n, int* styles, int stylesCount)
	{
	}

	Int32Rect WxStatusBarFactory::GetFieldRect(void* handle, int i)
	{
		return Int32Rect();
	}

	void WxStatusBarFactory::SetMinHeight(void* handle, int height)
	{
	}

	int WxStatusBarFactory::GetBorderX(void* handle)
	{
		return 0;
	}

	int WxStatusBarFactory::GetBorderY(void* handle)
	{
		return 0;
	}
}
