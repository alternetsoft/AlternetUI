using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Represents a collection of visual state settings for a control, including background, foreground, images,
    /// colors, and borders for different control states. It is up to control to decide on how to use these settings when it paints itself
    /// in different states. This class provides a structured way to manage and apply these settings consistently.
    /// </summary>
    /// <remarks>Use this class to customize the appearance of a control based on its visual state, such as
    /// normal, hovered, or disabled. The settings allow for fine-grained control over how a control is rendered in each
    /// state. Methods are provided to clone the settings and to adjust border configurations for specific states. This
    /// class is typically used in scenarios where consistent and state-aware visual customization is required across
    /// controls.</remarks>
    public partial class ControlStateSettings
    {
        /// <summary>
        /// Gets or sets background paint actions.
        /// </summary>
        public ControlStatePaintActions? BackgroundActions { get; set; }

        /// <summary>
        /// Gets or sets background brushes.
        /// </summary>
        public ControlStateBrushes? Backgrounds { get; set; }

        /// <summary>
        /// Gets or sets foreground brushes.
        /// </summary>
        public ControlStateBrushes? Foregrounds { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ControlStatePens"/>.
        /// </summary>
        public ControlStatePens? Pens { get; set; }

        /// <summary>
        /// Gets or sets foreground images.
        /// </summary>
        public ControlStateImages? Images { get; set; }

        /// <summary>
        /// Gets or sets background images associated with the control.
        /// </summary>
        public ControlStateImages? BackgroundImages { get; set; }

        /// <summary>
        /// Gets or sets foregound svg images.
        /// </summary>
        public ControlStateSvgImages? SvgImages { get; set; }

        /// <summary>
        /// Gets or sets background SVG images associated with the control.
        /// </summary>
        public ControlStateSvgImages? BackgroundSvgImages { get; set; }

        /// <summary>
        /// Gets or sets foreground image sets.
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
        /// Gets whether <see cref="SvgImages"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherSvgImages => SvgImages?.HasOtherStates ?? false;

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
        /// Gets whether <see cref="BackgroundSvgImages"/> has settings not only for
        /// <see cref="VisualControlState.Normal"/> control state.
        /// </summary>
        public bool HasOtherSvgBackgrounds => BackgroundSvgImages?.HasOtherStates ?? false;

        /// <summary>
        /// Creates clone of this object.
        /// </summary>
        /// <returns></returns>
        public virtual ControlStateSettings Clone()
        {
            ControlStateSettings result = new();

            result.BackgroundActions = BackgroundActions?.Clone();
            result.Backgrounds = Backgrounds?.Clone();
            result.Foregrounds = Foregrounds?.Clone();
            result.Pens = Pens?.Clone();
            result.Images = Images?.Clone();
            result.BackgroundImages = BackgroundImages?.Clone();
            result.ImageSets = ImageSets?.Clone();
            result.Colors = Colors?.Clone();
            result.Borders = Borders?.Clone();
            result.SvgImages = SvgImages?.Clone();
            result.BackgroundSvgImages = BackgroundSvgImages?.Clone();

            return result;
        }

        /// <summary>
        /// Sets border in the disabled state equal to border in the hovered state.
        /// </summary>
        public virtual void DisabledBorderAsHovered()
        {
            if (Borders is null)
                return;
            Borders.Disabled = Borders.Hovered;
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

        /// <summary>
        /// Sets background paint actions to draw push button background.
        /// <see cref="AbstractControl.DrawDefaultBackground"/> uses
        /// this information when it paints background of the control.
        /// </summary>
        public virtual void SetAsPushButton()
        {
            BackgroundActions ??= new();
            BackgroundActions.SetAsPushButton();
        }
    }
}
