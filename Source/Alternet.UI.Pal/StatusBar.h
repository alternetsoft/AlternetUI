#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class StatusBarPanel;
    class Window;

    class StatusBar : public Control
    {
#include "Api/StatusBar.inc"
    public:

        wxWindow* CreateWxWindowCore(wxWindow* parent) override;

        wxStatusBar* GetWxStatusBar();

        void SetOwnerWindow(Window* window);

        void OnItemChanged(int index);
    protected:
        void ApplyEnabled(bool value) override;
        void ApplyBounds(const Rect& value) override;

    private:
        std::vector<StatusBarPanel*> _items;

        wxStatusBar* _wxStatusBar = nullptr;
        Window* _ownerWindow = nullptr;

        void CreateWxStatusBar(Window* window);
        void DestroyWxStatusBar();
        void RecreateWxStatusBar(Window* window);

        bool _sizingGripperVisible = true;

        void ApplyItems(size_t startIndex, optional<int> count = nullopt);
    };
}
