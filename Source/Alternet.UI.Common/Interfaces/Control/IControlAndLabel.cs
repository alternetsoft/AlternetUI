using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains <see cref="Label"/> and <see cref="MainControl"/> properties.
    /// </summary>
    public interface IControlAndLabel
    {
        /// <summary>
        /// Gets label control.
        /// </summary>
        AbstractControl Label { get; }

        /// <summary>
        /// Gets main control.
        /// </summary>
        AbstractControl MainControl { get; }
    }
}
