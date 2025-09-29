using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains system color values
    /// for the light theme in case when operating system
    /// provides no system colors.
    /// </summary>
    public class SystemColorsLight : ISystemColorStructs
    {
        private static ISystemColorStructs? defaultValue;

        /// <summary>
        /// Gets the default instance of <see cref="SystemColorsLight"/>
        /// as <see cref="ISystemColorStructs"/>.
        /// </summary>
        public static ISystemColorStructs Default
        {
            get
            {
                return defaultValue ??= new SystemColorsLight();
            }

            set
            {
                defaultValue = value;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ColorStruct"/> for the appropriate system color
        /// when the light theme is selected. This value is used when operating system
        /// provides no system colors.
        /// </summary>
        public ColorStruct ActiveBorder { get; set; } = (255, 180, 180, 180);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ActiveCaption { get; set; } = (255, 153, 180, 209);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ActiveCaptionText { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct AppWorkspace { get; set; } = (255, 171, 171, 171);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ButtonFace { get; set; } = (255, 240, 240, 240);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ButtonHighlight { get; set; } = (255, 255, 255, 255);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ButtonShadow { get; set; } = (255, 160, 160, 160);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Control { get; set; } = (255, 240, 240, 240);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ControlDark { get; set; } = (255, 160, 160, 160);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ControlDarkDark { get; set; } = (255, 105, 105, 105);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ControlLight { get; set; } = (255, 227, 227, 227);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ControlLightLight { get; set; } = (255, 255, 255, 255);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Desktop { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct GradientActiveCaption { get; set; } = (255, 185, 209, 234);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct GradientInactiveCaption { get; set; } = (255, 215, 228, 242);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct GrayText { get; set; } = (255, 109, 109, 109);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Highlight { get; set; } = (255, 0, 120, 215);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct HighlightText { get; set; } = (255, 255, 255, 255);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct HotTrack { get; set; } = (255, 0, 102, 204);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct InactiveBorder { get; set; } = (255, 244, 247, 252);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct InactiveCaption { get; set; } = (255, 191, 205, 219);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct InactiveCaptionText { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Info { get; set; } = (255, 255, 255, 225);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct InfoText { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Menu { get; set; } = (255, 240, 240, 240);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct MenuBar { get; set; } = (255, 240, 240, 240);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct MenuHighlight { get; set; } = (255, 0, 120, 215);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct MenuText { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ScrollBar { get; set; } = (255, 200, 200, 200);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct WindowFrame { get; set; } = (255, 100, 100, 100);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Window { get; set; } = (255, 255, 255, 255);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct WindowText { get; set; } = (255, 0, 0, 0);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct ControlText { get; set; } = (255, 0, 0, 0);
    }
}