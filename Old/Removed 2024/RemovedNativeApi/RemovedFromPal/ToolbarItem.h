#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"
#include "Menu.h"
#include "ImageSet.h"

namespace Alternet::UI
{
    class Toolbar;

    class ToolbarItem : public Control
    {
#include "Api/ToolbarItem.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxWindow* CreateWxWindowUnparented() override;

        struct ToolInfo
        {
            int id = -1;
            wxString text;
            wxString toolTipText;
            wxBitmapBundle image;
            wxBitmapBundle disabledImage;
            wxItemKind kind = wxItemKind::wxITEM_NORMAL;
            wxMenu* dropDownMenu = nullptr;
        };

        ToolInfo* GetToolInfo();

        static ToolbarItem* GetToolbarItemById(int id);

        void RaiseClick();

        bool GetEnabled() override;
        virtual void SetEnabled(bool value) override;

        void SetParentToolbar(Toolbar* value, optional<int> index);
        Toolbar* GetParentToolbar();

        void InsertWxTool(int index);
        void RemoveWxTool();

    protected:

        void OnToolTipChanged() override;

        void ShowCore() override;

        Rect RetrieveBounds() override;
        void ApplyBounds(const Rect& value) override;
        Size SizeToClientSize(const Size& size) override;

        void UpdateWxWindowParent() override;

    private:
        ImageSet* _image = nullptr;
        ImageSet* _disabledImage = nullptr;

        enum class ToolbarItemFlags
        {
            None = 0,
            Enabled = 1 << 0,
            Checked = 1 << 1,
        };

        Toolbar* _parentToolbar = nullptr;
        optional<int> _indexInParentToolbar;

        Menu* _dropDownMenu = nullptr;

        static wxString CoerceWxToolText(const string& value);

        FlagsAccessor<ToolbarItemFlags> _flags;

        ToolInfo* _toolInfo = nullptr;
        string _text;

        string _managedCommandId;

        bool _isCheckable = false;

        inline static std::map<int, ToolbarItem*> s_itemsByIdsMap;

        void CreateToolInfo();
        void DestroyWxTool();
        void RecreateTool();

        bool IsSeparator();

        void ApplyChecked();

        void ApplyEnabled();

        wxToolBar* GetToolbar();

        wxToolBarToolBase* _wxTool = nullptr;
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::ToolbarItem::ToolbarItemFlags> { static const bool enable = true; };