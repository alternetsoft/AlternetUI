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
        void OnItemCollapsed(wxTreeEvent& event);
        void OnItemExpanded(wxTreeEvent& event);
        void OnItemBeginLabelEdit(wxTreeEvent& event);
        void OnItemEndLabelEdit(wxTreeEvent& event);

    private:
        void ApplyImageList(wxTreeCtrl* value);

        void OnItemLabelEditEvent(wxTreeEvent& event, TreeViewEvent e);

        long GetStyle();

        void RecreateTreeCtrl();

        wxTreeCtrl* GetTreeCtrl();

        TreeViewSelectionMode _selectionMode = TreeViewSelectionMode::Single;
        bool _allowLabelEdit = false;
        ImageList* _imageList = nullptr;

        bool _skipSelectionChangedEvent = false;
        bool _skipExpandedEvent = false;
    };
}
