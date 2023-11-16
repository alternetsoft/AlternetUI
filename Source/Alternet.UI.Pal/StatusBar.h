#pragma once
#include "Common.h"
#include "ApiTypes.h"
#include "Object.h"

namespace Alternet::UI
{
    class StatusBarPanel;
    class Window;

    // https://docs.wxwidgets.org/3.2/classwx_status_bar.html
    class StatusBar : public Object
    {
#include "Api/StatusBar.inc"
    public:
        wxStatusBar* GetWxStatusBar();

        void SetOwnerWindow(Window* window);
    protected:
    private:
        wxStatusBar* _wxStatusBar = nullptr;
        Window* _ownerWindow = nullptr;

        void CreateWxStatusBar();
        void DestroyWxStatusBar();
        void RecreateWxStatusBar();

        bool _sizingGripperVisible = true;
    };
}
