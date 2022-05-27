#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Menu;

    class MenuItem : public Control
    {
#include "Api/MenuItem.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxMenuItem* GetWxMenuItem();

        static MenuItem* GetMenuItemById(int id);

        void RaiseClick();

        void SetParentMenu(Menu* value, optional<int> index);

        static wxString CoerceWxItemText(string value);

    protected:

        void ShowCore() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;

    private:

        wxMenuItem* _menuItem = nullptr;
        string _text;

        Menu* _parentMenu = nullptr;
        optional<int> _indexInParentMenu;

        inline static std::map<int, MenuItem*> s_itemsByIdsMap;

        void CreateWxMenuItem();
        void DestroyWxMenuItem();
        void RecreateWxMenuItem();
        void RecreateWxMenuItemIfNeeded();
    };
}
