using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Dockable toolbar, managed by <see cref="AuiManager"/>.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    public class AuiToolbar : Control
    {
        /// <summary>
        /// Defines spacer width which is used in <see cref="AddSpacer"/>.
        /// </summary>
        public const int DefaultSpacerWidth = 5;

        private readonly Dictionary<int, ToolData> toolData = new();
        private int idCounter = 0;
        private int showingDropDown = 0;

        /// <summary>
        /// Occurs when the tool is clicked.
        /// </summary>
        public event EventHandler? ToolDropDown;

        /// <summary>
        /// Occurs when the tool is clicked, after <see cref="ToolDropDown"/>.
        /// </summary>
        /// <remarks>It is better to process commands here than in
        /// <see cref="ToolDropDown"/> event handler.</remarks>
        public event EventHandler? ToolCommand;

        /// <summary>
        /// Occurs when the user begins toolbar dragging by the mouse.
        /// </summary>
        public event EventHandler? BeginDrag;

        /// <summary>
        /// Occurs when the tool is clicked with middle mouse button.
        /// </summary>
        public event EventHandler? ToolMiddleClick;

        /// <summary>
        /// Occurs when the tool overflow button is clicked.
        /// </summary>
        public event EventHandler? OverflowClick;

        /// <summary>
        /// Occurs when the tool is clicked with right mouse button.
        /// </summary>
        public event EventHandler? ToolRightClick;

        /// <summary>
        /// Defines default visual style for the newly created
        /// <see cref="AuiToolbar"/> controls.
        /// </summary>
        public static AuiToolbarCreateStyle DefaultCreateStyle { get; set; }
            = AuiToolbarCreateStyle.DefaultStyle;

        /// <summary>
        /// Defines visual style and behavior of the <see cref="AuiToolbar"/> control.
        /// </summary>
        /// <remarks>
        /// When this property is changed, control is recreated.
        /// </remarks>
        public AuiToolbarCreateStyle CreateStyle
        {
            get
            {
                return (AuiToolbarCreateStyle)Handler.CreateStyle;
            }

            set
            {
                Handler.CreateStyle = (int)value;
            }
        }

        /// <summary>
        /// Gets id of the tool passed in the event handler.
        /// </summary>
        /// <remarks>
        /// Use it in the <see cref="ToolDropDown"/> or other event handlers.
        /// </remarks>
        [Browsable(false)]
        public int EventToolId
        {
            get
            {
                return NativeControl.EventToolId;
            }
        }

        /// <summary>
        /// Gets whether is drop down part of the tool was clicked when event is fired.
        /// </summary>
        /// <remarks>
        /// Use it in the <see cref="ToolDropDown"/> or other event handlers.
        /// </remarks>
        [Browsable(false)]
        public bool EventIsDropDownClicked
        {
            get
            {
                return NativeControl.EventIsDropDownClicked;
            }
        }

        /// <summary>
        /// Gets mouse click point passed in the event handler.
        /// </summary>
        /// <remarks>
        /// Use it in the <see cref="ToolDropDown"/> or other event handlers.
        /// </remarks>
        [Browsable(false)]
        public Int32Point EventClickPoint
        {
            get
            {
                return NativeControl.EventClickPoint;
            }
        }

        /// <summary>
        /// Gets tool item rectangle passed in the event handler.
        /// </summary>
        /// <remarks>
        /// Use it in the <see cref="ToolDropDown"/> or other event handlers.
        /// </remarks>
        [Browsable(false)]
        public Int32Rect EventItemRect
        {
            get
            {
                return NativeControl.EventItemRect;
            }
        }

        /// <summary>
        /// Gets or sets the default size of each tool bitmap.
        /// </summary>
        /// <remarks>
        /// This should be called to tell the toolbar what the tool bitmap
        /// size is. Call it before you add tools.
        /// </remarks>
        /// <remarks>
        /// The default bitmap size is platform-dependent: for example, it is
        /// 16*15 for Windows and 24*24 for GTK. This size does not necessarily
        /// indicate the best size to use for the toolbars on the given platform,
        /// for this you should use ArtProvider's GetNativeSizeHint()
        /// but in any case, as the bitmap size is deduced automatically from
        /// the size of the bitmaps associated with the tools added to the
        /// toolbar, it is usually unnecessary to set this property explicitly.
        /// </remarks>
        /// <remarks>
        /// Note that this is the size of the bitmap you pass to
        /// <see cref="AddTool"/>, and not the eventual size of the tool button.
        /// </remarks>
        public Size ToolBitmapSize
        {
            get => NativeControl.GetToolBitmapSize();
            set => NativeControl.SetToolBitmapSize(value);
        }

        /// <summary>
        /// Gets or sets tool border padding.
        /// </summary>
        public int ToolBorderPadding
        {
            get => NativeControl.GetToolBorderPadding();
            set => NativeControl.SetToolBorderPadding(value);
        }

        /// <summary>
        /// Gets or sets tool text orientation.
        /// </summary>
        public AuiToolbarTextOrientation ToolTextOrientation
        {
            get => (AuiToolbarTextOrientation)NativeControl.GetToolTextOrientation();
            set => NativeControl.SetToolTextOrientation((int)value);
        }

        /// <summary>
        /// Gets or sets the value used for packing tools.
        /// </summary>
        /// <remarks>
        /// The packing is used for spacing in the vertical direction if
        /// the toolbar is horizontal, and for spacing in the horizontal
        /// direction if the toolbar is vertical. The default value is 1.
        /// </remarks>
        public int ToolPacking
        {
            get => NativeControl.GetToolPacking();
            set => NativeControl.SetToolPacking(value);
        }

        /// <summary>
        /// Gets or sets the default separator size.
        /// </summary>
        /// <remarks>
        /// The default value is 5.
        /// </remarks>
        public int ToolSeparation
        {
            get => NativeControl.GetToolSeparation();
            set => NativeControl.SetToolSeparation(value);
        }

        /// <summary>
        /// Gets or sets whether toolbar overflow button is visible.
        /// </summary>
        public bool OverflowVisible
        {
            get => NativeControl.GetOverflowVisible();
            set => NativeControl.SetOverflowVisible(value);
        }

        /// <summary>
        /// Gets or sets whether toolbar gripper is visible.
        /// </summary>
        public bool GripperVisible
        {
            get => NativeControl.GetGripperVisible();
            set => NativeControl.SetGripperVisible(value);
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

        /// <summary>
        /// Creates <see cref="ImageSet"/> and loads Svg data from the specified
        /// <paramref name="url"/>.
        /// </summary>
        /// <param name="url">File or resource url with Svg data.</param>
        /// <param name="imageSize">Svg image width and height.</param>
        public static ImageSet LoadSvgImage(string url, Int32Size imageSize)
        {
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height);
            return result;
        }

        /// <summary>
        /// Creates <see cref="ImageSet"/> and loads Svg data from the specified
        /// <paramref name="url"/>.
        /// </summary>
        /// <remarks>
        /// Gets DPI settings from <paramref name="dpiControl"/> and selects appropriate
        /// images size using <see cref="Toolbar.GetDefaultImageSize(Control)"/>.
        /// </remarks>
        /// <param name="url">File or resource url with Svg data.</param>
        /// <param name="dpiControl">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI settings.</param>
        public static ImageSet LoadSvgImage(string url, Control dpiControl)
        {
            return ImageSet.FromSvgUrlForToolbar(url, dpiControl);
        }

        /// <summary>
        /// Adds a tool to the toolbar.
        /// </summary>
        /// <param name="label">The string to be displayed with the tool.</param>
        /// <param name="bitmap">The primary tool bitmap.</param>
        /// <param name="shortHelpString">This string is used for the tools
        /// tooltip.</param>
        /// <param name="itemKind">
        /// May be <see cref="AuiToolbarItemKind.Normal"/> for a normal button
        /// (default), <see cref="AuiToolbarItemKind.Check"/> for a checkable
        /// tool (such tool stays pressed after it had been toggled) or
        /// <see cref="AuiToolbarItemKind.Radio"/> for a checkable tool which
        /// makes part of a radio group of tools each of which is automatically
        /// unchecked whenever another button in the group is checked.
        /// <see cref="AuiToolbarItemKind.Dropdown"/> specifies that a drop-down
        /// menu button will appear next to the tool button.
        /// </param>
        /// <param name="disabledBitmap">The bitmap used when the tool is disabled.
        /// If it is not specified (default), the disabled bitmap is
        /// automatically generated by greying the normal one.</param>
        /// <param name="longHelpString">This string is shown in the statusbar
        /// (if any) of the parent frame when the mouse pointer is inside
        /// the tool.</param>
        /// <remarks>
        /// After you have added tools to a toolbar, you must call
        /// <see cref="Realize"/> in order to have the tools appear.
        /// </remarks>
        /// <returns>An <see cref="int"/> by which the tool may be identified
        /// in subsequent operations.</returns>
        public int AddTool(
            string? label,
            ImageSet? bitmap = null,
            string? shortHelpString = null,
            AuiToolbarItemKind itemKind = AuiToolbarItemKind.Normal,
            ImageSet? disabledBitmap = null,
            string? longHelpString = null)
        {
            int toolId = GenNewId();
            label ??= string.Empty;
            shortHelpString ??= string.Empty;
            longHelpString ??= string.Empty;
            NativeControl.AddTool2(
                toolId,
                label,
                bitmap?.NativeImageSet,
                disabledBitmap?.NativeImageSet,
                (int)itemKind,
                shortHelpString!,
                longHelpString!,
                IntPtr.Zero);
            return toolId;
        }

        /// <summary>
        /// Gets kind of the tool item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public AuiToolbarItemKind GetToolKind(int toolId)
        {
            var result = NativeControl.GetToolKind(toolId);
            return (AuiToolbarItemKind)result;
        }

        /// <summary>
        /// Adds a text label to the toolbar.
        /// </summary>
        /// <param name="label">The string to be displayed with the tool.</param>
        /// <param name="width">Tool width or -1 for auto width.
        /// Optional parameter.</param>
        /// <returns>An <see cref="int"/> by which the tool may be identified
        /// in subsequent operations.</returns>
        public int AddLabel(string label, int width = -1)
        {
            int toolId = GenNewId();
            NativeControl.AddLabel(toolId, label, width);
            return toolId;
        }

        /// <summary>
        /// Adds any control to the toolbar, typically e.g. a <see cref="ComboBox"/> or
        /// <see cref="TextBox"/>.
        /// </summary>
        /// <param name="control">The control to be added.</param>
        public int AddControl(Control control)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));
            control.Parent = this;
            int toolId = GenNewId();
            NativeControl.AddControl(toolId, control.WxWidget, string.Empty);
            return toolId;
        }

        /// <summary>
        /// Adds a separator for spacing groups of tools.
        /// </summary>
        /// <remarks>
        /// Notice that the separator uses the look appropriate for the current
        /// platform so it can be a vertical line (Windows, some versions of Linux)
        /// or just an empty space or something else.
        /// </remarks>
        public void AddSeparator()
        {
            NativeControl.AddSeparator();
        }

        /// <summary>
        /// Adds an empty space for spacing groups of tools.
        /// </summary>
        /// <param name="pixels">Width of an empty space.</param>
        /// <remarks>
        /// If <paramref name="pixels"/> is not specified,
        /// <see cref="DefaultSpacerWidth"/> is used as a spacer width.
        /// </remarks>
        public void AddSpacer(int? pixels = null)
        {
            pixels ??= DefaultSpacerWidth;
            NativeControl.AddSpacer(pixels.Value);
        }

        /// <summary>
        /// Adds a stretchable space to the toolbar.
        /// </summary>
        public void AddStretchSpacer(int proportion = 1)
        {
            NativeControl.AddStretchSpacer(proportion);
        }

        /// <summary>
        /// This function should be called after you have added tools.
        /// </summary>
        /// <returns><c>true</c> if toolbar visual representation was updated
        /// successfully, <c>false</c> otherwise.</returns>
        public bool Realize()
        {
            return NativeControl.Realize();
        }

        /// <summary>
        /// Deletes all the tools in the toolbar.
        /// </summary>
        public void Clear()
        {
            NativeControl.Clear();
        }

        /// <summary>
        /// Removes the specified tool from the toolbar and deletes it.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns><c>true</c> if the tool was removed or <c>false</c> otherwise,
        /// e.g. if the tool with the given ID was not found.</returns>
        /// <remarks>
        /// Note that if this tool was added by <see cref="AddControl"/>,
        /// the associated control is not deleted and must either be
        /// reused (e.g.by reparenting it under a different window)
        /// or destroyed by caller.
        /// </remarks>
        public bool DeleteTool(int toolId)
        {
            return NativeControl.DeleteTool(toolId);
        }

        /// <summary>
        /// Removes the tool at the given position from the toolbar.
        /// </summary>
        /// <remarks>
        /// Note that if this tool was added by <see cref="AddControl"/>,
        /// the associated control is not deleted and must either be
        /// reused (e.g.by reparenting it under a different window)
        /// or destroyed by caller.
        /// </remarks>
        /// <param name="index">The index, or position, of a previously
        /// added tool.</param>
        /// <returns><c>true</c> if the tool was removed or <c>false</c> otherwise,
        /// e.g. if the tool with the given ID was not found.</returns>
        public bool DeleteByIndex(int index)
        {
            return NativeControl.DeleteByIndex(index);
        }

        /// <summary>
        /// Finds tool by its ID and returns tool index (position).
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>The index, or position, of a previously
        /// added tool.</returns>
        public int GetToolIndex(int toolId)
        {
            return NativeControl.GetToolIndex(toolId);
        }

        /// <summary>
        /// Gets whether the specified tool fits in the toolbar visible area.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>
        /// <c>true</c> if tool fits in the toolbar, <c>false</c> otherwise.
        /// </returns>
        public bool GetToolFits(int toolId)
        {
            return NativeControl.GetToolFits(toolId);
        }

        /// <summary>
        /// Returns the specified tool rectangle in the toolbar.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Position and size of the tool in the toolbar.</returns>
        public Rect GetToolRect(int toolId)
        {
            return NativeControl.GetToolRect(toolId);
        }

        /// <summary>
        /// Gets whether the specified tool fits in the toolbar visible area.
        /// </summary>
        /// <param name="index">The index, or position, of a previously
        /// added tool.</param>
        /// <returns>
        /// <c>true</c> if tool fits in the toolbar, <c>false</c> otherwise.
        /// </returns>
        public bool GetToolFitsByIndex(int index)
        {
            return NativeControl.GetToolFitsByIndex(index);
        }

        /// <summary>
        /// Gets whether all tools are inside visible area of the toolbar.
        /// </summary>
        /// <returns><c>true</c> if all tools are visible, <c>false</c>
        /// otherwise.</returns>
        public bool GetToolBarFits()
        {
            return NativeControl.GetToolBarFits();
        }

        /// <summary>
        /// Toggles a tool on or off.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="state">If <c>true</c>, toggles the tool on,
        /// otherwise toggles it off.</param>
        /// <remarks>
        /// This does not cause any event to get emitted.
        /// </remarks>
        /// <remarks>
        /// Only applies to a tool that has been specified as a toggle tool.
        /// </remarks>
        public void ToggleTool(int toolId, bool state)
        {
            NativeControl.ToggleTool(toolId, state);
        }

        /// <summary>
        /// Returns whether a tool is on or off.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>If <c>true</c>, the tool is on,
        /// otherwise it is off.</returns>
        public bool GetToolToggled(int toolId)
        {
            return NativeControl.GetToolToggled(toolId);
        }

        /// <summary>
        /// Set the values to be used as margins for the toolbar.
        /// </summary>
        /// <param name="left">Left margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        /// <remarks>
        /// This must be called before the tools are added if absolute
        /// positioning is to be used, and the default (zero-size) margins
        /// are to be overridden.
        /// </remarks>
        public void SetMargins(int left, int right, int top, int bottom)
        {
            NativeControl.SetMargins(left, right, top, bottom);
        }

        /// <summary>
        /// Enables or disables the tool.
        /// </summary>
        /// <param name="toolId">ID of the tool to enable or disable.</param>
        /// <param name="state">If <c>true</c>, enables the tool,
        /// otherwise disables it.</param>
        /// <remarks>
        /// Some implementations will change the visible state of the
        /// tool to indicate that it is disabled.
        /// </remarks>
        /// <remarks>You need to call <see cref="Control.Invalidate"/> after changing
        /// enabled state of the tool.</remarks>
        public void EnableTool(int toolId, bool state)
        {
            NativeControl.EnableTool(toolId, state);
        }

        /// <summary>
        /// Called to determine whether a tool is enabled (responds to user input).
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns><c>true</c> if the tool is enabled, <c>false</c>
        /// otherwise.</returns>
        public bool GetToolEnabled(int toolId)
        {
            return NativeControl.GetToolEnabled(toolId);
        }

        /// <summary>
        /// Set whether the specified toolbar item has a drop down button.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="dropdown">
        ///  if <c>true</c> adds a drop down button to the toolbar item,
        ///  if <c>false</c> removes a drop down button.
        /// </param>
        /// <remarks>
        /// This is only valid for normal tools.
        /// </remarks>
        public void SetToolDropDown(int toolId, bool dropdown)
        {
            NativeControl.SetToolDropDown(toolId, dropdown);
        }

        /// <summary>
        /// Returns whether the specified toolbar item has an associated
        /// drop down button.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>
        ///  <c>true</c> if the toolbar item has a drop down button,
        ///  <c>false</c> otherwise.
        /// </returns>
        public bool GetToolDropDown(int toolId)
        {
            return NativeControl.GetToolDropDown(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item proportion.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="proportion">new toolbar item proportion</param>
        public void SetToolProportion(int toolId, int proportion)
        {
            NativeControl.SetToolProportion(toolId, proportion);
        }

        /// <summary>
        /// Gets the specified toolbar item proportion.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Toolbar item proportion value.</returns>
        public int GetToolProportion(int toolId)
        {
            return NativeControl.GetToolProportion(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="sticky">new Sticky property value.</param>
        public void SetToolSticky(int toolId, bool sticky)
        {
            NativeControl.SetToolSticky(toolId, sticky);
        }

        /// <summary>
        /// Shows context menu under the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="contextMenu">Context menu.</param>
        public void ShowDropDownMenu(int toolId, ContextMenu contextMenu)
        {
            if (showingDropDown > 0)
                return;

            showingDropDown++;

            try
            {
                Window? window = ParentWindow;
                if (window == null)
                    return;

                SetToolSticky(toolId, true);

                Rect toolRect = GetToolRect(toolId);
                Point pt = ClientToScreen(toolRect.BottomLeft);
                pt = window.ScreenToClient(pt);

                window.ShowPopupMenu(contextMenu, (int)pt.X, (int)pt.Y);
                DoOnCaptureLost();
                SetToolSticky(toolId, false);
            }
            finally
            {
                showingDropDown--;
            }
        }

        /// <summary>
        /// Gets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Toolbar item Sticky property value.</returns>
        public bool GetToolSticky(int toolId)
        {
            return NativeControl.GetToolSticky(toolId);
        }

        /// <summary>
        /// Gets the specified toolbar item label text.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public string GetToolLabel(int toolId)
        {
            return NativeControl.GetToolLabel(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item label text.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="label">New toolbar item label text.</param>
        public void SetToolLabel(int toolId, string label)
        {
            NativeControl.SetToolLabel(toolId, label);
        }

        /// <summary>
        /// Sets the specified toolbar item image.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="bitmap">New toolbar item image.</param>
        public void SetToolBitmap(int toolId, ImageSet? bitmap)
        {
            NativeControl.SetToolBitmap(toolId, bitmap?.NativeImageSet);
        }

        /// <summary>
        /// Returns the short help for the given tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <remarks>Usually short help is shown as a hint when mouse is
        /// over tool item.</remarks>
        public string GetToolShortHelp(int toolId)
        {
            return NativeControl.GetToolShortHelp(toolId);
        }

        /// <summary>
        /// Registers event handler which is called when tool is clicked.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="ev">Event handler.</param>
        public void AddToolOnClick(int toolId, EventHandler ev)
        {
            var toolData = GetToolData(toolId, true);
            toolData!.Click += ev;
        }

        /// <summary>
        /// Sets a menu used as this toolbar item drop-down.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="menu">Drop down menu.</param>
        public void SetToolDropDownMenu(int toolId, ContextMenu menu)
        {
            var toolData = GetToolData(toolId, true);
            toolData!.DropDownMenu = menu;
        }

        /// <summary>
        /// Gets a menu used as this toolbar item drop-down.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public ContextMenu? GetToolDropDownMenu(int toolId)
        {
            var toolData = GetToolData(toolId, false);
            if (toolData == null)
                return null;
            return toolData.DropDownMenu;
        }

        /// <summary>
        /// Sets when a drop down menu associated with the toolbar item is shown.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="onEvent">Type of event which shows a drop down menu.</param>
        public void SetToolDropDownOnEvent(int toolId, AuiToolbarItemDropDownOnEvent onEvent)
        {
            var toolData = GetToolData(toolId, true);
            toolData!.DropDownOnEvent = onEvent;
        }

        /// <summary>
        /// Gets type of the event which shows a drop down menu associated with the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public AuiToolbarItemDropDownOnEvent GetToolDropDownOnEvent(int toolId)
        {
            var toolData = GetToolData(toolId, false);
            if (toolData == null)
                return AuiToolbarItemDropDownOnEvent.None;
            return toolData.DropDownOnEvent;
        }

        /// <summary>
        /// Unregisters event handler which was called when tool is clicked.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="ev">Event handler.</param>
        public void RemoveToolOnClick(int toolId, EventHandler ev)
        {
            var toolData = GetToolData(toolId, false);
            if (toolData == null)
                return;
            toolData.Click -= ev;
        }

        /// <summary>
        /// Sets the short help for the given tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="helpString">The string for the short help.</param>
        /// <remarks>
        /// An application might use short help for identifying the
        /// tool purpose in a tooltip.
        /// </remarks>
        public void SetToolShortHelp(int toolId, string helpString)
        {
            NativeControl.SetToolShortHelp(toolId, helpString);
        }

        /// <summary>
        /// Returns the long help for the given tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <remarks>
        /// You might use the long help for displaying the tool
        /// purpose on the status line.
        /// </remarks>
        public string GetToolLongHelp(int toolId)
        {
            return NativeControl.GetToolLongHelp(toolId);
        }

        /// <summary>
        /// Sets the long help for the given tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="helpString">A string for the long help.</param>
        /// <remarks>
        /// You might use the long help for displaying the tool
        /// purpose on the status line.
        /// </remarks>
        public void SetToolLongHelp(int toolId, string helpString)
        {
            NativeControl.SetToolLongHelp(toolId, helpString);
        }

        /// <summary>
        /// Returns the number of tools in the toolbar.
        /// </summary>
        public int GetToolCount()
        {
            return (int)NativeControl.GetToolCount();
        }

        /// <summary>
        /// Sets minimal width of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        /// <param name="width">New minimal width of a toolbar item.</param>
        public void SetToolMinWidth(int toolId, int width)
        {
            var minHeight = GetToolMinHeight(toolId);
            NativeControl.SetToolMinSize(toolId, width, minHeight);
        }

        /// <summary>
        /// Sets new minimal width of the toolbar item if its greater than previous width.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        /// <param name="width">New minimal width of a toolbar item.</param>
        public void GrowToolMinWidth(int toolId, int width)
        {
            var size = GetToolMinSize(toolId);
            NativeControl.SetToolMinSize(toolId, Math.Max(width, size.Width), size.Height);
        }

        /// <summary>
        /// Sets new minimal size of the toolbar item if its greater than previous size.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        /// <param name="width">New minimal width of a toolbar item.</param>
        /// <param name="height">New minimal height of a toolbar item.</param>
        public void GrowToolMinSize(int toolId, int width, int height)
        {
            var size = GetToolMinSize(toolId);
            NativeControl.SetToolMinSize(
                toolId,
                Math.Max(width, size.Width),
                Math.Max(height, size.Height));
        }

        /// <summary>
        /// Sets new minimal size of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        /// <param name="width">New minimal width of a toolbar item.</param>
        /// <param name="height">New minimal height of a toolbar item.</param>
        public void SetToolMinSize(int toolId, int width, int height)
        {
            NativeControl.SetToolMinSize(toolId, width, height);
        }

        /// <summary>
        /// Gets minimal size of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public Int32Size GetToolMinSize(int toolId)
        {
            return NativeControl.GetToolMinSize(toolId);
        }

        /// <summary>
        /// Gets minimal height of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public int GetToolMinHeight(int toolId)
        {
            return NativeControl.GetToolMinSize(toolId).Height;
        }

        /// <summary>
        /// Gets minimal width of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public int GetToolMinWidth(int toolId)
        {
            return NativeControl.GetToolMinSize(toolId).Width;
        }

        internal void SetToolAlignment(int toolId, int align)
        {
            NativeControl.SetAlignment(toolId, (int)align);
        }

        internal int GetToolAlignment(int toolId)
        {
            return NativeControl.GetAlignment(toolId);
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

        /// <summary>
        /// Destroys the tool with the given ID and its associated control, if any.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns><c>true</c> if the tool was destroyed or
        /// <c>false</c> otherwise, e.g. if the tool with the given ID
        /// was not found or if the provided index is out of range.</returns>
        internal bool DestroyTool(int toolId)
        {
            return NativeControl.DestroyTool(toolId);
        }

        /// <summary>
        /// Destroys the tool at the given position and its associated control, if any.
        /// </summary>
        /// <param name="idx">The index, or position, of a previously added tool.</param>
        /// <returns><c>true</c> if the tool was destroyed or
        /// <c>false</c> otherwise, e.g. if the tool with the given ID
        /// was not found or if the provided index is out of range.</returns>
        internal bool DestroyToolByIndex(int idx)
        {
            return NativeControl.DestroyToolByIndex(idx);
        }

        /// <summary>
        /// Sets the art provider to be used by the toolbar.
        /// </summary>
        /// <param name="art">New art provider.</param>
        internal void SetArtProvider(IntPtr art)
        {
            NativeControl.SetArtProvider(art);
        }

        internal void RaiseOverflowClick(EventArgs e)
        {
            OnOverflowClick(e);
            OverflowClick?.Invoke(this, e);
        }

        internal void RaiseToolRightClick(EventArgs e)
        {
            OnToolRightClick(e);
            ToolRightClick?.Invoke(this, e);
        }

        internal void RaiseToolCommand(EventArgs e)
        {
            OnToolCommand(e);
            ToolCommand?.Invoke(this, e);
            ProcessToolClick();
        }

        internal void ProcessToolClick()
        {
            var toolData = GetToolData(EventToolId, false);
            if (toolData == null)
                return;
            toolData.OnClick(this, EventArgs.Empty);

            AuiToolbarItemDropDownOnEvent dropDownOnEvent = toolData.DropDownOnEvent;

            if (dropDownOnEvent == AuiToolbarItemDropDownOnEvent.None)
                return;

            ContextMenu? menu = toolData.DropDownMenu;
            if (menu == null)
                return;

            var show = false;

            if (dropDownOnEvent == AuiToolbarItemDropDownOnEvent.Click && !EventIsDropDownClicked)
                show = true;
            if (dropDownOnEvent == AuiToolbarItemDropDownOnEvent.ClickArrow &&
                EventIsDropDownClicked)
                show = true;

            if (!show)
                return;
            ShowDropDownMenu(EventToolId, menu);
        }

        internal void RaiseToolDropDown(EventArgs e)
        {
            OnToolDropDown(e);
            ToolDropDown?.Invoke(this, e);
        }

        internal void RaiseToolMiddleClick(EventArgs e)
        {
            OnToolMiddleClick(e);
            ToolMiddleClick?.Invoke(this, e);
        }

        internal void RaiseBeginDrag(EventArgs e)
        {
            OnBeginDrag(e);
            BeginDrag?.Invoke(this, e);
        }

        /// <summary>
        /// Returns the associated art provider.
        /// </summary>
        internal IntPtr GetArtProvider()
        {
            return NativeControl.GetArtProvider();
        }

        internal ToolData? GetToolData(int toolId, bool add)
        {
            if (toolData.TryGetValue(toolId, out ToolData? tool))
                return tool;

            if (add)
            {
                tool = new();
                toolData.Add(toolId, tool);
                return tool;
            }

            return null;
        }

        internal void DoOnCaptureLost()
        {
            NativeControl.DoOnCaptureLost();
        }

        internal void DoOnLeftUp(int x, int y)
        {
            NativeControl.DoOnLeftUp(x, y);
        }

        internal void DoOnLeftDown(int x, int y)
        {
            NativeControl.DoOnLeftDown(x, y);
        }

        /// <summary>
        /// Called when the tool is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnToolDropDown(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the tool is clicked, after <see cref="OnToolDropDown"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnToolCommand(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the tool overflow button is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnOverflowClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the tool is clicked with right mouse button.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnToolRightClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the user begins toolbar dragging by the mouse.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnToolMiddleClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the user begins toolbar dragging by the mouse.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains
        /// the event data.</param>
        protected virtual void OnBeginDrag(EventArgs e)
        {
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

        internal class ToolData
        {
            public event EventHandler? Click;

            public ContextMenu? DropDownMenu { get; set; }

            public AuiToolbarItemDropDownOnEvent DropDownOnEvent { get; set; }

            public void OnClick(object? sender, EventArgs ev)
            {
                Click?.Invoke(sender, ev);
            }
        }
    }
}