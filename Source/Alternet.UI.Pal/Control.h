#pragma once

#include "Common.h"
#include "DrawingContext.h"
#include "Object.h"

namespace Alternet::UI
{
    class Control : public Object
    {
#include "Api/Control.inc"
    public:
        virtual wxWindow* CreateWxWindowCore(wxWindow* parent) = 0;

        wxWindow* GetWxWindow();
        bool IsWxWindowCreated();

        std::vector<Control*> GetChildren();

        bool GetDoNotDestroyWxWindow();
        void SetDoNotDestroyWxWindow(bool value);

    protected:
        void CreateWxWindow();

        void RecreateWxWindowIfNeeded();

        virtual void OnWxWindowCreated();
        virtual void OnWxWindowDestroying();
        virtual void OnWxWindowDestroyed();

        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow();

        virtual Color RetrieveBackgroundColor();
        virtual void ApplyBackgroundColor(const Color& value);

        virtual Color RetrieveForegroundColor();
        virtual void ApplyForegroundColor(const Color& value);

        virtual Rect RetrieveBounds();
        virtual void ApplyBounds(const Rect& value);

        bool EventsSuspended() override;

        virtual void SetWxWindowParent(wxWindow* parent);

    private:
        enum class DelayedControlFlags
        {
            None = 0,
            Visible = 1 << 0,
            Frozen = 1 << 1,
            Enabled = 1 << 2,
        };

        enum class ControlFlags
        {
            None = 0,
            DoNotDestroyWxWindow = 1 << 0,
            CreatingWxWindow = 1 << 1,
        };

        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        std::vector<Control*> _children;
        ControlFlags _flags = ControlFlags::None;
        int _beginUpdateCount = 0;

        DelayedFlags<Control, DelayedControlFlags> _delayedFlags;
        DelayedValue<Control, Rect> _bounds;
        DelayedValue<Control, Color> _backgroundColor;
        DelayedValue<Control, Color> _foregroundColor;

        DelayedValues _delayedValues;

        bool RetrieveVisible();
        void ApplyVisible(bool value);

        bool RetrieveEnabled();
        void ApplyEnabled(bool value);

        bool RetrieveFrozen();
        void ApplyFrozen(bool value);

        virtual Size ClientSizeToSize(const Size& clientSize);
        virtual Size SizeToClientSize(const Size& size);

        bool GetFlag(ControlFlags flag);
        void SetFlag(ControlFlags flag, bool value);

        void OnPaint(wxPaintEvent& event);
        void OnEraseBackground(wxEraseEvent& event);

        void OnMouseCaptureLost(wxEvent& event);

        void OnMouseMove(wxMouseEvent& event);
        void OnMouseEnter(wxMouseEvent& event);
        void OnMouseLeave(wxMouseEvent& event);
        void OnMouseLeftButtonDown(wxMouseEvent& event);
        void OnMouseLeftButtonUp(wxMouseEvent& event);
        void OnVisibleChanged(wxShowEvent& event);

        void UpdateWxWindowParent();
        void DestroyWxWindow(bool finalDestroy = false);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::DelayedControlFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags> { static const bool enable = true; };

