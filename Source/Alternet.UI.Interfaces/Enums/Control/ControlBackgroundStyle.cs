using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates control background painting styles.
    /// </summary>
    public enum ControlBackgroundStyle
    {
        /// <summary>
        /// Default background style value indicating that the background may be erased in
        /// the user-defined 'Erase Background' event handler.
        /// </summary>
        /// <remarks>
        /// If no such event handler is defined (or if it skips the event), the effect of this
        /// style is the same as <see cref="System"/>. If an empty handler (not skipping the event)
        /// is defined, the effect is the same as <see cref="Paint"/>, i.e. the background is not
        /// erased at all until 'Paint' event handler is executed.
        /// </remarks>
        /// <remarks>
        /// This is the only background style value for which erase background events are
        /// generated at all.
        /// </remarks>
        Erase,

        /// <summary>
        /// Use the default background, as determined by the system or the current theme.
        /// </summary>
        /// <remarks>
        /// If the control has been assigned a non-default background color, it will be used for
        /// erasing its background. Otherwise the default background (which might be a gradient
        /// or a pattern) will be used.
        /// </remarks>
        /// <remarks>
        /// 'Erase Background' event will not be generated at all for controls with this style.
        /// </remarks>
        System,

        /// <summary>
        /// Indicates that the background is only erased in the user-defined 'Paint' event handler.
        /// </summary>
        /// <remarks>
        /// Using this style avoids flicker which would result from redrawing the background twice
        /// if the 'Paint' handler entirely overwrites it. It must not be used however if the
        /// paint handler leaves any parts of the control unpainted as their contents is then
        /// undetermined. Only use it if you repaint the whole control in your handler.
        /// </remarks>
        /// <remarks>
        /// 'Erase Background' event will not be generated at all for controls with this style.
        /// </remarks>
        Paint,

        /// <summary>
        /// Indicates that the control background is not erased, letting the parent control show through.
        /// </summary>
        /// <remarks>
        /// Currently this style is only supported in MacOs and Linux with compositing available,
        /// see Control IsTransparentBackgroundSupported().
        /// </remarks>
        Transparent,
    }
}