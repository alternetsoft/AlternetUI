using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Alternet.Base.Collections;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Defines the base class for controls, which are components with
    /// visual representation.
    /// </summary>
    [DesignerCategory("Code")]
    [DefaultProperty("Text")]
    [DefaultEvent("Click")]
    public partial class Control
        : FrameworkElement, ISupportInitialize, IDisposable, IFocusable,
        IWin32Window, ITextProperty, IComponent, IControl, INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets or sets min element size.
        /// </summary>
        public static Coord MinElementSize = 32;

        /// <summary>
        /// Gets or sets whether <see cref="DebugBackgroundColor"/> property is used.
        /// </summary>
        public static bool UseDebugBackgroundColor = false;

        private static readonly SizeD DefaultControlSize = SizeD.NaN;
        private static int groupIndexCounter;
        private static Font? defaultFont;
        private static Font? defaultMonoFont;
        private static Control? hoveredControl;

        private ControlStyles controlStyle = ControlStyles.UserPaint | ControlStyles.StandardClick
            | ControlStyles.Selectable | ControlStyles.StandardDoubleClick
            | ControlStyles.AllPaintingInWmPaint | ControlStyles.UseTextForAccessibility;

        private bool enabled = true;
        private int handlerTextChanging;
        private int rowIndex;
        private int columnIndex;
        private int columnSpan = 1;
        private int rowSpan = 1;
        private ISite? site;
        private bool isMouseLeftButtonDown;
        private int layoutSuspendCount;
        private IFlagsAndAttributes? flagsAndAttributes;
        private MouseEventArgs? dragEventArgs;
        private PointD dragEventMousePos;
        private IComponentDesigner? designer;
        private Color? backgroundColor;
        private Color? foregroundColor;
        private bool parentBackgroundColor;
        private bool parentForegroundColor;
        private bool parentFont;
        private ControlCollection? children;
        private SizeD suggestedSize = DefaultControlSize;
        private Thickness margin;
        private Thickness padding;
        private string title = string.Empty;
        private IControlHandler? handler;
        private ControlStateSettings? stateObjects;
        private Font? font;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private bool inLayout;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        private ControlExtendedProps? extendedProps = null;
        private Thickness? minMargin;
        private Thickness? minPadding;
        private Thickness? minChildMargin;
        private bool visible = true;
        private Control? parent;
        private int updateCount = 0;
        private ControlFlags stateFlags;
        private Cursor? cursor;
        private string? toolTip;
        private ObjectUniqueId? uniqueId;
        private string? text;
        private DockStyle dock;
        private LayoutStyle? layout;
        private RectD reportedBounds = RectD.MinusOne;
        private Coord? scaleFactor;
        private SizeD? dpi;

        /// <summary>
        /// Initializes a new instance of the <see cref="Control"/> class.
        /// </summary>
        public Control()
        {
            var defaults = GetDefaults(ControlKind);
            defaults.RaiseInitDefaults(this);
            Designer?.RaiseCreated(this);
        }

        /// <summary>
        /// Gets the position of the mouse cursor in screen coordinates.
        /// </summary>
        /// <returns>
        /// A <see cref="PointD" /> that contains the coordinates of the mouse cursor
        /// relative to the upper-left corner of the screen.
        /// </returns>
        public static PointD MousePosition
        {
            get
            {
                return Mouse.GetPosition();
            }
        }

        /// <summary>
        /// Gets the default foreground color of the control.
        /// </summary>
        /// <returns>
        /// The default foreground <see cref="Color" /> of the control.
        /// The default is <see cref="SystemColors.ControlText" />.
        /// </returns>
        public static Color DefaultForeColor => SystemColors.ControlText;

        /// <summary>
        /// Gets the default background color of the control.
        /// </summary>
        /// <returns>
        /// The default background <see cref="Color" /> of the control.
        /// The default is <see cref="SystemColors.Control" />.</returns>
        public static Color DefaultBackColor => SystemColors.Control;

        /// <summary>
        /// Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in
        /// a pressed state.</summary>
        /// <returns>
        /// A bitwise combination of the <see cref="ModifierKeys" /> values.
        /// The default is <see cref="ModifierKeys.None" />.</returns>
        [Browsable(false)]
        public static ModifierKeys KeyModifiers => Keyboard.Modifiers;

        /// <summary>
        /// Gets the default font used for controls.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for UI controls. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font DefaultFont
        {
            get => defaultFont ?? Font.Default;
            set
            {
                defaultFont = value;
                FontFactory.Handler.SetDefaultFont(value);
            }
        }

        /// <summary>
        /// Gets hovered control.
        /// </summary>
        /// <remarks>
        /// Do not change this property, this is done by the library.
        /// </remarks>
        public static Control? HoveredControl
        {
            get => hoveredControl;

            set
            {
                if (hoveredControl == value)
                    return;
                hoveredControl = value;
                HoveredControlChanged?.Invoke(hoveredControl, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the default fixed width font used for controls.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for UI controls which require fixed width font.
        /// The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font DefaultMonoFont
        {
            get => defaultMonoFont ?? Font.DefaultMono;
            set
            {
                defaultMonoFont = value;
            }
        }

        object IControl.NativeControl => Handler.GetNativeControl();

        /// <summary>
        /// Gets scale factor used in device-independent units to/from
        /// pixels conversions.
        /// </summary>
        /// <returns></returns>
        [Browsable(false)]
        public virtual Coord ScaleFactor
        {
            get
            {
                return scaleFactor ??= Handler.GetPixelScaleFactor();
            }
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which can be used to measure text size
        /// and for other measure purposes.
        /// </summary>
        [Browsable(false)]
        public virtual Graphics MeasureCanvas
        {
            get
            {
                var measureCanvas = GraphicsFactory.GetOrCreateMemoryCanvas(ScaleFactor);
                return measureCanvas;
            }
        }

        /// <summary>
        /// Gets or sets border for all states of the control.
        /// </summary>
        [Browsable(false)]
        public virtual ControlStateBorders? Borders
        {
            get
            {
                return stateObjects?.Borders;
            }

            set
            {
                stateObjects ??= new();
                stateObjects.Borders = value;
            }
        }

        /// <summary>
        /// Gets or sets layout style of the child controls.
        /// </summary>
        [DefaultValue(null)]
        public virtual LayoutStyle? Layout
        {
            get
            {
                return layout;
            }

            set
            {
                if (layout == value)
                    return;
                layout = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this element responds to hit testing during
        /// user interaction.
        /// </summary>
        /// <remarks>
        /// The default value is <c>false</c>. Setting <see cref="InputTransparent"/> to <c>true</c>
        /// makes the element invisible to touch and pointer input. The input is passed to the first
        /// non-input-transparent element that is visually behind the input transparent element.
        /// </remarks>
        [Browsable(false)]
        public virtual bool InputTransparent { get; set; }

        /// <summary>
        /// Gets or sets whether layout rules are ignored for this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IgnoreLayout { get; set; }

        /// <summary>
        /// Gets or sets the title of the control.
        /// </summary>
        /// <value>The title of the control.</value>
        /// <remarks>
        /// It's up to control and its parent to decide on how this property will be used.
        /// For example if control is a child of the <see cref="TabControl"/>, <see cref="Title"/>
        /// is displayed as a tab text.
        /// </remarks>
        public virtual string Title
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                    return;

                title = value;
                RaiseTitleChanged();
            }
        }

        /// <summary>
        /// Gets or sets which control borders are docked to its parent control and determines
        /// how a control is resized with its parent.
        /// </summary>
        /// <returns>One of the <see cref="DockStyle" /> values.
        /// The default is <see cref="DockStyle.None" />.</returns>
        /// <remarks>
        /// Currently this property is used only when control is placed
        /// inside the <see cref="LayoutPanel"/>.
        /// </remarks>
        [Category("Layout")]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(DockStyle.None)]
        public virtual DockStyle Dock
        {
            get
            {
                return dock;
            }

            set
            {
                if (value != dock)
                {
                    dock = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ContextMenuStrip" /> associated
        /// with this control.</summary>
        /// <returns>The <see cref="ContextMenuStrip" /> for this control,
        /// or <see langword="null" /> if there is no attached <see cref="ContextMenuStrip"/>.
        /// The default is <see langword="null" />.</returns>
        [Category("Behavior")]
        [DefaultValue(null)]
        [Browsable(false)]
        public virtual ContextMenuStrip? ContextMenuStrip
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Input Method Editor (IME) mode of the control.
        /// </summary>
        /// <returns>One of the <see cref="ImeMode" /> values.
        /// The default is <see cref="ImeMode.Inherit" />.</returns>
        [Category("Behavior")]
        [Localizable(true)]
        [Browsable(false)]
        public virtual ImeMode ImeMode { get; set; } = ImeMode.Off;

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        [DefaultValue("")]
        public virtual string Text
        {
            get
            {
                return text ?? string.Empty;
            }

            set
            {
                value ??= string.Empty;

                var forced = StateFlags.HasFlag(ControlFlags.ForceTextChange);
                if (forced)
                    StateFlags &= ~ControlFlags.ForceTextChange;

                if (!forced && text == value)
                    return;
                text = value;

                if (handlerTextChanging == 0)
                {
                    var coercedText = CoerceTextForHandler(value);

                    if (forced || Handler.Text != coercedText)
                        Handler.Text = coercedText;
                }

                RaiseTextChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cursor that the control should normally display.
        /// </summary>
        /// <remarks>
        /// Notice that the control cursor also sets it for the children of the control implicitly.
        /// </remarks>
        /// <remarks>
        /// The cursor may be <c>null</c> in which case the control cursor will be
        /// reset back to default.
        /// </remarks>
        [Browsable(false)]
        public virtual Cursor? Cursor
        {
            get
            {
                return cursor;
            }

            set
            {
                if (cursor == value)
                    return;
                cursor = value;
                Handler.SetCursor(value);
            }
        }

        /// <summary>
        /// Gets unique id of this control.
        /// </summary>
        [Browsable(false)]
        public virtual ObjectUniqueId UniqueId
        {
            get
            {
                return uniqueId ??= new();
            }
        }

        /// <summary>
        /// Gets custom flags and attributes provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public virtual IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return flagsAndAttributes ??= Factory.CreateFlagsAndAttributes();
            }

            set
            {
                flagsAndAttributes = value;
            }
        }

        /// <summary>
        /// Gets custom flags provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomFlags CustomFlags => FlagsAndAttributes.Flags;

        /// <summary>
        /// Gets or sets cached data for the layout engine.
        /// </summary>
        [Browsable(false)]
        public virtual object? LayoutData
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets additional properties which are layout specific.
        /// </summary>
        [Browsable(false)]
        public virtual object? LayoutProps
        {
            get;
            set;
        }

        /// <summary>
        /// Gets custom attributes provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomAttributes CustomAttr => FlagsAndAttributes.Attr;

        /// <summary>
        /// Gets or sets size of the <see cref="Control"/>'s client area, in
        /// device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD ClientSize
        {
            get
            {
                if (IsDummy)
                    return SizeD.Empty;
                return Handler.ClientSize;
            }

            set
            {
                if (ClientSize == value)
                    return;
                Handler.ClientSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets control flags.
        /// </summary>
        [Browsable(false)]
        public virtual ControlFlags StateFlags
        {
            get => stateFlags;
            internal set => stateFlags = value;
        }

        /// <summary>
        /// Executes assigned action immediately.
        /// </summary>
        [Browsable(false)]
        public virtual Action<Control>? InitAction
        {
            get
            {
                return null;
            }

            set
            {
                value?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets whether layout is suspended.
        /// </summary>
        [Browsable(false)]
        public bool IsLayoutSuspended => layoutSuspendCount != 0;

        /// <summary>
        /// Gets whether layout is currently performed.
        /// </summary>
        [Browsable(false)]
        public bool IsLayoutPerform => inLayout;

        /// <summary>
        /// Gets or sets <see cref="IComponentDesigner"/> instance which
        /// connects control with the designer.
        /// </summary>
        [Browsable(false)]
        public virtual IComponentDesigner? Designer
        {
            get
            {
                return designer ?? ComponentDesigner.Default;
            }

            set
            {
                designer = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is over the
        /// <see cref="Control"/>.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseOver
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseCaptured
        {
            get
            {
                return Handler.IsMouseCaptured;
            }
        }

        /// <summary>
        /// Gets or sets data (images, colors, borders, pens, brushes, etc.) for different
        /// control states.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        [Browsable(false)]
        public virtual ControlStateSettings? StateObjects
        {
            get
            {
                return stateObjects;
            }

            set
            {
                stateObjects = value;
            }
        }

        IntPtr IWin32Window.Handle => default;

        /// <summary>
        /// Gets or sets background brushes attached to this control.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        [Browsable(false)]
        public virtual ControlStateBrushes? Backgrounds
        {
            get => stateObjects?.Backgrounds;
            set
            {
                stateObjects ??= new();
                stateObjects.Backgrounds = value;
            }
        }

        /// <summary>
        /// Gets or sets foreground brushes attached to this control.
        /// </summary>
        [Browsable(false)]
        public virtual ControlStateBrushes? Foregrounds
        {
            get => stateObjects?.Foregrounds;
            set
            {
                stateObjects ??= new();
                stateObjects.Foregrounds = value;
            }
        }

        /// <summary>
        /// Gets the distance, in dips, between the right edge of the control and the left
        /// edge of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the right edge of the
        /// control and the left edge of its container's client area.</returns>
        [Category("Layout")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Coord Right
        {
            get => Bounds.Right;
            set => Left = value - Width;
        }

        /// <summary>
        /// Gets the distance, in dips, between the bottom edge of the control and the top edge
        /// of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the bottom edge of
        /// the control and the top edge of its container's client area.</returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Category("Layout")]
        public Coord Bottom
        {
            get => Bounds.Bottom;
            set => Top = value - Height;
        }

        /// <summary>
        /// Gets control index in the <see cref="Children"/> of the container control.
        /// </summary>
        [Browsable(false)]
        public int? IndexInParent
        {
            get
            {
                var index = Parent?.children?.IndexOf(this);
                return index;
            }
        }

        /// <summary>
        /// Gets next visible sibling control.
        /// </summary>
        [Browsable(false)]
        public virtual Control? NextSibling
        {
            get
            {
                var index = IndexInParent;
                if (index is null)
                    return null;

                var chi = Parent!.Children;
                var count = chi.Count;

                for (int i = index.Value + 1; i < count; i++)
                {
                    var child = chi[i];
                    if (child.Visible)
                        return child;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the object that contains data about the control.
        /// </summary>
        /// <value>An <see cref="object"/> that contains data about the control.
        /// The default is <c>null</c>.</value>
        /// <remarks>
        /// Any type derived from the <see cref="object"/> class can be assigned
        /// to this property.
        /// A common use for the <see cref="Tag"/> property is to store data that
        /// is closely associated with the control.
        /// </remarks>
        [Browsable(false)]
        public virtual object? Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tool-tip that is displayed for this element
        /// in the user interface.
        /// </summary>
        [DefaultValue(null)]
        public virtual string? ToolTip
        {
            get
            {
                return toolTip;
            }

            set
            {
                if (toolTip == value)
                    return;
                toolTip = value;
                OnToolTipChanged(EventArgs.Empty);
                ToolTipChanged?.Invoke(this, EventArgs.Empty);
                Handler.SetToolTip(GetRealToolTip());
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> bounds relative to the parent,
        /// in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD Bounds
        {
            get => Handler.Bounds;
            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (Bounds == value)
                    return;
                Handler.Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance between the left edge of the control
        /// and the left edge of its container's client area.
        /// </summary>
        public virtual Coord Left
        {
            get
            {
                return Bounds.Left;
            }

            set
            {
                var bounds = Bounds;
                if (bounds.Left == value)
                    return;
                Bounds = new RectD(value, bounds.Top, bounds.Width, bounds.Height);
            }
        }

        /// <summary>
        /// Gets or sets the distance between the top edge of the control
        /// and the top edge of its container's client area.
        /// </summary>
        public virtual Coord Top
        {
            get
            {
                return Bounds.Top;
            }

            set
            {
                var bounds = Bounds;
                if (bounds.Top == value)
                    return;
                Bounds = new RectD(bounds.Left, value, bounds.Width, bounds.Height);
            }
        }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the control, in
        /// device-independent units.
        /// </summary>
        /// <value>The position of the control's upper-left corner, in device-independent units.</value>
        [Browsable(false)]
        public virtual PointD Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new RectD(value, Bounds.Size);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its
        /// child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public virtual bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;

                visible = value;
                OnVisibleChanged(EventArgs.Empty);
                VisibleChanged?.Invoke(this, EventArgs.Empty);
                Parent?.ChildVisibleChanged?.Invoke(Parent, new BaseEventArgs<Control>(this));
                Handler.Visible = value;
                Parent?.PerformLayout();
                if (visible)
                    AfterShow?.Invoke(this, EventArgs.Empty);
                else
                    AfterHide?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond
        /// to user interaction.
        /// </summary>
        /// <value><c>true</c> if the control can respond to user
        /// interaction; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        /// <remarks>
        /// With the <see cref="Enabled"/> property, you can enable or disable
        /// controls at run time.
        /// For example, you can disable controls that do not apply to the
        /// current state of the application.
        /// You can also disable a control to restrict its use. For example, a
        /// button can be disabled to prevent the user from clicking it.
        /// </remarks>
        /// <remarks>
        /// Notice that this property can return false even if this control itself hadn't been
        /// explicitly disabled when one of its parent controls is disabled. To get the intrinsic
        /// status of this control, use <see cref="IsThisEnabled"/>.
        /// </remarks>
        public virtual bool Enabled
        {
            get
            {
                return enabled && IsParentEnabled;
            }

            set
            {
                if (enabled == value)
                    return;
                enabled = value;
                RaiseEnabledChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is intrinsically enabled.
        /// </summary>
        /// <remarks>
        /// This property is mostly used for the library itself, user code should normally use
        /// <see cref="Enabled"/> instead.
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsThisEnabled
        {
            get
            {
                return enabled;
            }

            set
            {
                Enabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Parent"/> of this control can respond
        /// to user interaction.
        /// </summary>
        /// <remarks>
        /// If <see cref="Parent"/> is not specified, returns <c>true</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual bool IsParentEnabled
        {
            get
            {
                return Parent?.Enabled ?? true;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="IFileSystem"/> which is used in the control.
        /// If this property is <c>null</c> (default), <see cref="FileSystem.Default"/>
        /// is used as file system provider.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        [Browsable(false)]
        public virtual IFileSystem? FileSystem { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control has a native window
        /// handle associated with it.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if a native window handle has been assigned to the
        /// control; otherwise, <see langword="false" />.</returns>
        [Browsable(false)]
        public virtual bool IsHandleCreated
        {
            get
            {
                return (handler is not null) && handler.IsNativeControlCreated
                    && Handler.IsHandleCreated;
            }
        }

        /// <summary>
        /// Gets the collection of child controls contained within the control.
        /// </summary>
        /// <value>A <see cref="Collection{T}"/> representing the collection
        /// of controls contained within the control.</value>
        /// <remarks>
        /// A Control can act as a parent to a collection of controls.
        /// For example, when several controls are added to a
        /// <see cref="Window"/>, each of the controls is a member
        /// of the <see cref="Collection{T}"/> assigned to the
        /// <see cref="Children"/> property of the window, which is derived
        /// from the <see cref="Control"/> class.
        /// <para>You can manipulate the controls in the
        /// <see cref="Collection{T}"/> assigned to the <see cref="Children"/>
        /// property by using the methods available in the
        /// <see cref="Collection{T}"/> class.</para>
        /// <para>When adding several controls to a parent control, it
        /// is recommended that you call the <see cref="SuspendLayout"/> method
        /// before initializing the controls to be added.
        /// After adding the controls to the parent control, call the
        /// <see cref="ResumeLayout"/> method. Doing so will increase the
        /// performance of applications with many controls.</para>
        /// </remarks>
        [Content]
        [Browsable(false)]
        public virtual ControlCollection Children
        {
            get
            {
                if (children == null)
                {
                    children = new();
                    children.ItemInserted += Children_ItemInserted;
                    children.ItemRemoved += Children_ItemRemoved;
                }

                return children;
            }
        }

        /// <summary>
        /// Gets last time when mouse double click was done.
        /// </summary>
        [Browsable(false)]
        public long? LastDoubleClickTimestamp { get; set; }

        /// <summary>
        /// Same as <see cref="Children"/>.
        /// </summary>
        [Browsable(false)]
        public ControlCollection Controls => Children;

        /// <summary>
        /// Gets whether there are any items in the <see cref="Children"/> list.
        /// </summary>
        [Browsable(false)]
        public bool HasChildren
        {
            get
            {
                return children != null && children.Count > 0;
            }
        }

        /// <summary>
        /// Gets or sets the parent container of the control.
        /// </summary>
        [Browsable(false)]
        public virtual Control? Parent
        {
            get => parent;
            set
            {
                if (parent == value)
                    return;
                parent?.Children.Remove(this);
                value?.Children.Add(this);
                RaiseParentChanged();
                stateFlags |= ControlFlags.ParentAssigned;
            }
        }

        /// <summary>
        /// Gets the parent window of the control).
        /// </summary>
        [Browsable(false)]
        public virtual Window? ParentWindow
        {
            get
            {
                var result = Parent;
                while (true)
                {
                    if (result == null)
                        return null;
                    if (result is Window window)
                        return window;
                    result = result.Parent;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>The size of the control, in device-independent units.
        /// The default value is <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the control.
        /// Set this property to <see cref="SizeD"/>(<see cref="Coord.NaN"/>,
        /// <see cref="Coord.NaN"/>) to specify system-default sizing
        /// behavior when the control is first shown.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeD Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Bounds = new RectD(Bounds.Location, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <value>The width of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the width of the control.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        public virtual Coord Width
        {
            get => Size.Width;
            set => Size = new(value, Height);
        }

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        /// <value>The height of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the height of the control.
        /// Set this property to <see cref="Coord.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        public virtual Coord Height
        {
            get => Size.Height;
            set => Size = new(Width, value);
        }

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent units.
        /// The default value is <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual
        /// size is calculated by the layout system.
        /// Set this property to <see cref="SizeD"/>
        /// (<see cref="Coord.NaN"/>, <see cref="Coord.NaN"/>) to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        [Browsable(false)]
        public virtual SizeD SuggestedSize
        {
            get
            {
                return suggestedSize;
            }

            set
            {
                if (suggestedSize == value)
                    return;

                suggestedSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the suggested width of the control.
        /// </summary>
        /// <value>The suggested width of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the control. An
        /// actual width is calculated by the layout system.
        /// Set this property to <see cref="Coord.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        public Coord SuggestedWidth
        {
            get => suggestedSize.Width;

            set
            {
                SuggestedSize = new(value, SuggestedSize.Height);
            }
        }

        /// <summary>
        /// Gets or sets the suggested height of the control.
        /// </summary>
        /// <value>The suggested height of the control, in device-independent units.
        /// The default value is <see cref="Coord.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the control. An
        /// actual height is calculated by the layout system.
        /// Set this property to <see cref="Coord.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        public Coord SuggestedHeight
        {
            get => suggestedSize.Height;

            set
            {
                SuggestedSize = new(SuggestedSize.Width, value);
            }
        }

        /// <summary>
        /// Gets or set a value indicating whether the control paints itself
        /// rather than the operating system doing so.
        /// </summary>
        /// <value>If <c>true</c>, the control paints itself rather than the
        /// operating system doing so.
        /// If <c>false</c>, the <see cref="Paint"/> event is not raised.</value>
        [Browsable(false)]
        public virtual bool UserPaint
        {
            get => Handler.UserPaint;
            set
            {
                if (value && !CanUserPaint)
                    return;
                Handler.UserPaint = value;
            }
        }

        /// <summary>
        /// Gets whether left mouse button is over control and is down.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseLeftButtonDown
        {
            get
            {
                return isMouseLeftButtonDown && IsMouseOver;
            }

            internal set
            {
                isMouseLeftButtonDown = value;
            }
        }

        /// <summary>
        /// Gets or sets override value for the <see cref="VisualState"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="VisualStateOverride"/> is specified, it's value
        /// used instead of dynamic state calculation when <see cref="VisualState"/>
        /// returns its value.
        /// </remarks>
        [Browsable(false)]
        public virtual VisualControlState? VisualStateOverride { get; set; }

        /// <summary>
        /// Gets current <see cref="VisualControlState"/>.
        /// </summary>
        [Browsable(false)]
        public virtual VisualControlState VisualState
        {
            get
            {
                if (VisualStateOverride is not null)
                    return VisualStateOverride.Value;

                if (!Enabled)
                    return VisualControlState.Disabled;
                if (IsMouseOver)
                {
                    if (IsMouseLeftButtonDown)
                        return VisualControlState.Pressed;
                    else
                        return VisualControlState.Hovered;
                }

                if (Focused)
                    return VisualControlState.Focused;
                return VisualControlState.Normal;
            }
        }

        /// <summary>
        /// Gets whether user paint is supported for this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanUserPaint => true;

        /// <summary>
        /// Gets or sets minimal value of the child's <see cref="Margin"/> property.
        /// </summary>
        public virtual Thickness? MinChildMargin
        {
            get
            {
                return minChildMargin;
            }

            set
            {
                if (minChildMargin == value)
                    return;
                minChildMargin = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the outer margin of a control.
        /// </summary>
        /// <value>Provides margin values for the control. The default value is a
        /// <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The margin is the space between this control and the adjacent control.
        /// Margin is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the margin can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Margin"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        public virtual Thickness Margin
        {
            get
            {
                if (minMargin == null)
                    margin.ApplyMin(MinMargin);
                if (Parent is not null && Parent.MinChildMargin is not null)
                {
                    var result = margin;
                    result.ApplyMin(Parent.MinChildMargin.Value);
                    return result;
                }

                return margin;
            }

            set
            {
                value.ApplyMin(MinMargin);
                if (margin == value)
                    return;

                margin = value;

                OnMarginChanged(EventArgs.Empty);
                MarginChanged?.Invoke(this, EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the padding inside a control.
        /// </summary>
        /// <value>Provides padding values for the control. The default value is
        /// a <see cref="Thickness"/> with all properties equal to 0 (zero).</value>
        /// <remarks>
        /// The padding is the amount of space between the content of a
        /// <see cref="Control"/> and its border.
        /// Padding is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the padding can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Padding"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        public virtual Thickness Padding
        {
            get
            {
                if (minPadding == null)
                    padding.ApplyMin(MinPadding);
                return padding;
            }

            set
            {
                value.ApplyMin(MinPadding);
                if (padding == value)
                    return;

                padding = value;

                OnPaddingChanged(EventArgs.Empty);
                PaddingChanged?.Invoke(this, EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Getw whether control is performing updates.
        /// </summary>
        /// <remarks>
        /// This property is <c>true</c> after call to <see cref="BeginUpdate"/>
        /// and before call to <see cref="EndUpdate"/>.
        /// </remarks>
        [Browsable(false)]
        public bool InUpdates => updateCount > 0;

        /// <summary>
        /// Gets or sets the minimum size the window can be resized to.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD MinimumSize
        {
            get
            {
                return Handler.MinimumSize;
            }

            set
            {
                if (MinimumSize == value)
                    return;
                Handler.MinimumSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the maximum size the window can be resized to.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD MaximumSize
        {
            get
            {
                return Handler.MaximumSize;
            }

            set
            {
                if (MaximumSize == value)
                    return;
                Handler.MaximumSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the minimum width the window can be resized to.
        /// </summary>
        public virtual Coord? MinWidth
        {
            get
            {
                var result = MinimumSize.Width;
                if (result == -1 || result == 0)
                    return null;
                return result;
            }

            set
            {
                if (value is null)
                    MinimumSize = new(-1, MinimumSize.Height);
                else
                    MinimumSize = new(value.Value, MinimumSize.Height);
            }
        }

        /// <summary>
        /// Gets or sets the minimum height the window can be resized to.
        /// </summary>
        public virtual Coord? MinHeight
        {
            get
            {
                var result = MinimumSize.Height;
                if (result == -1 || result == 0)
                    return null;
                return result;
            }

            set
            {
                if (value is null)
                    MinimumSize = new(MinimumSize.Width, -1);
                else
                    MinimumSize = new(MinimumSize.Width, value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the maximum width the window can be resized to.
        /// </summary>
        public virtual Coord? MaxWidth
        {
            get
            {
                var result = MaximumSize.Width;
                if (result == -1 || result == 0)
                    return null;
                return result;
            }

            set
            {
                if (value is null)
                    MaximumSize = new(-1, MaximumSize.Height);
                else
                    MaximumSize = new(value.Value, MaximumSize.Height);
            }
        }

        /// <summary>
        /// Gets or sets the maximum height the window can be resized to.
        /// </summary>
        public virtual Coord? MaxHeight
        {
            get
            {
                var result = MaximumSize.Height;
                if (result == -1 || result == 0)
                    return null;
                return result;
            }

            set
            {
                if (value is null)
                    MaximumSize = new(MaximumSize.Width, -1);
                else
                    MaximumSize = new(MaximumSize.Width, value.Value);
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Browsable(true)]
        public virtual Color? BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                if (backgroundColor == value)
                    return;
                backgroundColor = value;

                if (backgroundColor is null)
                    ResetBackgroundColor(ResetColorType.Auto);
                else
                    Handler.BackgroundColor = backgroundColor;
                Refresh();

                foreach (var child in Children)
                {
                    if (child.ParentBackColor)
                        child.BackgroundColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="BackgroundColor"/> is automatically
        /// updated when parent's <see cref="BackgroundColor"/> is changed.
        /// </summary>
        public virtual bool ParentBackColor
        {
            get => parentBackgroundColor;
            set
            {
                if (parentBackgroundColor == value)
                    return;
                parentBackgroundColor = value;
                if (value && Parent is not null)
                    BackgroundColor = Parent.BackgroundColor;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ForegroundColor"/> is automatically
        /// updated when parent's <see cref="ForegroundColor"/> is changed.
        /// </summary>
        public virtual bool ParentForeColor
        {
            get => parentForegroundColor;
            set
            {
                if (parentForegroundColor == value)
                    return;
                parentForegroundColor = value;
                if (value && Parent is not null)
                    ForegroundColor = Parent.ForegroundColor;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Font"/> is automatically
        /// updated when parent's <see cref="Font"/> is changed.
        /// </summary>
        public virtual bool ParentFont
        {
            get => parentFont;
            set
            {
                if (parentFont == value)
                    return;
                parentFont = value;
                if (value && Parent is not null)
                    Font = Parent.Font;
            }
        }

        /// <summary>
        /// Returns <see cref="ControlSet"/> filled with <see cref="Children"/>.
        /// </summary>
        [Browsable(false)]
        public virtual ControlSet ChildrenSet => new(Children);

        /// <summary>
        /// Gets real background color for the control.
        /// </summary>
        /// <remarks>
        /// This property returns color value even if <see cref="BackgroundColor"/>
        /// is <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual Color RealBackgroundColor
        {
            get
            {
                return Handler.BackgroundColor;
            }
        }

        /// <summary>
        /// Gets real foreground color for the control.
        /// </summary>
        /// <remarks>
        /// This property returns color value even if <see cref="ForegroundColor"/>
        /// is <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual Color RealForegroundColor
        {
            get
            {
                return Handler.ForegroundColor;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color ForeColor
        {
            get
            {
                var result = ForegroundColor;

                if (result is null)
                    return RealForegroundColor;
                else
                    return result;
            }

            set
            {
                ForegroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        [Browsable(false)]
        public virtual Color BackColor
        {
            get
            {
                var result = BackgroundColor;

                if (result is null)
                    return RealBackgroundColor;
                else
                    return result;
            }

            set
            {
                BackgroundColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
        [Browsable(true)]
        public virtual Color? ForegroundColor
        {
            get
            {
                return foregroundColor;
            }

            set
            {
                if (foregroundColor == value && value != null)
                    return;
                foregroundColor = value;

                if (foregroundColor is null)
                    ResetForegroundColor(ResetColorType.Auto);
                else
                    Handler.ForegroundColor = foregroundColor;
                Refresh();

                foreach (var child in Children)
                {
                    if (child.ParentForeColor)
                        child.ForegroundColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets group indexes which are assigned to this control.
        /// </summary>
        [Browsable(false)]
        public virtual int[]? GroupIndexes { get; set; }

        /// <summary>
        /// Gets or sets group indexes of this control. Group indexes are used
        /// in <see cref="GetGroup(int, bool)"/> method.
        /// </summary>
        /// <remarks>
        /// This property modifies <see cref="GroupIndexes"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual int? GroupIndex
        {
            get
            {
                if (GroupIndexes is null || GroupIndexes.Length == 0)
                    return null;
                return GroupIndexes[0];
            }

            set
            {
                if (value is null)
                    GroupIndexes = null;
                else
                    GroupIndexes = new int[] { value.Value };
            }
        }

        /// <summary>
        /// Gets intrinsic layout padding of the native control.
        /// </summary>
        [Browsable(false)]
        public virtual Thickness IntrinsicLayoutPadding
        {
            get => Handler.IntrinsicLayoutPadding;
        }

        /// <summary>
        /// Gets intrinsic preferred size padding of the native control.
        /// </summary>
        [Browsable(false)]
        public virtual Thickness IntrinsicPreferredSizePadding
            => Handler.IntrinsicPreferredSizePadding;

        /// <summary>
        /// Gets or sets column index which is used in <see cref="GetColumnGroup"/> and
        /// by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        [Browsable(false)]
        public virtual int ColumnIndex
        {
            get => columnIndex;

            set
            {
                if (value < 0)
                    value = 0;
                if (value == columnIndex)
                    return;
                columnIndex = value;
                RaiseCellChanged();
            }
        }

        /// <summary>
        /// Gets or sets row index which is used in <see cref="GetRowGroup"/> and
        /// by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        [Browsable(false)]
        public virtual int RowIndex
        {
            get => rowIndex;

            set
            {
                if (value < 0)
                    value = 0;
                if (value == rowIndex)
                    return;
                rowIndex = value;
                RaiseCellChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the total number of columns
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        [Browsable(false)]
        public virtual int ColumnSpan
        {
            get => columnSpan;
            set
            {
                if (value < 1)
                    value = 1;
                if (value == columnSpan)
                    return;
                columnSpan = value;
                RaiseCellChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates the total number of rows
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        [Browsable(false)]
        public virtual int RowSpan
        {
            get => rowSpan;

            set
            {
                if (value < 1)
                    value = 1;
                if (value == rowSpan)
                    return;
                rowSpan = value;
                RaiseCellChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background brush for the control.
        /// </summary>
        [Browsable(false)]
        public virtual Brush? Background
        {
            get => Backgrounds?.Normal;
            set
            {
                Backgrounds ??= new();
                Backgrounds.Normal = value;
                BackgroundChanged?.Invoke(this, EventArgs.Empty);
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets background images attached to this control.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        [Browsable(false)]
        public virtual ControlStateImages? BackgroundImages
        {
            get => stateObjects?.BackgroundImages;
            set
            {
                stateObjects ??= new();
                stateObjects.BackgroundImages = value;
            }
        }

        /// <summary>
        /// Gets or sets the background image displayed in the control.
        /// </summary>
        /// <returns>An <see cref="Image" /> that represents the image to display in
        /// the background of the control.</returns>
        [Category("Appearance")]
        [DefaultValue(null)]
        [Localizable(true)]
        [Browsable(false)]
        public virtual Image? BackgroundImage
        {
            get
            {
                return BackgroundImages?.Normal;
            }

            set
            {
                BackgroundImages ??= new();
                BackgroundImages.Normal = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground brush for the control.
        /// </summary>
        [Browsable(false)]
        public virtual Brush? Foreground
        {
            get => Foregrounds?.Normal;
            set
            {
                Foregrounds ??= new();
                Foregrounds.Normal = value;
                ForegroundChanged?.Invoke(this, EventArgs.Empty);
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="Font"/> to apply to the text displayed by
        /// the control. The default is the value of <c>null</c>.</value>
        public virtual Font? Font
        {
            get => font;
            set
            {
                if (font == value)
                    return;

                PerformLayoutAndInvalidate(() =>
                {
                    font = value;
                    OnFontChanged(EventArgs.Empty);
                    FontChanged?.Invoke(this, EventArgs.Empty);

                    Handler.Font = value;

                    foreach (var child in Children)
                    {
                        if (child.ParentFont)
                            child.Font = value;
                    }
                });
            }
        }

        /// <summary>
        /// Gets real font value.
        /// </summary>
        /// <remarks>
        /// Returns font even if <see cref="Font"/> property is <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual Font? RealFont => Handler.Font;

        /// <summary>
        /// Gets or sets whether control's font is bold.
        /// </summary>
        public virtual bool IsBold
        {
            get
            {
                return Handler.IsBold;
            }

            set
            {
                if (IsBold == value)
                    return;
                PerformLayoutAndInvalidate(() =>
                {
                    Handler.IsBold = value;
                });
            }
        }

        /// <summary>
        /// Returns true if control's background color is darker than foreground color.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsDarkBackground
        {
            get
            {
                var foregroundColor = RealForegroundColor;
                var backgroundColor = RealBackgroundColor;

                if (backgroundColor.IsBlack)
                    return true;

                if (foregroundColor.IsEmpty || backgroundColor.IsEmpty)
                    return SystemSettings.IsUsingDarkBackground;

                return SystemSettings.IsDarkBackground(foregroundColor, backgroundColor);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it
        /// is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is
        /// <c>null</c>.</value>
        public virtual VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                VerticalAlignmentChanged?.Invoke(this, EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets <see cref="Control.Children"/> or an empty array if there are no
        /// child controls.
        /// </summary>
        /// <remarks>
        /// This method doesn't allocate memory if there are no children.
        /// </remarks>
        [Browsable(false)]
        public IReadOnlyList<Control> AllChildren
        {
            get
            {
                if (HasChildren)
                    return Children;

                return Array.Empty<Control>();
            }
        }

        /// <summary>
        /// Gets all child controls which are visible and included in the layout.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="AllChildren"/>, <see cref="Control.Visible"/>
        /// and <see cref="Control.IgnoreLayout"/> properties.
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<Control> AllChildrenInLayout
        {
            get
            {
                var controls = AllChildren;
                var all = true;

                foreach (var control in controls)
                {
                    if (ChildIgnoresLayout(control))
                    {
                        all = false;
                        break;
                    }
                }

                if (all)
                    return controls;

                List<Control> result = new();

                foreach (var control in controls)
                {
                    if (!ChildIgnoresLayout(control))
                        result.Add(control);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when
        /// it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is
        /// <c>null</c>.</value>
        public virtual HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                HorizontalAlignmentChanged?.Invoke(this, EventArgs.Empty);
                PerformLayout();
            }
        }

        /// <summary>
        /// Returns rectangle in which custom drawing need to be performed.
        /// Useful for custom draw controls
        /// </summary>
        [Browsable(false)]
        public virtual RectD DrawClientRectangle
        {
            get
            {
                var size = ClientSize;
                return new(0, 0, size.Width, size.Height);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data
        /// that the user drags onto it.
        /// </summary>
        /// <value><c>true</c> if drag-and-drop operations are allowed in the
        /// control; otherwise, <c>false</c>. The default is <c>false</c>.</value>
        [Browsable(false)]
        public virtual bool AllowDrop
        {
            get => Handler.AllowDrop;
            set => Handler.AllowDrop = value;
        }

        /// <summary>
        /// Gets or sets the background style of the control.
        /// </summary>
        /// <remarks><see cref="ControlBackgroundStyle.Transparent"/> style is not possible
        /// to set as it is not supported on all platforms.</remarks>
        [Browsable(false)]
        public ControlBackgroundStyle BackgroundStyle
        {
            get
            {
                return Handler.BackgroundStyle;
            }

            set
            {
                if (value == ControlBackgroundStyle.Transparent)
                    return;
                Handler.BackgroundStyle = value;
            }
        }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// <see cref="Control"/>, in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD ClientRectangle => new(PointD.Empty, ClientSize);

        /// <summary>
        /// Gets a rectangle which describes an area inside of the
        /// <see cref="Control"/> available
        /// for positioning (layout) of its child controls, in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD ChildrenLayoutBounds
        {
            get
            {
                var childrenBounds = ClientRectangle;
                if (childrenBounds.SizeIsEmpty)
                    return RectD.Empty;

                var padding = Padding;
                var intrinsicPadding = Handler.IntrinsicLayoutPadding;

                return new RectD(
                    new PointD(
                        padding.Left + intrinsicPadding.Left,
                        padding.Top + intrinsicPadding.Top),
                    childrenBounds.Size - padding.Size - intrinsicPadding.Size);
            }
        }

        /// <summary>
        /// Gets a <see cref="IControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public virtual IControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the language direction for this control.
        /// </summary>
        /// <remarks>
        /// Note that <see cref="LangDirection.Default"/> is returned if layout direction
        /// is not supported.
        /// </remarks>
        [Browsable(false)]
        public virtual LangDirection LangDirection
        {
            get
            {
                return Handler.LangDirection;
            }

            set
            {
                if (value == LangDirection.Default)
                    return;
                Handler.LangDirection = value;
            }
        }

        /// <summary>
        /// Gets or sets the site of the control.
        /// </summary>
        /// <returns>The <see cref="System.ComponentModel.ISite" /> associated
        /// with the <see cref="Control" />, if any.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [Browsable(false)]
        public virtual ISite? Site
        {
            get => site;
            set => site = value;
        }

        /// <summary>
        /// Returns control identifier.
        /// </summary>
        [Browsable(false)]
        public virtual ControlTypeId ControlKind => ControlTypeId.Control;

        /// <summary>
        /// Gets or sets whether <see cref="Idle"/> event is fired.
        /// </summary>
        [Browsable(false)]
        public virtual bool ProcessIdle
        {
            get
            {
                return Handler.ProcessIdle;
            }

            set
            {
                Handler.ProcessIdle = value;
            }
        }

        /// <summary>
        /// Gets absolute position of the control. Returned value is <see cref="Location"/>
        /// plus all control's parents locations.
        /// </summary>
        [Browsable(false)]
        public PointD AbsolutePosition
        {
            get
            {
                PointD origin;

                if (ParentWindow == null)
                    origin = PointD.MinValue;
                else
                    origin = PointD.Empty;

                var ancestors = AllParents;

                var result = Location;

                foreach(var item in ancestors)
                {
                    result += item.Location;
                }

                return origin + result;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether this control or it's child controls have validation errors.
        /// </summary>
        /// <returns><c>true</c> if the control currently has validation errors;
        /// otherwise, <c>false</c>.</returns>
        [Browsable(false)]
        public virtual bool HasErrors
        {
            get
            {
                return GetErrors(null).Cast<object>().Any();
            }
        }

        /// <summary>
        /// Enumerates all parent controls.
        /// </summary>
        [Browsable(false)]
        public IEnumerable<Control> AllParents
        {
            get
            {
                var result = Parent;

                while (result != null)
                {
                    yield return result;
                    result = result.Parent;
                }
            }
        }

        /// <summary>
        /// Gets number of children items.
        /// </summary>
        [Browsable(false)]
        public int ChildCount => Children.Count;

        IControl? IControl.NextSibling => NextSibling;

        IControl? IControl.Parent
        {
            get => Parent;
            set => Parent = value as Control;
        }

        IWindow? IControl.ParentWindow => ParentWindow;

        /// <inheritdoc />
        internal override IEnumerable<FrameworkElement> LogicalChildrenCollection
            => HasChildren ? Children : Array.Empty<FrameworkElement>();

        /// <inheritdoc/>
        [Browsable(false)]
        internal override IReadOnlyList<FrameworkElement> ContentElements
        {
            get
            {
                if (children == null)
                    return Array.Empty<FrameworkElement>();
                return children;
            }
        }

        /// <summary>
        /// Gets a value indicating which of the modifier keys (SHIFT, CTRL, and ALT) is in
        /// a pressed state.</summary>
        /// <returns>
        /// A bitwise combination of the <see cref="Keys" /> values.
        /// The default is <see cref="Keys.None" />.</returns>
        protected static Keys ModifierKeys
        {
            get
            {
                var modifiers = Keyboard.Modifiers;
                return modifiers.ToKeys();
            }
        }

        /// <summary>
        /// Gets or sets border style of the control.
        /// </summary>
        protected virtual ControlBorderStyle BorderStyle
        {
            get
            {
                return Handler.BorderStyle;
            }

            set
            {
                Handler.BorderStyle = value;
            }
        }

        /// <summary>
        /// Gets whether this control is dummy control.
        /// </summary>
        protected virtual bool IsDummy => false;

        /// <summary>
        /// Gets child control at the specified index in the collection of child controls.
        /// </summary>
        /// <param name="index">Index of the child control.</param>
        /// <returns></returns>
        public IControl GetControl(int index) => Children[index];
    }
}