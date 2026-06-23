using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with radio button control.
    /// </summary>
    public interface IRadioButtonHandler
    {
        /// <summary>
        /// Gets or sets whether or not this control is checked.
        /// </summary>
        bool IsChecked { get; set; }
    }
}
