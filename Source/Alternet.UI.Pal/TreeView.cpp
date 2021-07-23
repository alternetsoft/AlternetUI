#include "TreeView.h"

namespace Alternet::UI
{
    TreeView::TreeView()
    {
        CreateWxWindow();
    }

    TreeView::~TreeView()
    {
        auto window = GetWxWindow();
        if (window != nullptr)
        {
            //window->Unbind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
            //window->Unbind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);
        }
    }

    ImageList* TreeView::GetImageList()
    {
        return _imageList;
    }

    void TreeView::SetImageList(ImageList* value)
    {
        // todo: memory management.
        _imageList = value;
        if (IsWxWindowCreated())
            ApplyImageList(GetTreeCtrl());
    }

    void* TreeView::GetRootItem()
    {
        auto item = GetTreeCtrl()->GetRootItem();
        return item;
    }

    TreeViewSelectionMode TreeView::GetSelectionMode()
    {
        return _selectionMode;
    }

    wxTreeCtrl* TreeView::GetTreeCtrl()
    {
        return dynamic_cast<wxTreeCtrl*>(GetWxWindow());
    }

    std::vector<wxTreeItemId> TreeView::GetSelectedItems()
    {
        return std::vector<wxTreeItemId>();
    }

    void TreeView::SetSelectedItems(const std::vector<wxTreeItemId>& value)
    {
    }

    void TreeView::SetSelectionMode(TreeViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        auto oldSelection = GetSelectedItems();

        _selectionMode = value;
        RecreateWxWindowIfNeeded();

        SetSelectedItems(oldSelection);
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
        wxTreeItemId parentItemId(parentItem);
        return ConvertToIntChecked(GetTreeCtrl()->GetChildrenCount(parentItemId, /*recursively:*/ false));
    }

    void* TreeView::InsertItem(void* parentItem, int index, const string& text, int imageIndex)
    {
        wxTreeItemId parentItemId(parentItem);
        auto item = GetTreeCtrl()->InsertItem(parentItem, index, wxStr(text), imageIndex);
        return item;
    }

    void TreeView::RemoveItem(void* item)
    {
        wxTreeItemId itemId(item);
        GetTreeCtrl()->Delete(itemId);
    }

    void TreeView::ClearItems(void* parentItem)
    {
        wxTreeItemId parentItemId(parentItem);
        GetTreeCtrl()->DeleteChildren(parentItem);
    }

    void TreeView::ClearSelected()
    {
        GetTreeCtrl()->Unselect();
    }

    void TreeView::SetSelected(void* item, bool value)
    {
        wxTreeItemId itemId(item);
        GetTreeCtrl()->SelectItem(itemId, value);
    }

    void TreeView::ApplyImageList(wxTreeCtrl* value)
    {
        value->SetImageList(_imageList == nullptr ? nullptr : _imageList->GetImageList());
    }

    wxWindow* TreeView::CreateWxWindowCore(wxWindow* parent)
    {
        auto value = new wxTreeCtrl(
            parent,
            wxID_ANY,
            wxDefaultPosition,
            wxDefaultSize,
            GetStyle());

        value->AddRoot("");

        ApplyImageList(value);

        //value->Bind(wxEVT_LIST_ITEM_SELECTED, &ListView::OnItemSelected, this);
        //value->Bind(wxEVT_LIST_ITEM_DESELECTED, &ListView::OnItemDeselected, this);

        return value;
    }

    long TreeView::GetStyle()
    {
        return wxTR_TWIST_BUTTONS | wxTR_HAS_BUTTONS | wxTR_HIDE_ROOT | wxTR_LINES_AT_ROOT | wxTR_NO_LINES |
            (_selectionMode == TreeViewSelectionMode::Single ? wxTR_SINGLE : wxTR_MULTIPLE);
    }
}
