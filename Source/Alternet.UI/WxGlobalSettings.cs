using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains static properties which allow to customize WxWidgets behavior.
    /// </summary>
    public static class WxGlobalSettings
    {
        /// <summary>
        /// Gets or sets whether to disable native point and size transformations
        /// inside the library. When <see cref="InternalGraphicsTransform"/> is True,
        /// Graphics.Transform is processed inside the library. By default it is False on Windows
        /// and True on macOs and Linux.
        /// </summary>
        public static bool InternalGraphicsTransform;

        static WxGlobalSettings()
        {
            InternalGraphicsTransform = App.IsMacOS;
        }

        /// <summary>
        /// Contains macOs related settings.
        /// </summary>
        public static class MacOs
        {
            /// <summary>
            /// Gets or sets whether <see cref="ColorDialog"/> show method
            /// returns OK if color is changed. On macOs color dialog has no
            /// OK and CANCEL buttons, so the only way is to check for color change.
            /// </summary>
            public static bool ColorDialogAcceptIfChanged = true;
        }

        /// <summary>
        /// Contains MSW related settings.
        /// </summary>
        public static class Msw
        {

        }

        /// <summary>
        /// Contains Linux related settings.
        /// </summary>
        public static class Linux
        {
            /// <summary>
            /// Indicates whether GTK CSS should be loaded automatically at application startup.
            /// </summary>
            /// <remarks>
            /// If set to <c>true</c>, the system will attempt to read and apply a stylesheet from
            /// a file named &lt;ApplicationFileName&gt;.gtk.css located in the application directory.
            /// <para><b>Note:</b> This property should be set <i>before</i> the application
            /// is created to have any effect.</para>
            /// </remarks>
            public static bool LoadGtkCss = true;

            /// <summary>
            /// Indicates whether custom GTK CSS should be injected during application startup.
            /// </summary>
            /// <remarks>
            /// <para>
            /// If set to <c>true</c>, the value of <see cref="GtkCss"/> will be applied
            /// to relevant GTK widgets using a <c>GtkCssProvider</c>.
            /// This allows for dynamic theming or visual customization of scrollbars,
            /// backgrounds, or other interface elements.
            /// </para>
            /// <para>
            /// <b>Important:</b> This must be set <i>before</i> the application is created
            /// to have any effect.
            /// </para>
            /// </remarks>
            public static bool InjectGtkCss = true;

            /// <summary>
            /// Contains the GTK CSS stylesheet to be injected, if <see cref="InjectGtkCss"/> is enabled.
            /// </summary>
            /// <remarks>
            /// <para>
            /// The stylesheet should follow GTK's CSS syntax, targeting elements
            /// like <c>scrollbar</c>, <c>button</c>, or <c>window</c>.
            /// Can be modified at runtime to enable theme switching or fine-grained styling control.
            /// </para>
            /// <para>
            /// <b>Important:</b> This must be set <i>before</i> the application is created
            /// to have any effect.
            /// </para>
            /// </remarks>
            public static string? GtkCss;
        }
    }
}
