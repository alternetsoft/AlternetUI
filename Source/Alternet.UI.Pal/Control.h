#pragma once

#include "Common.h"
#include "DrawingContext.h"
#include "Object.h"
#include "UnmanagedDataObject.h"
#include "DropTarget.h"

namespace Alternet::UI
{
    class Window;

    class wxWidgetExtender
    {
    public:
        Control* _palControl = nullptr;

        static wxWidgetExtender* AsExtender(wxWindow* control)
        {
            return dynamic_cast<wxWidgetExtender*>(control);
        }

        wxWidgetExtender()
        {

        }
    };
  
    class Control : public Object
    {
#include "Api/Control.inc"
    public:
        long GetBorderStyle();
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

        wxDragResult RaiseDragOver(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        wxDragResult RaiseDragEnter(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        wxDragResult RaiseDragDrop(const wxPoint& location, wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        void RaiseDragLeave();

        Window* GetParentWindow();

    protected:
        int64_t createStyles = 0;
        bool bindScrollEvents = true;

        virtual void OnParentChanged();
        virtual void OnAnyParentChanged();

        virtual void OnToolTipChanged();

        void CreateWxWindow();

        void RecreateWxWindowIfNeeded();
        void ScheduleRecreateWxWindow(std::function<void()> postRecreateAction);
        void ScheduleRecreateWxWindow();

        virtual void OnWxWindowCreated();
        virtual void OnBeforeDestroyWxWindow();
        virtual void OnWxWindowDestroyed(wxWindow* window);

        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow(Control* child);

        virtual Color RetrieveBackgroundColor();
        virtual void ApplyBackgroundColor(const Color& value);

        virtual Color RetrieveForegroundColor();
        virtual void ApplyForegroundColor(const Color& value);

        virtual Rect RetrieveBounds();
        virtual void ApplyBounds(const Rect& value);

        struct ScrollInfo
        {
            bool visible = false;
            int value = 0;
            int largeChange = 0;
            int maximum = 0;
        };

        virtual ScrollInfo RetrieveVerticalScrollBarInfo();
        virtual void ApplyVerticalScrollBarInfo(const ScrollInfo& value);

        virtual ScrollInfo RetrieveHorizontalScrollBarInfo();
        virtual void ApplyHorizontalScrollBarInfo(const ScrollInfo& value);

        bool EventsSuspended() override;

        virtual void SetWxWindowParent(wxWindow* parent);

        virtual void ShowCore();
        virtual void HideCore();

        bool IsDestroyingWxWindow();
        bool IsRecreatingWxWindow();

        virtual bool RetrieveEnabled();
        virtual void ApplyEnabled(bool value);

        virtual Size ClientSizeToSize(const Size& clientSize);
        virtual Size SizeToClientSize(const Size& size);

        virtual void UpdateWxWindowParent();

        virtual void OnBeginInit();
        virtual void OnEndInit();

        bool IsInitInProgress();

    private:

        bool CanSetScrollbar();

        void NotifyAllChildrenOnParentChange();

        enum class DelayedControlFlags
        {
            None = 0,
            Visible = 1 << 0,
            Frozen = 1 << 1,
            Enabled = 1 << 2
        };

        enum class ControlFlags
        {
            None = 0,
            DoNotDestroyWxWindow = 1 << 0,
            CreatingWxWindow = 1 << 1,
            ClientSizeCacheValid = 1 << 2,
            UserPaint = 1 << 3,
            InitInProgress = 1 << 4,
            PostInitWxWindowRecreationPending = 1 << 5,
            DestroyingWxWindow = 1 << 6,
            IsScrollable = 1 << 7,
            TabStop = 1 << 8,
        };

        void SetRecreatingWxWindow(bool value);

        int _recreatingWxWindowCounter = 0;

        std::vector<std::function<void()>> _postInitActions;

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
        DelayedValue<Control, ScrollInfo> _verticalScrollBarInfo;
        DelayedValue<Control, ScrollInfo> _horizontalScrollBarInfo;

        DelayedValues _delayedValues;

        DropTarget* _dropTarget = nullptr;

        optional<string> _toolTip;

        static DragDropEffects GetDragDropEffects(wxDragResult input);
        static wxDragResult GetDragResult(DragDropEffects input);
        static int GetDoDragDropFlags(DragDropEffects allowedEffects);

        wxDragResult RaiseDragAndDropEvent(
            const wxPoint& location,
            wxDragResult defaultDragResult,
            wxDataObjectComposite* dataObjectComposite,
            ControlEvent event);

        void CreateDropTarget();
        void DestroyDropTarget();

        bool RetrieveVisible();
        void ApplyVisible(bool value);

        bool RetrieveFrozen();
        void ApplyFrozen(bool value);

        void OnPaint(wxPaintEvent& event);
        void OnEraseBackground(wxEraseEvent& event);

        void OnMouseCaptureLost(wxEvent& event);

        void OnMouseEnter(wxMouseEvent& event);
        void OnMouseLeave(wxMouseEvent& event);
        void OnVisibleChanged(wxShowEvent& event);
        void OnSizeChanged(wxSizeEvent& event);
        void OnDestroy(wxWindowDestroyEvent& event);

        void OnScrollTop(wxScrollWinEvent& event);
        void OnScrollBottom(wxScrollWinEvent& event);
        void OnScrollLineUp(wxScrollWinEvent& event);
        void OnScrollLineDown(wxScrollWinEvent& event);
        void OnScrollPageUp(wxScrollWinEvent& event);
        void OnScrollPageDown(wxScrollWinEvent& event);
        void OnScrollThumbTrack(wxScrollWinEvent& event);
        void OnScrollThumbRelease(wxScrollWinEvent& event);

        DelayedValue<Control, Control::ScrollInfo>& GetScrollInfoDelayedValue(const wxScrollWinEvent& event);

        void ApplyScroll(wxScrollWinEvent& event, int position);

        void OnGotFocus(wxFocusEvent& event);
        void OnLostFocus(wxFocusEvent& event);

        void ApplyToolTip();

        void DestroyWxWindow();

        Size GetClientSizeCore();

        static ControlsByWxWindowsMap s_controlsByWxWindowsMap;

        static void AssociateControlWithWxWindow(wxWindow* wxWindow, Control* control);
        static void RemoveWxWindowControlAssociation(wxWindow* wxWindow);

        ScrollInfo GetScrollInfo(ScrollBarOrientation orientation);
        void SetScrollInfo(ScrollBarOrientation orientation, const ScrollInfo& value);
        wxOrientation GetWxScrollOrientation(ScrollBarOrientation orientation);
        ScrollBarOrientation GetScrollOrientation(wxOrientation orientation);
        DelayedValue<Control, ScrollInfo>& GetScrollInfoDelayedValue(ScrollBarOrientation orientation);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::DelayedControlFlags> { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags> { static const bool enable = true; };

