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
    /// Provides access to the methods and properties of the control.
    /// </summary>
    public interface IControl : IDisposable, IWin32Window
    {
        /// <summary>
        /// Occurs when the user scrolls through the control contents using scrollbars.
        /// </summary>
        event ScrollEventHandler? Scroll;

        /// <summary>
        /// Occurs when the window is activated in code or by the user.
        /// </summary>
        /// <remarks>
        /// To activate a window at run time using code, call the <see cref="Window.Activate"/> method.
        /// You can use this event for
        /// tasks such as updating the contents of the window based on changes made to the
        /// window's data when the window was not activated.
        /// </remarks>
        event EventHandler? Activated;

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        event EventHandler? TitleChanged;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        event MouseEventHandler? MouseMove;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a
        /// mouse button is pressed.
        /// </summary>
        event MouseEventHandler? MouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button
        /// is released.</summary>
        event MouseEventHandler? MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        event MouseEventHandler? MouseWheel;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and left
        /// mouse button is pressed.
        /// </summary>
        event MouseEventHandler? MouseLeftButtonDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and right
        /// mouse button is pressed.
        /// </summary>
        event MouseEventHandler? MouseRightButtonDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and right mouse button
        /// is released.</summary>
        event MouseEventHandler? MouseRightButtonUp;

        /// <summary>
        /// Occurs when the control needs to layout its children.</summary>
        public event EventHandler<HandledEventArgs>? CustomLayout;

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        event MouseEventHandler? MouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and left mouse button
        /// is released.</summary>
        event MouseEventHandler? MouseLeftButtonUp;

        /// <summary>
        /// Occurs when the control's handle is created.
        /// </summary>
        event EventHandler? HandleCreated;

        /// <summary>
        /// Occurs when the control's handle is destroyed.
        /// </summary>
        event EventHandler? HandleDestroyed;

        /// <summary>
        /// Occurs when the window loses focus and is no longer the active window.
        /// </summary>
        /// <remarks>
        /// You can use this event to perform tasks such as updating another window in
        /// your application with data from the deactivated window.
        /// </remarks>
        event EventHandler? Deactivated;

        /// <summary>
        /// Occurs when the contol gets focus.
        /// </summary>
        event EventHandler? GotFocus;

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source
        /// to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <remarks>
        /// Currently this event is not called.
        /// </remarks>
        event QueryContinueDragEventHandler? QueryContinueDrag;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        event KeyEventHandler? KeyDown;

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        event KeyEventHandler? KeyUp;

        /// <summary>
        /// Occurs when the control lost focus.
        /// </summary>
        event EventHandler? LostFocus;

        /// <summary>
        /// Occurs when the <see cref="Control.Text" /> property value changes.
        /// </summary>
        event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when a character, space or backspace key is pressed while the control has focus.
        /// </summary>
        event KeyPressEventHandler? KeyPress;

        /// <summary>
        /// Occurs when the <see cref="CurrentState"/> property value changes.
        /// </summary>
        event EventHandler? CurrentStateChanged;

        /// <summary>
        /// Occurs when the <see cref="ToolTip"/> property value changes.
        /// </summary>
        event EventHandler? ToolTipChanged;

        /// <summary>
        /// Occurs when the <see cref="IsMouseOver"/> property value changes.
        /// </summary>
        event EventHandler? IsMouseOverChanged;

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        event EventHandler? Click;

        /// <summary>
        /// Occurs when <see cref="Parent"/> is changed.
        /// </summary>
        event EventHandler? ParentChanged;

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <remarks>
        /// The <see cref="Paint"/> event is raised when the control is redrawn.
        /// It passes an instance of <see cref="PaintEventArgs"/> to the method(s)
        /// that handles the <see cref="Paint"/> event.
        /// </remarks>
        event EventHandler<PaintEventArgs>? Paint;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        event EventHandler? MarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        event EventHandler? PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs after control was shown.
        /// </summary>
        event EventHandler? AfterShow;

        /// <summary>
        /// Occurs after control was hidden.
        /// </summary>
        event EventHandler? AfterHide;

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
        event EventHandler? MouseCaptureLost;

        /// <summary>
        /// When implemented by a class, occurs when user requests help for a control
        /// </summary>
        event HelpEventHandler? HelpRequested;

        /// <summary>
        /// Occurs when exception is raised inside <see cref="AvoidException"/>.
        /// </summary>
        event EventHandler<ControlExceptionEventArgs>? ProcessException;

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        event EventHandler? MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        event EventHandler? MouseLeave;

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        event EventHandler? EnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Background"/> property changes.
        /// </summary>
        event EventHandler? BackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Foreground"/> property changes.
        /// </summary>
        event EventHandler? ForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> property changes.
        /// </summary>
        event EventHandler? FontChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/>
        /// property changes.
        /// </summary>
        event EventHandler? VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/>
        /// property changes.
        /// </summary>
        event EventHandler? HorizontalAlignmentChanged;

        /// <summary>
        /// Occurs when the control's size is changed.
        /// </summary>
        event EventHandler? SizeChanged;

        /// <summary>
        /// Occurs when the application finishes processing events and is
        /// about to enter the idle state. This is the same as <see cref="Application.Idle"/>
        /// but on the control level.
        /// </summary>
        /// <remarks>
        /// Use <see cref="ProcessIdle"/> property to specify whether <see cref="Idle"/>
        /// event is fired.
        /// </remarks>
        event EventHandler? Idle;

        /// <summary>
        /// Occurs when the control's size is changed.
        /// </summary>
        event EventHandler? Resize;

        /// <summary>
        /// Occurs when the control's location is changed.
        /// </summary>
        event EventHandler? LocationChanged;

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
        event EventHandler<DragStartEventArgs>? DragStart;

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        event EventHandler<DragEventArgs>? DragDrop;

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        event EventHandler<DragEventArgs>? DragOver;

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        event EventHandler<DragEventArgs>? DragEnter;

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        event EventHandler? DragLeave;

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

            /// <summary>
            /// Indicates that start location was applied to the window.
            /// </summary>
            /// <remarks>
            /// Start location is applied only once.
            /// This flag is set after start location was applied.
            /// This flag is used in <see cref="Window"/>.
            /// </remarks>
            StartLocationApplied = 2,
        }

        /// <summary>
        /// Gets <see cref="Graphics"/> which can be used to measure text size
        /// and for other measure purposes.
        /// </summary>
        Graphics MeasureCanvas { get; }

        /// <summary>
        /// Gets border for all states of the control.
        /// </summary>
        ControlStateBorders? Borders { get; set; }

        /// <summary>
        /// Gets or sets layout style of the child controls.
        /// </summary>
        LayoutStyle? Layout { get; set; }

        /// <summary>
        /// Gets or sets whether mouse events are bubbled to parent control.
        /// </summary>
        bool BubbleMouse { get; set; }

        /// <summary>
        /// Gets or sets whether layout rules are ignored for this control.
        /// </summary>
        bool IgnoreLayout { get; set; }

        /// <summary>
        /// Gets or sets the title of the control.
        /// </summary>
        /// <value>The title of the control.</value>
        /// <remarks>
        /// It's up to control and its parent to decide on how this property will be used.
        /// For example if control is a child of the <see cref="TabControl"/>, <see cref="Title"/>
        /// is displayed as a tab text.
        /// </remarks>
        string Title { get; set; }

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
        DockStyle Dock { get; set; }

        /// <summary>
        /// Gets or sets whether controls is scrollable.
        /// </summary>
        bool IsScrollable { get; set; }

        /// <summary>
        /// Gets or sets the Input Method Editor (IME) mode of the control.
        /// </summary>
        /// <returns>One of the <see cref="ImeMode" /> values.
        /// The default is <see cref="ImeMode.Inherit" />.</returns>
        ImeMode ImeMode { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control can receive focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the control can receive focus;
        /// otherwise, <see langword="false" />.
        /// </returns>
        bool CanFocus { get; }

        /// <summary>
        /// Gets or sets the text associated with this control.
        /// </summary>
        /// <returns>
        /// The text associated with this control.
        /// </returns>
        string Text { get; set; }

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
        Cursor? Cursor { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control, or one of its child
        /// controls, currently has the input focus.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if the control or one of its child controls
        /// currently has the input focus; otherwise, <see langword="false" />.</returns>
        bool ContainsFocus { get; }

        /// <summary>
        /// Gets unique id of this control.
        /// </summary>
        ObjectUniqueId UniqueId { get; }

        /// <summary>
        /// Gets custom flags and attributes provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        IFlagsAndAttributes FlagsAndAttributes { get; set; }

        /// <summary>
        /// Gets custom flags provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        ICustomFlags CustomFlags { get; }

        /// <summary>
        /// Gets or sets cached data for the layout engine.
        /// </summary>
        object? LayoutData { get; set; }

        /// <summary>
        /// Gets or sets additional properties which are layout specific.
        /// </summary>
        object? LayoutProps { get; set; }

        /// <summary>
        /// Gets custom attributes provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        ICustomAttributes CustomAttr { get; }

        /// <summary>
        /// Gets or sets size of the <see cref="Control"/>'s client area, in
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        SizeD ClientSize { get; set; }

        /// <summary>
        /// Gets control flags.
        /// </summary>
        ControlFlags StateFlags { get; }

        /// <summary>
        /// Gets whether layout is suspended.
        /// </summary>
        bool IsLayoutSuspended { get; }

        /// <summary>
        /// Gets whether layout is currently performed.
        /// </summary>
        bool IsLayoutPerform { get; }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is over the
        /// <see cref="Control"/>.
        /// </summary>
        bool IsMouseOver { get; }

        /// <summary>
        /// Gets a value indicating whether the mouse is captured to this control.
        /// </summary>
        bool IsMouseCaptured { get; }

        /// <summary>
        /// Gets or sets data (images, colors, borders, pens, brushes, etc.) for different
        /// control states.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        ControlStateSettings? StateObjects { get; set; }

        /// <summary>
        /// Gets or sets background brushes attached to this control.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        ControlStateBrushes? Backgrounds { get; set; }

        /// <summary>
        /// Gets or sets foreground brushes attached to this control.
        /// </summary>
        ControlStateBrushes? Foregrounds { get; set; }

        /// <summary>
        /// Gets whether this control itself can have focus.
        /// </summary>
        bool IsFocusable { get; }

        /// <summary>
        /// Gets the distance, in dips, between the right edge of the control and the left
        /// edge of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the right edge of the
        /// control and the left edge of its container's client area.</returns>
        double Right { get; set; }

        /// <summary>
        /// Gets the distance, in dips, between the bottom edge of the control and the top edge
        /// of its container's client area.
        /// </summary>
        /// <returns>A value representing the distance, in dips, between the bottom edge of
        /// the control and the top edge of its container's client area.</returns>
        double Bottom { get; set; }

        /// <summary>
        /// Gets control index in the <see cref="Children"/> of the container control.
        /// </summary>
        int? IndexInParent { get; }

        /// <summary>
        /// Gets next visible sibling control.
        /// </summary>
        IControl? NextSibling { get; }

        /// <summary>
        /// Gets whether this control can have focus right now.
        /// </summary>
        /// <remarks>
        /// If this property returns true, it means that calling <see cref="SetFocus"/> will put
        /// focus either to this control or one of its children. If you need to know whether
        /// this control accepts focus itself, use <see cref="IsFocusable"/>.
        /// </remarks>
        bool CanAcceptFocus { get; }

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
        object? Tag { get; set; }

        /// <summary>
        /// Gets or sets the tool-tip that is displayed for this element
        /// in the user interface.
        /// </summary>
        string? ToolTip { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Control"/> bounds relative to the parent,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        RectD Bounds { get; set; }

        /// <summary>
        /// Gets or sets the distance between the left edge of the control
        /// and the left edge of its container's client area.
        /// </summary>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets the distance between the top edge of the control
        /// and the top edge of its container's client area.
        /// </summary>
        double Top { get; set; }

        /// <summary>
        /// Gets or sets the location of upper-left corner of the control, in
        /// device-independent units (1/96th inch per unit).
        /// </summary>
        /// <value>The position of the control's upper-left corner, in logical
        /// units (1/96th of an inch).</value>
        PointD Location { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the control and all its
        /// child controls are displayed.
        /// </summary>
        /// <value><c>true</c> if the control and all its child controls are
        /// displayed; otherwise, <c>false</c>. The default is <c>true</c>.</value>
        bool Visible { get; set; }

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
        bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IFileSystem"/> which is used in the control.
        /// If this property is <c>null</c> (default), <see cref="FileSystem.Default"/>
        /// is used as file system provider.
        /// </summary>
        /// <remarks>
        /// It is up to control to decide whether and how this property is used.
        /// </remarks>
        IFileSystem? FileSystem { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control has a native window
        /// handle associated with it.
        /// </summary>
        /// <returns>
        /// <see langword="true" /> if a native window handle has been assigned to the
        /// control; otherwise, <see langword="false" />.</returns>
        public bool IsHandleCreated { get; }

        /// <summary>
        /// Gets last time when mouse double click was done.
        /// </summary>
        long? LastDoubleClickTimestamp { get; set; }

        /// <summary>
        /// Gets whether there are any items in the <see cref="Children"/> list.
        /// </summary>
        public bool HasChildren { get; }

        /// <summary>
        /// Gets or sets the parent container of the control.
        /// </summary>
        IControl? Parent { get; set; }

        /// <summary>
        /// Gets the parent window of the control).
        /// </summary>
        IWindow? ParentWindow { get; }

        /// <summary>
        /// Gets or sets the size of the control.
        /// </summary>
        /// <value>The size of the control, in device-independent units (1/96th inch per unit).
        /// The default value is <see cref="Alternet.Drawing.SizeD"/>(<see cref="double.NaN"/>,
        /// <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the size of the control.
        /// Set this property to <see cref="Alternet.Drawing.SizeD"/>(<see cref="double.NaN"/>,
        /// <see cref="double.NaN"/>) to specify system-default sizing
        /// behavior when the control is first shown.
        /// </remarks>
        SizeD Size { get; set; }

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
        double Width { get; set; }

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
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the suggested size of the control.
        /// </summary>
        /// <value>The suggested size of the control, in device-independent
        /// units (1/96th inch per unit).
        /// The default value is <see cref="Alternet.Drawing.SizeD"/>
        /// (<see cref="double.NaN"/>, <see cref="double.NaN"/>)/>.
        /// </value>
        /// <remarks>
        /// This property specifies the suggested size of the control. An actual
        /// size is calculated by the layout system.
        /// Set this property to <see cref="Alternet.Drawing.SizeD"/>
        /// (<see cref="double.NaN"/>, <see cref="double.NaN"/>) to specify auto
        /// sizing behavior.
        /// The value of this property is always the same as the value that was
        /// set to it and is not changed by the layout system.
        /// </remarks>
        SizeD SuggestedSize { get; set; }

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
        double SuggestedWidth { get; set; }

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
        double SuggestedHeight { get; set; }

        /// <summary>
        /// Gets or set a value indicating whether the control paints itself
        /// rather than the operating system doing so.
        /// </summary>
        /// <value>If <c>true</c>, the control paints itself rather than the
        /// operating system doing so.
        /// If <c>false</c>, the <see cref="Paint"/> event is not raised.</value>
        bool UserPaint { get; set; }

        /// <summary>
        /// Gets number of children items.
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// Gets whether left mouse button is over control and is down.
        /// </summary>
        bool IsMouseLeftButtonDown { get; }

        /// <summary>
        /// Gets or sets override value for the <see cref="CurrentState"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="CurrentStateOverride"/> is specified, it's value
        /// used instead of dynamic state calculation when <see cref="CurrentState"/>
        /// returns its value.
        /// </remarks>
        GenericControlState? CurrentStateOverride { get; set; }

        /// <summary>
        /// Gets current <see cref="GenericControlState"/>.
        /// </summary>
        GenericControlState CurrentState { get; }

        /// <summary>
        /// Gets whether user paint is supported for this control.
        /// </summary>
        bool CanUserPaint { get; }

        /// <summary>
        /// Gets or sets minimal value of the child's <see cref="Margin"/> property.
        /// </summary>
        Thickness? MinChildMargin { get; set; }

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
        Thickness Margin { get; set; }

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
        Thickness Padding { get; set; }

        /// <summary>
        /// Getw whether control is performing updates.
        /// </summary>
        /// <remarks>
        /// This property is <c>true</c> after call to <see cref="BeginUpdate"/>
        /// and before call to <see cref="EndUpdate"/>.
        /// </remarks>
        bool InUpdates { get; }

        /// <summary>
        /// Gets or sets the minimum size the window can be resized to.
        /// </summary>
        SizeD MinimumSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size the window can be resized to.
        /// </summary>
        SizeD MaximumSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum width the window can be resized to.
        /// </summary>
        double? MinWidth { get; set; }

        /// <summary>
        /// Gets or sets the minimum height the window can be resized to.
        /// </summary>
        double? MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the maximum width the window can be resized to.
        /// </summary>
        double? MaxWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum height the window can be resized to.
        /// </summary>
        double? MaxHeight { get; set; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        Color? BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets whether <see cref="BackgroundColor"/> is automatically
        /// updated when parent's <see cref="BackgroundColor"/> is changed.
        /// </summary>
        bool ParentBackColor { get; set; }

        /// <summary>
        /// Gets or sets whether <see cref="ForegroundColor"/> is automatically
        /// updated when parent's <see cref="ForegroundColor"/> is changed.
        /// </summary>
        bool ParentForeColor { get; set; }

        /// <summary>
        /// Gets or sets whether <see cref="Font"/> is automatically
        /// updated when parent's <see cref="Font"/> is changed.
        /// </summary>
        bool ParentFont { get; set; }

        /// <summary>
        /// Gets real background color for the control.
        /// </summary>
        /// <remarks>
        /// This property returns color value even if <see cref="BackgroundColor"/>
        /// is <c>null</c>.
        /// </remarks>
        Color RealBackgroundColor { get; }

        /// <summary>
        /// Gets real foreground color for the control.
        /// </summary>
        /// <remarks>
        /// This property returns color value even if <see cref="ForegroundColor"/>
        /// is <c>null</c>.
        /// </remarks>
        Color RealForegroundColor { get; }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the background color for the control.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color for the control.
        /// </summary>
        Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets group indexes which are assigned to this control.
        /// </summary>
        int[]? GroupIndexes { get; set; }

        /// <summary>
        /// Gets or sets group indexes of this control. Group indexes are used
        /// in <see cref="GetGroup(int, bool)"/> method.
        /// </summary>
        /// <remarks>
        /// This property modifies <see cref="GroupIndexes"/>.
        /// </remarks>
        int? GroupIndex { get; set; }

        /// <summary>
        /// Gets intrinsic layout padding of the native control.
        /// </summary>
        Thickness IntrinsicLayoutPadding { get; }

        /// <summary>
        /// Gets intrinsic preferred size padding of the native control.
        /// </summary>
        Thickness IntrinsicPreferredSizePadding { get; }

        /// <summary>
        /// Gets or sets column index which is used in <see cref="GetColumnGroup"/> and
        /// by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int ColumnIndex { get; set; }

        /// <summary>
        /// Gets or sets row index which is used in <see cref="GetRowGroup"/> and
        /// by the <see cref="Grid"/> control.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of columns
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int ColumnSpan { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates the total number of rows
        /// this control's content spans within a container.
        /// </summary>
        /// <remarks>
        /// Currently this property works only in <see cref="Grid"/> container.
        /// </remarks>
        int RowSpan { get; set; }

        /// <summary>
        /// Gets or sets the background brush for the control.
        /// </summary>
        Brush? Background { get; set; }

        /// <summary>
        /// Gets or sets background images attached to this control.
        /// </summary>
        /// <remarks>
        /// Usage of this data depends on the control.
        /// </remarks>
        ControlStateImages? BackgroundImages { get; set; }

        /// <summary>
        /// Gets or sets the background image displayed in the control.
        /// </summary>
        /// <returns>An <see cref="Image" /> that represents the image to display in
        /// the background of the control.</returns>
        Image? BackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the foreground brush for the control.
        /// </summary>
        Brush? Foreground { get; set; }

        /// <summary>
        /// Gets or sets the font of the text displayed by the control.
        /// </summary>
        /// <value>The <see cref="Font"/> to apply to the text displayed by
        /// the control. The default is the value of <c>null</c>.</value>
        Font? Font { get; set; }

        /// <summary>
        /// Gets real font value.
        /// </summary>
        /// <remarks>
        /// Returns font even if <see cref="Font"/> property is <c>null</c>.
        /// </remarks>
        Font? RealFont { get; }

        /// <summary>
        /// Gets or sets whether control's font is bold.
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Returns true if control's background color is darker than foreground color.
        /// </summary>
        bool IsDarkBackground { get; }

        /// <summary>
        /// Gets or sets the vertical alignment applied to this control when it
        /// is positioned within a parent control.
        /// </summary>
        /// <value>A vertical alignment setting. The default is
        /// <c>null</c>.</value>
        VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the horizontal alignment applied to this control when
        /// it is positioned within a parent control.
        /// </summary>
        /// <value>A horizontal alignment setting. The default is
        /// <c>null</c>.</value>
        HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can give the focus to this control
        /// using the TAB key.
        /// </summary>
        bool TabStop { get; set; }

        /// <summary>
        /// Returns rectangle in which custom drawing need to be performed.
        /// Useful for custom draw controls
        /// </summary>
        RectD DrawClientRectangle { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the control can accept data
        /// that the user drags onto it.
        /// </summary>
        /// <value><c>true</c> if drag-and-drop operations are allowed in the
        /// control; otherwise, <c>false</c>. The default is <c>false</c>.</value>
        bool AllowDrop { get; set; }

        /// <summary>
        /// Gets or sets the background style of the control.
        /// </summary>
        /// <remarks><see cref="ControlBackgroundStyle.Transparent"/> style is not possible
        /// to set as it is not supported on all platforms.</remarks>
        ControlBackgroundStyle BackgroundStyle { get; set; }

        /// <summary>
        /// Gets a rectangle which describes the client area inside of the
        /// <see cref="Control"/>,
        /// in device-independent units (1/96th inch per unit).
        /// </summary>
        RectD ClientRectangle { get; }

        /// <summary>
        /// Gets a rectangle which describes an area inside of the
        /// <see cref="Control"/> available
        /// for positioning (layout) of its child controls, in device-independent
        /// units (1/96th inch per unit).
        /// </summary>
        RectD ChildrenLayoutBounds { get; }

        /// <summary>
        /// Gets or sets the language direction for this control.
        /// </summary>
        /// <remarks>
        /// Note that <see cref="LangDirection.Default"/> is returned if layout direction
        /// is not supported.
        /// </remarks>
        LangDirection LangDirection { get; set; }

        /// <summary>
        /// Gets or sets the site of the control.
        /// </summary>
        /// <returns>The <see cref="System.ComponentModel.ISite" /> associated
        /// with the <see cref="Control" />, if any.</returns>
        ISite? Site { get; set; }

        /// <summary>
        /// Gets a value indicating whether the control has input focus.
        /// </summary>
        bool Focused { get; }

        /// <summary>
        /// Returns control identifier.
        /// </summary>
        ControlTypeId ControlKind { get; }

        /// <summary>
        /// Gets or sets value indicating whether this control accepts
        /// input or not (i.e. behaves like a static text) and so doesn't need focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        bool AcceptsFocus { get; set; }

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
        bool AcceptsFocusFromKeyboard { get; set; }

        /// <summary>
        /// Indicates whether this control or one of its children accepts focus.
        /// </summary>
        /// <remarks>
        /// Default value is true.
        /// </remarks>
        bool AcceptsFocusRecursively { get; set; }

        /// <summary>
        /// Sets all focus related properties (<see cref="AcceptsFocus"/>,
        /// <see cref="AcceptsFocusFromKeyboard"/>, <see cref="AcceptsFocusRecursively"/>) in one call.
        /// </summary>
        bool AcceptsFocusAll { get; set; }

        /// <summary>
        /// Gets or sets whether <see cref="Idle"/> event is fired.
        /// </summary>
        bool ProcessIdle { get; set; }

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        int PixelFromDip(double value);

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        SizeI PixelFromDip(SizeD value);

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        PointI PixelFromDip(PointD value);

        /// <summary>
        /// Converts device-independent units (1/96th inch per unit) to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        RectI PixelFromDip(RectD value);

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        SizeD PixelToDip(SizeI value);

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        PointD PixelToDip(PointI value);

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        RectD PixelToDip(RectI value);

        /// <summary>
        /// Converts pixels to device-independent units (1/96th inch per unit).
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        double PixelToDip(int value);

        /// <summary>
        /// Gets child control at the specified index in the collection of child controls.
        /// </summary>
        /// <param name="index">Index of the child control.</param>
        /// <returns></returns>
        IControl GetControl(int index);

        /// <summary>
        /// Returns the DPI of the display used by this control.
        /// </summary>
        /// <remarks>
        /// The returned value is different for different windows on
        /// systems with support for per-monitor DPI values,
        /// such as Microsoft Windows.
        /// </remarks>
        /// <returns>
        /// A <see cref="Size"/> value that represents DPI of the display
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        SizeD GetDPI();

        /// <summary>
        /// Gets scale factor used in device-independent units (1/96th inch per unit) to/from
        /// pixels conversions.
        /// </summary>
        /// <returns></returns>
        double GetPixelScaleFactor();
    }
}
