using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains <see cref="Color"/> related static methods.
    /// </summary>
    public static class ColorUtils
    {
        internal static WxSystemColour Convert(KnownColor color)
        {
            return color switch
            {
                KnownColor.ActiveBorder => (WxSystemColour)0x0A,
                KnownColor.ActiveCaption => (WxSystemColour)0x02,
                KnownColor.ActiveCaptionText => (WxSystemColour)0x09,
                KnownColor.AppWorkspace => (WxSystemColour)0x0C,
                KnownColor.ButtonFace => (WxSystemColour)0x0F,
                KnownColor.ButtonHighlight => (WxSystemColour)0x14,
                KnownColor.ButtonShadow => (WxSystemColour)0x10,
                KnownColor.Control => (WxSystemColour)0x0F,
                KnownColor.ControlDark => (WxSystemColour)0x10,
                KnownColor.ControlDarkDark => (WxSystemColour)0x15,
                KnownColor.ControlLight => (WxSystemColour)0x16,
                KnownColor.ControlLightLight => (WxSystemColour)0x14,
                KnownColor.ControlText => (WxSystemColour)0x12,
                KnownColor.Desktop => (WxSystemColour)0x01,
                KnownColor.GradientActiveCaption => (WxSystemColour)0x1B,
                KnownColor.GradientInactiveCaption => (WxSystemColour)0x1C,
                KnownColor.GrayText => (WxSystemColour)0x11,
                KnownColor.Highlight => (WxSystemColour)0x0D,
                KnownColor.HighlightText => (WxSystemColour)0x0E,
                KnownColor.HotTrack => (WxSystemColour)0x1A,
                KnownColor.InactiveBorder => (WxSystemColour)0x0B,
                KnownColor.InactiveCaption => (WxSystemColour)0x03,
                KnownColor.InactiveCaptionText => (WxSystemColour)0x13,
                KnownColor.Info => (WxSystemColour)0x18,
                KnownColor.InfoText => (WxSystemColour)0x17,
                KnownColor.Menu => (WxSystemColour)0x04,
                KnownColor.MenuBar => (WxSystemColour)0x1E,
                KnownColor.MenuHighlight => (WxSystemColour)0x1D,
                KnownColor.MenuText => (WxSystemColour)0x07,
                KnownColor.ScrollBar => (WxSystemColour)0x00,
                KnownColor.Window => (WxSystemColour)0x05,
                KnownColor.WindowFrame => (WxSystemColour)0x06,
                KnownColor.WindowText => (WxSystemColour)0x08,
                _ => WxSystemColour.SYS_COLOUR_MAX,
            };
        }
    }
}