using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Alternet.Drawing;
using Alternet.UI.Localization;

namespace Alternet.UI
{
    /// <summary>
    /// Popup window with <see cref="ToolBar"/> control.
    /// </summary>
    public partial class PopupToolBar : PopupWindow<ToolBar>
    {
        private static PopupToolBar? defaultPopup;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupToolBar"/> class.
        /// </summary>
        public PopupToolBar()
        {
            MainPanel.Padding = 0;
            MainPanel.Margin = 0;
            MainControl.Margin = 4;
            Padding = 0;
            HasBorder = false;
            Resizable = false;
            BottomToolBar.Visible = false;
            HideOnClick = false;
            HideOnDoubleClick = false;
            HideOnEscape = true;
            FocusPopupOwnerOnHide = true;
            HideOnEnter = true;
            HideOnDeactivate = true;

            MainControl.ToolClick += OnToolClick;
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupToolBar"/>.
        /// </summary>
        public static new PopupToolBar Default
        {
            get
            {
                if (defaultPopup == null)
                {
                    defaultPopup = new PopupToolBar();
                }

                return defaultPopup;
            }

            set
            {
                defaultPopup = value;
            }
        }

        /// <inheritdoc/>
        public override bool DefaultMinimizeEnabled => false;

        /// <inheritdoc/>
        public override bool DefaultMaximizeEnabled => false;

        /// <inheritdoc/>
        public override bool DefaultHasTitleBar => false;

        /// <inheritdoc/>
        public override bool DefaultTopMost => true;

        /// <inheritdoc/>
        public override bool DefaultCloseEnabled => false;

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var preferredSize = MainControl.GetPreferredSize(
                (Graphics.HalfOfMaxValue, Graphics.HalfOfMaxValue));

            var lastChild = MainControl.LastVisibleChild;
            if (lastChild is not null)
            {
                preferredSize.Height = Math.Max(
                    preferredSize.Height,
                    lastChild.Bounds.Bottom + MainControl.Margin.Bottom + MainControl.Padding.Bottom);
            }

            preferredSize += MainControl.Margin.Size + MainPanel.Margin.Size + MainPanel.Padding.Size
                + Padding.Size + 1;
            return preferredSize;
        }

        /// <summary>
        /// Handles the event when a tool is clicked.
        /// </summary>
        /// <remarks>This method is intended to be overridden in a derived class
        /// to provide custom handling for tool click events.
        /// The base implementation closes the popup if button is clicked.</remarks>
        /// <param name="sender">The source of the event, typically the tool that was clicked.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        protected virtual void OnToolClick(object? sender, EventArgs e)
        {
            if (sender is not SpeedButton speedButton)
                return;
            if(speedButton.DropDownMenu is null)
                HidePopup(ModalResult.Accepted);
        }

        /// <inheritdoc/>
        protected override void BeforeShowPopup()
        {
            var preferredSize = GetPreferredSize();
            ClientSize = preferredSize;
            MinimumSize = MinimumSize.ClampTo(Size);
        }

        /// <inheritdoc/>
        protected override ToolBar CreateMainControl()
        {
            var result = new ToolBar()
            {
                HasBorder = false,
            };

            return result;
        }
    }
}