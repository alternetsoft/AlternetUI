#pragma once

#include "Common.h"
#include "DrawingContext.h"
#include "Object.h"
#include "ImageSet.h"
#include "UnmanagedDataObject.h"
#include "DropTarget.h"

namespace Alternet::UI
{
    class wxWindowBaseUnprotected: public wxWindowBase
    {
    public:
        static void NotifyCaptureLost() 
        {
            wxWindowBase::NotifyCaptureLost();
        }
    };

    // wxDummyPanel ===========================================

    class wxDummyPanel : public wxPanel
    {
    public:        
        wxDummyPanel(wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxNO_BORDER,
            const wxString& name = wxASCII_STR(wxPanelNameStr))
        {
            Create(parent, winid, pos, size, style, name);
        }

        wxDummyPanel(wxString idstr)
        {
#ifdef _DEBUG
            SetName(idstr);
#endif
        }
    };

    class Window;

    // wxWidgetExtender ===========================================

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

    class wxScrolledWindow2 : public wxScrolledWindow, public wxWidgetExtender
    {
    public:
        Control* _owner = nullptr;

        virtual bool AcceptsFocus() const override;
        virtual bool AcceptsFocusFromKeyboard() const override;
        virtual bool AcceptsFocusRecursively() const override;

        wxScrolledWindow2() {}
        wxScrolledWindow2(
            Control* owner,
            wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxNO_BORDER)
        {
            _owner = owner;
            Create(parent, winid, pos, size, style);
        }
    protected:
    };

    class wxScrolledCanvas2 : public wxScrolled<wxWindow>, public wxWidgetExtender
    {
    public:
        Control* _owner = nullptr;

        virtual bool AcceptsFocus() const override;
        virtual bool AcceptsFocusFromKeyboard() const override;
        virtual bool AcceptsFocusRecursively() const override;

        wxScrolledCanvas2() {}
        wxScrolledCanvas2(
            Control* owner,
            wxWindow* parent,
            wxWindowID winid = wxID_ANY,
            const wxPoint& pos = wxDefaultPosition,
            const wxSize& size = wxDefaultSize,
            long style = wxNO_BORDER)
        {
            _owner = owner;
            Create(parent, winid, pos, size, style);
        }
    protected:
    };

    // Control ===========================================
                                      
    class Control : public Object
    {
#include "Api/Control.inc"
    public:
        bool IsInitInProgress();

        wxCursor _cursor = wxNullCursor;

        void OnTextChanged(wxCommandEvent& event);
        void OnDpiChanged(wxDPIChangedEvent& event);

        long BuildStyle(long style, long element, bool value); 
        void UpdateWindowStyle(long element, bool value);
        static wxString GetMouseEventDesc(const wxMouseEvent& ev);

        virtual wxWindow* CreateWxWindowCore(wxWindow* parent) = 0;

        virtual wxWindow* CreateWxWindowUnparented() = 0;

        wxWindow* GetWxWindow();
        bool IsWxWindowCreated();

        std::vector<Control*> GetChildren();

        bool HasExtraStyle(long extra);
        void SetExtraStyle(long extra, bool value);
        bool GetDoNotDestroyWxWindow();
        void SetDoNotDestroyWxWindow(bool value);

        WX_DECLARE_HASH_MAP(wxWindow*, Control*,
            wxPointerHash, wxPointerEqual,
            ControlsByWxWindowsMap);

        Control* GetParent();

        static Control* TryFindControlByWxWindow(wxWindow* wxWindow);

