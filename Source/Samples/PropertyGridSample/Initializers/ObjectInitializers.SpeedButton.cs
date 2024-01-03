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

            var images = KnownSvgImages.GetForSize(
                button.GetSvgColor(KnownSvgColor.Normal),
                32);
            var imagesDisabled = KnownSvgImages.GetForSize(
                button.GetSvgColor(KnownSvgColor.Disabled),
                32);

            button.Image = images.ImgOk.AsImage(32);
            button.DisabledImage = imagesDisabled.ImgOk.AsImage(32);
        }
    }
}
