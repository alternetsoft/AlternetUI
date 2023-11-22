using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Control : FrameworkElement, ISupportInitialize, IDisposable, IControl, IFocusable
    {
        /// <summary>
        /// Identifies the <see cref="Enabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.Register(
                    "Enabled",
                    typeof(bool),
                    typeof(Control),
                    new FrameworkPropertyMetadata(
                            true,
                            PropMetadataOption.AffectsPaint,
                            new PropertyChangedCallback(OnEnabledPropertyChanged),
                            null,
                            isAnimationProhibited: true,
                            UpdateSourceTrigger.PropertyChanged));

        private static readonly Size DefaultSize = Size.NaN;
        private static int groupIndexCounter;

        private IFlagsAndAttributes? flagsAndAttributes;
        private MouseButtonEventArgs? dragEventArgs;
        private Point dragEventMousePos;
        private IComponentDesigner? designer;
        private Color? backgroundColor;
        private Color? foregroundColor;
        private Collection<Control>? children;
        private Size suggestedSize = DefaultSize;
        private Thickness margin;
        private Thickness padding;
        private ControlHandler? handler;
        private Brush? background;
        private Brush? foreground;
        private Font? font;
        private Brush? borderBrush;
        private VerticalAlignment verticalAlignment = VerticalAlignment.Stretch;
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
        /// Occurs when the <see cref="ToolTip"/> property value changes.
        /// </summary>
        public event EventHandler? ToolTipChanged;

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
        /// Occurs when the value of the "BorderBrush" property changes.
        /// </summary>
        internal event EventHandler? BorderBrushChanged;

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
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsMouseCaptured => Handler.IsMouseCaptured;

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
                return (bool)GetValue(EnabledProperty);
            }

            set
            {
                SetValue(EnabledProperty, value);
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
            get => Handler.UserPaint;
            set => Handler.UserPaint = value;
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
            get => background;
            set
            {
                if (background == value)
                    return;

                background = value;
                BackgroundChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the foreground brush for the control.
        /// </summary>
        [Browsable(false)]
        public virtual Brush? Foreground
        {
            get => foreground;
            set
            {
                if (foreground == value)
                    return;

                foreground = value;
                ForegroundChanged?.Invoke(this, EventArgs.Empty);
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
                return NativeControl?.ProcessIdle ?? false;
            }

            set
            {
                if (NativeControl is not null)
                    NativeControl.ProcessIdle = value;
            }
        }

        internal static int ScreenShotCounter { get; set; } = 0;

        internal bool ProcessUIUpdates
        {
            get
            {
                return NativeControl?.ProcessUIUpdates ?? false;
            }

            set
            {
                if (NativeControl is not null)
                    NativeControl.ProcessUIUpdates = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocus
        {
            get
            {
                var result = NativeControl?.AcceptsFocus;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocus = value;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// focus from keyboard or not.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        /// <returns>
        /// Return false to indicate that while this control can,
        /// in principle, have focus if the user clicks
        /// it with the mouse, it shouldn't be included
        /// in the TAB traversal chain when using the keyboard.
        /// </returns>
        internal bool AcceptsFocusFromKeyboard
        {
            get
            {
                var result = NativeControl?.AcceptsFocusFromKeyboard;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusFromKeyboard = value;
            }
        }

        /// <summary>
        /// Indicates whether this control or one of its children accepts focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        internal bool AcceptsFocusRecursively
        {
            get
            {
                var result = NativeControl?.AcceptsFocusRecursively;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusRecursively = value;
            }
        }

        internal bool AcceptsFocusAll
        {
            get
            {
                var result = NativeControl?.AcceptsFocusAll;
                return result ?? false;
            }

            set
            {
                if (NativeControl is null)
                    return;
                NativeControl.AcceptsFocusAll = value;
            }
        }

        /// <summary>
        /// Gets or sets the border brush of the control.
        /// </summary>
        [Browsable(false)]
        internal virtual Brush? BorderBrush
        {
            get => borderBrush;
            set
            {
                if (borderBrush == value)
                    return;

                borderBrush = value;
                BorderBrushChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal IntPtr WxWidget => NativeControl!.WxWidget;

        internal virtual bool IsDummy => false;

        internal Thickness MinMargin
        {
            get
            {
                if (minMargin == null)
                {
                    minMargin = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinMargin);
                }

                return minMargin.Value;
            }
        }

        internal Thickness MinPadding
        {
            get
            {
                if (minPadding == null)
                {
                    minPadding = AllPlatformDefaults.
                        GetAsThickness(ControlKind, ControlDefaultsId.MinPadding);
                }

                return minPadding.Value;
            }
        }

        internal bool HasExtendedProps => extendedProps != null;

        /// <summary>
        /// Gets or sets border style of the control.
        /// </summary>
        internal virtual ControlBorderStyle BorderStyle
        {
            get
            {
                var nc = NativeControl;
                if (nc is null)
                    return ControlBorderStyle.Default;
                return (ControlBorderStyle)nc.BorderStyle;
            }

            set
            {
                var nc = NativeControl;
                if (nc is null)
                    return;
                nc.BorderStyle = (int)value;
            }
        }

        /// <summary>
        /// Gets <see cref="NativeControl"/> attached to this control.
        /// </summary>
        internal Native.Control? NativeControl => Handler.NativeControl;

        internal ControlExtendedProps ExtendedProps
        {
            get
            {
                extendedProps ??= new();
                return extendedProps;
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<FrameworkElement> LogicalChildrenCollection
            => HasChildren ? Children : Array.Empty<FrameworkElement>();

        private IControlHandlerFactory? ControlHandlerFactory { get; set; }

        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        /// <param name="controlType">Type of the control.</param>
        /// <param name="renderSize">Render size. Ignored on most operating systems.</param>
        public static IReadOnlyFontAndColor GetStaticDefaultFontAndColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return new FontAndColor.ControlStaticDefaultFontAndColor(controlType, renderSize);
        }

        /// <summary>
        /// Returns the currently focused control, or <see langword="null"/> if
        /// no control is focused.
        /// </summary>
        public static Control? GetFocusedControl()
        {
            return ControlHandler.GetFocusedControl();
        }

        /// <summary>
        /// Generates new group index.
        /// </summary>
        public static int NewGroupIndex() => groupIndexCounter++;

        /// <summary>
        /// Gets <see cref="ControlDefaults"/> fof the specified <see cref="ControlTypeId"/>.
        /// </summary>
        /// <param name="controlId"></param>
        /// <returns></returns>
        public static ControlDefaults GetDefaults(ControlTypeId controlId) =>
            AllPlatformDefaults.PlatformCurrent.Controls[controlId];

        /// <summary>
        /// Gets known svg color depending on the value of
        /// <see cref="IsDarkBackground"/> property.
        /// </summary>
        /// <param name="knownSvgColor">Known svg color identifier.</param>
        public Color GetSvgColor(KnownSvgColor knownSvgColor)
        {
            return SvgColors.GetSvgColor(knownSvgColor, IsDarkBackground);
        }

        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        public IReadOnlyFontAndColor GetDefaultFontAndColor()
        {
            return new FontAndColor.ControlDefaultFontAndColor(this);
        }

        /// <summary>
        /// Hides tooltip if it is visible. This method doesn't change <see cref="ToolTip"/>
        /// property.
        /// </summary>
        public virtual void HideToolTip()
        {
            if (NativeControl is null)
                return;
            NativeControl.UnsetToolTip();
            NativeControl.ToolTip = toolTip;
        }

        /// <summary>
        /// Resets bacgkround color to the default value.
        /// </summary>
        public virtual void ResetBackgroundColor(ResetColorType method = ResetColorType.Auto)
        {
            ResetColor(true, method);
        }

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        public virtual void ResetForegroundColor(ResetColorType method = ResetColorType.Auto)
        {
            ResetColor(false, method);
        }

        /// <summary>
        /// Gets the subset of <see cref="Children"/> collection with
        /// child controls of specific type.
        /// </summary>
        /// <remarks>
        /// This method is useful, for example, when you need to get
        /// all <see cref="Button"/> or <see cref="CheckBox"/> child controls.
        /// </remarks>
        public IEnumerable<T> ChildrenOfType<T>()
        {
            if (HasChildren)
                return Children.OfType<T>();
            return Array.Empty<T>();
        }

        /// <summary>
        /// Executes a delegate asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters
        /// of the same number and type that are contained in the args parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to the
        /// given method. This can be <c>null</c> if no arguments are needed.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result
        /// of the operation.</returns>
        public IAsyncResult BeginInvoke(Delegate method, object?[] args)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return SynchronizationService.BeginInvoke(method, args);
        }

        /// <summary>
        /// Creates native control if its not already created.
        /// </summary>
        public void HandleNeeded()
        {
            NativeControl?.Required();
        }

        /// <summary>
        /// Executes a delegate asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="method">A delegate to a method that takes no
        /// parameters.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result of
        /// the operation.</returns>
        public IAsyncResult BeginInvoke(Delegate method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return BeginInvoke(method, []);
        }

        /// <summary>
        /// Executes an action asynchronously on the thread that the control
        /// was created on.
        /// </summary>
        /// <param name="action">An action to execute.</param>
        /// <returns>An <see cref="IAsyncResult"/> that represents the result
        /// of the operation.</returns>
        public IAsyncResult BeginInvoke(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            return BeginInvoke(action, []);
        }

        /// <summary>
        /// Retrieves the return value of the asynchronous operation represented
        /// by the <see cref="IAsyncResult"/> passed.
        /// </summary>
        /// <param name="result">The <see cref="IAsyncResult"/> that represents
        /// a specific invoke asynchronous operation, returned when calling
        /// <see cref="BeginInvoke(Delegate)"/>.</param>
        /// <returns>The <see cref="object"/> generated by the
        /// asynchronous operation.</returns>
        public object? EndInvoke(IAsyncResult result)
        {
            if (result == null)
                throw new ArgumentNullException(nameof(result));
            return SynchronizationService.EndInvoke(result);
        }

        /// <summary>
        /// Executes the specified delegate, on the thread that owns the control,
        /// with the specified list of arguments.
        /// </summary>
        /// <param name="method">A delegate to a method that takes parameters of
        /// the same number and type that are contained in the
        /// <c>args</c> parameter.</param>
        /// <param name="args">An array of objects to pass as arguments to
        /// the specified method. This parameter can be <c>null</c> if the
        /// method takes no arguments.</param>
        /// <returns>An <see cref="object"/> that contains the return value
        /// from the delegate being invoked, or <c>null</c> if the delegate has
        /// no return value.</returns>
        public object? Invoke(Delegate method, object?[] args)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return SynchronizationService.Invoke(method, args);
        }

        /// <summary>
        /// Executes the specified delegate on the thread that owns the control.
        /// </summary>
        /// <param name="method">A delegate that contains a method to be called
        /// in the control's thread context.</param>
        /// <returns>An <see cref="object"/> that contains the return value from
        /// the delegate being invoked, or <c>null</c> if the delegate has no
        /// return value.</returns>
        public object? Invoke(Delegate method)
        {
            if (method == null)
                throw new ArgumentNullException(nameof(method));
            return Invoke(method, []);
        }

        /// <summary>
        /// Executes the specified action on the thread that owns the control.
        /// </summary>
        /// <param name="action">An action to be called in the control's
        /// thread context.</param>
        public void Invoke(Action action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            Invoke(action, []);
        }

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        public void CaptureMouse()
        {
            Handler?.CaptureMouse();
        }

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        public void ReleaseMouseCapture()
        {
            Handler?.ReleaseMouseCapture();
        }

        /// <summary>
        /// Raises the <see cref="Click"/> event and calls
        /// <see cref="OnClick(EventArgs)"/>.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public virtual void RaiseClick(EventArgs e)
        {
            OnClick(e);
            Click?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Idle"/> event and calls
        /// <see cref="OnIdle(EventArgs)"/>.
        /// See <see cref="Idle"/> event description for more details.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        public virtual void RaiseIdle(EventArgs e)
        {
            OnIdle(e);
            Idle?.Invoke(this, e);
        }

        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        /// <remarks>Showing the control is equivalent to setting the
        /// <see cref="Visible"/> property to <c>true</c>.
        /// After the <see cref="Show"/> method is called, the
        /// <see cref="Visible"/> property
        /// returns a value of <c>true</c> until the <see cref="Hide"/> method
        /// is called.</remarks>
        public void Show() => Visible = true;

        /// <summary>
        /// Gets the child control at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the child control
        /// to get.</param>
        /// <returns>The child control at the specified index in the
        /// <see cref="Children"/> list.</returns>
        public Control? GetChildOrNull(int index = 0)
        {
            if (!HasChildren)
                return null;
            if (index >= Children.Count || index < 0)
                return null;
            return Children[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlSet"/> class.
        /// </summary>
        /// <param name="controls">Controls.</param>
        public ControlSet Group(params Control[] controls) => new (controls);

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which are members of the
        /// specified group.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public ControlSet GetGroup(int groupIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.MemberOfGroup(groupIndex))
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetGroup(groupIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Gets all child controls recursively.
        /// </summary>
        public ControlSet GetChildrenRecursive()
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                result.Add(control);
                ControlSet subSet = control.GetChildrenRecursive();
                result.AddRange(subSet.Items);
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which have <see cref="ColumnIndex"/>
        /// property equal to <paramref name="columnIndex"/>.
        /// </summary>
        /// <param name="columnIndex">Column index.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public ControlSet GetColumnGroup(int columnIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.ColumnIndex == columnIndex)
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetColumnGroup(columnIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Gets <see cref="ControlSet"/> with all controls which have <see cref="RowIndex"/>
        /// property equal to <paramref name="rowIndex"/>.
        /// </summary>
        /// <param name="rowIndex">Column index.</param>
        /// <param name="recursive">Whether to check child controls recursively.</param>
        public ControlSet GetRowGroup(int rowIndex, bool recursive = false)
        {
            if (!HasChildren)
                return ControlSet.Empty;
            List<Control> result = [];
            foreach (var control in Children)
            {
                if (control.RowIndex == rowIndex)
                    result.Add(control);
                if (recursive)
                {
                    ControlSet subSet = control.GetRowGroup(rowIndex, true);
                    result.AddRange(subSet.Items);
                }
            }

            return new ControlSet(result);
        }

        /// <summary>
        /// Checks whether this control is a member of the specified group.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        public bool MemberOfGroup(int groupIndex)
        {
            var indexes = GroupIndexes;

            if (indexes is null)
                return false;
            return Array.IndexOf<int>(indexes, groupIndex) >= 0;
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginUpdate"/>
        /// and <see cref="EndUpdate"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public void DoInsideUpdate(Action action)
        {
            BeginUpdate();
            try
            {
                action();
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        public void DoInsideLayout(Action action)
        {
            SuspendLayout();
            try
            {
                action();
            }
            finally
            {
                ResumeLayout();
            }
        }

        /// <summary>
        /// Returns enumeration with the list of visible child controls.
        /// </summary>
        /// <seealso cref="GetVisibleChildOrNull"/>
        public IReadOnlyList<Control> GetVisibleChildren()
        {
            if (HasChildren)
            {
                List<Control> result = [];
                foreach (var item in Children)
                {
                    if (item.Visible)
                        result.Add(item);
                }

                return result;
            }

            return Array.Empty<Control>();
        }

        /// <summary>
        /// Gets the child control at the specified index in the
        /// list of visible child controls.
        /// </summary>
        /// <param name="index">The zero-based index of the child control
        /// to get.</param>
        /// <returns>The child control at the specified index in the
        /// visible child controls list.</returns>
        /// <seealso cref="GetVisibleChildren"/>
        public Control? GetVisibleChildOrNull(int index = 0)
        {
            var childs = GetVisibleChildren();
            foreach (Control control in childs)
            {
                if (!control.Visible)
                    continue;
                if (index == 0)
                    return control;
                index--;
            }

            return null;
        }

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <remarks>
        /// Hiding the control is equivalent to setting the
        /// <see cref="Visible"/> property to <c>false</c>.
        /// After the <see cref="Hide"/> method is called, the
        /// <see cref="Visible"/> property
        /// returns a value of <c>false</c> until the <see cref="Show"/> method
        /// is called.
        /// </remarks>
        public void Hide() => Visible = false;

        /// <summary>
        /// Creates the <see cref="DrawingContext"/> for the control.
        /// </summary>
        /// <returns>The <see cref="DrawingContext"/> for the control.</returns>
        /// <remarks>
        /// The <see cref="DrawingContext"/> object that you retrieve through the
        /// <see cref="CreateDrawingContext"/> method should not normally
        /// be retained after the current UI event has been processed,
        /// because anything painted
        /// with that object will be erased with the next paint event. Therefore
        /// you cannot cache
        /// the <see cref="DrawingContext"/> object for reuse, except to use
        /// non-visual methods like
        /// <see cref="DrawingContext.MeasureText(string, Font)"/>.
        /// Instead, you must call <see cref="CreateDrawingContext"/> every time
        /// that you want to use the <see cref="DrawingContext"/> object,
        /// and then call its Dispose() when you are finished using it.
        /// </remarks>
        public DrawingContext CreateDrawingContext() =>
            Handler.CreateDrawingContext();

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public void Invalidate() => Handler?.Invalidate();

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public void Update() => Handler?.Update();

        /// <summary>
        /// Sets the specified bounds of the control to new location and size.
        /// </summary>
        /// <param name="newBounds">New location and size.</param>
        /// <param name="specified">Specifies which bounds to use when applying new
        /// location and size.</param>
        public void SetBounds(Rect newBounds, BoundsSpecified specified)
        {
            SetBounds(newBounds.X, newBounds.Y, newBounds.Width, newBounds.Height, specified);
        }

        /// <summary>
        /// Sets the specified bounds of the control to new location and size.
        /// </summary>
        /// <param name="x">The new <see cref="Left"/> property value of
        /// the control.</param>
        /// <param name="y">The new <see cref="Top"/> property value
        /// of the control.</param>
        /// <param name="width">The new <see cref="Width"/> property value
        /// of the control.</param>
        /// <param name="height">The new <see cref="Height"/> property value
        /// of the control.</param>
        /// <param name="specified">Specifies which bounds to use when applying new
        /// location and size.</param>
        public void SetBounds(
            double x,
            double y,
            double width,
            double height,
            BoundsSpecified specified)
        {
            var bounds = Bounds;
            Rect result = new(x, y, width, height);

            if ((specified & BoundsSpecified.X) == 0)
                result.X = bounds.X;

            if ((specified & BoundsSpecified.Y) == 0)
                result.Y = bounds.Y;

            if ((specified & BoundsSpecified.Width) == 0)
                result.Width = bounds.Width;

            if ((specified & BoundsSpecified.Height) == 0)
                result.Height = bounds.Height;

            if (result != bounds)
                Handler.Bounds = result;
        }

        /// <summary>
        /// Forces the control to invalidate itself and immediately redraw itself
        /// and any child controls. Calls <see cref="Invalidate"/> and <see cref="Update"/>.
        /// </summary>
        public virtual void Refresh()
        {
            Invalidate();
            Update();
        }

        /// <summary>
        /// Temporarily suspends the layout logic for the control.
        /// </summary>
        /// <remarks>
        /// The layout logic of the control is suspended until the
        /// <see cref="ResumeLayout"/> method is called.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/>
        /// methods are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the
        /// <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then
        /// call the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public void SuspendLayout()
        {
            Handler?.SuspendLayout();
        }

        /// <summary>
        /// Gets the sizer of which this control is a member, if any, otherwise <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public ISizer? GetContainingSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetContainingSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Gets the sizer associated with the control by a previous call to <see cref="SetSizer"/>,
        /// or <c>null</c>.
        /// </summary>
        public ISizer? GetSizer()
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Sets the control to have the given layout sizer.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        /// <remarks>
        /// The control will then own the object, and will take care of its deletion.
        /// If an existing layout constraints object is already owned by the control,
        /// it will be deleted if the <paramref name="deleteOld"/> parameter is <c>true</c>.
        /// </remarks>
        /// <remarks>
        /// This function will also update layout so that the sizer will be effectively
        /// used to layout the control children whenever it is resized.
        /// </remarks>
        /// <remarks>
        /// This function enables and disables Layout automatically.
        /// </remarks>
        public void SetSizer(ISizer? sizer, bool deleteOld = true)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizer(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizer(sizer.Handle, deleteOld);
        }

        /// <summary>
        /// Gets <see cref="Display"/> where this control is shown.
        /// </summary>
        /// <returns></returns>
        public Display GetDisplay() => new(this);

        /// <summary>
        /// This method calls SetSizer() and then updates the initial control size to the
        /// size needed to accommodate all sizer elements and sets the size hints which,
        /// if this control is a top level one, prevent the user from resizing it to be
        /// less than this minimal size.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        public void SetSizerAndFit(ISizer? sizer, bool deleteOld = false)
        {
            var nativeControl = Handler?.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizerAndFit(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizerAndFit(sizer.Handle, deleteOld);
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point on the screen
        /// to client-area coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that specifies the
        /// screen coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point ScreenToClient(Point point)
        {
            return Handler.ScreenToClient(point);
        }

        /// <summary>
        /// Converts the client-area coordinates of a specified point to
        /// screen coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that contains the
        /// client coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point ClientToScreen(Point point)
        {
            return Handler.ClientToScreen(point);
        }

        /// <summary>
        /// Converts the screen coordinates of a specified point in
        /// device-independent units (1/96th inch per unit) to device (pixel) coordinates.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that specifies the
        /// screen device-independent coordinates to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Int32Point ScreenToDevice(Point point)
        {
            return Handler.ScreenToDevice(point);
        }

        /// <summary>
        /// Converts the device (pixel) coordinates of a specified point
        /// to coordinates in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="point">A <see cref="Point"/> that contains the coordinates
        /// in device-independent units (1/96th inch per unit) to be converted.</param>
        /// <returns>The converted cooridnates.</returns>
        public Point DeviceToScreen(Int32Point point)
        {
            return Handler.DeviceToScreen(point);
        }

        /// <summary>
        /// Changes <see cref="Cursor"/> property.
        /// </summary>
        /// <param name="value">New cursor.</param>
        public void SetCursor(Cursor? value)
        {
            Cursor = value;
        }

        /// <summary>
        /// Resumes the usual layout logic.
        /// </summary>
        /// <param name="performLayout"><c>true</c> to execute pending
        /// layout requests; otherwise, <c>false</c>.</param>
        /// <remarks>
        /// Resumes the usual layout logic after <see cref="SuspendLayout"/> has
        /// been called.
        /// When the <c>performLayout</c> parameter is set to <c>true</c>,
        /// an immediate layout occurs.
        /// <para>
        /// The <see cref="SuspendLayout"/> and <see cref="ResumeLayout"/> methods
        /// are used in tandem to suppress
        /// multiple layouts while you adjust multiple attributes of the control.
        /// For example, you would typically call the
        /// <see cref="SuspendLayout"/> method, then set some
        /// properties of the control, or add child controls to it, and then call
        /// the <see cref="ResumeLayout"/>
        /// method to enable the changes to take effect.
        /// </para>
        /// </remarks>
        public virtual void ResumeLayout(bool performLayout = true)
        {
            Handler?.ResumeLayout(performLayout);
            if (performLayout)
                PerformLayout();
        }

        /// <summary>
        /// Maintains performance while performing slow operations on a control
        /// by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        public virtual void BeginUpdate()
        {
            updateCount++;
            Handler?.BeginUpdate();
        }

        /// <summary>
        /// Resumes painting the control after painting is suspended by the
        /// <see cref="BeginUpdate"/> method.
        /// </summary>
        public virtual void EndUpdate()
        {
            updateCount--;
            Handler?.EndUpdate();
        }

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SuspendLayout"/> method was called before calling
        /// the <see cref="PerformLayout"/> method,
        /// the layout is suppressed.
        /// </remarks>
        public virtual void PerformLayout()
        {
            Handler?.PerformLayout();
        }

        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can
        /// be fitted, in device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="availableSize">The available space that a parent element
        /// can allocate a child control.</param>
        /// <returns>A <see cref="SuggestedSize"/> representing the width and height of
        /// a rectangle, in device-independent units (1/96th inch per unit).</returns>
        public virtual Size GetPreferredSize(Size availableSize)
        {
            return Handler.GetPreferredSize(availableSize);
        }

        /// <summary>
        /// Starts the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to start
        /// the initialization of a control.
        /// The <see cref="EndInit"/> method ends the initialization. Using the
        /// <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void BeginInit()
        {
            SuspendLayout();
            Handler?.BeginInit();
        }

        /// <summary>
        /// Ends the initialization process for this control.
        /// </summary>
        /// <remarks>
        /// Runtime environments and design tools can use this method to end
        /// the initialization of a control.
        /// The <see cref="BeginInit"/> method starts the initialization. Using
        /// the <see cref="BeginInit"/> and <see cref="EndInit"/> methods
        /// prevents the control from being used before it is fully initialized.
        /// </remarks>
        public virtual void EndInit()
        {
            Handler?.EndInit();
            ResumeLayout();
        }

        /// <summary>
        /// Same as <see cref="Enabled"/> but implemented as method.
        /// </summary>
        /// <param name="value"></param>
        public void SetEnabled(bool value) => Enabled = value;

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns><see langword="true"/> if the input focus request was
        /// successful; otherwise, <see langword="false"/>.</returns>
        /// <remarks>The <see cref="SetFocus"/> method returns true if the
        /// control successfully received input focus.</remarks>
        public virtual bool SetFocus()
        {
            return Handler.SetFocus();
        }

        /// <summary>
        /// Saves screenshot of this control.
        /// </summary>
        /// <param name="fileName">Name of the file to which screenshot
        /// will be saved.</param>
        /// <remarks>This function works only on Windows.</remarks>
        public virtual void SaveScreenshot(string fileName)
        {
            Handler?.SaveScreenshot(fileName);
        }

        /// <summary>
        /// Pops up the given menu at the specified coordinates, relative to this window,
        /// and returns control when the user has dismissed the menu.
        /// </summary>
        /// <remarks>
        /// If a menu item is selected, the corresponding menu event is generated and will
        /// be processed as usual. If coordinates are not specified (-1,-1), the current
        /// mouse cursor position is used.
        /// </remarks>
        /// <remarks>
        /// It is recommended to not explicitly specify coordinates when calling PopupMenu
        /// in response to mouse click, because some of the ports(namely, on Linux)
        /// can do a better job of positioning the menu in that case.
        /// </remarks>
        /// <param name="menu">The menu to pop up.</param>
        /// <param name="x">The X position where the menu will appear.</param>
        /// <param name="y">The Y position where the menu will appear.</param>
        public virtual void ShowPopupMenu(Menu menu, int x = -1, int y = -1)
        {
            if (menu.Items.Count == 0)
                return;
            Handler.ShowPopupMenu(menu, x, y);
        }

        /// <summary>
        /// Focuses the next control.
        /// </summary>
        /// <param name="forward"><see langword="true"/> to move forward in the
        /// tab order; <see langword="false"/> to move backward in the tab
        /// order.</param>
        /// <param name="nested"><see langword="true"/> to include nested
        /// (children of child controls) child controls; otherwise,
        /// <see langword="false"/>.</param>
        public virtual void FocusNextControl(bool forward = true, bool nested = true)
        {
            Handler?.FocusNextControl(forward, nested);
        }

        /// <summary>
        /// Begins a drag-and-drop operation.
        /// </summary>
        /// <remarks>
        /// Begins a drag operation. The <paramref name="allowedEffects"/>
        /// determine which drag operations can occur.
        /// </remarks>
        /// <param name="data">The data to drag.</param>
        /// <param name="allowedEffects">One of the
        /// <see cref="DragDropEffects"/> values.</param>
        /// <returns>
        /// A value from the <see cref="DragDropEffects"/> enumeration that
        /// represents the final effect that was
        /// performed during the drag-and-drop operation.
        /// </returns>
        public virtual DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects)
        {
            return Handler.DoDragDrop(data, allowedEffects);
        }

        /// <summary>
        /// Forces the re-creation of the underlying native control.
        /// </summary>
        public virtual void RecreateWindow()
        {
            Handler?.NativeControl?.RecreateWindow();
        }

        /// <summary>
        /// Returns the DPI of the display used by this control.
        /// </summary>
        /// <remarks>
        /// The returned value is different for different windows on
        /// systems with support for per-monitor DPI values,
        /// such as Microsoft Windows.
        /// </remarks>
        /// <returns>
        /// A <see cref="SuggestedSize"/> value that represents DPI of the display
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        public virtual Size GetDPI()
        {
            if (Handler != null)
                return Handler.GetDPI();
            if (Parent != null)
                return Parent.GetDPI();
            throw new NotSupportedException();
        }

        /// <summary>
        /// Checks whether using transparent background might work.
        /// </summary>
        /// <returns><c>true</c> if background transparency is supported.</returns>
        /// <remarks>
        /// If this function returns <c>false</c>, setting <see cref="BackgroundStyle"/> with
        /// <see cref="ControlBackgroundStyle.Transparent"/> is not going to work. If it
        /// returns <c>true</c>, setting transparent style should normally succeed.
        /// </remarks>
        /// <remarks>
        /// Notice that this function would typically be called on the parent of a
        /// control you want to set transparent background style for as the control for
        /// which this method is called must be fully created.
        /// </remarks>
        public bool IsTransparentBackgroundSupported()
        {
            return NativeControl?.IsTransparentBackgroundSupported() ?? false;
        }

        /// <summary>
        /// Call this function to force one or both scrollbars to be always shown, even if
        /// the control is big enough to show its entire contents without scrolling.
        /// </summary>
        /// <param name="hflag">Whether the horizontal scroll bar should always be visible.</param>
        /// <param name="vflag">Whether the vertical scroll bar should always be visible.</param>
        /// <remarks>
        /// This function is currently only implemented under Mac/Carbon.
        /// </remarks>
        public void AlwaysShowScrollbars(bool hflag = true, bool vflag = true)
        {
            NativeControl?.AlwaysShowScrollbars(hflag, vflag);
        }

        /// <summary>
        /// Performs some action for the each child of the control.
        /// </summary>
        /// <typeparam name="T">Specifies type of the child control.</typeparam>
        /// <param name="action">Specifies action which will be called for the
        /// each child.</param>
        public virtual void ForEachChild<T>(Action<T> action)
        {
            foreach (var child in ChildrenOfType<T>())
                action(child);
        }

        /// <summary>
        /// Disable control recreate when properties that require control
        /// recreation are changed.
        /// </summary>
        public virtual void BeginIgnoreRecreate()
        {
            Handler?.BeginIgnoreRecreate();
        }

        /// <summary>
        /// Creates new <see cref="HorizontalStackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual HorizontalStackPanel AddHorizontalStackPanel()
        {
            var result = new HorizontalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="Button"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual Button AddButton(string text, Action? action = null)
        {
            var result = new Button(text)
            {
                Parent = this,
            };

            if(action is not null)
                result.Click += Result_Click;

            return result;

            void Result_Click(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="CheckBox"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual CheckBox AddCheckBox(string text, Action? action = null)
        {
            var result = new CheckBox(text)
            {
                Parent = this,
            };

            if (action is not null)
                result.CheckedChanged += Result_CheckedChanged;

            return result;

            void Result_CheckedChanged(object? sender, EventArgs e)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Creates new <see cref="Label"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual Label AddLabel(string text)
        {
            var result = new Label(text)
            {
                Parent = this,
            };

            return result;
        }

        /// <summary>
        /// Gets whether one of this control's parents equals <paramref name="testParent"/>.
        /// </summary>
        /// <param name="testParent">Control to test as an indirect parent.</param>
        public virtual bool HasIndirectParent(Control? testParent)
        {
            var p = Parent;
            while (true)
            {
                if (p == testParent)
                    return true;
                if (p == null)
                    return false;
                p = p.Parent;
            }
        }

        /// <summary>
        /// Creates new <see cref="VerticalStackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual VerticalStackPanel AddVerticalStackPanel()
        {
            var result = new VerticalStackPanel
            {
                Parent = this,
            };
            return result;
        }

        /// <summary>
        /// Creates new <see cref="GroupBox"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual GroupBox AddGroupBox(string? title = default)
        {
            var result = new GroupBox
            {
                Parent = this,
            };

            if (title is not null)
                result.Title = title;
            return result;
        }

        /// <summary>
        /// Creates new <see cref="StackPanel"/> and adds it to the <see cref="Children"/>.
        /// </summary>
        public virtual StackPanel AddStackPanel(bool isVertical = true)
        {
            StackPanelOrientation orientation;

            if (isVertical)
                orientation = StackPanelOrientation.Vertical;
            else
                orientation = StackPanelOrientation.Horizontal;

            var result = new StackPanel
            {
                Parent = this,
                Orientation = orientation,
            };
            return result;
        }

        /// <summary>
        /// Enable control recreate if it's required after it was previously
        /// disabled by <see cref="BeginIgnoreRecreate"/>
        /// </summary>
        public virtual void EndIgnoreRecreate()
        {
            Handler?.EndIgnoreRecreate();
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public int PixelFromDip(double value)
        {
            return Native.Control.DrawingFromDip(value, this.WxWidget);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public Int32Size PixelFromDip(Size value)
        {
            return new(PixelFromDip(value.Width), PixelFromDip(value.Height));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public Int32Point PixelFromDip(Point value)
        {
            return new(PixelFromDip(value.X), PixelFromDip(value.Y));
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public Int32Rect PixelFromDip(Rect value)
        {
            return new(PixelFromDip(value.Location), PixelFromDip(value.Size));
        }

        /// <summary>
        /// Gets scale factor used in device-independent units (1/96th inch per unit) to/from
        /// pixels conversions.
        /// </summary>
        /// <returns></returns>
        public double GetPixelScaleFactor()
        {
            return Native.Control.DrawingDPIScaleFactor(this.WxWidget);
        }

        /// <summary>
        /// Converts <see cref="Int32Size"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Size"/> in pixels.</param>
        /// <returns></returns>
        public Size PixelToDip(Int32Size value)
        {
            return new(PixelToDip(value.Width), PixelToDip(value.Height));
        }

        /// <summary>
        /// Converts <see cref="Int32Point"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Point"/> in pixels.</param>
        /// <returns></returns>
        public Point PixelToDip(Int32Point value)
        {
            return new(PixelToDip(value.X), PixelToDip(value.Y));
        }

        /// <summary>
        /// Converts <see cref="Int32Rect"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="Int32Rect"/> in pixels.</param>
        /// <returns></returns>
        public Rect PixelToDip(Int32Rect value)
        {
            return new(PixelToDip(value.Location), PixelToDip(value.Size));
        }

        /// <summary>
        /// Converts pixels to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        public double PixelToDip(int value)
        {
            return Native.Control.DrawingToDip(value, this.WxWidget);
        }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        public double PixelFromDipF(double value)
        {
            return Native.Control.DrawingFromDipF(value, this.WxWidget);
        }

        internal static void NotifyCaptureLost()
        {
            Native.Control.NotifyCaptureLost();
        }

        internal static Color GetClassDefaultAttributesBgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesBgColor(
                (int)controlType,
                (int)renderSize);
        }

        internal static Color GetClassDefaultAttributesFgColor(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            return Native.Control.GetClassDefaultAttributesFgColor(
                (int)controlType,
                (int)renderSize);
        }

        internal static Font? GetClassDefaultAttributesFont(
            ControlTypeId controlType,
            ControlRenderSizeVariant renderSize = ControlRenderSizeVariant.Normal)
        {
            var font = Native.Control.GetClassDefaultAttributesFont(
                (int)controlType,
                (int)renderSize);
            return Font.FromInternal(font);
        }

        internal Color? GetDefaultAttributesBgColor()
        {
            CheckDisposed();
            return NativeControl?.GetDefaultAttributesBgColor();
        }

        internal Color? GetDefaultAttributesFgColor()
        {
            CheckDisposed();
            return NativeControl?.GetDefaultAttributesFgColor();
        }

        internal Font? GetDefaultAttributesFont()
        {
            CheckDisposed();
            return Font.FromInternal(NativeControl?.GetDefaultAttributesFont());
        }

        /// <summary>
        /// Forces the re-creation of the handler for the control.
        /// </summary>
        /// <remarks>
        /// The <see cref="RecreateHandler"/> method is called whenever
        /// re-execution of handler creation logic is needed.
        /// For example, this may happen when visual theme changes.
        /// </remarks>
        internal void RecreateHandler()
        {
            if (handler != null)
                DetachHandler();

            Invalidate();
        }

        internal void RaiseSizeChanged(EventArgs e) => OnSizeChanged(e);

        internal void RaiseLocationChanged(EventArgs e) => OnLocationChanged(e);

        internal void RaiseMouseCaptureLost()
        {
            OnMouseCaptureLost();
            MouseCaptureLost?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMouseEnter()
        {
            OnMouseEnter();
            MouseEnter?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseMouseLeave()
        {
            OnMouseLeave();
            MouseLeave?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseChildInserted(Control childControl) =>
            OnChildInserted(childControl);

        internal void RaiseChildRemoved(Control childControl) =>
            OnChildInserted(childControl);

        internal void InvokeOnLayout()
        {
            OnLayout();
        }

        internal void RaisePaint(PaintEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));

            OnPaint(e);
            Paint?.Invoke(this, e);
        }

        internal void RaiseGotFocus(EventArgs e)
        {
            OnGotFocus(e);
        }

        internal void RaiseLostFocus(EventArgs e)
        {
            OnLostFocus(e);
        }

        internal void SetParentInternal(Control? value)
        {
            parent = value;
            LogicalParent = value;
        }

        internal virtual Size GetPreferredSizeLimited(Size availableSize)
        {
            var result = GetPreferredSize(availableSize);
            var minSize = MinimumSize;
            var maxSize = MaximumSize;
            var preferredSize = result.ApplyMinMax(minSize, maxSize);
            return preferredSize;
        }

        internal void RaiseDragStart(DragStartEventArgs e) => OnDragStart(e);

        internal void RaiseDragDrop(DragEventArgs e) => OnDragDrop(e);

        internal void RaiseDragOver(DragEventArgs e) => OnDragOver(e);

        internal void RaiseDragEnter(DragEventArgs e) => OnDragEnter(e);

        internal void RaiseDragLeave(EventArgs e) => OnDragLeave(e);

        internal void SendMouseDownEvent(int x, int y)
        {
            Handler?.NativeControl?.SendMouseDownEvent(x, y);
        }

        internal void SendMouseUpEvent(int x, int y)
        {
            Handler?.NativeControl?.SendMouseUpEvent(x, y);
        }

        /// <summary>
        /// Ensures that the control <see cref="Handler"/> is created,
        /// creating and attaching it if necessary.
        /// </summary>
        protected internal void EnsureHandlerCreated()
        {
            if (handler == null)
            {
                CreateAndAttachHandler();
            }
        }

        /// <summary>
        /// Disconnects the current control <see cref="Handler"/> from
        /// the control.
        /// This method calls <see cref="ControlHandler.Detach"/>.
        /// </summary>
        protected internal void DetachHandler()
        {
            if (handler == null)
                throw new InvalidOperationException();
            OnHandlerDetaching(EventArgs.Empty);
            handler.Detach();
            handler = null;
        }

        /// <summary>
        /// Raises the <see cref="SizeChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnSizeChanged(EventArgs e) =>
            SizeChanged?.Invoke(this, e);

        /// <summary>
        /// Raises the <see cref="LocationChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnLocationChanged(EventArgs e) =>
            LocationChanged?.Invoke(this, e);

        /// <summary>
        /// Called when the control should reposition the child controls of
        /// the control.
        /// </summary>
        protected virtual void OnLayout()
        {
            Handler?.OnLayout();
            RaiseLayoutUpdated();
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is inserted into
        /// the <see cref="Control.Children"/> or
        /// <see cref="ControlHandler.VisualChildren"/> collection.
        /// </summary>
        protected virtual void OnChildInserted(Control childControl)
        {
        }

        /// <summary>
        /// Called when a <see cref="Control"/> is removed from the
        /// <see cref="Control.Children"/> or
        /// <see cref="ControlHandler.VisualChildren"/> collections.
        /// </summary>
        protected virtual void OnChildRemoved(Control childControl)
        {
        }

        /// <summary>
        /// Called when the control is clicked.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnClick(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the application is in idle state.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnIdle(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnVisibleChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control loses mouse capture.
        /// </summary>
        protected virtual void OnMouseCaptureLost()
        {
        }

        /// <summary>
        /// Called when the mouse pointer enters the control.
        /// </summary>
        protected virtual void OnMouseEnter()
        {
        }

        /// <summary>
        /// Called when the mouse pointer leaves the control.
        /// </summary>
        protected virtual void OnMouseLeave()
        {
        }

        /// <summary>
        /// Called before the current control handler is detached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnHandlerDetaching(EventArgs e)
        {
        }

        /// <summary>
        /// Gets an <see cref="IControlHandlerFactory"/> to use when creating
        /// new control handlers for this control.
        /// </summary>
        protected IControlHandlerFactory GetEffectiveControlHandlerHactory() =>
            ControlHandlerFactory ??
                Application.Current.VisualTheme.ControlHandlerFactory;

        /// <summary>
        /// Creates a handler for the control.
        /// </summary>
        /// <remarks>
        /// You typically should not call the <see cref="CreateHandler"/>
        /// method directly.
        /// The preferred method is to call the
        /// <see cref="EnsureHandlerCreated"/> method, which forces a handler
        /// to be created for the control.
        /// </remarks>
        protected virtual ControlHandler CreateHandler()
        {
            return new GenericControlHandler();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
#if DEBUG
            KeyInfo.Run(KnownKeys.ShowDeveloperTools, e, DebugUtils.ShowDeveloperTools);
#endif
        }

        /// <inheritdoc/>
        protected override void DisposeManagedResources()
        {
            Designer?.RaiseDisposed(this);
            /*var children = Handler.AllChildren.ToArray();*/

            SuspendLayout();
            if(HasChildren)
                Children.Clear();
            if(Handler.HasVisualChildren)
                Handler.VisualChildren.Clear();
            ResumeLayout(performLayout: false);

            // TODO
            /* foreach (var child in children) child.Dispose();*/

            DetachHandler();
        }

        /// <summary>
        /// Called when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnMarginChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnPaddingChanged(EventArgs e)
        {
        }

        /// <summary>
        /// Called when the control is redrawn. See <see cref="Paint"/> for details.
        /// </summary>
        /// <param name="e">An <see cref="PaintEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnPaint(PaintEventArgs e)
        {
        }

        /// <summary>
        /// Called after a new control handler is attached.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the event
        /// data.</param>
        protected virtual void OnHandlerAttached(EventArgs e)
        {
        }

        /// <summary>
        /// Raises the <see cref="DragDrop"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragDrop(DragEventArgs e)
        {
            DragDrop?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragStart"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragStartEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragStart(DragStartEventArgs e)
        {
            DragStart?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragOver"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnDragOver(DragEventArgs e)
        {
            DragOver?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragEnter"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DragEventArgs"/>
        /// that contains the event data.</param>
        protected virtual void OnDragEnter(DragEventArgs e)
        {
            DragEnter?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="DragLeave"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnDragLeave(EventArgs e)
        {
            DragLeave?.Invoke(this, e);
        }

        /// <summary>
        /// This method is invoked when the control gets focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnGotFocus(EventArgs e)
        {
            GotFocus?.Invoke(this, e);
            Designer?.RaiseGotFocus(this);
        }

        /// <summary>
        /// This method is invoked when the control lost focus.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that
        /// contains the event data.</param>
        protected virtual void OnLostFocus(EventArgs e)
        {
            LostFocus?.Invoke(this, e);
        }

        /// <inheritdoc/>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed && e.Source == this)
            {
                dragEventArgs = e;
                dragEventMousePos = e.GetPosition(this);
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (dragEventArgs is null || e.Source != this)
                return;
            var mousePos = e.GetPosition(this);
            var args = new DragStartEventArgs(dragEventMousePos, mousePos, dragEventArgs, e);
            RaiseDragStart(args);
            if(args.DragStarted || args.Cancel)
                dragEventArgs = null;
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            /*if (e.LeftButton == MouseButtonState.Released && e.Source == this)*/
            dragEventArgs = null;
        }

        /// <summary>
        /// Raises the <see cref="ToolTipChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnToolTipChanged(EventArgs e)
        {
            ToolTipChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ParentChanged"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnParentChanged(EventArgs e)
        {
            Designer?.RaiseParentChanged(this);
            ParentChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Called when the enabled of the <see cref="Enabled"/> property changes.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        protected virtual void OnEnabledChanged(EventArgs e)
        {
        }

        private protected override bool GetIsEnabled() => Enabled;

        private protected void SetVisibleValue(bool value) => visible = value;

        /// <summary>
        /// Callback for changes to the Enabled property
        /// </summary>
        private static void OnEnabledPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            Control control = (Control)d;
            control?.OnEnabledPropertyChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        private void CreateAndAttachHandler()
        {
            handler = CreateHandler();
            handler?.Attach(this);
            OnHandlerAttached(EventArgs.Empty);
        }

        private void Children_ItemInserted(object? sender, int index, Control item)
        {
            item.SetParentInternal(this);
        }

        private void Children_ItemRemoved(object? sender, int index, Control item)
        {
            item.SetParentInternal(null);
        }

        private void ResetColor(bool isBackground, ResetColorType method = ResetColorType.Auto)
        {
            if (NativeControl is null)
                return;

            void SetColor(Color? color)
            {
                if (isBackground)
                {
                    backgroundColor = color;
                    if (color is null)
                        NativeControl.ResetBackgroundColor();
                    else
                        NativeControl.BackgroundColor = color.Value;
                }
                else
                {
                    foregroundColor = color;
                    if (color is null)
                        NativeControl.ResetForegroundColor();
                    else
                        NativeControl.ForegroundColor = color.Value;
                }
            }

            if (method == ResetColorType.Auto)
                SetColor(null);
            else
            {
                var colors = FontAndColor.GetResetColors(method, this);
                var color = isBackground ? colors?.BackgroundColor : colors?.ForegroundColor;
                SetColor(color);
            }

            Refresh();
        }

        /// <summary>
        /// Raises the <see cref="EnabledChanged"/> event and calls
        /// <see cref="OnEnabledChanged(EventArgs)"/>.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> that contains the
        /// event data.</param>
        private void RaiseEnabledChanged(EventArgs e)
        {
            OnEnabledChanged(e);
            EnabledChanged?.Invoke(this, e);
        }

#pragma warning disable
        private void OnEnabledPropertyChanged(bool oldValue, bool newValue)
#pragma warning restore
        {
            RaiseEnabledChanged(EventArgs.Empty);
        }
    }
}