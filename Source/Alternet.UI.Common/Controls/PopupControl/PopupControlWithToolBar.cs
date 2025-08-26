using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a popup control that contains a <see cref="ToolBar"/> as its content.
    /// </summary>
    public class PopupControlWithToolBar : PopupControl<ToolBar>, IContextMenuHost
    {
        private static PopupControlWithToolBar? defaultPopup;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupControlWithToolBar"/> class.
        /// </summary>
        public PopupControlWithToolBar()
        {
            Content.HorizontalAlignment = HorizontalAlignment.Left;
            Content.VerticalAlignment = VerticalAlignment.Top;
            Content.Margin = PopupToolBar.DefaultContentMargin;
            Content.ParentForeColor = true;
            Content.ParentBackColor = true;

            HideOnEscape = true;
            FocusContainerOnClose = true;
            HideOnEnter = true;
            HideOnClickParent = true;

            Content.ToolClick += OnToolClick;
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="PopupControlWithToolBar"/>.
        /// </summary>
        public static PopupControlWithToolBar Default
        {
            get
            {
                if (defaultPopup == null)
                {
                    defaultPopup = new ();
                }

                return defaultPopup;
            }

            set
            {
                defaultPopup = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the remaining label images are required
        /// for the toolbar items.
        /// When set to <c>true</c>, the control will ensure that all label images
        /// are assigned. For items without a specific label image, a default transparent image
        /// will be used.
        /// </summary>
        public virtual bool NeedsRemainingLabelImages { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the label text width is maximized.
        /// When set to <c>true</c>, the control will adjust the width of the label text
        /// to be the same for all toolbar items, based on the widest label text.
        /// </summary>
        public virtual bool IsLabelTextWidthMaximized { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the width of the right-side element is maximized.
        /// </summary>
        public virtual bool IsRightSideElementWidthMaximized { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the remaining images are
        /// required for the toolbar items.
        /// When set to <c>true</c>, the control will ensure that all images
        /// are assigned. For items without a specific image, a default transparent image
        /// will be used.
        /// </summary>
        public virtual bool NeedsRemainingImages { get; set; } = true;

        AbstractControl IContextMenuHost.ContextMenuHost => Content;

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(SizeD availableSize)
        {
            var preferredSize = Content.GetPreferredSize(SizeD.HalfOfMaxValueI);

            var lastChild = Content.GetVisibleChildWithMaxBottom();
            if (lastChild is not null)
            {
                preferredSize.Height = Math.Max(
                    preferredSize.Height,
                    lastChild.Bounds.Bottom + lastChild.Margin.Bottom);
            }

            preferredSize += Content.Padding.Size + Content.Margin.Size + Margin.Size + Padding.Size + 2;
            return preferredSize;
        }

        /// <summary>
        /// Updates the minimum size of the control based on its content and layout requirements.
        /// </summary>
        /// <remarks>This method recalculates the minimum size of the control by
        /// considering the current
        /// layout state  and content adjustments. It ensures that the
        /// control's size constraints are updated to reflect
        /// any changes in its content or layout preferences.</remarks>
        public virtual void UpdateMinimumSize()
        {
            Content.DoInsideLayout(() =>
            {
                if (NeedsRemainingImages)
                {
                    Content.AddRemainingImages();
                }

                if (NeedsRemainingLabelImages)
                {
                    Content.AddRemainingLabelImages();
                }

                if (IsLabelTextWidthMaximized)
                    Content.MaximizeLabelTextWidth();

                if (IsRightSideElementWidthMaximized)
                    Content.MaximizeToolRightSideElementWidth();
            });

            Content.SetSizeToContent();

            var preferredSize = GetPreferredSize();
            ClientSize = preferredSize;
            MinimumSize = MinimumSize.ClampTo(Size);
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
            if (speedButton.DropDownMenu is null)
                Close(UI.PopupCloseReason.Other);
        }
    }
}
