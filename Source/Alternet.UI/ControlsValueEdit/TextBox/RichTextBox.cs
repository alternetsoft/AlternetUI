using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements rich text editor functionality.
    /// </summary>
    public class RichTextBox : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RichTextBox"/> class.
        /// </summary>
        public RichTextBox()
        {
        }

        /// <inheritdoc/>
        public override ControlId ControlKind => ControlId.RichTextBox;

        [Browsable(false)]
        internal new NativeRichTextBoxHandler Handler
        {
            get
            {
                CheckDisposed();
                return (NativeRichTextBoxHandler)base.Handler;
            }
        }

        internal Native.RichTextBox NativeControl => Handler.NativeControl;

        /// <inheritdoc/>
        protected override ControlHandler CreateHandler()
        {
            return new NativeRichTextBoxHandler();
        }

        /// <inheritdoc/>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
    }
}
