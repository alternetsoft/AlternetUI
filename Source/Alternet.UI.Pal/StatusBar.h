#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Control.h"

namespace Alternet::UI
{
    class StatusBarPanel;
    class Window;

    // https://docs.wxwidgets.org/3.2/classwx_status_bar.html
    class StatusBar : public Control
    {
#include "Api/StatusBar.inc"
    public:
        wxWindow* CreateWxWindowCore(wxWindow* parent) override;
        wxStatusBar* GetWxStatusBar();

        void SetOwnerWindow(Window* window);
    protected:
    private:
        wxStatusBar* _wxStatusBar = nullptr;
        Window* _ownerWindow = nullptr;

        void CreateWxStatusBar(Window* window);
        void DestroyWxStatusBar();
        void RecreateWxStatusBar(Window* window);

        bool _sizingGripperVisible = true;
    };
}
