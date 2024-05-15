using System;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents the context menu.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="ContextMenu"/> class represents shortcut menus that can be displayed when
    /// the user clicks the right mouse
    /// button over a control or area of the form. Shortcut menus are typically used to
    /// combine different menu items
    /// from a <see cref="MainMenu"/> of a form that are useful for the user given the context
    /// of the application. For example, you
    /// can use a shortcut menu assigned to a <see cref="TextBox"/> control to provide menu items
    /// for changing the font of the text,
    /// finding text within the control, or <see cref="Clipboard"/> features for copying and
    /// pasting text. You can also display new
    /// <see cref="MenuItem"/> objects in a shortcut menu that are not located within
    /// a <see cref="MainMenu"/> to provide situation specific
    /// commands that are not appropriate for the <see cref="MainMenu"/> to display.
    /// </para>
    /// </remarks>
    [ControlCategory("MenusAndToolbars")]
    public partial class ContextMenu : Menu
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
        }

        /// <summary>
        /// Occurs when the control is opening.
        /// </summary>
        [Category("Action")]
        public event CancelEventHandler? Opening;

        /// <summary>
        /// Occurs when the control is closing.
        /// </summary>
        [Category("Action")]
        public event EventHandler? Closing;

        /// <inheritdoc cref="NonVisualControl.Left"/>
        [Browsable(false)]
        public override bool Visible { get => base.Visible; set => base.Visible = value; }

        /// <inheritdoc cref="NonVisualControl.Left"/>
        [Browsable(false)]
        public override bool Enabled { get => base.Enabled; set => base.Enabled = value; }

        /// <inheritdoc cref="NonVisualControl.Left"/>
        [Browsable(false)]
        public override string? ToolTip { get => base.ToolTip; set => base.ToolTip = value; }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ContextMenu;

        internal new IContextMenuHandler Handler => (IContextMenuHandler)base.Handler;

        public void RaiseClosing(EventArgs e)
        {
            OnClosing(e);
        }

        public void RaiseOpening(CancelEventArgs e)
        {
            OnOpening(e);
        }

        /// <summary>
        /// Displays the menu at the specified position.
        /// </summary>
        /// <param name="control">A <see cref="Control"/> that specifies the control with which
        /// this shortcut menu is associated.</param>
        /// <param name="position">
        /// A <see cref="PointD"/> that specifies the coordinates at which to display the menu.
        /// These coordinates are specified relative
        /// to the client coordinates of the control specified in the control parameter.</param>
        /// <remarks>
        /// Typically, a <see cref="ContextMenu"/> is displayed when the user clicks the right
        /// mouse button on a control
        /// or area of the form that the <see cref="ContextMenu"/> is bound to. You can use
        /// this method to manually display
        /// the shortcut menu at a specific location and bind it with a specific control. This
        /// method does not return until the menu is dismissed.
        /// </remarks>
        /// <remarks>
        /// If <paramref name="position"/> is <c>null</c> (default value), popup menu is shown
        /// under the control specified in the <paramref name="control"/> parameter.
        /// </remarks>
        public void Show(IControl control, PointD? position = null)
        {
            if (Items.Count == 0)
                return;
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            var e = new CancelEventArgs();
            RaiseOpening(e);
            if (e.Cancel)
                return;
            Handler.Show(control, position);
            RaiseClosing(EventArgs.Empty);
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return NativePlatform.Default.CreateContextMenuHandler(this);
        }

        /// <summary>
        /// Raises the <see cref="Opening" /> event.</summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> that contains
        /// the event data.</param>
        protected virtual void OnOpening(CancelEventArgs e)
        {
            ForEachItem(UpdateEnabled, true);
            Opening?.Invoke(this, e);

            void UpdateEnabled(MenuItem item)
            {
                var func = item.EnabledFunc;
                if (func is null)
                    return;
                item.Enabled = func();
            }
        }

        /// <summary>
        /// Raises the <see cref="Closing" /> event.</summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains
        /// the event data.</param>
        protected virtual void OnClosing(EventArgs e)
        {
            Closing?.Invoke(this, e);
        }
    }
}