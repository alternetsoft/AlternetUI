using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alternet.Winforms
{
    /// <summary>
    /// Provides utility methods for working with WinForms.
    /// </summary>
    public static class WinformsUtils
    {
        private static bool handlersInitialized;

        static WinformsUtils()
        {
            InitHandlers();
        }

        /// <summary>
        /// Gets the main form of the application, if available.
        /// </summary>
        public static Form? MainForm => Application.OpenForms.Count > 0 ? Application.OpenForms[0] : null;

        /// <summary>
        /// Initializes the system settings handler for WinForms applications.
        /// </summary>
        public static void InitHandlers()
        {
            if (!handlersInitialized)
            {
                handlersInitialized = true;
                Alternet.UI.SystemSettings.Handler = new SystemSettingsHandlerWinforms();
            }
        }

        /// <summary>
        /// Creates a new <see cref="Alternet.Drawing.ColorStruct"/> from the specified <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="color">The <see cref="System.Drawing.Color"/> to convert.</param>
        /// <returns>A new <see cref="Alternet.Drawing.ColorStruct"/> representing the specified color.</returns>
        public static Alternet.Drawing.ColorStruct CreateColorStruct(System.Drawing.Color color)
        {
            return new Alternet.Drawing.ColorStruct(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Gets the corresponding <see cref="System.Drawing.Color"/>
        /// for the specified <see cref="Alternet.Drawing.KnownSystemColor"/>.
        /// </summary>
        /// <param name="index">The known system color.</param>
        /// <returns>The corresponding <see cref="System.Drawing.Color"/> if available; otherwise, <c>null</c>.</returns>
        public static System.Drawing.Color? GetColor(Alternet.Drawing.KnownSystemColor index)
        {
            switch (index)
            {
                case Alternet.Drawing.KnownSystemColor.ActiveBorder:
                    return System.Drawing.SystemColors.ActiveBorder;
                case Alternet.Drawing.KnownSystemColor.ActiveCaption:
                    return System.Drawing.SystemColors.ActiveCaption;
                case Alternet.Drawing.KnownSystemColor.ActiveCaptionText:
                    return System.Drawing.SystemColors.ActiveCaptionText;
                case Alternet.Drawing.KnownSystemColor.AppWorkspace:
                    return System.Drawing.SystemColors.AppWorkspace;
                case Alternet.Drawing.KnownSystemColor.Control:
                    return System.Drawing.SystemColors.Control;
                case Alternet.Drawing.KnownSystemColor.ControlDark:
                    return System.Drawing.SystemColors.ControlDark;
                case Alternet.Drawing.KnownSystemColor.ControlDarkDark:
                    return System.Drawing.SystemColors.ControlDarkDark;
                case Alternet.Drawing.KnownSystemColor.ControlLight:
                    return System.Drawing.SystemColors.ControlLight;
                case Alternet.Drawing.KnownSystemColor.ControlLightLight:
                    return System.Drawing.SystemColors.ControlLightLight;
                case Alternet.Drawing.KnownSystemColor.ControlText:
                    return System.Drawing.SystemColors.ControlText;
                case Alternet.Drawing.KnownSystemColor.Desktop:
                    return System.Drawing.SystemColors.Desktop;
                case Alternet.Drawing.KnownSystemColor.GrayText:
                    return System.Drawing.SystemColors.GrayText;
                case Alternet.Drawing.KnownSystemColor.Highlight:
                    return System.Drawing.SystemColors.Highlight;
                case Alternet.Drawing.KnownSystemColor.HighlightText:
                    return System.Drawing.SystemColors.HighlightText;
                case Alternet.Drawing.KnownSystemColor.HotTrack:
                    return System.Drawing.SystemColors.HotTrack;
                case Alternet.Drawing.KnownSystemColor.InactiveBorder:
                    return System.Drawing.SystemColors.InactiveBorder;
                case Alternet.Drawing.KnownSystemColor.InactiveCaption:
                    return System.Drawing.SystemColors.InactiveCaption;
                case Alternet.Drawing.KnownSystemColor.InactiveCaptionText:
                    return System.Drawing.SystemColors.InactiveCaptionText;
                case Alternet.Drawing.KnownSystemColor.Info:
                    return System.Drawing.SystemColors.Info;
                case Alternet.Drawing.KnownSystemColor.InfoText:
                    return System.Drawing.SystemColors.InfoText;
                case Alternet.Drawing.KnownSystemColor.Menu:
                    return System.Drawing.SystemColors.Menu;
                case Alternet.Drawing.KnownSystemColor.MenuText:
                    return System.Drawing.SystemColors.MenuText;
                case Alternet.Drawing.KnownSystemColor.ScrollBar:
                    return System.Drawing.SystemColors.ScrollBar;
                case Alternet.Drawing.KnownSystemColor.Window:
                    return System.Drawing.SystemColors.Window;
                case Alternet.Drawing.KnownSystemColor.WindowFrame:
                    return System.Drawing.SystemColors.WindowFrame;
                case Alternet.Drawing.KnownSystemColor.WindowText:
                    return System.Drawing.SystemColors.WindowText;
                case Alternet.Drawing.KnownSystemColor.ButtonFace:
                    return System.Drawing.SystemColors.ButtonFace;
                case Alternet.Drawing.KnownSystemColor.ButtonHighlight:
                    return System.Drawing.SystemColors.ButtonHighlight;
                case Alternet.Drawing.KnownSystemColor.ButtonShadow:
                    return System.Drawing.SystemColors.ButtonShadow;
                case Alternet.Drawing.KnownSystemColor.GradientActiveCaption:
                    return System.Drawing.SystemColors.GradientActiveCaption;
                case Alternet.Drawing.KnownSystemColor.GradientInactiveCaption:
                    return System.Drawing.SystemColors.GradientInactiveCaption;
                case Alternet.Drawing.KnownSystemColor.MenuBar:
                    return System.Drawing.SystemColors.MenuBar;
                case Alternet.Drawing.KnownSystemColor.MenuHighlight:
                    return System.Drawing.SystemColors.MenuHighlight;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the scale factor of the specified WinForms control.
        /// </summary>
        /// <param name="control">The WinForms control.</param>
        /// <returns>The scale factor of the control.</returns>
        public static float GetScaleFactor(Control? control)
        {
            control ??= MainForm;

            if (control == null)
                return 1f;

#if NET471_OR_GREATER || NET8_0_OR_GREATER
            float dpi = control.DeviceDpi;
            return dpi / 96f;
#else
            using var g = control.CreateGraphics();
            return g.DpiX / 96f;
#endif
        }
    }
}
