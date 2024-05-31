﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Specifies sets of objects (images, colors, borders, pens, brushes) for different control states.
    /// </summary>
    public class ControlStateSettings
    {
        /// <summary>
        /// Gets or sets <see cref="ControlStateBrushes"/>.
        /// </summary>
        public ControlStateBrushes? Backgrounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateBrushes"/>.
        /// </summary>
        public ControlStateBrushes? Foregrounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStatePens"/>.
        /// </summary>
        public ControlStatePens? Pens { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateImages"/>.
        /// </summary>
        public ControlStateImages? Images { get; set; }

        /// <summary>
        /// Gets or sets background images associated with the control.
        /// </summary>
        public ControlStateImages? BackgroundImages { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateImageSets"/>.
        /// </summary>
        public ControlStateImageSets? ImageSets { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateColors"/>.
        /// </summary>
        public ControlStateColors? Colors { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStateBorders"/>.
        /// </summary>
        public ControlStateBorders? Borders { get; set; }

        /// <summary>
        /// Gets whether <see cref="Borders"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherBorders => Borders?.HasOtherStates ?? false;

        /// <summary>
        /// Gets whether <see cref="Images"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherImages => Images?.HasOtherStates ?? false;

        /// <summary>
        /// Gets whether <see cref="Colors"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherColors => Colors?.HasOtherStates ?? false;

        /// <summary>
        /// Gets whether <see cref="Backgrounds"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherBackgrounds => Backgrounds?.HasOtherStates ?? false;

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateSettings Clone()
        {
            ControlStateSettings result = new();

            result.Backgrounds = Backgrounds?.Clone();
            result.Foregrounds = Foregrounds?.Clone();
            result.Pens = Pens?.Clone();
            result.Images = Images?.Clone();
            result.BackgroundImages = BackgroundImages?.Clone();
            result.ImageSets = ImageSets?.Clone();
            result.Colors = Colors?.Clone();
            result.Borders = Borders?.Clone();

            return result;
        }

        /// <summary>
        /// Sets border in the normal state equal to border in the hovered state.
        /// </summary>
        public virtual void NormalBorderAsHovered()
        {
            if (Borders is null)
                return;
            Borders.Normal = Borders.Hovered;
        }
    }
}
