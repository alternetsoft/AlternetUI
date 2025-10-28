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
    public partial class PopupWindow<T> : Window
        where T : AbstractControl, new()
    {
        /// <summary>
        /// Gets or sets whether popup window has an additional border by default.
        /// </summary>
        public static bool DefaultHasAdditionalBorder = true;

        private readonly VerticalStackPanel mainPanel = new();
        private readonly ControlSubscriber notification = new();
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

            mainPanel.HasBorder = DefaultHasAdditionalBorder;
            mainPanel.Parent = this;
            Padding = DefaultPadding;
            var buttons = bottomToolBar.AddSpeedBtn(KnownButton.OK, KnownButton.Cancel);
            bottomToolBar.ItemSize = Math.Max(bottomToolBar.ItemSize, MinElementSize);
            ButtonIdOk = buttons[0];
            ButtonIdCancel = buttons[1];
            bottomToolBar.SuspendLayout();
            bottomToolBar.Padding = DefaultBottomToolBarPadding;
            bottomToolBar.MinHeight = bottomToolBar.ItemSize + bottomToolBar.Padding.Vertical;
            bottomToolBar.SetToolAlignRight(ButtonIdOk, true);
            bottomToolBar.SetToolAlignRight(ButtonIdCancel, true);
            bottomToolBar.SetToolAction(ButtonIdOk, OnOkButtonClick);
            bottomToolBar.SetToolAction(ButtonIdCancel, OnCancelButtonClick);
            bottomToolBar.VerticalAlignment = UI.VerticalAlignment.Bottom;
            bottomToolBar.ResumeLayout();
            bottomToolBar.Parent = mainPanel;
            Deactivated += OnPopupDeactivated;
            MainControl.Required();
            Disposed += OnPopupWindowDisposed;
            HideOnDeactivate = true;
        }

        /// <summary>
        /// Flags that specify which components (such as padding and margin) should be included
        /// when calculating the interior size of the popup window and its main control.
        /// </summary>
        [Flags]
        public enum InteriorSizeFlags
        {
            /// <summary>
            /// Include the padding of the main control.
            /// </summary>
            ControlPadding = 1,

            /// <summary>
            /// Include the margin of the main control.
            /// </summary>
            ControlMargin = 2,

            /// <summary>
            /// Include the padding of the main panel.
            /// </summary>
            PanelPadding = 4,

            /// <summary>
            /// Include the margin of the main panel.
            /// </summary>
            PanelMargin = 8,

            /// <summary>
            /// Include the padding of the popup window itself.
            /// </summary>
            Padding = 16,

            /// <summary>
            /// Include difference between window size and client size (window border).
            /// </summary>
            WindowBorder = 32,

            /// <summary>
            /// Include all available components (control and panel padding/margin, and window padding).
            /// </summary>
            All = ControlPadding | ControlMargin | PanelPadding | PanelMargin | Padding | WindowBorder,
        }

        /// <summary>
        /// Gets or sets default bottom toolbar padding.
        /// </summary>
        public static Thickness DefaultBottomToolBarPadding { get; set; } = (5, 5, 0, 0);

        /// <summary>
        /// Gets or sets default popup window padding.
        /// </summary>
        public static Thickness DefaultPadding { get; set; } = (5, 5, 5, 10);

        /// <summary>
        /// Gets the <see cref="ControlSubscriber"/> associated with the notification system.
        /// This subscriber is used to manage notifications related to the popup window.
        /// It is automatically added to the global notification system when the popup
        /// window is shown and removed when it is hidden.
        /// </summary>
        public virtual ControlSubscriber Subscriber => notification;

        /// <summary>
        /// Gets or sets whether 'Ok' and 'Cancel' buttons are visible.
        /// </summary>
        [Browsable(false)]
        public bool ShowOkAndCancel
        {
            get => ShowOkButton && ShowCancelButton;
            set
            {
                ShowOkButton = value;
                ShowCancelButton = value;
            }
        }

        /// <summary>
        /// Gets or sets whether 'Ok' button is visible.
        /// </summary>
        public virtual bool ShowOkButton
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
        /// Gets or sets the increment of the popup window location.
        /// This is used to ensure that the popup window does not overlap owner control.
        /// </summary>
        public virtual SizeD PopupLocationIncrement { get; set; }

        /// <summary>
        /// Gets or sets whether 'Cancel' button is visible.
        /// </summary>
        public virtual bool ShowCancelButton
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
        public ContainerControl MainPanel => mainPanel;

        /// <summary>
        /// Gets a value indicating whether a popup can be displayed.
        /// </summary>
        public virtual bool CanShowPopup
        {
            get
            {
                return true;
            }
        }

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
        public virtual bool DefaultHasTitleBar => true;

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
        public virtual bool WasShown { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public virtual bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets owner of the popup window.
        /// </summary>
        /// <remarks>
        /// Usually owner of the popup window is a control under which popup is shown.
        /// </remarks>
        [Browsable(false)]
        public AbstractControl? PopupOwner { get; set; }

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
        /// Gets whether popup was accepted.
        /// </summary>
        [Browsable(false)]
        public bool IsPopupAccepted => PopupResult == ModalResult.Accepted;

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
        /// Gets collection of the visible popup windows of the specified type.
        /// Result is sorted by last activation time.
        /// </summary>
        public static IEnumerable<T1> GetVisiblePopups<T1>()
            where T1 : PopupWindow<T>
        {
            if (!App.HasApplication)
                yield break;

            var windows = App.Current.VisibleWindows
                .Where(x => x is T1)
                .OrderBy(x => x.LastShownTime).ToArray();

            foreach (var window in windows)
            {
                if (window is T1 popupWindow)
                {
                    yield return popupWindow;
                }
            }
        }

        /// <summary>
        /// Calculates the interior size of the control based on the specified flags.
        /// </summary>
        /// <remarks>The method calculates the interior size by summing the sizes of the specified
        /// components, such as padding and margin, for both the main control and its panel.
        /// Use the  <paramref name="flags"/> parameter to customize which components are included
        /// in the  calculation.</remarks>
        /// <param name="flags">A combination of <see cref="InteriorSizeFlags"/> values
        /// that specify which components  (e.g., padding,
        /// margin) should be included in the calculation.
        /// The default value is  <see cref="InteriorSizeFlags.All"/>,
        /// which includes all components.</param>
        /// <returns>A <see cref="SizeD"/> structure representing the total
        /// interior size based on the  specified flags. Returns
        /// <see cref="SizeD.Empty"/> if <paramref name="flags"/> is set to 0.</returns>
        public virtual SizeD GetInteriorSize(InteriorSizeFlags flags = InteriorSizeFlags.All)
        {
            if (flags == 0)
                return SizeD.Empty;
            var result = SizeD.Empty;

            if (flags.HasFlag(InteriorSizeFlags.ControlPadding))
                result += MainControl.Padding.Size;

            if (flags.HasFlag(InteriorSizeFlags.ControlMargin))
                result += MainControl.Margin.Size;

            if (flags.HasFlag(InteriorSizeFlags.PanelPadding))
                result += MainPanel.Padding.Size;

            if (flags.HasFlag(InteriorSizeFlags.PanelMargin))
                result += MainPanel.Margin.Size;

            if (flags.HasFlag(InteriorSizeFlags.Padding))
                result += Padding.Size;

            if (flags.HasFlag(InteriorSizeFlags.WindowBorder))
                result += InteriorSize;

            return result;
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
                mainControl.SetFocusIdle();
            }
            else
            {
                SetFocusIdle();
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
            SizeD newSize = (clientWidth, clientHeight);

            newSize += InteriorSize;

            Size = newSize + new SizeD(1, 0);
            Size = newSize;
            Refresh();
            PerformLayout();
        }

        /// <summary>
        /// Displays a popup window near the specified control.
        /// If position is not specified, the popup is shown below and to the left of the control.
        /// </summary>
        /// <param name="control">The control which is used to calculate the popup position.
        /// This parameter cannot be <see langword="null"/>.</param>
        /// <param name="position">The optional horizontal and vertical alignment of the popup
        /// relative to the control. If <see langword="null"/>, the default alignment is used.</param>
        public virtual void ShowPopup(AbstractControl control, HVDropDownAlignment? position = null)
        {
            PopupOwner = control;

            var posDip = control.ClientToScreen(PointD.Empty);
            posDip += PopupLocationIncrement;
            var szDip = control.Size;

            RunWhenIdle(() =>
            {
                if(!DisposingOrDisposed)
                    ShowPopup(posDip, szDip, position);
            });
        }

        /// <summary>
        /// Shows popup at the specified location.
        /// </summary>
        /// <param name="ptOrigin">Popup window location.</param>
        /// <param name="sizePopup">The size of the popup window.</param>
        /// <param name="position">The optional horizontal and vertical alignment of the popup
        /// relative to the control. If <see langword="null"/>, the default alignment is used.</param>
        /// <remarks>
        /// The popup is positioned at (ptOrigin + size) if it opens below and to the right
        /// (default), at (ptOrigin - sizePopup) if it opens above and to the left.
        /// </remarks>
        /// <remarks>
        /// <paramref name="ptOrigin"/> and <paramref name="sizePopup"/> are specified in
        /// device-independent units.
        /// </remarks>
        public virtual void ShowPopup(
            PointD? ptOrigin,
            SizeD sizePopup,
            HVDropDownAlignment? position = null)
        {
            if(!CanShowPopup)
                return;

            if (!WasShown)
            {
                SetClientSizeTo(MainControl.Size);
            }

            BeforeShowPopup();

            if(ptOrigin is not null)
            {
                SetPositionInDips(ptOrigin.Value, sizePopup, position);
            }

            PopupResult = ModalResult.None;
            ActiveControl = MainControl;
            ShowAndFocus();
            WasShown = true;
        }

        /// <summary>
        /// Shows popup window using the specified options.
        /// This method aligns popup window location inside the specified display's
        /// client area using given horizontal and vertical alignment.
        /// </summary>
        /// <param name="horz">Horizontal alignment of the window
        /// inside display's client area.</param>
        /// <param name="vert">Vertical alignment of the window
        /// inside display's client area.</param>
        /// <param name="display">Display which client area is used
        /// as a container for the window.</param>
        /// <param name="shrinkSize">Whether to shrink size of the window
        /// to fit in the display client area. Optional. Default is <c>true</c>.</param>
        public virtual void ShowPopup(
            HorizontalAlignment? horz,
            VerticalAlignment? vert,
            Display? display = null,
            bool shrinkSize = true)
        {
            SetLocationOnDisplay(horz, vert, display, shrinkSize);
            ShowPopup(null, SizeD.Empty);
        }

        /// <summary>
        /// Hides popup window.
        /// </summary>
        /// <param name="result">New <see cref="PopupResult"/> value.</param>
        /// <param name="focusOwner"></param>
        public virtual void HidePopup(ModalResult result, bool focusOwner = true)
        {
            if (!Visible)
                return;
            PopupResult = result;

            Post(Hide);

            var a = PopupOwner;

            PopupOwner = null;

            if (focusOwner)
            {
                Post(() =>
                {
                    if (a is not null)
                    {
                        if (FocusPopupOwnerOnHide)
                        {
                            a.ParentWindow?.ShowAndFocus();
                            a.SetFocusIfPossible();
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Move the popup window to the right position, i.e. such that it is entirely visible.
        /// </summary>
        /// <param name="ptOrigin">Must be given in screen coordinates.</param>
        /// <param name="size">The size of the popup window.</param>
        /// <param name="position">The optional horizontal and vertical alignment of the popup
        /// relative to the control. If <see langword="null"/>, the default alignment is used.</param>
        /// <remarks>
        /// Default popup position is (ptOrigin + size) if it opens below and to the right
        /// (default), at (ptOrigin - size) if it opens above and to the left.
        /// </remarks>
        /// <remarks>
        /// <paramref name="ptOrigin"/> and <paramref name="size"/> are specified in
        /// device-independent units.
        /// </remarks>
        public virtual void SetPositionInDips(
            PointD ptOrigin,
            SizeD size,
            HVDropDownAlignment? position = null)
        {
            SizeD sizeSelf = Size;

            ptOrigin += AlignUtils.GetDropDownPosition(size, sizeSelf, position) - size;

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

            // is there enough space to put the popup below the window
            // (where we put it by default)?
            Coord y = ptOrigin.Y + size.Height;
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
            Coord x = ptOrigin.X;

            if (App.Current.LangDirection == LangDirection.RightToLeft)
            {
                // shift the window to the left instead of the right.
                x -= size.Width;
                x -= sizeSelf.Width;        // also shift it by window width.
            }
            else
            {
                x += size.Width;
            }

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

            x = Math.Max(x, posScreen.X);

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

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                AddGlobalNotification(notification);
            }
            else
            {
                RemoveGlobalNotification(notification);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            RemoveGlobalNotification(notification);
            base.DisposeManaged();
        }

        /// <summary>
        /// Creates main control of the popup window.
        /// </summary>
        protected virtual T CreateMainControl() => new();

        /// <summary>
        /// Override to bind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events to bind.</param>
        protected virtual void BindEvents(AbstractControl? control)
        {
            if (control is null)
                return;
            control.MouseDoubleClick += OnMainControlMouseDoubleClick;
            control.MouseLeftButtonUp += OnMainControlMouseLeftButtonUp;
        }

        /// <summary>
        /// Override to unbind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events to unbind.</param>
        protected virtual void UnbindEvents(AbstractControl? control)
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

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (HideOnEscape && e.Key == Key.Escape && e.ModifierKeys == UI.ModifierKeys.None)
            {
                e.Suppressed();
                HidePopup(ModalResult.Canceled);
            }
            else
            if (HideOnEnter && e.Key == Key.Enter && e.ModifierKeys == UI.ModifierKeys.None)
            {
                e.Suppressed();
                HidePopup(ModalResult.Accepted);
            }

            base.OnKeyDown(e);
        }

        private void OnOkButtonClick()
        {
            HidePopup(ModalResult.Accepted);
        }

        private void OnCancelButtonClick()
        {
            HidePopup(ModalResult.Canceled);
        }

        private void OnPopupWindowDisposed(object? sender, EventArgs e)
        {
        }

        private void OnPopupDeactivated(object? sender, EventArgs e)
        {
            if (HideOnDeactivate && Visible)
            {
                Post(() => HidePopup(ModalResult.Canceled, focusOwner: false));
            }
        }
    }
}