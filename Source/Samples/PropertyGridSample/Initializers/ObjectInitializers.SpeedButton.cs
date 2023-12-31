using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.UI;
using Alternet.Drawing;

namespace PropertyGridSample
{
    internal partial class ObjectInitializers
    {
        public static void InitSpeedButton(object control)
        {
            if (control is not SpeedButton button)
                return;
            button.Text = "speedButton";
            button.Image = KnownSvgImages.GetForSize(SystemColors.ControlText, 32).ImgOk.AsImage(32);
        }
    }
}
