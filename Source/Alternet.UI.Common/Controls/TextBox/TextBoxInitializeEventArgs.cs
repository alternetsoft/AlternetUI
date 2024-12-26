using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Contains event arguments for the <see cref="CustomTextBox"/> initializer methods.
    /// </summary>
    public class TextBoxInitializeEventArgs : BaseEventArgs
    {
        /// <summary>
        /// Gets or sets whether to assign and use char validator when control is initialized.
        /// If value is Null (default), <see cref="TextBoxInitializers.UseCharValidator"/>
        /// is used.
        /// </summary>
        public virtual bool? UseCharValidator { get; set; }
    }
}
