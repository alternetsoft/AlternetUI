#pragma once
#include "Common.h"
#include "Control.h"
#include "ImageList.h"

namespace Alternet::UI
{
    class TreeView : public Control
    {
#include "Api/TreeView.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void OnSelectionChanged(wxCommandEvent& event);

    private:
        void ApplyImageList(wxTreeCtrl* value);

        long GetStyle();

        wxTreeCtrl* GetTreeCtrl();

        TreeViewSelectionMode _selectionMode = TreeViewSelectionMode::Single;
        ImageList* _imageList = nullptr;

        std::vector<wxTreeItemId> GetSelectedItems();
        void SetSelectedItems(const std::vector<wxTreeItemId>& value);

        struct Item
        {
            ~Item()
            {
                for (auto i : items)
                    delete i;
                items.clear();
            }

            wxString text;
            int imageIndex = -1;
            std::vector<Item*> items;
        };

        Item* GetItemsSnapshot();
        void GetItemsSnapshot(wxTreeItemId itemId, TreeView::Item* item);

        void RestoreItemsSnapshot(Item* root);
        void RestoreItemsSnapshot(wxTreeItemId itemId, TreeView::Item* item);
    };
}
