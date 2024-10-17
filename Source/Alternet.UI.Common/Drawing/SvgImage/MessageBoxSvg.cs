using System;
using System.Collections.Generic;
using System.Text;

using Alternet.UI;

namespace Alternet.Drawing
{
    /// <summary>
    /// Contains known svg images for the message box dialogs.
    /// </summary>
    public static class MessageBoxSvg
    {
        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Information"/>.
        /// </summary>
        public static SvgImage? Information { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Warning"/>.
        /// </summary>
        public static SvgImage? Warning { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Error"/>.
        /// </summary>
        public static SvgImage? Error { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Question"/>.
        /// </summary>
        public static SvgImage? Question { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Hand"/>.
        /// Image normally contains a white X in a circle with a red background.
        /// </summary>
        public static SvgImage? Hand { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Exclamation"/>.
        /// Image normally exclamation point in a
        /// triangle with a yellow background.
        /// </summary>
        public static SvgImage? Exclamation { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Asterisk"/>.
        /// Image normally contains a symbol consisting of
        /// a lowercase letter i in a circle.
        /// </summary>
        public static SvgImage? Asterisk { get; set; }

        /// <summary>
        /// Icon for the <see cref="MessageBoxIcon.Stop"/>.
        /// Image normally contains a symbol consisting
        /// of white X in a circle with a red background.
        /// </summary>
        public static SvgImage? Stop { get; set; }
    }
}
