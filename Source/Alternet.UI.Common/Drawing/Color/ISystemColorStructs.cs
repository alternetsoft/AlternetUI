using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Provides a set of properties representing system color
    /// values as <see cref="ColorStruct"/>s.
    /// </summary>
    public interface ISystemColorStructs
    {
        /// <summary>
        /// Gets or sets <see cref="ColorStruct"/> for the appropriate system color.
        /// </summary>
        ColorStruct ActiveBorder { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ActiveCaption { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ActiveCaptionText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct AppWorkspace { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ButtonFace { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ButtonHighlight { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ButtonShadow { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Control { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ControlDark { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ControlDarkDark { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ControlLight { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ControlLightLight { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Desktop { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct GradientActiveCaption { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct GradientInactiveCaption { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct GrayText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Highlight { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct HighlightText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct HotTrack { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct InactiveBorder { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct InactiveCaption { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct InactiveCaptionText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Info { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct InfoText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Menu { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct MenuBar { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct MenuHighlight { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct MenuText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ScrollBar { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct WindowFrame { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct Window { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct WindowText { get; set; }

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        ColorStruct ControlText { get; set; }
    }
}
