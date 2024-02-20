using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Enumerates flags which customize <see cref="ISizer"/> behavior and appearance.
    /// </summary>
    [Flags]
    internal enum SizerFlag
    {
        /// <summary>
        /// Border width will be applied to the left side of the sizer item.
        /// </summary>
        Left = 0x0010,

        /// <summary>
        /// Border width will be applied to the right side of the sizer item.
        /// </summary>
        Right = 0x0020,

        /// <summary>
        /// Border width will be applied to the top side of the sizer item.
        /// </summary>
        Top = 0x0040,

        /// <summary>
        /// Border width will be applied to the bottom side of the sizer item.
        /// </summary>
        Bottom = 0x0080,

        /// <summary>
        /// Border width will be applied to all sides of the sizer item.
        /// </summary>
        All = Top | Bottom | Right | Left,

        /// <summary>
        /// The item will be expanded to fill the space assigned to the item. When used for the
        /// items of <see cref="IBoxSizer"/>, the expansion only happens in the transversal
        /// direction of the sizer as only the item proportion governs its behaviour in
        /// the principal sizer direction. But when it is used for the items of
        /// <see cref="IGridSizer"/>, this flag can be combined with the alignment flags
        /// which override it in the corresponding direction if specified,
        /// e.g. (<see cref="Expand"/> | <see cref="AlignCenterVertical"/>) would expand the item only
        /// horizontally but center it vertically. Notice that this doesn't work for the
        /// default left/top alignment and <see cref="Expand"/> still applies in both
        /// directions if it is combined with wxALIGN_LEFT or wxALIGN_TOP.
        /// </summary>
        Expand = 0x2000,

        /// <summary>
        /// Content is horizontally aligned on the center.
        /// </summary>
        AlignCenterHorizontal = 0x0100,

        /// <summary>
        /// Content is horizontally aligned on the left.
        /// </summary>
        AlignLeft = 0,

        /// <summary>
        /// Content is vertically aligned at the top.
        /// </summary>
        AlignTop = AlignLeft,

        /// <summary>
        /// Content is horizontally aligned on the right.
        /// </summary>
        AlignRight = 0x0200,

        /// <summary>
        /// Content is vertically aligned at the bottom.
        /// </summary>
        AlignBottom = 0x0400,

        /// <summary>
        /// Content is vertically aligned at the center.
        /// </summary>
        AlignCenterVertical = 0x0800,

        /// <summary>
        /// Content is aligned at the center.
        /// </summary>
        AlignCenter = AlignCenterHorizontal | AlignCenterVertical,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the left.
        /// </summary>
        AlignBottomLeft = AlignBottom,

        /// <summary>
        /// Content is vertically aligned at the bottom, and horizontally
        /// aligned on the right.
        /// </summary>
        AlignBottomRight = AlignBottom | AlignRight,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the left.
        /// </summary>
        AlignTopLeft = AlignLeft,

        /// <summary>
        /// Content is vertically aligned at the top, and horizontally
        /// aligned on the right.
        /// </summary>
        AlignTopRight = Top | AlignRight,

        /// <summary>
        /// Content is vertically aligned at the center, and horizontally
        /// aligned on the right.
        /// </summary>
        AlignCenterRight = AlignCenterVertical | AlignRight,

        /// <summary>
        /// The item will be expanded as much as possible while also maintaining its aspect ratio.
        /// </summary>
        Shaped = 0x4000,

        /// <summary>
        /// Normally sizers use the "best", i.e. most appropriate, size of the control
        /// to determine what the minimal size of control items should be. This allows
        /// layouts to adjust correctly when the item contents, and hence its best size,
        /// changes. If this behaviour is unwanted, <see cref="FixedMinSize"/> can be used
        /// to fix minimal size of the control to its initial value and not change it
        /// any more in the future. Note that the same thing can be accomplished by
        /// setting <see cref="Control.MinimumSize"/> explicitly as well.
        /// </summary>
        FixedMinSize = 0x8000,

        /// <summary>
        /// Normally sizers don't allocate space for hidden controls or other items.
        /// This flag overrides this behaviour so that sufficient space is allocated
        /// for the control even if it isn't visible. This makes it possible to
        /// dynamically show and hide controls without resizing parent dialog,
        /// for example.
        /// </summary>
        ReserveSpaceEvenIfHidden = 0x0002,
    }
}