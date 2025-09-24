using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Provides access to the methods and properties of the control.
    /// </summary>
    public interface IControl : IDisposable, IWin32Window, ILayoutItem
    {
        /// <summary>
        /// Occurs before the <see cref="AbstractControl.KeyDown" /> event when a key is pressed
        /// while focus is on this control.
        /// </summary>
        [Category("Key")]
        event PreviewKeyDownEventHandler? PreviewKeyDown;

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
        event EventHandler<HandledEventArgs>? CustomLayout;

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
        /// Occurs when the control gets focus.
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
        /// Occurs when the <see cref="AbstractControl.Text" /> property value changes.
        /// </summary>
        event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when a character, space or backspace key is pressed while the control has focus.
        /// </summary>
        event KeyPressEventHandler? KeyPress;

        /// <summary>
        /// Occurs when the <see cref="VisualState"/> property value changes.
        /// </summary>
        event EventHandler? VisualStateChanged;

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
        /// Occurs when the value of the <c>Margin</c> property changes.
        /// </summary>
        event EventHandler? MarginChanged;

        /// <summary>
        /// Occurs when the value of the <c>Padding</c> property changes.
        /// </summary>
        event EventHandler? PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <c>Visible</c> property changes.
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
        event EventHandler<ThrowExceptionEventArgs>? ProcessException;

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
        /// Gets scale factor used in device-independent units to/from
        /// pixels conversions.
        /// </summary>
        /// <returns></returns>
        Coord ScaleFactor { get; }

        /// <summary>
        /// Gets or sets the rendering flags that determine how the control is rendered.
        /// </summary>
        ControlRenderingFlags RenderingFlags { get; set; }

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
        /// Gets or sets whether mouse events are bubbled to parent control.
        /// </summary>
        bool InputTransparent { get; set; }

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
        /// Gets or sets context menu for this control.
        /// </summary>
        ContextMenuStrip ContextMenuStrip { get; set; }

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
        ICustomFlags<string> CustomFlags { get; }

        /// <summary>
        /// Gets custom attributes provider associated with the control.
        /// You can store any custom data here.
        /// </summary>
        ICustomAttributes<string, object> CustomAttr { get; }

        /// <summary>
        /// Gets control flags.
        /// </summary>
        ControlFlags StateFlags { get; }

        /// <inheritdoc cref="AbstractControl.VertScrollBarInfo"/>
        ScrollBarInfo VertScrollBarInfo { get; set; }

        /// <inheritdoc cref="AbstractControl.HorzScrollBarInfo"/>
        ScrollBarInfo HorzScrollBarInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether the mouse pointer is over the
        /// <see cref="AbstractControl"/>.
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
        /// Gets control index in the children collection of the container control.
        /// </summary>
        int? IndexInParent { get; }

        /// <summary>
        /// Gets next visible sibling control.
        /// </summary>
        IControl? NextSibling { get; }

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
        bool IsHandleCreated { get; }

        /// <summary>
        /// Gets last time when mouse double click was done.
        /// </summary>
        long? LastDoubleClickTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the parent container of the control.
        /// </summary>
        IControl? Parent { get; set; }

        /// <summary>
        /// Gets the parent window of the control).
        /// </summary>
        IWindow? ParentWindow { get; }

        /// <summary>
        /// Gets or set a value indicating whether the control paints itself
        /// rather than the operating system doing so.
        /// </summary>
        /// <value>If <c>true</c>, the control paints itself rather than the
        /// operating system doing so.
        /// If <c>false</c>, the <see cref="Paint"/> event is not raised.</value>
        bool UserPaint { get; set; }

        /// <summary>
        /// Gets whether left mouse button is over control and is down.
        /// </summary>
        bool IsMouseLeftButtonDown { get; }

        /// <summary>
        /// Gets or sets override value for the <see cref="VisualState"/>
        /// property.
        /// </summary>
        /// <remarks>
        /// When <see cref="VisualStateOverride"/> is specified, its value
        /// used instead of dynamic state calculation when <see cref="VisualState"/>
        /// returns its value.
        /// </remarks>
        VisualControlState? VisualStateOverride { get; set; }

        /// <summary>
        /// Gets current <see cref="VisualControlState"/>.
        /// </summary>
        VisualControlState VisualState { get; }

        /// <summary>
        /// Gets whether user paint is supported for this control.
        /// </summary>
        bool CanUserPaint { get; }

        /// <summary>
        /// Gets whether control is performing updates.
        /// </summary>
        /// <remarks>
        /// This property is <c>true</c> after call to <see cref="BeginUpdate"/>
        /// and before call to <see cref="EndUpdate"/>.
        /// </remarks>
        bool InUpdates { get; }

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
        /// Gets or sets group indexes of this control.
        /// </summary>
        /// <remarks>
        /// This property modifies <see cref="GroupIndexes"/>.
        /// </remarks>
        int? GroupIndex { get; set; }

        /// <summary>
        /// Gets native control instance.
        /// </summary>
        object NativeControl { get; }

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
        /// Gets or sets whether control's font is bold.
        /// </summary>
        bool IsBold { get; set; }

        /// <summary>
        /// Returns true if control's background color is darker than foreground color.
        /// </summary>
        bool IsDarkBackground { get; }

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
        /// Gets or sets the site of the control.
        /// </summary>
        /// <returns>The <see cref="System.ComponentModel.ISite" /> associated
        /// with the <see cref="AbstractControl" />, if any.</returns>
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
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        int PixelFromDip(Coord value);

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        SizeI PixelFromDip(SizeD value);

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        PointI PixelFromDip(PointD value);

        /// <summary>
        /// Converts device-independent units to pixels.
        /// </summary>
        /// <param name="value">Value in device-independent units.</param>
        /// <returns></returns>
        RectI PixelFromDip(RectD value);

        /// <summary>
        /// Converts <see cref="SizeI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="SizeI"/> in pixels.</param>
        /// <returns></returns>
        SizeD PixelToDip(SizeI value);

        /// <summary>
        /// Converts <see cref="PointI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="PointI"/> in pixels.</param>
        /// <returns></returns>
        PointD PixelToDip(PointI value);

        /// <summary>
        /// Converts <see cref="RectI"/> to device-independent units.
        /// </summary>
        /// <param name="value"><see cref="RectI"/> in pixels.</param>
        /// <returns></returns>
        RectD PixelToDip(RectI value);

        /// <summary>
        /// Converts pixels to device-independent units.
        /// </summary>
        /// <param name="value">Value in pixels.</param>
        /// <returns></returns>
        Coord PixelToDip(int value);

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
        /// A <see cref="SizeD"/> value that represents DPI of the display
        /// used by this control. If the DPI is not available,
        /// returns Size(0,0) object.
        /// </returns>
        SizeD GetDPI();

        /// <summary>
        /// Sets <see cref="Title"/> property.
        /// </summary>
        /// <param name="title">New title</param>
        void SetTitle(string? title);

        /// <summary>
        /// Brings the control to the front of the z-order.
        /// </summary>
        void BringToFront();

        /// <summary>
        /// Sends the control to the back of the z-order.
        /// </summary>
        void SendToBack();

        /// <summary>
        /// Gets the background brush for specified state of the control.
        /// </summary>
        Brush? GetBackground(VisualControlState state);

        /// <summary>
        /// Gets the border settings for specified state of the control.
        /// </summary>
        BorderSettings? GetBorderSettings(VisualControlState state);

        /// <summary>
        /// Gets the foreground brush for specified state of the control.
        /// </summary>
        Brush? GetForeground(VisualControlState state);

        /// <summary>
        /// Gets known svg color depending on the value of
        /// <see cref="IsDarkBackground"/> property.
        /// </summary>
        /// <param name="knownSvgColor">Known svg color identifier.</param>
        Color GetSvgColor(KnownSvgColor knownSvgColor = KnownSvgColor.Normal);

        /// <summary>
        /// Gets control's default font and colors as <see cref="IReadOnlyFontAndColor"/>.
        /// </summary>
        IReadOnlyFontAndColor GetDefaultFontAndColor();

        /// <summary>
        /// Hides tooltip if it is visible. This method doesn't change <see cref="ToolTip"/>
        /// property.
        /// </summary>
        void HideToolTip();

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        void ResetForegroundColor(ResetColorType method);

        /// <summary>
        /// Resets background color to the default value.
        /// </summary>
        void ResetBackgroundColor(ResetColorType method);

        /// <summary>
        /// Resets background color to the default value.
        /// </summary>
        void ResetBackgroundColor();

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        void ResetForegroundColor();

        /// <summary>
        /// Resets foreground color to the default value.
        /// </summary>
        void ResetForeColor();

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's
        /// update region, which is the area that will be repainted at the next
        /// paint operation), and causes a paint message to be sent to the
        /// control.</summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region to invalidate.</param>
        void Invalidate(RectD rect);

        /// <summary>
        /// Same as <see cref="Invalidate(RectD)"/> but has additional
        /// parameter <paramref name="eraseBackground"/>.
        /// </summary>
        /// <param name="rect">A <see cref="RectD" /> that represents the region to invalidate.</param>
        /// <param name="eraseBackground">Specifies whether to erase background.</param>
        void RefreshRect(RectD rect, bool eraseBackground = true);

        /// <summary>
        /// Creates native control if its not already created.
        /// </summary>
        void HandleNeeded();

        /// <summary>
        /// Calls <see cref="PerformLayout"/> and <see cref="Invalidate()"/>.
        /// </summary>
        void PerformLayoutAndInvalidate(Action? action = null, bool layoutParent = true);

        /// <summary>
        /// Sets value of the <see cref="Text"/> property.
        /// </summary>
        /// <param name="value">New value of the <see cref="Text"/> property.</param>
        void SetText(object? value);

        /// <summary>
        /// Executes <see cref="Action"/> and calls <see cref="ProcessException"/>
        /// event if exception was raised during execution.
        /// </summary>
        /// <param name="action"></param>
        bool AvoidException(Action action);

        /// <summary>
        /// Captures the mouse to the control.
        /// </summary>
        void CaptureMouse();

        /// <summary>
        /// Releases the mouse capture, if the control held the capture.
        /// </summary>
        void ReleaseMouseCapture();

        /// <summary>
        /// Raises the <see cref="Click"/> event.
        /// See <see cref="Click"/> event description for more details.
        /// </summary>
        void RaiseClick(EventArgs e);

        /// <summary>
        /// Displays the control to the user.
        /// </summary>
        /// <remarks>Showing the control is equivalent to setting the
        /// <c>Visible</c> property to <c>true</c>.
        /// After the <see cref="Show"/> method is called, the
        /// <c>Visible</c> property
        /// returns a value of <c>true</c> until the <see cref="Hide"/> method
        /// is called.</remarks>
        void Show();

        /// <summary>
        /// Sets or releases mouse capture.
        /// </summary>
        /// <param name="value"><c>true</c> to set mouse capture; <c>false</c> to release it.</param>
        void SetMouseCapture(bool value);

        /// <summary>
        /// Checks whether this control is a member of the specified group.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        bool MemberOfGroup(int? groupIndex);

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="SuspendLayout"/>
        /// and <see cref="ResumeLayout"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        void DoInsideLayout(Action action, bool layoutParent = true);

        /// <summary>
        /// Conceals the control from the user.
        /// </summary>
        /// <remarks>
        /// Hiding the control is equivalent to setting the
        /// <c>Visible</c> property to <c>false</c>.
        /// After the <see cref="Hide"/> method is called, the
        /// <c>Visible</c> property
        /// returns a value of <c>false</c> until the <see cref="Show"/> method
        /// is called.
        /// </remarks>
        void Hide();

        /// <summary>
        /// Creates the <see cref="Graphics"/> for the control.
        /// </summary>
        /// <returns>The <see cref="Graphics"/> for the control.</returns>
        /// <remarks>
        /// The <see cref="Graphics"/> object that you retrieve through the
        /// <see cref="CreateDrawingContext"/> method should not normally
        /// be retained after the current UI event has been processed,
        /// because anything painted
        /// with that object will be erased with the next paint event. Therefore
        /// you cannot cache
        /// the <see cref="Graphics"/> object for reuse, except to use
        /// non-visual methods like
        /// <see cref="Graphics.MeasureText(ReadOnlySpan{char}, Font)"/>.
        /// Instead, you must call <see cref="CreateDrawingContext"/> every time
        /// that you want to use the <see cref="Graphics"/> object,
        /// and then call its Dispose() when you are finished using it.
        /// </remarks>
        Graphics CreateDrawingContext();

        /// <summary>
        /// Same as <see cref="CreateDrawingContext"/>. Added mainly for legacy code.
        /// </summary>
        /// <returns></returns>
        Graphics CreateGraphics();

        /// <summary>
        /// Forces the control to invalidate itself and immediately redraw itself
        /// and any child controls.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        void Invalidate();

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
        void SuspendLayout();

        /// <inheritdoc cref="AbstractControl.SetSizeToContent"/>
        void SetSizeToContent(
            WindowSizeToContentMode mode = WindowSizeToContentMode.WidthAndHeight,
            SizeD? additionalSpace = null);

        /// <summary>
        /// Converts the screen coordinates of a specified point on the screen
        /// to client-area coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that specifies the
        /// screen coordinates to be converted.</param>
        /// <returns>The converted coordinates.</returns>
        PointD ScreenToClient(PointD point);

        /// <summary>
        /// Converts the client-area coordinates of a specified point to
        /// screen coordinates.
        /// </summary>
        /// <param name="point">A <see cref="PointD"/> that contains the
        /// client coordinates to be converted.</param>
        /// <returns>The converted coordinates.</returns>
        PointD ClientToScreen(PointD point);

        /// <summary>
        /// Changes <see cref="Cursor"/> property.
        /// </summary>
        /// <param name="value">New cursor.</param>
        void SetCursor(Cursor? value);

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
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        void ResumeLayout(bool performLayout = true, bool layoutParent = true);

        /// <summary>
        /// Executes <paramref name="action"/> between calls to <see cref="BeginUpdate"/>
        /// and <see cref="EndUpdate"/>.
        /// </summary>
        /// <param name="action">Action that will be executed.</param>
        /// <remarks>
        /// Do not recreate control (or its child controls), add or remove child controls between
        /// <see cref="BeginUpdate"/> and <see cref="EndUpdate"/> calls.
        /// This method is mainly for multiple add or remove of the items in list like controls.
        /// </remarks>
        void DoInsideUpdate(Action action);

        /// <summary>
        /// Gets used <see cref="IFileSystem"/> provider.
        /// </summary>
        /// <returns>
        /// Returns value of the <see cref="FileSystem"/> property if it is not <c>null</c>;
        /// otherwise returns <see cref="Alternet.UI.FileSystem.Default"/>.
        /// </returns>
        IFileSystem GetFileSystem();

        /// <summary>
        /// Maintains performance while performing slow operations on a control
        /// by preventing the control from
        /// drawing until the <see cref="EndUpdate"/> method is called.
        /// </summary>
        /// <remarks>
        /// Do not recreate control (or its child controls), add or remove child controls between
        /// <see cref="BeginUpdate"/> and <see cref="EndUpdate"/> calls.
        /// This method is mainly for multiple add or remove of the items in list like controls.
        /// </remarks>
        int BeginUpdate();

        /// <summary>
        /// Resumes painting the control after painting is suspended by the
        /// <see cref="BeginUpdate"/> method.
        /// </summary>
        int EndUpdate();

        /// <summary>
        /// Forces the control to apply layout logic to child controls.
        /// </summary>
        /// <remarks>
        /// If the <see cref="SuspendLayout"/> method was called before calling
        /// the <see cref="PerformLayout"/> method,
        /// the layout is suppressed.
        /// </remarks>
        /// <param name="layoutParent">Specifies whether to call parent's
        /// <see cref="PerformLayout"/>. Optional. By default is <c>true</c>.</param>
        void PerformLayout(bool layoutParent = true);

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
        void BeginInit();

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
        void EndInit();

        /// <summary>
        /// Same as <see cref="Enabled"/> but implemented as method.
        /// </summary>
        /// <param name="value"></param>
        void SetEnabled(bool value);

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns><see langword="true"/> if the input focus request was
        /// successful; otherwise, <see langword="false"/>.</returns>
        /// <remarks>The <see cref="SetFocus"/> method returns true if the
        /// control successfully received input focus.</remarks>
        bool SetFocus();

        /// <summary>
        /// Sets input focus to the control.
        /// </summary>
        /// <returns>
        ///   <see langword="true" /> if the input focus request was successful;
        ///   otherwise, <see langword="false" />.
        /// </returns>
        /// <remarks>
        /// Same as <see cref="SetFocus"/>.
        /// </remarks>
        bool Focus();

        /// <summary>
        /// Sets input focus to the control if it can accept it.
        /// </summary>
        bool SetFocusIfPossible();

        /// <summary>
        /// Gets <see cref="ToolTip"/> value for use in the native control.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Override this method to customize tooltip. For example <see cref="SpeedButton"/>
        /// overrides it to add shortcuts.
        /// </remarks>
        object? GetRealToolTip();

        /// <summary>
        /// Sets children font.
        /// </summary>
        /// <param name="font">New font value</param>
        /// <param name="recursive">Whether to apply to all children recursively.</param>
        void SetChildrenFont(Font? font, bool recursive = false);

        /// <summary>
        /// Sets children background color.
        /// </summary>
        /// <param name="color">New background color value</param>
        /// <param name="recursive">Whether to apply to all children recursively.</param>
        void SetChildrenBackgroundColor(Color? color, bool recursive = false);

        /// <summary>
        /// Sets children background color.
        /// </summary>
        /// <param name="color">New background color value</param>
        /// <param name="recursive">Whether to apply to all children recursively.</param>
        void SetChildrenBackgroundColor<T>(Color? color, bool recursive = false);

        /// <summary>
        /// Sets children foreground color.
        /// </summary>
        /// <param name="color">New foreground color value</param>
        /// <param name="recursive">Whether to apply to all children recursively.</param>
        void SetChildrenForegroundColor(Color? color, bool recursive = false);

        /// <summary>
        /// Sets children foreground color.
        /// </summary>
        /// <param name="color">New foreground color value</param>
        /// <param name="recursive">Whether to apply to all children recursively.</param>
        void SetChildrenForegroundColor<T>(Color? color, bool recursive = false);

        /// <summary>
        /// Focuses the next control.
        /// </summary>
        /// <param name="forward"><see langword="true"/> to move forward in the
        /// tab order; <see langword="false"/> to move backward in the tab
        /// order.</param>
        /// <param name="nested"><see langword="true"/> to include nested
        /// (children of child controls) child controls; otherwise,
        /// <see langword="false"/>.</param>
        void FocusNextControl(bool forward = true, bool nested = true);

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
        DragDropEffects DoDragDrop(object data, DragDropEffects allowedEffects);

        /// <summary>
        /// Forces the re-creation of the underlying native control.
        /// </summary>
        void RecreateWindow();

        /// <summary>
        /// Checks whether using transparent background might work.
        /// </summary>
        /// <returns><c>true</c> if background transparency is supported.</returns>
        /// <remarks>
        /// If this function returns <c>false</c>, setting transparent background
        /// is not going to work. If it
        /// returns <c>true</c>, setting transparent style should normally succeed.
        /// </remarks>
        /// <remarks>
        /// Notice that this function would typically be called on the parent of a
        /// control you want to set transparent background style for as the control for
        /// which this method is called must be fully created.
        /// </remarks>
        bool IsTransparentBackgroundSupported();

        /// <summary>
        /// Called when the control should reposition its child controls.
        /// </summary>
        void OnLayout();

        /// <summary>
        /// Performs some action for the each child of the control.
        /// </summary>
        /// <typeparam name="T">Specifies type of the child control.</typeparam>
        /// <param name="action">Specifies action which will be called for the
        /// each child.</param>
        void ForEachChild<T>(Action<T> action)
            where T : AbstractControl;

        /// <summary>
        /// Same as <see cref="App.Log"/>.
        /// </summary>
        /// <param name="s"></param>
        void Log(object? s);

        /// <summary>
        /// Sets image for the specified control state.
        /// </summary>
        /// <param name="value">Image.</param>
        /// <param name="state">Control state.</param>
        void SetImage(Image? value, VisualControlState state = VisualControlState.Normal);

        /// <summary>
        /// Sets background brush for the specified control state.
        /// </summary>
        /// <param name="value">Background brush.</param>
        /// <param name="state">Control state.</param>
        void SetBackground(Brush? value, VisualControlState state = VisualControlState.Normal);

        /// <summary>
        /// Sets border settings for the specified control state.
        /// </summary>
        /// <param name="value">Border settings.</param>
        /// <param name="state">Control state.</param>
        void SetBorder(BorderSettings? value, VisualControlState state = VisualControlState.Normal);

        /// <summary>
        /// Resets the <see cref="Font" /> property to its default value.</summary>
        void ResetFont();

        /// <summary>
        /// Resets the <see cref="Cursor" /> property to its default value.</summary>
        void ResetCursor();

        /// <summary>
        /// Resets the <see cref="BackColor" /> property to its default value.
        /// </summary>
        void ResetBackColor();

        /// <summary>
        /// Computes the location of the specified client point into screen coordinates.
        /// </summary>
        /// <param name="p">The client coordinate <see cref="PointD" /> to convert.</param>
        /// <returns>A <see cref="PointD" /> that represents the converted <see cref="PointD" />,
        /// <paramref name="p" />, in screen coordinates.</returns>
        PointD PointToScreen(PointD p);

        /// <summary>
        /// Computes the location of the specified screen point into client coordinates.
        /// </summary>
        /// <param name="p">The screen coordinate <see cref="PointD" /> to convert.</param>
        /// <returns> A <see cref="PointD" /> that represents the converted
        /// <see cref="PointD" />, <paramref name="p" />, in client coordinates.</returns>
        PointD PointToClient(PointD p);

        /// <summary>
        /// Invalidates the specified region of the control (adds it to the control's update
        /// region, which is the area that will be repainted at the next paint operation), and
        /// causes a paint message to be sent to the control. Optionally, invalidates the
        /// child controls assigned to the control.</summary>
        /// <param name="region">The <see cref="Region" /> to invalidate.</param>
        /// <param name="invalidateChildren">
        /// <see langword="true" /> to invalidate the control's child controls;
        /// otherwise, <see langword="false"/>.</param>
        void Invalidate(Region? region, bool invalidateChildren = false);

        /// <summary>
        /// Gets background color from the default attributes.
        /// </summary>
        /// <returns></returns>
        Color? GetDefaultAttributesBgColor();

        /// <summary>
        /// Gets foreground color from the default attributes.
        /// </summary>
        /// <returns></returns>
        Color? GetDefaultAttributesFgColor();

        /// <summary>
        /// Gets font from the default attributes.
        /// </summary>
        /// <returns></returns>
        Font? GetDefaultAttributesFont();
    }
}
