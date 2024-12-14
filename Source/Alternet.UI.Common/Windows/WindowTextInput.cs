using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Implements window with <see cref="Label"/>, <see cref="TextBox"/> and buttons.
    /// </summary>
    public partial class WindowTextInput : DialogWindow
    {
        private readonly PanelOkCancelButtons buttons = new()
        {
        };

        private readonly Label label = new(CommonStrings.Default.EnterValue)
        {
            Margin = 5,
        };

        private readonly TextBoxAndButton edit = new()
        {
            Margin = 5,
            MinWidth = 200,
            ButtonsVisible = false,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowTextInput"/> class.
        /// </summary>
        public WindowTextInput()
        {
            Padding = 5;
            MinimizeEnabled = false;
            StartLocation = WindowStartLocation.CenterOwner;
            MaximizeEnabled = false;
            Resizable = false;
            Title = CommonStrings.Default.WindowTitleInput;
            Layout = LayoutStyle.Vertical;
            label.Parent = this;
            edit.HorizontalAlignment = HorizontalAlignment.Stretch;
            edit.Parent = this;
            buttons.Parent = this;
            SetSizeToContent();
            ActiveControl = edit;
            Buttons.UseModalResult = true;
        }

        /// <summary>
        /// Gets label.
        /// </summary>
        [Browsable(false)]
        public Label Label => label;

        /// <summary>
        /// Gets value editor.
        /// </summary>
        [Browsable(false)]
        public TextBoxAndButton Edit => edit;

        /// <summary>
        /// Gets panel with buttons.
        /// </summary>
        [Browsable(false)]
        public PanelOkCancelButtons Buttons => buttons;
    }
}
