using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Alternet.Base.Collections;
using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the method that will handle paint event.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PaintEventArgs" /> that contains the event data.</param>
    public delegate void PaintEventHandler(object? sender, PaintEventArgs e);

    /// <summary>
    /// Defines the base abstract class for controls, which are components with
    /// visual representation.
    /// </summary>
    [DesignerCategory("Code")]
    [DefaultProperty("Text")]
    [DefaultEvent("Click")]
    public abstract partial class AbstractControl
        : FrameworkElement, ISupportInitialize, IDisposable, IFocusable,
        IWin32Window, ITextProperty, IComponent, IControl, INotifyDataErrorInfo
    {
        /// <summary>
        /// Gets or sets default value for <see cref="ParentFont"/> property.
        /// </summary>
        public static bool DefaultUseParentFont = false;

        /// <summary>
        /// Gets or sets min element size in device-independent units.
        /// </summary>
        public static Coord MinElementSize = 32;

        /// <summary>
        /// Gets or sets whether <see cref="DebugBackgroundColor"/> property is used.
        /// </summary>
        public static bool UseDebugBackgroundColor = false;

        private static readonly SizeD DefaultControlSize = SizeD.NaN;

        private static long? mouseWheelTimestamp;
        private static int groupIndexCounter;
        private static Font? defaultFont;
        private static Font? defaultMonoFont;
        private static AbstractControl? hoveredControl;
        private static List<IControlNotification>? globalNotifications;

        private bool enabled = true;
        private bool isMouseLeftButtonDown;
        private bool parentBackgroundColor;
        private bool parentForegroundColor;
        private bool parentFont = DefaultUseParentFont;
        private bool ignoreSuggestedWidth;
        private bool ignoreSuggestedHeight;
        private bool inLayout;
        private bool visible = true;

        private ControlStyles controlStyle = ControlStyles.UserPaint | ControlStyles.StandardClick
            | ControlStyles.Selectable | ControlStyles.StandardDoubleClick
            | ControlStyles.AllPaintingInWmPaint | ControlStyles.UseTextForAccessibility;

#pragma warning disable
        private Graphics? measureCanvas;
#pragma warning restore

        private Coord? scaleFactorOverride;
        private Coord? scaleFactor;

        private Color? textBackColor;
        private Color? backgroundColor;
        private Color? foregroundColor;
        private FontStyle fontStyle;
        private Font? font;

        private SizeD minimumSize;
        private SizeD maximumSize;
        private SizeD? dpi;
        private SizeD suggestedSize = DefaultControlSize;

        private PointD layoutOffset;
        private SizeD? layoutMaxSize;

        private Collection<InputBinding>? inputBindings;
        private Caret? caret;
        private WindowSizeToContentMode minSizeGrowMode = WindowSizeToContentMode.None;
        private CaretInfo? caretInfo;
        private MouseEventArgs? dragEventArgs;
        private PointD lastMouseDownPos;
        private IComponentDesigner? designer;
        private ControlCollection? children;
        private object? title;
        private ControlStateSettings? stateObjects;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        private ControlExtendedProps? extendedProps = null;
        private AbstractControl? parent;
        private ControlFlags stateFlags;
        private Cursor? cursor;
        private DockStyle dock;
        private LayoutStyle? layout;
        private List<IControlNotification>? notifications;
        private int paintCounter;

        private RectD reportedBounds = RectD.MinusOne;
        private RectD bounds;

        private int rowIndex;
        private int columnIndex;
        private int columnSpan = 1;
        private int rowSpan = 1;
        private int layoutSuspendCount;
        private int updateCount = 0;

        private Thickness margin;
        private Thickness padding;
        private Thickness? minMargin;
        private Thickness? minPadding;
        private Thickness? minChildMargin;

        private string? toolTip;
        private string? text;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractControl"/> class.
        /// </summary>
        public AbstractControl()
        {
            var defaults = GetDefaults(ControlKind);
            defaults.RaiseInitDefaults(this);
            OnCreateControl();
            Designer?.RaiseCreated(this, EventArgs.Empty);
            RaiseNotifications((n) => n.AfterCreate(this, EventArgs.Empty));
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
        public static AbstractControl? HoveredControl
        {
            get => hoveredControl;

            set
            {
                if (hoveredControl == value)
                    return;
                hoveredControl?.RaiseVisualStateChanged(EventArgs.Empty);

                if (value is not null)
                {
                    if (!value.IsMouseOver)
                        value = null;
                }

                hoveredControl = value;
                HoveredControlChanged?.Invoke(hoveredControl, EventArgs.Empty);
                hoveredControl?.RaiseVisualStateChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets global collection of the attached <see cref="IControlNotification"/> objects.
        /// These notifications are called for the each created control.
        /// </summary>
        public static IEnumerable<IControlNotification> GlobalNotifications
        {
            get
            {
                if (globalNotifications is null)
                    return Array.Empty<IControlNotification>();
                return globalNotifications;
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
                if (ScaleFactorOverride is not null)
                    return ScaleFactorOverride.Value;

                var newScaleFactor = RequestScaleFactor();

                if(newScaleFactor is not null)
                {
                    scaleFactor ??= newScaleFactor;
                }

                if (scaleFactor is null)
                    return Display.Primary.ScaleFactor;

                return scaleFactor.Value;
            }
        }

        /// <summary>
        /// Gets an override value
        /// </summary>
        [Browsable(false)]
        public virtual Coord? ScaleFactorOverride
        {
            get
            {
                return scaleFactorOverride;
            }

            set
            {
                scaleFactorOverride = value;
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
                Graphics? result = null;

                if(Graphics.RequireMeasure(ScaleFactor, ref result))
                {
                    measureCanvas = result;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the TAB key is received and
        /// processed by the control.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the TAB key is received by the control;
        /// <see langword="false" />, if the TAB key is not received by the control.
        /// The default is <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// Normally, TAB key is used for passing to the next control in a dialog.
        /// </remarks>
        public virtual bool WantTab { get; set; }

        /// <summary>
        /// Gets or sets the tab order of the control within its container.
        /// </summary>
        public virtual int TabIndex { get; set; }

        /// <summary>
        /// Gets time when this control was last clicked.
        /// </summary>
        [Browsable(false)]
        public long? LastClickedTimestamp
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets mouse position when mouse down event was received.
        /// </summary>
        [Browsable(false)]
        public PointD LastMouseDownPos => lastMouseDownPos;

        /// <summary>
        /// Gets or sets whether <see cref="LongTap"/> event is raised.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanLongTap { get; set; }

        /// <summary>
        /// Gets or sets whether key presses are sent to the parent control.
        /// </summary>
        [Browsable(false)]
        public virtual bool BubbleKeys { get; set; }

        /// <summary>
        /// Gets or sets border for all visual states of the control.
        /// Usage of this property depends on the control. Not all controls support it.
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

        /// <inheritdoc/>
        [Browsable(false)]
        public virtual object NativeControl => this;

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
        /// Gets or sets the title of the control as object.
        /// There is also <see cref="Title"/> property.
        /// </summary>
        /// <value>The title of the control.</value>
        /// <remarks>
        /// It's up to control and its parent to decide on how this property will be used.
        /// For example if control is a child of the <see cref="TabControl"/>, <see cref="Title"/>
        /// is displayed as a tab text.
        /// </remarks>
        /// <remarks>
        /// <see cref="TitleAsObject"/> and <see cref="Title"/> are stored in the same field.
        /// </remarks>
        [Browsable(false)]
        public virtual object? TitleAsObject
        {
            get
            {
                return title;
            }

            set
            {
                if (title == value)
                    return;
                if (title?.Equals(value) ?? false)
                    return;
                title = value;
                RaiseTitleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="CharValidator"/> is ignored. Default is True.
        /// </summary>
        public virtual bool IgnoreCharValidator { get; set; } = true;

        /// <summary>
        /// Gets or sets the title of the control as string.
        /// There is also <see cref="TitleAsObject"/> property.
        /// </summary>
        /// <value>The title of the control.</value>
        /// <remarks>
        /// It's up to control and its parent to decide on how this property will be used.
        /// For example if control is a child of the <see cref="TabControl"/>, <see cref="Title"/>
        /// is displayed as a tab text.
        /// </remarks>
        /// <remarks>
        /// <see cref="TitleAsObject"/> and <see cref="Title"/> are stored in the same field.
        /// </remarks>
        public string Title
        {
            get
            {
                return TitleAsObject?.ToString() ?? string.Empty;
            }

            set
            {
                TitleAsObject = value;
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
                    /*
                    Do not uncomment or we will have an exception
                    if(Parent is not null && !IgnoreLayout)
                    */
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets whether this object has items in the <see cref="InputBindings"/>
        /// collection.
        /// </summary>
        [Browsable(false)]
        public bool HasInputBindings
        {
            get
            {
                return inputBindings is not null && inputBindings.Count > 0;
            }
        }

        /// <summary>
        /// Gets the collection of input bindings associated with this control
        /// and its visible child controls. Only bindings from visible and
        /// enabled controls are returned.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<InputBinding> InputBindingsRecursive
        {
            get
            {
                if (Visible && IsEnabled)
                {
                    if (HasInputBindings)
                    {
                        foreach (var binding in InputBindings)
                        {
                            yield return binding;
                        }
                    }

                    foreach (var control in ChildrenRecursive)
                    {
                        if (!control.Visible || !control.HasInputBindings || !control.IsEnabled)
                            continue;
                        foreach (var binding in control.InputBindings)
                        {
                            yield return binding;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the collection of input bindings associated with this control.
        /// </summary>
        [Browsable(false)]
        public virtual Collection<InputBinding> InputBindings
        {
            get
            {
                if (inputBindings is null)
                {
                    inputBindings = new();

                    inputBindings.ItemRemoved += (sender, index, item) =>
                    {
                        RemoveBinding(item);
                    };

                    inputBindings.ItemInserted += (sender, index, item) =>
                    {
                        AddBinding(item);
                    };
                }

                return inputBindings;

                void RemoveBinding(InputBinding binding)
                {
                }

                void AddBinding(InputBinding binding)
                {
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ContextMenuStrip" /> associated
        /// with this control.
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
        /// <returns>
        /// The <see cref="ContextMenuStrip" /> for this control,
        /// or <see langword="null" /> if there is no attached <see cref="ContextMenuStrip"/>.
        /// The default is <see langword="null" />.
        /// </returns>
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

                if (!forced && text == value)
                    return;
                text = value;

                RaiseTextChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets offset of the layout which is applied to the position of
        /// the child controls.
        /// This property is used when layout of the child controls is
        /// <see cref="LayoutStyle.Scroll"/>.
        /// </summary>
        [Browsable(false)]
        public virtual PointD LayoutOffset
        {
            get => layoutOffset;

            set
            {
                if (layoutOffset == value)
                    return;
                layoutOffset = value;
                PerformLayout(false);
            }
        }

        /// <summary>
        /// Gets or sets maximal size of the layout which is used together with
        /// <see cref="LayoutOffset"/> when scrollbar position is updated.
        /// This property is used when layout of the child controls is
        /// <see cref="LayoutStyle.Scroll"/>. If this property is Null (default value),
        /// automatic calculation is performed.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD? LayoutMaxSize
        {
            get
            {
                return layoutMaxSize;
            }

            set
            {
                if (layoutMaxSize == value)
                    return;
                layoutMaxSize = value;
                PerformLayout(false);
            }
        }

        /// <summary>
        /// Gets internally painted caret information. This is used on some platforms
        /// where native caret is not available.
        /// </summary>
        [Browsable(false)]
        public virtual CaretInfo? CaretInfo
        {
            get => caretInfo;

            set
            {
                if (caretInfo == value)
                    return;
                caretInfo = value;
                InvalidateCaret();
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
            }
        }

        /// <summary>
        /// Gets attributes provider which allows to
        /// access items using integer identifiers.
        /// You can store any custom data here.
        /// </summary>
        [Browsable(false)]
        public ICustomAttributes<int, object> IntAttr
        {
            get
            {
                return IntFlagsAndAttributes.Attr;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Text"/> property should be localizable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsTextLocalized
        {
            get
            {
                return IntFlags[LocalizationManager.ShouldLocalizeTextIdentifier];
            }

            set
            {
                IntFlags[LocalizationManager.ShouldLocalizeTextIdentifier] = value;
            }
        }

        /// <summary>
        /// Gets or sets how to increase minimal size when size is increased.
        /// </summary>
        public virtual WindowSizeToContentMode MinSizeGrowMode
        {
            get => minSizeGrowMode;
            set
            {
                if (minSizeGrowMode == value)
                    return;
                minSizeGrowMode = value;
                GrowMinSize(value);
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="ToolTip"/> property should be localizable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsToolTipLocalized
        {
            get
            {
                return IntFlags[LocalizationManager.ShouldLocalizeToolTipIdentifier];
            }

            set
            {
                IntFlags[LocalizationManager.ShouldLocalizeToolTipIdentifier] = value;
            }
        }

        /// <summary>
        /// Gets or sets whether <see cref="Title"/> property should be localizable.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsTitleLocalized
        {
            get
            {
                return IntFlags[LocalizationManager.ShouldLocalizeTitleIdentifier];
            }

            set
            {
                IntFlags[LocalizationManager.ShouldLocalizeTitleIdentifier] = value;
            }
        }

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
        /// Gets or sets size of the <see cref="AbstractControl"/>'s client area, in
        /// device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD ClientSize
        {
            get
            {
                if (IsDummy)
                    return SizeD.Empty;
                return Size;
            }

            set
            {
                value = value.ApplyMinMax(MinimumSize, MaximumSize);
                if (ClientSize == value)
                    return;
                Size = value;
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
        public virtual Action<AbstractControl>? InitAction
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
        /// <see cref="AbstractControl"/>.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseOver
        {
            get
            {
                if (!VisibleOnScreen)
                    return false;
                var pt = Mouse.GetPosition(this);
                var result = ClientRectangle.Contains(pt);
                return result;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="Caret"/> associated with this control.
        /// </summary>
        [Browsable(false)]
        public virtual Caret? Caret
        {
            get
            {
                return caret;
            }

            set
            {
                if (caret == value)
                    return;
                caret = value;
                CaretControlChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseCaptured
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets data (images, colors, borders, pens, brushes, etc.) for different
        /// control states.
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
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

        /// <summary>
        /// Gets the DPI of the display used by this control. Returns width property of
        /// the result of <see cref="GetDPI"/> call.
        /// </summary>
        [Browsable(false)]
        public Coord DPI
        {
            get
            {
                return GetDPI().Width;
            }
        }

        IntPtr IWin32Window.Handle => default;

        /// <summary>
        /// Gets or sets background brushes attached to this control.
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
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
        /// Usage of this property depends on the control. Not all controls support it.
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
        /// Gets the first child of the control if it exists or <c>null</c> otherwise.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? FirstChild => GetChildOrNull();

        /// <summary>
        /// Gets collection of all children recursively.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<AbstractControl> ChildrenRecursive
        {
            get
            {
                if (HasChildren)
                {
                    foreach (var child in Children)
                    {
                        yield return child;
                        foreach (var childOfChild in child.ChildrenRecursive)
                        {
                            yield return childOfChild;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the first visible child of the control if it exists or <c>null</c> otherwise.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? FirstVisibleChild
        {
            get
            {
                if (!HasChildren)
                    return null;
                foreach(var child in Children)
                {
                    if (child.Visible)
                        return child;
                }

                return null;
            }
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
        public virtual AbstractControl? NextSibling
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
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="AbstractControl"/> bounds relative to the parent,
        /// in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD Bounds
        {
            get => bounds;
            set
            {
                value.Size = value.Size.ApplyMinMax(MinimumSize, MaximumSize);
                if (Bounds == value)
                    return;
                bounds = value;
                ReportBoundsChanged();
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
        /// This is the same as <see cref="Visible"/> property.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        public bool IsVisible
        {
            get
            {
                return Visible;
            }

            set
            {
                Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its
        /// child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        [Browsable(false)]
        public virtual bool Visible
        {
            get => visible;

            set
            {
                if (visible == value)
                    return;

                visible = value;
                RaiseVisibleChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond
        /// to user interaction.
        /// </summary>
        /// <value>
        /// <c>true</c> if the control can respond to user
        /// interaction; otherwise, <c>false</c>. The default is <c>true</c>.
        /// </value>
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
        [Browsable(false)]
        public virtual bool Enabled
        {
            get
            {
                return enabled && IsParentEnabled;
            }

            set
            {
                if (DisposingOrDisposed)
                    return;
                if (enabled == value)
                    return;
                enabled = value;
                RaiseEnabledChanged(EventArgs.Empty);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control can respond
        /// to user interaction. This is the same as <see cref="Enabled"/> property.
        /// Read more information there.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return Enabled;
            }

            set
            {
                Enabled = value;
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
        public abstract bool IsHandleCreated { get; }

        /// <inheritdoc />
        public override IEnumerable<FrameworkElement> LogicalChildrenCollection
            => HasChildren ? Children : Array.Empty<FrameworkElement>();

        /// <inheritdoc/>
        [Browsable(false)]
        public override IReadOnlyList<FrameworkElement> ContentElements
        {
            get
            {
                if (children == null)
                    return Array.Empty<FrameworkElement>();
                return children;
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
        /// from the <see cref="AbstractControl"/> class.
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
                    children.ItemInserted += (s, index, item) => RaiseChildInserted(index, item);
                    children.ItemRemoved += (s, index, item) => RaiseChildRemoved(item);
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
        /// Gets or sets whether key bindings are validated using
        /// <see cref="KeyGesture.IsValid(Key, UI.ModifierKeys)"/> before they are used
        /// in <see cref="ExecuteKeyBinding"/>. Default is True.
        /// </summary>
        [Browsable(false)]
        public virtual bool ValidateKeyBinding { get; set; } = true;

        /// <summary>
        /// Gets whether control has visible children controls.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasVisibleChildren
        {
            get
            {
                if (!HasChildren)
                    return false;

                foreach(var child in Children)
                {
                    if (child.Visible)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets or sets the parent container of the control.
        /// </summary>
        [Browsable(false)]
        public virtual AbstractControl? Parent
        {
            get => parent;
            set
            {
                if (parent == value)
                    return;

                if(parent is not null)
                {
                    if(parent.HasChildren)
                        parent.Children.Remove(this);
                }

                parent = null;
                base.LogicalParent = null;

                if (!DisposingOrDisposed)
                {
                    value?.Children.Add(this);
                }
            }
        }

        /// <summary>
        /// Gets whether control has visible parent window.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsParentWindowVisible
        {
            get
            {
                return ParentWindow?.Visible ?? false;
            }
        }

        /// <summary>
        /// Gets whether control is visible on screen.
        /// </summary>
        [Browsable(false)]
        public virtual bool VisibleOnScreen
        {
            get
            {
                if (!Visible)
                    return false;
                var parent = Parent;
                if (parent is null)
                    return false;
                var result = parent.VisibleOnScreen;
                return result;
            }
        }

        /// <summary>
        /// Gets the parent window of the control.
        /// </summary>
        [Browsable(false)]
        public virtual Window? ParentWindow
        {
            get
            {
                return Root as Window;
            }
        }

        /// <summary>
        /// Gets whether this control has parent.
        /// </summary>
        [Browsable(false)]
        public bool HasParent
        {
            get
            {
                return Parent != null;
            }
        }

        /// <summary>
        /// Gets whether this control is the root control (has no parent).
        /// </summary>
        [Browsable(false)]
        public bool IsRoot => Parent == null;

        /// <summary>
        /// Gets the root parent control in the chain of parent controls.
        /// If parent control is null, returns this control.
        /// </summary>
        [Browsable(false)]
        public virtual AbstractControl Root
        {
            get
            {
                var parent = Parent;
                if (parent is null)
                    return this;
                var result = parent.Root;
                return result;
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
                SizeD result = DefaultControlSize;

                if (!IgnoreSuggestedWidth)
                    result.Width = suggestedSize.Width;

                if (!IgnoreSuggestedHeight)
                    result.Height = suggestedSize.Height;

                return result;
            }

            set
            {
                if (suggestedSize == value)
                    return;

                suggestedSize = value;

                if (IgnoreSuggestedHeight && IgnoreSuggestedWidth)
                    return;

                PerformLayout();
            }
        }

        /// <summary>
        /// Sets whether to ignore suggested size.
        /// </summary>
        [Browsable(false)]
        public virtual bool IgnoreSuggestedSize
        {
            set
            {
                if (ignoreSuggestedHeight != value || ignoreSuggestedWidth != value)
                {
                    ignoreSuggestedHeight = value;
                    ignoreSuggestedWidth = value;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to ignore suggested height.
        /// </summary>
        public virtual bool IgnoreSuggestedHeight
        {
            get
            {
                return ignoreSuggestedHeight;
            }

            set
            {
                if (ignoreSuggestedHeight == value)
                    return;
                ignoreSuggestedHeight = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets whether to ignore suggested width.
        /// </summary>
        public virtual bool IgnoreSuggestedWidth
        {
            get
            {
                return ignoreSuggestedWidth;
            }

            set
            {
                if (ignoreSuggestedWidth == value)
                    return;
                ignoreSuggestedWidth = value;
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
        [DefaultValue(Coord.NaN)]
        public Coord SuggestedWidth
        {
            get => SuggestedSize.Width;

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
        [DefaultValue(Coord.NaN)]
        public Coord SuggestedHeight
        {
            get => SuggestedSize.Height;

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
            get => true;

            set
            {
            }
        }

        /// <summary>
        /// Gets whether left mouse button is down.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseLeftButtonDown
        {
            get
            {
                return isMouseLeftButtonDown;
            }

            set
            {
                isMouseLeftButtonDown = value;
            }
        }

        /// <summary>
        /// Gets or sets override value for the <see cref="VisualState"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="VisualStateOverride"/> is specified, its value
        /// used instead of dynamic state calculation when <see cref="VisualState"/>
        /// returns its value.
        /// </remarks>
        [Browsable(false)]
        public virtual VisualControlState? VisualStateOverride { get; set; }

        /// <summary>
        /// Gets or sets override value for the <see cref="VisualStates"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="VisualStatesOverride"/> is specified, its value
        /// used instead of dynamic state calculation when <see cref="VisualStates"/>
        /// returns its value.
        /// </remarks>
        [Browsable(false)]
        public virtual VisualControlStates? VisualStatesOverride { get; set; }

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

                var state = VisualStates;

                if (state.HasFlag(VisualControlStates.Disabled))
                    return VisualControlState.Disabled;
                if (state.HasFlag(VisualControlStates.Pressed))
                    return VisualControlState.Pressed;
                if (state.HasFlag(VisualControlStates.Hovered))
                    return VisualControlState.Hovered;
                if (state.HasFlag(VisualControlStates.Focused))
                    return VisualControlState.Focused;
                return VisualControlState.Normal;
            }
        }

        /// <summary>
        /// Gets visual control states as flags enumeration.
        /// </summary>
        [Browsable(false)]
        public virtual VisualControlStates VisualStates
        {
            get
            {
                if (VisualStatesOverride is not null)
                    return VisualStatesOverride.Value;

                VisualControlStates result = 0;

                if (!Enabled)
                    result |= VisualControlStates.Disabled;
                if (IsDummy)
                    return result;
                if (IsMouseLeftButtonDown)
                    result |= VisualControlStates.Pressed;
                if (IsMouseOver)
                    result |= VisualControlStates.Hovered;
                if (Focused)
                    result |= VisualControlStates.Focused;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="GenericControlAction"/> which is executed when the control
        /// received focus.
        /// </summary>
        [Browsable(false)]
        public virtual GenericControlAction RunAfterGotFocus { get; set; } = GenericControlAction.None;

        /// <summary>
        /// Gets or sets <see cref="GenericControlAction"/> which is executed when the control
        /// lost focus.
        /// </summary>
        [Browsable(false)]
        public virtual GenericControlAction RunAfterLostFocus { get; set; } = GenericControlAction.None;

        /// <summary>
        /// Gets or sets whether to convert touch events to mouse events and to pass
        /// them to the appropriate mouse event handlers. Default is <c>true</c>.
        /// </summary>
        [Browsable(false)]
        public virtual bool TouchEventsAsMouse { get; set; } = true;

        /// <summary>
        /// Gets whether user paint is supported for this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanUserPaint => true;

        /// <summary>
        /// Gets or sets minimal value of the child's <see cref="Margin"/> property.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
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
                if(HasChildren)
                    PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the outer top margin of a control.
        /// </summary>
        public Coord MarginTop
        {
            get => Margin.Top;
            set => Margin = Margin.WithTop(value);
        }

        /// <summary>
        /// Gets or sets the outer bottom margin of a control.
        /// </summary>
        public Coord MarginBottom
        {
            get => Margin.Bottom;
            set => Margin = Margin.WithBottom(value);
        }

        /// <summary>
        /// Gets or sets the outer right margin of a control.
        /// </summary>
        public Coord MarginRight
        {
            get => Margin.Right;
            set => Margin = Margin.WithRight(value);
        }

        /// <summary>
        /// Gets or sets the outer left margin of a control.
        /// </summary>
        public Coord MarginLeft
        {
            get => Margin.Left;
            set => Margin = Margin.WithLeft(value);
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
        [Browsable(false)]
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

                if(Parent is not null)
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
        /// <see cref="AbstractControl"/> and its border.
        /// Padding is set as a <see cref="Thickness"/> structure rather than as
        /// a number so that the padding can be set asymmetrically.
        /// The <see cref="Thickness"/> structure itself supports string
        /// type conversion so that you can specify an asymmetric <see cref="Padding"/>
        /// in UIXML attribute syntax also.
        /// </remarks>
        [Browsable(false)]
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
        /// Gets or sets bounds in pixels. You should not normally use this property
        /// unless this control is a top level window.
        /// </summary>
        [Browsable(false)]
        public virtual RectI BoundsInPixels
        {
            get
            {
                return Bounds.PixelFromDip(ScaleFactor);
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (BoundsInPixels == value)
                    return;
                Bounds = value.PixelToDip(ScaleFactor);
            }
        }

        /// <summary>
        /// Gets or sets control's location in pixels. You should not normally use this property
        /// unless this control is a top level window.
        /// </summary>
        [Browsable(false)]
        public virtual PointI LocationInPixels
        {
            get
            {
                return BoundsInPixels.Location;
            }

            set
            {
                BoundsInPixels = BoundsInPixels.WithLocation(value);
            }
        }

        /// <summary>
        /// Gets or sets control's size in pixels. You should not normally use this property
        /// unless this control is a top level window.
        /// </summary>
        [Browsable(false)]
        public virtual SizeI SizeInPixels
        {
            get
            {
                return BoundsInPixels.Size;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                BoundsInPixels = BoundsInPixels.WithSize(value);
            }
        }

        /// <summary>
        /// Gets or sets the top padding inside a control.
        /// </summary>
        public Coord PaddingTop
        {
            get => Padding.Top;
            set => Padding = Padding.WithTop(value);
        }

        /// <summary>
        /// Gets or sets the bottom padding inside a control.
        /// </summary>
        public Coord PaddingBottom
        {
            get => Padding.Bottom;
            set => Padding = Padding.WithBottom(value);
        }

        /// <summary>
        /// Gets or sets the right padding inside a control.
        /// </summary>
        public Coord PaddingRight
        {
            get => Padding.Right;
            set => Padding = Padding.WithRight(value);
        }

        /// <summary>
        /// Gets or sets the left padding inside a control.
        /// </summary>
        public Coord PaddingLeft
        {
            get => Padding.Left;
            set => Padding = Padding.WithLeft(value);
        }

        /// <summary>
        /// Gets whether control is performing updates.
        /// </summary>
        /// <remarks>
        /// This property is <c>true</c> after call to <see cref="BeginUpdate"/>
        /// and before call to <see cref="EndUpdate"/>.
        /// </remarks>
        [Browsable(false)]
        public bool InUpdates => updateCount > 0;

        /// <summary>
        /// Gets or sets tooltip provider.
        /// </summary>
        [Browsable(false)]
        public virtual IToolTipProvider? ToolTipProvider { get; set; }

        /// <summary>
        /// Gets or sets the minimum size the window can be resized to.
        /// </summary>
        [Browsable(false)]
        public virtual SizeD MinimumSize
        {
            get
            {
                return minimumSize;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (MinimumSize == value)
                    return;
                minimumSize = value;
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
                return maximumSize;
            }

            set
            {
                value.Width = Math.Max(0, value.Width);
                value.Height = Math.Max(0, value.Height);
                if (MaximumSize == value)
                    return;
                maximumSize = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets the minimum width the window can be resized to.
        /// </summary>
        [DefaultValue(null)]
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
        [DefaultValue(null)]
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
        [DefaultValue(null)]
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
        /// Gets or sets <see cref="ForegroundColor"/>, <see cref="BackgroundColor"/>
        /// and <see cref="Font"/> as single <see cref="IReadOnlyFontAndColor"/> object.
        /// </summary>
        [Browsable(false)]
        public IReadOnlyFontAndColor AsFontAndColor
        {
            get
            {
                var allNull = BackgroundColor is null && ForegroundColor is null && Font is null;
                if (allNull)
                    return UI.FontAndColor.Null;
                return new FontAndColor(ForegroundColor, BackgroundColor, Font);
            }

            set
            {
                BackgroundColor = value?.BackgroundColor;
                ForegroundColor = value?.ForegroundColor;
                Font = value?.Font;
            }
        }

        /// <summary>
        /// Gets whether <see cref="ScaleFactor"/> is greater than 1.
        /// </summary>
        [Browsable(false)]
        public virtual bool HasScaleFactor
        {
            get
            {
                return ScaleFactor > SizeD.One.Width;
            }
        }

        /// <summary>
        /// Gets whether this is control has handler to the platform control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsPlatformControl => false;

        /// <summary>
        /// Gets or sets the maximum height the window can be resized to.
        /// </summary>
        [DefaultValue(null)]
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
                InternalSetColor(false, value);
                RaiseForegroundColorChanged();
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
                InternalSetColor(true, value);
                RaiseBackgroundColorChanged();
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
                    BackgroundColor = Parent.BackColor;
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
                    ForegroundColor = Parent.ForeColor;
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
                    Font = Parent.RealFont;
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
                return SystemColors.Control;
            }
        }

        /// <summary>
        /// Gets real font value.
        /// </summary>
        /// <remarks>
        /// Returns font even if <see cref="Font"/> property is <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual Font RealFont
        {
            get
            {
                var result = Font ?? AbstractControl.DefaultFont;

                if (fontStyle == 0)
                    return result;

                return result.WithStyle(fontStyle);
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
                return SystemColors.ControlText;
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
        [DefaultValue(null)]
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
            get
            {
                return Thickness.Empty;
            }
        }

        /// <summary>
        /// Gets intrinsic preferred size padding of the native control.
        /// </summary>
        [Browsable(false)]
        public virtual Thickness IntrinsicPreferredSizePadding
        {
            get
            {
                return Thickness.Empty;
            }
        }

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
                RaiseCellChanged(EventArgs.Empty);
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
                RaiseCellChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets <see cref="RowIndex"/> and <see cref="ColumnIndex"/> properties
        /// as tuple with two <see cref="int"/> values.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        [Browsable(false)]
        public virtual RowColumnIndex RowColumn
        {
            get
            {
                return new (RowIndex, ColumnIndex);
            }

            set
            {
                SetRowColumn(value.Row, value.Column);
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
                RaiseCellChanged(EventArgs.Empty);
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
                RaiseCellChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the background brush for the control.
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
        /// <remarks>
        /// Currently <see cref="PictureBox"/> and <see cref="Border"/> use brush assigned to
        /// this property for background painting.
        /// </remarks>
        [Browsable(false)]
        [DefaultValue(null)]
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
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
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
        /// Usage of this property depends on the control. Not all controls support it.
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
        /// Usage of this property depends on the control. Not all controls support it.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
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
        /// <value>
        /// The <see cref="Font"/> to apply to the text displayed by
        /// the control. The default is the value of <c>null</c>.
        /// </value>
        /// <remarks>
        /// If <see cref="Font"/> is not specified, <see cref="AbstractControl.DefaultFont"/> is used.
        /// </remarks>
        [DefaultValue(null)]
        public virtual Font? Font
        {
            get
            {
                return font;
            }

            set
            {
                if (font == value)
                    return;

                font = value;

                RaiseFontChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets whether control's font is not specified.
        /// </summary>
        [Browsable(false)]
        public bool HasDefaultFont
        {
            get
            {
                return font is null;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="FontStyle"/> override. It is used instead of the
        /// <see cref="Font"/> style when real font value is calculated.
        /// This property is ignored when it equals <see cref="FontStyle.Regular"/> (default value).
        /// </summary>
        [Browsable(false)]
        public virtual FontStyle FontStyleOverride
        {
            get
            {
                return fontStyle;
            }

            set
            {
                if (fontStyle == value)
                    return;

                RaiseFontChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether font style override is bold.
        /// </summary>
        public virtual bool IsBold
        {
            get
            {
                return fontStyle.HasFlag(FontStyle.Bold);
            }

            set
            {
                var oldValue = IsBold;

                if (value)
                    fontStyle |= FontStyle.Bold;
                else
                    fontStyle &= ~FontStyle.Bold;

                var newValue = IsBold;

                if (newValue == oldValue)
                    return;

                RaiseFontChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets whether font style override is underlined.
        /// </summary>
        public virtual bool IsUnderline
        {
            get
            {
                return fontStyle.HasFlag(FontStyle.Underline);
            }

            set
            {
                var oldValue = IsUnderline;

                if (value)
                    fontStyle |= FontStyle.Underline;
                else
                    fontStyle &= ~FontStyle.Underline;

                var newValue = IsUnderline;

                if (newValue == oldValue)
                    return;

                RaiseFontChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control is currently re-creating its handle.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the control is currently re-creating its handle;
        /// otherwise, <see langword="false" />.</returns>
        [Category("Behavior")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool RecreatingHandle
        {
            get
            {
                return false;
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
        /// Gets or sets alignment as <see cref="HVAlignment"/>.
        /// This property allows to avoid extra layout calculation when you need
        /// to set both <see cref="HorizontalAlignment"/> and <see cref="VerticalAlignment"/>.
        /// </summary>
        [Browsable(false)]
        public HVAlignment Alignment
        {
            get
            {
                return new(HorizontalAlignment, VerticalAlignment);
            }

            set
            {
                var verticalChanged = verticalAlignment != value.Vertical;
                var horizontalChanged = horizontalAlignment != value.Horizontal;
                var anyChanged = verticalChanged || horizontalChanged;

                if (!anyChanged)
                    return;

                if (verticalChanged)
                {
                    verticalAlignment = value.Vertical;
                    VerticalAlignmentChanged?.Invoke(this, EventArgs.Empty);
                }

                if (horizontalChanged)
                {
                    horizontalAlignment = value.Horizontal;
                    HorizontalAlignmentChanged?.Invoke(this, EventArgs.Empty);
                }

                if (Parent is not null && !IgnoreLayout)
                    PerformLayout();
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
                if (Parent is not null && !IgnoreLayout)
                    PerformLayout();
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
                if(Parent is not null && !IgnoreLayout)
                    PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control has a border.
        /// </summary>
        /// <remarks>
        /// It's up to the control descendant how this property is used.
        /// </remarks>
        [Browsable(false)]
        public virtual bool HasBorder
        {
            get
            {
                return false;
            }

            set
            {
            }
        }

        /// <summary>
        /// Returns this control if it is visible; otherwise returns <c>null</c>.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? OnlyVisible
        {
            get
            {
                if (Visible)
                    return this;
                return null;
            }
        }

        /// <summary>
        /// Gets <see cref="AbstractControl.Children"/> or an empty array if there are no
        /// child controls.
        /// </summary>
        /// <remarks>
        /// This method doesn't allocate memory if there are no children.
        /// </remarks>
        [Browsable(false)]
        public IReadOnlyList<AbstractControl> AllChildren
        {
            get
            {
                if (HasChildren)
                    return Children;

                return Array.Empty<AbstractControl>();
            }
        }

        /// <summary>
        /// Gets all child controls which are visible and included in the layout.
        /// </summary>
        /// <remarks>
        /// This method uses <see cref="AllChildren"/>, <see cref="AbstractControl.Visible"/>
        /// and <see cref="AbstractControl.IgnoreLayout"/> properties.
        /// </remarks>
        [Browsable(false)]
        public virtual IReadOnlyList<AbstractControl> AllChildrenInLayout
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

                List<AbstractControl> result = new(controls.Count);

                foreach (var control in controls)
                {
                    if (!ChildIgnoresLayout(control))
                        result.Add(control);
                }

                return result;
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
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// <see cref="AbstractControl"/>, in device-independent units.
        /// </summary>
        [Browsable(false)]
        public virtual RectD ClientRectangle => new(PointD.Empty, ClientSize);

        /// <summary>
        /// Returns control identifier.
        /// </summary>
        [Browsable(false)]
        public virtual ControlTypeId ControlKind => ControlTypeId.Control;

        /// <summary>
        /// Gets absolute position of the control. Returned value is <see cref="Location"/>
        /// plus all control's parents locations.
        /// </summary>
        [Browsable(false)]
        public virtual PointD AbsolutePosition
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
        /// Gets a value that indicates whether this control or its child controls
        /// have validation errors.
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
        /// Gets or sets background color of the text.
        /// </summary>
        /// <remarks>
        /// By default is <c>null</c>. It means how additional background is painted
        /// under the text. It's up to control how this property is used.
        /// </remarks>
        [DefaultValue(null)]
        [Browsable(false)]
        public virtual Color? TextBackColor
        {
            get
            {
                return textBackColor;
            }

            set
            {
                if (textBackColor == value)
                    return;
                textBackColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets attacher <see cref="ICustomCharValidator"/> provider.
        /// </summary>
        [Browsable(false)]
        public virtual ICustomCharValidator? CharValidator { get; set; }

        /// <summary>
        /// Enumerates all parent controls.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<AbstractControl> AllParents
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
        /// Gets number of child controls.
        /// </summary>
        [Browsable(false)]
        public int ChildCount => Children.Count;

        /// <summary>
        /// Gets number of visible child controls.
        /// </summary>
        [Browsable(false)]
        public int VisibleChildCount
        {
            get
            {
                if (!HasChildren)
                    return 0;

                int result = 0;

                foreach (var control in Children)
                {
                    if (control.Visible)
                        result++;
                }

                return result;
            }
        }

        IControl? IControl.NextSibling => NextSibling;

        IControl? IControl.Parent
        {
            get => Parent;
            set
            {
                Parent = value as AbstractControl;
            }
        }

        IWindow? IControl.ParentWindow => ParentWindow;

        /// <summary>
        /// Gets collection of the attached <see cref="IControlNotification"/> objects.
        /// </summary>
        [Browsable(false)]
        public virtual IEnumerable<IControlNotification> Notifications
        {
            get
            {
                if (notifications is null)
                    return Array.Empty<IControlNotification>();
                return notifications;
            }
        }
    }
}