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
        wxWindow* CreateWxWindowUnparented() override;

        wxToolBar* GetWxToolBar();

        void SetOwnerWindow(Window* window);

        void OnItemChanged(int index);
        Toolbar(bool mainToolbar);

    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;

    private:
        bool _mainToolbar = true;
        bool _noDivider = false;
        bool _isVertical = false;
        bool _isBottom = false;
        bool _isRight = false;

        void OnToolbarCommand(wxCommandEvent& event);

        void InsertWxItem(int index);
        void RemoveWxItem(int index);

        std::vector<ToolbarItem*> _items;
        Window* _ownerWindow = nullptr;

        wxToolBar* _wxToolBar = nullptr;

        void CreateWxToolbar();
        void DestroyWxToolbar();
        void RecreateWxToolbar();

        bool _itemTextVisible = true;
        bool _itemImagesVisible = true;

        ImageToText _imageToTextDisplayMode = ImageToText::Horizontal;
    };
}