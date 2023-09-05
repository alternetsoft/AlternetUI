using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Connects <see cref="Control"/> and visual designer.
    /// </summary>
    public interface IComponentDesigner
    {
        /// <summary>
        /// Notifies designer about property change.
        /// </summary>
        /// <param name="sender">Object instance which property was changed.</param>
        /// <param name="propName">Property name. Optional. If <c>null</c>, more than one
        /// property were changed.</param>
        void PropertyChanged(object? sender, string? propName);
    }
}
