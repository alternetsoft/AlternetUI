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

        WX_DECLARE_HASH_MAP(wxWindow*, Control*,
            wxPointerHash, wxPointerEqual,
            ControlsByWxWindowsMap);

        Control* GetParent();

        static Control* TryFindControlByWxWindow(wxWindow* wxWindow);

    protected:
        void CreateWxWindow();

        void RecreateWxWindowIfNeeded();

        virtual void OnWxWindowCreated();
        virtual void OnWxWindowDestroying();
        virtual void OnWxWindowDestroyed();

        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow(Control* child);

        virtual Color RetrieveBackgroundColor();
        virtual void ApplyBackgroundColor(const Color& value);

        virtual Color RetrieveForegroundColor();
        virtual void ApplyForegroundColor(const Color& value);

        virtual Rect RetrieveBounds();
        virtual void ApplyBounds(const Rect& value);

        bool EventsSuspended() override;

        virtual void SetWxWindowParent(wxWindow* parent);

        virtual void ShowCore();
        virtual void HideCore();

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
            ClientSizeCacheValid = 1 << 2,
            UserPaint = 1 << 3,
        };

        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        std::vector<Control*> _children;
        FlagsAccessor<ControlFlags> _flags;

        int _beginUpdateCount = 0;
        Size _clientSizeCache;

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

        void OnPaint(wxPaintEvent& event);
        void OnEraseBackground(wxEraseEvent& event);

        void OnMouseCaptureLost(wxEvent& event);

        void OnMouseEnter(wxMouseEvent& event);
        void OnMouseLeave(wxMouseEvent& event);
        void OnVisibleChanged(wxShowEvent& event);
        void OnSizeChanged(wxSizeEvent& event);

        void UpdateWxWindowParent();
        void DestroyWxWindow(bool finalDestroy = false);

        void DestroyWxWindowAndAllChildren();

        Size GetClientSizeCore();

        static ControlsByWxWindowsMap s_controlsByWxWindowsMap;

        static void AssociateControlWithWxWindow(wxWindow* wxWindow, Control* control);
        static void RemoveWxWindowControlAssociation(wxWindow* wxWindow);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::DelayedControlFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags> { static const bool enable = true; };

