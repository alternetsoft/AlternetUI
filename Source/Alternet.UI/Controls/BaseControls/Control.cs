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
        : FrameworkElement, ISupportInitialize, IDisposable, IControl, IFocusable
    {
        /// <summary>
        /// Gets or sets whether <see cref="DebugBackgroundColor"/> property is used.
        /// </summary>
        public static bool UseDebugBackgroundColor = false;

        private static readonly Size DefaultControlSize = Size.NaN;

        private static int groupIndexCounter;

        private bool isMouseLeftButtonDown;
        private bool enabled = true;
        private int layoutSuspendCount;
        private IFlagsAndAttributes? flagsAndAttributes;
        private MouseButtonEventArgs? dragEventArgs;
        private Point dragEventMousePos;
        private IComponentDesigner? designer;
        private Color? backgroundColor;
        private Color? foregroundColor;
        private Collection<Control>? children;
        private Size suggestedSize = DefaultControlSize;
        private Thickness margin;
        private Thickness padding;
        private ControlHandler? handler;
        private ControlStateSettings? stateObjects;
        private Font? font;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
        private bool inLayout;
        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Stretch;
        private ControlExtendedProps? extendedProps = null;
        private Thickness? minMargin;
        private Thickness? minPadding;
        private bool visible = true;
        private Control? parent;
        private int updateCount = 0;
        private ControlFlags stateFlags;
        private Cursor? cursor;
        private string? toolTip;
        private ObjectUniqueId? uniqueId;

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
        /// Occurs when the contol gets focus.
        /// </summary>
        public event EventHandler? GotFocus;

        /// <summary>
        /// Occurs when the control lost focus.
        /// </summary>
        public event EventHandler? LostFocus;

        /// <summary>
        /// Occurs when the <see cref="CurrentState"/> property value changes.
        /// </summary>
        public event EventHandler? CurrentStateChanged;

        /// <summary>
        /// Occurs when the <see cref="ToolTip"/> property value changes.
        /// </summary>
        public event EventHandler? ToolTipChanged;

        /// <summary>
        /// Occurs when the <see cref="IsMouseOver"/> property value changes.
        /// </summary>
        public event EventHandler? IsMouseOverChanged;

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        public virtual event EventHandler? Click;

        /// <summary>
        /// Occurs when <see cref="Parent"/> is changed.
        /// </summary>
        public event EventHandler? ParentChanged;

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <remarks>
        /// The <see cref="Paint"/> event is raised when the control is redrawn.
        /// It passes an instance of <see cref="PaintEventArgs"/> to the method(s)
        /// that handles the <see cref="Paint"/> event.
        /// </remarks>
        public event EventHandler<PaintEventArgs>? Paint;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        public event EventHandler? MarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        public event EventHandler? PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        public event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs when the control loses mouse capture.
        /// </summary>
        /// <remarks>
        /// In rare scenarios, you might need to detect unexpected input.
        /// For example, consider the following scenarios.
        /// <list type="bullet">
        /// <item>During a mouse operation, the user opens the Start menu by
        /// pressing the Windows key or CTRL+ESC.</item>
        /// <item>During a mouse operation, the user switches to another program
        /// by pressing ALT+TAB.</item>
        /// <item>During a mouse operation, another program displays a window or
        /// a message box that takes focus away from the current application.</item>
        /// </list>
        /// Mouse operations can include clicking and holding the mouse on a form
        /// or a control, or performing a mouse drag operation.
        /// If you have to detect when a form or a control loses mouse capture
        /// for these and related unexpected scenarios, you can use the
        /// <see cref="MouseCaptureLost"/> event.
        /// </remarks>
        public event EventHandler? MouseCaptureLost;

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        public event EventHandler? MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        public event EventHandler? MouseLeave;

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        public event EventHandler? EnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Background"/> property changes.
        /// </summary>
        public event EventHandler? BackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Foreground"/> property changes.
        /// </summary>
        public event EventHandler? ForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> property changes.
        /// </summary>
        public event EventHandler? FontChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/>
        /// property changes.
        /// </summary>
        public event EventHandler? VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/>
        /// property changes.
        /// </summary>
        public event EventHandler? HorizontalAlignmentChanged;

        /// <summary>
        /// Occurs when the control's size is changed.
        /// </summary>
        public event EventHandler? SizeChanged;

        /// <summary>
        /// Occurs when the control's location is changed.
        /// </summary>
        public event EventHandler? LocationChanged;

        /// <summary>
        /// Occurs when a drag-and-drop operation needs to be started.
        /// </summary>
        /// <example>
        /// The simplest event implementation is the following:
        /// <code>
        /// private IDataObject GetDataObject()
        /// {
        ///     var result = new DataObject();
        ///     result.SetData(DataFormats.Text, "Test data string.");
        ///     return result;
        /// }
        ///
        /// private void ControlPanel_DragStart(object sender, DragStartEventArgs e)
        /// {
        ///     if (e.DistanceIsLess)
        ///         return;
        ///     e.DragStarted = true;
        ///     var result = DoDragDrop(GetDataObject(), DragDropEffects.Copy | DragDropEffects.Move);
        /// }
        /// </code>
        /// </example>
        public event EventHandler<DragStartEventArgs>? DragStart;

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        public event EventHandler<DragEventArgs>? DragDrop;

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        public event EventHandler<DragEventArgs>? DragOver;

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        public event EventHandler<DragEventArgs>? DragEnter;

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        public event EventHandler? DragLeave;

        /// <summary>
        /// Occurs when the application finishes processing and is
        /// about to enter the idle state. This is the same as <see cref="Application.Idle"/>
        /// but on the control level.
        /// </summary>
        public event EventHandler? Idle;

        /// <summary>
        /// Internal control flags.
        /// </summary>
        [Flags]
        public enum ControlFlags
        {
            /// <summary>
            /// Indicates that <see cref="Parent"/> was already assigned.
            /// </summary>
            /// <remarks>
            /// This flag is set after <see cref="Parent"/> was changed. It can be used
            /// in the <see cref="ParentChanged"/> event. It allows
            /// to determine whether <see cref="Parent"/> is changed for the first time.
            /// </remarks>
            ParentAssigned = 1,
        }

        /// <summary>
        /// Gets the default font used for controls.
        /// </summary>
        /// <value>
        /// The default <see cref="Font"/> for UI controls. The value returned will
        /// vary depending on the user's operating system and the local settings
        /// of their system.
        /// </value>
        public static Font DefaultFont => Font.Default;

        /// <summary>
        /// Gets border for all states of the control.
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
        /// Gets or sets different behavior and visualization options.
        /// </summary>
        [Browsable(false)]
        public virtual ControlOptions BehaviorOptions { get; set; }

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
                if (cursor is null)
                    NativeControl?.SetCursor(default);
                else
                    NativeControl?.SetCursor(cursor.Handle);
            }
        }

        /// <summary>
        /// Gets unique id of this control.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId UniqueId
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
        public IFlagsAndAttributes FlagsAndAttributes
        {
            get
            {
                return flagsAndAttributes ??= Factory.CreateFlagsAndAttributes();
            }
        }

        /// <summary>
        /// Gets or sets size of the <see cref="Control"/>'s client area, in
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        [Browsable(false)]
        public virtual Size ClientSize
        {
            get => Handler.ClientSize;
            set => Handler.ClientSize = value;
        }

        /// <summary>
        /// Gets control flags.
        /// </summary>
        [Browsable(false)]
        public ControlFlags StateFlags => stateFlags;

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
        public IComponentDesigner? Designer
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
            get
            {
                return NativeControl.IsMouseOver;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseCaptured => Handler.IsMouseCaptured;

        /// <summary>
        /// Gets or sets data (images, colors, borders, pens, brushes, etc.) for different
        /// control states.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        [Browsable(false)]
        public ControlStateSettings? StateObjects
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
        /// Gets whether this control itself can have focus.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsFocusable => Handler.IsFocusable;

        /// <summary>
        /// Gets whether this control can have focus right now.
        /// </summary>
        /// <remarks>
        /// If this property returns true, it means that calling <see cref="SetFocus"/> will put
        /// focus either to this control or one of its children. If you need to know whether
        /// this control accepts focus itself, use <see cref="IsFocusable"/>.
        /// </remarks>
        [Browsable(false)]
        public virtual bool CanAcceptFocus => Handler.CanAcceptFocus;

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
        public object? Tag
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tool-tip that is displayed for this element
        /// in the user interface.
        /// </summary>
        [DefaultValue(null)]
        [Localizability(LocalizationCategory.ToolTip)]
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
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> bounds relative to the parent,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        [Browsable(false)]
        public virtual Rect Bounds
        {
            get => Handler.Bounds;
            set => Handler.Bounds = value;
        }

        /// <summary>
        /// Gets or sets the distance between the left edge of the control
        /// and the left edge of its container's client area.
        /// </summary>
        public virtual double Left
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
                Bounds = new Rect(value, bounds.Top, bounds.Width, bounds.Height);
            }
        }

        /// <summary>
        /// Gets or sets the distance between the top edge of the control
        /// and the top edge of its container's client area.
        /// </summary>
        public virtual double Top
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
                Bounds = new Rect(bounds.Left, value, bounds.Width, bounds.Height);
            }
        }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the control, in
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        /// <value>The position of the control's upper-left corner, in logical
        /// units (1/96th of an inch).</value>
        [Browsable(false)]
        public virtual Point Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new Rect(value, Bounds.Size);
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
        public virtual bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                if (enabled == value)
                    return;
                enabled = value;
                RaiseEnabledChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets a <see cref="ControlHandler"/> associated with this class.
        /// </summary>
        [Browsable(false)]
        public virtual ControlHandler Handler
        {
            get
            {
                EnsureHandlerCreated();
                return handler ?? throw new InvalidOperationException();
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
        public virtual Collection<Control> Children
        {
            get
            {
                if (children == null)
                {
                    children = new Collection<Control>() { ThrowOnNullAdd = true };
                    children.ItemInserted += Children_ItemInserted;
                    children.ItemRemoved += Children_ItemRemoved;
                }

                return children;
            }
        }

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
                OnParentChanged(EventArgs.Empty);
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
                    if (result is Window)
                        return (Window?)result;
                    result = result.Parent;
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>The size of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="Drawing.Size"/>(<see cref="double.NaN"/>,
        /// <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the control.
        /// Set this property to <see cref="Drawing.Size"/>(<see cref="double.NaN"/>,
        /// <see cref="double.NaN"/>) to specify system-default sizing
        /// behavior when the control is first shown.
        /// </remarks>
        [Browsable(false)]
        public virtual Size Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Handler.Bounds = new Rect(Bounds.Location, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        /// <value>The width of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the width of the control.
        /// Set this property to <see cref="double.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        public virtual double Width
        {
            get => Size.Width;
            set => Size = new(value, Height);
        }

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        /// <value>The height of the control, in device-independent units
        /// (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the height of the control.
        /// Set this property to <see cref="double.NaN"/> to specify system-default sizing
        /// behavior before the control is first shown.
        /// </remarks>
        public virtual double Height
        {
            get => Size.Height;
            set => Size = new(Width, value);
        }

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent
        /// units (1/96th inch per unit).
        /// The default value is <see cref="Drawing.Size"/>
        /// (<see cref="double.NaN"/>, <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual
        /// size is calculated by the layout system.
        /// Set this property to <see cref="Drawing.Size"/>
        /// (<see cref="double.NaN"/>, <see cref="double.NaN"/>) to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        [Browsable(false)]
        public Size SuggestedSize
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
            }
        }

        /// <summary>
        /// Gets or sets the suggested width of the control.
        /// </summary>
        /// <value>The suggested width of the control, in device-independent
        /// units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested width of the control. An
        /// actual width is calculated by the layout system.
        /// Set this property to <see cref="double.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        public double SuggestedWidth
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
        /// <value>The suggested height of the control, in device-independent
        /// units (1/96th inch per unit).
        /// The default value is <see cref="double.NaN"/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested height of the control. An
        /// actual height is calculated by the layout system.
        /// Set this property to <see cref="double.NaN"/> to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        public double SuggestedHeight
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
            get => NativeControl.UserPaint;
            set
            {
                if (value && !CanUserPaint)
                    return;
                NativeControl.UserPaint = value;
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
        /// Gets or sets override value for the <see cref="CurrentState"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="CurrentStateOverride"/> is specified, it's value
        /// used instead of dynamic state calculation when <see cref="CurrentState"/>
        /// returns its value.
        /// </remarks>
        [Browsable(false)]
        public virtual GenericControlState? CurrentStateOverride { get; set; }

        /// <summary>
        /// Gets current <see cref="GenericControlState"/>.
        /// </summary>
        [Browsable(false)]
        public virtual GenericControlState CurrentState
        {
            get
            {
                if (CurrentStateOverride is not null)
                    return CurrentStateOverride.Value;

                if (!enabled)
                    return GenericControlState.Disabled;
                if (IsMouseOver)
                {
                    if(IsMouseLeftButtonDown)
                        return GenericControlState.Pressed;
                    else
                        return GenericControlState.Hovered;
                }

                if (IsFocused)
                    return GenericControlState.Focused;
                return GenericControlState.Normal;
            }
        }

        /// <summary>
        /// Gets whether user paint is supported for this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool CanUserPaint => true;

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
        public virtual Size MinimumSize
        {
            get => Handler.MinimumSize;
            set => Handler.MinimumSize = value;
        }

        /// <summary>
        /// Gets or sets the maximum size the window can be resized to.
        /// </summary>
        [Browsable(false)]
        public virtual Size MaximumSize
        {
            get => Handler.MaximumSize;
            set => Handler.MaximumSize = value;
        }

        /// <summary>
        /// Gets or sets the minimum width the window can be resized to.
        /// </summary>
        public virtual double? MinWidth
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
        public virtual double? MinHeight
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
        public virtual double? MaxWidth
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
        public virtual double? MaxHeight
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
        public virtual Color? BackgroundColor
        {
            get
            {
                return backgroundColor;
            }

            set
            {
                if (backgroundColor == value && value != null)
                    return;
                backgroundColor = value;

                if (NativeControl is not null)
                {
                    if (backgroundColor is null)
                        ResetBackgroundColor();
                    else
                        NativeControl.BackgroundColor = backgroundColor.Value;
                    Refresh();
                }
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
        public virtual Color? RealBackgroundColor
        {
            get
            {
                var result = NativeControl?.BackgroundColor;
                return result;
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
        public virtual Color? RealForegroundColor
        {
            get
            {
                var result = NativeControl?.ForegroundColor;
                return result;
            }
        }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
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

                if (NativeControl is not null)
                {
                    if (foregroundColor is null)
                        ResetForegroundColor();
                    else
                        NativeControl.ForegroundColor = foregroundColor.Value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets group indexes which are assigned to this control.
        /// </summary>
        [Browsable(false)]
        public int[]? GroupIndexes { get; set; }

        /// <summary>
        /// Gets or sets group indexes of this control. Group indexes are used
        /// in <see cref="GetGroup(int, bool)"/> method.
        /// </summary>
        /// <remarks>
        /// This property modifies <see cref="GroupIndexes"/>.
        /// </remarks>
        [Browsable(false)]
        public int? GroupIndex
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
                    GroupIndexes = [value.Value];
            }
        }

        /// <summary>
        /// Gets or sets column index which is used in <see cref="GetColumnGroup"/>.
        /// </summary>
        public int? ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets row index which is used in <see cref="GetRowGroup"/>.
        /// </summary>
        public int? RowIndex { get; set; }

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

                font = value;
                FontChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets real font value.
        /// </summary>
        /// <remarks>
        /// Returns font event if <see cref="Font"/> property is <c>null</c>.
        /// </remarks>
        [Browsable(false)]
        public virtual Font? RealFont => Font.FromInternal(NativeControl?.Font);

        /// <summary>
        /// Gets or sets whether control's font is bold.
        /// </summary>
        public virtual bool IsBold
        {
            get
            {
                if (NativeControl is null)
                    return false;
                return NativeControl.IsBold;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.IsBold = value;
                Handler.RaiseLayoutChanged();
                PerformLayout();
                Refresh();
            }
        }

        /// <summary>
        /// Returns true if control's background color is darker than foreground color.
        /// </summary>
        [Browsable(false)]
        public bool IsDarkBackground
        {
            get
            {
                var foregroundColor = RealForegroundColor;
                var backgroundColor = RealBackgroundColor;

                if (foregroundColor is null || backgroundColor is null)
                    return SystemSettings.IsUsingDarkBackground;

                return ColorUtils.IsDarkBackground(foregroundColor.Value, backgroundColor.Value);
            }
        }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it
        /// is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is
        /// <see cref="VerticalAlignment.Stretch"/>.</value>
        public virtual VerticalAlignment VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                VerticalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when
        /// it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is
        /// <see cref="HorizontalAlignment.Stretch"/>.</value>
        public virtual HorizontalAlignment HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                HorizontalAlignmentChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control
        /// using the TAB key.
        /// </summary>
        public virtual bool TabStop
        {
            get => Handler.TabStop;
            set => Handler.TabStop = value;
        }

        /// <summary>
        /// Returns rectangle in which custom drawing need to be performed.
        /// Useful for custom draw controls
        /// </summary>
        [Browsable(false)]
        public virtual Rect DrawClientRectangle
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
            get
            {
                return Handler.AllowDrop;
            }

            set
            {
                Handler.AllowDrop = value;
            }
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
                return (ControlBackgroundStyle?)NativeControl?.GetBackgroundStyle()
                    ?? ControlBackgroundStyle.System;
            }

            set
            {
                if (value == ControlBackgroundStyle.Transparent)
                    return;
                NativeControl?.SetBackgroundStyle((int)value);
            }
        }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// <see cref="Control"/>,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        [Browsable(false)]
        public virtual Rect ClientRectangle => new(Point.Empty, ClientSize);

        /// <summary>
        /// Gets a rectangle which describes an area inside of the
        /// <see cref="Control"/> available
        /// for positioning (layout) of its child controls, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        [Browsable(false)]
        public virtual Rect ChildrenLayoutBounds
        {
            get
            {
                var childrenBounds = ClientRectangle;
                if (childrenBounds.IsEmpty)
                    return Rect.Empty;

                var padding = Padding;
                var intrinsicPadding = NativeControl.IntrinsicLayoutPadding;

                return new Rect(
                    new Point(
                        padding.Left + intrinsicPadding.Left,
                        padding.Top + intrinsicPadding.Top),
                    childrenBounds.Size - padding.Size - intrinsicPadding.Size);
            }
        }

        /// <summary>
        /// Gets or sets the layout direction for this control.
        /// </summary>
        /// <remarks>
        /// Note that <see cref="LayoutDirection.Default"/> is returned if layout direction
        /// is not supported.
        /// </remarks>
        public LayoutDirection LayoutDirection
        {
            get
            {
                var control = NativeControl;
                if (control is null)
                    return LayoutDirection.Default;

                return (LayoutDirection)control.LayoutDirection;
            }

            set
            {
                if (value == LayoutDirection.Default)
                    return;
                var control = NativeControl;
                if (control is null)
                    return;
                control.LayoutDirection = (int)value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsFocused => Handler.IsFocused;

        /// <summary>
        /// Returns control identifier.
        /// </summary>
        [Browsable(false)]
        public virtual ControlTypeId ControlKind => ControlTypeId.Control;

        /// <summary>
        /// Gets or sets whether <see cref="Idle"/> event is fired.
        /// </summary>
        [Browsable(false)]
        public bool ProcessIdle
        {
            get
            {
                return NativeControl.ProcessIdle;
            }

            set
            {
                NativeControl.ProcessIdle = value;
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection
            => HasChildren ? Children : Array.Empty<FrameworkElement>();

        private IControlHandlerFactory? ControlHandlerFactory { get; set; }
    }
}