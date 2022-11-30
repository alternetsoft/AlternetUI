#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"

namespace Alternet::UI
{
    class Toolbar;

    class ToolbarItem : public Control
    {
#include "Api/ToolbarItem.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxToolBarToolBase* GetWxTool();

        static ToolbarItem* GetToolbarItemById(int id);

        void RaiseClick();

        bool GetEnabled() override;
        virtual void SetEnabled(bool value) override;

        void SetParentToolbar(Toolbar* value, optional<int> index);
        Toolbar* GetParentToolbar();

    protected:

        void ShowCore() override;

        Rect RetrieveBounds() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;

        void UpdateWxWindowParent() override;

    private:

        bool _settingParentToolbar = false;

        enum class ToolbarItemFlags
        {
            None = 0,
            Enabled = 1 << 0,
            Checked = 1 << 1,
        };

        Toolbar* _parentToolbar = nullptr;
        optional<int> _indexInParentToolbar;

        static wxString CoerceWxToolText(const string& value);

        FlagsAccessor<ToolbarItemFlags> _flags;

        wxToolBarToolBase* _tool = nullptr;
        string _text;

        string _managedCommandId;

        inline static std::map<int, ToolbarItem*> s_itemsByIdsMap;

        void CreateWxTool();
        void DestroyWxTool();
        void RecreateWxTool();

        bool IsSeparator();

    };
}

template<> struct enable_bitmask_operators<Alternet::UI::ToolbarItem::ToolbarItemFlags> { static const bool enable = true; };