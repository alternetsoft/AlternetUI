using System;
using System.Collections.Generic;
using System.Text;

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

        /// <inheritdoc/>
        public override Thickness DefaultPanelSize
        {
            get
            {
                return DefaultBorderThickness;
            }
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
