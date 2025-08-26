using System;
using System.Collections.Generic;
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
        private IList<IContextMenuHost>? hostControls;

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

        /// <summary>
        /// Occurs when the control is closing.
        /// </summary>
        [Category("Action")]
        public event EventHandler<ToolStripDropDownClosedEventArgs>? Closed;

        /// <summary>
        /// This property has no meaning.
        /// </summary>
        [Browsable(false)]
        public override bool Visible
        {
            get => base.Visible;
            set => base.Visible = value;
        }

        /// <summary>
        /// Gets list of host controls. It can contain <see cref="IContextMenuHost"/> controls
        /// such as <see cref="PopupToolBar"/> or <see cref="PopupControlWithToolBar"/>.
        /// </summary>
        [Browsable(false)]
        public IList<IContextMenuHost>? HostControls
        {
            get
            {
                return hostControls;
            }
        }

        /// <summary>
        /// This property has no meaning.
        /// </summary>
        [Browsable(false)]
        public override bool Enabled
        {
            get => base.Enabled;
            set => base.Enabled = value;
        }

        /// <summary>
        /// Gets a value indicating whether the current instance is currently
        /// displayed within a host control.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsShownInHostControl
        {
            get
            {
                if (HostControls is null)
                    return false;
                foreach (var host in HostControls)
                {
                    if (Internal(host))
                        return true;
                }

                bool Internal(IContextMenuHost? host)
                {
                    if (host is null || !host.IsVisible)
                        return false;

                    var dataContext = host.ContextMenuHost.DataContext as IMenuProperties;
                    var result = dataContext?.UniqueId == UniqueId;
                    return result;
                }

                return false;
            }
        }

        /// <summary>
        /// This property has no meaning.
        /// </summary>
        [Browsable(false)]
        public override string? ToolTip
        {
            get => base.ToolTip;
            set => base.ToolTip = value;
        }

        /// <inheritdoc/>
        public override ControlTypeId ControlKind => ControlTypeId.ContextMenu;

        /// <summary>
        /// Gets handler.
        /// </summary>
        [Browsable(false)]
        public new IContextMenuHandler Handler => (IContextMenuHandler)base.Handler;

        /// <summary>
        /// Copies the properties from the specified <see cref="IMenuProperties"/>
        /// source to the current instance.
        /// </summary>
        /// <param name="source">The source object containing the properties to copy.
        /// Cannot be <see langword="null"/>.</param>
        public virtual void Assign(IMenuProperties source)
        {
            Items.SetCount(source.Count, () => new MenuItem());

            for (var i = 0; i < source.Count; i++)
            {
                if (source.GetItem(i) is not IMenuItemProperties sourceItem)
                    continue;
                Items[i].Assign(sourceItem);
            }
        }

        /// <summary>
        /// Shows the context menu as a drop-down menu under the specified control.
        /// This method uses native menu via handler.
        /// </summary>
        /// <param name="control">The control with which this context menu is associated.</param>
        /// <param name="afterShow">The action to be invoked after the menu is shown.</param>
        /// <param name="position">The position of the drop-down menu.</param>
        public virtual void ShowAsDropDown(
            AbstractControl control,
            Action? afterShow = null,
            HVDropDownAlignment? position = null)
        {
            if (control is null)
                throw new ArgumentNullException(nameof(control));

            Post(() =>
            {
                var pt = AlignUtils.GetDropDownPosition(control.Size, 0, position);
                Show(control, pt);
                afterShow?.Invoke();
            });
        }

        /// <summary>
        /// Raises the <see cref="Closing" /> event and <see cref="OnClosing"/> method.</summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains
        /// the event data.</param>
        public void RaiseClosing(EventArgs e)
        {
            Closing?.Invoke(this, e);
            Closed?.Invoke(this, new(ToolStripDropDownCloseReason.Other));
            OnClosing(e);
        }

        /// <summary>
        /// Raises the <see cref="Opening" /> event and <see cref="OnOpening"/> method.</summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains
        /// the event data.</param>
        public void RaiseOpening(CancelEventArgs e)
        {
            ForEachItem(UpdateEnabled, true);
            Opening?.Invoke(this, e);

            static void UpdateEnabled(MenuItem item)
            {
                var func = item.EnabledFunc;
                if (func is null)
                    return;
                item.Enabled = func();
            }

            OnOpening(e);
        }

        /// <summary>
        /// Retrieves the first host control of the specified type
        /// from the available host controls.
        /// </summary>
        /// <remarks>This method iterates through the collection of host controls
        /// and returns the first
        /// instance that matches the specified type <typeparamref name="T"/>.
        /// If no matching host control is found, the
        /// method returns <see langword="null"/>.</remarks>
        /// <typeparam name="T">The type of the host control to retrieve.
        /// Must be a class that implements <see cref="IContextMenuHost"/>.</typeparam>
        /// <returns>The first host control of type <typeparamref name="T"/> if found;
        /// otherwise, <see langword="null"/>.</returns>
        public virtual T? GetHostControl<T>()
            where T : class, IContextMenuHost
        {
            if (HostControls is not null)
            {
                foreach (var host in HostControls)
                {
                    if (host is T typedHost)
                        return typedHost;
                }
            }

            return null;
        }

        /// <summary>
        /// Adds a host control to the collection of context menu hosts if it is not already present.
        /// </summary>
        /// <remarks>This method ensures that the specified host control is added only once to the
        /// collection.</remarks>
        /// <param name="host">The host control to add. Cannot be <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="host"/>
        /// is <see langword="null"/>.</exception>
        public virtual void AddHostControl(IContextMenuHost host)
        {
            if (host is null)
                throw new ArgumentNullException(nameof(host));
            if (hostControls is null)
                hostControls = new List<IContextMenuHost>();
            if (!hostControls.Contains(host))
                hostControls.Add(host);
        }

        /// <summary>
        /// Displays the context menu in a popup window.
        /// </summary>
        /// <remarks>This method creates and configures a popup window to display the context menu.
        /// If the popup is already initialized, it reuses the existing instance.
        /// The <paramref name="afterShow"/> callback
        /// is executed after the popup is fully displayed, allowing additional actions
        /// to be performed.</remarks>
        /// <param name="control">The control that specifies the position origin.</param>
        /// <param name="afterShow">An optional callback action to be invoked after the
        /// popup is displayed.</param>
        /// <param name="dropDownMenuPosition">An optional parameter specifying the alignment
        /// of the popup relative to the control. If not provided,
        /// a default alignment is used.</param>
        public virtual void ShowInPopup(
            AbstractControl control,
            Action? afterShow = null,
            HVDropDownAlignment? dropDownMenuPosition = null)
        {
            var hostControl = GetHostControl<PopupToolBar>();

            if (hostControl is null)
            {
                var popupWindow = new PopupToolBar();
                hostControl = popupWindow;
                AddHostControl(popupWindow);
                popupWindow.MainControl.DataContext = this;
                popupWindow.MainControl.ConfigureAsContextMenu();
                popupWindow.Activated += (s, e) =>
                {
                    Post(() =>
                    {
                        PopupToolBar.IsHideOnDeactivateSuppressed = false;
                        afterShow?.Invoke();
                    });
                };
            }

            if (hostControl is PopupToolBar popupToolBar)
            {
                PopupToolBar.IsHideOnDeactivateSuppressed = true;
                popupToolBar.ShowPopup(control, dropDownMenuPosition);
            }
        }

        /// <summary>
        /// Displays the context menu inside the specified container at the given position.
        /// Uses <see cref="PopupControlWithToolBar"/> as the host control.
        /// </summary>
        /// <remarks>If the context menu is not already hosted
        /// within a <see cref="PopupControlWithToolBar"/>,
        /// a new instance of <see cref="PopupControlWithToolBar"/> is created
        /// and configured as the host. The control
        /// is positioned within the container, ensuring it remains visible within
        /// the container's bounds.</remarks>
        /// <param name="container">The container in which the context menu will be displayed.
        /// This parameter cannot be null.</param>
        /// <param name="position">The position within the container where the context menu
        /// should be displayed. If null, the current mouse position
        /// relative to the container is used.</param>
        public virtual void ShowInsideControl(AbstractControl container, PointD? position = null)
        {
            var hostControl = GetHostControl<PopupControlWithToolBar>();

            if (hostControl is null)
            {
                var popupWindow = new PopupControlWithToolBar();
                popupWindow.Content.DataContext = this;
                popupWindow.Content.ConfigureAsContextMenu();
                hostControl = popupWindow;
                AddHostControl(popupWindow);
            }

            var pos = position ?? Mouse.GetPosition(container);

            if (hostControl is PopupControlWithToolBar popupToolBar)
            {
                popupToolBar.Container = container;
                popupToolBar.UpdateMinimumSize();
                popupToolBar.UpdateMaxPopupSize();

                var containerRect = popupToolBar.GetContainerRect();

                if (containerRect is not null)
                {
                    popupToolBar.EnsureVisible(
                                ref pos,
                                containerRect.Value,
                                adjustLine: false);
                }

                popupToolBar.Location = pos;
                popupToolBar.Parent = container;

                popupToolBar.ClosedAction = () =>
                {
                    popupToolBar.Parent = null;
                    popupToolBar.Container = null;
                };

                popupToolBar.Show();
            }
        }

        /// <summary>
        /// Displays the menu at the specified position.
        /// </summary>
        /// <param name="control">A <see cref="AbstractControl"/> that specifies
        /// the control with which
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
        public virtual void Show(AbstractControl control, PointD? position = null)
        {
            if (Items.Count == 0)
                return;
            if (control is null)
                return;
            try
            {
                var e = new CancelEventArgs();
                RaiseOpening(e);
                if (e.Cancel)
                    return;
                Handler.Show(control, position);
                RaiseClosing(EventArgs.Empty);
            }
            finally
            {
            }
        }

        /// <inheritdoc/>
        protected override IControlHandler CreateHandler()
        {
            return (IControlHandler)ControlFactory.Handler.CreateContextMenuHandler(this);
        }

        /// <summary>
        /// Called when menu is opening.
        /// </summary>
        /// <param name="e">A <see cref="CancelEventArgs" /> that contains
        /// the event data.</param>
        protected virtual void OnOpening(CancelEventArgs e)
        {
        }

        /// <summary>
        /// Called when menu is closing.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains
        /// the event data.</param>
        protected virtual void OnClosing(EventArgs e)
        {
        }
    }
}