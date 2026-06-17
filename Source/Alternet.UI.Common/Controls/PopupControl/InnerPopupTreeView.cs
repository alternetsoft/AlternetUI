using System;
using System.Collections.Generic;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a tree view control used in inner popups.
    /// This control is supposed to be shown inside the client area of another control.
    /// </summary>
    public partial class InnerPopupTreeView : ResizablePopupControl<StdTreeView>
    {
        /// <summary>
        /// Gets or sets default minimal popup width.
        /// </summary>
        public static float DefaultMinPopupWidth = 200;

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupTreeView"/> class.
        /// </summary>
        public InnerPopupTreeView()
            : base(useScrollViewer: false)
        {
            Content.HasBorder = false;
            Content.Margin = 10;
            HasBorder = false;
            BorderControl.ToolBar.ParentBackColor = true;
            BorderControl.CloseButton.ClickAction = () => Hide();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the minimum size of the control is updated
        /// before it is shown on the screen.
        /// </summary>
        public virtual bool AllowUpdateMinimumSize { get; set; } = true;

        /// <summary>
        /// Gets or sets the extra content size to be added to the control's size when 
        /// calculating the minimum size of the popup.
        /// </summary>
        public virtual SizeD ExtraContentSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether the control's width is automatically adjusted based on its items.
        /// </summary>
        public virtual bool AutoWidthFromItems { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the control's height is automatically adjusted based on its items.
        /// </summary>
        public virtual bool AutoHeightFromItems { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to use default minimal popup width.
        /// </summary>
        public virtual bool UseDefaultMinPopupWidth { get; set; } = true;

        /// <summary>
        /// Shows popup inside the container.
        /// </summary>
        /// <param name="container">The container in which to show the popup.</param>
        /// <param name="align">The alignment to use when positioning the popup.</param>
        /// <param name="onClose">An optional action to execute when the popup is closed.</param>
        public virtual void ShowInsideControlAligned(
            AbstractControl? container,
            HVDropDownAlignment? align = null,
            Action? onClose = null)
        {
            if (container is null)
                return;

            align ??= Alternet.UI.HVDropDownAlignment.Center;

            var position = new PointD(0, 0);

            if (align is not null && align.Value.IsPositionUsed)
            {
                position = align.Value.Position;
                align = null;
            }

            ShowInContainer(container, position, align: align);
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
            if (!AllowUpdateMinimumSize)
                return;

            float minWidth = 0;
            float minHeight = 0;

            if (AutoWidthFromItems || AutoHeightFromItems)
            {
                var contentSize = Content.ListBox.GetContentSize(Content.MeasureCanvas);

                if(AutoWidthFromItems)
                    minWidth = contentSize.Width;
                if(AutoHeightFromItems)
                    minHeight = contentSize.Height;
            }

            var preferredSize  = new SizeD(minWidth, minHeight);

            preferredSize += Content.Padding.Size + Content.Margin.Size
                + Margin.Size + Padding.Size + ExtraContentSize + BorderControl.InteriorBorderSize;

            if (UseDefaultMinPopupWidth)
                preferredSize.Width = Math.Max(preferredSize.Width, DefaultMinPopupWidth);

            Size = preferredSize;
            MinimumSize = MinimumSize.ClampTo(Size);
        }

        /// <summary>
        /// Sets the size and location of the control within the specified container,
        /// optionally using a given position
        /// and alignment.
        /// </summary>
        /// <param name="container">The container in which to position and size the control. Cannot be null.</param>
        /// <param name="position">The position, relative to the container, where the control should be placed.
        /// If null, a default position is used.</param>
        /// <param name="align">The alignment to use when positioning the control within the container.
        /// If null, the default alignment is applied.</param>
        public virtual void SetSizeAndLocationInContainer(AbstractControl container,
            PointD? position = null,
            HVDropDownAlignment? align = null)
        {
            UpdateMinimumSize();
            UpdateMaxPopupSize();
            SetLocationInContainer(container, position, align);
        }

        /// <summary>
        /// Sets the location of the control within the specified container,
        /// optionally using a given position and alignment.
        /// </summary>
        /// <param name="container">The container in which to position the control. Cannot be null.</param>
        /// <param name="position">The position, relative to the container, where the control should be placed.
        /// If null, a default position is used.</param>
        /// <param name="align">The alignment to use when positioning the control within the container.
        /// If null, the default alignment is applied.</param>
        public virtual void SetLocationInContainer(
            AbstractControl container,
            PointD? position = null,
            HVDropDownAlignment? align = null)
        {
            var pos = Mouse.CoercePosition(position, container);

            var containerRect = GetContainerRect();

            if (containerRect is null)
                return;

            bool isValid = RectD.IsValid(containerRect);

            if (isValid)
            {
                var popupRect = new RectD(pos, Size);

                popupRect.Right = Math.Min(popupRect.Right, containerRect.Value.Width);
                popupRect.Bottom = Math.Min(popupRect.Bottom, containerRect.Value.Height);

                pos = popupRect.Location;
            }

            Location = pos.ClampToZero();

            if (align is not null && isValid)
            {
                Location = AlignUtils.GetDropDownPosition(
                        containerRect.Value.Size,
                        Size,
                        align);
            }
        }

        /// <summary>
        /// Sets <see cref="ContextMenu"/> as a source of the items for the tree view.
        /// </summary>
        /// <param name="menu">The context menu to use as the source of items.</param>
        public virtual void SetItemSource(ContextMenu menu)
        {
            var rootItem = menu.AsTreeViewRootItem();
            Content.RootItem = rootItem;
        }

        /// <summary>
        /// Displays the control as a popup within the specified container at the given position and alignment.
        /// </summary>
        /// <remarks>If the popup is already displayed in another container, it will be removed from that
        /// container before being shown in the new one. The popup's position and size are adjusted to ensure it remains
        /// within the bounds of the specified container.</remarks>
        /// <param name="container">The container control in which to display the popup. Cannot be null.</param>
        /// <param name="position">The position, in container coordinates, where the popup should appear.
        /// If null, the current mouse position is used.</param>
        /// <param name="align">The alignment of the popup relative to the container.
        /// If null, the default alignment is used.</param>
        public virtual void ShowInContainer(
            AbstractControl container,
            PointD? position = null,
            HVDropDownAlignment? align = null)
        {
            Parent = container;
            Container = container;

            SetSizeAndLocationInContainer(container, position, align);

            ClosedAction = () =>
            {
                Parent = null;
                Container = null;
            };

            Show();
        }
    }
}
