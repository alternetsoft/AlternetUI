using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a resizable border control.
    /// </summary>
    public partial class ResizableBorder : DockedSubPanelContainer
    {
        /// <summary>
        /// Gets or sets the minimum thickness of the resizable border.
        /// </summary>
        public static float MinBorderThickness = 3f;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableBorder"/> class.
        /// </summary>
        public ResizableBorder()
        {
        }

        /// <summary>
        /// Gets right grip control which is used on the right side of the resizable border.
        /// </summary>
        [Browsable(false)]
        public GripControl RightGripControl => (GripControl)RightPanel;

        /// <summary>
        /// Gets left grip control which is used on the left side of the resizable border.
        /// </summary>
        [Browsable(false)]
        public GripControl LeftGripControl => (GripControl)LeftPanel;

        /// <summary>
        /// Gets top grip control which is used on the top side of the resizable border.
        /// </summary>
        [Browsable(false)]
        public GripControl TopGripControl => (GripControl)TopPanel;

        /// <summary>
        /// Gets bottom grip control which is used on the bottom side of the resizable border.
        /// </summary>
        [Browsable(false)]
        public GripControl BottomGripControl => (GripControl)BottomPanel;

        /// <inheritdoc/>
        public override Thickness DefaultPanelSize
        {
            get
            {
                return MinBorderThickness;
            }
        }

        /// <summary>
        /// Gets width and height of the interior borders.
        /// This is the size of side panels which are used to resize the control.
        /// </summary>
        public virtual SizeD InteriorBorderSize
        {
            get
            {
                var leftWidth = LeftPanel.Visible ? LeftPanel.Size.Width : 0;
                var rightWidth = RightPanel.Visible ? RightPanel.Size.Width : 0;
                var topHeight = TopPanel.Visible ? TopPanel.Size.Height : 0;
                var bottomHeight = BottomPanel.Visible ? BottomPanel.Size.Height : 0;

                return new SizeD(leftWidth + rightWidth, topHeight + bottomHeight);
            }
        }

        /// <summary>
        /// Sets target control for the resizable border.
        /// This control will be resized when the user drags the grips of the resizable border.
        /// </summary>
        /// <param name="target">The target control to be resized.</param>
        public virtual void SetResizeTarget(AbstractControl target)
        {
            RightGripControl.Target = target;
            LeftGripControl.Target = target;
            TopGripControl.Target = target;
            BottomGripControl.Target = target;
        }

        /// <summary>
        /// Updates width and height of the resizable borders.
        /// </summary>
        /// <param name="kind">The kind of border metric to use.</param>
        /// <returns>The current instance of <see cref="ResizableBorder"/> to allow method chaining.</returns>
        public virtual ResizableBorder UseWindowBorderSize(Window.FrameMetrics.BorderMetricKind kind)
        {
            var size = Window.FrameMetrics.GetBorderSize(kind, this);
            RightPanel.Width = MathF.Max(size.Width, MinBorderThickness);
            LeftPanel.Width = MathF.Max(size.Width, MinBorderThickness);
            TopPanel.Height = MathF.Max(size.Height, MinBorderThickness);
            BottomPanel.Height = MathF.Max(size.Height, MinBorderThickness);
            return this;
        }

        /// <summary>
        /// Updates the colors of the grip controls based on the specified parameters.
        /// </summary>
        /// <param name="isDark">Indicates whether the dark theme is applied.</param>
        /// <param name="isActive">Indicates whether the window is active.</param>
        /// <returns>The current instance of <see cref="ResizableBorder"/> to allow method chaining.</returns>
        public virtual ResizableBorder UseWindowBorderColors(bool isDark, bool isActive)
        {
            TopGripControl.UseWindowBorderColors(isDark, isActive);
            BottomGripControl.UseWindowBorderColors(isDark, isActive);
            LeftGripControl.UseWindowBorderColors(isDark, isActive);
            RightGripControl.UseWindowBorderColors(isDark, isActive);
            return this;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="GripControl"/> class which is used for the resizable border.
        /// </summary>
        /// <returns>A new instance of <see cref="GripControl"/>.</returns>
        protected virtual GripControl CreateGripControl()
        {
            return new GripControl();
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateRightPanel()
        {
            GripControl result = CreateGripControl().ConfigureAsRightBorder().SetTarget(this);
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateLeftPanel()
        {
            GripControl result = CreateGripControl().ConfigureAsLeftBorder().SetTarget(this);
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateTopPanel()
        {
            GripControl result = CreateGripControl().ConfigureAsTopBorder().SetTarget(this);
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateBottomPanel()
        {
            GripControl result = CreateGripControl().ConfigureAsBottomBorder().SetTarget(this);
            return result;
        }
    }
}
