using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /*

    WindowFrameMetrics

    SystemSettings.GetMetric(SystemSettingsMetric.)

    Related SystemSettingsMetric:

        /// <summary>
        /// Width of single border.
        /// </summary>
        BorderX = 2,

        /// <summary>
        /// Height of single border.
        /// </summary>
        BorderY = 3,
    
        /// <summary>
        /// Width of a 3D border.
        /// </summary>
        EdgeX = 10,

        /// <summary>
        /// Height of a 3D border.
        /// </summary>
        EdgeY = 11,

        /// <summary>
        /// The default width of an icon.
        /// </summary>
        IconX = 15,

        /// <summary>
        /// The default height of an icon.
        /// </summary>
        IconY = 16,

        /// <summary>
        /// Width of the window frame for a THICK FRAME window.
        /// </summary>
        FrameSizeX = 23,

        /// <summary>
        /// Height of the window frame for a THICK FRAME window.
        /// </summary>
        FrameSizeY = 24,

        /// <summary>
        /// Recommended width of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconX = 25,

        /// <summary>
        /// Recommended height of a small icon (in window captions, and small icon view).
        /// </summary>
        SmallIconY = 26,

        /// <summary>
        /// Height of normal caption area.
        /// </summary>
        CaptionY = 32,    
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
