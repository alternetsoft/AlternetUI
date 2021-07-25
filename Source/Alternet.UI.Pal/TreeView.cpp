#include "TreeView.h"

namespace Alternet::UI
{
    TreeView::TreeView()
    {
        CreateWxWindow();
    }

    TreeView::~TreeView()
    {
        auto window = GetTreeCtrl();
        if (window != nullptr)
        {
            window->Unbind(wxEVT_TREE_SEL_CHANGED, &TreeView::OnSelectionChanged, this);
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
        auto array = OpenSelectedItemsArray();
        int count = GetSelectedItemsItemCount(array);

        std::vector<wxTreeItemId> items(count);
        for (int i = 0; i < count; i++)
            items[i] = GetSelectedItemsItemAt(array, i);

        CloseSelectedItemsArray(array);

        return items;
    }

    void TreeView::SetSelectedItems(const std::vector<wxTreeItemId>& value)
    {
        ClearSelected();
        for (auto index : value)
            SetSelected(index, true);
    }

    TreeView::Item* TreeView::GetItemsSnapshot()
    {
        auto item = new Item();
        GetItemsSnapshot(GetTreeCtrl()->GetRootItem(), item);
        return item;
    }

    void TreeView::GetItemsSnapshot(wxTreeItemId itemId, TreeView::Item* item)
    {
        auto control = GetTreeCtrl();
        if (itemId != control->GetRootItem())
        {
            item->text = control->GetItemText(itemId);
            item->imageIndex = control->GetItemImage(itemId);
        }

        wxTreeItemIdValue cookie;
        auto childId = control->GetFirstChild(itemId, cookie);
        while (childId.IsOk())
        {
            auto childItem = new Item();
            item->items.push_back(childItem);
            GetItemsSnapshot(childId, childItem);

            childId = control->GetNextSibling(childId);
        }
    }

    void TreeView::RestoreItemsSnapshot(TreeView::Item* root)
    {
        auto control = GetTreeCtrl();
        control->Freeze();
        control->DeleteAllItems();
        control->AddRoot("");
        RestoreItemsSnapshot(control->GetRootItem(), root);
        control->Thaw();
    }

    void TreeView::RestoreItemsSnapshot(wxTreeItemId itemId, TreeView::Item* item)
    {
        auto control = GetTreeCtrl();
        for (auto child : item->items)
        {
            auto childId = control->AppendItem(itemId, child->text, child->imageIndex);
            RestoreItemsSnapshot(childId, child);
        }
    }

    void TreeView::SetSelectionMode(TreeViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        //auto oldSelection = GetSelectedItems();

        _selectionMode = value;

        auto snapshot = GetItemsSnapshot();

        RecreateWxWindowIfNeeded();

        RestoreItemsSnapshot(snapshot);
        delete snapshot;
        snapshot = nullptr;


        //SetSelectedItems(oldSelection);
    }

    void* TreeView::OpenSelectedItemsArray()
    {
        auto array = new wxArrayTreeItemIds();
        GetTreeCtrl()->GetSelections(*array);

        return array;
    }

    int TreeView::GetSelectedItemsItemCount(void* array)
    {
        return ((wxArrayTreeItemIds*)array)->GetCount();
    }

    void* TreeView::GetSelectedItemsItemAt(void* array, int index)
    {
        return (*((wxArrayTreeItemIds*)array))[index];
    }

    void TreeView::CloseSelectedItemsArray(void* array)
    {
        delete ((wxArrayTreeItemIds*)array);
    }

    int TreeView::GetItemCount(void* parentItem)
    {
        wxTreeItemId parentItemId(parentItem);
        return ConvertToIntChecked(GetTreeCtrl()->GetChildrenCount(parentItemId, /*recursively:*/ false));
    }

    void* TreeView::InsertItem(void* parentItem, void* insertAfter, const string& text, int imageIndex)
    {
        wxTreeItemId parentItemId(parentItem);
        wxTreeItemId insertAfterId(insertAfter);
        auto item = GetTreeCtrl()->InsertItem(parentItem, insertAfterId, wxStr(text), imageIndex);
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
        GetTreeCtrl()->UnselectAll();
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

        value->Bind(wxEVT_TREE_SEL_CHANGED, &TreeView::OnSelectionChanged, this);

        return value;
    }

    void TreeView::OnSelectionChanged(wxCommandEvent& event)
    {
        RaiseEvent(TreeViewEvent::SelectionChanged);
    }

    long TreeView::GetStyle()
    {
        return wxTR_TWIST_BUTTONS | wxTR_HAS_BUTTONS | wxTR_HIDE_ROOT | wxTR_LINES_AT_ROOT | wxTR_NO_LINES |
            (_selectionMode == TreeViewSelectionMode::Single ? wxTR_SINGLE : wxTR_MULTIPLE);
    }
}
