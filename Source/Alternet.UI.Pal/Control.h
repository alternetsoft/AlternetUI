#pragma once

#include "Common.h"
#include "DrawingContext.h"

namespace Alternet::UI
{
    class Control
    {
#include "Api/Control.inc"
    public:
        virtual wxWindow* CreateWxWindowCore(wxWindow* parent) = 0;

        Control* GetParent();

        wxWindow* GetWxWindow();
        bool IsWxWindowCreated();

    protected:
        void CreateWxWindow();

        virtual void OnWxWindowCreated();
        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow();

        virtual SizeF GetDefaultSize();

        virtual bool DoNotBindPaintEvent();

    private:
        enum class ControlFlags
        {
            None = 0,
            Visible = 1 << 0,
        };

        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        std::vector<Control*> _children;

        DelayedFlags<Control, ControlFlags> _flags;
        DelayedValue<Control, RectangleF> _bounds;
        DelayedValues _delayedValues;

        bool RetrieveVisible();
        void ApplyVisible(bool value);

        RectangleF RetrieveBounds();
        void ApplyBounds(const RectangleF& value);

        void OnPaint(wxPaintEvent& event);

        void OnMouseCaptureLost(wxEvent& event);

        void OnMouseMove(wxMouseEvent& event);
        void OnMouseEnter(wxMouseEvent& event);
        void OnMouseLeave(wxMouseEvent& event);
        void OnMouseLeftButtonDown(wxMouseEvent& event);
        void OnMouseLeftButtonUp(wxMouseEvent& event);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags> { static const bool enable = true; };

