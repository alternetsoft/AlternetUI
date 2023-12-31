using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Implements speed button control.
    /// </summary>
    public class SpeedButton : PictureBox
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpeedButton"/> class.
        /// </summary>
        public SpeedButton()
        {
            AcceptsFocusAll = false;
            ImageStretch = false;
            Borders ??= new();

            var border = BorderSettings.Default.Clone();
            border.UniformCornerRadius = 50;
            border.UniformRadiusIsPercent = true;
            Borders.SetObject(border, GenericControlState.Hovered);
            Borders.SetObject(border, GenericControlState.Pressed);
        }
    }
}
