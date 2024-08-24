﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class Control
    {
        /// <summary>
        /// Occurs when hovered control is changed. Control is hovered when mouse pointer is over it.
        /// </summary>
        public static event EventHandler? HoveredControlChanged;

        /// <summary>
        /// Occurs when focused control is changed.
        /// </summary>
        public static event EventHandler? FocusedControlChanged;

        /// <summary>
        /// Occurs inside <see cref="GetPreferredSize(SizeD)"/> method.
        /// </summary>
        /// <remarks>
        /// If default <see cref="Control.GetPreferredSize(SizeD)"/> call is not needed,
        /// set <see cref="HandledEventArgs.Handled"/>
        /// property to <c>true</c>.
        /// </remarks>
        public static event EventHandler<DefaultPreferredSizeEventArgs>? GlobalGetPreferredSize;

        /// <summary>
        /// Occurs when the the control should reposition its child controls.
        /// </summary>
        /// <remarks>
        /// If default layout is not needed, set <see cref="HandledEventArgs.Handled"/>
        /// property to <c>true</c>.
        /// </remarks>
        public static event EventHandler<DefaultLayoutEventArgs>? GlobalOnLayout;

        /// <summary>
        /// Occurs when cell settings are changed.
        /// </summary>
        /// <remarks>
        /// Cell settings include <see cref="RowIndex"/>, <see cref="ColumnIndex"/>,
        /// <see cref="RowSpan"/>, <see cref="ColumnSpan"/> and other properties.
        /// </remarks>
        [Category("Layout")]
        public event EventHandler? CellChanged;

        /// <summary>
        /// Occurs when the window is activated in code or by the user.
        /// </summary>
        /// <remarks>
        /// To activate a window at run time using code, call the <see cref="Window.Activate"/> method.
        /// You can use this event for
        /// tasks such as updating the contents of the window based on changes made to the
        /// window's data when the window was not activated.
        /// </remarks>
        [Category("Focus")]
        public event EventHandler? Activated;

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? TitleChanged;

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseMove;

        /// <summary>
        /// Occurs when screen is touched by the user's finger.
        /// </summary>
        /// <remarks>
        /// This event is not fired on all platforms.
        /// </remarks>
        [Category("Mouse")]
        public event EventHandler<TouchEventArgs>? Touch;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a
        /// mouse button is pressed.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button
        /// is released.</summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseUp;

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseWheel;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and left
        /// mouse button is pressed.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseLeftButtonDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and right
        /// mouse button is pressed.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseRightButtonDown;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and right mouse button
        /// is released.</summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseRightButtonUp;

        /// <summary>
        /// Occurs when the control needs to layout its children.</summary>
        [Category("Layout")]
        public event EventHandler<HandledEventArgs>? CustomLayout;

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseDoubleClick;

        /// <summary>
        /// Occurs when the mouse pointer is over the control and left mouse button
        /// is released.</summary>
        [Category("Mouse")]
        public event MouseEventHandler? MouseLeftButtonUp;

        /// <summary>
        /// Occurs when the control's handle is created.
        /// </summary>
        [Category("Private")]
        [Browsable(false)]
        public event EventHandler? HandleCreated;

        /// <summary>
        /// Occurs when the control's handle is destroyed.
        /// </summary>
        [Category("Private")]
        [Browsable(false)]
        public event EventHandler? HandleDestroyed;

        /// <summary>
        /// Occurs when the window loses focus and is no longer the active window.
        /// </summary>
        /// <remarks>
        /// You can use this event to perform tasks such as updating another window in
        /// your application with data from the deactivated window.
        /// </remarks>
        [Category("Focus")]
        public event EventHandler? Deactivated;

        /// <summary>
        /// Occurs when the contol gets focus.
        /// </summary>
        [Category("Focus")]
        public event EventHandler? GotFocus;

        /// <summary>
        /// Occurs during a drag-and-drop operation and enables the drag source
        /// to determine whether the drag-and-drop operation should be canceled.
        /// </summary>
        /// <remarks>
        /// Currently this event is not called.
        /// </remarks>
        [Category("Drag Drop")]
        public event QueryContinueDragEventHandler? QueryContinueDrag;

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        [Category("Key")]
        public event KeyEventHandler? KeyDown;

        /// <summary>
        /// Occurs when a key is released while the control has focus.
        /// </summary>
        [Category("Key")]
        public event KeyEventHandler? KeyUp;

        /// <summary>
        /// Occurs when the control lost focus.
        /// </summary>
        [Category("Focus")]
        public event EventHandler? LostFocus;

        /// <summary>
        /// Occurs when the <see cref="Control.Text" /> property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? TextChanged;

        /// <summary>
        /// Occurs when a character, space or backspace key is pressed while the control has focus.
        /// </summary>
        [Category("Key")]
        public event KeyPressEventHandler? KeyPress;

        /// <summary>
        /// Occurs when the <see cref="VisualState"/> property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? VisualStateChanged;

        /// <summary>
        /// Occurs when the <see cref="ToolTip"/> property value changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? ToolTipChanged;

        /// <summary>
        /// Occurs when the <see cref="IsMouseOver"/> property value changes.
        /// </summary>
        [Category("Mouse")]
        public event EventHandler? IsMouseOverChanged;

        /// <summary>
        /// Occurs when the control is clicked.
        /// </summary>
        [Category("Action")]
        public virtual event EventHandler? Click;

        /// <summary>
        /// Occurs when <see cref="Parent"/> is changed.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? ParentChanged;

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <remarks>
        /// The <see cref="Paint"/> event is raised when the control is redrawn.
        /// It passes an instance of <see cref="PaintEventArgs"/> to the method(s)
        /// that handles the <see cref="Paint"/> event.
        /// </remarks>
        [Category("Appearance")]
        public event EventHandler<PaintEventArgs>? Paint;

        /// <summary>
        /// Occurs when the value of the <see cref="Margin"/> property changes.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? MarginChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Padding"/> property changes.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? PaddingChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Visible"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? VisibleChanged;

        /// <summary>
        /// Occurs after control was shown.
        /// </summary>
        public event EventHandler? AfterShow;

        /// <summary>
        /// Occurs after control was hidden.
        /// </summary>
        public event EventHandler? AfterHide;

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
        [Category("Mouse")]
        public event EventHandler? MouseCaptureLost;

        /// <summary>
        /// When implemented by a class, occurs when user requests help for a control
        /// </summary>
        [Category("Behavior")]
        public event HelpEventHandler? HelpRequested;

        /// <summary>
        /// Occurs when exception is raised inside <see cref="AvoidException"/>.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler<ThrowExceptionEventArgs>? ProcessException;

        /// <summary>
        /// Occurs when the mouse pointer enters the control.
        /// </summary>
        [Category("Mouse")]
        public event EventHandler? MouseEnter;

        /// <summary>
        /// Occurs when the mouse pointer leaves the control.
        /// </summary>
        [Category("Mouse")]
        public event EventHandler? MouseLeave;

        /// <summary>
        /// Occurs when the child control is removed from this control.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler<BaseEventArgs<Control>>? ChildRemoved;

        /// <summary>
        /// Occurs when the child control's <see cref="Visible"/> property is changed.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler<BaseEventArgs<Control>>? ChildVisibleChanged;

        /// <summary>
        /// Occurs when the child control is added to this control.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler<BaseEventArgs<Control>>? ChildInserted;

        /// <summary>
        /// Occurs when the value of the <see cref="Enabled"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? EnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Background"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? BackgroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Foreground"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? ForegroundChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Font"/> property changes.
        /// </summary>
        [Category("Property Changed")]
        public event EventHandler? FontChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="VerticalAlignment"/>
        /// property changes.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? VerticalAlignmentChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HorizontalAlignment"/>
        /// property changes.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? HorizontalAlignmentChanged;

        /// <summary>
        /// Occurs when the control's size is changed.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? SizeChanged;

        /// <summary>
        /// Occurs when the application finishes processing events and is
        /// about to enter the idle state. This is the same as <see cref="App.Idle"/>
        /// but on the control level.
        /// </summary>
        /// <remarks>
        /// Use <see cref="ProcessIdle"/> property to specify whether <see cref="Idle"/>
        /// event is fired.
        /// </remarks>
        [Category("Behavior")]
        public event EventHandler? Idle;

        /// <summary>
        /// Occurs when the control's size is changed.
        /// </summary>
        [Category("Layout")]
        public event EventHandler? Resize;

        /// <summary>
        /// Occurs when the control's location is changed.
        /// </summary>
        [Category("Layout")]
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
        [Category("Drag Drop")]
        public event EventHandler<DragStartEventArgs>? DragStart;

        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<DragEventArgs>? DragDrop;

        /// <summary>
        /// Occurs when an object is dragged over the control's bounds.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<DragEventArgs>? DragOver;

        /// <summary>
        /// Occurs when an object is dragged into the control's bounds.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler<DragEventArgs>? DragEnter;

        /// <summary>
        /// Occurs when an object is dragged out of the control's bounds.
        /// </summary>
        [Category("Drag Drop")]
        public event EventHandler? DragLeave;

        /// <summary>
        /// Occurs when the DPI setting changes on the display device
        /// where the form is currently displayed.
        /// </summary>
        public event DpiChangedEventHandler? DpiChanged;

        /// <summary>
        /// Occurs when the system colors change.
        /// </summary>
        [Category("Behavior")]
        public event EventHandler? SystemColorsChanged;

        /// <summary>
        /// Occurs when the validation errors have changed for this control or it's child controls.
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;
    }
}
