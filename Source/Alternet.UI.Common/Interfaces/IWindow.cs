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
    /// Provides access to the methods and properties of the window.
    /// </summary>
    public interface IWindow : IControl
    {
        /// <summary>
        /// Occurs when the value of the "Menu" property changes.
        /// </summary>
        event EventHandler? MenuChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Icon"/> property changes.
        /// </summary>
        event EventHandler? IconChanged;

        /// <summary>
        /// Occurs when the value of the "Statusbar" property changes.
        /// </summary>
        event EventHandler? StatusBarChanged;

        /// <summary>
        /// Occurs when the value of the "Owner" property changes.
        /// </summary>
        event EventHandler? OwnerChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="ShowInTaskbar"/> property changes.
        /// </summary>
        event EventHandler? ShowInTaskbarChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="State"/> property changes.
        /// </summary>
        event EventHandler? StateChanged;

        /// <summary>
        /// Occurs before the window is closed.
        /// </summary>
        /// <remarks>
        /// The <see cref="Closing"/> event occurs as the window is being closed. When a
        /// window is closed, it is disposed,
        /// releasing all resources associated with the form. If you cancel this event, the
        /// window remains opened.
        /// To cancel the closure of a window, set the <see cref="CancelEventArgs.Cancel"/>
        /// property of the
        /// <see cref="WindowClosingEventArgs"/> passed to your event handler to <c>true</c>.
        /// </remarks>
        event EventHandler<WindowClosingEventArgs>? Closing;

        /// <summary>
        /// Occurs when the window is closed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Closed"/> event occurs after the window has been closed by the user
        /// or programmatically.
        /// To prevent a window from closing, handle the <see cref="Closing"/> event and set
        /// the <see cref="CancelEventArgs.Cancel"/> property
        /// of the <see cref="WindowClosingEventArgs"/> passed to your event handler to true.
        /// </para>
        /// <para>
        /// You can use this event to perform tasks such as freeing resources used by the window
        /// and to save information entered in the form or to update its parent window.
        /// </para>
        /// </remarks>
        event EventHandler? Closed;

        /// <summary>
        /// Occurs when the value of the <see cref="MaximizeEnabled"/> property changes.
        /// </summary>
        event EventHandler? MaximizeEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="MinimizeEnabled"/> property changes.
        /// </summary>
        event EventHandler? MinimizeEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="CloseEnabled"/> property changes.
        /// </summary>
        event EventHandler? CloseEnabledChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="TopMost"/> property changes.
        /// </summary>
        event EventHandler? AlwaysOnTopChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="IsToolWindow"/> property changes.
        /// </summary>
        event EventHandler? IsToolWindowChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasSystemMenu"/> property changes.
        /// </summary>
        event EventHandler? HasSystemMenuChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="Resizable"/> property changes.
        /// </summary>
        event EventHandler? ResizableChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasBorder"/> property changes.
        /// </summary>
        event EventHandler? HasBorderChanged;

        /// <summary>
        /// Occurs when the value of the <see cref="HasTitleBar"/> property changes.
        /// </summary>
        event EventHandler? HasTitleBarChanged;

        /// <summary>
        /// Gets a value indicating whether the window is the currently active window for
        /// this application.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the form will receive key events
        /// before the event is passed to the control that has focus.</summary>
        /// <returns>
        ///   <see langword="true" /> if the form will receive all
        ///   key events; <see langword="false" /> if the currently selected
        ///   control on the form receives key events.
        ///   The default is <see langword="false" />.</returns>
        bool KeyPreview { get; set; }

        /// <summary>
        /// Gets or sets whether to supress 'Esc' key.
        /// </summary>
        /// <remarks>
        /// When 'Esc' key is not handled by the application, operating system beeps when it
        /// is pressed. Set <c>true</c> to this property in order to handle 'Esc' key
        /// so no sound will be played.
        /// </remarks>
        bool SuppressEsc { get; set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether window has title bar.
        /// </summary>
        /// <remarks>
        /// On some operating systems additional properties need to be set in order to hide
        /// the title bar. For example, <see cref="CloseEnabled"/>, <see cref="MinimizeEnabled"/>,
        /// <see cref="MaximizeEnabled"/>, <see cref="HasSystemMenu"/>, <see cref="HasBorder"/>
        /// and some other properties can affect on the title bar
        /// visibility.
        /// </remarks>
        bool HasTitleBar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window has a border.
        /// </summary>
        bool HasBorder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window can be resized by user.
        /// </summary>
        bool Resizable { get; set; }

        /// <summary>
        /// A tool window does not appear in the Windows taskbar or in the window that appears
        /// when the user presses ALT+TAB.
        /// On Windows, a tool window doesn't have minimize and maximize buttons.
        /// </summary>
        bool IsToolWindow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to create a special popup window.
        /// </summary>
        /// <remarks>
        /// Set this flag to create a special popup window: it will be always shown
        /// on top of other windows, will capture the mouse and will be dismissed when
        /// the mouse is clicked outside of it or if it loses focus in any other way.
        /// </remarks>
        bool IsPopupWindow { get; set; }

        /// <summary>
        /// Gets or sets whether system menu is visible for this window.
        /// </summary>
        bool HasSystemMenu { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window should be placed above all
        /// non-topmost windows and
        /// should stay above them, even when the window is deactivated.
        /// </summary>
        bool TopMost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled close button.
        /// </summary>
        bool CloseEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled maximize button.
        /// </summary>
        bool MaximizeEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the Windows or
        /// Linux taskbar.
        /// </summary>
        /// <value>
        /// <see langword="true"/> to display the form in the Windows taskbar at run time;
        /// otherwise, <see langword="false"/>. The default is <see langword="true"/>.
        /// </value>
        /// <remarks>
        /// You can use this property to prevent users from selecting your form through the taskbar.
        /// For example, if you display a Find and Replace tool window in your application,
        /// you might want to prevent that window from being selected through the taskbar
        /// because you would need both the application's main window and the Find and Replace
        /// tool window displayed in order to process searches appropriately.
        /// </remarks>
        bool ShowInTaskbar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the window has an enabled minimize button.
        /// </summary>
        bool MinimizeEnabled { get; set; }

        /// <summary>
        /// Gets or sets the window that owns this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> that represents the window that is the owner of this window.
        /// </value>
        /// <remarks>
        /// When a window is owned by another window, it is closed or hidden with the owner window.
        /// </remarks>
        Window? Owner { get; set; }

        /// <summary>
        /// Gets or sets the position of the window when first shown.
        /// </summary>
        /// <value>A <see cref="WindowStartLocation"/> that represents the starting position
        /// of the window.</value>
        /// <remarks>
        /// This property enables you to set the starting position of the window when it is
        /// first shown.
        /// This property should be set before the window is shown.
        /// </remarks>
        WindowStartLocation StartLocation { get; set; }

        /// <summary>
        /// Gets an array of <see cref="Window"/> objects that represent all windows that are
        /// owned by this window.
        /// </summary>
        /// <value>
        /// A <see cref="Window"/> array that represents the owned windows for this window.
        /// </value>
        /// <remarks>
        /// This property returns an array that contains all windows that are owned by this
        /// window. To make a window owned by another window, set the
        /// <see cref="Owner"/> property.
        /// When a window is owned by another window, it is closed or hidden
        /// with the owner window.
        /// </remarks>
        Window[] OwnedWindows { get; }

        /// <summary>
        /// Gets or sets a value that indicates whether window is minimized,
        /// maximized, or normal.
        /// </summary>
        WindowState State { get; set; }

        /// <summary>
        /// Gets a value indicating whether this window is displayed modally.
        /// </summary>
        bool Modal { get; }

        /// <summary>
        /// Gets or sets the icon for the window.
        /// </summary>
        /// <value>
        /// An <see cref="ImageSet"/> that represents the icon for the window.
        /// </value>
        /// <remarks>
        /// A window's icon designates the picture that represents the window in the taskbar
        /// as well as the icon that is displayed for the control box of the window.
        /// If images of several sizes are contained within the <see cref="ImageSet"/>, the
        /// most fitting size is selected automatically.
        /// </remarks>
        IconSet? Icon { get; set; }

        /// <summary>
        /// Gets window kind (window, dialog, etc.).
        /// </summary>
        /// <returns></returns>
        WindowKind GetWindowKind();
    }
}
