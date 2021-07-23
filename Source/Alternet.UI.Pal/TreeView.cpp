#include "TreeView.h"

namespace Alternet::UI
{
    TreeView::TreeView()
    {
    }
    TreeView::~TreeView()
    {
    }

    ImageList* TreeView::GetImageList()
    {
        return nullptr;
    }
    void TreeView::SetImageList(ImageList* value)
    {
    }
    void* TreeView::GetRootItem()
    {
        return nullptr;
    }
    TreeViewSelectionMode TreeView::GetSelectionMode()
    {
        return TreeViewSelectionMode();
    }
    void TreeView::SetSelectionMode(TreeViewSelectionMode value)
    {
    }
    void* TreeView::OpenSelectedItemsArray()
    {
        return nullptr;
    }
    int TreeView::GetSelectedItemsItemCount(void* array)
    {
        return 0;
    }
    void* TreeView::GetSelectedItemsItemAt(void* array, int index)
    {
        return nullptr;
    }
    void TreeView::CloseSelectedItemsArray(void* array)
    {
    }
    int TreeView::GetItemCount(void* parentItem)
    {
        return 0;
    }
    void TreeView::InsertItemAt(void* parentItem, int index, const string& text, int imageIndex)
    {
    }
    void TreeView::RemoveItem(void* item)
    {
    }
    void TreeView::ClearItems(void* parentItem)
    {
    }
    void TreeView::ClearSelected()
    {
    }
    void TreeView::SetSelected(void* item, bool value)
    {
    }
    wxWindow* TreeView::CreateWxWindowCore(wxWindow* parent)
    {
        return nullptr;
    }
}
