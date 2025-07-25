using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements url editor with validation.
    /// </summary>
    [ControlCategory("Editors")]
    public partial class ValueEditorUrl : ValueEditorString
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUrl"/> class.
        /// </summary>
        /// <param name="parent">Parent of the control.</param>
        public ValueEditorUrl(Control parent)
            : this()
        {
            Parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUrl"/> class.
        /// </summary>
        /// <param name="title">Label text.</param>
        /// <param name="url">Default url value.</param>
        public ValueEditorUrl(string title, string? url = default)
                    : base(title, url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueEditorUrl"/> class.
        /// </summary>
        public ValueEditorUrl()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets <see cref="UriKind"/> of the url.
        /// </summary>
        public virtual UriKind UrlKind { get; set; } = UriKind.Absolute;

        /// <summary>
        /// Gets whether editor contains a valid url.
        /// </summary>
        [Browsable(false)]
        public virtual bool IsValidUrl => ValidationUtils.IsValidUrl(Text, UrlKind);

        /// <inheritdoc/>
        protected override bool IsValidText
        {
            get
            {
                return IsValidUrl;
            }
        }

        /// <inheritdoc/>
        protected override void Init()
        {
            base.Init();
            TextBox.SetErrorText(ValueValidatorKnownError.UrlIsExpected);
        }
    }
}
