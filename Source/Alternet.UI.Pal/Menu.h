#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class Menu : public Object
    {
#include "Api/Menu.inc"
    public:
        static string _eventMenuItemId;

        static wxString CoerceMenuText(const string& value);

    protected:
    private:
    };

    class wxAlternetMenu;
    class wxAlternetMenuBar;

    class wxAlternetMenuItem : public wxMenuItem
    {
    public:
        wxAlternetMenu* ownerMenu = nullptr;
        ImageSet* _normalImage = nullptr;
        string _id;

        wxAlternetMenuItem(wxMenu* parent = nullptr,
            int id = wxID_ANY,
            const wxString& text = wxEmptyString,
            const wxString& help = wxEmptyString,
            wxItemKind kind = wxITEM_NORMAL,
            wxMenu* subMenu = nullptr)
            : wxMenuItem(parent, id, text, help, kind, subMenu)
        {
        }

        wxAlternetMenuItem* GetSubMenuItemById(const string& id);
    };

    class wxAlternetMenu : public wxMenu
    {
    public:
        wxAlternetMenuBar* ownerMenuBar = nullptr;
        wxAlternetMenuItem* ownerMenuItem = nullptr;
        string _id;

        wxAlternetMenu(const wxString& title = wxEmptyString, long style = 0)
            : wxMenu(title, style)
        {
            Bind(wxEVT_MENU, &wxAlternetMenu::OnMenuCommand, this);
            Bind(wxEVT_MENU_OPEN, &wxAlternetMenu::OnMenuOpen, this);
            Bind(wxEVT_MENU_CLOSE, &wxAlternetMenu::OnMenuClose, this);
            Bind(wxEVT_MENU_HIGHLIGHT, &wxAlternetMenu::OnMenuHighlight, this);
        }

        wxAlternetMenuItem* GetItemById(const string& id)
        {
            for (int n = GetMenuItemCount() - 1; n >= 0; --n)
            {
                wxMenuItem* item = FindItemByPosition(n);
                auto alternetItem = wxDynamicCast(item, wxAlternetMenuItem);
                if (alternetItem != nullptr && alternetItem->_id == id)
                    return alternetItem;
            }

            for (int n = GetMenuItemCount() - 1; n >= 0; --n)
            {
                wxMenuItem* item = FindItemByPosition(n);
                auto alternetItem = wxDynamicCast(item, wxAlternetMenuItem);
                alternetItem = alternetItem->GetSubMenuItemById(id);
                if (alternetItem != nullptr)
                    return alternetItem;
            }

            return nullptr;
        }

        int GetItemIndex(const string& id)
        {
            for (int n = GetMenuItemCount() - 1; n >= 0; --n)
            {
                wxMenuItem* item = FindItemByPosition(n);
                auto alternetItem = wxDynamicCast(item, wxAlternetMenuItem);

                if (alternetItem != nullptr && alternetItem->_id == id)
                    return n;
            }

            return -1;
		}

    private:
        void OnMenuCommand(wxCommandEvent& event);
        void OnMenuOpen(wxMenuEvent& event);
        void RaiseMenuEvent(wxMenuEvent& event, Menu::MenuEvent eventType);
        void RaiseMenuItemEvent(wxMenuEvent& event, Menu::MenuEvent eventType);
        void OnMenuClose(wxMenuEvent& event);
        void OnMenuHighlight(wxMenuEvent& event);
    };

    class wxAlternetMenuBar : public wxMenuBar
    {
    public:
        wxFrame* ownerFrame = nullptr;
        string _id;

        wxAlternetMenuBar(long style = 0)
            : wxMenuBar(style)
        {
        }

        wxAlternetMenu* GetItemById(const string& id)
        {
            for (int i = 0; i < GetMenuCount(); i++)
            {
                wxMenu* menu = GetMenu(i);
                if (menu != nullptr)
                {
                    auto alternetMenu = wxDynamicCast(menu, wxAlternetMenu);
                    if (alternetMenu != nullptr && alternetMenu->_id == id)
                        return alternetMenu;
                }
            }
            return nullptr;
        }

        int GetItemIndex(const string& id)
        {
            for (int i = 0; i < GetMenuCount(); i++)
            {
                wxMenu* menu = GetMenu(i);
                if (menu != nullptr)
                {
                    auto alternetMenu = wxDynamicCast(menu, wxAlternetMenu);
                    if (alternetMenu != nullptr && alternetMenu->_id == id)
                        return i;
                }
            }
            return -1;
		}
    };
}
