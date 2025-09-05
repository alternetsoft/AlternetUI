#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"

namespace Alternet::UI
{
    class MainMenu : public Object
    {
#include "Api/MainMenu.inc"
    public:
        wxMenuBar* GetWxMenuBar();

        void OnItemRoleChanged(MenuItem* item);

        std::vector<Menu*> GetItems();

        void SetItemHidden(Menu* item, bool hidden);
        bool IsItemHidden(Menu* item);
    protected:
    private:
        wxMenuBar* menuBar;
            
        void ApplyItemRoles();

        void OnMenuCommand(wxCommandEvent& event);

        int GetItemLogicalIndex(Menu* item);
        int LogicalIndexToWxIndex(int logicalIndex);

        bool IsItemHidden(int index);

        void InsertWxItem(int index);
        void RemoveWxItem(int index);

        std::vector<Menu*> _items;

        std::set<int> _hiddenItemIndices;
        std::map<Menu*, string> _itemTextByMenu;
    };
}