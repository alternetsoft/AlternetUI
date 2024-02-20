using System;

namespace Alternet.UI
{
    /// <summary>
    /// These are the possible <see cref="IAuiDockArt"/> metric settings.
    /// </summary>
    internal enum AuiDockArtSetting
    {
        /// <summary>
        /// Customizes the sash size.
        /// </summary>
        SashSize = 0,

        /// <summary>
        /// Customizes the caption size.
        /// </summary>
        CaptionSize = 1,

        /// <summary>
        /// Customizes the gripper size.
        /// </summary>
        GripperSize = 2,

        /// <summary>
        /// Customizes the pane border size.
        /// </summary>
        PaneBorderSize = 3,

        /// <summary>
        /// Customizes the pane button size.
        /// </summary>
        PaneButtonSize = 4,

        /// <summary>
        /// Customizes the background color, which corresponds to the client area.
        /// </summary>
        BackgroundColor = 5,

        /// <summary>
        /// Customizes the sash color.
        /// </summary>
        SashColor = 6,

        /// <summary>
        /// Customizes the active caption color.
        /// </summary>
        ActiveCaptionColor = 7,

        /// <summary>
        /// Customizes the active caption gradient color.
        /// </summary>
        ActiveCaptionGradientColor = 8,

        /// <summary>
        /// Customizes the inactive caption color.
        /// </summary>
        InactiveCaptionColor = 9,

        /// <summary>
        /// Customizes the inactive gradient caption color.
        /// </summary>
        InactiveCaptionGradientColor = 10,

        /// <summary>
        /// Customizes the active caption text color.
        /// </summary>
        ActiveCaptionTextColor = 11,

        /// <summary>
        /// Customizes the inactive caption text color.
        /// </summary>
        InactiveCaptionTextColor = 12,

        /// <summary>
        /// Customizes the border color.
        /// </summary>
        BorderColor = 13,

        /// <summary>
        /// Customizes the gripper color.
        /// </summary>
        GripperColor = 14,

        /// <summary>
        /// Customizes the caption font.
        /// </summary>
        CaptionFont = 15,

        /// <summary>
        /// Customizes the gradient type (no gradient, vertical or horizontal).
        /// </summary>
        GradientType = 16,
    }
}