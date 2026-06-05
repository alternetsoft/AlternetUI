using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /*

    WindowFrameMetrics

    SystemSettings.GetMetric(SystemSettingsMetric.)
        /// The default width of an icon.
        IconX = 15,

        /// The default height of an icon.
        IconY = 16,

        /// Recommended width of a small icon (in window captions, and small icon view).
        SmallIconX = 25,

        /// Recommended height of a small icon (in window captions, and small icon view).
        SmallIconY = 26,

    SystemSettings.GetMetric(SystemSettingsMetric.)
    Related SystemSettingsMetric:

        /// Width of single border.
        BorderX

        /// Height of single border.
        BorderY
    
        /// Width of a 3D border.
        EdgeX

        /// Height of a 3D border.
        EdgeY

        /// Width of the window frame for a THICK FRAME window.
        FrameSizeX

        /// Height of the window frame for a THICK FRAME window.
        FrameSizeY

        /// Height of normal caption area.
        CaptionY
    */

    /// <summary>
    /// Represents a control that can be used as a window title bar.
    /// </summary>
    public partial class TitleBarControl : ToolBar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TitleBarControl"/> class.
        /// </summary>
        public TitleBarControl()
        {
        }
    }
}
