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
        /// Gets or sets a value indicating whether a popup window disappears automatically
        /// when the user presses "Enter" key.
        /// </summary>
        public bool HideOnEnter { get; set; } = true;

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
        public ModalResult PopupResult
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
        public void PopupUnderControl(Control control)
        {
            PopupResult = ModalResult.None;
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

        private void HidePopup(ModalResult result)
        {
            if (!Visible)
                return;
            PopupResult = result;
            Hide();
        }

        private void PopupWindow_KeyDown(object sender, KeyEventArgs e)
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