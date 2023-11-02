using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// The <see cref="PopupWindow"/> displays content in a separate window that floats
    /// over the current application window.
    /// </summary>
    public class PopupWindow : Window
    {
        private static readonly BorderSettings Settings = BorderSettings.Default.Clone();
        private readonly Border border = new();
        private ModalResult popupResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupWindow"/> class.
        /// </summary>
        public PopupWindow()
            : base()
        {
            MakeAsPopup();
            border.Settings = Settings;
            border.Parent = this;
            Deactivated += Popup_Deactivated;
            KeyDown += PopupWindow_KeyDown;
        }

        /// <summary>
        /// Gets or sets default border of the <see cref="PopupWindow"/>.
        /// </summary>
        [Browsable(false)]
        public static BorderSettings DefaultBorder
        {
            get
            {
                return Settings;
            }

            set
            {
                if (value == null)
                    Settings.Assign(BorderSettings.Default);
                else
                    Settings.Assign(value);
            }
        }

        /// <summary>
        /// Gets border of the <see cref="PopupWindow"/>.
        /// </summary>
        public Border Border => border;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Escape" key.
        /// </summary>
        public bool HideOnEscape { get; set; } = true;

        /// <summary>
        /// Gets or sets owner of the popup window.
        /// </summary>
        /// <remarks>Usually owner of the popup window is a control under which popup is
        /// shown using <see cref="ShowPopup"/> method.</remarks>
        public Control? PopupOwner { get; set; }

        /// <summary>
        /// Gets or sets whether to focus <see cref="PopupOwner"/> control when popup is closed.
        /// </summary>
        public bool FocusPopupOwnerOnHide { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Enter" key.
        /// </summary>
        public bool HideOnEnter { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user double clicks left mouse button.
        /// </summary>
        public bool HideOnDoubleClick { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks left mouse button.
        /// </summary>
        public bool HideOnClick { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user clicks mouse outside it or if it loses focus in any other way.
        /// </summary>
        public bool HideOnDeactivate { get; set; } = true;

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
        /// Focuses first child control of the <see cref="Border"/>.
        /// </summary>
        public virtual void FocusChildControl()
        {
            if (Border.HasChildren)
            {
                var child = Border.Children[0];
                if (child.IsFocusable)
                    child.SetFocus();
            }
            else
            {
                if (IsFocusable)
                    SetFocus();
            }
        }

        /// <summary>
        /// Shows popup under bottom left corner of the specified control.
        /// </summary>
        /// <param name="control">Control.</param>
        public void ShowPopup(Control control)
        {
            PopupResult = ModalResult.None;
            PopupOwner = control;

            control.BeginInvoke(() =>
            {
                HandleNeeded();
                var bl = control.Handler.ClientRectangle.BottomLeft;
                var blScreen = control.ClientToScreen(bl);
                Location = blScreen;
                SetSizeToContent();
                Show();
                FocusChildControl();
            });
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
                Hide();
                Application.DoEvents();
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
        /// Default implementation of the left mouse button double click event
        /// for the main control of the popup window.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMainControlMouseDoubleClick(object? sender, MouseButtonEventArgs e)
        {
            if (HideOnDoubleClick && e.ChangedButton == MouseButton.Left)
            {
                e.Handled = true;
                HidePopup(ModalResult.Accepted);
            }
        }

        /// <summary>
        /// Creates main control of the popup window.
        /// </summary>
        protected virtual Control CreateMainControl() => new Control();

        /// <summary>
        /// Override to bind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events are binded.</param>
        protected virtual void BindEvents(Control control)
        {
            control.MouseDoubleClick += OnMainControlMouseDoubleClick;
            control.MouseLeftButtonUp += OnMainControlMouseLeftButtonUp;
        }

        /// <summary>
        /// Override to unbind events to the main control of the popup window.
        /// </summary>
        /// <param name="control">Control which events are unbinded.</param>
        protected virtual void UnbindEvents(Control control)
        {
            control.MouseDoubleClick -= OnMainControlMouseDoubleClick;
            control.MouseLeftButtonUp -= OnMainControlMouseLeftButtonUp;
        }

        /// <summary>
        /// Default implementation of the left mouse button up event
        /// for the popup control.
        /// </summary>
        /// <param name="sender">Event object.</param>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnMainControlMouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
        {
            if (HideOnClick)
            {
                e.Handled = true;
                HidePopup(ModalResult.Accepted);
            }
        }

        private void PopupWindow_KeyDown(object? sender, KeyEventArgs e)
        {
            if(HideOnEscape && e.Key == Key.Escape && e.Modifiers == ModifierKeys.None)
            {
                e.Handled = true;
                HidePopup(ModalResult.Canceled);
            }
            else
            if (HideOnEnter && e.Key == Key.Enter && e.Modifiers == ModifierKeys.None)
            {
                e.Handled = true;
                HidePopup(ModalResult.Accepted);
            }
        }

        private void Popup_Deactivated(object? sender, EventArgs e)
        {
            if (HideOnDeactivate && Visible)
                HidePopup(ModalResult.Canceled);
        }
    }
}