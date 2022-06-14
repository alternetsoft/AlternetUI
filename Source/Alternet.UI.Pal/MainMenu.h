#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"

namespace Alternet::UI
{
    class MainMenu : public Control
    {
#include "Api/MainMenu.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxMenuBar* GetWxMenuBar();

        void OnItemRoleChanged(MenuItem* item);

        void OnEndInit() override;
    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;
    private:
        void ApplyItemRoles();

        std::vector<Menu*> _items;
    };
}