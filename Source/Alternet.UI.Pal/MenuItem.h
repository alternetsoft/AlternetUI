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

        static wxString CoerceWxItemText(string value, MenuItem* menuItem);

        bool GetEnabled() override;
        virtual void SetEnabled(bool value) override;

    protected:

        void ShowCore() override;

        Rect RetrieveBounds() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;

        void UpdateWxWindowParent() override;

    private:

        enum class MenuItemFlags
        {
            None = 0,
            Enabled = 1 << 0,
            Checked = 1 << 1,
        };

        FlagsAccessor<MenuItemFlags> _flags;

        wxMenuItem* _menuItem = nullptr;
        string _text;

        Menu* _parentMenu = nullptr;
        optional<int> _indexInParentMenu;
        string _managedCommandId;
        wxAcceleratorEntry* _accelerator = nullptr;

        inline static std::map<int, MenuItem*> s_itemsByIdsMap;

        void CreateWxMenuItem();
        void DestroyWxMenuItem();
        void RecreateWxMenuItem();

        void DestroyAcceleratorIfNeeded();

        bool IsSeparator();
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::MenuItem::MenuItemFlags> { static const bool enable = true; };