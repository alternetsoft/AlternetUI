using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Dockable toolbar, managed by <see cref="AuiManager"/>.
    /// </summary>
    public class AuiToolbar : Control
    {
        private int idCounter = 0;

        public event EventHandler? ToolDropDown;

        public event EventHandler? BeginDrag;

        public event EventHandler? ToolMiddleClick;

        public event EventHandler? OverflowClick;

        public event EventHandler? ToolRightClick;

        public int EventToolId
        {
            get
            {
                return NativeControl.EventToolId;
            }
        }

        protected virtual void OnToolDropDown(EventArgs e)
        {
        }

        internal void RaiseToolDropDown(EventArgs e)
        {
            OnToolDropDown(e);
            ToolDropDown?.Invoke(this, e);
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.AuiToolbar;

        internal new NativeAuiToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeAuiToolbarHandler)base.Handler;
            }
        }

        internal Native.AuiToolBar NativeControl => Handler.NativeControl;

        public int AddTool(
            string label,
            ImageSet? bitmapBundle,
            string? shortHelpString,
            AuiToolbarItemKind itemKind = AuiToolbarItemKind.Normal)
        {
            int toolId = GenNewId();
            NativeControl.AddTool(
                toolId,
                label,
                bitmapBundle?.NativeImageSet,
                shortHelpString!,
                (int)itemKind);
            return toolId;
        }

        public int AddTool(
            string label,
            ImageSet? bitmapBundle,
            ImageSet? disabledBitmapBundle,
            AuiToolbarItemKind itemKind,
            string? shortHelpString,
            string? longHelpString,
            IntPtr clientData)
        {
            int toolId = GenNewId();
            NativeControl.AddTool2(
                toolId,
                label,
                bitmapBundle?.NativeImageSet,
                disabledBitmapBundle?.NativeImageSet,
                (int)itemKind,
                shortHelpString!,
                longHelpString!,
                clientData);
            return toolId;
        }

        public int AddTool(
            ImageSet? bitmapBundle,
            ImageSet? disabledBitmapBundle,
            bool toggle,
            IntPtr clientData,
            string? shortHelpString,
            string? longHelpString)
        {
            int toolId = GenNewId();
            NativeControl.AddTool3(
                toolId,
                bitmapBundle?.NativeImageSet,
                disabledBitmapBundle?.NativeImageSet,
                toggle,
                clientData,
                shortHelpString!,
                longHelpString!);
            return toolId;
        }

        public AuiToolbarItemKind GetToolKind(int toolId)
        {
            var result = NativeControl.GetToolKind(toolId);
            return (AuiToolbarItemKind)result;
        }

        public int AddLabel(string label, int width = -1)
        {
            int toolId = GenNewId();
            NativeControl.AddLabel(toolId, label, width);
            return toolId;
        }

        public void AddControl(Control control, string label)
        {
            NativeControl.AddControl(control.WxWidget, label);
        }

        public void AddSeparator()
        {
            NativeControl.AddSeparator();
        }

        public void AddSpacer(int pixels)
        {
            NativeControl.AddSpacer(pixels);
        }

        public void AddStretchSpacer(int proportion = 1)
        {
            NativeControl.AddStretchSpacer(proportion);
        }

        public bool Realize()
        {
            return NativeControl.Realize();
        }

        public void Clear()
        {
            NativeControl.Clear();
        }

        /*
Destroys the tool with the given ID and its associated window, if any.
toolId	ID of a previously added tool.
true if the tool was destroyed or false otherwise, e.g. if the tool with the given ID was not found.
         */
        public bool DestroyTool(int toolId)
        {
            return NativeControl.DestroyTool(toolId);
        }

        /*
Destroys the tool at the given position and its associated window, if any.
idx	The index, or position, of a previously added tool.
Returns
true if the tool was destroyed or false otherwise, e.g. if the provided index is out of range.
         */
        public bool DestroyToolByIndex(int idx)
        {
            return NativeControl.DestroyToolByIndex(idx);
        }

        // Note that these methods do _not_ delete the associated control, if any.
        // Use DestroyTool() or DestroyToolByIndex() if this is wanted.
        /*
         Removes the tool with the given ID from the toolbar.
        Note that if this tool was added by AddControl(), the associated control is not deleted and must either be reused (e.g. by reparenting it under a different window) or destroyed by caller. If this behaviour is unwanted, prefer using DestroyTool() instead.
        Parameters
        toolId	ID of a previously added tool.
        true if the tool was removed or false otherwise, e.g. if the tool with the given ID was not found.
         */
        public bool DeleteTool(int toolId)
        {
            return NativeControl.DeleteTool(toolId);
        }

        /*
Removes the tool at the given position from the toolbar.
Note that if this tool was added by AddControl(), the associated control is not deleted and must either be reused (e.g. by reparenting it under a different window) or destroyed by caller. If this behaviour is unwanted, prefer using DestroyToolByIndex() instead.
idx	The index, or position, of a previously added tool.
true if the tool was removed or false otherwise, e.g. if the provided index is out of range.
         */
        public bool DeleteByIndex(int index)
        {
            return NativeControl.DeleteByIndex(index);
        }

        public int GetToolIndex(int toolId)
        {
            return NativeControl.GetToolIndex(toolId);
        }

        public bool GetToolFits(int toolId)
        {
            return NativeControl.GetToolFits(toolId);
        }

        public Rect GetToolRect(int toolId)
        {
            return NativeControl.GetToolRect(toolId);
        }

        public bool GetToolFitsByIndex(int toolId)
        {
            return NativeControl.GetToolFitsByIndex(toolId);
        }

        public bool GetToolBarFits()
        {
            return NativeControl.GetToolBarFits();
        }

        internal void SetToolBitmapSize(Size size)
        {
            NativeControl.SetToolBitmapSize(size);
        }

        internal Size GetToolBitmapSize()
        {
            return NativeControl.GetToolBitmapSize();
        }

        internal bool GetOverflowVisible()
        {
            return NativeControl.GetOverflowVisible();
        }

        internal void SetOverflowVisible(bool visible)
        {
            NativeControl.SetOverflowVisible(visible);
        }

        internal bool GetGripperVisible()
        {
            return NativeControl.GetGripperVisible();
        }

        internal void SetGripperVisible(bool visible)
        {
            NativeControl.SetGripperVisible(visible);
        }

        public void ToggleTool(int toolId, bool state)
        {
            NativeControl.ToggleTool(toolId, state);
        }

        public bool GetToolToggled(int toolId)
        {
            return NativeControl.GetToolToggled(toolId);
        }

        public void SetMargins(int left, int right, int top, int bottom)
        {
            NativeControl.SetMargins(left, right, top, bottom);
        }

        public void EnableTool(int toolId, bool state)
        {
            NativeControl.EnableTool(toolId, state);
        }

        public bool GetToolEnabled(int toolId)
        {
            return NativeControl.GetToolEnabled(toolId);
        }

        /*
        Set whether the specified toolbar item has a drop down button.
        This is only valid for wxITEM_NORMAL tools.
         */
        public void SetToolDropDown(int toolId, bool dropdown)
        {
            NativeControl.SetToolDropDown(toolId, dropdown);
        }

        /*
         * Returns whether the specified toolbar item has an associated drop down button.

         */
        public bool GetToolDropDown(int toolId)
        {
            return NativeControl.GetToolDropDown(toolId);
        }

        internal void SetToolBorderPadding(int padding)
        {
            NativeControl.SetToolBorderPadding(padding);
        }

        internal int GetToolBorderPadding()
        {
            return NativeControl.GetToolBorderPadding();
        }

        internal void SetToolTextOrientation(AuiToolbarTextOrientation orientation)
        {
            NativeControl.SetToolTextOrientation((int)orientation);
        }

        internal AuiToolbarTextOrientation GetToolTextOrientation()
        {
            return (AuiToolbarTextOrientation)NativeControl.GetToolTextOrientation();
        }

        internal void SetToolPacking(int packing)
        {
            NativeControl.SetToolPacking(packing);
        }

        internal int GetToolPacking()
        {
            return NativeControl.GetToolPacking();
        }

        public void SetToolProportion(int toolId, int proportion)
        {
            NativeControl.SetToolProportion(toolId, proportion);
        }

        public int GetToolProportion(int toolId)
        {
            return NativeControl.GetToolProportion(toolId);
        }

        internal void SetToolSeparation(int separation)
        {
            NativeControl.SetToolSeparation(separation);
        }

        internal int GetToolSeparation()
        {
            return NativeControl.GetToolSeparation();
        }

        public void SetToolSticky(int toolId, bool sticky)
        {
            NativeControl.SetToolSticky(toolId, sticky);
        }

        public bool GetToolSticky(int toolId)
        {
            return NativeControl.GetToolSticky(toolId);
        }

        public string GetToolLabel(int toolId)
        {
            return NativeControl.GetToolLabel(toolId);
        }

        public void SetToolLabel(int toolId, string label)
        {
            NativeControl.SetToolLabel(toolId, label);
        }

        public void SetToolBitmap(int toolId, ImageSet? bitmapBundle)
        {
            NativeControl.SetToolBitmap(toolId, bitmapBundle?.NativeImageSet);
        }

        public string GetToolShortHelp(int toolId)
        {
            return NativeControl.GetToolShortHelp(toolId);
        }

        public void SetToolShortHelp(int toolId, string helpString)
        {
            NativeControl.SetToolShortHelp(toolId, helpString);
        }

        public string GetToolLongHelp(int toolId)
        {
            return NativeControl.GetToolLongHelp(toolId);
        }

        public void SetToolLongHelp(int toolId, string helpString)
        {
            NativeControl.SetToolLongHelp(toolId, helpString);
        }

        public ulong GetToolCount()
        {
            return NativeControl.GetToolCount();
        }

        internal IntPtr FindControl(int windowId)
        {
            return NativeControl.FindControl(windowId);
        }

        internal IntPtr FindToolByPosition(int x, int y)
        {
            return NativeControl.FindToolByPosition(x, y);
        }

        internal IntPtr FindToolByIndex(int idx)
        {
            return NativeControl.FindToolByIndex(idx);
        }

        internal IntPtr FindTool(int toolId)
        {
            return NativeControl.FindTool(toolId);
        }

        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return GetEffectiveControlHandlerHactory().CreateAuiToolbarHandler(this);
        }

        private int GenNewId()
        {
            int result = idCounter;
            idCounter++;
            return result;
        }
    }
}

