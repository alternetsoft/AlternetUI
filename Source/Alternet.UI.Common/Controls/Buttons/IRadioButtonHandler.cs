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
    public interface IRadioButtonHandler : IControlHandler
    {
        /// <summary>
        /// Gets or sets an action which is called when checked state is changed.
        /// </summary>
        Action? CheckedChanged { get; set; }

        /// <summary>
        /// Gets or sets whether or not this control is checked.
        /// </summary>
        bool IsChecked { get; set; }
    }
}
