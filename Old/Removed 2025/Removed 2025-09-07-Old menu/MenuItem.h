#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Menu;
    class MainMenu;

    class MenuItem : public Object
    {
#include "Api/MenuItem.inc"
    public:

        struct RoleBasedOverrideData
        {
            wxMenu* parentMenuOverride = nullptr;
            string preservedText;
            Key preservedKey;
            ModifierKeys preservedModifierKeys;
        };

        wxMenuItem* GetWxMenuItem();

        static MenuItem* GetMenuItemById(int id);

        void RaiseClick();
        void RaiseMenuOpen();
        void RaiseMenuClose();
        void RaiseMenuHighlight();

        void SetParentMenu(Menu* value, optional<int> index);
        Menu* GetParentMenu();

        void SetRoleBasedOverrideData(optional<RoleBasedOverrideData> value);
        optional<RoleBasedOverrideData> GetRoleBasedOverrideData();

        static wxString CoerceWxItemText(string value, MenuItem* menuItem);

        MainMenu* FindParentMainMenu();

        Key GetShortcutKey();
        ModifierKeys GetShortcutModifierKeys();
    protected:
        ImageSet* _normalImage = nullptr;
        ImageSet* _disabledImage = nullptr;

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

        Key _shortcutKey = Key::None;
        ModifierKeys _shortcutModifierKeys = ModifierKeys::None;

        Menu* _parentMenu = nullptr;
        optional<size_t> _indexInParentMenu;

        optional<RoleBasedOverrideData> _roleBasedOverrideData;

        string _managedCommandId;
        wxAcceleratorEntry* _accelerator = nullptr;
        string _role;

        inline static std::map<int, MenuItem*> s_itemsByIdsMap;

        void CreateWxMenuItem();
        void DestroyWxMenuItem();
        void RecreateWxMenuItem();

        void DestroyAcceleratorIfNeeded();

        bool IsSeparator();
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::MenuItem::MenuItemFlags> { static const bool enable = true; };