#pragma once

#include "Common.h"

namespace Alternet::UI
{
    enum class ControlFlags
    {
        None = 0,
        Visible = 1 << 0,
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::ControlFlags> { static const bool enable = true; };

namespace Alternet::UI
{
    class Control
    {
#include "Api/Control.inc"
    public:
        virtual wxWindow* CreateWxWindowCore() = 0;

        Control* GetParent();

        wxWindow* GetWxWindow();
    protected:
        void CreateWxWindow();
        bool IsWxWindowCreated();

        virtual wxWindow* GetParentingWxWindow();

    private:
        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        DelayedFlags<Control, ControlFlags> _flags;
        std::vector<Control*> _children;

        bool RetrieveVisible();
        void ApplyVisible(bool value);
    };
}
