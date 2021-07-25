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

        bool _skipSelectionChangedEvent = false;
    };
}
