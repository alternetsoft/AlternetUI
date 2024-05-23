using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Dockable toolbar, managed by <see cref="AuiManager"/>.
    /// </summary>
    [ControlCategory("MenusAndToolbars")]
    internal partial class AuiToolbar : Control
    {
        /// <summary>
        /// Defines spacer width which is used in <see cref="AddSpacer"/>.
        /// </summary>
        public const int DefaultSpacerWidth = 5;

        private readonly Dictionary<int, ToolData> toolData = new();
        private int idCounter = 1;
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

        /// <inheritdoc/>
        public override IReadOnlyList<Control> AllChildrenInLayout
            => Array.Empty<Control>();

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
        /// Gets name of the tool passed in the event handler. If name is empty,
        /// tool id is returned.
        /// </summary>
        [Browsable(false)]
        public string EventToolNameOrId
        {
            get
            {
                var id = EventToolId;
                var name = GetToolName(id);
                if (name is null)
                    return id.ToString();
                return name;
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
        public PointI EventClickPoint
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
        public RectI EventItemRect
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
        public SizeI ToolBitmapSizeInPixels
        {
            get => NativeControl.GetToolBitmapSizeInPixels();
            set => NativeControl.SetToolBitmapSizeInPixels(value);
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
        public override ControlTypeId ControlKind => ControlTypeId.AuiToolbar;

        internal new AuiToolbarHandler Handler
        {
            get
            {
                CheckDisposed();
                return (AuiToolbarHandler)base.Handler;
            }
        }

        /// <summary>
        /// Gets or sets the associated art provider.
        /// </summary>
        internal IAuiToolbarArt ArtProvider
        {
            get
            {
                var result = NativeControl.GetArtProvider();
                return new AuiToolbarArt(result, false);
            }

            set
            {
                NativeControl.SetArtProvider(((AuiToolbarArt)value).Handle);
            }
        }

        internal Native.AuiToolBar NativeControl => Handler.NativeControl;

        /// <summary>
        /// Creates <see cref="ImageSet"/> and loads Svg data from the specified
        /// <paramref name="url"/>.
        /// </summary>
        /// <param name="url">File or resource url with Svg data.</param>
        /// <param name="imageSize">Svg image width and height.</param>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet LoadSvgImage(string url, SizeI imageSize, Color? color = null)
        {
            var result = ImageSet.FromSvgUrl(url, imageSize.Width, imageSize.Height, color);
            return result;
        }

        /// <inheritdoc cref="LoadSvgImage(string, SizeI, Color?)"/>
        public static ImageSet LoadSvgImage(string url, SizeD imageSize, Color? color = null)
        {
            var result = ImageSet.FromSvgUrl(url, (int)imageSize.Width, (int)imageSize.Height, color);
            return result;
        }

        /// <summary>
        /// Creates <see cref="ImageSet"/> and loads Svg data from the specified
        /// <paramref name="url"/>.
        /// </summary>
        /// <remarks>
        /// Gets DPI settings from <paramref name="dpiControl"/> and selects appropriate
        /// images size using <see cref="ToolBarUtils.GetDefaultImageSize(Control)"/>.
        /// </remarks>
        /// <param name="url">File or resource url with Svg data.</param>
        /// <param name="dpiControl">Control which <see cref="Control.GetDPI"/> method
        /// is used to get DPI settings.</param>
        /// <param name="color">Svg fill color. Optional.
        /// If provided, svg fill color is changed to the specified value.</param>
        public static ImageSet LoadSvgImage(string url, Control dpiControl, Color? color = null)
        {
            return ToolBarUtils.FromSvgUrlForToolbar(url, dpiControl, color);
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
            AuiToolbarItemKind? itemKind = AuiToolbarItemKind.Normal,
            ImageSet? disabledBitmap = null,
            string? longHelpString = null)
        {
            int toolId = GenNewId();
            label ??= string.Empty;
            shortHelpString ??= label;
            longHelpString ??= string.Empty;
            itemKind ??= AuiToolbarItemKind.Normal;
            NativeControl.AddTool2(
                toolId,
                label,
                (UI.Native.ImageSet?)bitmap?.Handler,
                (UI.Native.ImageSet?)disabledBitmap?.Handler,
                (int)itemKind.Value,
                shortHelpString!,
                longHelpString!,
                IntPtr.Zero);
            return toolId;
        }

        public int AddToolButton(string? label, SvgImage image)
        {
            var imageSize = PanelAuiManager.GetBaseToolSvgSize().Width;
            return AddTool(
                        label,
                        image.AsNormal(imageSize, IsDarkBackground),
                        label,
                        AuiToolbarItemKind.Normal,
                        image.AsDisabled(imageSize, IsDarkBackground));
        }

        /// <summary>
        /// Gets kind of the tool item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public AuiToolbarItemKind GetToolKind(int toolId)
        {
            if (toolId <= 0)
                return AuiToolbarItemKind.Normal;

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
            NativeControl.AddControl(toolId, WxPlatform.WxWidget(control), string.Empty);
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
        public int AddSeparator()
        {
            int toolId = GenNewId();
            NativeControl.AddSeparator(toolId);
            return toolId;
        }

        /// <summary>
        /// Adds an empty space for spacing groups of tools.
        /// </summary>
        /// <param name="pixels">Width of an empty space.</param>
        /// <remarks>
        /// If <paramref name="pixels"/> is not specified,
        /// <see cref="DefaultSpacerWidth"/> is used as a spacer width.
        /// </remarks>
        public int AddSpacer(int? pixels = null)
        {
            pixels ??= DefaultSpacerWidth;
            int toolId = GenNewId();
            NativeControl.AddSpacer(toolId, pixels.Value);
            return toolId;
        }

        /// <summary>
        /// Adds a stretchable space to the toolbar.
        /// </summary>
        public int AddStretchSpacer(int proportion = 1)
        {
            int toolId = GenNewId();
            NativeControl.AddStretchSpacer(toolId, proportion);
            return toolId;
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
            toolData.Clear();
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
            if (toolId <= 0)
                return false;
            var result = NativeControl.DeleteTool(toolId);
            if (result)
                toolData.Remove(toolId);
            return result;
        }

        /// <summary>
        /// Finds tool by its ID and returns tool index (position).
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>The index, or position, of a previously
        /// added tool.</returns>
        public int GetToolIndex(int toolId)
        {
            if (toolId <= 0)
                return -1;
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
            if (toolId <= 0)
                return false;
            return NativeControl.GetToolFits(toolId);
        }

        /// <summary>
        /// Returns the specified tool rectangle in the toolbar.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Position and size of the tool in the toolbar in device-independent units
        /// (1/96 inch).</returns>
        public RectD GetToolRect(int toolId)
        {
            if (toolId <= 0)
                return RectD.Empty;
            return NativeControl.GetToolRect(toolId);
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return false;
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
        /// <remarks>You need to call <see cref="Control.Invalidate()"/> after changing
        /// enabled state of the tool.</remarks>
        public void EnableTool(int toolId, bool state)
        {
            if (toolId > 0)
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
            if (toolId > 0)
                return NativeControl.GetToolEnabled(toolId);
            else
                return false;
        }

        /// <inheritdoc/>
        public override void OnLayout()
        {
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return false;
            return NativeControl.GetToolDropDown(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item proportion.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="proportion">new toolbar item proportion</param>
        public void SetToolProportion(int toolId, int proportion)
        {
            if (toolId <= 0)
                return;
            NativeControl.SetToolProportion(toolId, proportion);
        }

        /// <summary>
        /// Gets the specified toolbar item proportion.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Toolbar item proportion value.</returns>
        public int GetToolProportion(int toolId)
        {
            if (toolId <= 0)
                return -1;
            return NativeControl.GetToolProportion(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="sticky">new Sticky property value.</param>
        public void SetToolSticky(int toolId, bool sticky)
        {
            if (toolId <= 0)
                return;
            NativeControl.SetToolSticky(toolId, sticky);
        }

        /// <summary>
        /// Shows context menu under the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="contextMenu">Context menu.</param>
        public void ShowDropDownMenu(int toolId, ContextMenu contextMenu)
        {
            if (toolId <= 0)
                return;
            ShowMe();

            void ShowMe()
            {
                if (showingDropDown > 0)
                    return;

                showingDropDown++;

                try
                {
                    Window? window = ParentWindow;
                    if (window == null)
                        return;

                    var pt = GetToolPopupLocation(toolId);
                    if (pt is not null)
                    {
                        SetToolSticky(toolId, true);
                        var menuLocation = window.ScreenToClient(pt.Value);
                        contextMenu?.Show(window, (menuLocation.X, menuLocation.Y));
                        SetToolSticky(toolId, false);
                    }
                }
                finally
                {
                    showingDropDown--;
                }
            }
        }

        /// <summary>
        /// Gets location of the popup window for the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Popup window location in screen coordinates.</returns>
        public PointD? GetToolPopupLocation(int toolId)
        {
            if (toolId <= 0)
                return null;
            RectD toolRect = GetToolRect(toolId);
            PointD pt = ClientToScreen(toolRect.BottomLeft);
            return pt;
        }

        /// <summary>
        /// Gets the specified toolbar item Sticky property value.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <returns>Toolbar item Sticky property value.</returns>
        public bool GetToolSticky(int toolId)
        {
            if (toolId <= 0)
                return false;
            return NativeControl.GetToolSticky(toolId);
        }

        /// <summary>
        /// Gets the specified toolbar item label text.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public string GetToolLabel(int toolId)
        {
            if (toolId <= 0)
                return string.Empty;
            return NativeControl.GetToolLabel(toolId);
        }

        /// <summary>
        /// Sets the specified toolbar item label text.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="label">New toolbar item label text.</param>
        public void SetToolLabel(int toolId, string label)
        {
            if (toolId <= 0)
                return;
            NativeControl.SetToolLabel(toolId, label);
        }

        /// <summary>
        /// Sets the specified toolbar item image.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="bitmap">New toolbar item image.</param>
        public void SetToolBitmap(int toolId, ImageSet? bitmap)
        {
            if (toolId <= 0)
                return;
            NativeControl.SetToolBitmap(toolId, (UI.Native.ImageSet?)bitmap?.Handler);
        }

        /// <summary>
        /// Returns the short help for the given tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <remarks>Usually short help is shown as a hint when mouse is
        /// over tool item.</remarks>
        public string GetToolShortHelp(int toolId)
        {
            if (toolId <= 0)
                return string.Empty;
            return NativeControl.GetToolShortHelp(toolId);
        }

        /// <summary>
        /// Registers event handler which is called when tool is clicked.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="ev">Event handler.</param>
        public void AddToolOnClick(int toolId, EventHandler ev)
        {
            if (toolId <= 0)
                return;
            var toolData = GetToolData(toolId, true);
            toolData!.Click += ev;
        }

        /// <summary>
        /// Gets name of the specified tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <remarks>
        /// Name of the tool can be used for the debug or other purposes.
        /// </remarks>
        public string? GetToolName(int toolId)
        {
            if (toolId <= 0)
                return string.Empty;
            var toolData = GetToolData(toolId, false);
            if (toolData == null)
                return null;
            return toolData.Name;
        }

        /// <summary>
        /// Sets name of the specified tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="name">Name of the tool</param>
        /// <remarks>
        /// Name of the tool can be used for the debug or other purposes.
        /// </remarks>
        public void SetToolName(int toolId, string? name)
        {
            if (toolId <= 0)
                return;
            var toolData = GetToolData(toolId, true);
            toolData!.Name = name;
        }

        /// <summary>
        /// Gets user data associated with the specified tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public object? GetToolTag(int toolId)
        {
            if (toolId <= 0)
                return null;
            var toolData = GetToolData(toolId, false);
            if (toolData == null)
                return null;
            return toolData.Tag;
        }

        /// <summary>
        /// Sets user data associated with the specified tool.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="tag">User data.</param>
        public void SetToolTag(int toolId, object? tag)
        {
            if (toolId <= 0)
                return;
            var toolData = GetToolData(toolId, true);
            toolData!.Tag = tag;
        }

        /// <summary>
        /// Sets a menu used as this toolbar item drop-down.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        /// <param name="menu">Drop down menu.</param>
        public void SetToolDropDownMenu(int toolId, ContextMenu menu)
        {
            if (toolId <= 0)
                return;
            var toolData = GetToolData(toolId, true);
            toolData!.DropDownMenu = menu;
        }

        /// <summary>
        /// Gets a menu used as this toolbar item drop-down.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public ContextMenu? GetToolDropDownMenu(int toolId)
        {
            if (toolId <= 0)
                return null;
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
            if (toolId <= 0)
                return;
            var toolData = GetToolData(toolId, true);
            toolData!.DropDownOnEvent = onEvent;
        }

        /// <summary>
        /// Gets type of the event which shows a drop down menu associated with the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added tool.</param>
        public AuiToolbarItemDropDownOnEvent GetToolDropDownOnEvent(int toolId)
        {
            if (toolId <= 0)
                return AuiToolbarItemDropDownOnEvent.None;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return string.Empty;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return;
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
            if (toolId <= 0)
                return;
            NativeControl.SetToolMinSize(toolId, width, height);
        }

        /// <summary>
        /// Gets minimal size of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public SizeI GetToolMinSize(int toolId)
        {
            if (toolId <= 0)
                return SizeI.Empty;
            return NativeControl.GetToolMinSize(toolId);
        }

        /// <summary>
        /// Gets minimal height of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public int GetToolMinHeight(int toolId)
        {
            if (toolId <= 0)
                return 0;
            return NativeControl.GetToolMinSize(toolId).Height;
        }

        /// <summary>
        /// Gets minimal heights of the toolbar items.
        /// </summary>
        /// <param name="toolIds">IDs of a previously added toolbar items.</param>
        public int[] GetToolMinHeights(params int[] toolIds)
        {
            var length = toolIds.Length;
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
                result[i] = GetToolMinHeight(toolIds[i]);
            return result;
        }

        /// <summary>
        /// Gets maximal value in minimal heights of the specified toolbar items.
        /// </summary>
        /// <param name="toolIds">IDs of a previously added toolbar items.</param>
        public int GetToolMaxOfMinHeights(params int[] toolIds)
        {
            var heights = GetToolMinHeights(toolIds);
            var result = MathUtils.Max(heights);
            return result;
        }

        /// <summary>
        /// Gets minimal width of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public int GetToolMinWidth(int toolId)
        {
            if (toolId <= 0)
                return 0;
            return NativeControl.GetToolMinSize(toolId).Width;
        }

        /// <summary>
        /// Gets alignment of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        public SizerFlag GetToolAlignment(int toolId)
        {
            if (toolId <= 0)
                return SizerFlag.AlignLeft;
            return (SizerFlag)NativeControl.GetAlignment(toolId);
        }

        /// <summary>
        /// Sets alignment of the toolbar item.
        /// </summary>
        /// <param name="toolId">ID of a previously added toolbar item.</param>
        /// <param name="align">New alignment value.</param>
        internal void SetToolAlignment(int toolId, SizerFlag align)
        {
            if (toolId <= 0)
                return;
            NativeControl.SetAlignment(toolId, (int)align);
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
            if (toolId <= 0)
                return default;
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
            if (toolId <= 0)
                return false;
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
        }

        internal void ProcessToolClick()
        {
            var toolData = GetToolData(EventToolId, false);
            if (toolData == null)
                return;
            BeginInvoke(() =>
            {
                toolData.OnClick(this, EventArgs.Empty);
            });

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

            BeginInvoke(() =>
            {
                ShowDropDownMenu(EventToolId, menu);
            });
        }

        internal void RaiseToolDropDown(EventArgs e)
        {
            OnToolDropDown(e);
            ToolDropDown?.Invoke(this, e);
            ProcessToolClick();
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
        internal bool DeleteByIndex(int index)
        {
            var result = NativeControl.DeleteByIndex(index);
            return result;
        }

        internal void DoOnLeftUp(int x, int y)
        {
            NativeControl.DoOnLeftUp(x, y);
        }

        /// <summary>
        /// Gets whether the specified tool fits in the toolbar visible area.
        /// </summary>
        /// <param name="index">The index, or position, of a previously
        /// added tool.</param>
        /// <returns>
        /// <c>true</c> if tool fits in the toolbar, <c>false</c> otherwise.
        /// </returns>
        internal bool GetToolFitsByIndex(int index)
        {
            return NativeControl.GetToolFitsByIndex(index);
        }

        internal void DoOnLeftDown(int x, int y)
        {
            NativeControl.DoOnLeftDown(x, y);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return new AuiToolbarHandler();
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

        private int GenNewId()
        {
            int result = idCounter;
            idCounter++;
            return result;
        }

        internal class ToolData
        {
            public event EventHandler? Click;

            public string? Name { get; set; }

            public object? Tag { get; set; }

            public ContextMenu? DropDownMenu { get; set; }

            public AuiToolbarItemDropDownOnEvent DropDownOnEvent { get; set; }

            public void OnClick(object? sender, EventArgs ev)
            {
                Click?.Invoke(sender, ev);
            }
        }
    }
}