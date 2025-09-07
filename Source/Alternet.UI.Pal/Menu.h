#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class MenuItem;
    class MainMenu;

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

    private:
        void OnMenuCommand(wxCommandEvent& event)
        {
            event.StopPropagation();
			Menu::_eventMenuItemId = _id;

/*
        auto item = MenuItem::GetMenuItemById(event.GetId());
        if (item == nullptr)
            return;
        item->RaiseClick();
*/
        }

        void OnMenuOpen(wxMenuEvent& event)
        {
            event.StopPropagation();
            Menu::_eventMenuItemId = _id;
            Menu::RaiseStaticEvent(MenuEvent::Opened);

            /*

        for (auto item : _items)
        {
            item->RaiseMenuOpen();
        }
*/
        }

        void OnMenuClose(wxMenuEvent& event)
        {
            event.StopPropagation();
            Menu::_eventMenuItemId = _id;

            /*
            RaiseEvent(MenuEvent::Closed);

            for (auto item : _items)
            {
                item->RaiseMenuClose();
            }
*/
        }

        void OnMenuHighlight(wxMenuEvent& event)
        {
            event.StopPropagation();
            Menu::_eventMenuItemId = _id;

/*
            auto item = MenuItem::GetMenuItemById(evt.GetMenuId());
            if (item == nullptr)
                return;
            item->RaiseMenuHighlight();
*/
        }
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
    };

    class Menu : public Object
    {
#include "Api/Menu.inc"
    public:
        static string _eventMenuItemId;
    protected:
    private:
    };
}
