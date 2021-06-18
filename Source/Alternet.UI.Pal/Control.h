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

        static wxFrame* GetParkingWindow();

        std::vector<Control*> GetChildren();

        bool GetDoNotDestroyWxWindow();
        void SetDoNotDestroyWxWindow(bool value);

    protected:
        void CreateWxWindow();

        static void DestroyParkingWindow();
        static bool IsParkingWindowCreated();

        virtual void OnWxWindowCreated();
        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow();

        virtual Color RetrieveBackgroundColor();
        virtual void ApplyBackgroundColor(const Color& value);

        virtual Color RetrieveForegroundColor();
        virtual void ApplyForegroundColor(const Color& value);

    private:
        enum class DelayedControlFlags
        {
            None = 0,
            Visible = 1 << 0,
        };

        enum class ControlFlags
        {
            None = 0,
            DoNotDestroyWxWindow = 1 << 0,
        };

        inline static wxFrame* s_parkingWindow = nullptr;

        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        std::vector<Control*> _children;
        ControlFlags _flags = ControlFlags::None;

        DelayedFlags<Control, DelayedControlFlags> _delayedFlags;
        DelayedValue<Control, RectangleF> _bounds;
        DelayedValue<Control, Color> _backgroundColor;
        DelayedValue<Control, Color> _foregroundColor;
        DelayedValues _delayedValues;

        bool RetrieveVisible();
        void ApplyVisible(bool value);

        RectangleF RetrieveBounds();
        void ApplyBounds(const RectangleF& value);

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
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::DelayedControlFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags> { static const bool enable = true; };

