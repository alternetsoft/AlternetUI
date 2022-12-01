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
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxToolBar* GetWxToolBar();

        void SetOwnerWindow(Window* window);
    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;

    private:
        void OnToolbarCommand(wxCommandEvent& event);

        void InsertWxItem(int index);
        void RemoveWxItem(int index);

        std::vector<ToolbarItem*> _items;
        Window* _ownerWindow = nullptr;

        wxToolBar* _wxToolBar = nullptr;

        void CreateWxToolbar(Window* window);
        void DestroyWxToolbar();
        void RecreateWxToolbar(Window* window);

        bool _itemTextVisible = true;
        bool _itemImagesVisible = true;

        ToolbarItemImageToTextDisplayMode _imageToTextDisplayMode = ToolbarItemImageToTextDisplayMode::Horizontal;
    };
}
