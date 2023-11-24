using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Alternet.Drawing;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies a set of <see cref="Image"/> for different control states.
    /// </summary>
    public class ControlStateImages : ControlStateObjects<Image>
    {
        /// <summary>
        /// Gets <see cref="ControlStateImages"/> with empty state images.
        /// </summary>
        public static readonly ControlStateImages Empty = new()
        {
            Immutable = true,
        };

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for normal control state.
        /// </summary>
        [Browsable(false)]
        [Obsolete("'NormalImage' is deprecated, please use 'Normal' instead.")]
        public virtual Image? NormalImage
        {
            get => Normal;
            set => Normal = value;
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for normal control state.
        /// </summary>
        [Browsable(false)]
        [Obsolete("'FocusedImage' is deprecated, please use 'Focused' instead.")]
        public virtual Image? FocusedImage
        {
            get => Focused;
            set => Focused = value;
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for hovered control state.
        /// </summary>
        [Browsable(false)]
        [Obsolete("'HoveredImage' is deprecated, please use 'Hovered' instead.")]
        public virtual Image? HoveredImage
        {
            get => Hovered;
            set => Hovered = value;
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for pressed control state.
        /// </summary>
        [Browsable(false)]
        [Obsolete("'PressedImage' is deprecated, please use 'Pressed' instead.")]
        public virtual Image? PressedImage
        {
            get => Pressed;
            set => Pressed = value;
        }

        /// <summary>
        /// Gets or sets an <see cref="Image"/> for disabled control state.
        /// </summary>
        [Browsable(false)]
        [Obsolete("'DisabledImage' is deprecated, please use 'Disabled' instead.")]
        public virtual Image? DisabledImage
        {
            get => Disabled;
            set => Disabled = value;
        }

        /// <summary>
        /// Gets <see cref="Image"/> for the specified state or <see cref="NormalImage"/> if
        /// image for that state is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        [Obsolete("'GetImage' is deprecated, please use 'GetObject' instead.")]
        public Image? GetImage(GenericControlState state) => GetObjectOrNormal(state);

        /// <summary>
        /// Gets <see cref="Image"/> for the specified state or <c>null</c> if image for that state
        /// is not specified.
        /// </summary>
        /// <param name="state">Control state.</param>
        [Obsolete("'GetImageOrNull' is deprecated, please use 'GetObjectOrNull' instead.")]
        public Image? GetImageOrNull(GenericControlState state) => GetObjectOrNull(state);
    }
}