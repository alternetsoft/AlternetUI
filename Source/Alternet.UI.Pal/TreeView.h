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
        void OnItemCollapsing(wxTreeEvent& event);
        void OnItemExpanding(wxTreeEvent& event);
        void OnItemBeginLabelEdit(wxTreeEvent& event);
        void OnItemEndLabelEdit(wxTreeEvent& event);

    private:
        void ApplyImageList(wxTreeCtrl* value);

        void OnItemLabelEditEvent(wxTreeEvent& event, TreeViewEvent e);

        long GetStyle();

        virtual void RecreateWxWindowIfNeeded() override;

        wxTreeCtrl* GetTreeCtrl();

        TreeViewSelectionMode _selectionMode = TreeViewSelectionMode::Single;

        bool hasBorder = true;
        bool _allowLabelEdit = false;
        bool _fullRowSelect = false;
        bool _showRootLines = true;
        bool _showLines = false;
        bool _showExpandButtons = true;
        bool _variableRowHeight = false;
        bool _rowLines = false;
        bool _hideRoot = true;
        bool _twistButtons = true;

        ImageList* _imageList = nullptr;

        bool _skipSelectionChangedEvent = false;
        bool _skipExpandedEvent = false;

        static TreeViewHitTestLocations GetHitTestLocationsFromWxFlags(int flags);

        class HitTestResult
        {
        public:
            TreeViewHitTestLocations locations = TreeViewHitTestLocations::None;
            wxTreeItemId item = 0;
        };
    };
}
