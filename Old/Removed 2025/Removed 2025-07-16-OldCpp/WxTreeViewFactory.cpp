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
		wxTreeItemId itemId(item);
		return ((wxTreeCtrl*)handle)->GetItemTextColour(itemId);
	}

	Color WxTreeViewFactory::GetItemBackgroundColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		return ((wxTreeCtrl*)handle)->GetItemBackgroundColour(itemId);
	}

	void WxTreeViewFactory::SetItemTextColor(void* handle, void* item, const Color& color)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemTextColour(itemId, color);
	}

	void WxTreeViewFactory::SetItemBackgroundColor(void* handle, void* item, const Color& color)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemBackgroundColour(itemId, color);
	}

	void WxTreeViewFactory::ResetItemTextColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemTextColour(itemId, wxColour());
	}
	
	void WxTreeViewFactory::ResetItemBackgroundColor(void* handle, void* item)
	{
		wxTreeItemId itemId(item);
		((wxTreeCtrl*)handle)->SetItemBackgroundColour(itemId, wxColour());
	}
}
