using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements generic control which is handled inside the library.
    /// Generic control doesn't have native handle.
    /// </summary>
    public partial class GenericControl : AbstractControl
    {
        private bool isClipped = false;
        private bool showDropDownMenuWhenClicked;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericControl"/> class.
        /// </summary>
        public GenericControl()
        {
            UserPaint = true;
        }

        /// <summary>
        /// Gets or sets different behavior and visualization options.
        /// </summary>
        [Browsable(false)]
        public virtual ControlRefreshOptions RefreshOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drop down menu
        /// is shown when the control is clicked. Default is <see langword="true"/>.
        /// </summary>
        [Browsable(true)]
        public virtual bool ShowDropDownMenuWhenClicked
        {
            get => showDropDownMenuWhenClicked;

            set
            {
                showDropDownMenuWhenClicked = value;
            }
        }

        /// <inheritdoc/>
        public override bool IsHandleCreated => true;

        /// <summary>
        /// Gets the first parent control in the parent chain which
        /// is not <see cref="GenericControl"/>.
        /// </summary>
        [Browsable(false)]
        public AbstractControl? NonGenericParent
        {
            get
            {
                var result = Parent;

                while (result is not null && result is GenericControl)
                {
                    result = result.Parent;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the first parent control in the parent chain which
        /// has attached native control.
        /// </summary>
        [Browsable(false)]
        public Control? ParentWithNativeControl
        {
            get
            {
                var result = Parent;

                while (result is not null && result is not Control)
                {
                    result = result.Parent;
                }

                return result as Control;
            }
        }

        /// <inheritdoc/>
        public override Cursor? Cursor
        {
            get
            {
                return base.Cursor;
            }

            set
            {
                if (Cursor == value)
                    return;
                base.Cursor = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ContextMenu"/> which is shown when control is clicked
        /// with left mouse button. Do not mix this with <see cref="ContextMenu"/> which is
        /// shown when right mouse button is clicked.
        /// </summary>
        [Browsable(false)]
        public virtual ContextMenu? DropDownMenu { get; set; }

        /// <summary>
        /// Gets or sets whether control contents is clipped and is not painted outside it's bounds.
        /// </summary>
        [Browsable(true)]
        public virtual bool IsClipped
        {
            get
            {
                return isClipped;
            }

            set
            {
                if (isClipped == value)
                    return;
                isClipped = value;
                Refresh();
            }
        }

        /// <summary>
        /// Invalidates the control and causes a paint message to be sent to
        /// the control.
        /// </summary>
        public override void Invalidate()
        {
            if (!VisibleOnScreen)
                return;

            var result = Parent;
            var bounds = Bounds;

            while (true)
            {
                if (result is null)
                    return;
                if (result is Control)
                    break;

                bounds.Location += result.Location;
                result = result.Parent;
            }

            result.Invalidate(bounds);
        }

        /// <summary>
        /// Causes the control to redraw the invalidated regions.
        /// </summary>
        public override void Update()
        {
            Invalidate();
        }

        /// <summary>
        /// Default painting method of the <see cref="GenericControl"/>
        /// and its descendants.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        public virtual void DefaultPaint(PaintEventArgs e)
        {
        }

        /// <inheritdoc/>
        public override BorderSettings? GetBorderSettings(VisualControlState state)
        {
            return UserControl.HandleGetBorderSettings(this, state);
        }

        /// <inheritdoc/>
        public override PaintEventHandler? GetBackgroundAction(VisualControlState state)
        {
            return UserControl.HandleGetBackgroundActions(this, state);
        }

        /// <inheritdoc/>
        public override Brush? GetBackground(VisualControlState state)
        {
            return UserControl.HandleGetBackground(this, state);
        }

        /// <summary>
        /// Shows attached drop down menu.
        /// </summary>
        protected virtual void ShowDropDownMenu()
        {
            DropDownMenu?.ShowAsDropDown(this);
        }

        /// <inheritdoc/>
        protected override void OnMouseLeftButtonDown(MouseEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!Enabled)
                return;
            RaiseClick(e);
            if (ShowDropDownMenuWhenClicked)
                ShowDropDownMenu();
            Invalidate();
        }

        /// <inheritdoc/>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnVisualStateChanged(EventArgs e)
        {
            base.OnVisualStateChanged(e);
            UserControl.HandleOnVisualStateChanged(this, RefreshOptions);
        }

        /// <inheritdoc/>
        protected sealed override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DoInsideClipped(
                e.ClipRectangle,
                () =>
                {
                    DefaultPaint(e);
                },
                IsClipped);
        }
    }
}
