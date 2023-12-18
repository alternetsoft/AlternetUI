#pragma warning disable
using ApiCommon;
using Alternet.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeApi.Api
{
    public class WxControlFactory
    {
        public static bool Close(IntPtr handle, bool force = false) => default;
        public static bool DestroyWindow(IntPtr handle) => default;
        public static bool DestroyChildren(IntPtr handle) => default;
        public static bool IsBeingDeleted(IntPtr handle) => default;

        // label is just the same as the title (but for, e.g., buttons it
        // makes more sense to speak about labels), title access
        // is available from wxTLW classes only (frames, dialogs)
        public static void SetLabel(IntPtr handle, string label) { }
        public static string GetLabel(IntPtr handle) => default;

        // the window name is used for resource setting in X, it is not the
        // same as the window title/label
        public static void SetName(IntPtr handle, string name) { }
        public static string GetName(IntPtr handle) => default;

        // get or change the layout direction (LTR or RTL) for this window,
        // wxLayout_Default is returned if layout direction is not supported
        public static int /*LayoutDirection*/ GetLayoutDirection(IntPtr handle) => default;
        public static void SetLayoutDirection(IntPtr handle, /*LayoutDirection*/ int dir) { }

        // mirror coordinates for RTL layout if this window uses it and if the
        // mirroring is not done automatically like Win32
        public static int AdjustForLayoutDirection(IntPtr handle, int x,
                                                 int width,
                                                 int widthTotal) => default;

        // window id uniquely identifies the window among its siblings unless
        // it is wxID_ANY which means "don't care"
        public static void SetId(IntPtr handle, int winid) { }

        public static int GetId(IntPtr handle) => default;

        // generate a unique id (or count of them consecutively), returns a
        // valid id in the auto-id range or wxID_NONE if failed.  If using
        // autoid management, it will mark the id as reserved until it is
        // used (by assigning it to a wxWindowIDRef) or unreserved.
        public static int NewControlId(int count = 1) => default; //real static 

        // If an ID generated from NewControlId is not assigned to a wxWindowIDRef,
        // it must be unreserved
        public static void UnreserveControlId(int id, int count = 1) { } //real static 

        // set the window size and/or position
        public static void SetSize2(IntPtr handle, int x, int y, int width,
            int height/*, int sizeFlags = wxSIZE_AUTO*/) { }
        public static void SetSize(IntPtr handle, int width, int height) { }
        public static void Move(IntPtr handle, int x, int y, int flags /*= wxSIZE_USE_EXISTING*/) { }
        public static void SetPosition(IntPtr handle, int width, int height) { }

        // Z-order
        public static void Raise(IntPtr handle) { }
        public static void Lower(IntPtr handle) { }

        // client size is the size of area available for subwindows
        public static void SetClientSize(IntPtr handle, int width, int height) { }

        // get the window position (pointers may be NULL): notice that it is in
        // client coordinates for child windows and screen coordinates for the
        // top level ones, use GetScreenPosition() if you need screen
        // coordinates for all kinds of windows
        public static PointI GetPosition(IntPtr handle) => default;

        // get the window position in screen coordinates
        public static PointI GetScreenPosition(IntPtr handle) => default;

        // get the window size (pointers may be NULL)
        public static SizeI GetSize(IntPtr handle) => default;

        public static SizeI GetClientSize(IntPtr handle) => default;

        // get the position and size at once
        public static RectI GetRect(IntPtr handle) => default;

        public static RectI GetScreenRect(IntPtr handle) => default;

        // get the origin of the client area of the window relative to the
        // window top left corner (the client area may be shifted because of
        // the borders, scrollbars, other decorations...)
        public static PointI GetClientAreaOrigin(IntPtr handle) => default;

        // get the client rectangle in window (i.e. client) coordinates
        public static RectI GetClientRect(IntPtr handle) => default;

        // client<->window size conversion
        public static SizeI ClientToWindowSize(IntPtr handle, SizeI size) => default;
        public static SizeI WindowToClientSize(IntPtr handle, SizeI size) => default;

        // get the size best suited for the window (in fact, minimal
        // acceptable size using which it will still look "nice" in
        // most situations)
        public static SizeI GetBestSize(IntPtr handle) => default;

        // Determine the best size in the other direction if one of them is
        // fixed. This is used with windows that can wrap their contents and
        // returns input-independent best size for the others.
        public static int GetBestHeight(IntPtr handle, int width) => default;
        public static int GetBestWidth(IntPtr handle, int height) => default;

        public static void SetScrollHelper(IntPtr handle, IntPtr sh) { }
        public static IntPtr GetScrollHelper(IntPtr handle) => default;

        // reset the cached best size value so it will be recalculated the
        // next time it is needed.
        public static void InvalidateBestSize(IntPtr handle) { }
        public static void CacheBestSize(IntPtr handle, SizeI size) { }

        // This function will merge the window's best size into the window's
        // minimum size, giving priority to the min size components, and
        // returns the results.
        public static SizeI GetEffectiveMinSize(IntPtr handle) => default;

        // A 'Smart' SetSize that will fill in default size values with 'best'
        // size.  Sets the minsize to what was passed in.
        public static void SetInitialSize(IntPtr handle, SizeI size/*=wxDefaultSize*/) { }

        // the generic centre function - centers the window on parent by`
        // default or on screen if it doesn't have parent or
        // wxCENTER_ON_SCREEN flag is given
        public static void Center(IntPtr handle, int dir /*= wxBOTH*/) { }

        // centre with respect to the parent window
        public static void CenterOnParent(IntPtr handle, int dir /*= wxBOTH*/) { }

        // set window size to wrap around its children
        public static void Fit(IntPtr handle) { }

        // set size to satisfy children
        public static void FitInside(IntPtr handle) { }

        // SetSizeHints is actually for setting the size hints
        // for the wxTLW for a Window Manager - hence the name -
        // and it is therefore overridden in wxTLW to do that.
        // In wxWindow(Base), it has (unfortunately) been abused
        // to mean the same as SetMinSize() and SetMaxSize().

        public static void SetSizeHints(IntPtr handle, int minW, int minH,
                                   int maxW /*= wxDefaultCoord*/, int maxH /*= wxDefaultCoord*/,
                                   int incW /*= wxDefaultCoord*/, int incH /*= wxDefaultCoord*/)
        { }

        // Call these to override what GetBestSize() returns. This
        // method is only because it is overridden in wxTLW
        // as a different API for SetSizeHints().
        public static void SetMinSize(IntPtr handle, SizeI minSize) { }
        public static void SetMaxSize(IntPtr handle, SizeI maxSize) { }

        // Like Set*Size, but for client, not window, size
        public static void SetMinClientSize(IntPtr handle, SizeI size) { }
        public static void SetMaxClientSize(IntPtr handle, SizeI size) { }

        // Override these methods to impose restrictions on min/max size.
        // The easier way is to call SetMinSize() and SetMaxSize() which
        // will have the same effect. Doing both is non-sense.
        public static SizeI GetMinSize(IntPtr handle) => default;
        public static SizeI GetMaxSize(IntPtr handle) => default;

        // Like Get*Size, but for client, not window, size
        public static SizeI GetMinClientSize(IntPtr handle) => default;
        public static SizeI GetMaxClientSize(IntPtr handle) => default;

        // Get the min and max values one by one
        public static int GetMinWidth(IntPtr handle) => default;
        public static int GetMinHeight(IntPtr handle) => default;
        public static int GetMaxWidth(IntPtr handle) => default;
        public static int GetMaxHeight(IntPtr handle) => default;

        // Methods for accessing the size of a window.  For most
        // windows this is just the client area of the window, but for
        // some like scrolled windows it is more or less independent of
        // the screen window size.  You may override the DoXXXVirtual
        // methods below for classes where that is the case.
        public static void SetVirtualSize(IntPtr handle, int x, int y) { }
        public static SizeI GetVirtualSize(IntPtr handle) => default;

        // Return the largest of ClientSize and BestSize (as determined
        // by a sizer, interior children, or other means)
        public static SizeI GetBestVirtualSize(IntPtr handle) => default;

        // Return the magnification of the content of this window for the platforms
        // using logical pixels different from physical ones, i.e. those for which
        // wxHAS_DPI_INDEPENDENT_PIXELS is defined. For the other ones, always
        // returns 1, regardless of DPI scale factor returned by the function below.
        public static double GetContentScaleFactor(IntPtr handle) => default;

        // Return the ratio of the DPI used by this window to the standard DPI,
        // e.g. 1 for standard DPI screens and 2 for "200% scaling".
        public static double GetDPIScaleFactor(IntPtr handle) => default;

        // return the size of the left/right and top/bottom borders in x and y
        // components of the result respectively
        public static SizeI GetWindowBorderSize(IntPtr handle) => default;

        // wxSizer and friends use this to give a chance to a component to recalc
        // its min size once one of the final size components is known. Override
        // this function when that is useful (such as for wxStaticText which can
        // stretch over several lines). Parameter availableOtherDir
        // tells the item how much more space there is available in the opposite
        // direction (-1 if unknown).
        public static bool InformFirstDirection(IntPtr handle, int direction, int size,
            int availableOtherDir) => default;

        // sends a size event to the window using its current size - this has an
        // effect of refreshing the window layout
        // by default the event is sent, i.e. processed immediately, but if flags
        // value includes wxSEND_EVENT_POST then it's posted, i.e. only schedule
        // for later processing
        public static void SendSizeEvent(IntPtr handle, int flags) { }

        // this is a safe wrapper for GetParent()->SendSizeEvent(): it checks that
        // we have a parent window and it's not in process of being deleted
        // this is used by controls such as tool/status bars changes to which must
        // also result in parent re-layout
        public static void SendSizeEventToParent(IntPtr handle, int flags) { }

        // this is a more readable synonym for SendSizeEvent(wxSEND_EVENT_POST)
        public static void PostSizeEvent(IntPtr handle) { }

        // this is the same as SendSizeEventToParent() but using
        // PostSizeEvent()
        public static void PostSizeEventToParent(IntPtr handle) { }

        // These functions should be used before repositioning the children of
        // this window to reduce flicker or, in MSW case, even avoid display
        // corruption in some situations (so they're more than just optimization).
        // EndRepositioningChildren() should be called if and only if
        // BeginRepositioningChildren() returns true. To ensure that this is always
        // done automatically, use ChildrenRepositioningGuard class below.
        public static bool BeginRepositioningChildren(IntPtr handle) => default;
        public static void EndRepositioningChildren(IntPtr handle) { }

        // returns true if window was shown/hidden, false if the nothing was
        // done (window was already shown/hidden)
        public static bool Show(IntPtr handle, bool show = true) => default;
        public static bool Hide(IntPtr handle) => default;

        // show or hide the window with a special effect, not implemented on
        // most platforms (where it is the same as Show()/Hide() respectively)
        // timeout specifies how long the animation should take, in ms, the
        // default value of 0 means to use the default (system-dependent) value
        public static bool ShowWithEffect(IntPtr handle, /*wxShowEffect*/ int effect,
            uint timeout) => default;
        public static bool HideWithEffect(IntPtr handle, /*wxShowEffect*/ int effect,
            uint timeout) => default;

        // returns true if window was enabled/disabled, false if nothing done
        public static bool Enable(IntPtr handle, bool enable = true) => default;
        public static bool Disable(IntPtr handle) => default;

        public static bool IsShown(IntPtr handle) => default;

        // returns true if the window is really enabled and false otherwise,
        // whether because it had been explicitly disabled itself or because
        // its parent is currently disabled -- then this method returns false
        // whatever is the intrinsic state of this window, use IsThisEnabled(0
        // to retrieve it. In other words, this relation always holds:
        //   IsEnabled() == IsThisEnabled() && parent.IsEnabled()
        public static bool IsEnabled(IntPtr handle) => default;

        // returns the internal window state independently of the parent(s)
        // state, i.e. the state in which the window would be if all its
        // parents were enabled (use IsEnabled() above to get the effective
        // window state)
        public static bool IsThisEnabled(IntPtr handle) => default;

        // returns true if the window is visible, i.e. IsShown() returns true
        // if called on it and all its parents up to the first TLW
        public static bool IsShownOnScreen(IntPtr handle) => default;

        // get/set window style (setting style won't update the window and so
        // is only useful for internal usage)
        public static void SetWindowStyleFlag(IntPtr handle, long style) { }
        public static long GetWindowStyleFlag(IntPtr handle) => default;

        // just some (somewhat shorter) synonyms
        public static void SetWindowStyle(IntPtr handle, long style) { }
        public static long GetWindowStyle(IntPtr handle) => default;

        // check if the flag is set
        public static bool HasFlag(IntPtr handle, int flag) => default;

        public static bool IsRetained(IntPtr handle) => default;

        // turn the flag on if it had been turned off before and vice versa,
        // return true if the flag is currently turned on
        public static bool ToggleWindowStyle(IntPtr handle, int flag) => default;

        // extra style: the less often used style bits which can't be set with
        // SetWindowStyleFlag()
        public static void SetExtraStyle(IntPtr handle, long exStyle) { }
        public static long GetExtraStyle(IntPtr handle) => default;

        public static bool HasExtraStyle(IntPtr handle, int exFlag) => default;

        public static void SetThemeEnabled(IntPtr handle, bool enableTheme) { }
        public static bool GetThemeEnabled(IntPtr handle) => default;

        // set focus to this window
        public static void SetFocus(IntPtr handle) { }

        // set focus to this window as the result of a keyboard action
        public static void SetFocusFromKbd(IntPtr handle) { }

        // return the window which currently has the focus or NULL
        public static IntPtr FindFocus() => default; //real static 

        // return true if the window has focus (handles composite windows
        // correctly - returns true if GetMainWindowOfCompositeControl()
        // has focus)
        public static bool HasFocus(IntPtr handle) => default;

        // can this window have focus in principle?
        // the difference between AcceptsFocus[FromKeyboard]() and CanAcceptFocus
        // [FromKeyboard]() is that the former functions are meant to be
        // overridden in the derived classes to simply return false if the
        // control can't have focus, while the latter are meant to be used by
        // this class clients and take into account the current window state
        public static bool AcceptsFocus(IntPtr handle) => default;

        // can this window or one of its children accept focus?
        // usually it's the same as AcceptsFocus() but is overridden for
        // container windows
        public static bool AcceptsFocusRecursively(IntPtr handle) => default;

        // can this window be given focus by keyboard navigation? if not, the
        // only way to give it focus (provided it accepts it at all) is to
        // click it
        public static bool AcceptsFocusFromKeyboard(IntPtr handle) => default;

        // Disable any input focus from the keyboard
        public static void DisableFocusFromKeyboard(IntPtr handle) { }

        // Can this window be focused right now, in its current state? This
        // shouldn't be called at all if AcceptsFocus() returns false.
        // It is a convenient helper for the various functions using it below
        // but also a hook allowing to override the default logic for some rare
        // cases (currently just wxRadioBox in wxMSW) when it's inappropriate.
        public static bool CanBeFocused(IntPtr handle) => default;

        // can this window itself have focus?
        public static bool IsFocusable(IntPtr handle) => default;

        // can this window have focus right now?
        // if this method returns true, it means that calling SetFocus() will
        // put focus either to this window or one of its children, if you need
        // to know whether this window accepts focus itself, use IsFocusable(IntPtr handle)
        public static bool CanAcceptFocus(IntPtr handle) => default;

        // can this window be assigned focus from keyboard right now?
        public static bool CanAcceptFocusFromKeyboard(IntPtr handle) => default;

        // call this when the return value of AcceptsFocus() changes
        public static void SetCanFocus(IntPtr handle, bool canFocus) { }

        // call to customize visible focus indicator if possible in the port
        public static void EnableVisibleFocus(IntPtr handle, bool enabled) { }

        // navigates inside this window
        public static bool NavigateIn(IntPtr handle,
            int flags/*= wxNavigationKeyEvent::IsForward*/) => default;

        // navigates in the specified direction from this window, this is
        // equivalent to GetParent()->NavigateIn()
        public static bool Navigate(IntPtr handle,
            int flags/*= wxNavigationKeyEvent::IsForward*/) => default;

        // this function will generate the appropriate call to Navigate() if the
        // key event is one normally used for keyboard navigation and return true
        // in this case
        // public static bool HandleAsNavigationKey(wxKeyEvent& event) => default;

            // move this window just before/after the specified one in tab order
            // (the other window must be our sibling!)
        public static void MoveBeforeInTabOrder(IntPtr handle, IntPtr win) { }
        public static void MoveAfterInTabOrder(IntPtr handle, IntPtr win) { }

        // get the list of children
        public static IntPtr GetChildren(IntPtr handle) => default;

        // get the window before/after this one in the parents children list,
        // returns NULL if this is the first/last window
        public static IntPtr GetPrevSibling(IntPtr handle) => default;
        public static IntPtr GetNextSibling(IntPtr handle) => default;

        // get the parent or the parent of the parent
        public static IntPtr GetParent(IntPtr handle) => default;
        public static IntPtr GetGrandParent(IntPtr handle) => default;

        // is this window a top level one?
        public static bool IsTopLevel(IntPtr handle) => default;

        // is this window a child or grand child of this one (inside the same TLW)?
        public static bool IsDescendant(IntPtr handle, IntPtr win) => default;

        // it doesn't really change parent, use Reparent() instead
        public static void SetParent(IntPtr handle, IntPtr parent) { }

        // change the real parent of this window, return true if the parent
        // was changed, false otherwise (error or newParent == oldParent)
        public static bool Reparent(IntPtr handle, IntPtr newParent) => default;

        // implementation mostly
        public static void AddChild(IntPtr handle, IntPtr child) { }
        public static void RemoveChild(IntPtr handle, IntPtr child) { }

        // returns true if the child is in the client area of the window, i.e. is
        // not scrollbar, toolbar etc.
        public static bool IsClientAreaChild(IntPtr handle, IntPtr child) => default;

        // find window among the descendants of this one either by id or by
        // name (return NULL if not found)
        public static IntPtr FindWindow(IntPtr handle, long winid) => default;
        public static IntPtr FindWindow2(IntPtr handle, string name) => default;

        // Find a window among any window (all return NULL if not found)
        public static IntPtr FindWindowById(long winid, IntPtr parent = default) => default; //real static 
        public static IntPtr FindWindowByName(string name, IntPtr parent = default) => default;//real static 
        public static IntPtr FindWindowByLabel(string label, IntPtr parent = default) => default;//real static 

        public static void SetValidator(IntPtr handle, IntPtr validator) { }
        public static IntPtr GetValidator(IntPtr handle) => default;

        // validate the correctness of input, return true if ok
        public static bool Validate(IntPtr handle) => default;

        // transfer data between internal and GUI representations
        public static bool TransferDataToWindow(IntPtr handle) => default;
        public static bool TransferDataFromWindow(IntPtr handle) => default;

        public static void InitDialog(IntPtr handle) { }

        public static void SetAcceleratorTable(IntPtr handle, IntPtr accel) { }
        public static IntPtr GetAcceleratorTable(IntPtr handle) => default;

        bool RegisterHotKey(IntPtr handle, int hotkeyId, int modifiers, int keycode) => default;
        bool UnregisterHotKey(IntPtr handle, int hotkeyId) => default;

        // Get the DPI used by the given window or wxSize(0, 0) if unknown.
        public static SizeI GetDPI(IntPtr handle) => default;

        // Some ports need to modify the font object when the DPI of the window it
        // is used with changes, this function can be used to do it.
        // Currently it is only used in wxMSW and is not considered to be part of
        // wxWidgets public API.
        public static void WXAdjustFontToOwnPPI(IntPtr handle, IntPtr font) { }

        // All pixel coordinates used in wx API are in logical pixels, which
        // are the same as physical screen pixels under MSW, but same as DIPs
        // (see below) under the other ports. The functions defined here can be
        // used under all platforms to convert between them without using any
        // preprocessor checks.

        public static SizeI FromPhys(SizeI sz, IntPtr w) => default;//real static 
        public static PointI FromPhys2(PointI pt, IntPtr w) => default;//real static 
        public static int FromPhys3(int d, IntPtr w) => default;//real static 

        public static SizeI FromPhys4(SizeI sz) => default;
        public static PointI FromPhys5(PointI pt) => default;
        public static int FromPhys6(int d) => default;

        public static SizeI ToPhys(SizeI sz, IntPtr w) => default;//real static 
        public static PointI ToPhys2(PointI pt, IntPtr w) => default;//real static 
        public static int ToPhys3(int d, IntPtr w) => default;//real static 
        public static SizeI ToPhys4(SizeI sz) => default;
        public static PointI ToPhys5(PointI pt) => default;
        public static int ToPhys6(int d) => default;

        // DPI-independent pixels, or DIPs, are pixel values for the standard
        // 96 DPI display, they are scaled to take the current resolution into
        // account (i.e. multiplied by the same factor as returned by
        // GetDPIScaleFactor()) if necessary for the current platform.
        // To support monitor-specific resolutions, prefer using the non-static
        // member functions or use a valid (non-null) window pointer.
        // Similarly, currently in practice the factor is the same in both
        // horizontal and vertical directions, but this could, in principle,
        // change too, so prefer using the overloads taking Int32Point or wxSize.
        public static SizeI FromDIP(SizeI sz, IntPtr window) => default;//real static 
        public static PointI FromDIP2(PointI pt, IntPtr window) => default;//real static 
        public static int FromDIP3(int d, IntPtr w) => default;//real static 
        public static SizeI FromDIP4(IntPtr handle, SizeI sz) => default;
        public static PointI FromDIP5(IntPtr handle, PointI pt) => default;
        public static int FromDIP6(IntPtr handle, int d) => default;

        public static SizeI ToDIP(SizeI sz, IntPtr w) => default;//real static 
        public static PointI ToDIP2(PointI pt, IntPtr w) => default;//real static 
        public static int ToDIP3(int d, IntPtr w) => default;//real static 
        public static SizeI ToDIP4(IntPtr handle, SizeI sz) => default;
        public static PointI ToDIP5(IntPtr handle, PointI pt) => default;
        public static int ToDIP6(int d) => default;

        // Dialog units are based on the size of the current font.
        public static PointI ConvertPixelsToDialog(IntPtr handle, PointI pt) => default;
        public static PointI ConvertDialogToPixels(IntPtr handle, PointI pt) => default;
        public static SizeI ConvertPixelsToDialog2(IntPtr handle, SizeI sz) => default;
        public static SizeI ConvertDialogToPixels2(IntPtr handle, SizeI sz) => default;

        // move the mouse to the specified position
        public static void WarpPointer(IntPtr handle, int x, int y) { }

        // start or end mouse capture, these functions maintain the stack of
        // windows having captured the mouse and after calling ReleaseMouse(IntPtr handle)
        // the mouse is not released but returns to the window which had
        // captured it previously (if any)
        public static void CaptureMouse(IntPtr handle) { }
        public static void ReleaseMouse(IntPtr handle) { }

        // get the window which currently captures the mouse or NULL
        public static IntPtr GetCapture() => default;//real static 

        // does this window have the capture?
        public static bool HasCapture(IntPtr handle) => default;

        // enable the specified touch events for this window, return false if
        // the requested events are not supported
        public static bool EnableTouchEvents(IntPtr handle, int WXUNUSEDeventsMask) => default;

        // mark the specified rectangle (or the whole window) as "dirty" so it
        // will be repainted
        public static void RefreshRect(IntPtr handle, RectI rect, bool eraseBackground = true) { }

        // repaint all invalid areas of the window immediately
        public static void Update(IntPtr handle) { }

        // clear the window background
        public static void ClearBackground(IntPtr handle) { }

        // freeze the window: don't redraw it until it is thawed
        public static void Freeze(IntPtr handle) { }

        // thaw the window: redraw it after it had been frozen
        public static void Thaw(IntPtr handle) { }

        // return true if window had been frozen and not unthawed yet
        public static bool IsFrozen(IntPtr handle) => default;

        // adjust DC for drawing on this window
        public static void PrepareDC(IntPtr handle, IntPtr dc) { }

        // enable or disable double buffering
        public static void SetDoubleBuffered(IntPtr handle, bool on) { }

        // return true if the window contents is double buffered by the system
        public static bool IsDoubleBuffered(IntPtr handle) => default;

        // the update region of the window contains the areas which must be
        // repainted by the program
        public static IntPtr GetUpdateRegion(IntPtr handle) => default;

        // get the update rectangle region bounding box in client coords
        public static RectI GetUpdateClientRect(IntPtr handle) => default;

        // these functions verify whether the given point/rectangle belongs to
        // (or at least intersects with) the update region
        public static bool IsExposed(IntPtr handle, int x, int y) => default;
        public static bool IsExposed2(IntPtr handle, int x, int y, int w, int h) => default;

        // set/retrieve the window Colors (system defaults are used by
        // default): SetXXX() functions return true if Color was changed,
        // SetDefaultXXX() reset the "m_inheritXXX" flag after setting the
        // value to prevent it from being inherited by our children
        public static bool SetBackgroundColor(IntPtr handle, Color Color) => default;
        public static void SetOwnBackgroundColor(IntPtr handle, Color Color) { }

        public static Color GetBackgroundColor(IntPtr handle) => default;
        public static bool InheritsBackgroundColor(IntPtr handle) => default;

        public static bool UseBackgroundColor(IntPtr handle) => default;

        public static bool SetForegroundColor(IntPtr handle, Color Color) => default;
        public static void SetOwnForegroundColor(IntPtr handle, Color Color) { }

        public static Color GetForegroundColor(IntPtr handle) => default;
        public static bool UseForegroundColor(IntPtr handle) => default;
        public static bool InheritsForegroundColor(IntPtr handle) => default;

        // Set/get the background style.
        public static bool SetBackgroundStyle(IntPtr handle,
            /*wxBackgroundStyle*/ int style) => default;
        public static /*wxBackgroundStyle*/ int GetBackgroundStyle(IntPtr handle) => default;

        // returns true if the control has "transparent" areas such as a
        // wxStaticText and wxCheckBox and the background should be adapted
        // from a parent window
        public static bool HasTransparentBackground(IntPtr handle) => default;

        // Returns true if background transparency is supported for this
        // window, i.e. if calling SetBackgroundStyle(wxBG_STYLE_TRANSPARENT)
        // has a chance of succeeding. If reason argument is non-NULL, returns a
        // user-readable explanation of why it isn't supported if the return
        // value is false.
        public static bool IsTransparentBackgroundSupported(IntPtr handle,
            string reason /*= NULL*/) => default;

        // set/retrieve the font for the window (SetFont() returns true if the
        // font really changed)
        public static bool SetFont(IntPtr handle, IntPtr font) => default;
        public static void SetOwnFont(IntPtr handle, IntPtr font) { }

        public static IntPtr GetFont(IntPtr handle) => default;

        // set/retrieve the cursor for this window (SetCursor() returns true
        // if the cursor was really changed)
        public static bool SetCursor(IntPtr handle, IntPtr cursor) => default;
        public static IntPtr GetCursor(IntPtr handle) => default;

        // associate a caret with the window
        public static void SetCaret(IntPtr handle, IntPtr caret) { }
        // get the current caret (may be NULL)
        public static IntPtr GetCaret(IntPtr handle) => default;

        // get the (average) character size for the current font
        public static int GetCharHeight(IntPtr handle) => default;
        public static int GetCharWidth(IntPtr handle) => default;

        public static SizeI GetTextExtent(IntPtr handle, string str) => default;

        // Int32Point interface to do the same thing
        public static PointI ClientToScreen(IntPtr handle, PointI pt) => default;
        public static PointI ScreenToClient(IntPtr handle, PointI pt) => default;

        // test where the given (in client coords) point lies
        public static /*wxHitTest*/ int HitTest(IntPtr handle, int x, int y) => default;

        // get the window border style from the given flags: this is different from
        // simply doing flags & wxBORDER_MASK because it uses GetDefaultBorder(IntPtr handle) to
        // translate wxBORDER_DEFAULT to something reasonable
        public static /*wxBorder*/ int GetBorderEx(IntPtr handle, long flags) => default;

        // get border for the flags of this window
        public static /*wxBorder*/ int GetBorder(IntPtr handle) => default;

        // send wxUpdateUIEvents to this window, and children if recurse is true
        public static void UpdateWindowUI(IntPtr handle, long flags /*= wxUPDATE_UI_NONE*/) { }

        // show popup menu at the given position, generate events for the items
        // selected in it
        public static bool PopupMenu(IntPtr handle, IntPtr menu, int x, int y) => default;

        // simply return the id of the selected item or wxID_NONE without
        // generating any events
        public static int GetPopupMenuSelectionFromUser(IntPtr handle, IntPtr menu,
            int x, int y) => default;

        // override this method to return true for controls having multiple pages
        public static bool HasMultiplePages(IntPtr handle) => default;

        // can the window have the scrollbar in this orientation?
        public static bool CanScroll(IntPtr handle, int orient) => default;

        // does the window have the scrollbar in this orientation?
        public static bool HasScrollbar(IntPtr handle, int orient) => default;

        // configure the window scrollbars
        public static void SetScrollbar(IntPtr handle, int orient,
                                   int pos,
                                   int thumbvisible,
                                   int range,
                                   bool refresh = true) { }
        public static void SetScrollPos(IntPtr handle, int orient, int pos, bool refresh = true) { }
        public static int GetScrollPos(IntPtr handle, int orient) => default;
        public static int GetScrollThumb(IntPtr handle, int orient) => default;
        public static int GetScrollRange(IntPtr handle, int orient) => default;

        // scroll window to the specified position
        // void ScrollWindow(int dx, int dy, wxRect* rect = NULL);

        // scrolls window by line/page: note that not all controls support this
        // return true if the position changed, false otherwise
        public static bool ScrollLines(IntPtr handle, int lines) => default;
        public static bool ScrollPages(IntPtr handle, int pages) => default;

        // convenient wrappers for ScrollLines/Pages
        public static bool LineUp(IntPtr handle) => default;
        public static bool LineDown(IntPtr handle) => default;
        public static bool PageUp(IntPtr handle) => default;
        public static bool PageDown(IntPtr handle) => default;

        // call this to always show one or both scrollbars, even if the window
        // is big enough to not require them
        public static void AlwaysShowScrollbars(IntPtr handle, bool horz = true, bool vert = true) { }

        // return true if AlwaysShowScrollbars() had been called before for the
        // corresponding orientation
        public static bool IsScrollbarAlwaysShown(IntPtr handle, int orient) => default;

        // associate this help text with this window
        public static void SetHelpText(IntPtr handle, string text) { }

        // get the help string associated with the given position in this window
        // notice that pt may be invalid if event origin is keyboard or unknown
        // and this method should return the global window help text then
        public static string GetHelpTextAtPoint(IntPtr handle, PointI pt,
            /*wxHelpEvent::Origin*/ int origin) => default;
        // returns the position-independent help text
        public static string GetHelpText(IntPtr handle) => default;

        // the easiest way to set a tooltip for a window is to use this method
        public static void SetToolTip(IntPtr handle, string tip) { }
        // attach a tooltip to the window, pointer can be NULL to remove
        // existing tooltip
        public static void SetToolTip2(IntPtr handle, IntPtr tip) { }
        // more readable synonym for SetToolTip(NULL)
        public static void UnsetToolTip(IntPtr handle) { }
        // get the associated tooltip or NULL if none
        public static IntPtr GetToolTip(IntPtr handle) => default;
        public static string GetToolTipText(IntPtr handle) => default;

        // Use the same tool tip as the given one (which can be NULL to indicate
        // that no tooltip should be used) for this window. This is currently only
        // used by wxCompositeWindow::DoSetToolTip() implementation and is not part
        // of the public wx API.
        //
        // Returns true if tip was valid and we copied it or false if it was NULL
        // and we reset our own tooltip too.
        public static bool CopyToolTip(IntPtr handle, IntPtr tip) => default;

        // set/retrieve the drop target associated with this window (may be
        // NULL; it's owned by the window and will be deleted by it)
        public static void SetDropTarget(IntPtr handle, IntPtr dropTarget) { }
        public static IntPtr GetDropTarget(IntPtr handle) => default;

        // Accept files for dragging
        public static void DragAcceptFiles(IntPtr handle, bool accept) { }

        // set theraints for this window or retrieve them (may be NULL)
        public static void SetConstraints(IntPtr handle, IntPtr constraints) { }
        public static IntPtr GetConstraints(IntPtr handle) => default;

        // implementation only
        public static void UnsetConstraints(IntPtr handle, IntPtr c) { }
        public static IntPtr GetConstraintsInvolvedIn(IntPtr handle) => default;
        public static void AddConstraintReference(IntPtr handle, IntPtr otherWin) { }
        public static void RemoveConstraintReference(IntPtr handle, IntPtr otherWin) { }
        public static void DeleteRelatedConstraints(IntPtr handle) { }
        public static void ResetConstraints(IntPtr handle) { }

        // these methods may be overridden for special layout algorithms
        public static void SetConstraintSizes(IntPtr handle, bool recurse = true) { }
        // public static bool LayoutPhase1(int* noChanges) => default;
        // public static bool LayoutPhase2(int* noChanges) => default;
        public static bool DoPhase(IntPtr handle, int phase) => default;

        // these methods are but normally won't be overridden
        public static void SetSizeConstraint(IntPtr handle, int x, int y, int w, int h) { }
        public static void MoveConstraint(IntPtr handle, int x, int y) { }
        public static SizeI GetSizeConstraint(IntPtr handle) => default;
        public static SizeI GetClientSizeConstraint(IntPtr handle) => default;
        public static SizeI GetPositionConstraint(IntPtr handle) => default;

        // when usingraints or sizers, it makes sense to update
        // children positions automatically whenever the window is resized
        // - this is done if autoLayout is on
        public static void SetAutoLayout(IntPtr handle, bool autoLayout) { }
        public static bool GetAutoLayout(IntPtr handle) => default;

        // lay out the window and its children
        public static bool Layout(IntPtr handle) => default;

        // sizers
        public static void SetSizer(IntPtr handle, IntPtr sizer, bool deleteOld = true) { }
        public static void SetSizerAndFit(IntPtr handle, IntPtr sizer, bool deleteOld = true) { }

        public static IntPtr GetSizer(IntPtr handle) => default;

        // Track if this window is a member of a sizer
        public static void SetContainingSizer(IntPtr handle, IntPtr sizer) { }
        public static IntPtr GetContainingSizer(IntPtr handle) => default;

        // Override to create a specific accessible object.
        public static IntPtr CreateAccessible(IntPtr handle) => default;

        // Sets the accessible object.
        public static void SetAccessible(IntPtr handle, IntPtr accessible) { }

        // Returns the accessible object.
        public static IntPtr GetAccessible(IntPtr handle) => default;

        // Returns the accessible object, calling CreateAccessible if necessary.
        // May return NULL, in which case system-provide accessible is used.
        public static IntPtr GetOrCreateAccessible(IntPtr handle) => default;

        // Set window transparency if the platform supports it
        public static bool SetTransparent(IntPtr handle, byte alpha) => default;
        public static bool CanSetTransparent(IntPtr handle) => default;

        // get the handle of the window for the underlying window system: this
        // is only used for wxWin itself or for user code which wants to call
        // platform-specific APIs
        public static IntPtr GetHandle(IntPtr handle) => default;

        // inherit the parents visual attributes if they had been explicitly set
        // by the user (i.e. we don't inherit default attributes) and if we don't
        // have our own explicitly set
        public static void InheritAttributes(IntPtr handle) { }

        // returns false from here if this window doesn't want to inherit the
        // parents Colors even if InheritAttributes() would normally do it
        // this just provides a simple way to customize InheritAttributes(IntPtr handle)
        // behaviour in the most common case
        public static bool ShouldInheritColors(IntPtr handle) => default;

        // returns true if the window can be positioned outside of parent's client
        // area (normal windows can't, but e.g. menubar or statusbar can):
        public static bool CanBeOutsideClientArea(IntPtr handle) => default;

        // returns true if the platform should explicitly apply a theme border. Currently
        // used only by Windows
        public static bool CanApplyThemeBorder(IntPtr handle) => default;

        // returns the main window of composite control; this is the window
        // that FindFocus returns if the focus is in one of composite control's
        // windows
        public static IntPtr GetMainWindowOfCompositeControl(IntPtr handle) => default;

        /*enum NavigationKind
        {
            Navigation_Tab,
            Navigation_Accel
        };*/

        // If this function returns true, keyboard events of the given kind can't
        // escape from it. A typical example of such "navigation domain" is a top
        // level window because pressing TAB in one of them must not transfer focus
        // to a different top level window. But it's not limited to them, e.g. MDI
        // children frames are not top level windows (and their IsTopLevel()
        // returns false) but still are self-contained navigation domains for the
        // purposes of TAB navigation -- but not for the accelerators.
        public static bool IsTopNavigationDomain(IntPtr handle,
            /*NavigationKind*/ int kind) => default;

        // This is an internal helper function implemented by text-like controls.
        public static IntPtr WXGetTextEntry(IntPtr handle) => default;
    }
}

/*
         // do the window-specific processing after processing the update event
        void DoUpdateWindowUI(wxUpdateUIEvent& event);

 
  
         // get the width/height/... of the text using current or specified font
        void GetTextExtent(string str,
                           int* x, int* y,
                           int* descent = NULL,
                           int* externalLeading = NULL,
                           IntPtr font = NULL) { }

// get the default attributes for the controls of this class: we
// provide a function which can be used to query the default
// attributes of an existing control and a static function which can
// be used even when no existing object of the given class is
// available, but which won't return any styles specific to this
// particular control, of course (e.g. "Ok" button might have
// different -- bold for example -- font)
wxVisualAttributes GetDefaultAttributes(IntPtr handle) => default;
        wxVisualAttributes GetClassDefaultAttributes(
            wxWindowVariant variant = wxWINDOW_VARIANT_NORMAL) => default;;//real static 

 
         // sets the window variant, calls internally DoSetVariant if variant
        // has changed
    void SetWindowVariant(wxWindowVariant variant){}
    wxWindowVariant GetWindowVariant(IntPtr handle) => default;


 */