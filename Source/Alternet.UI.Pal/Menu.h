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

        wxMenu* GetWxMenu();

        static Menu* TryFindMenuByWxMenu(wxMenu* wxMenu);

        WX_DECLARE_HASH_MAP(wxMenu*, Menu*,
            wxPointerHash, wxPointerEqual,
            MenusByWxMenusMap);

        void OnMenuCommand(wxCommandEvent& evt);
        void OnMenuOpen(wxMenuEvent& evt);
        void OnMenuClose(wxMenuEvent& evt);
        void OnMenuHighlight(wxMenuEvent& evt);

        void SetParent(MainMenu* mainMenu);
        void SetParent(MenuItem* item);

        MainMenu* GetParentMainMenu();

        MainMenu* FindParentMainMenu();

        std::vector<MenuItem*> GetItems();

        void DetachAndRecreateWxMenu();

        void CreateWxMenu();
        void UnregisterWxMenu();
    protected:

    private:

        wxMenu* _menu = nullptr;
        
        MainMenu* _parentMainMenu = nullptr;
        MenuItem* _parentMenuItem = nullptr;

        std::vector<MenuItem*> _items;

        static MenusByWxMenusMap s_menusByWxMenusMap;

        static void AssociateMenuWithWxMenu(wxMenu* wxMenu, Menu* menu);
        static void RemoveWxMenuAssociation(wxMenu* wxMenu);

    };
}
