using System;
using System.Collections.Generic;
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
        /// Gets or sets the thickness of the resizable border.
        /// </summary>
        public static float DefaultBorderThickness = 3f;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResizableBorder"/> class.
        /// </summary>
        public ResizableBorder()
        {
        }

        /// <summary>
        /// Gets right grip control which is used on the right side of the resizable border.
        /// </summary>
        public GripControl RightGripControl => (GripControl)RightPanel;

        /// <summary>
        /// Gets left grip control which is used on the left side of the resizable border.
        /// </summary>
        public GripControl LeftGripControl => (GripControl)LeftPanel;

        /// <summary>
        /// Gets top grip control which is used on the top side of the resizable border.
        /// </summary>
        public GripControl TopGripControl => (GripControl)TopPanel;
        
        /// <summary>
        /// Gets bottom grip control which is used on the bottom side of the resizable border.
        /// </summary>
        public GripControl BottomGripControl => (GripControl)BottomPanel;

        /// <inheritdoc/>
        public override Thickness DefaultPanelSize
        {
            get
            {
                return DefaultBorderThickness;
            }
        }

        /// <summary>
        /// Updates the colors of the grip control to match the current theme.
        /// </summary>
        /// <param name="grip">The grip control to update.</param>
        /// <param name="isDark">Indicates whether the dark theme is applied.</param>
        /// <param name="isActive">Indicates whether the window is active.</param>
        public virtual void UseWindowBorderColors(GripControl grip, bool isDark, bool isActive)
        {
            grip.ParentBackColor = false;
            grip.BackColor = DefaultColors.GetEffectiveWindowBorderColor(isDark, isActive);
        }

        /// <summary>
        /// Updates the colors of the grip controls based on the specified parameters.
        /// </summary>
        /// <param name="isDark">Indicates whether the dark theme is applied.</param>
        /// <param name="isActive">Indicates whether the window is active.</param>
        public virtual void UseWindowBorderColors(bool isDark, bool isActive)
        {
            UseWindowBorderColors(TopGripControl, isDark, isActive);
            UseWindowBorderColors(BottomGripControl, isDark, isActive);
            UseWindowBorderColors(LeftGripControl, isDark, isActive);
            UseWindowBorderColors(RightGripControl, isDark, isActive);
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateRightPanel()
        {
            GripControl result = new ();
            result.SuggestedSize = AbstractControl.DefaultControlSuggestedSize;
            result.ImageKind = GripControl.GripImageKind.None;
            result.Dock = DockStyle.Right;
            result.Cursor = Cursors.SizeWE;
            result.SizeAction = GripControl.GripSizeAction.ChangeWidth;
            result.Target = this;
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateLeftPanel()
        {
            GripControl result = new();
            result.SuggestedSize = AbstractControl.DefaultControlSuggestedSize;
            result.Dock = DockStyle.Left;
            result.ImageKind = GripControl.GripImageKind.None;
            result.Cursor = Cursors.SizeWE;
            result.SizeAction = GripControl.GripSizeAction.ChangeWidth;
            result.InvertLeftDelta = true;
            result.InvertWidthDelta = true;
            result.MoveAction = GripControl.GripMoveAction.ChangeLeft;
            result.Target = this;
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateTopPanel()
        {
            GripControl result = new();
            result.SuggestedSize = AbstractControl.DefaultControlSuggestedSize;
            result.Dock = DockStyle.Top;
            result.ImageKind = GripControl.GripImageKind.None;
            result.InvertTopDelta = true;
            result.InvertHeightDelta = true;
            result.MoveAction = GripControl.GripMoveAction.ChangeTop;
            result.Cursor = Cursors.SizeNS;
            result.Target = this;
            return result;
        }

        /// <inheritdoc/>
        protected override AbstractControl CreateBottomPanel()
        {
            GripControl result = new();
            result.SuggestedSize = AbstractControl.DefaultControlSuggestedSize;
            result.ImageKind = GripControl.GripImageKind.None;
            result.Dock = DockStyle.Bottom;
            result.Cursor = Cursors.SizeNS;
            result.SizeAction = GripControl.GripSizeAction.ChangeHeight;
            result.Target = this;
            return result;
        }
    }
}
