#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class ToolbarItem;
    class Window;

    class Toolbar : public Control
    {
#include "Api/Toolbar.inc"
    public:
        Toolbar(bool mainToolbar);

        wxToolBar* GetToolbar();
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        void SetOwnerWindow(Window* window);

        void OnItemChanged(int index);

    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;

    private:
        void OnToolbarCommand(wxCommandEvent& event);

        void InsertWxItem(int index);
        void RemoveWxItem(int index);

        bool _mainToolbar = true;
        std::vector<ToolbarItem*> _items;
        Window* _ownerWindow = nullptr;

        void DestroyWxToolbar();
        void RecreateWxToolbar();

        bool _itemTextVisible = true;
        bool _itemImagesVisible = true;

        ToolbarItemImageToTextDisplayMode _imageToTextDisplayMode = ToolbarItemImageToTextDisplayMode::Horizontal;
    };
}
