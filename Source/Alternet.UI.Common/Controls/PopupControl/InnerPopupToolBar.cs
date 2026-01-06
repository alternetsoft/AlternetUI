using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a popup control that contains a <see cref="ToolBar"/> as its content.
    /// This control is used to display a toolbar in a popup manner, typically for context menus or tool palettes.
    /// </summary>
    public class InnerPopupToolBar : PopupControl<ToolBar>, IContextMenuHost
    {
        /// <summary>
        /// Gets or sets whether to show debug corners when control is painted.
        /// </summary>
        public static new bool ShowDebugCorners = false;

        /// <summary>
        /// Gets or sets default minimal popup toolbar width.
        /// </summary>
        public static float DefaultMinPopupWidth = 200;

        private static InnerPopupToolBar? defaultPopup;

        private readonly ControlSubscriber notification = new ();

        private WeakReferenceValue<AbstractControl> relatedControl = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerPopupToolBar"/> class.
        /// </summary>
        public InnerPopupToolBar()
        {
            if (App.IsMaui)
                FitParentScrollbars = false;

            Content.HorizontalAlignment = HorizontalAlignment.Left;
            Content.VerticalAlignment = VerticalAlignment.Top;
            Content.Margin = PopupToolBar.DefaultContentMargin;
            Content.ParentForeColor = true;
            Content.ParentBackColor = true;

            SuppressParentMouse = true;
            SuppressParentKeyDown = true;
            SuppressParentKeyPress = true;
            SuppressKeyPress = true;
            SuppressKeyDown = true;
            HideOnSiblingHide = true;
            HideOnSiblingShow = true;
            HideOnEscape = true;
            FocusContainerOnClose = true;
            HideOnEnter = true;
            HideOnClickParent = true;
            HideOnClickOutside = true;
            CloseOnSiblingClose = true;

            Content.ToolClick += OnToolClick;

            bool InsidePopup(object? c)
            {
                return (c as AbstractControl)?.HasIndirectParent<InnerPopupToolBar>() ?? false;
            }

            notification.BeforeControlKeyDown += (s, e) =>
            {
                if (!Visible)
                    return;
                var insidePopup = InsidePopup(s);

                if(insidePopup || !SuppressKeyDown)
                    return;
                e.Suppressed();
            };

            bool CloseIfSibling(AbstractControl? source)
            {
                if (!Visible)
                    return false;

                if (source is InnerPopupToolBar)
                    return false;

                if (RelatedControl?.IsSibling(source) ?? false)
                {
                    CloseWhenIdle(ModalResult.Canceled);
                    return true;
                }

                return false;
            }

            notification.AfterControlMouseMove += (s, e) =>
            {
                CloseIfSibling(e.Source as AbstractControl);
            };

            notification.BeforeControlMouseDown += (s, e) =>
            {
                if (!Visible)
                    return;

                var closed = CloseIfSibling(e.Source as AbstractControl);

                var insidePopup = InsidePopup(e.Source);

                if (SuppressParentMouseDown)
                {
                    e.Handled = !insidePopup;
                }

                if (!insidePopup && HideOnClickOutside && !closed)
                {
                    CloseWhenIdle(ModalResult.Canceled);
                }
            };

            notification.BeforeControlMouseUp += (s, e) =>
            {
                if (!Visible)
                    return;

                CloseIfSibling(e.Source as AbstractControl);

                if (SuppressParentMouseUp)
                {
                    var insidePopup = InsidePopup(e.Source);
                    e.Handled = !insidePopup;
                }
            };

            Content.ParentBackColor = true;
            Content.ParentForeColor = true;
            BackColor = DefaultColors.GetWindowBackColor(SystemSettings.AppearanceIsDark);
            ForeColor = DefaultColors.GetWindowForeColor(SystemSettings.AppearanceIsDark);
        }

        /// <summary>
        /// Gets or sets default instance of the <see cref="InnerPopupToolBar"/>.
        /// </summary>
        public static InnerPopupToolBar Default
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

        /// <inheritdoc/>
        public override bool ParentBackColor
        {
            get => false;

            set
            {
            }
        }

        /// <inheritdoc/>
        public override bool ParentForeColor
        {
            get => false;

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use default minimal popup toolbar width.
        /// </summary>
        public virtual bool UseDefaultMinPopupWidth { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether key down event for the controls which
        /// are not children of the popup control should be suppressed.
        /// </summary>
        public virtual bool SuppressKeyDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether key press event for the controls which
        /// are not children of the popup control should be suppressed.
        /// </summary>
        public virtual bool SuppressKeyPress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the popup should close
        /// when a tool button is clicked. Default is <c>true</c>.
        /// </summary>
        public virtual bool CloseOnToolClick { get; set; } = true;

        /// <summary>
        /// Gets or sets the source control that called the popup.
        /// </summary>
        public virtual AbstractControl? RelatedControl
        {
            get => relatedControl.Value;
            set => relatedControl.Value = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this popup should close
        /// when a sibling popup is closed.
        /// </summary>
        public virtual bool CloseOnSiblingClose { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the popup
        /// should be hidden when a click occurs outside of it.
        /// </summary>
        public virtual bool HideOnClickOutside
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether mouse events
        /// should be suppressed for the parent control.
        /// </summary>
        public virtual bool SuppressParentMouse
        {
            get
            {
                return SuppressParentMouseUp && SuppressParentMouseDown;
            }

            set
            {
                SuppressParentMouseUp = value;
                SuppressParentMouseDown = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the parent control's mouse up events
        /// should be suppressed when this control is visible.
        /// </summary>
        public virtual bool SuppressParentMouseUp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the parent control's mouse down events
        /// should be suppressed when this control is visible.
        /// </summary>
        public virtual bool SuppressParentMouseDown { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the remaining label images are required
        /// for the toolbar items.
        /// When set to <c>true</c>, the control will ensure that all label images
        /// are assigned. For items without a specific label image, a default transparent image
        /// will be used.
        /// </summary>
        public virtual bool NeedsRemainingLabelImages { get; set; } = true;

        /// <inheritdoc/>
        public override bool Visible
        {
            get => base.Visible;

            set
            {
                if (value == Visible)
                    return;

                if (value)
                {
                }
                else
                {
                }

                base.Visible = value;
            }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether the minimum size of the control is updated
        /// before it is shown on the screen.
        /// </summary>
        public virtual bool AllowUpdateMinimumSize { get; set; } = true;

        /// <summary>
        /// Gets or sets the last used alignment when showing the popup in a container.
        /// </summary>
        public virtual HVDropDownAlignment? LastUsedAlignment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the popup's position should be updated
        /// when the container is resized.
        /// </summary>
        public virtual bool UpdatePositionOnContainerResize { get; set; } = true;

        AbstractControl IContextMenuHost.ContextMenuHost => Content;

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var preferredSize = Content.GetPreferredSize(PreferredSizeContext.MaxCoord);

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

        /// <inheritdoc/>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (!Visible)
                return;

            App.AddIdleTask(() =>
            {
                SetSizeAndLocationInContainer();
            });
        }

        /// <summary>
        /// Sets the location of the control within its current container,
        /// optionally using last used alignment.
        /// </summary>
        public virtual void SetLocationInContainer()
        {
            var c = GetContainer();

            if (c is null)
                return;
            SetLocationInContainer(c, Location, LastUsedAlignment);
        }

        /// <summary>
        /// Sets the size and location of the control within its current container,
        /// optionally using a given position and last used alignment.
        /// </summary>
        public virtual void SetSizeAndLocationInContainer()
        {
            var c = GetContainer();

            if (c is null)
                return;
            SetSizeAndLocationInContainer(c, Location, LastUsedAlignment);
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
        /// <param name="align">The alignment of the popup relative to the container. If null, the default alignment is used.</param>
        public virtual void ShowInContainer(
            AbstractControl container,
            PointD? position = null,
            HVDropDownAlignment? align = null)
        {
            LastUsedAlignment = align;

            Parent = container;
            Container = container;

            SetSizeAndLocationInContainer(container, position, align);

            ClosedAction = () =>
            {
                Parent = null;
                Container = null;
            };

            Content.UpdateCommandState();
            ControlUtils.ResetIsMouseLeftButtonDown(this);
            Show();
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
        /// Closes sibling toolbars that are configured to close when a sibling is closed.
        /// </summary>
        /// <remarks>This method iterates through the visible siblings of the current object
        /// and closes any sibling that is an <see cref="InnerPopupToolBar"/> with the
        /// <see cref="InnerPopupToolBar.CloseOnSiblingClose"/> property
        /// set to <see langword="true"/>. The siblings are
        /// closed with a modal result of  <see cref="ModalResult.Canceled"/>.</remarks>
        public virtual void CloseSiblings()
        {
            foreach (var sibling in VisibleSiblings)
            {
                if (sibling is InnerPopupToolBar siblingPopup && siblingPopup.CloseOnSiblingClose)
                {
                    siblingPopup.CloseWhenIdle(ModalResult.Canceled);
                }
            }
        }

        /// <inheritdoc/>
        public override void Close(PopupCloseReason? reason)
        {
            base.Close(reason);
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
            if(!AllowUpdateMinimumSize)
                return;

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

            if (UseDefaultMinPopupWidth)
                preferredSize.Width = Math.Max(preferredSize.Width, DefaultMinPopupWidth);

            ClientSize = preferredSize;
            MinimumSize = MinimumSize.ClampTo(Size);
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnSiblingVisibleChanged(AbstractControl sibling)
        {
            base.OnSiblingVisibleChanged(sibling);
        }

        /// <inheritdoc/>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is null)
            {
                RemoveGlobalNotification(notification);
            }
            else
            {
                AddGlobalNotification(notification);
            }
        }

        /// <inheritdoc/>
        protected override void DisposeManaged()
        {
            RemoveGlobalNotification(notification);
            base.DisposeManaged();
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentKeyDown(object? sender, KeyEventArgs e)
        {
            base.OnBeforeParentKeyDown(sender, e);
        }

        /// <inheritdoc/>
        protected override void OnBeforeParentKeyPress(object? sender, KeyPressEventArgs e)
        {
            base.OnBeforeParentKeyPress(sender, e);
        }

        /// <inheritdoc/>
        protected override bool HideWhenSiblingHidden(AbstractControl sibling)
        {
            return sibling is not InnerPopupToolBar;
        }

        /// <inheritdoc/>
        protected override bool GetDefaultParentBackColor() => false;

        /// <inheritdoc/>
        protected override bool GetDefaultParentForeColor() => false;

        /// <inheritdoc/>
        protected override bool HideWhenSiblingShown(AbstractControl sibling)
        {
            return sibling is not InnerPopupToolBar;
        }

        /// <inheritdoc/>
        protected override void DefaultPaintDebug(PaintEventArgs e)
        {
            if (ShowDebugCorners)
                BorderSettings.DrawDesignCorners(e.Graphics, e.ClientRectangle);
        }

        /// <inheritdoc/>
        protected override void OnAfterParentSizeChanged(object? sender, HandledEventArgs e)
        {
            base.OnAfterParentSizeChanged(sender, e);

            if (UpdatePositionOnContainerResize)
                SetSizeAndLocationInContainer();
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetLocationInContainer();
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
            if(!CloseOnToolClick)
                return;

            if (sender is not SpeedButton speedButton)
                return;
            if (speedButton.DropDownMenu is null)
            {
                CloseSiblings();
                Close(UI.PopupCloseReason.Other);
            }
        }
    }
}
