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
    /// This class displays content in a separate window that floats
    /// over the current application window.
    /// </summary>
    /// <typeparam name="T">Type of the main control.</typeparam>
    public partial class PopupWindow<T> : DialogWindow
        where T : Control, new()
    {
        private readonly VerticalStackPanel mainPanel = new();
        private readonly ToolBar bottomToolBar = new();
        private ModalResult popupResult;
        private T? mainControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindow{T}"/> class.
        /// </summary>
        public PopupWindow()
        {
            MinimizeEnabled = DefaultMinimizeEnabled;
            MaximizeEnabled = DefaultMaximizeEnabled;
            ShowInTaskbar = false;
            StartLocation = WindowStartLocation.Manual;
            HasTitleBar = DefaultHasTitleBar;
            TopMost = DefaultTopMost;
            CloseEnabled = DefaultCloseEnabled;
            HasSystemMenu = false;

            mainPanel.Parent = this;
            Padding = DefaultPadding;
            var buttons = bottomToolBar.AddSpeedBtn(KnownButton.OK, KnownButton.Cancel);
            bottomToolBar.ItemSize = Math.Max(bottomToolBar.ItemSize, MinElementSize);
            ButtonIdOk = buttons[0];
            ButtonIdCancel = buttons[1];
            bottomToolBar.SuspendLayout();
            bottomToolBar.Padding = DefaultBotttomToolBarPadding;
            bottomToolBar.MinHeight = bottomToolBar.ItemSize + bottomToolBar.Padding.Vertical;
            bottomToolBar.SetToolAlignRight(ButtonIdOk, true);
            bottomToolBar.SetToolAlignRight(ButtonIdCancel, true);
            bottomToolBar.SetToolAction(ButtonIdOk, OnOkButtonClick);
            bottomToolBar.SetToolAction(ButtonIdCancel, OnCancelButtonClick);
            bottomToolBar.VerticalAlignment = UI.VerticalAlignment.Bottom;
            bottomToolBar.ResumeLayout();
            bottomToolBar.Parent = mainPanel;
            Deactivated += Popup_Deactivated;
            KeyDown += PopupWindow_KeyDown;
            MainControl.Required();
            Disposed += PopupWindow_Disposed;
            HideOnDeactivate = !BaseApplication.IsLinuxOS;
        }

        /// <summary>
        /// Gets or sets default bottom toolbar padding.
        /// </summary>
        public static Thickness DefaultBotttomToolBarPadding { get; set; } = (5, 5, 0, 0);

        /// <summary>
        /// Gets or sets default popup window padding.
        /// </summary>
        public static Thickness DefaultPadding { get; set; } = (5, 5, 5, 10);

        /// <summary>
        /// Gets or sets whether popups are shown using <see cref="DialogWindow.ShowModal()"/>
        /// (true) or <see cref="Control.Show"/> (false).
        /// </summary>
        /// <remarks>
        /// Under Linux popups are always shown as modal dialogs.
        /// </remarks>
        public static bool ModalPopups
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether 'Ok' button is visible.
        /// </summary>
        public bool ShowOkButton
        {
            get
            {
                return GetButtonVisible(ButtonIdOk);
            }

            set
            {
                if (ShowOkButton == value)
                    return;
                SetButtonVisible(ButtonIdOk, value);
            }
        }

        /// <summary>
        /// Gets or sets whether 'Cancel' button is visible.
        /// </summary>
        public bool ShowCancelButton
        {
            get
            {
                return GetButtonVisible(ButtonIdCancel);
            }

            set
            {
                if (ShowCancelButton == value)
                    return;
                SetButtonVisible(ButtonIdCancel, value);
            }
        }

        /// <summary>
        /// Gets 'Ok' button id.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdOk { get; }

        /// <summary>
        /// Gets 'Cancel' button id.
        /// </summary>
        [Browsable(false)]
        public ObjectUniqueId ButtonIdCancel { get; }

        /// <summary>
        /// Gets main panel (parent of the main control).
        /// </summary>
        [Browsable(false)]
        public Control MainPanel => mainPanel;

        /// <summary>
        /// Gets bottom toolbar with 'Ok', 'Cancel' and other buttons.
        /// </summary>
        [Browsable(false)]
        public ToolBar BottomToolBar => bottomToolBar;

        /// <summary>
        /// Gets default value of the <see cref="Window.MinimizeEnabled"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual bool DefaultMinimizeEnabled => false;

        /// <summary>
        /// Gets default value of the <see cref="Window.MaximizeEnabled"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual bool DefaultMaximizeEnabled => false;

        /// <summary>
        /// Gets default value of the <see cref="Window.HasTitleBar"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual bool DefaultHasTitleBar => false;

        /// <summary>
        /// Gets default value of the <see cref="Window.TopMost"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual bool DefaultTopMost => true;

        /// <summary>
        /// Gets default value of the <see cref="Window.CloseEnabled"/> property.
        /// </summary>
        [Browsable(false)]
        public virtual bool DefaultCloseEnabled => false;

        /// <summary>
        /// Gets whether popup window was already shown at least one time.
        /// </summary>
        [Browsable(false)]
        public bool WasShown { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets owner of the popup window.
        /// </summary>
        /// <remarks>Usually owner of the popup window is a control under which popup is
        /// shown using <see cref="ShowPopup(Control)"/> method.</remarks>
        [Browsable(false)]
        public Control? PopupOwner { get; set; }

        /// <summary>
        /// Gets or sets whether to focus <see cref="PopupOwner"/> control when popup is closed.
        /// </summary>
        public virtual bool FocusPopupOwnerOnHide { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Enter" key.
        /// </summary>
        public virtual bool HideOnEnter { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user double clicks left mouse button.
        /// </summary>
        public virtual bool HideOnDoubleClick { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks left mouse button.
        /// </summary>
        public virtual bool HideOnClick { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks mouse outside it or if it loses focus in any other way.
        /// </summary>
        public virtual bool HideOnDeactivate { get; set; }

        /// <summary>
        /// Gets or sets the popup result value, which is updated when popup is closed.
        /// This property is set to <see cref="ModalResult.None"/> at the moment
        /// when popup is shown.
        /// </summary>
        [Browsable(false)]
        public virtual ModalResult PopupResult
        {
            get
            {
                return popupResult;
            }

            set
            {
                popupResult = value;
            }
        }

        /// <summary>
        /// Gets or sets main control used in the popup window.
        /// </summary>
        [Browsable(false)]
        public virtual T MainControl
        {
            get
            {
                if (mainControl == null)
                {
                    mainControl = CreateMainControl();
                    mainControl.VerticalAlignment = UI.VerticalAlignment.Fill;
                    mainControl.Parent = mainPanel;
                    BindEvents(mainControl);
                }

                return mainControl;
            }

            set
            {
                if (mainControl == value || value is null)
                    return;
                if (mainControl is not null)
                {
                    UnbindEvents(mainControl);
                    mainControl.Parent = null;
                }

                mainControl = value;
                mainControl.VerticalAlignment = UI.VerticalAlignment.Fill;
                BindEvents(mainControl);
                mainControl.Parent = mainPanel;
            }
        }

        /// <summary>
        /// Gets whether specified toolbar button is visible.
        /// </summary>
        /// <param name="buttonId">Button id.</param>
        /// <returns></returns>
        public virtual bool GetButtonVisible(ObjectUniqueId buttonId)
        {
            return BottomToolBar.GetToolVisible(buttonId) && BottomToolBar.Visible;
        }

        /// <summary>
        /// Sets whether specified toolbar button is visible.
        /// </summary>
        /// <param name="buttonId">Button id.</param>
        /// <param name="visible">Button visible state.</param>
        public virtual void SetButtonVisible(ObjectUniqueId buttonId, bool visible)
        {
            BottomToolBar.SetToolVisible(buttonId, visible);
        }

        /// <summary>
        /// Focuses <see cref="MainControl"/>.
        /// </summary>
        public virtual void FocusMainControl()
        {
            if (mainControl is not null)
            {
                if (mainControl.IsFocusable)
                    mainControl.SetFocus();
            }
            else
            {
                if (IsFocusable)
                    SetFocus();
            }
        }

        /// <summary>
        /// Sets client size to the specified value.
        /// </summary>
        /// <param name="size">New client size.</param>
        public virtual void SetClientSizeTo(SizeD? size = null)
        {
            var toolHeight = BottomToolBar.Visible ? (BottomToolBar.MinHeight ?? 0) : 0;

            var ms = size ?? MainControl.Size;

            var clientHeight =
                ms.Height + toolHeight + Padding.Vertical;
            var clientWidth = ms.Width + Padding.Horizontal;
            var newSize = (clientWidth, clientHeight);
            ClientSize = newSize + new SizeD(1, 0);
            ClientSize = newSize;
            Refresh();
            SendSizeEvent();
        }

        /// <summary>
        /// Shows popup under bottom left corner of the specified control.
        /// </summary>
        /// <param name="control">Control.</param>
        public virtual void ShowPopup(Control control)
        {
            PopupOwner = control;

            var posDip = control.ClientToScreen(new PointD(0, 0));
            var szDip = control.Size;
            var sz = (0, szDip.Height);

            control.BeginInvoke(() =>
            {
                ShowPopup(posDip, sz);
            });
        }

        /// <summary>
        /// Shows popup at the specified location.
        /// </summary>
        /// <param name="ptOrigin">Popup window location.</param>
        /// <param name="sizePopup">The size of the popup window.</param>
        /// <remarks>
        /// The popup is positioned at (ptOrigin + size) if it opens below and to the right
        /// (default), at (ptOrigin - sizePopup) if it opens above and to the left.
        /// </remarks>
        /// <remarks>
        /// <paramref name="ptOrigin"/> and <paramref name="sizePopup"/> are specified in
        /// device-inpependent units (1/96 inch).
        /// </remarks>
        public virtual void ShowPopup(PointD ptOrigin, SizeD sizePopup)
        {
            PopupResult = ModalResult.None;

            if (!WasShown)
            {
                SetClientSizeTo(MainControl.Size);
            }

            SetPositionInDips(ptOrigin, sizePopup);
            BeforeShowPopup();
            Show();
            WasShown = true;
            FocusMainControl();
            if (BaseApplication.IsLinuxOS || ModalPopups)
            {
                if (ShowModal() == ModalResult.Accepted)
                    HidePopup(ModalResult.Accepted);
                else
                    HidePopup(ModalResult.Canceled);
            }
        }

        /// <summary>
        /// Hides popup window.
        /// </summary>
        /// <param name="result">New <see cref="PopupResult"/> value.</param>
        public virtual void HidePopup(ModalResult result)
        {
            if (!Visible)
                return;
            PopupResult = result;

            BeginInvoke(() =>
            {
                if (Modal)
                    ModalResult = result;
                else
                    Hide();
                BaseApplication.DoEvents();
                if (PopupOwner is not null && FocusPopupOwnerOnHide)
                {
                    PopupOwner.ParentWindow?.Activate();
                    if (PopupOwner.CanAcceptFocus)
                        PopupOwner.SetFocus();
                }

                PopupOwner = null;
            });
        }

        /// <summary>
        /// Move the popup window to the right position, i.e. such that it is entirely visible.
        /// </summary>
        /// <param name="ptOrigin">Must be given in screen coordinates.</param>
        /// <param name="size">The size of the popup window.</param>
        /// <remarks>
        /// The popup is positioned at (ptOrigin + size) if it opens below and to the right
        /// (default), at (ptOrigin - sizePopup) if it opens above and to the left.
        /// </remarks>
        /// <remarks>
        /// <paramref name="ptOrigin"/> and <paramref name="size"/> are specified in
        /// device-inpependent units (1/96 inch).
        /// </remarks>
        internal virtual void SetPositionInDips(PointD ptOrigin, SizeD size)
        {
            // determine the position and size of the screen we clamp the popup to
            PointD posScreen;
            SizeD sizeScreen;
            Display display;

            int displayNum = Display.GetFromPoint(PixelFromDip(ptOrigin));
            if (displayNum != -1)
            {
                display = new Display(displayNum);
                RectD rectScreen = display.ClientAreaDip;
                posScreen = rectScreen.Location;
                sizeScreen = rectScreen.Size;
            }
            else
            {
                // just use the primary one then
                display = Display.Primary;
                posScreen = PointD.Empty;
                sizeScreen = display.ClientAreaDip.Size;
            }

            SizeD sizeSelf = display.PixelFromDip(Size);

            // is there enough space to put the popup below the window (where we put it
            // by default)?
            double y = ptOrigin.Y + size.Height;
            if (y + sizeSelf.Height > posScreen.Y + sizeScreen.Height)
            {
                // check if there is enough space above
                if (ptOrigin.Y > sizeSelf.Height)
                {
                    // do position the control above the window
                    y -= size.Height + sizeSelf.Height;
                }

                // else: not enough space below nor above, leave below
            }

            // now check left/right too
            double x = ptOrigin.X;

            if (BaseApplication.Current.LangDirection == LangDirection.RightToLeft)
            {
                // shift the window to the left instead of the right.
                x -= size.Width;
                x -= sizeSelf.Width;        // also shift it by window width.
            }
            else
                x += size.Width;

            if (x + sizeSelf.Width > posScreen.X + sizeScreen.Width)
            {
                // check if there is enough space to the left
                if (ptOrigin.X > sizeSelf.Width)
                {
                    // do position the control to the left
                    x -= size.Width + sizeSelf.Width;
                }

                // else: not enough space there either, leave in default position
            }

            Location = (x, y);
        }

        /// <summary>
        /// Default implementation of the left mouse button double click event
        /// for the main control of the popup window.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMainControlMouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (HideOnDoubleClick && e.ChangedButton == MouseButton.Left)
            {
                HidePopup(ModalResult.Accepted);
            }
        }

        /// <inheritdoc/>
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            e.Cancel = true;
            HidePopup(ModalResult.Canceled);
        }

        /// <summary>
        /// Creates main control of the popup window.
        /// </summary>
        protected virtual T CreateMainControl() => new();

        /// <summary>
        /// Override to bind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events are binded.</param>
        protected virtual void BindEvents(Control? control)
        {
            if (control is null)
                return;
            control.MouseDoubleClick += OnMainControlMouseDoubleClick;
            control.MouseLeftButtonUp += OnMainControlMouseLeftButtonUp;
        }

        /// <summary>
        /// Override to unbind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events are unbinded.</param>
        protected virtual void UnbindEvents(Control? control)
        {
            if (control is null)
                return;
            control.MouseDoubleClick -= OnMainControlMouseDoubleClick;
            control.MouseLeftButtonUp -= OnMainControlMouseLeftButtonUp;
        }

        /// <summary>
        /// Called before popup is shown.
        /// </summary>
        protected virtual void BeforeShowPopup()
        {
        }

        /// <summary>
        /// Default implementation of the left mouse button up event
        /// for the popup control.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMainControlMouseLeftButtonUp(object? sender, MouseEventArgs e)
        {
            if (HideOnClick && HideOnClickPoint(e.Location))
            {
                HidePopup(ModalResult.Accepted);
            }
        }

        /// <summary>
        /// Gets whether mouse click on the specified point in the control closes popup window.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <returns></returns>
        protected virtual bool HideOnClickPoint(PointD point) => true;

        private void PopupWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            if (HideOnEscape && e.Key == Key.Escape && e.ModifierKeys == UI.ModifierKeys.None)
            {
                e.Handled = true;
                HidePopup(ModalResult.Canceled);
            }
            else
            if (HideOnEnter && e.Key == Key.Enter && e.ModifierKeys == UI.ModifierKeys.None)
            {
                e.Handled = true;
                HidePopup(ModalResult.Accepted);
            }
        }

        private void OnOkButtonClick()
        {
            HidePopup(ModalResult.Accepted);
        }

        private void OnCancelButtonClick()
        {
            HidePopup(ModalResult.Canceled);
        }

        private void PopupWindow_Disposed(object? sender, EventArgs e)
        {
        }

        private void Popup_Deactivated(object? sender, EventArgs e)
        {
            if (HideOnDeactivate && Visible && !Modal)
                HidePopup(ModalResult.Canceled);
        }
    }
}