using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains system color values for the Linux dark theme in case when
    /// operating system provides no system colors.
    /// </summary>
    public class SystemColorsDarkLinux : ISystemColorStructs
    {
        private static ISystemColorStructs? defaultValue;

        /// <summary>
        /// Gets the default instance of <see cref="SystemColorsDarkLinux"/>
        /// as <see cref="ISystemColorStructs"/>.
        /// </summary>
        public static ISystemColorStructs Default
        {
            get
            {
                return defaultValue ??= new SystemColorsDarkLinux();
            }

            set
            {
                defaultValue = value;
            }
        }

        /// <inheritdoc/>
        public ColorStruct ActiveBorder { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct ActiveCaption { get; set; } = (255, 34, 34, 34);

        /// <inheritdoc/>
        public ColorStruct ActiveCaptionText { get; set; } = (255, 247, 247, 247);

        /// <inheritdoc/>
        public ColorStruct AppWorkspace { get; set; } = (255, 39, 39, 39);

        /// <inheritdoc/>
        public ColorStruct ButtonFace { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct ButtonHighlight { get; set; } = (255, 60, 60, 60);

        /// <inheritdoc/>
        public ColorStruct ButtonShadow { get; set; } = (255, 24, 24, 24);

        /// <inheritdoc/>
        public ColorStruct Control { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct ControlDark { get; set; } = (255, 24, 24, 24);

        /// <inheritdoc/>
        public ColorStruct ControlDarkDark { get; set; } = (255, 0, 0, 0);

        /// <inheritdoc/>
        public ColorStruct ControlLight { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct ControlLightLight { get; set; } = (255, 60, 60, 60);

        /// <inheritdoc/>
        public ColorStruct Desktop { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct GradientActiveCaption { get; set; } = (255, 34, 34, 34);

        /// <inheritdoc/>
        public ColorStruct GradientInactiveCaption { get; set; } = (255, 44, 44, 44);

        /// <inheritdoc/>
        public ColorStruct GrayText { get; set; } = (255, 146, 146, 146);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Highlight { get; set; } = (255, 0, 120, 215);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct HighlightText { get; set; } = (255, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct HotTrack { get; set; } = (255, 240, 135, 98);

        /// <inheritdoc/>
        public ColorStruct InactiveBorder { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct InactiveCaption { get; set; } = (255, 44, 44, 44);

        /// <inheritdoc/>
        public ColorStruct InactiveCaptionText { get; set; } = (255, 247, 247, 247);

        /// <inheritdoc/>
        public ColorStruct Info { get; set; } = (204, 0, 0, 0);

        /// <inheritdoc/>
        public ColorStruct InfoText { get; set; } = (255, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct Menu { get; set; } = (255, 29, 29, 29);

        /// <inheritdoc/>
        public ColorStruct MenuBar { get; set; } = (255, 44, 44, 44);

        /// <inheritdoc/>
        public ColorStruct MenuHighlight { get; set; } = (38, 247, 247, 247);

        /// <inheritdoc/>
        public ColorStruct MenuText { get; set; } = (255, 247, 247, 247);

        /// <inheritdoc/>
        public ColorStruct ScrollBar { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct WindowFrame { get; set; } = (255, 55, 55, 55);

        /// <inheritdoc/>
        public ColorStruct Window { get; set; } = (255, 39, 39, 39);

        /// <inheritdoc/>
        public ColorStruct WindowText { get; set; } = (255, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct ControlText { get; set; } = (255, 247, 247, 247);
    }
}