        wxDragResult RaiseDragOver(const wxPoint& location,
            wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        wxDragResult RaiseDragEnter(const wxPoint& location,
            wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        wxDragResult RaiseDragDrop(const wxPoint& location,
            wxDragResult defaultDragResult, wxDataObjectComposite* dataObjectComposite);
        void RaiseDragLeave();

        Window* GetParentWindow();

        bool _acceptsFocus = true;
        bool _acceptsFocusFromKeyboard = true;
        bool _acceptsFocusRecursively = true;

        virtual void ShowCore();
        virtual void HideCore();
        void ApplyVisible(bool value);

        bool HasEnabledChilds();

        virtual void OnSysColorChanged(wxSysColourChangedEvent& event);

    protected:
        bool _wantChars = false;
        bool _showVertScrollBar = false;
        bool _showHorzScrollBar = false;
        bool _scrollBarAlwaysVisible = false;
        bool _destroyed = false;
        bool _allowDoubleBuffered = true;
        
        bool bindScrollEvents = true;
        int _ignoreRecreate = 0;
        wxBorder _borderStyle = wxBorder::wxBORDER_NONE;
        bool _allowDefaultContextMenu = true;
        int _disableRecreateCounter = 0;
        SizeI _eventOldDpi;
        SizeI _eventNewDpi;
        wxWindow* _eventFocusWindow = nullptr;

        bool IsNullOrDeleting();

        long GetDefaultStyle();

        virtual void OnPaint(wxPaintEvent& event);
        virtual void OnEraseBackground(wxEraseEvent& event);

        void OnMouseCaptureLost(wxEvent& event);

        virtual void OnMouseWheel(wxMouseEvent& event);
        virtual void OnContextMenu(wxContextMenuEvent& event);
        virtual void OnMouseLeftUp(wxMouseEvent& event);
        virtual void OnMouseRightUp(wxMouseEvent& event);
        virtual void OnVisibleChanged(wxShowEvent& event);
        virtual void OnSizeChanged(wxSizeEvent& event);
        virtual void OnLocationChanged(wxMoveEvent& event);
        virtual void OnDestroy(wxWindowDestroyEvent& event);
        virtual void OnIdle(wxIdleEvent& event);
        virtual void OnSetCursor(wxSetCursorEvent& event);
        virtual void OnActivate(wxActivateEvent& event);

        virtual void OnScrollTop(wxScrollWinEvent& event);
        virtual void OnScrollBottom(wxScrollWinEvent& event);
        virtual void OnScrollLineUp(wxScrollWinEvent& event);
        virtual void OnScrollLineDown(wxScrollWinEvent& event);
        virtual void OnScrollPageUp(wxScrollWinEvent& event);
        virtual void OnScrollPageDown(wxScrollWinEvent& event);
        virtual void OnScrollThumbTrack(wxScrollWinEvent& event);
        virtual void OnScrollThumbRelease(wxScrollWinEvent& event);

        virtual void OnParentChanged();
        virtual void OnAnyParentChanged();

        virtual void OnToolTipChanged();

        virtual void CreateWxWindow();

        virtual void RecreateWxWindowIfNeeded();
        void ScheduleRecreateWxWindow(std::function<void()> postRecreateAction);
        void ScheduleRecreateWxWindow();

        virtual void OnWxWindowCreated();
        virtual void OnBeforeDestroyWxWindow();
        virtual void OnWxWindowDestroyed(wxWindow* window);

        DelayedValues& GetDelayedValues();

        virtual wxWindow* GetParentingWxWindow(Control* child);

        bool EventsSuspended() override;

        virtual void SetWxWindowParent(wxWindow* parent);

        bool IsDestroyingWxWindow();
        bool IsRecreatingWxWindow();

        virtual bool RetrieveEnabled();
        virtual void ApplyEnabled(bool value);

        virtual Size ClientSizeToSize(const Size& clientSize);
        virtual Size SizeToClientSize(const Size& size);

        virtual void UpdateWxWindowParent();

        virtual void OnBeginInit();
        virtual void OnEndInit();

    private:
        string _textValue = u"";
        RectD _eventBounds;
        bool _destroying = false;

        bool CanSetScrollbar();

        void NotifyAllChildrenOnParentChange();

        enum class DelayedControlFlags
        {
            None = 0,
            Visible = 1 << 0,
            Enabled = 1 << 1
        };

        enum class ControlFlags
        {
            None = 0,
            DoNotDestroyWxWindow = 1 << 0,
            CreatingWxWindow = 1 << 1,
            UserPaint = 1 << 3,
            InitInProgress = 1 << 4,
            PostInitWxWindowRecreationPending = 1 << 5,
            DestroyingWxWindow = 1 << 6,
            IsScrollable = 1 << 7,
            TabStop = 1 << 8,
            Active = 1 << 9,
        };

        void SetRecreatingWxWindow(bool value);

        int _recreatingWxWindowCounter = 0;

        std::vector<std::function<void()>> _postInitActions;

        wxWindow* _wxWindow = nullptr;
        Control* _parent = nullptr;
        std::vector<Control*> _children;
        FlagsAccessor<ControlFlags> _flags;

        int _beginUpdateCount = 0;

        DelayedFlags<Control, DelayedControlFlags> _delayedFlags;

        DelayedValues _delayedValues;

        DropTarget* _dropTarget = nullptr;

        optional<string> _toolTip;

        int _scrollbarEvtKind = 0;
        int _scrollbarEvtPosition = 0;

        static DragDropEffects GetDragDropEffects(wxDragResult input);
        static wxDragResult GetDragResult(DragDropEffects input);
        static int GetDoDragDropFlags(DragDropEffects allowedEffects);

        wxDragResult RaiseDragAndDropEvent(
            const wxPoint& location,
            wxDragResult defaultDragResult,
            wxDataObjectComposite* dataObjectComposite,
            ControlEvent event);

        void CreateDropTarget();
        void DestroyDropTarget(bool setDropTarget);

        bool RetrieveVisible();

        void ApplyScroll(int evtKind, wxScrollWinEvent& event, int position);

        void OnGotFocus(wxFocusEvent& event);
        void OnLostFocus(wxFocusEvent& event);

        void ApplyToolTip();

        void DestroyWxWindow();

        static ControlsByWxWindowsMap s_controlsByWxWindowsMap;

        static void AssociateControlWithWxWindow(wxWindow* wxWindow, Control* control);
        static void RemoveWxWindowControlAssociation(wxWindow* wxWindow);

        wxOrientation GetWxScrollOrientation(ScrollBarOrientation orientation);
        ScrollBarOrientation GetScrollOrientation(wxOrientation orientation);
    public:
        void LogSizeMethod(std::string methodName, const Size& value);
        void LogRectMethod(wxString name, const Rect& value, const wxRect& wx);
    };
}

template<> struct enable_bitmask_operators<Alternet::UI::Control::DelayedControlFlags>
    { static const bool enable = true; };
template<> struct enable_bitmask_operators<Alternet::UI::Control::ControlFlags>
    { static const bool enable = true; };

