#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class ToolbarItem;

    class Toolbar : public Control
    {
#include "Api/Toolbar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxToolBar* GetWxToolBar();

    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;

    private:
        void OnToolbarCommand(wxCommandEvent& event);

        void InsertWxItem(int index);
        void RemoveWxItem(int index);

        std::vector<ToolbarItem*> _items;
    };
}
