using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Customization styles of <see cref="AuiManager"/>.
    /// </summary>
    [Flags]
    internal enum AuiManagerOption
    {
        /// <summary>
        /// Allow a pane to be undocked to take the form of a mini frame.
        /// </summary>
        AllowFloating = 1 << 0,

        /// <summary>
        /// Change the color of the title bar of the pane when it is activated.
        /// </summary>
        AllowActivePane = 1 << 1,

        /// <summary>
        /// Make the pane transparent during its movement.
        /// </summary>
        TransparentDrag = 1 << 2,

        /// <summary>
        /// The possible location for docking is indicated by a translucent area.
        /// </summary>
        TransparentHint = 1 << 3,

        /// <summary>
        /// The possible location for docking is indicated by gradually
        /// appearing partially transparent hint.
        /// </summary>
        VenetianBlindsHint = 1 << 4,

        /// <summary>
        /// The possible location for docking is indicated by a rectangular outline.
        /// </summary>
        RectangleHint = 1 << 5,

        /// <summary>
        /// The translucent area where the pane could be docked appears gradually.
        /// </summary>
        HintFade = 1 << 6,

        /// <summary>
        /// Used in complement of VenetianBlindsHint to show the docking
        /// hint immediately.
        /// </summary>
        NoVenetianBlindsFade = 1 << 7,

        /// <summary>
        /// When a docked pane is resized, its content is refreshed in live
        /// (instead of moving the border alone and refreshing the content
        /// at the end).
        /// </summary>
        LiveResize = 1 << 8,

        /// <summary>
        /// Default behaviour. It combines AllowFloating, TransparentHint, HintFade,
        /// NoVenetianBlindsFade.
        /// </summary>
        Default = AllowFloating | TransparentHint | HintFade | NoVenetianBlindsFade,
    }
}