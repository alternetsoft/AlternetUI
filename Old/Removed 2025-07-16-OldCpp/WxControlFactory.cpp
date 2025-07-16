#include "WxControlFactory.h"

namespace Alternet::UI
{
	WxControlFactory::WxControlFactory()
	{

	}

	WxControlFactory::~WxControlFactory()
	{
	}

	void WxControlFactory::SetConstraintSizes(void* handle, bool recurse) { }
	
	bool WxControlFactory::DoPhase(void* handle, int phase) { return false;}
	
	void WxControlFactory::SetSizeConstraint(void* handle, int x, int y,
		int w, int h) { }
	
	void WxControlFactory::MoveConstraint(void* handle, int x, int y) { }
	
	Int32Size WxControlFactory::GetSizeConstraint(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetClientSizeConstraint(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetPositionConstraint(void* handle) { return Int32Size();}
	
	void WxControlFactory::SetAutoLayout(void* handle, bool autoLayout) { }
	
	bool WxControlFactory::GetAutoLayout(void* handle) { return false;}
	
	bool WxControlFactory::Layout(void* handle) { return false;}
	
	void WxControlFactory::SetSizer(void* handle, void* sizer, bool deleteOld) { return ;}
	
	void WxControlFactory::SetSizerAndFit(void* handle, void* sizer, bool deleteOld) { return ;}
	
	void* WxControlFactory::GetSizer(void* handle) { return nullptr;}
	
	void WxControlFactory::SetContainingSizer(void* handle, void* sizer) { return ;}
	
	void* WxControlFactory::GetContainingSizer(void* handle) { return nullptr;}
	
	void* WxControlFactory::CreateAccessible(void* handle) { return nullptr;}
	
	void WxControlFactory::SetAccessible(void* handle, void* accessible) { return ;}
	
	void* WxControlFactory::GetAccessible(void* handle) { return nullptr;}
	
	void* WxControlFactory::GetOrCreateAccessible(void* handle) { return nullptr;}
	
	bool WxControlFactory::SetTransparent(void* handle, uint8_t alpha) { return false;}
	
	bool WxControlFactory::CanSetTransparent(void* handle) { return false;}
	
	void* WxControlFactory::GetHandle(void* handle) { return nullptr;}
	
	void WxControlFactory::InheritAttributes(void* handle) { return ;}
	
	bool WxControlFactory::ShouldInheritColors(void* handle) { return false;}
	
	bool WxControlFactory::CanBeOutsideClientArea(void* handle) { return false;}
	
	bool WxControlFactory::CanApplyThemeBorder(void* handle) { return false;}
	
	void* WxControlFactory::GetMainWindowOfCompositeControl(void* handle) { return nullptr;}
	
	bool WxControlFactory::IsTopNavigationDomain(void* handle, int kind) { return false;}
	
	void* WxControlFactory::WXGetTextEntry(void* handle) { return nullptr;}
	
	void WxControlFactory::ReleaseMouse(void* handle) { return ;}
	
	void* WxControlFactory::GetCapture() { return nullptr;}
	
	bool WxControlFactory::HasCapture(void* handle) { return false;}
	
	bool WxControlFactory::EnableTouchEvents(void* handle, int WXUNUSEDeventsMask) { return false;}
	
	void WxControlFactory::RefreshRect(void* handle, const Int32Rect& rect,
		bool eraseBackground) { return ;}
	
	void WxControlFactory::Update(void* handle) { return ;}
	
	void WxControlFactory::ClearBackground(void* handle) { return ;}
	
	void WxControlFactory::Freeze(void* handle) { return ;}
	
	void WxControlFactory::Thaw(void* handle) { return ;}
	
	bool WxControlFactory::IsFrozen(void* handle) { return false;}
	
	void WxControlFactory::PrepareDC(void* handle, void* dc) { return ;}
	
	void WxControlFactory::SetDoubleBuffered(void* handle, bool on) { return ;}
	
	bool WxControlFactory::IsDoubleBuffered(void* handle) { return false;}
	
	void* WxControlFactory::GetUpdateRegion(void* handle) { return nullptr;}
	
	Int32Rect WxControlFactory::GetUpdateClientRect(void* handle) { return Int32Rect();}
	
	bool WxControlFactory::IsExposed(void* handle, int x, int y) { return false;}
	
	bool WxControlFactory::IsExposed2(void* handle, int x, int y, int w, int h) { return false;}
	
	bool WxControlFactory::SetBackgroundColor(void* handle, const Color& Color) { return false;}
	
	void WxControlFactory::SetOwnBackgroundColor(void* handle, const Color& Color) { }
	
	Color WxControlFactory::GetBackgroundColor(void* handle) { return Color();}
	
	bool WxControlFactory::InheritsBackgroundColor(void* handle) { return false;}
	
	bool WxControlFactory::UseBackgroundColor(void* handle) { return false;}
	
	bool WxControlFactory::SetForegroundColor(void* handle, const Color& Color) { return false;}
	
	void WxControlFactory::SetOwnForegroundColor(void* handle, const Color& Color) { return ;}
	
	Color WxControlFactory::GetForegroundColor(void* handle) { return Color();}
	
	bool WxControlFactory::UseForegroundColor(void* handle) { return false;}
	
	bool WxControlFactory::InheritsForegroundColor(void* handle) { return false;}
	
	bool WxControlFactory::SetBackgroundStyle(void* handle, int style) { return false;}
	
	int WxControlFactory::GetBackgroundStyle(void* handle) { return 0;}
	
	bool WxControlFactory::HasTransparentBackground(void* handle) { return false;}
	
	bool WxControlFactory::IsTransparentBackgroundSupported(void* handle,
		const string& reason) { return false;}
	
	bool WxControlFactory::SetFont(void* handle, void* font) { return false;}
	
	void WxControlFactory::SetOwnFont(void* handle, void* font) { return ;}
	
	void* WxControlFactory::GetFont(void* handle) { return nullptr;}
	
	bool WxControlFactory::SetCursor(void* handle, void* cursor) { return false;}
	
	void* WxControlFactory::GetCursor(void* handle) { return nullptr;}
	
	void WxControlFactory::SetCaret(void* handle, void* caret) { return ;}
	
	void* WxControlFactory::GetCaret(void* handle) { return nullptr;}
	
	int WxControlFactory::GetCharHeight(void* handle) { return 0;}
	
	int WxControlFactory::GetCharWidth(void* handle) { return 0;}
	
	Int32Size WxControlFactory::GetTextExtent(void* handle, const string& str) { return Int32Size();}
	
	Int32Point WxControlFactory::ClientToScreen(void* handle, const Int32Point& pt)
	{ return Int32Point();}
	
	Int32Point WxControlFactory::ScreenToClient(void* handle, const Int32Point& pt)
	{ return Int32Point();}
	
	int WxControlFactory::HitTest(void* handle, int x, int y) { return 0;}
	
	int WxControlFactory::GetBorderEx(void* handle, int64_t flags) { return 0;}
	
	int WxControlFactory::GetBorder(void* handle) { return 0;}
	
	void WxControlFactory::UpdateWindowUI(void* handle, int64_t flags) { return ;}
	
	bool WxControlFactory::PopupMenu(void* handle, void* menu, int x, int y) { return false;}
	
	int WxControlFactory::GetPopupMenuSelectionFromUser(void* handle, void* menu,
		int x, int y) { return 0;}
	
	bool WxControlFactory::HasMultiplePages(void* handle) { return false;}
	
	bool WxControlFactory::CanScroll(void* handle, int orient) { return false;}
	
	bool WxControlFactory::HasScrollbar(void* handle, int orient) { return false;}
	
	void WxControlFactory::SetScrollbar(void* handle, int orient, int pos, int thumbvisible,
		int range, bool refresh) { }
	
	void WxControlFactory::SetScrollPos(void* handle, int orient, int pos, bool refresh) { }
	
	int WxControlFactory::GetScrollPos(void* handle, int orient) { return 0;}
	
	int WxControlFactory::GetScrollThumb(void* handle, int orient) { return 0;}
	
	int WxControlFactory::GetScrollRange(void* handle, int orient) { return 0;}
	
	bool WxControlFactory::ScrollLines(void* handle, int lines) { return false;}
	
	bool WxControlFactory::ScrollPages(void* handle, int pages) { return false;}
	
	bool WxControlFactory::LineUp(void* handle) { return false;}
	
	bool WxControlFactory::LineDown(void* handle) { return false;}
	
	bool WxControlFactory::PageUp(void* handle) { return false;}
	
	bool WxControlFactory::PageDown(void* handle) { return false;}
	
	void WxControlFactory::AlwaysShowScrollbars(void* handle, bool horz, bool vert) { }
	
	bool WxControlFactory::IsScrollbarAlwaysShown(void* handle, int orient) { return false;}
	
	void WxControlFactory::SetHelpText(void* handle, const string& text) { }
	
	string WxControlFactory::GetHelpTextAtPoint(void* handle, const Int32Point& pt,
		int origin) { return wxStr(wxEmptyString);}
	
	string WxControlFactory::GetHelpText(void* handle) { return wxStr(wxEmptyString);}
	
	void WxControlFactory::SetToolTip(void* handle, const string& tip) { return ;}
	
	void WxControlFactory::SetToolTip2(void* handle, void* tip) { return ;}
	
	void WxControlFactory::UnsetToolTip(void* handle) { return ;}
	
	void* WxControlFactory::GetToolTip(void* handle) { return nullptr;}
	
	string WxControlFactory::GetToolTipText(void* handle) { return wxStr(wxEmptyString);}
	
	bool WxControlFactory::CopyToolTip(void* handle, void* tip) { return false;}
	
	void WxControlFactory::SetDropTarget(void* handle, void* dropTarget) { return ;}
	
	void* WxControlFactory::GetDropTarget(void* handle) { return nullptr;}
	
	void WxControlFactory::DragAcceptFiles(void* handle, bool accept) { return ;}
	
	void WxControlFactory::SetConstraints(void* handle, void* constraints) { return ;}
	
	void* WxControlFactory::GetConstraints(void* handle) { return nullptr;}
	
	void WxControlFactory::UnsetConstraints(void* handle, void* c) { return ;}
	
	void* WxControlFactory::GetConstraintsInvolvedIn(void* handle) { return nullptr;}
	
	void WxControlFactory::AddConstraintReference(void* handle, void* otherWin) { return ;}
	
	void WxControlFactory::RemoveConstraintReference(void* handle, void* otherWin) { return ;}
	
	void WxControlFactory::DeleteRelatedConstraints(void* handle) { return ;}
	
	void WxControlFactory::ResetConstraints(void* handle) { return ;}
	
	bool WxControlFactory::HasFlag(void* handle, int flag) { return false;}
	
	bool WxControlFactory::IsRetained(void* handle) { return false;}
	
	bool WxControlFactory::ToggleWindowStyle(void* handle, int flag) { return false;}
	
	void WxControlFactory::SetExtraStyle(void* handle, int64_t exStyle) { return ;}
	
	int64_t WxControlFactory::GetExtraStyle(void* handle) { return 0;}
	
	bool WxControlFactory::HasExtraStyle(void* handle, int exFlag) { return false;}
	
	void WxControlFactory::SetThemeEnabled(void* handle, bool enableTheme) { return ;}
	
	bool WxControlFactory::GetThemeEnabled(void* handle) { return false;}
	
	void WxControlFactory::SetFocus(void* handle) { return ;}
	
	void WxControlFactory::SetFocusFromKbd(void* handle) { return ;}
	
	void* WxControlFactory::FindFocus() { return nullptr;}
	
	bool WxControlFactory::HasFocus(void* handle) { return false;}
	
	bool WxControlFactory::AcceptsFocus(void* handle) { return false;}
	
	bool WxControlFactory::AcceptsFocusRecursively(void* handle) { return false;}
	
	bool WxControlFactory::AcceptsFocusFromKeyboard(void* handle) { return false;}
	
	void WxControlFactory::DisableFocusFromKeyboard(void* handle) { return ;}
	
	bool WxControlFactory::CanBeFocused(void* handle) { return false;}
	
	bool WxControlFactory::IsFocusable(void* handle) { return false;}
	
	bool WxControlFactory::CanAcceptFocus(void* handle) { return false;}
	
	bool WxControlFactory::CanAcceptFocusFromKeyboard(void* handle) { return false;}
	
	void WxControlFactory::SetCanFocus(void* handle, bool canFocus) { return ;}
	
	void WxControlFactory::EnableVisibleFocus(void* handle, bool enabled) { return ;}
	
	bool WxControlFactory::NavigateIn(void* handle, int flags) { return false;}
	
	bool WxControlFactory::Navigate(void* handle, int flags) { return false;}
	
	void WxControlFactory::MoveBeforeInTabOrder(void* handle, void* win) { return ;}
	
	void WxControlFactory::MoveAfterInTabOrder(void* handle, void* win) { return ;}
	
	void* WxControlFactory::GetChildren(void* handle) { return nullptr ;}
	
	void* WxControlFactory::GetPrevSibling(void* handle) { return nullptr;}
	
	void* WxControlFactory::GetNextSibling(void* handle) { return nullptr;}
	
	void* WxControlFactory::GetParent(void* handle) { return nullptr;}
	
	void* WxControlFactory::GetGrandParent(void* handle) { return nullptr;}
	
	bool WxControlFactory::IsTopLevel(void* handle) { return false;}
	
	bool WxControlFactory::IsDescendant(void* handle, void* win) { return false;}
	
	void WxControlFactory::SetParent(void* handle, void* parent) { return ;}
	
	bool WxControlFactory::Reparent(void* handle, void* newParent) { return false;}
	
	void WxControlFactory::AddChild(void* handle, void* child) { return ;}
	
	void WxControlFactory::RemoveChild(void* handle, void* child) { return ;}
	
	bool WxControlFactory::IsClientAreaChild(void* handle, void* child) { return false;}
	
	void* WxControlFactory::FindWindow(void* handle, int64_t winid) { return nullptr;}
	
	void* WxControlFactory::FindWindow2(void* handle, const string& name) { return nullptr;}
	
	void* WxControlFactory::FindWindowById(int64_t winid, void* parent) { return nullptr;}
	
	void* WxControlFactory::FindWindowByName(const string& name, void* parent) { return nullptr;}
	
	void* WxControlFactory::FindWindowByLabel(const string& label, void* parent) { return nullptr;}
	
	void WxControlFactory::SetValidator(void* handle, void* validator) { return ;}
	
	void* WxControlFactory::GetValidator(void* handle) { return nullptr;}
	
	bool WxControlFactory::Validate(void* handle) { return false;}
	
	bool WxControlFactory::TransferDataToWindow(void* handle) { return false;}
	
	bool WxControlFactory::TransferDataFromWindow(void* handle) { return false;}
	
	void WxControlFactory::InitDialog(void* handle) { return ;}
	
	void WxControlFactory::SetAcceleratorTable(void* handle, void* accel) { return ;}
	
	void* WxControlFactory::GetAcceleratorTable(void* handle) { return nullptr;}
	
	Int32Size WxControlFactory::GetDPI(void* handle) { return Int32Size();}
	
	void WxControlFactory::WXAdjustFontToOwnPPI(void* handle, void* font) { return ;}
	
	Int32Size WxControlFactory::FromPhys(const Int32Size& sz, void* w) { return Int32Size();}
	
	Int32Point WxControlFactory::FromPhys2(const Int32Point& pt, void* w) { return Int32Point();}
	
	int WxControlFactory::FromPhys3(int d, void* w) { return 0;}
	
	Int32Size WxControlFactory::FromPhys4(const Int32Size& sz) { return Int32Size();}
	
	Int32Point WxControlFactory::FromPhys5(const Int32Point& pt) { return Int32Point();}
	
	int WxControlFactory::FromPhys6(int d) { return 0;}
	
	Int32Size WxControlFactory::ToPhys(const Int32Size& sz, void* w) { return Int32Size();}
	
	Int32Point WxControlFactory::ToPhys2(const Int32Point& pt, void* w) { return Int32Point();}
	
	int WxControlFactory::ToPhys3(int d, void* w) { return 0;}
	
	Int32Size WxControlFactory::ToPhys4(const Int32Size& sz) { return Int32Size();}
	
	Int32Point WxControlFactory::ToPhys5(const Int32Point& pt) { return Int32Point();}
	
	int WxControlFactory::ToPhys6(int d) { return 0;}
	
	Int32Size WxControlFactory::FromDIP(const Int32Size& sz, void* window) { return Int32Size();}
	
	Int32Point WxControlFactory::FromDIP2(const Int32Point& pt, void* window) { return Int32Point();}
	
	int WxControlFactory::FromDIP3(int d, void* w) { return 0;}
	
	Int32Size WxControlFactory::FromDIP4(void* handle, const Int32Size& sz) { return Int32Size();}
	
	Int32Point WxControlFactory::FromDIP5(void* handle, const Int32Point& pt) { return Int32Point();}
	
	int WxControlFactory::FromDIP6(void* handle, int d) { return 0;}
	
	Int32Size WxControlFactory::ToDIP(const Int32Size& sz, void* w) { return Int32Size();}
	
	Int32Point WxControlFactory::ToDIP2(const Int32Point& pt, void* w) { return Int32Point();}
	
	int WxControlFactory::ToDIP3(int d, void* w) { return 0;}
	
	Int32Size WxControlFactory::ToDIP4(void* handle, const Int32Size& sz) { return Int32Size();}
	
	Int32Point WxControlFactory::ToDIP5(void* handle, const Int32Point& pt) { return Int32Point();}
	
	int WxControlFactory::ToDIP6(int d) { return 0;}
	
	Int32Point WxControlFactory::ConvertPixelsToDialog(void* handle,
		const Int32Point& pt) { return Int32Point();}
	
	Int32Point WxControlFactory::ConvertDialogToPixels(void* handle,
		const Int32Point& pt) { return Int32Point();}
	
	Int32Size WxControlFactory::ConvertPixelsToDialog2(void* handle,
		const Int32Size& sz) { return Int32Size();}
	
	Int32Size WxControlFactory::ConvertDialogToPixels2(void* handle,
		const Int32Size& sz) { return Int32Size();}
	
	void WxControlFactory::WarpPointer(void* handle, int x, int y) { return ;}
	
	void WxControlFactory::CaptureMouse(void* handle) { return ;}
	
	bool WxControlFactory::Close(void* handle, bool force) { return false;}
	
	bool WxControlFactory::DestroyWindow(void* handle) { return false;}
	
	bool WxControlFactory::DestroyChildren(void* handle) { return false;}
	
	bool WxControlFactory::IsBeingDeleted(void* handle) { return false;}
	
	void WxControlFactory::SetLabel(void* handle, const string& label) { return ;}
	
	string WxControlFactory::GetLabel(void* handle) { return wxStr(wxEmptyString);}
	
	void WxControlFactory::SetName(void* handle, const string& name) { return ;}
	
	string WxControlFactory::GetName(void* handle) { return wxStr(wxEmptyString);}
	
	int WxControlFactory::GetLayoutDirection(void* handle) { return 0;}
	
	void WxControlFactory::SetLayoutDirection(void* handle, int dir) { return ;}
	
	int WxControlFactory::AdjustForLayoutDirection(void* handle, int x,
		int width, int widthTotal) { return 0;}
	
	void WxControlFactory::SetId(void* handle, int winid) { return ;}
	
	int WxControlFactory::GetId(void* handle) { return 0;}
	
	int WxControlFactory::NewControlId(int count) { return 0;}
	
	void WxControlFactory::UnreserveControlId(int id, int count) { return ;}
	
	void WxControlFactory::SetSize2(void* handle, int x, int y, int width, int height) { return ;}
	
	void WxControlFactory::SetSize(void* handle, int width, int height) { return ;}
	
	void WxControlFactory::Move(void* handle, int x, int y, int flags) { return ;}
	
	void WxControlFactory::SetPosition(void* handle, int width, int height) { return ;}
	
	void WxControlFactory::Raise(void* handle) { return ;}
	
	void WxControlFactory::Lower(void* handle) { return ;}
	
	void WxControlFactory::SetClientSize(void* handle, int width, int height) { return ;}
	
	Int32Point WxControlFactory::GetPosition(void* handle) { return Int32Point();}
	
	Int32Point WxControlFactory::GetScreenPosition(void* handle) { return Int32Point();}
	
	Int32Size WxControlFactory::GetSize(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetClientSize(void* handle) { return Int32Size();}
	
	Int32Rect WxControlFactory::GetRect(void* handle) { return Int32Rect();}
	
	Int32Rect WxControlFactory::GetScreenRect(void* handle) { return Int32Rect();}
	
	Int32Point WxControlFactory::GetClientAreaOrigin(void* handle) { return Int32Point();}
	
	Int32Rect WxControlFactory::GetClientRect(void* handle) { return Int32Rect();}
	
	Int32Size WxControlFactory::ClientToWindowSize(void* handle, const Int32Size& size)
	{ return Int32Size();}
	
	Int32Size WxControlFactory::WindowToClientSize(void* handle, const Int32Size& size)
	{ return Int32Size();}

	Int32Size WxControlFactory::GetBestSize(void* handle) { return Int32Size();}
	
	int WxControlFactory::GetBestHeight(void* handle, int width) { return 0;}
	
	int WxControlFactory::GetBestWidth(void* handle, int height) { return 0;}
	
	void WxControlFactory::SetScrollHelper(void* handle, void* sh) { return ;}
	
	void* WxControlFactory::GetScrollHelper(void* handle) { return nullptr;}
	
	void WxControlFactory::InvalidateBestSize(void* handle) { return ;}
	
	void WxControlFactory::CacheBestSize(void* handle, const Int32Size& size) { return ;}
	
	Int32Size WxControlFactory::GetEffectiveMinSize(void* handle) { return Int32Size();}
	
	void WxControlFactory::SetInitialSize(void* handle, const Int32Size& size) { return ;}
	
	void WxControlFactory::Center(void* handle, int dir) { return ;}
	
	void WxControlFactory::CenterOnParent(void* handle, int dir) { return ;}
	
	void WxControlFactory::Fit(void* handle) { return ;}
	
	void WxControlFactory::FitInside(void* handle) { return ;}
	
	void WxControlFactory::SetSizeHints(void* handle, int minW, int minH, int maxW, int maxH,
		int incW, int incH) { return ;}
	
	void WxControlFactory::SetMinSize(void* handle, const Int32Size& minSize) { return ;}
	
	void WxControlFactory::SetMaxSize(void* handle, const Int32Size& maxSize) { return ;}
	
	void WxControlFactory::SetMinClientSize(void* handle, const Int32Size& size) { return ;}
	
	void WxControlFactory::SetMaxClientSize(void* handle, const Int32Size& size) { return ;}
	
	Int32Size WxControlFactory::GetMinSize(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetMaxSize(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetMinClientSize(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetMaxClientSize(void* handle) { return Int32Size();}
	
	int WxControlFactory::GetMinWidth(void* handle) { return 0;}
	
	int WxControlFactory::GetMinHeight(void* handle) { return 0;}
	
	int WxControlFactory::GetMaxWidth(void* handle) { return 0;}
	
	int WxControlFactory::GetMaxHeight(void* handle) { return 0;}
	
	void WxControlFactory::SetVirtualSize(void* handle, int x, int y) { return ;}
	
	Int32Size WxControlFactory::GetVirtualSize(void* handle) { return Int32Size();}
	
	Int32Size WxControlFactory::GetBestVirtualSize(void* handle) { return Int32Size();}
	
	double WxControlFactory::GetContentScaleFactor(void* handle) { return 0;}
	
	double WxControlFactory::GetDPIScaleFactor(void* handle) { return 0;}
	
	Int32Size WxControlFactory::GetWindowBorderSize(void* handle) { return Int32Size();}
	
	bool WxControlFactory::InformFirstDirection(void* handle, int direction, int size,
		int availableOtherDir) { return false;}
	
	void WxControlFactory::SendSizeEvent(void* handle, int flags) { return ;}
	
	void WxControlFactory::SendSizeEventToParent(void* handle, int flags) { return ;}
	
	void WxControlFactory::PostSizeEvent(void* handle) { return ;}
	
	void WxControlFactory::PostSizeEventToParent(void* handle) { return ;}
	
	bool WxControlFactory::BeginRepositioningChildren(void* handle) { return false;}
	
	void WxControlFactory::EndRepositioningChildren(void* handle) { return ;}
	
	bool WxControlFactory::Show(void* handle, bool show) { return false;}
	
	bool WxControlFactory::Hide(void* handle) { return false;}
	
	bool WxControlFactory::ShowWithEffect(void* handle, int effect, uint32_t timeout) { return false;}
	
	bool WxControlFactory::HideWithEffect(void* handle, int effect, uint32_t timeout) { return false;}
	
	bool WxControlFactory::Enable(void* handle, bool enable) { return false;}
	
	bool WxControlFactory::Disable(void* handle) { return false;}
	
	bool WxControlFactory::IsShown(void* handle) { return false;}
	
	bool WxControlFactory::IsEnabled(void* handle) { return false;}
	
	bool WxControlFactory::IsThisEnabled(void* handle) { return false;}
	
	bool WxControlFactory::IsShownOnScreen(void* handle) { return false;}
	
	void WxControlFactory::SetWindowStyleFlag(void* handle, int64_t style) { return ;}
	
	int64_t WxControlFactory::GetWindowStyleFlag(void* handle) { return 0;}
	
	void WxControlFactory::SetWindowStyle(void* handle, int64_t style) { }
	
	int64_t WxControlFactory::GetWindowStyle(void* handle) { return 0;}
}
