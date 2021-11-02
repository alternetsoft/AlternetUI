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
            window->Unbind(wxEVT_TREE_ITEM_EXPANDED, &TreeView::OnItemExpanded, this);
            window->Unbind(wxEVT_TREE_ITEM_COLLAPSED, &TreeView::OnItemCollapsed, this);
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

    void TreeView::SetSelectionMode(TreeViewSelectionMode value)
    {
        if (_selectionMode == value)
            return;

        _selectionMode = value;
        _skipSelectionChangedEvent = true;
        _skipExpandedEvent = true;
        RecreateWxWindowIfNeeded();
        
        RaiseEvent(TreeViewEvent::ControlRecreated);

        _skipSelectionChangedEvent = false;
        _skipExpandedEvent = false;
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

    void* TreeView::InsertItem(void* parentItem, void* insertAfter, const string& text, int imageIndex, bool parentIsExpanded)
    {
        wxTreeItemId parentItemId(parentItem);
        wxTreeItemId insertAfterId(insertAfter);
        auto control = GetTreeCtrl();
        auto item = control->InsertItem(parentItem, insertAfterId, wxStr(text), imageIndex);

        if (parentItemId != control->GetRootItem())
        {
            if (parentIsExpanded)
                control->Expand(parentItemId);
            else
                control->Collapse(parentItemId);
        }

        return item;
    }

    void TreeView::RemoveItem(void* item)
    {
        wxTreeItemId itemId(item);
        auto control = GetTreeCtrl();
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
        value->Bind(wxEVT_TREE_ITEM_EXPANDED, &TreeView::OnItemExpanded, this);
        value->Bind(wxEVT_TREE_ITEM_COLLAPSED, &TreeView::OnItemCollapsed, this);

        return value;
    }

    void TreeView::OnSelectionChanged(wxCommandEvent& event)
    {
        auto window = dynamic_cast<wxWindow*>(event.GetEventObject());
        if (window->IsBeingDeleted())
            return;

        if (!_skipSelectionChangedEvent)
            RaiseEvent(TreeViewEvent::SelectionChanged);
    }

    void TreeView::OnItemCollapsed(wxTreeEvent& event)
    {
        TreeViewItemEventData data { event.GetItem() };
        RaiseEvent(TreeViewEvent::ItemCollapsed, &data);
    }

    void TreeView::OnItemExpanded(wxTreeEvent& event)
    {
        if (_skipExpandedEvent)
            return;

        TreeViewItemEventData data{ event.GetItem() };
        RaiseEvent(TreeViewEvent::ItemExpanded, &data);
    }

    long TreeView::GetStyle()
    {
        return wxTR_TWIST_BUTTONS | wxTR_HAS_BUTTONS | wxTR_HIDE_ROOT | wxTR_LINES_AT_ROOT | wxTR_NO_LINES |
            (_selectionMode == TreeViewSelectionMode::Single ? wxTR_SINGLE : wxTR_MULTIPLE);
    }
}
