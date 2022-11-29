#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"

namespace Alternet::UI
{
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

    protected:

        void ShowCore() override;

        Rect RetrieveBounds() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;

        void UpdateWxWindowParent() override;

    private:

        enum class ToolbarItemFlags
        {
            None = 0,
            Enabled = 1 << 0,
            Checked = 1 << 1,
        };

        FlagsAccessor<ToolbarItemFlags> _flags;

        wxToolBarToolBase* _toolbarItem = nullptr;
        string _text;

        string _managedCommandId;

        inline static std::map<int, ToolbarItem*> s_itemsByIdsMap;

        void CreateWxTool();
        void DestroyWxTool();
        void RecreateWxTool();

        bool IsSeparator();

    };
}
