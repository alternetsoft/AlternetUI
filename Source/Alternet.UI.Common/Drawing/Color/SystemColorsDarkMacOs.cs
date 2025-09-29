using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains system color values for the macOs dark theme in case when operating system
    /// provides no system colors.
    /// </summary>
    public class SystemColorsDarkMacOs : ISystemColorStructs
    {
        private static ISystemColorStructs? defaultValue;

        /// <summary>
        /// Gets the default instance of <see cref="SystemColorsDarkMacOs"/>
        /// as <see cref="ISystemColorStructs"/>.
        /// </summary>
        public static ISystemColorStructs Default
        {
            get
            {
                return defaultValue ??= new SystemColorsDarkMacOs();
            }

            set
            {
                defaultValue = value;
            }
        }

        /// <inheritdoc/>
        public ColorStruct ActiveBorder { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct ActiveCaption { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct ActiveCaptionText { get; set; } = (216, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct AppWorkspace { get; set; } = (255, 38, 38, 38);

        /// <inheritdoc/>
        public ColorStruct ButtonFace { get; set; } = (255, 38, 38, 38);

        /// <inheritdoc/>
        public ColorStruct ButtonHighlight { get; set; } = (25, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct ButtonShadow { get; set; } = (68, 0, 0, 0);

        /// <inheritdoc/>
        public ColorStruct Control { get; set; } = (255, 38, 38, 38);

        /// <inheritdoc/>
        public ColorStruct ControlDark { get; set; } = (68, 0, 0, 0);

        /// <inheritdoc/>
        public ColorStruct ControlDarkDark { get; set; } = (68, 0, 0, 0);

        /// <inheritdoc/>
        public ColorStruct ControlLight { get; set; } = (25, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct ControlLightLight { get; set; } = (25, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct Desktop { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct GradientActiveCaption { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct GradientInactiveCaption { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct GrayText { get; set; } = (63, 255, 255, 255);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct Highlight { get; set; } = (255, 0, 120, 215);

        /// <summary>
        /// <inheritdoc cref="ActiveBorder"/>.
        /// </summary>
        public ColorStruct HighlightText { get; set; } = (255, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct HotTrack { get; set; } = (255, 53, 134, 255);

        /// <inheritdoc/>
        public ColorStruct InactiveBorder { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct InactiveCaption { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct InactiveCaptionText { get; set; } = (216, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct Info { get; set; } = (255, 38, 38, 38);

        /// <inheritdoc/>
        public ColorStruct InfoText { get; set; } = (216, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct Menu { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct MenuBar { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct MenuHighlight { get; set; } = (255, 52, 93, 241);

        /// <inheritdoc/>
        public ColorStruct MenuText { get; set; } = (216, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct ScrollBar { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct WindowFrame { get; set; } = (255, 154, 154, 154);

        /// <inheritdoc/>
        public ColorStruct Window { get; set; } = (255, 23, 23, 23);

        /// <inheritdoc/>
        public ColorStruct WindowText { get; set; } = (216, 255, 255, 255);

        /// <inheritdoc/>
        public ColorStruct ControlText { get; set; } = (216, 255, 255, 255);
    }
}
