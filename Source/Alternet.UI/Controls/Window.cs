using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a window that makes up an application's user interface.
    /// </summary>
    /// <remarks>A <see cref="Window"/> is a representation of any window displayed in your application.</remarks>
    [System.ComponentModel.DesignerCategory("Code")]
    public class Window : Control
    {
        private string title = "";
        private WindowStartPosition startPosition = WindowStartPosition.SystemDefaultLocation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
        {
            Application.Current.RegisterWindow(this);
            SetVisibleValue(false);
            Bounds = new RectangleF(100, 100, 400, 400);
        }

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> property changes.
        /// </summary>
        public event EventHandler? TitleChanged;

        /// <summary>
        /// Occurs before the window is closed.
        /// </summary>
        /// <remarks>
        /// The <see cref="Closing"/> event occurs as the window is being closed. When a window is closed, it is disposed,
        /// releasing all resources associated with the form. If you cancel this event, the window remains opened.
        /// To cancel the closure of a window, set the <see cref="CancelEventArgs.Cancel"/> property of the
        /// <see cref="WindowClosingEventArgs"/> passed to your event handler to <c>true</c>.
        /// </remarks>
        public event EventHandler<WindowClosingEventArgs>? Closing;

        /// <summary>
        /// Occurs when the window is closed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Closed"/> event occurs after the window has been closed by the user or programmatically.
        /// To prevent a window from closing, handle the <see cref="Closing"/> event and set the <see cref="CancelEventArgs.Cancel"/> property
        /// of the <see cref="WindowClosingEventArgs"/> passed to your event handler to true.
        /// </para>
        /// <para>
        /// You can use this event to perform tasks such as freeing resources used by the window
        /// and to save information entered in the form or to update its parent window.
        /// </para>
        /// </remarks>
        public event EventHandler<WindowClosedEventArgs>? Closed;

        /// <summary>
        /// Gets or sets a title of this window.
        /// </summary>
        /// <remarks>A string that contains a title of this window. Default value is empty string ("").</remarks>
        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                TitleChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the position of the window when first shown.
        /// </summary>
        /// <value>A <see cref="WindowStartPosition"/> that represents the starting position of the window.</value>
        /// <remarks>
        /// This property enables you to set the starting position of the window when it is first shown.
        /// This property should be set before the window is shown.
        /// </remarks>
        /// <exception cref="InvalidEnumArgumentException">The value specified is outside the range of valid values.</exception>
        public WindowStartPosition StartPosition
        {
            get => startPosition;
            set
            {
                SourceGenerated.EnumValidator.Validate(value);
                startPosition = value;
            }
        }

        public SizeF Size
        {
            get
            {
                return Bounds.Size;
            }

            set
            {
                Handler.Bounds = new RectangleF(Bounds.Location, value);
            }
        }

        public override float Width { get => Size.Width; set => Size = new SizeF(value, Height); }

        public override float Height { get => Size.Height; set => Size = new SizeF(Width, value); }

        public PointF Location
        {
            get
            {
                return Bounds.Location;
            }

            set
            {
                Bounds = new RectangleF(value, Bounds.Size);
            }
        }

        public RectangleF Bounds
        {
            get
            {
                return Handler.Bounds;
            }

            set
            {
                Handler.Bounds = value;
            }
        }

        internal void RaiseClosing(WindowClosingEventArgs e) => OnClosing(e);

        internal void RaiseClosed(WindowClosedEventArgs e) => OnClosed(e);

        internal void RecreateAllHandlers()
        {
            void GetAllChildren(Control control, List<Control> result)
            {
                foreach (var child in control.Children)
                    GetAllChildren(child, result);

                if (control != this)
                    result.Add(control);
            }

            var children = new List<Control>();
            GetAllChildren(this, children);

            foreach (var child in children)
                child.DetachHandler();

            foreach (var child in children.AsEnumerable().Reverse())
                child.EnsureHandlerCreated();
        }

        protected virtual void OnClosing(WindowClosingEventArgs e) => Closing?.Invoke(this, e);

        protected virtual void OnClosed(WindowClosedEventArgs e) => Closed?.Invoke(this, e);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                Application.Current.UnregisterWindow(this);
        }

        protected override ControlHandler CreateHandler() => new NativeWindowHandler();
    }
}