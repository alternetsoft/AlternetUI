using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Defines a template for the <see cref="RichToolTip"/> control.
    /// </summary>
    public interface IRichToolTipTemplate
    {
        /// <summary>
        /// Gets control which contains title.
        /// </summary>
        Label TitleLabel { get; }

        /// <summary>
        /// Gets control which contains message.
        /// </summary>
        Label MessageLabel { get; }

        /// <summary>
        /// Gets control which contains image.
        /// </summary>
        PictureBox PictureBox { get; }

        /// <summary>
        /// Gets the root control for the template.
        /// </summary>
        TemplateControl RootControl { get; }
    }
}
