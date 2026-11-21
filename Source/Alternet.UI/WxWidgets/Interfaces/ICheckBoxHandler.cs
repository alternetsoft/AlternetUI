using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Contains methods and properties which allow to work with checkbox control.
    /// </summary>
    public interface ICheckBoxHandler
    {
        /// <inheritdoc cref="CheckBox.CheckState"/>
        CheckState CheckState { get; set; }

        /// <inheritdoc cref="CheckBox.AllowAllStatesForUser"/>
        bool AllowAllStatesForUser { get; set; }

        /// <inheritdoc cref="CheckBox.AlignRight"/>
        bool AlignRight { get; set; }

        /// <inheritdoc cref="CheckBox.ThreeState"/>
        bool ThreeState { get; set; }
    }
}
