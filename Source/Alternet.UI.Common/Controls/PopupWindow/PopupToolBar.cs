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
    public partial class PopupToolBar : PopupWindow<ToolBar>, IContextMenuHost
    {
        /// <summary>
        /// Represents the default margin applied to <see cref="ToolBar"/>,
        /// measured in device-independent units.
        /// </summary>
        /// <remarks>This value is commonly used to provide consistent spacing around content in user
        /// interface layouts.</remarks>
        public static Thickness DefaultContentMargin = 4;

        private static PopupToolBar? defaultPopup;
        private static int hideOnDeactivateSuppressCounter;

        private bool hideOtherPopupsSuppressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupToolBar"/> class.
        /// </summary>
        public PopupToolBar()
        {
            MainPanel.Padding = 0;
            MainPanel.Margin = 0;
            MainControl.Margin = DefaultContentMargin;
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

            Subscriber.AfterControlMouseEnter += (s, e) =>
            {
                if (!Visible)
                    return;
                var control = s as AbstractControl;

                if (PopupOwner?.IsSibling(control) ?? false)
                {
                    if (!PopupOwner.HasIndirectParent<PopupToolBar>())
                        return;
                    HideAllAfterThis();
                    HideOnlyThisPopup();
                }
            };

            Subscriber.AfterControlVisibleChanged += (s, e) =>
            {
                if (!Visible || s == this)
                    return;
                if (s is not PopupToolBar control)
                    return;
                if (!control.Visible)
                    return;
                if (PopupOwner?.IsSibling(control.PopupOwner) ?? false)
                {
                    HideAllAfterThis();
                    HideOnlyThisPopup();
                }
            };
        }

        /// <summary>
        /// Indicates whether the suppression of the "Hide on Deactivate" behavior is enabled.
        /// Default is <c>false</c>.
        /// </summary>
        public static bool IsHideOnDeactivateSuppressed
        {
            get
            {
                return hideOnDeactivateSuppressCounter > 0;
            }
        }

        /// <summary>
        /// Gets a collection of currently visible popup toolbars.
        /// </summary>
        /// <remarks>This property retrieves all popup toolbars that are currently visible in the
        /// application. Use this property to enumerate or interact with the
        /// visible popup toolbars.</remarks>
        public static IEnumerable<PopupToolBar> VisiblePopupToolbars
        {
            get
            {
                return GetVisiblePopups<PopupToolBar>();
            }
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
        public override bool HideOnDeactivate
        {
            get
            {
                if(IsHideOnDeactivateSuppressed)
                    return false;

                return base.HideOnDeactivate;
            }

            set
            {
                base.HideOnDeactivate = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent <see cref="PopupToolBar"/> that contains this instance.
        /// </summary>
        [Browsable(false)]
        public PopupToolBar? ParentPopup { get; set; }

        /// <summary>
        /// Gets an enumerable collection of parent <see cref="PopupToolBar"/> instances,
        /// starting from the immediate parent and traversing up the hierarchy.
        /// </summary>
        /// <remarks>This property returns the sequence of parent popups
        /// in the hierarchy, with the
        /// immediate parent first, followed by its parent, and so on,
        /// until no more parents exist.</remarks>
        [Browsable(false)]
        public virtual IEnumerable<PopupToolBar> ParentPopups
        {
            get
            {
                var current = ParentPopup;
                while (current is not null)
                {
                    yield return current;
                    current = current.ParentPopup;
                }
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

        /// <inheritdoc/>
        public override bool CanShowPopup
        {
            get
            {
                return MainControl.HasChildren;
            }
        }

        AbstractControl IContextMenuHost.ContextMenuHost
        {
            get
            {
                return MainControl;
            }
        }

        /// <summary>
        /// Restores the default behavior of hiding <see cref="PopupToolBar"/> window on deactivation
        /// by decrementing the suppression.
        /// counter.
        /// </summary>
        /// <remarks>This method reduces the internal suppression counter by one.
        /// If the counter reaches zero, the application will resume its default behavior
        /// of hiding on deactivation. Ensure that calls to this
        /// method are balanced with calls that suppress the hide-on-deactivate behavior.</remarks>
        public static void RestoreHideOnDeactivate()
        {
            if (hideOnDeactivateSuppressCounter > 0)
                hideOnDeactivateSuppressCounter--;
        }

        /// <summary>
        /// Prevents the application from hiding <see cref="PopupToolBar"/> when it is deactivated.
        /// </summary>
        /// <remarks>This method increments an internal counter to suppress
        /// the default behavior of hiding
        /// the <see cref="PopupToolBar"/> window when the application or window loses focus.
        /// Call this method to ensure the window remains visible during
        /// deactivation. To restore the default behavior, a corresponding decrement method
        /// must be called.</remarks>
        public static void SuppressHideOnDeactivate()
        {
            hideOnDeactivateSuppressCounter++;
        }

        /// <summary>
        /// Determines whether the specified <see cref="PopupToolBar"/> is a parent
        /// of this popup. This method checks the hierarchy of parent popups using the
        /// <see cref="ParentPopups"/> property.
        /// </summary>
        /// <param name="popup">The <see cref="PopupToolBar"/> to check.</param>
        /// <returns><see langword="true"/> if the specified <see cref="PopupToolBar"/>
        /// is a parent popup; otherwise, <see
        /// langword="false"/>.</returns>
        public virtual bool IsParentPopup(PopupToolBar popup)
        {
            return ParentPopups.Contains(popup);
        }

        /// <summary>
        /// Hides the current popup without affecting the visibility of other popups.
        /// </summary>
        /// <remarks>This method ensures that only the current popup is hidden,
        /// while suppressing any side
        /// effects that might otherwise hide other popups. It restores
        /// the original state after the operation
        /// completes.If the popup is already not visible, the method does nothing.</remarks>
        public virtual void HideOnlyThisPopup()
        {
            if (!Visible)
                return;
            SuppressHideOnDeactivate();
            hideOtherPopupsSuppressed = true;
            try
            {
                Hide();
            }
            finally
            {
                hideOtherPopupsSuppressed = false;
                RestoreHideOnDeactivate();
            }
        }

        /// <summary>
        /// Hides all visible popup toolbars that appear after the current instance in the sequence.
        /// </summary>
        /// <remarks>This method identifies the current instance within the collection of visible popup
        /// toolbars and hides all subsequent toolbars in the sequence. If the current instance is not found in the
        /// collection, the method performs no action.</remarks>
        public virtual void HideAllAfterThis()
        {
            var popups = VisiblePopupToolbars.ToArray();
            var idx = Array.IndexOf(popups, this);
            if (idx < 0)
                return;
            for (int i = idx + 1; i < popups.Length; i++)
            {
                popups[i].HideOnlyThisPopup();
            }
        }

        /// <inheritdoc/>
        public override SizeD GetPreferredSize(PreferredSizeContext context)
        {
            var preferredSize = MainControl.GetPreferredSize(SizeD.MaxCoord);

            var lastChild = MainControl.GetVisibleChildWithMaxBottom();
            if (lastChild is not null)
            {
                preferredSize.Height = Math.Max(
                    preferredSize.Height,
                    lastChild.Bounds.Bottom + lastChild.Margin.Bottom);
            }

            preferredSize += GetInteriorSize() + 2;
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
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
                return;

            HideAllAfterThis();

            if (!hideOtherPopupsSuppressed)
            {
                HideOtherPopups();
            }
        }

        /// <summary>
        /// Hides all other visible pop-up toolbars except the current one.
        /// </summary>
        /// <remarks>This method iterates through all visible windows in the
        /// application and hides any
        /// window that is a <see cref="PopupToolBar"/> instance,
        /// excluding the current window. The pop-ups are hidden
        /// with a modal result of <see cref="ModalResult.Canceled"/>.</remarks>
        protected virtual void HideOtherPopups()
        {
            var windows = App.Current?.VisibleWindows.ToArray() ?? Array.Empty<Window>();
            foreach (var window in windows)
            {
                if (window != this && window is PopupToolBar popup)
                {
                    Post(() =>
                    {
                        popup.HidePopup(ModalResult.Canceled);
                    });
                }
            }
        }

        /// <inheritdoc/>
        protected override void BeforeShowPopup()
        {
            MainControl.DoInsideLayout(() =>
            {
                if (NeedsRemainingImages)
                {
                    MainControl.AddRemainingImages();
                }

                if (NeedsRemainingLabelImages)
                {
                    MainControl.AddRemainingLabelImages();
                }

                if (IsLabelTextWidthMaximized)
                    MainControl.MaximizeLabelTextWidth();

                if (IsRightSideElementWidthMaximized)
                    MainControl.MaximizeToolRightSideElementWidth();
            });

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