/*

wxToolBarToolBase* wxToolBar::AddCheckTool	(	int 	toolId,
const wxString & 	label,
const wxBitmap & 	bitmap1,
const wxBitmap & 	bmpDisabled = wxNullBitmap,
const wxString & 	shortHelp = wxEmptyString,
const wxString & 	longHelp = wxEmptyString,
wxObject * 	clientData = NULL 
)		
Adds a new check (or toggle) tool to the toolbar.

The parameters are the same as in AddTool().
==
virtual wxToolBarToolBase* wxToolBar::AddControl	(	wxControl * 	control,
const wxString & 	label = wxEmptyString 
)		
virtual
Adds any control to the toolbar, typically e.g. a wxComboBox.

Parameters
control	The control to be added.
label	Text to be displayed near the control.
Remarks
wxMSW: the label is only displayed if there is enough space available below the embedded control.
wxMac: labels are only displayed if wxWidgets is built with wxMAC_USE_NATIVE_TOOLBAR set to 1
==
wxToolBarToolBase* wxToolBar::AddRadioTool	(	int 	toolId,
const wxString & 	label,
const wxBitmap & 	bitmap1,
const wxBitmap & 	bmpDisabled = wxNullBitmap,
const wxString & 	shortHelp = wxEmptyString,
const wxString & 	longHelp = wxEmptyString,
wxObject * 	clientData = NULL 
)		
Adds a new radio tool to the toolbar.

Consecutive radio tools form a radio group such that exactly one button in the group is pressed at any moment, in other words whenever a button in the group is pressed the previously pressed button is automatically released. You should avoid having the radio groups of only one element as it would be impossible for the user to use such button.

By default, the first button in the radio group is initially pressed, the others are not.

See also
AddTool()
==
virtual wxToolBarToolBase* wxToolBar::AddSeparator	(		)	
virtual
Adds a separator for spacing groups of tools.

Notice that the separator uses the look appropriate for the current platform so it can be a vertical line (MSW, some versions of GTK) or just an empty space or something else.

See also
AddTool(), SetToolSeparation(), AddStretchableSpace()
wxToolBarToolBase* wxToolBar::AddStretchableSpace	(		)	
Adds a stretchable space to the toolbar.

Any space not taken up by the fixed items (all items except for stretchable spaces) is distributed in equal measure between the stretchable spaces in the toolbar. The most common use for this method is to add a single stretchable space before the items which should be right-aligned in the toolbar, but more exotic possibilities are possible, e.g. a stretchable space may be added in the beginning and the end of the toolbar to centre all toolbar items.

See also
AddTool(), AddSeparator(), InsertStretchableSpace()
Since
2.9.1
==
virtual wxToolBarToolBase* wxToolBar::AddTool	(	wxToolBarToolBase * 	tool	)	
virtual
Adds a tool to the toolbar.

Parameters
tool	The tool to be added.
Remarks
After you have added tools to a toolbar, you must call Realize() in order to have the tools appear.
See also
AddSeparator(), AddCheckTool(), AddRadioTool(), InsertTool(), DeleteTool(), Realize(), SetDropdownMenu()
==
wxToolBarToolBase* wxToolBar::AddTool	(	int 	toolId,
const wxString & 	label,
const wxBitmap & 	bitmap,
const wxString & 	shortHelp = wxEmptyString,
wxItemKind 	kind = wxITEM_NORMAL 
)		
Adds a tool to the toolbar.

This most commonly used version has fewer parameters than the full version below which specifies the more rarely used button features.

Parameters
toolId	An integer by which the tool may be identified in subsequent operations.
label	The string to be displayed with the tool.
bitmap	The primary tool bitmap.
shortHelp	This string is used for the tools tooltip.
kind	May be wxITEM_NORMAL for a normal button (default), wxITEM_CHECK for a checkable tool (such tool stays pressed after it had been toggled) or wxITEM_RADIO for a checkable tool which makes part of a radio group of tools each of which is automatically unchecked whenever another button in the group is checked. wxITEM_DROPDOWN specifies that a drop-down menu button will appear next to the tool button (only GTK+ and MSW). Call SetDropdownMenu() afterwards.
Remarks
After you have added tools to a toolbar, you must call Realize() in order to have the tools appear.
See also
AddSeparator(), AddCheckTool(), AddRadioTool(), InsertTool(), DeleteTool(), Realize(), SetDropdownMenu()
==
wxToolBarToolBase* wxToolBar::AddTool	(	int 	toolId,
const wxString & 	label,
const wxBitmap & 	bitmap,
const wxBitmap & 	bmpDisabled,
wxItemKind 	kind = wxITEM_NORMAL,
const wxString & 	shortHelpString = wxEmptyString,
const wxString & 	longHelpString = wxEmptyString,
wxObject * 	clientData = NULL 
)		
Adds a tool to the toolbar.

Parameters
toolId	An integer by which the tool may be identified in subsequent operations.
label	The string to be displayed with the tool.
bitmap	The primary tool bitmap.
bmpDisabled	The bitmap used when the tool is disabled. If it is equal to wxNullBitmap (default), the disabled bitmap is automatically generated by greying the normal one.
kind	May be wxITEM_NORMAL for a normal button (default), wxITEM_CHECK for a checkable tool (such tool stays pressed after it had been toggled) or wxITEM_RADIO for a checkable tool which makes part of a radio group of tools each of which is automatically unchecked whenever another button in the group is checked. wxITEM_DROPDOWN specifies that a drop-down menu button will appear next to the tool button (only GTK+ and MSW). Call SetDropdownMenu() afterwards.
shortHelpString	This string is used for the tools tooltip.
longHelpString	This string is shown in the statusbar (if any) of the parent frame when the mouse pointer is inside the tool.
clientData	An optional pointer to client data which can be retrieved later using GetToolClientData().
Remarks
After you have added tools to a toolbar, you must call Realize() in order to have the tools appear.
See also
AddSeparator(), AddCheckTool(), AddRadioTool(), InsertTool(), DeleteTool(), Realize(), SetDropdownMenu()
==
virtual void wxToolBar::ClearTools	(		)	
virtual
Deletes all the tools in the toolbar.
==
virtual bool wxToolBar::DeleteTool	(	int 	toolId	)	
virtual
Removes the specified tool from the toolbar and deletes it.

If you don't want to delete the tool, but just to remove it from the toolbar (to possibly add it back later), you may use RemoveTool() instead.

Note
It is unnecessary to call Realize() for the change to take place, it will happen immediately.
Returns
true if the tool was deleted, false otherwise.
See also
==
virtual bool wxToolBar::DeleteToolByPos	(	size_t 	pos	)	
virtual
This function behaves like DeleteTool() but it deletes the tool at the specified position and not the one with the given id.
==
virtual void wxToolBar::EnableTool	(	int 	toolId,
bool 	enable 
)		
virtual
Enables or disables the tool.

Parameters
toolId	ID of the tool to enable or disable, as passed to AddTool().
enable	If true, enables the tool, otherwise disables it.
Remarks
Some implementations will change the visible state of the tool to indicate that it is disabled.
See also
GetToolEnabled(), ToggleTool()
==
wxToolBarToolBase* wxToolBar::FindById	(	int 	id	)	const
Returns a pointer to the tool identified by id or NULL if no corresponding tool is found.
==
virtual wxControl* wxToolBar::FindControl	(	int 	id	)	
virtual
Returns a pointer to the control identified by id or NULL if no corresponding control is found.
==
virtual wxToolBarToolBase* wxToolBar::FindToolForPosition	(	wxCoord 	x,
Finds a tool for the given mouse position.
x	X position.
y	Y position.
Returns
A pointer to a tool if a tool is found, or NULL otherwise.
Remarks
Currently not implemented in wxGTK (always returns NULL there).
wxSize wxToolBar::GetMargins	(		)	const
Returns the left/right and top/bottom margins, which are also used for inter-toolspacing.
==

virtual wxSize wxToolBar::GetToolBitmapSize	(		)	const
virtual
Returns the size of bitmap that the toolbar expects to have.

The default bitmap size is platform-dependent: for example, it is 16*15 for MSW and 24*24 for GTK. This size does not necessarily indicate the best size to use for the toolbars on the given platform, for this you should use wxArtProvider::GetNativeSizeHint(wxART_TOOLBAR) but in any case, as the bitmap size is deduced automatically from the size of the bitmaps associated with the tools added to the toolbar, it is usually unnecessary to call SetToolBitmapSize() explicitly.

Remarks
Note that this is the size of the bitmap you pass to AddTool(), and not the eventual size of the tool button.
==
const wxToolBarToolBase* wxToolBar::GetToolByPos	(	int 	pos	)	const
Returns a pointer to the tool at ordinal position pos.
Don't confuse this with FindToolForPosition().
==
virtual wxObject* wxToolBar::GetToolClientData	(	int 	toolId	)	const
virtual
Get any client data associated with the tool.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
Returns
Client data, or NULL if there is none.
==
virtual bool wxToolBar::GetToolEnabled	(	int 	toolId	)	const
virtual
Called to determine whether a tool is enabled (responds to user input).

Parameters
toolId	ID of the tool in question, as passed to AddTool().
Returns
true if the tool is enabled, false otherwise.
==
virtual wxString wxToolBar::GetToolLongHelp	(	int 	toolId	)	const
virtual
Returns the long help for the given tool.
toolId	ID of the tool in question, as passed to AddTool().
==
virtual int wxToolBar::GetToolPacking	(		)	const
virtual
Returns the value used for packing tools.
==
virtual int wxToolBar::GetToolPos	(	int 	toolId	)	const
virtual
Returns the tool position in the toolbar, or wxNOT_FOUND if the tool is not found.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
size_t wxToolBar::GetToolsCount	(		)	const
Returns the number of tools in the toolbar.
==
virtual int wxToolBar::GetToolSeparation	(		)	const
virtual
Returns the default separator size.
==
virtual wxString wxToolBar::GetToolShortHelp	(	int 	toolId	)	const
virtual
Returns the short help for the given tool.
Parameters
toolId	ID of the tool in question, as passed to AddTool().
==
virtual wxSize wxToolBar::GetToolSize	(		)	const
virtual
Returns the size of a whole button, which is usually larger than a tool bitmap because of added 3D effects.
==
virtual bool wxToolBar::GetToolState	(	int 	toolId	)	const
virtual
Gets the on/off state of a toggle tool.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
Returns
true if the tool is toggled on, false otherwise.
==
virtual wxToolBarToolBase* wxToolBar::InsertControl	(	size_t 	pos,
wxControl * 	control,
const wxString & 	label = wxEmptyString 
)		
virtual
Inserts the control into the toolbar at the given position.

You must call Realize() for the change to take place.
==
virtual wxToolBarToolBase* wxToolBar::InsertSeparator	(	size_t 	pos	)	
virtual
Inserts the separator into the toolbar at the given position.

You must call Realize() for the change to take place.
==
wxToolBarToolBase* wxToolBar::InsertStretchableSpace	(	size_t 	pos	)	
Inserts a stretchable space at the given position.
See AddStretchableSpace() for details about stretchable spaces.
==
wxToolBarToolBase* wxToolBar::InsertTool	(	size_t 	pos,
int 	toolId,
const wxString & 	label,
const wxBitmap & 	bitmap,
const wxBitmap & 	bmpDisabled = wxNullBitmap,
wxItemKind 	kind = wxITEM_NORMAL,
const wxString & 	shortHelp = wxEmptyString,
const wxString & 	longHelp = wxEmptyString,
wxObject * 	clientData = NULL 
)		
Inserts the tool with the specified attributes into the toolbar at the given position.

You must call Realize() for the change to take place.

See also
AddTool(), InsertControl(), InsertSeparator()
Returns
The newly inserted tool or NULL on failure. Notice that with the overload taking tool parameter the caller is responsible for deleting the tool in the latter case.
==
wxToolBarToolBase* wxToolBar::InsertTool	(	size_t 	pos,
wxToolBarToolBase * 	tool 
)		
Inserts the tool with the specified attributes into the toolbar at the given position.

You must call Realize() for the change to take place.

See also
AddTool(), InsertControl(), InsertSeparator()
Returns
The newly inserted tool or NULL on failure. Notice that with the overload taking tool parameter the caller is responsible for deleting the tool in the latter case.
==
virtual bool wxToolBar::OnLeftClick	(	int 	toolId,
bool 	toggleDown 
)		
virtual
Called when the user clicks on a tool with the left mouse button.

This is the old way of detecting tool clicks; although it will still work, you should use the EVT_MENU() or EVT_TOOL() macro instead.

Parameters
toolId	The identifier passed to AddTool().
toggleDown	true if the tool is a toggle and the toggle is down, otherwise is false.
Returns
If the tool is a toggle and this function returns false, the toggle state (internal and visual) will not be changed. This provides a way of specifying that toggle operations are not permitted in some circumstances.
See also
OnMouseEnter(), OnRightClick()
==
virtual void wxToolBar::OnMouseEnter	(	int 	toolId	)	
virtual
This is called when the mouse cursor moves into a tool or out of the toolbar.

This is the old way of detecting mouse enter events; although it will still work, you should use the EVT_TOOL_ENTER() macro instead.

Parameters
toolId	Greater than -1 if the mouse cursor has moved into the tool, or -1 if the mouse cursor has moved. The programmer can override this to provide extra information about the tool, such as a short description on the status line.
Remarks
With some derived toolbar classes, if the mouse moves quickly out of the toolbar, wxWidgets may not be able to detect it. Therefore this function may not always be called when expected.
virtual void wxToolBar::OnRightClick	(	int 	toolId,
long 	x,
long 	y 
)		
virtual
Deprecated:
This is the old way of detecting tool right clicks; although it will still work, you should use the EVT_TOOL_RCLICKED() macro instead.
Called when the user clicks on a tool with the right mouse button. The programmer should override this function to detect right tool clicks.

Parameters
toolId	The identifier passed to AddTool().
x	The x position of the mouse cursor.
y	The y position of the mouse cursor.
Remarks
A typical use of this member might be to pop up a menu.
See also
OnMouseEnter(), OnLeftClick()
==
virtual bool wxToolBar::Realize	(		)	
This function should be called after you have added tools.
==
virtual wxToolBarToolBase* wxToolBar::RemoveTool	(	int 	id	)	
Removes the given tool from the toolbar but doesn't delete it.
This allows inserting/adding this tool back to this (or another) toolbar later.
Note
It is unnecessary to call Realize() for the change to take place, it will happen immediately.
===
bool wxToolBar::SetDropdownMenu	(	int 	id,
wxMenu * 	menu 
)		
Sets the dropdown menu for the tool given by its id.

The tool itself will delete the menu when it's no longer needed. Only supported under GTK+ und MSW.

If you define a EVT_TOOL_DROPDOWN() handler in your program, you must call wxEvent::Skip() from it or the menu won't be displayed.
==

virtual void wxToolBar::SetMargins	(	int 	x,
int 	y 
)		
virtual
Set the values to be used as margins for the toolbar.

Parameters
x	Left margin, right margin and inter-tool separation value.
y	Top margin, bottom margin and inter-tool separation value.
Remarks
This must be called before the tools are added if absolute positioning is to be used, and the default (zero-size) margins are to be overridden.
See also
GetMargins()
==
void wxToolBar::SetMargins	(	const wxSize & 	size	)	
Set the margins for the toolbar.

Parameters
size	Margin size.
Remarks
This must be called before the tools are added if absolute positioning is to be used, and the default (zero-size) margins are to be overridden.
See also
GetMargins(), wxSize
==
virtual void wxToolBar::SetToolBitmapSize	(	const wxSize & 	size	)	
virtual
Sets the default size of each tool bitmap.
The default bitmap size is 16 by 15 pixels.

Parameters
size	The size of the bitmaps in the toolbar.
Remarks
This should be called to tell the toolbar what the tool bitmap size is. Call it before you add tools.
==
virtual void wxToolBar::SetToolClientData	(	int 	id,
wxObject * 	clientData 
)		
virtual
Sets the client data associated with the tool.

Parameters
id	ID of the tool in question, as passed to AddTool().
clientData	The client data to use.
==
virtual void wxToolBar::SetToolDisabledBitmap	(	int 	id,
const wxBitmap & 	bitmap 
)		
virtual
Sets the bitmap to be used by the tool with the given ID when the tool is in a disabled state.
This can only be used on Button tools, not controls.

Parameters
id	ID of the tool in question, as passed to AddTool().
bitmap	Bitmap to use for disabled tools.
Note
The native toolbar classes on the main platforms all synthesize the disabled bitmap from the normal bitmap, so this function will have no effect on those platforms.
==
virtual void wxToolBar::SetToolLongHelp	(	int 	toolId,
const wxString & 	helpString 
)		
virtual
Sets the long help for the given tool.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
helpString	A string for the long help.
Remarks
You might use the long help for displaying the tool purpose on the status line.
==
virtual void wxToolBar::SetToolNormalBitmap	(	int 	id,
const wxBitmap & 	bitmap 
)		
virtual
Sets the bitmap to be used by the tool with the given ID.

This can only be used on Button tools, not controls.

Parameters
id	ID of the tool in question, as passed to AddTool().
bitmap	Bitmap to use for normals tools.
==
virtual void wxToolBar::SetToolPacking	(	int 	packing	)	
virtual
Sets the value used for spacing tools.

The default value is 1.

Parameters
packing	The value for packing.
Remarks
The packing is used for spacing in the vertical direction if the toolbar is horizontal, and for spacing in the horizontal direction if the toolbar is vertical.
==
virtual void wxToolBar::SetToolSeparation	(	int 	separation	)	
virtual
Sets the default separator size.
The default value is 5.

Parameters
separation	The separator size.
==
virtual void wxToolBar::SetToolShortHelp	(	int 	toolId,
const wxString & 	helpString 
)		
Sets the short help for the given tool.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
helpString	The string for the short help.
Remarks
An application might use short help for identifying the tool purpose in a tooltip.
==
virtual void wxToolBar::ToggleTool	(	int 	toolId,
bool 	toggle 
)		
virtual
Toggles a tool on or off.

This does not cause any event to get emitted.

Parameters
toolId	ID of the tool in question, as passed to AddTool().
toggle	If true, toggles the tool on, otherwise toggles it off.
Remarks
Only applies to a tool that has been specified as a toggle tool. 
 */