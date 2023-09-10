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
            switch (color)
            {
                case KnownColor.ActiveBorder:
                    return (WxSystemColour)0x0A;
                case KnownColor.ActiveCaption:
                    return (WxSystemColour)0x02;
                case KnownColor.ActiveCaptionText:
                    return (WxSystemColour)0x09;
                case KnownColor.AppWorkspace:
                    return (WxSystemColour)0x0C;
                case KnownColor.ButtonFace:
                    return (WxSystemColour)0x0F;
                case KnownColor.ButtonHighlight:
                    return (WxSystemColour)0x14;
                case KnownColor.ButtonShadow:
                    return (WxSystemColour)0x10;
                case KnownColor.Control:
                    return (WxSystemColour)0x0F;
                case KnownColor.ControlDark:
                    return (WxSystemColour)0x10;
                case KnownColor.ControlDarkDark:
                    return (WxSystemColour)0x15;
                case KnownColor.ControlLight:
                    return (WxSystemColour)0x16;
                case KnownColor.ControlLightLight:
                    return (WxSystemColour)0x14;
                case KnownColor.ControlText:
                    return (WxSystemColour)0x12;
                case KnownColor.Desktop:
                    return (WxSystemColour)0x01;
                case KnownColor.GradientActiveCaption:
                    return (WxSystemColour)0x1B;
                case KnownColor.GradientInactiveCaption:
                    return (WxSystemColour)0x1C;
                case KnownColor.GrayText:
                    return (WxSystemColour)0x11;
                case KnownColor.Highlight:
                    return (WxSystemColour)0x0D;
                case KnownColor.HighlightText:
                    return (WxSystemColour)0x0E;
                case KnownColor.HotTrack:
                    return (WxSystemColour)0x1A;
                case KnownColor.InactiveBorder:
                    return (WxSystemColour)0x0B;
                case KnownColor.InactiveCaption:
                    return (WxSystemColour)0x03;
                case KnownColor.InactiveCaptionText:
                    return (WxSystemColour)0x13;
                case KnownColor.Info:
                    return (WxSystemColour)0x18;
                case KnownColor.InfoText:
                    return (WxSystemColour)0x17;
                case KnownColor.Menu:
                    return (WxSystemColour)0x04;
                case KnownColor.MenuBar:
                    return (WxSystemColour)0x1E;
                case KnownColor.MenuHighlight:
                    return (WxSystemColour)0x1D;
                case KnownColor.MenuText:
                    return (WxSystemColour)0x07;
                case KnownColor.ScrollBar:
                    return (WxSystemColour)0x00;
                case KnownColor.Window:
                    return (WxSystemColour)0x05;
                case KnownColor.WindowFrame:
                    return (WxSystemColour)0x06;
                case KnownColor.WindowText:
                    return (WxSystemColour)0x08;
                default:
                    return WxSystemColour.SYS_COLOUR_MAX;
            }
        }
    }
}