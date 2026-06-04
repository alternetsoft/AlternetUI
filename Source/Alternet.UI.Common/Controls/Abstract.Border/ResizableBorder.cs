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
        /// Updates the colors of the grip controls based on the specified parameters.
        /// </summary>
        /// <param name="isDark">Indicates whether the dark theme is applied.</param>
        /// <param name="isActive">Indicates whether the window is active.</param>
        public virtual void UseWindowBorderColors(bool isDark, bool isActive)
        {
            TopGripControl.UseWindowBorderColors(isDark, isActive);
            BottomGripControl.UseWindowBorderColors(isDark, isActive);
            LeftGripControl.UseWindowBorderColors(isDark, isActive);
            RightGripControl.UseWindowBorderColors(isDark, isActive);
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
