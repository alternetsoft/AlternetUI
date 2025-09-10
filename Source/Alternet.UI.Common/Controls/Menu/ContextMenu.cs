using System;
using System.Collections.Generic;
using System.ComponentModel;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="ContextMenu"/> class represents shortcut menus that can be displayed when
    /// the user clicks the right mouse button over a control or area of the form.
    /// </summary>
    /// <remarks>
    /// Shortcut menus are typically used to combine different menu items
    /// from a <see cref="MainMenu"/> of a form that are useful for the user given the context
    /// of the application. For example, you
    /// can use a shortcut menu assigned to a <see cref="TextBox"/> control to provide menu items
    /// for changing the font of the text,
    /// finding text within the control, or <see cref="Clipboard"/> features for copying and
    /// pasting text. You can also display new
    /// <see cref="MenuItem"/> objects in a shortcut menu that are not located within
    /// a <see cref="MainMenu"/> to provide situation specific
    /// commands that are not appropriate for the <see cref="MainMenu"/> to display.
    /// </remarks>
    [ControlCategory("MenusAndToolbars")]
    public partial class ContextMenu : Menu, IContextMenuProperties
    {
        private WeakReferenceValue<AbstractControl> relatedControl = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextMenu"/> class.
        /// </summary>
        public ContextMenu()
        {
        }

        /// <summary>
        /// Occurs when the menu is opening. This event is usually raised only for top level menus.
        /// You can also handle <see cref="MenuItem.Opened"/> to be notified when a menu item is opened.
        /// </summary>
        [Category("Action")]
        public event CancelEventHandler? Opening;

        /// <summary>
        /// Occurs when the menu is closing. This event is usually raised only for top level menus.
        /// </summary>
        [Category("Action")]
        public event EventHandler? Closing;

        /// <summary>
        /// Occurs when the menu is closed. This event is usually raised only for top level menus.
        /// </summary>
        [Category("Action")]
        public event EventHandler<ToolStripDropDownClosedEventArgs>? Closed;

        /// <summary>
        /// Gets or sets the control that was the last source of context menu invocation.
        /// </summary>
        public virtual AbstractControl? RelatedControl
        {
            get => relatedControl.Value;
            set => relatedControl.Value = value;
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
                if (HostObjects is null)
                    return false;
                foreach (var host in HostObjects)
                {
                    if (Internal(host as IContextMenuHost))
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
        /// Determines whether any visible child of the specified control
        /// implements the <see cref="IContextMenuHost"/>
        /// interface.
        /// </summary>
        /// <param name="control">The control whose children are to be inspected.
        /// Can be <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if any child of the specified control
        /// implements the <see cref="IContextMenuHost"/>
        /// interface;  otherwise, <see langword="false"/>. Returns <see langword="false"/>
        /// if <paramref name="control"/> is <see langword="null"/> or has no children.</returns>
        public static bool HostControlInChildren(AbstractControl? control)
        {
            if (control is null || !control.HasChildren)
                return false;

            foreach (var child in control.Children)
            {
                if (child.Visible && child is IContextMenuHost)
                    return true;
            }

            return false;
        }

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
            StaticMenuEvents.RaiseMenuClosing(this, e);
            Closed?.Invoke(this, new(ToolStripDropDownCloseReason.Other));
            OnClosing(e);

            ForEachItem(RaiseItemClosed, recursive: true);

            void RaiseItemClosed(MenuItem item)
            {
                item.RaiseClosed();
            }
        }

        /// <summary>
        /// Raises the <see cref="Opening" /> event and <see cref="OnOpening"/> method.</summary>
        /// <param name="e">A <see cref="EventArgs" /> that contains
        /// the event data.</param>
        public void RaiseOpening(CancelEventArgs e)
        {
            ForEachItem(UpdateEnabled, recursive: true);
            StaticMenuEvents.RaiseMenuOpening(this, e);
            Opening?.Invoke(this, e);

            static void UpdateEnabled(MenuItem item)
            {
                var func = item.EnabledFunc;
                if (func is null)
                    return;
                item.Enabled = func();
            }

            OnOpening(e);

            ForEachItem(RaiseItemOpened, recursive: true);

            void RaiseItemOpened(MenuItem item)
            {
                item.RaiseOpened();
            }
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
            if (Items.Count == 0)
                return;
            relatedControl.Value = control;

            var hostControl = GetHostObject<PopupToolBar>();

            if (hostControl is null)
            {
                var popupWindow = new PopupToolBar();
                hostControl = popupWindow;
                AddHostObject(popupWindow);
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
        /// Uses <see cref="InnerPopupToolBar"/> as the host control.
        /// </summary>
        /// <remarks>If the context menu is not already hosted
        /// within a <see cref="InnerPopupToolBar"/>,
        /// a new instance of <see cref="InnerPopupToolBar"/> is created
        /// and configured as the host. The control
        /// is positioned within the container, ensuring it remains visible within
        /// the container's bounds.</remarks>
        /// <param name="source">The control that triggered the context menu.</param>
        /// <param name="container">The container in which the context menu will be displayed.
        /// This parameter cannot be null.</param>
        /// <param name="position">The position within the container where the context menu
        /// should be displayed. If null, the current mouse position
        /// relative to the container is used.</param>
        /// <param name="onClose">The action to be invoked when the context menu is closed.</param>
        public virtual void ShowInsideControl(
            AbstractControl container,
            AbstractControl? source = null,
            PointD? position = null,
            Action? onClose = null)
        {
            var hostControl = GetHostObject<InnerPopupToolBar>();
            relatedControl.Value = null;

            if (hostControl is not null)
            {
                hostControl.RelatedControl = null;
            }

            if (Items.Count == 0)
                return;
            relatedControl.Value = source;

            if (hostControl is null)
            {
                hostControl = new InnerPopupToolBar();
                hostControl.Content.DataContext = this;
                hostControl.Content.ConfigureAsContextMenu();
                AddHostObject(hostControl);
            }

            hostControl.RelatedControl = source;
            var pos = Mouse.CoercePosition(position, container);

            if (hostControl is InnerPopupToolBar popupToolBar)
            {
                if(popupToolBar.Parent is not null)
                {
                    popupToolBar.Parent = null;
                    popupToolBar.Container = null;
                }

                popupToolBar.Container = container;
                popupToolBar.UpdateMinimumSize();
                popupToolBar.UpdateMaxPopupSize();

                var containerRect = popupToolBar.GetContainerRect();

                if (containerRect is not null)
                {
                    var popupRect = new RectD(pos, popupToolBar.Size);

                    popupRect.Right = Math.Min(popupRect.Right, containerRect.Value.Right);
                    popupRect.Bottom = Math.Min(popupRect.Bottom, containerRect.Value.Bottom);

                    pos = popupRect.Location;
                }

                popupToolBar.Location = pos.ClampToZero();
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
            ShowAtFactory(control, position);
        }

        /// <summary>
        /// Displays the context menu at the specified position using the provided control and menu factory.
        /// </summary>
        /// <remarks>If the control is <see langword="null"/> or the menu contains no items, the method
        /// does nothing. If the <paramref name="factory"/> is not provided, the default factory is used.
        /// The method raises the <c>Opening</c> event before displaying the menu, allowing the operation
        /// to be canceled. When the menu is closed, the <c>Closing</c> event is raised.</remarks>
        /// <param name="control">The control associated with the context menu. This parameter cannot
        /// be <see langword="null"/>.</param>
        /// <param name="position">The position where the context menu should be displayed,
        /// or <see langword="null"/> to use the default position.</param>
        /// <param name="factory">The menu factory used to create and manage the context menu,
        /// or <see langword="null"/> to use the default factory.</param>
        public virtual void ShowAtFactory(
            AbstractControl control,
            PointD? position = null,
            IMenuFactory? factory = null)
        {
            if (DisposingOrDisposed)
                return;
            relatedControl.Value = null;
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
                relatedControl.Value = control;

                factory ??= MenuUtils.Factory;

                factory?.Show(
                        this,
                        control,
                        position,
                        () =>
                        {
                            RaiseClosing(EventArgs.Empty);
                        });
            }
            finally
            {
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            base.DisposeManaged();
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