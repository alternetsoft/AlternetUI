#include "WxTreeViewFactory.h"

namespace Alternet::UI
{
	WxTreeViewFactory::WxTreeViewFactory()
	{

	}

	WxTreeViewFactory::~WxTreeViewFactory()
	{
	}

	void WxTreeViewFactory::SetItemBold(void* handle, void* item, bool bold)
	{
		wxTreeItemId itemId(item);

		((wxTreeCtrl*)handle)->SetItemBold(itemId, bold);
	}

	Color WxTreeViewFactory::GetItemTextColor(void* handle, void* item)
	{
		return Color();
	}

	Color WxTreeViewFactory::GetItemBackgroundColor(void* handle, void* item)
	{
		return Color();
	}

	void WxTreeViewFactory::SetItemTextColor(void* handle, void* item, const Color& color)
	{

	}

	void WxTreeViewFactory::SetItemBackgroundColor(void* handle, void* item, const Color& color)
	{

	}
}
