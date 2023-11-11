using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Base class for text editors.
    /// </summary>
    public abstract class CustomTextEdit : Control
    {
        /// <summary>
        /// Gets whether <see cref="Text"/> is null or empty.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrEmpty => string.IsNullOrEmpty(Text);

        /// <summary>
        /// Gets whether <see cref="Text"/> is null or white space.
        /// </summary>
        [Browsable(false)]
        public bool IsNullOrWhiteSpace => string.IsNullOrWhiteSpace(Text);

        /// <summary>
        /// Gets or sets the text contents of the text box.
        /// </summary>
        /// <value>A string containing the text contents of the text box. The
        /// default is an empty string ("").</value>
        /// <remarks>
        /// Getting this property returns a string copy of the contents of the
        /// text box. Setting this property replaces the contents of the text box
        /// with the specified string.
        /// </remarks>
        [DefaultValue("")]
        [Localizability(LocalizationCategory.Text)]
        public abstract string Text { get; set; }
    }
}
